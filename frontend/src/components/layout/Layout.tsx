import type { ReactNode } from 'react'
import { Link, useLocation, useNavigate } from 'react-router-dom'
import { useAuth } from '../../contexts/AuthContext'
import { ClipboardList, Factory, LayoutDashboard, Package } from 'lucide-react'

interface Props {
  children: ReactNode
}

const menuItems = [
  { path: '/dashboard', label: 'Dashboard', icon: LayoutDashboard },
  { path: '/ordens', label: 'Ordens', icon: ClipboardList },
  { path: '/produtos', label: 'Produtos', icon: Package },
  { path: '/linhas', label: 'Linhas', icon: Factory },
]

export default function Layout({ children }: Props) {
  const { pathname } = useLocation()
  const { usuario, logout } = useAuth()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  return (
    <div className="flex h-screen bg-gray-900 text-white">
      {/* Sidebar */}
      <aside className="w-64 bg-gray-800 flex flex-col">
        <div className="p-6 border-b border-gray-700">
          <h1 className="text-xl font-bold flex items-center gap-2">
            <Factory className="w-5 h-5" />
            IndústryPulse
            </h1>
          <p className="text-gray-400 text-sm mt-1">KPIs Industriais</p>
        </div>

        <nav className="flex-1 p-4 space-y-1">
          {menuItems.map((item) => {
            const Icon = item.icon
            return (
            <Link
              key={item.path}
              to={item.path}
              className={`flex items-center gap-2 px-4 py-3 rounded-lg transition-colors ${
                pathname === item.path
                  ? 'bg-blue-600 text-white'
                  : 'text-gray-300 hover:bg-gray-700'
              }`}
            >
              <Icon className="w-5 h-5 text-blue-400" />
              {item.label}
            </Link>
            )
          })}
        </nav>

        <div className="p-4 border-t border-gray-700">
          <p className="text-gray-400 text-sm">{usuario?.nome}</p>
          <p className="text-gray-500 text-xs">{usuario?.perfil}</p>
          <button
            onClick={handleLogout}
            className="mt-2 text-red-400 hover:text-red-300 text-sm transition-colors"
          >
            Sair
          </button>
        </div>
      </aside>

      {/* Conteúdo */}
      <main className="flex-1 overflow-auto p-6">
        {children}
      </main>
    </div>
  )
}