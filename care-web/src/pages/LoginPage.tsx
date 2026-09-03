import { useState, type FormEvent } from 'react'
import { Link, Navigate, useNavigate } from 'react-router-dom'
import { useAuth } from '../lib/AuthContext'
import './AuthPages.css'

export function LoginPage() {
  const { session, login } = useAuth()
  const navigate = useNavigate()
  const [phone, setPhone] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [busy, setBusy] = useState(false)

  if (session) return <Navigate to="/app" replace />

  async function onSubmit(event: FormEvent) {
    event.preventDefault()
    setError('')
    setBusy(true)
    try {
      await login(phone, password)
      navigate('/app')
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Login failed')
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="auth-page">
      <div className="bg-atmosphere" />
      <div className="auth-content phone-frame">
        <header className="auth-hero fade-up">
          <img src="/brand/logo.png" alt="C.A.R.E logo" className="brand-mark" />
          <h1 className="brand-title">C.A.R.E</h1>
          <p className="brand-subtitle">Car Alert &amp; Reminder Eye</p>
          <p className="tagline">An extra eye watching the back seat for you.</p>
        </header>

        <form className="auth-card fade-up fade-up-delay-1" onSubmit={onSubmit}>
          <h2>Sign in</h2>
          <p className="auth-copy">Monitor your vehicle after shutdown and get presence alerts in real time.</p>

          {error ? <div className="error-banner">{error}</div> : null}

          <div className="field">
            <label htmlFor="phone">Phone number</label>
            <input
              id="phone"
              inputMode="tel"
              autoComplete="tel"
              placeholder="05XXXXXXXX"
              value={phone}
              onChange={(e) => setPhone(e.target.value)}
              required
            />
          </div>

          <div className="field">
            <label htmlFor="password">Password</label>
            <input
              id="password"
              type="password"
              autoComplete="current-password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          <button className="btn btn-primary" type="submit" disabled={busy}>
            {busy ? 'Signing in…' : 'Sign in'}
          </button>

          <p className="switch-line">
            New guardian? <Link to="/register">Create account</Link>
          </p>
        </form>
      </div>
    </div>
  )
}
