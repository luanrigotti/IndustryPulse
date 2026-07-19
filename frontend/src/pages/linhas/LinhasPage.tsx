import { useEffect, useState } from 'react'
import { buscarLinhas } from '../../api/linhas'
import type { Linha } from '../../types'
import api from '../../api/axios'

interface FormLinha {
  nome: string
  descricao: string
  capacidadeHora: number | ''
}

export default function LinhasPage() {
  const [linhas, setLinhas] = useState<Linha[]>([])
  const [carregando, setCarregando] = useState(true)
  const [modalAberto, setModalAberto] = useState(false)
  const [form, setForm] = useState<FormLinha>({
    nome: '',
    descricao: '',
    capacidadeHora: ''
  })

  const carregar = async () => {
    try {
      const data = await buscarLinhas()
      setLinhas(data)
    } finally {
      setCarregando(false)
    }
  }

  useEffect(() => { carregar() }, [])

  const handleCriar = async (e: React.FormEvent) => {
    e.preventDefault()
    await api.post('/linhas', form)
    setModalAberto(false)
    setForm({ nome: '', descricao: '', capacidadeHora: '' })
    await carregar()
  }

  const handleDesativar = async (id: number) => {
    await api.delete(`/linhas/${id}`)
    await carregar()
  }

  const handleAtivar = async (id: number) => {
  await api.put(`/linhas/${id}`)
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
        <h2 className="text-2xl font-bold text-white">Linhas de Produção</h2>
        <button
          onClick={() => setModalAberto(true)}
          className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg transition-colors"
        >
          + Nova Linha
        </button>
      </div>

      <div className="bg-gray-800 rounded-lg overflow-hidden">
        <table className="w-full">
          <thead>
            <tr className="border-b border-gray-700">
              <th className="text-left px-6 py-4 text-gray-400 text-sm">Nome</th>
              <th className="text-left px-6 py-4 text-gray-400 text-sm">Descrição</th>
              <th className="text-left px-6 py-4 text-gray-400 text-sm">Capacidade/hora</th>
              <th className="text-left px-6 py-4 text-gray-400 text-sm">Status</th>
              <th className="text-left px-6 py-4 text-gray-400 text-sm">Ações</th>
            </tr>
          </thead>
          <tbody>
            {linhas.length === 0 ? (
              <tr>
                <td colSpan={5} className="text-center py-12 text-gray-500">
                  Nenhuma linha cadastrada
                </td>
              </tr>
            ) : (
              linhas.map((linha) => (
                <tr key={linha.id} className="border-b border-gray-700">
                  <td className="px-6 py-4 text-white font-medium">
                    {linha.nome}
                  </td>
                  <td className="px-6 py-4 text-gray-400 text-sm">
                    {linha.descricao}
                  </td>
                  <td className="px-6 py-4 text-gray-400 text-sm">
                    {linha.capacidadeHora} un/h
                  </td>
                  <td className="px-6 py-4">
                    <span className={`px-2 py-1 rounded text-xs font-medium ${
                      linha.ativa
                        ? 'bg-green-900 text-green-300'
                        : 'bg-gray-700 text-gray-400'
                    }`}>
                      {linha.ativa ? 'Ativa' : 'Inativa'}
                    </span>
                  </td>
                  <td className="px-6 py-4">
                    {linha.ativa ? (
                      <button
                        onClick={() => handleDesativar(linha.id)}
                        className="text-xs px-2 py-1 bg-red-800 hover:bg-red-700 text-white rounded transition-colors"
                      >
                        Desativar
                      </button>
                    ) : (
                      <button
                        onClick={() => handleAtivar(linha.id)}
                        className="text-xs px-2 py-1 bg-green-800 hover:bg-green-700 text-white rounded transition-colors"
                      >
                        Ativar
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
            <h3 className="text-white font-bold text-lg mb-4">Nova Linha de Produção</h3>
            <form onSubmit={handleCriar} className="space-y-4">
              <div>
                <label className="block text-gray-400 text-sm mb-1">Nome</label>
                <input
                  type="text"
                  value={form.nome}
                  onChange={(e) => setForm({ ...form, nome: e.target.value })}
                  className="w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded text-white"
                  placeholder="Linha A"
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
                <label className="block text-gray-400 text-sm mb-1">Capacidade por hora</label>
                <input
                  type="number"
                  value={form.capacidadeHora}
                  onChange={(e) => {
                    const val = e.target.value;
                    setForm({ ... form, capacidadeHora: val === '' ? '' : Number(val) })
                  }}
                  onFocus={(e) => e.target.select()}
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