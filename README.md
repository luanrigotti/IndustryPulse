# 🏭 IndústryPulse

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet)
![React](https://img.shields.io/badge/React-18-61DAFB?style=flat&logo=react)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-4169E1?style=flat&logo=postgresql)
![License](https://img.shields.io/badge/License-MIT-green?style=flat)
![Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-yellow?style=flat)

## Descrição

Dashboard de KPIs industriais fullstack baseado em processos reais de PCP (Planejamento e Controle da Produção).

Desenvolvido com base em 3 anos de experiência em ambiente industrial — os indicadores implementados são utilizados no dia a dia de fábricas. Transforma dados de ordens de produção em indicadores visuais em tempo real, eliminando a dependência de relatórios manuais no ERP.

**Funcionalidades:**
- Dashboard com OEE, cumprimento de prazo, eficiência e ordens atrasadas
- Gestão de ordens com fluxo de status — Aberta → Em Andamento → Finalizada
- Registro de paradas com motivo e duração — base do Pareto de perdas
- Cadastro de produtos e linhas de produção
- Autenticação com JWT e perfis de acesso
- Testes unitários com cobertura dos principais fluxos de negócio

> ⚠️ **Projeto em desenvolvimento.** O sistema está funcional localmente mas ainda não possui deploy em produção. Novas melhorias e funcionalidades serão implementadas continuamente.

## Tecnologias

**Back-end:** ASP.NET Core 8 · Entity Framework Core 8 · PostgreSQL · JWT · Swagger

**Front-end:** React 18 · TypeScript · Tailwind CSS · Recharts · Axios

**Testes:** xUnit · Moq · FluentAssertions

**Arquitetura:** Clean Architecture · Repository Pattern · Rich Domain Model

## Instalação

### Requisitos
- .NET 8 SDK
- Node.js 18+
- PostgreSQL

### Back-end

```bash
git clone https://github.com/luanrigotti/IndustryPulse.git
cd IndustryPulse

# Configure a connection string em src/IndustryPulse.API/appsettings.json
# "DefaultConnection": "Host=localhost;Port=5432;Database=IndustryPulseDB;Username=postgres;Password=sua_senha"

dotnet ef database update \
  --project src/IndustryPulse.Infrastructure/IndustryPulse.Infrastructure.csproj \
  --startup-project src/IndustryPulse.API/IndustryPulse.API.csproj

dotnet run --project src/IndustryPulse.API/IndustryPulse.API.csproj
```

### Front-end

```bash
cd frontend
npm install
npm run dev
```

## Uso

- **Frontend:** http://localhost:5173
- **Swagger:** http://localhost:5222/swagger
- **Login:** admin@industrypulse.com / Admin@123

## Autor

**Luan Martini Rigotti**

Graduando em Análise e Desenvolvimento de Sistemas na UCS. 3 anos de experiência em ambiente industrial. Em transição para desenvolvimento de software com foco em C# e ASP.NET Core.

[LinkedIn](https://linkedin.com/in/luanrigotti) · [GitHub](https://github.com/luanrigotti)

## Licença

MIT
