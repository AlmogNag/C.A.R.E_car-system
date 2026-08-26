import { useNavigate } from 'react-router-dom'
import { useAuth } from '../lib/AuthContext'

export function SettingsPage() {
  const { session, logout } = useAuth()
  const navigate = useNavigate()

  if (!session) return null

  function onLogout() {
    logout()
    navigate('/login')
  }

  return (
    <div className="page-body">
      <header className="app-header fade-up">
        <div className="brand-inline">
          <img src="/brand/logo.png" alt="" />
          <div>
            <strong>C.A.R.E</strong>
            <span>Settings</span>
          </div>
        </div>
      </header>

      <section className="settings-card fade-up fade-up-delay-1">
        <h2>Guardian profile</h2>
        <div className="settings-row">
          <span>Name</span>
          <strong>{session.fullName}</strong>
        </div>
        <div className="settings-row">
          <span>Phone</span>
          <strong>{session.phone}</strong>
        </div>
        <div className="settings-row">
          <span>Camera</span>
          <strong>{session.cameraId}</strong>
        </div>
      </section>

      <section className="settings-card fade-up fade-up-delay-2">
        <h2>How alerts work</h2>
        <p style={{ margin: 0, color: 'var(--muted)', lineHeight: 1.5, fontSize: '0.95rem' }}>
          After the engine turns off, the Raspberry Pi scans with PIR and camera. A detection
          updates Firebase, and this app surfaces the alert with the cabin capture.
        </p>
      </section>

      <button className="btn btn-ghost fade-up fade-up-delay-3" type="button" onClick={onLogout}>
        Sign out
      </button>
    </div>
  )
}
