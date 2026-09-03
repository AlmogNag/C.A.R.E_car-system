# C.A.R.E Web UI

Modern mobile-first UI for **C.A.R.E (Car Alert & Reminder Eye)** — the cabin safety companion that listens to the Raspberry Pi → Firebase alert pipeline.

## Screens

- **Login / Register** — same `Users/{phone}` shape as the WinForms app
- **Home** — live secured / alert status for the linked camera
- **Alerts** — cabin capture (Base64 from RTDB), source metadata, acknowledge/reset
- **Settings** — profile + sign out

## Firebase paths (unchanged)

- `Users/{phone}` → `{ fullName, password, cameraId }`
- `AlertSystem/{cameraId}` → `{ isAlert, imageUrl, timestamp, alertSource, motionDetected, humanDetected }`

Realtime updates use the RTDB REST streaming protocol against:

`https://care-c0bdb-default-rtdb.europe-west1.firebasedatabase.app`

## Run locally

```bash
cd care-web
npm install
npm run dev
```

## Design notes

Visual language follows the C.A.R.E brand deck: deep cabin night palette, electric blue accents, circular logo treatment, and an alert composition inspired by the product mockup (warning → capture → acknowledge).
