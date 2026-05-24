import api from './axios'
import type { Produto } from '../types'

export const buscarProdutos = async (): Promise<Produto[]> => {
  const response = await api.get<Produto[]>('/produtos')
  return response.data
}