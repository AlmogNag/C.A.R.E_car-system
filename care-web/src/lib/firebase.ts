const DATABASE_URL =
  'https://care-c0bdb-default-rtdb.europe-west1.firebasedatabase.app'

function url(path: string) {
  const clean = path.replace(/^\/+|\/+$/g, '')
  return `${DATABASE_URL}/${clean}.json`
}

export async function firebaseGet<T>(path: string): Promise<T | null> {
  const res = await fetch(url(path))
  if (!res.ok) {
    throw new Error(`Firebase GET failed (${res.status})`)
  }
  return (await res.json()) as T | null
}

export async function firebasePut<T>(path: string, data: T): Promise<void> {
  const res = await fetch(url(path), {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  })
  if (!res.ok) {
    throw new Error(`Firebase PUT failed (${res.status})`)
  }
}

export async function firebasePatch(
  path: string,
  data: Record<string, unknown>,
): Promise<void> {
  const res = await fetch(url(path), {
    method: 'PATCH',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  })
  if (!res.ok) {
    throw new Error(`Firebase PATCH failed (${res.status})`)
  }
}

type StreamHandler = (value: unknown, path: string | null) => void

/**
 * Subscribe to Realtime Database changes via the REST streaming protocol
 * (same open RTDB access pattern as the WinForms client).
 */
export function firebaseSubscribe(
  path: string,
  onValue: StreamHandler,
): () => void {
  const source = new EventSource(url(path))

  const handle = (event: MessageEvent) => {
    try {
      const payload = JSON.parse(event.data) as {
        path: string | null
        data: unknown
      }
      onValue(payload.data, payload.path)
    } catch (error) {
      console.error('Failed to parse Firebase stream event', error)
    }
  }

  source.addEventListener('put', handle as EventListener)
  source.addEventListener('patch', handle as EventListener)

  source.onerror = () => {
    // EventSource reconnects automatically; keep quiet unless closed.
  }

  return () => {
    source.close()
  }
}
