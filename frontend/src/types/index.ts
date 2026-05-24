export const StatusOrdem = {
  Aberta: 'Aberta',
  EmAndamento: 'EmAndamento',
  Finalizada: 'Finalizada',
  Cancelada: 'Cancelada'
} as const

export type StatusOrdem = typeof StatusOrdem[keyof typeof StatusOrdem]

export const MotivoParada = {
  Setup: 'Setup',
  Manutencao: 'Manutencao',
  FaltaMaterial: 'FaltaMaterial',
  Qualidade: 'Qualidade',
  Outros: 'Outros'
} as const

export type MotivoParada = typeof MotivoParada[keyof typeof MotivoParada]

export interface Ordem {
  id: number
  numero: string
  status: StatusOrdem
  nomeProduto: string
  nomeLinhaProducao: string
  quantidadePlanejada: number
  quantidadeProduzida: number
  percentualConclusao: number
  estaAtrasada: boolean
  dataAbertura: string
  dataPrevisao: string
  dataFinalizacao?: string
  observacao?: string
}

export interface Produto {
  id: number
  codigo: string
  descricao: string
  unidadeMedida: string
  tempoProducaoMinutos: number
  ativo: boolean
}

export interface Linha {
  id: number
  nome: string
  descricao: string
  capacidadeHora: number
  ativa: boolean
}

export interface KpiGeral {
  oeePercentual: number
  taxaCumprimentoPrazo: number
  eficienciaProducao: number
  totalOrdensAbertas: number
  totalOrdensEmAndamento: number
  totalOrdensFinalizadas: number
  totalOrdensCanceladas: number
  totalOrdensAtrasadas: number
  tempoMedioParadaMinutos: number
}

export interface EficienciaLinha {
  linhaId: number
  nomeLinha: string
  capacidadeHora: number
  quantidadeProduzida: number
  eficienciaPercentual: number
  totalOrdens: number
  totalMinutosParada: number
}

export interface ProducaoDiaria {
  data: string
  quantidadePlanejada: number
  quantidadeProduzida: number
  totalOrdens: number
}

export interface ParetoParada {
  motivo: string
  totalMinutos: number
  ocorrencias: number
  percentualAcumulado: number
}

export interface TokenResponse {
  token: string
  refreshToken: string
  nomeUsuario: string
  perfil: string
}