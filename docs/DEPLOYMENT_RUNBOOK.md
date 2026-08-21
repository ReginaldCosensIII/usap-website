# Deployment Runbook

This document records the initial deployment baseline. It will be updated as the deployment environment is configured and verified.

**Nothing in this document has been configured or verified on IIS. All IIS sections are pending.**

---

## Deployment model

| Item | Value |
|---|---|
| Deployment type | Framework-dependent |
| Hosting | IIS via ASP.NET Core Module (ANCM) |
| Target OS | Windows Server (CES-managed) |
| Runtime requirement | ASP.NET Core Runtime `10.0.x` (installed via Hosting Bundle) |
| Publish command | `dotnet publish` with Release configuration |

---

## Server requirements

### CES Dev server

- [ ] .NET SDK `10.0.400` installed (needed if `dotnet run` or `dotnet build` on server)
- [ ] ASP.NET Core Hosting Bundle for .NET 10 installed
- [ ] ASP.NET Core Module (ANCM) registered in IIS (installed by Hosting Bundle)
- [ ] IIS configured with a site and application pool

### Production server

- [ ] ASP.NET Core Hosting Bundle for .NET 10 installed (no SDK required for published output)
- [ ] ASP.NET Core Module (ANCM) registered in IIS (installed by Hosting Bundle)
- [ ] IIS configured with a site and application pool

---

## HTTPS certificate approach

| Environment | Certificate requirement |
|---|---|
| Local development | ASP.NET Core development certificate (self-signed localhost) — NOT used for IIS |
| CES Dev (IIS) | A certificate that matches the Dev hostname — self-signed or internal CA acceptable; testers must trust it |
| Production (IIS) | Approved trusted certificate from a recognized CA — must match the production hostname |

**Certificate private keys must never be committed to Git or stored in application settings files.**

Production certificate management is the responsibility of the party controlling the production server and domain.

---

## Configuration approach

Production and environment-specific configuration is supplied through:

1. IIS environment variables set in the application pool or site configuration.
2. `web.config` `<environmentVariables>` section (for non-secret values).
3. No secrets appear in `appsettings.json` or `appsettings.Production.json`.

---

## Pending information

The following details are not yet known and will be recorded here when available:

- [ ] CES Dev server hostname and IIS site name
- [ ] CES Dev application pool name and identity
- [ ] Production server hostname
- [ ] Production application pool name and identity
- [ ] HTTPS certificate source and renewal plan for Production
- [ ] File system path for published application on Dev and Production
- [ ] Deploy trigger (manual publish, CI/CD, or other)
- [ ] Rollback procedure
- [ ] Post-deploy verification checklist
- [ ] Monitoring and alerting approach

---

## Known limitations

- IIS has not been configured or verified as of 2026-08-20.
- The local development certificate is not applicable to IIS deployments.
- No deployment has been performed.
- No rollback procedure has been defined.
