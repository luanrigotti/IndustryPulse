import { useEffect, useState } from 'react'
import { buscarProdutos } from '../../api/produtos'
import type { Produto } from '../../types'
import api from '../../api/axios'

export default function ProdutosPage() {
  const [produtos, setProdutos] = useState<Produto[]>([])
  const [carregando, setCarregando] = useState(true)
  const [modalAberto, setModalAberto] = useState(false)
  const [form, setForm] = useState({
    codigo: '',
    descricao: '',
    unidadeMedida: '',
    tempoProducaoMinutos: 0
  })

  const carregar = async () => {
    try {
      const data = await buscarProdutos()
      setProdutos(data)
    } finally {
      setCarregando(false)
    }
  }

  useEffect(() => { carregar() }, [])

  const handleCriar = async (e: React.FormEvent) => {
    e.preventDefault()
    await api.post('/produtos', form)
    setModalAberto(false)
    setForm({ codigo: '', descricao: '', unidadeMedida: '', tempoProducaoMinutos: 0 })
    await carregar()
  }

  const handleDesativar = async (id: number) => {
    await api.delete(`/produtos/${id}`)
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
        <h2 className="text-2xl font-bold text-white">Produtos</h2>
        <button
          onClick={() => setModalAberto(true)}
          className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg transition-colors"
        >
          + Novo Produto
        </button>
      </div>

      <div className="bg-gray-800 rounded-lg overflow-hidden">
        <table className="w-full">
          <thead>
            <tr className="border-b border-gray-700">
              <th className="text-left px-6 py-4 text-gray-400 text-sm">Código</th>
              <th className="text-left px-6 py-4 text-gray-400 text-sm">Descrição</th>
              <th className="text-left px-6 py-4 text-gray-400 text-sm">Unidade</th>
              <th className="text-left px-6 py-4 text-gray-400 text-sm">Tempo (min)</th>
              <th className="text-left px-6 py-4 text-gray-400 text-sm">Status</th>
              <th className="text-left px-6 py-4 text-gray-400 text-sm">Ações</th>
            </tr>
          </thead>
          <tbody>
            {produtos.length === 0 ? (
              <tr>
                <td colSpan={6} className="text-center py-12 text-gray-500">
                  Nenhum produto cadastrado
                </td>
              </tr>
            ) : (
              produtos.map((produto) => (
                <tr key={produto.id} className="border-b border-gray-700">
                  <td className="px-6 py-4 text-white font-mono text-sm">
                    {produto.codigo}
                  </td>
                  <td className="px-6 py-4 text-gray-300 text-sm">
                    {produto.descricao}
                  </td>
                  <td className="px-6 py-4 text-gray-400 text-sm">
                    {produto.unidadeMedida}
                  </td>
                  <td className="px-6 py-4 text-gray-400 text-sm">
                    {produto.tempoProducaoMinutos}
                  </td>
                  <td className="px-6 py-4">
                    <span className={`px-2 py-1 rounded text-xs font-medium ${
                      produto.ativo
                        ? 'bg-green-900 text-green-300'
                        : 'bg-gray-700 text-gray-400'
                    }`}>
                      {produto.ativo ? 'Ativo' : 'Inativo'}
                    </span>
                  </td>
                  <td className="px-6 py-4">
                    {produto.ativo && (
                      <button
                        onClick={() => handleDesativar(produto.id)}
                        className="text-xs px-2 py-1 bg-red-800 hover:bg-red-700 text-white rounded transition-colors"
                      >
                        Desativar
                      </button>
                    )}
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {modalAberto && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <div className="bg-gray-800 rounded-lg p-6 w-full max-w-md">
            <h3 className="text-white font-bold text-lg mb-4">Novo Produto</h3>
            <form onSubmit={handleCriar} className="space-y-4">
              <div>
                <label className="block text-gray-400 text-sm mb-1">Código</label>
                <input
                  type="text"
                  value={form.codigo}
                  onChange={(e) => setForm({ ...form, codigo: e.target.value })}
                  className="w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded text-white"
                  required
                />
              </div>
              <div>
                <label className="block text-gray-400 text-sm mb-1">Descrição</label>
                <input
                  type="text"
                  value={form.descricao}
                  onChange={(e) => setForm({ ...form, descricao: e.target.value })}
                  className="w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded text-white"
                  required
                />
              </div>
              <div>
                <label className="block text-gray-400 text-sm mb-1">Unidade de Medida</label>
                <input
                  type="text"
                  value={form.unidadeMedida}
                  onChange={(e) => setForm({ ...form, unidadeMedida: e.target.value })}
                  className="w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded text-white"
                  placeholder="UN, KG, M..."
                  required
                />
              </div>
              <div>
                <label className="block text-gray-400 text-sm mb-1">Tempo de Produção (min)</label>
                <input
                  type="number"
                  value={form.tempoProducaoMinutos}
                  onChange={(e) => setForm({ ...form, tempoProducaoMinutos: Number(e.target.value) })}
                  className="w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded text-white"
                  min={1}
                  required
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
                  Criar
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  )
}