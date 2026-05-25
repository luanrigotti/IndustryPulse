import api from './axios'
import type { TokenResponse } from '../types'

export const login = async (email: string, senha: string): Promise<TokenResponse> => {
  const response = await api.post<TokenResponse>('/auth/login', { email, senha })
  return response.data
}