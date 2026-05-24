import api from './axios'
import type { KpiGeral, EficienciaLinha, ProducaoDiaria, ParetoParada } from '../types'

export const buscarKpis = async (inicio: string, fim: string): Promise<KpiGeral> => {
  const response = await api.get<KpiGeral>('/dashboard/kpis', {
    params: { inicio, fim }
  })
  return response.data
}

export const buscarEficienciaLinhas = async (
  inicio: string,
  fim: string
): Promise<EficienciaLinha[]> => {
  const response = await api.get<EficienciaLinha[]>('/dashboard/eficiencia-linhas', {
    params: { inicio, fim }
  })
  return response.data
}

export const buscarProducaoDiaria = async (dias: number): Promise<ProducaoDiaria[]> => {
  const response = await api.get<ProducaoDiaria[]>('/dashboard/producao-diaria', {
    params: { dias }
  })
  return response.data
}

export const buscarParetoParadas = async (
  inicio: string,
  fim: string
): Promise<ParetoParada[]> => {
  const response = await api.get<ParetoParada[]>('/dashboard/pareto-paradas', {
    params: { inicio, fim }
  })
  return response.data
}