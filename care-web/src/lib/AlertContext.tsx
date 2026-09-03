import { createContext, useContext } from 'react'
import type { AlertPayload } from './types'

type AlertContextValue = {
  alert: AlertPayload
  connected: boolean
  cameraId: string
}

export const AlertContext = createContext<AlertContextValue | null>(null)

export function useAlertContext() {
  const ctx = useContext(AlertContext)
  if (!ctx) throw new Error('useAlertContext must be used within AppLayout')
  return ctx
}
