import { NavLink } from 'react-router-dom'

function IconHome() {
  return (
    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8">
      <path d="M4 10.5 12 4l8 6.5V20a1 1 0 0 1-1 1h-5v-6H10v6H5a1 1 0 0 1-1-1v-9.5Z" />
    </svg>
  )
}

function IconBell() {
  return (
    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8">
      <path d="M6.5 9.5a5.5 5.5 0 0 1 11 0c0 4.2 1.5 5.5 1.5 5.5H5s1.5-1.3 1.5-5.5Z" />
      <path d="M10 19a2 2 0 0 0 4 0" />
    </svg>
  )
}

function IconSettings() {
  return (
    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8">
      <circle cx="12" cy="12" r="3.2" />
      <path d="M19.4 13.2a1.6 1.6 0 0 0 .3 1.8l.1.1a1.8 1.8 0 1 1-2.5 2.5l-.1-.1a1.6 1.6 0 0 0-1.8-.3 1.6 1.6 0 0 0-1 1.5V19a1.8 1.8 0 1 1-3.6 0v-.1a1.6 1.6 0 0 0-1-1.5 1.6 1.6 0 0 0-1.8.3l-.1.1a1.8 1.8 0 1 1-2.5-2.5l.1-.1a1.6 1.6 0 0 0 .3-1.8 1.6 1.6 0 0 0-1.5-1H5a1.8 1.8 0 1 1 0-3.6h.1a1.6 1.6 0 0 0 1.5-1 1.6 1.6 0 0 0-.3-1.8l-.1-.1a1.8 1.8 0 1 1 2.5-2.5l.1.1a1.6 1.6 0 0 0 1.8.3h.1a1.6 1.6 0 0 0 1-1.5V5a1.8 1.8 0 1 1 3.6 0v.1a1.6 1.6 0 0 0 1 1.5 1.6 1.6 0 0 0 1.8-.3l.1-.1a1.8 1.8 0 1 1 2.5 2.5l-.1.1a1.6 1.6 0 0 0-.3 1.8v.1a1.6 1.6 0 0 0 1.5 1H19a1.8 1.8 0 1 1 0 3.6h-.1a1.6 1.6 0 0 0-1.5 1Z" />
    </svg>
  )
}

export function BottomNav({ alertActive }: { alertActive?: boolean }) {
  return (
    <nav className="bottom-nav" aria-label="Primary">
      <NavLink to="/app" end className={({ isActive }) => `nav-item${isActive ? ' active' : ''}`}>
        <IconHome />
        Home
      </NavLink>
      <NavLink
        to="/app/alerts"
        className={({ isActive }) => `nav-item${isActive || alertActive ? ' active' : ''}`}
      >
        <IconBell />
        Alerts
      </NavLink>
      <NavLink
        to="/app/settings"
        className={({ isActive }) => `nav-item${isActive ? ' active' : ''}`}
      >
        <IconSettings />
        Settings
      </NavLink>
    </nav>
  )
}
