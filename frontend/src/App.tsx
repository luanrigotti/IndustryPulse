import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { AuthProvider } from './contexts/AuthContext'
import ProtectedRoute from './components/layout/ProtectedRoute'
import Layout from './components/layout/Layout'
import LoginPage from './pages/auth/LoginPage'
import DashboardPage from './pages/dashboard/DashboardPage'
import OrdensPage from './pages/ordens/OrdensPage'
import ProdutosPage from './pages/produtos/ProdutosPage'
import LinhasPage from './pages/linhas/LinhasPage'

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/" element={<Navigate to="/dashboard" />} />

          <Route path="/dashboard" element={
            <ProtectedRoute>
              <Layout><DashboardPage /></Layout>
            </ProtectedRoute>
          } />

          <Route path="/ordens" element={
            <ProtectedRoute>
              <Layout><OrdensPage /></Layout>
            </ProtectedRoute>
          } />

          <Route path="/produtos" element={
            <ProtectedRoute>
              <Layout><ProdutosPage /></Layout>
            </ProtectedRoute>
          } />

          <Route path="/linhas" element={
            <ProtectedRoute>
              <Layout><LinhasPage /></Layout>
            </ProtectedRoute>
          } />
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  )
}