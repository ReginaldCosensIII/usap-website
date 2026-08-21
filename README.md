# USAP Website

ASP.NET Core 10 Razor Pages website for United States Antenna Products, LLC.
Delivered by Computer Enhancement Systems, Inc. (CES).
Currently in active development — Planning & Foundation milestone.

---

## Prerequisites

| Requirement | Version |
|---|---|
| .NET SDK | **10.0.400** (pinned in `global.json`) |
| Git | 2.x or later |
| OS | Windows x64 (IIS deployment target) |

Verify the local HTTPS dev certificate: `dotnet dev-certs https --check --trust`

---

## Common commands

```powershell
# Restore
dotnet restore USAP.Web.sln

# Build
dotnet build USAP.Web.sln --configuration Debug --no-restore
dotnet build USAP.Web.sln --configuration Release --no-restore

# Run (deterministic ports)
dotnet run --project src\USAP.Web\USAP.Web.csproj --urls "http://localhost:5080;https://localhost:5443"

# Format check
dotnet format USAP.Web.sln --verify-no-changes --no-restore

# Git status
git status --short --branch
```

---

## Configuration and secrets

`appsettings.json` and `appsettings.Development.json` contain no secrets.

Local development secrets (none required yet):

```powershell
dotnet user-secrets set "Section:Key" "value" --project src\USAP.Web\USAP.Web.csproj
```

Never commit secrets. Production settings belong in IIS environment variables.

---

## Documentation

| Document | Purpose |
|---|---|
| `docs/PROJECT_OVERVIEW.md` | Client, contractor, contract status, milestones, scope |
| `docs/ARCHITECTURE.md` | Runtime, SDK, solution structure, data approach, IIS plan |
| `docs/ROUTES_AND_CONTENT.md` | Route map and page status |
| `docs/CONTENT_AND_ASSETS.md` | Content status, reference assets, outstanding client inputs |
| `docs/DECISION_LOG.md` | Consequential decisions |
| `docs/DEPLOYMENT_RUNBOOK.md` | IIS deployment baseline |
| `docs/HANDOFF.md` | Repository ownership and operational state |

---

## Current state

- Foundation: minimal semantic shell, approved route stubs, no production content.
- No favicon or production branding supplied yet.
- Reference materials in `docs/reference-materials/` — not served publicly, not production content.
- Manual browser QA completed by Reggie on 2026-08-21.
- Builds: Debug and Release 0 errors / 0 warnings.
- All eight approved routes return HTTP 200.

---

## Git and approval workflow

All commits, pushes, and merges require explicit approval from Reggie.
The approved remote is `origin https://github.com/ReginaldCosensIII/usap-website.git`.
See `AGENTS.md` for the complete Implementation Agent workflow.

---

## Deployment

Framework-dependent deployment to CES Windows Server/IIS.
See `docs/DEPLOYMENT_RUNBOOK.md` for details.
