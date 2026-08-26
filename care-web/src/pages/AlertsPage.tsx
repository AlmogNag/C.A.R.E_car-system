import { useState } from 'react'
import { firebasePut } from '../lib/firebase'
import { useAlertContext } from '../lib/AlertContext'

function toImageSrc(imageUrl?: string) {
  if (!imageUrl) return null
  if (imageUrl.startsWith('data:')) return imageUrl
  return `data:image/jpeg;base64,${imageUrl}`
}

export function AlertsPage() {
  const { alert, cameraId, connected } = useAlertContext()
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState('')
  const active = Boolean(alert.isAlert)
  const imageSrc = toImageSrc(alert.imageUrl)

  async function acknowledge() {
    setBusy(true)
    setError('')
    try {
      await firebasePut(`AlertSystem/${cameraId}/isAlert`, false)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Could not reset alert')
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="page-body alert-screen">
      <header className="app-header fade-up">
        <div className="brand-inline">
          <img src="/brand/logo.png" alt="" />
          <div>
            <strong>C.A.R.E</strong>
            <span>Alert center</span>
          </div>
        </div>
        <span className={`status-pill ${connected ? '' : 'offline'} ${active ? 'alert' : ''}`}>
          <span className="dot" />
          {active ? 'Active' : 'Idle'}
        </span>
      </header>

      {active ? (
        <>
          <svg className="warning fade-up" viewBox="0 0 64 64" aria-hidden>
            <path
              d="M32 10 8 54h48L32 10Z"
              fill="none"
              stroke="currentColor"
              strokeWidth="4"
              strokeLinejoin="round"
            />
            <path d="M32 26v14" stroke="currentColor" strokeWidth="4" strokeLinecap="round" />
            <circle cx="32" cy="46" r="2.8" fill="currentColor" />
          </svg>
          <h1 className="fade-up fade-up-delay-1">ALERT</h1>
          <p className="sub fade-up fade-up-delay-1">
            {alert.humanDetected ? 'Presence detected' : 'Motion detected'}
          </p>
        </>
      ) : (
        <>
          <h1 className="fade-up" style={{ fontSize: '1.55rem', letterSpacing: '0.04em' }}>
            No active alert
          </h1>
          <p className="fade-up" style={{ color: 'var(--muted)', marginTop: '-0.4rem' }}>
            When the Raspberry Pi detects motion or a person after shutdown, the capture appears here.
          </p>
        </>
      )}

      <div className="capture-frame fade-up fade-up-delay-2">
        {imageSrc ? (
          <img src={imageSrc} alt="Cabin camera capture from C.A.R.E" />
        ) : (
          <div className="capture-empty">
            Waiting for the next cabin capture…
          </div>
        )}
      </div>

      <div className="meta-grid fade-up fade-up-delay-3">
        <div className="meta-item">
          <span>Source</span>
          <strong>{alert.alertSource || '—'}</strong>
        </div>
        <div className="meta-item">
          <span>Timestamp</span>
          <strong>{alert.timestamp || '—'}</strong>
        </div>
        <div className="meta-item">
          <span>Motion</span>
          <strong>{alert.motionDetected ? 'Yes' : 'No'}</strong>
        </div>
        <div className="meta-item">
          <span>Human</span>
          <strong>{alert.humanDetected ? 'Yes' : 'No'}</strong>
        </div>
      </div>

      {error ? <div className="error-banner">{error}</div> : null}

      {active ? (
        <button className="btn btn-primary" type="button" onClick={acknowledge} disabled={busy}>
          {busy ? 'Acknowledging…' : 'I checked — reset alert'}
        </button>
      ) : null}
    </div>
  )
}
