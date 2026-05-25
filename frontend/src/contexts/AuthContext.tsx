import { createContext, useContext, useState } from 'react'
import type { ReactNode } from 'react'
import { login as loginApi } from '../api/auth'

interface Usuario {
  nome: string
  perfil: string
}

interface AuthContextType {
  usuario: Usuario | null
  isAuthenticated: boolean
  login: (email: string, senha: string) => Promise<void>
  logout: () => void
}

const AuthContext = createContext<AuthContextType>({} as AuthContextType)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [usuario, setUsuario] = useState<Usuario | null>(() => {
    const stored = localStorage.getItem('usuario')
    return stored ? JSON.parse(stored) : null
  })

  const isAuthenticated = !!usuario

  const login = async (email: string, senha: string) => {
    const response = await loginApi(email, senha)
    localStorage.setItem('token', response.token)
    const user = { nome: response.nomeUsuario, perfil: response.perfil }
    localStorage.setItem('usuario', JSON.stringify(user))
    setUsuario(user)
  }

  const logout = () => {
    localStorage.removeItem('token')
    localStorage.removeItem('usuario')
    setUsuario(null)
  }

  return (
    <AuthContext.Provider value={{ usuario, isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  )
}

export const useAuth = () => useContext(AuthContext)