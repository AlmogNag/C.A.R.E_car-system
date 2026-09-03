import { useState, type FormEvent } from 'react'
import { Link, Navigate, useNavigate } from 'react-router-dom'
import { useAuth } from '../lib/AuthContext'
import './AuthPages.css'

export function RegisterPage() {
  const { session, register } = useAuth()
  const navigate = useNavigate()
  const [fullName, setFullName] = useState('')
  const [phone, setPhone] = useState('')
  const [password, setPassword] = useState('')
  const [cameraId, setCameraId] = useState('CARE_CAMERA_01')
  const [error, setError] = useState('')
  const [busy, setBusy] = useState(false)

  if (session) return <Navigate to="/app" replace />

  async function onSubmit(event: FormEvent) {
    event.preventDefault()
    setError('')
    setBusy(true)
    try {
      await register({ fullName, phone, password, cameraId })
      navigate('/app')
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Registration failed')
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="auth-page">
      <div className="bg-atmosphere" />
      <div className="auth-content phone-frame">
        <header className="auth-hero compact fade-up">
          <img src="/brand/logo.png" alt="C.A.R.E logo" className="brand-mark" />
          <h1 className="brand-title">Join C.A.R.E</h1>
          <p className="brand-subtitle">Link your phone to your cabin camera</p>
        </header>

        <form className="auth-card fade-up fade-up-delay-1" onSubmit={onSubmit}>
          {error ? <div className="error-banner">{error}</div> : null}

          <div className="field">
            <label htmlFor="name">Full name</label>
            <input
              id="name"
              value={fullName}
              onChange={(e) => setFullName(e.target.value)}
              required
            />
          </div>

          <div className="field">
            <label htmlFor="phone">Phone number</label>
            <input
              id="phone"
              inputMode="tel"
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
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          <div className="field">
            <label htmlFor="camera">Camera code</label>
            <input
              id="camera"
              value={cameraId}
              onChange={(e) => setCameraId(e.target.value)}
              required
            />
          </div>

          <button className="btn btn-primary" type="submit" disabled={busy}>
            {busy ? 'Creating…' : 'Create account'}
          </button>

          <p className="switch-line">
            Already have an account? <Link to="/login">Sign in</Link>
          </p>
        </form>
      </div>
    </div>
  )
}
