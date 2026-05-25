import api from './axios'
import type { Ordem } from '../types'

export const buscarOrdens = async (): Promise<Ordem[]> => {
  const response = await api.get<Ordem[]>('/ordens')
  return response.data
}

export const criarOrdem = async (data: {
  produtoId: number
  linhaProducaoId: number
  quantidadePlanejada: number
  dataPrevisao: string
  observacao?: string
}): Promise<Ordem> => {
  const response = await api.post<Ordem>('/ordens', data)
  return response.data
}

export const atualizarStatus = async (
  id: number,
  novoStatus: string
): Promise<void> => {
  await api.put(`/ordens/${id}/status`, { novoStatus })
}