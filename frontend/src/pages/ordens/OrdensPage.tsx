import { useEffect, useState } from 'react'
import { buscarOrdens, criarOrdem, atualizarStatus } from '../../api/ordens'
import { buscarProdutos } from '../../api/produtos'
import { buscarLinhas } from '../../api/linhas'
import type { Ordem, Produto, Linha } from '../../types'
import { StatusOrdem } from '../../types'

const statusCor: Record<string, string> = {
  Aberta: 'bg-blue-900 text-blue-300',
  EmAndamento: 'bg-yellow-900 text-yellow-300',
  Finalizada: 'bg-green-900 text-green-300',
  Cancelada: 'bg-red-900 text-red-300',
}

export default function OrdensPage() {
  const [ordens, setOrdens] = useState<Ordem[]>([])
  const [produtos, setProdutos] = useState<Produto[]>([])
  const [linhas, setLinhas] = useState<Linha[]>([])
  const [carregando, setCarregando] = useState(true)
  const [modalAberto, setModalAberto] = useState(false)
  const [form, setForm] = useState({
    produtoId: 0,
    linhaProducaoId: 0,
    quantidadePlanejada: 0,
    dataPrevisao: '',
    observacao: ''
  })

  const carregar = async () => {
    try {
      const [ordensData, produtosData, linhasData] = await Promise.all([
        buscarOrdens(),
        buscarProdutos(),
        buscarLinhas()
      ])
      setOrdens(ordensData)
      setProdutos(produtosData)
      setLinhas(linhasData)
    } finally {
      setCarregando(false)
    }
  }

  useEffect(() => { carregar() }, [])

  const handleCriar = async (e: React.FormEvent) => {
    e.preventDefault()
    await criarOrdem({
      ...form,
      dataPrevisao: new Date(form.dataPrevisao).toISOString()
    })
    setModalAberto(false)
    setForm({
      produtoId: 0,
      linhaProducaoId: 0,
      quantidadePlanejada: 0,
      dataPrevisao: '',
      observacao: ''
    })
    await carregar()
  }

  const handleAtualizarStatus = async (id: number, novoStatus: string) => {
    await atualizarStatus(id, novoStatus)
    await carregar()
  }

  if (carregando) {
    return (
      <div className="flex items-center justify-center h-full">
        <p className="text-gray-400">Carregando...</p>
      </div>
    )
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h2 className="text-2xl font-bold text-white">Ordens de Produção</h2>
        <button
          onClick={() => setModalAberto(true)}
          className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg transition-colors"
        >
          + Nova Ordem
        </button>
      </div>

      {/* Tabela */}
      <div className="bg-gray-800 rounded-lg overflow-hidden">
        <table className="w-full">
          <thead>
            <tr className="border-b border-gray-700">
              <th className="text-left px-6 py-4 text-gray-400 text-sm">Número</th>
              <th className="text-left px-6 py-4 text-gray-400 text-sm">Produto</th>
              <th className="text-left px-6 py-4 text-gray-400 text-sm">Linha</th>
              <th className="text-left px-6 py-4 text-gray-400 text-sm">Progresso</th>
              <th className="text-left px-6 py-4 text-gray-400 text-sm">Status</th>
              <th className="text-left px-6 py-4 text-gray-400 text-sm">Previsão</th>
              <th className="text-left px-6 py-4 text-gray-400 text-sm">Ações</th>
            </tr>
          </thead>
          <tbody>
            {ordens.length === 0 ? (
              <tr>
                <td colSpan={7} className="text-center py-12 text-gray-500">
                  Nenhuma ordem cadastrada
                </td>
              </tr>
            ) : (
              ordens.map((ordem) => (
                <tr
                  key={ordem.id}
                  className={`border-b border-gray-700 hover:bg-gray-750 ${
                    ordem.estaAtrasada ? 'bg-red-950' : ''
                  }`}
                >
                  <td className="px-6 py-4 text-white font-mono text-sm">
                    {ordem.numero}
                    {ordem.estaAtrasada && (
                      <span className="ml-2 text-red-400 text-xs">⚠️ Atrasada</span>
                    )}
                  </td>
                  <td className="px-6 py-4 text-gray-300 text-sm">
                    {ordem.nomeProduto}
                  </td>
                  <td className="px-6 py-4 text-gray-300 text-sm">
                    {ordem.nomeLinhaProducao}
                  </td>
                  <td className="px-6 py-4">
                    <div className="flex items-center gap-2">
                      <div className="flex-1 bg-gray-700 rounded-full h-2">
                        <div
                          className="bg-blue-500 h-2 rounded-full"
                          style={{
                            width: `${Math.min(ordem.percentualConclusao, 100)}%`
                          }}
                        />
                      </div>
                      <span className="text-gray-400 text-xs w-10">
                        {ordem.percentualConclusao}%
                      </span>
                    </div>
                  </td>
                  <td className="px-6 py-4">
                    <span className={`px-2 py-1 rounded text-xs font-medium ${statusCor[ordem.status]}`}>
                      {ordem.status}
                    </span>
                  </td>
                  <td className="px-6 py-4 text-gray-400 text-sm">
                    {new Date(ordem.dataPrevisao).toLocaleDateString('pt-BR')}
                  </td>
                  <td className="px-6 py-4">
                    <div className="flex gap-2">
                      {ordem.status === StatusOrdem.Aberta && (
                        <button
                          onClick={() => handleAtualizarStatus(ordem.id, 'EmAndamento')}
                          className="text-xs px-2 py-1 bg-yellow-700 hover:bg-yellow-600 text-white rounded transition-colors"
                        >
                          Iniciar
                        </button>
                      )}
                      {ordem.status === StatusOrdem.EmAndamento && (
                        <button
                          onClick={() => handleAtualizarStatus(ordem.id, 'Finalizada')}
                          className="text-xs px-2 py-1 bg-green-700 hover:bg-green-600 text-white rounded transition-colors"
                        >
                          Finalizar
                        </button>
                      )}
                      {ordem.status === StatusOrdem.Aberta && (
                        <button
                          onClick={() => handleAtualizarStatus(ordem.id, 'Cancelada')}
                          className="text-xs px-2 py-1 bg-red-800 hover:bg-red-700 text-white rounded transition-colors"
                        >
                          Cancelar
                        </button>
                      )}
                    </div>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {/* Modal nova ordem */}
      {modalAberto && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <div className="bg-gray-800 rounded-lg p-6 w-full max-w-md">
            <h3 className="text-white font-bold text-lg mb-4">Nova Ordem de Produção</h3>
            <form onSubmit={handleCriar} className="space-y-4">
              <div>
                <label className="block text-gray-400 text-sm mb-1">Produto</label>
                <select
                  value={form.produtoId}
                  onChange={(e) => setForm({ ...form, produtoId: Number(e.target.value) })}
                  className="w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded text-white"
                  required
                >
                  <option value={0}>Selecione...</option>
                  {produtos.map((p) => (
                    <option key={p.id} value={p.id}>{p.codigo} — {p.descricao}</option>
                  ))}
                </select>
              </div>

              <div>
                <label className="block text-gray-400 text-sm mb-1">Linha de Produção</label>
                <select
                  value={form.linhaProducaoId}
                  onChange={(e) => setForm({ ...form, linhaProducaoId: Number(e.target.value) })}
                  className="w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded text-white"
                  required
                >
                  <option value={0}>Selecione...</option>
                  {linhas.map((l) => (
                    <option key={l.id} value={l.id}>{l.nome}</option>
                  ))}
                </select>
              </div>

              <div>
                <label className="block text-gray-400 text-sm mb-1">Quantidade Planejada</label>
                <input
                  type="number"
                  value={form.quantidadePlanejada}
                  onChange={(e) => setForm({ ...form, quantidadePlanejada: Number(e.target.value) })}
                  className="w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded text-white"
                  min={1}
                  required
                />
              </div>

              <div>
                <label className="block text-gray-400 text-sm mb-1">Data de Previsão</label>
                <input
                  type="date"
                  value={form.dataPrevisao}
                  onChange={(e) => setForm({ ...form, dataPrevisao: e.target.value })}
                  className="w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded text-white"
                  required
                />
              </div>

              <div>
                <label className="block text-gray-400 text-sm mb-1">Observação</label>
                <textarea
                  value={form.observacao}
                  onChange={(e) => setForm({ ...form, observacao: e.target.value })}
                  className="w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded text-white"
                  rows={2}
                />
              </div>

              <div className="flex gap-3 pt-2">
                <button
                  type="button"
                  onClick={() => setModalAberto(false)}
                  className="flex-1 py-2 bg-gray-700 hover:bg-gray-600 text-white rounded transition-colors"
                >
                  Cancelar
                </button>
                <button
                  type="submit"
                  className="flex-1 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded transition-colors"
                >
                  Criar Ordem
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  )
}