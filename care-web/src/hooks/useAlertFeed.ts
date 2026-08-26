import { useEffect, useState } from 'react'
import { firebaseSubscribe } from '../lib/firebase'
import type { AlertPayload } from '../lib/types'

function mergeAlert(
  current: AlertPayload,
  data: unknown,
  path: string | null,
): AlertPayload {
  if (data == null) return current

  if (path == null || path === '/') {
    return { ...current, ...(data as AlertPayload) }
  }

  const key = path.replace(/^\//, '')
  return { ...current, [key]: data }
}

export function useAlertFeed(cameraId: string | undefined) {
  const [alert, setAlert] = useState<AlertPayload>({})
  const [connected, setConnected] = useState(false)

  useEffect(() => {
    if (!cameraId) return

    setAlert({})
    setConnected(false)

    const unsubscribe = firebaseSubscribe(
      `AlertSystem/${cameraId}`,
      (data, path) => {
        setConnected(true)
        setAlert((prev) => mergeAlert(prev, data, path))
      },
    )

    return unsubscribe
  }, [cameraId])

  return { alert, connected }
}
