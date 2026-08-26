import {
  createContext,
  useContext,
  useMemo,
  useState,
  type ReactNode,
} from 'react'
import {
  clearSession,
  loadSession,
  login as authLogin,
  register as authRegister,
} from '../lib/auth'
import type { AuthSession } from '../lib/types'

type AuthContextValue = {
  session: AuthSession | null
  login: (phone: string, password: string) => Promise<void>
  register: (input: {
    fullName: string
    phone: string
    password: string
    cameraId: string
  }) => Promise<void>
  logout: () => void
}

const AuthContext = createContext<AuthContextValue | null>(null)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [session, setSession] = useState<AuthSession | null>(() => loadSession())

  const value = useMemo<AuthContextValue>(
    () => ({
      session,
      async login(phone, password) {
        const next = await authLogin(phone.trim(), password)
        setSession(next)
      },
      async register(input) {
        const next = await authRegister({
          ...input,
          phone: input.phone.trim(),
          cameraId: input.cameraId.trim(),
          fullName: input.fullName.trim(),
        })
        setSession(next)
      },
      logout() {
        clearSession()
        setSession(null)
      },
    }),
    [session],
  )

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

export function useAuth() {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth must be used within AuthProvider')
  return ctx
}
