import type { AuthSession, Participant } from './types'
import { firebaseGet, firebasePut } from './firebase'

const SESSION_KEY = 'care.session'

export function loadSession(): AuthSession | null {
  try {
    const raw = localStorage.getItem(SESSION_KEY)
    if (!raw) return null
    return JSON.parse(raw) as AuthSession
  } catch {
    return null
  }
}

export function saveSession(session: AuthSession) {
  localStorage.setItem(SESSION_KEY, JSON.stringify(session))
}

export function clearSession() {
  localStorage.removeItem(SESSION_KEY)
}

export async function login(
  phone: string,
  password: string,
): Promise<AuthSession> {
  const user = await firebaseGet<Omit<Participant, 'phone'>>(
    `Users/${phone}`,
  )

  if (!user || user.password !== password) {
    throw new Error('Invalid phone number or password.')
  }

  const session: AuthSession = {
    fullName: user.fullName,
    phone,
    cameraId: user.cameraId,
  }
  saveSession(session)
  return session
}

export async function register(input: {
  fullName: string
  phone: string
  password: string
  cameraId: string
}): Promise<AuthSession> {
  const existing = await firebaseGet(`Users/${input.phone}`)
  if (existing) {
    throw new Error('This phone number is already registered.')
  }

  await firebasePut(`Users/${input.phone}`, {
    fullName: input.fullName,
    password: input.password,
    cameraId: input.cameraId,
  })

  const session: AuthSession = {
    fullName: input.fullName,
    phone: input.phone,
    cameraId: input.cameraId,
  }
  saveSession(session)
  return session
}
