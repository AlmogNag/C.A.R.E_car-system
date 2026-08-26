import './StatusHero.css'

type Props = {
  secured: boolean
  connected: boolean
  cameraId: string
  name: string
}

export function StatusHero({ secured, connected, cameraId, name }: Props) {
  return (
    <section className={`status-hero fade-up ${secured ? 'secured' : 'danger'}`}>
      <div className="status-ring" aria-hidden>
        <div className="status-core">
          {secured ? (
            <svg viewBox="0 0 64 64" className="status-icon">
              <path
                d="M32 8 12 16v14c0 14.5 8.8 24.8 20 28 11.2-3.2 20-13.5 20-28V16L32 8Z"
                fill="none"
                stroke="currentColor"
                strokeWidth="3"
              />
              <path
                d="m22 32 7 7 13-14"
                fill="none"
                stroke="currentColor"
                strokeWidth="3.5"
                strokeLinecap="round"
                strokeLinejoin="round"
              />
            </svg>
          ) : (
            <svg viewBox="0 0 64 64" className="status-icon alert">
              <path
                d="M32 10 8 54h48L32 10Z"
                fill="none"
                stroke="currentColor"
                strokeWidth="3.5"
                strokeLinejoin="round"
              />
              <path d="M32 26v14" stroke="currentColor" strokeWidth="3.5" strokeLinecap="round" />
              <circle cx="32" cy="46" r="2.4" fill="currentColor" />
            </svg>
          )}
        </div>
      </div>

      <p className="eyebrow">{secured ? 'System secured' : 'Presence detected'}</p>
      <h1>{secured ? 'All clear in the cabin' : 'Immediate check needed'}</h1>
      <p className="lede">
        {secured
          ? `Hi ${name.split(' ')[0]}, C.A.R.E is watching the back seat for you.`
          : 'Motion or presence was detected after shutdown. Open the alert for the live capture.'}
      </p>

      <div className="meta-row">
        <span className={`status-pill ${connected ? '' : 'offline'} ${secured ? '' : 'alert'}`}>
          <span className="dot" />
          {connected ? (secured ? 'Live · Clear' : 'Live · Alert') : 'Connecting…'}
        </span>
        <span className="camera-chip">{cameraId}</span>
      </div>
    </section>
  )
}
