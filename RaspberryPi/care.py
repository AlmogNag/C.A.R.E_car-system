import cv2
import os
import firebase_admin
import time
import subprocess
import sys
import base64
import RPi.GPIO as GPIO

from firebase_admin import credentials, db
from datetime import datetime
from pathlib import Path
from picamera2 import Picamera2


# --------------------------------------------------
# 1. Firebase Initialization
# --------------------------------------------------

cred = credentials.Certificate(
    "care-c0bdb-firebase-adminsdk-fbsvc-64406916fa.json"
)

if not firebase_admin._apps:
    firebase_admin.initialize_app(
        cred,
        {
            'databaseURL':
            'https://care-c0bdb-default-rtdb.europe-west1.firebasedatabase.app/'
        }
    )


# --------------------------------------------------
# 2. System Configuration
# --------------------------------------------------

CAMERA_ID = "CARE_CAMERA_01"

# PIR motion sensor is connected to GPIO 21
PIR_PIN = 21

GPIO.setmode(GPIO.BCM)
GPIO.setup(PIR_PIN, GPIO.IN)


# --------------------------------------------------
# 3. Directory and Driving Flag Setup
# --------------------------------------------------

out_dir = Path.home() / "Documents" / "CARE"
out_dir.mkdir(parents=True, exist_ok=True)

FLAG_FILE = out_dir / "is_driving.txt"


# --------------------------------------------------
# 4. Startup Logic
# --------------------------------------------------

if FLAG_FILE.exists():

    print(
        "REBOOT DETECTED: Engine was turned off. "
        "Starting safety scan..."
    )

    # Remove the flag so the system will not trigger again
    FLAG_FILE.unlink()

    # Give the parent time to exit the vehicle
    print("Waiting 10 seconds for parent to exit...")
    time.sleep(10)

else:

    # First boot represents the beginning of a trip
    print("FRESH BOOT: Trip started. Creating driving flag.")

    with open(FLAG_FILE, "w") as f:
        f.write("active")

    print(
        "System in Standby Mode during drive. "
        "Monitoring disabled until next reboot."
    )

    GPIO.cleanup()
    sys.exit(0)


# --------------------------------------------------
# 5. Human Detection Models
# --------------------------------------------------

face_cascade = cv2.CascadeClassifier(
    "/usr/share/opencv4/haarcascades/"
    "haarcascade_frontalface_default.xml"
)

hog = cv2.HOGDescriptor()
hog.setSVMDetector(
    cv2.HOGDescriptor_getDefaultPeopleDetector()
)


# --------------------------------------------------
# 6. Camera Startup
# --------------------------------------------------

picam2 = Picamera2()

picam2.configure(
    picam2.create_preview_configuration(
        main={
            "format": "RGB888",
            "size": (640, 480)
        }
    )
)

picam2.start()

# Give the camera a short time to stabilize
time.sleep(2)


# --------------------------------------------------
# 7. Safety Scan Configuration
# --------------------------------------------------

start_time = time.time()

# Scan for five minutes
timeout = 300

alert_sent = False


# --------------------------------------------------
# 8. Main Scanning Loop
# --------------------------------------------------

while not alert_sent:

    elapsed = time.time() - start_time
    remaining = max(0, int(timeout - elapsed))

    # Stop scanning after five minutes
    if elapsed > timeout:

        print(
            "Timeout: No motion or human detected. "
            "Shutting down system."
        )

        picam2.stop()
        cv2.destroyAllWindows()
        GPIO.cleanup()

        os.system("sudo shutdown -h now")
        break


    # --------------------------------------------------
    # Read PIR Motion Sensor
    # --------------------------------------------------

    motion_detected = (
        GPIO.input(PIR_PIN) == GPIO.HIGH
    )


    # --------------------------------------------------
    # Capture Camera Frame
    # --------------------------------------------------

    frame = picam2.capture_array()

    gray = cv2.cvtColor(
        frame,
        cv2.COLOR_RGB2GRAY
    )


    # --------------------------------------------------
    # Human Detection
    # --------------------------------------------------

    faces = face_cascade.detectMultiScale(
        gray,
        scaleFactor=1.1,
        minNeighbors=5
    )

    bodies, _ = hog.detectMultiScale(
        frame,
        winStride=(8, 8),
        scale=1.05
    )

    human_detected = (
        len(faces) > 0 or
        len(bodies) > 0
    )


    # --------------------------------------------------
    # System Status
    # --------------------------------------------------

    if motion_detected:
        print("PIR: Motion detected")

    if human_detected:
        print("CAMERA: Human detected")


    # --------------------------------------------------
    # UI Overlay
    # --------------------------------------------------

    try:

        temp = subprocess.check_output(
            ['vcgencmd', 'measure_temp']
        ).decode(
            'utf-8'
        ).replace(
            "temp=",
            ""
        ).strip()

    except Exception:

        temp = "N/A"


    cv2.putText(
        frame,
        f"CPU: {temp}",
        (10, 30),
        cv2.FONT_HERSHEY_SIMPLEX,
        0.7,
        (255, 255, 255),
        2
    )

    cv2.putText(
        frame,
        f"OFF IN: {remaining}s",
        (10, 60),
        cv2.FONT_HERSHEY_SIMPLEX,
        0.7,
        (0, 255, 255),
        2
    )

    pir_status = (
        "MOTION"
        if motion_detected
        else "CLEAR"
    )

    cv2.putText(
        frame,
        f"PIR: {pir_status}",
        (10, 90),
        cv2.FONT_HERSHEY_SIMPLEX,
        0.7,
        (0, 255, 0),
        2
    )


    cv2.imshow(
        "CARE Preview",
        frame
    )


    # --------------------------------------------------
    # Alert Trigger
    #
    # An alert is triggered if:
    # 1. PIR detects motion
    # OR
    # 2. Camera detects a human
    # --------------------------------------------------

    if motion_detected or human_detected:

        alert_sent = True

        print("!!! CARE ALERT TRIGGERED !!!")


        # --------------------------------------------------
        # Determine Alert Source
        # --------------------------------------------------

        if motion_detected and human_detected:

            alert_source = "BOTH"

            print(
                "Alert source: PIR + CAMERA"
            )

        elif motion_detected:

            alert_source = "PIR"

            print(
                "Alert source: PIR motion sensor"
            )

        else:

            alert_source = "CAMERA"

            print(
                "Alert source: Camera human detection"
            )


        # --------------------------------------------------
        # Mark Camera Detections
        # --------------------------------------------------

        for (x, y, w, h) in faces:

            cv2.rectangle(
                frame,
                (x, y),
                (x + w, y + h),
                (0, 255, 0),
                2
            )


        for (x, y, w, h) in bodies:

            cv2.rectangle(
                frame,
                (x, y),
                (x + w, y + h),
                (255, 0, 0),
                2
            )


        # --------------------------------------------------
        # Save Alert Image
        # --------------------------------------------------

        now = datetime.now()

        time_str = now.strftime(
            '%Y-%m-%d %H:%M:%S'
        )

        file_name = (
            f"alert_{now.strftime('%H%M%S')}.jpg"
        )

        local_path = str(
            out_dir / file_name
        )

        cv2.imwrite(
            local_path,
            frame
        )


        # --------------------------------------------------
        # Firebase Upload
        # --------------------------------------------------

        try:

            print(
                "-> Converting image to Base64 string..."
            )

            with open(
                local_path,
                "rb"
            ) as image_file:

                base64_string = (
                    base64.b64encode(
                        image_file.read()
                    ).decode(
                        'utf-8'
                    )
                )


            # Update current alert state
            camera_ref = db.reference(
                f'AlertSystem/{CAMERA_ID}'
            )

            print(
                "-> Syncing real-time status "
                "with Firebase..."
            )

            camera_ref.set(
                {
                    'isAlert': True,

                    'timestamp':
                    time_str,

                    'alertSource':
                    alert_source,

                    'motionDetected':
                    motion_detected,

                    'humanDetected':
                    human_detected,

                    'imageUrl':
                    base64_string
                }
            )


            # Store alert in history
            history_ref = db.reference(
                f'AlertsHistory/{CAMERA_ID}'
            )

            history_ref.push(
                {
                    'timestamp':
                    time_str,

                    'alertSource':
                    alert_source,

                    'motionDetected':
                    motion_detected,

                    'humanDetected':
                    human_detected,

                    'imageUrl':
                    base64_string
                }
            )


            print(
                "Alert successfully synced "
                "with Firebase."
            )

            break


        except Exception as e:

            print(
                f"Firebase Error: {e}"
            )

            break


    # --------------------------------------------------
    # Manual Exit
    # --------------------------------------------------

    if cv2.waitKey(1) & 0xFF == ord("q"):
        break


# --------------------------------------------------
# 9. Cleanup
# --------------------------------------------------

picam2.stop()

cv2.destroyAllWindows()

GPIO.cleanup()
