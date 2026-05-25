import api from './axios'
import type { Linha } from '../types'


export const buscarLinhas = async (): Promise<Linha[]> => {
  const response = await api.get<Linha[]>('/linhas')
  return response.data
}