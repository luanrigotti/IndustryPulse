import { useEffect, useState } from 'react'
import {
  BarChart, Bar, XAxis, YAxis, CartesianGrid,
  Tooltip, ResponsiveContainer, PieChart, Pie,
  Cell, Legend
} from 'recharts'
import {
  buscarKpis,
  buscarProducaoDiaria,
  buscarEficienciaLinhas,
  buscarParetoParadas
} from '../../api/dashboard'
import type { KpiGeral, ProducaoDiaria, EficienciaLinha, ParetoParada } from '../../types'
import { Calendar, CalendarClock, ChartColumn, Clock, Cog, Triangle, TriangleAlert, type LucideIcon } from 'lucide-react'

const CORES = ['#3b82f6', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6']

interface CardKpiProps {
  titulo: string
  valor: string
  icone: LucideIcon
  cor: string
  subtitulo?: string
}

function CardKpi({ titulo, valor, icone, cor, subtitulo }: CardKpiProps) {
  const Icone = icone;

  return (
    <div className="bg-gray-800 rounded-lg p-6">
      <div className="flex items-center justify-between mb-2">
        <span className="text-gray-400 text-sm">{titulo}</span>
        <Icone className={`w-6 h-6 ${cor}`} />
      </div>
      <p className={`text-3xl font-bold ${cor}`}>{valor}</p>
      {subtitulo && <p className="text-gray-500 text-xs mt-1">{subtitulo}</p>}
    </div>
  )
}

export default function DashboardPage() {
  const [kpis, setKpis] = useState<KpiGeral | null>(null)
  const [producao, setProducao] = useState<ProducaoDiaria[]>([])
  const [eficiencia, setEficiencia] = useState<EficienciaLinha[]>([])
  const [pareto, setPareto] = useState<ParetoParada[]>([])
  const [carregando, setCarregando] = useState(true)

  const hoje = new Date()
  const inicioMes = new Date(hoje.getFullYear(), hoje.getMonth(), 1)
    .toISOString()
  const fim = hoje.toISOString()

  useEffect(() => {
    const carregar = async () => {
      try {
        const [kpisData, producaoData, eficienciaData, paretoData] =
          await Promise.all([
            buscarKpis(inicioMes, fim),
            buscarProducaoDiaria(30),
            buscarEficienciaLinhas(inicioMes, fim),
            buscarParetoParadas(inicioMes, fim)
          ])
        setKpis(kpisData)
        setProducao(producaoData)
        setEficiencia(eficienciaData)
        setPareto(paretoData)
      } catch (err) {
        console.error('Erro ao carregar dashboard', err)
      } finally {
        setCarregando(false)
      }
    }
    carregar()
  }, [])

  if (carregando) {
    return (
      <div className="flex items-center justify-center h-full">
        <p className="text-gray-400">Carregando...</p>
      </div>
    )
  }

  const dadosStatus = [
    { name: 'Abertas', value: kpis?.totalOrdensAbertas ?? 0 },
    { name: 'Em Andamento', value: kpis?.totalOrdensEmAndamento ?? 0 },
    { name: 'Finalizadas', value: kpis?.totalOrdensFinalizadas ?? 0 },
    { name: 'Canceladas', value: kpis?.totalOrdensCanceladas ?? 0 },
  ].filter(d => d.value > 0)

  return (
    <div className="space-y-6">
      <h2 className="text-2xl font-bold text-white">Dashboard</h2>

      {/* Cards KPI */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        <CardKpi
          titulo="OEE"
          valor={`${kpis?.oeePercentual ?? 0}%`}
          icone={Cog}
          cor="text-blue-400"
          subtitulo="Meta: 85%"
        />
        <CardKpi
          titulo="Cumprimento de Prazo"
          valor={`${kpis?.taxaCumprimentoPrazo ?? 0}%`}
          icone={CalendarClock}
          cor={
            (kpis?.taxaCumprimentoPrazo ?? 0) >= 90
              ? 'text-green-400'
              : 'text-red-400'
          }
          subtitulo="Meta: 90%"
        />
        <CardKpi
          titulo="Eficiência"
          valor={`${kpis?.eficienciaProducao ?? 0}%`}
          icone={ChartColumn}
          cor="text-yellow-400"
          subtitulo="Produzido vs Planejado"
        />
        <CardKpi
          titulo="Ordens Atrasadas"
          valor={`${kpis?.totalOrdensAtrasadas ?? 0}`}
          icone={TriangleAlert}
          cor={
            (kpis?.totalOrdensAtrasadas ?? 0) === 0
              ? 'text-green-400'
              : 'text-red-400'
          }
          subtitulo="Requerem atenção"
        />
      </div>

      {/* Gráficos linha 1 */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">

        {/* Produção diária */}
        <div className="bg-gray-800 rounded-lg p-6">
          <h3 className="text-white font-medium mb-4">Produção Diária</h3>
          <ResponsiveContainer width="100%" height={250}>
            <BarChart data={producao.slice(-15)}>
              <CartesianGrid strokeDasharray="3 3" stroke="#374151" />
              <XAxis
                dataKey="data"
                stroke="#9ca3af"
                tick={{ fontSize: 10 }}
                tickFormatter={(v) => new Date(v).getDate().toString()}
              />
              <YAxis stroke="#9ca3af" tick={{ fontSize: 10 }} />
              <Tooltip
                contentStyle={{
                  backgroundColor: '#1f2937',
                  border: 'none',
                  borderRadius: '8px',
                  color: '#fff'
                }}
                labelFormatter={(v) => new Date(v).toLocaleDateString('pt-BR')}
              />
              <Bar dataKey="quantidadeProduzida" fill="#3b82f6" name="Produzido" />
              <Bar dataKey="quantidadePlanejada" fill="#6b7280" name="Planejado" />
            </BarChart>
          </ResponsiveContainer>
        </div>

        {/* Status das ordens */}
        <div className="bg-gray-800 rounded-lg p-6">
          <h3 className="text-white font-medium mb-4">Status das Ordens</h3>
          {dadosStatus.length === 0 ? (
            <div className="flex items-center justify-center h-48">
              <p className="text-gray-500">Nenhuma ordem no período</p>
            </div>
          ) : (
            <ResponsiveContainer width="100%" height={250}>
              <PieChart>
                <Pie
                  data={dadosStatus}
                  cx="50%"
                  cy="50%"
                  innerRadius={60}
                  outerRadius={100}
                  dataKey="value"
                  label={({ name, value }) => `${name}: ${value}`}
                >
                  {dadosStatus.map((_, index) => (
                    <Cell
                      key={`cell-${index}`}
                      fill={CORES[index % CORES.length]}
                    />
                  ))}
                </Pie>
                <Tooltip
                  contentStyle={{
                    backgroundColor: '#1f2937',
                    border: 'none',
                    borderRadius: '8px',
                    color: '#fff'
                  }}
                />
              </PieChart>
            </ResponsiveContainer>
          )}
        </div>
      </div>

      {/* Gráficos linha 2 */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">

        {/* Eficiência por linha */}
        <div className="bg-gray-800 rounded-lg p-6">
          <h3 className="text-white font-medium mb-4">Eficiência por Linha</h3>
          {eficiencia.length === 0 ? (
            <div className="flex items-center justify-center h-48">
              <p className="text-gray-500">Nenhum dado no período</p>
            </div>
          ) : (
            <ResponsiveContainer width="100%" height={250}>
              <BarChart data={eficiencia} layout="vertical">
                <CartesianGrid strokeDasharray="3 3" stroke="#374151" />
                <XAxis
                  type="number"
                  domain={[0, 100]}
                  stroke="#9ca3af"
                  tick={{ fontSize: 10 }}
                />
                <YAxis
                  type="category"
                  dataKey="nomeLinha"
                  stroke="#9ca3af"
                  tick={{ fontSize: 12 }}
                />
                <Tooltip
                  contentStyle={{
                    backgroundColor: '#1f2937',
                    border: 'none',
                    borderRadius: '8px',
                    color: '#fff'
                  }}
                  formatter={(value) => [`${value}%`, 'Eficiência']}
                />
                <Bar dataKey="eficienciaPercentual" fill="#10b981" name="Eficiência %" />
              </BarChart>
            </ResponsiveContainer>
          )}
        </div>

        {/* Pareto de paradas */}
        <div className="bg-gray-800 rounded-lg p-6">
          <h3 className="text-white font-medium mb-4">Pareto de Paradas</h3>
          {pareto.length === 0 ? (
            <div className="flex items-center justify-center h-48">
              <p className="text-gray-500">Nenhuma parada registrada</p>
            </div>
          ) : (
            <ResponsiveContainer width="100%" height={250}>
              <BarChart data={pareto}>
                <CartesianGrid strokeDasharray="3 3" stroke="#374151" />
                <XAxis
                  dataKey="motivo"
                  stroke="#9ca3af"
                  tick={{ fontSize: 10 }}
                />
                <YAxis stroke="#9ca3af" tick={{ fontSize: 10 }} />
                <Tooltip
                  contentStyle={{
                    backgroundColor: '#1f2937',
                    border: 'none',
                    borderRadius: '8px',
                    color: '#fff'
                  }}
                />
                <Bar dataKey="totalMinutos" fill="#f59e0b" name="Minutos" />
              </BarChart>
            </ResponsiveContainer>
          )}
        </div>
      </div>

      {/* Resumo de ordens */}
      <div className="bg-gray-800 rounded-lg p-6">
        <h3 className="text-white font-medium mb-4">Resumo do Período</h3>
        <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
          <div className="text-center">
            <p className="text-3xl font-bold text-blue-400">
              {kpis?.totalOrdensAbertas ?? 0}
            </p>
            <p className="text-gray-400 text-sm mt-1">Abertas</p>
          </div>
          <div className="text-center">
            <p className="text-3xl font-bold text-yellow-400">
              {kpis?.totalOrdensEmAndamento ?? 0}
            </p>
            <p className="text-gray-400 text-sm mt-1">Em Andamento</p>
          </div>
          <div className="text-center">
            <p className="text-3xl font-bold text-green-400">
              {kpis?.totalOrdensFinalizadas ?? 0}
            </p>
            <p className="text-gray-400 text-sm mt-1">Finalizadas</p>
          </div>
          <div className="text-center">
            <p className="text-3xl font-bold text-red-400">
              {kpis?.totalOrdensCanceladas ?? 0}
            </p>
            <p className="text-gray-400 text-sm mt-1">Canceladas</p>
          </div>
        </div>
      </div>
    </div>
  )
}