import { Navigate, Outlet } from 'react-router-dom'
import { BottomNav } from '../components/BottomNav'
import { useAlertFeed } from '../hooks/useAlertFeed'
import { useAuth } from '../lib/AuthContext'
import { AlertContext } from '../lib/AlertContext'
import './AppShell.css'

export function AppLayout() {
  const { session } = useAuth()
  const { alert, connected } = useAlertFeed(session?.cameraId)

  if (!session) return <Navigate to="/login" replace />

  const alertActive = Boolean(alert.isAlert)

  return (
    <AlertContext.Provider value={{ alert, connected, cameraId: session.cameraId }}>
      <div className="app-root">
        <div className="bg-atmosphere soft" />
        <div className="app-shell">
          <div className="phone-frame app-frame">
            <Outlet />
            <BottomNav alertActive={alertActive} />
          </div>
        </div>
      </div>
    </AlertContext.Provider>
  )
}
