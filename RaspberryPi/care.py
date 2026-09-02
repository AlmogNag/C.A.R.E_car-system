import cv2
import os
import firebase_admin
import time
import subprocess
import sys
import base64
import RPi.GPIO as GPIO

from firebase_admin import credentials, db, messaging
from datetime import datetime
from pathlib import Path
from picamera2 import Picamera2


# --------------------------------------------------
# 1. Firebase Initialization & Credentials
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
# 2. System Hardware Configuration
# --------------------------------------------------

# Unique hardware identifier for the specific device installed in the vehicle
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
# 4. Startup & Ignition Detection Logic
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
# 5. FCM Push Notification Helper Function
# --------------------------------------------------

def send_emergency_push(camera_id, capture_time, alert_source):
    """
    Sends a high-priority FCM emergency push notification directly
    to all mobile devices registered to this camera's topic.
    """
    try:
        message = messaging.Message(
            notification=messaging.Notification(
                title="🚨 C.A.R.E CRITICAL ALERT",
                body=f"Occupancy detected in vehicle! [{capture_time} via {alert_source}]. Check cabin immediately."
            ),
            android=messaging.AndroidConfig(
                priority="high",
                notification=messaging.AndroidNotification(
                    channel_id="care_emergency_channel",
                    priority="max",
                    sound="default",
                    default_sound=True,
                    default_vibrate_timings=True
                )
            ),
            data={
                "cameraId": camera_id,
                "timestamp": capture_time,
                "alertSource": alert_source
            },
            # Send the notification to the unique topic of this camera
            topic=camera_id 
        )

        response = messaging.send(message)
        print(f"[FCM] Push notification dispatched successfully to topic '{camera_id}': {response}")
    except Exception as push_err:
        print(f"[FCM] Error dispatching push notification: {push_err}")


# --------------------------------------------------
# 6. Human Detection Models
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
# 7. Camera Startup
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
# 8. Safety Scan Configuration
# --------------------------------------------------

start_time = time.time()

# Scan for five minutes
timeout = 300

alert_sent = False


# --------------------------------------------------
# 9. Main Scanning Loop
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

        # Stop the safety scan without shutting down the Raspberry Pi.
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
    # Human Detection (Faces & Full Bodies)
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
    # System Status & Feedback
    # --------------------------------------------------

    if motion_detected:
        print("PIR: Motion detected")

    if human_detected:
        print("CAMERA: Human detected")


    # --------------------------------------------------
    # UI Overlay (Local Debug Screen)
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

    pir_status = "MOTION" if motion_detected else "CLEAR"

    cv2.putText(
        frame,
        f"PIR: {pir_status}",
        (10, 90),
        cv2.FONT_HERSHEY_SIMPLEX,
        0.7,
        (0, 255, 0),
        2
    )

    cv2.imshow("CARE Preview", frame)


    # --------------------------------------------------
    # Alert Trigger
    # --------------------------------------------------

    if motion_detected or human_detected:

        alert_sent = True
        print("!!! CARE ALERT TRIGGERED !!!")

        # Determine Alert Source
        if motion_detected and human_detected:
            alert_source = "BOTH"
            print("Alert source: PIR + CAMERA")
        elif motion_detected:
            alert_source = "PIR"
            print("Alert source: PIR motion sensor")
        else:
            alert_source = "CAMERA"
            print("Alert source: Camera human detection")

        # Mark Camera Detections
        for (x, y, w, h) in faces:
            cv2.rectangle(frame, (x, y), (x + w, y + h), (0, 255, 0), 2)

        for (x, y, w, h) in bodies:
            cv2.rectangle(frame, (x, y), (x + w, y + h), (255, 0, 0), 2)

        # Save Alert Image Locally
        now = datetime.now()
        time_str = now.strftime('%Y-%m-%d %H:%M:%S')
        file_name = f"alert_{now.strftime('%H%M%S')}.jpg"
        local_path = str(out_dir / file_name)

        cv2.imwrite(local_path, frame)

        # --------------------------------------------------
        # Firebase Realtime Database Sync & Push Notification
        # --------------------------------------------------

        try:
            print("-> Converting image to Base64 string...")

            with open(local_path, "rb") as image_file:
                base64_string = base64.b64encode(image_file.read()).decode('utf-8')

            # 1. Update current live alert state
            camera_ref = db.reference(f'AlertSystem/{CAMERA_ID}')
            print("-> Syncing real-time status with Firebase...")

            camera_ref.set({
                'isAlert': True,
                'timestamp': time_str,
                'alertSource': alert_source,
                'motionDetected': motion_detected,
                'humanDetected': human_detected,
                'imageUrl': base64_string
            })

            # 2. Store alert in history log
            history_ref = db.reference(f'AlertsHistory/{CAMERA_ID}')
            history_ref.push({
                'timestamp': time_str,
                'alertSource': alert_source,
                'motionDetected': motion_detected,
                'humanDetected': human_detected,
                'imageUrl': base64_string
            })

            print("Alert successfully synced with Realtime Database.")

            # 3. Dispatch Push Notification to all subscribed devices
            print("-> Sending FCM push notification...")
            send_emergency_push(
                camera_id=CAMERA_ID,
                capture_time=time_str,
                alert_source=alert_source
            )

            # --------------------------------------------------
            # 4. Live View - available for 60 seconds
            # --------------------------------------------------

            print("Live View available for 60 seconds.")

            live_start_time = time.time()
            live_duration = 60

            live_ref = db.reference(
                f'LiveView/{CAMERA_ID}'
            )

            # Initial Live View state.
            # The app will change requested to True when the user
            # presses the Live Camera button.
            live_ref.set({
                'requested': False,
                'active': False,
                'frame': ''
            })

            while time.time() - live_start_time < live_duration:

                try:
                    live_data = live_ref.get() or {}

                    live_requested = live_data.get(
                        'requested',
                        False
                    )

                    if live_requested:

                        live_frame = picam2.capture_array()

                        success, buffer = cv2.imencode(
                            '.jpg',
                            live_frame
                        )

                        if success:

                            live_base64 = base64.b64encode(
                                buffer
                            ).decode('utf-8')

                            live_ref.update({
                                'active': True,
                                'frame': live_base64
                            })

                            print("Live frame sent.")

                    else:
                        live_ref.update({
                            'active': False
                        })

                    # About 2 updated frames per second
                    time.sleep(0.5)

                except Exception as live_err:
                    print(
                        f"Live View Error: {live_err}"
                    )
                    time.sleep(1)

            # Live View time is over
            live_ref.update({
                'requested': False,
                'active': False
            })

            print("Live View finished.")

            # Live View window is finished.
            # The Raspberry Pi remains powered on.
            break

        except Exception as e:
            print(f"Firebase Error: {e}")
            break


    # --------------------------------------------------
    # Manual Exit
    # --------------------------------------------------

    if cv2.waitKey(1) & 0xFF == ord("q"):
        break


# --------------------------------------------------
# 10. Cleanup & Resource Release
# --------------------------------------------------

picam2.stop()
cv2.destroyAllWindows()
GPIO.cleanup()
