import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { AuthProvider } from './contexts/AuthContext'
import ProtectedRoute from './components/layout/ProtectedRoute'
import Layout from './components/layout/Layout'
import LoginPage from './pages/auth/LoginPage'
import DashboardPage from './pages/dashboard/DashboardPage'


export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/" element={<Navigate to="/dashboard" />} />

          <Route
            path="/dashboard"
            element={
              <ProtectedRoute>
                <Layout>
                  <DashboardPage />
                </Layout>
              </ProtectedRoute>
            }
          />

          <Route
            path="/ordens"
            element={
              <ProtectedRoute>
                <Layout>
                  <div className="text-white">Ordens em construção...</div>
                </Layout>
              </ProtectedRoute>
            }
          />

          <Route
            path="/produtos"
            element={
              <ProtectedRoute>
                <Layout>
                  <div className="text-white">Produtos em construção...</div>
                </Layout>
              </ProtectedRoute>
            }
          />

          <Route
            path="/linhas"
            element={
              <ProtectedRoute>
                <Layout>
                  <div className="text-white">Linhas em construção...</div>
                </Layout>
              </ProtectedRoute>
            }
          />
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  )
}