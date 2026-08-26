import { Link } from 'react-router-dom'
import { StatusHero } from '../components/StatusHero'
import { useAlertContext } from '../lib/AlertContext'
import { useAuth } from '../lib/AuthContext'

export function HomePage() {
  const { session } = useAuth()
  const { alert, connected } = useAlertContext()
  const secured = !alert.isAlert

  if (!session) return null

  return (
    <div className="page-body">
      <header className="app-header fade-up">
        <div className="brand-inline">
          <img src="/brand/logo.png" alt="" />
          <div>
            <strong>C.A.R.E</strong>
            <span>Car Alert &amp; Reminder Eye</span>
          </div>
        </div>
      </header>

      <StatusHero
        secured={secured}
        connected={connected}
        cameraId={session.cameraId}
        name={session.fullName}
      />

      {!secured ? (
        <section className="alert-cta fade-up fade-up-delay-1">
          <h3>Alert ready</h3>
          <p>A capture from your cabin camera is waiting for review.</p>
          <Link className="btn btn-danger" to="/app/alerts">
            Open alert
          </Link>
        </section>
      ) : (
        <section className="cabin-visual fade-up fade-up-delay-1">
          <img src="/brand/child-seat.jpeg" alt="Child secured in a car seat" />
          <div className="caption">Automatic scan after engine off · Dual detection · Real-time alert</div>
        </section>
      )}

      <section className="feature-strip fade-up fade-up-delay-2">
        <div className="feature">
          <strong>Automatic</strong>
          <span>After shutdown</span>
        </div>
        <div className="feature">
          <strong>Dual sense</strong>
          <span>PIR + camera</span>
        </div>
        <div className="feature">
          <strong>Live push</strong>
          <span>via Firebase</span>
        </div>
      </section>
    </div>
  )
}
