# Decision Log

Consequential decisions only. Routine implementation details are not logged.

---

## DEC-001 — Target framework: net10.0

**Date:** 2026-08-20
**Decision:** Target `net10.0`.
**Reason:** SDK `10.0.400` is installed; ASP.NET Core Runtime `10.0.11` is present; CES environment will use matching runtimes.
**Impact:** All packages and runtime APIs must be compatible with `net10.0`. IIS Hosting Bundle must match.
**Follow-up:** CES Dev needs SDK + Hosting Bundle; Production needs Hosting Bundle only.

---

## DEC-002 — SDK pin: 10.0.400

**Date:** 2026-08-20
**Decision:** Pin to SDK `10.0.400` via `global.json` with `rollForward: latestPatch` and `allowPrerelease: false`.
**Reason:** Deterministic builds; `latestPatch` allows security updates within the `10.0.4xx` band without manual changes.
**Impact:** All contributors must have SDK `10.0.400` or a compatible patch installed.

---

## DEC-003 — ASP.NET Core Razor Pages with IIS hosting

**Date:** 2026-08-20
**Decision:** Server-rendered Razor Pages; framework-dependent deployment to IIS via ANCM.
**Reason:** Page-centric application; IIS is the existing CES infrastructure; framework-dependent avoids bundling the runtime.
**Impact:** All page handlers follow Razor Pages conventions. IIS requires the Hosting Bundle.

---

## DEC-004 — No database, CMS, SPA, or frontend build pipeline

**Date:** 2026-08-20
**Decision:** No ORM, CMS, SPA framework, or frontend build pipeline. Product and resource data will use static structured files.
**Reason:** Explicitly excluded by fixed scope. Adds unauthorized complexity and cost.
**Follow-up:** Static data format finalized in Catalog & Resources.

---

## DEC-005 — No Bootstrap, jQuery, or third-party frontend libraries

**Date:** 2026-08-20
**Decision:** Removed Bootstrap, jQuery, jquery-validation, and unobtrusive validation from the `dotnet new webapp` template output. Replaced with a minimal custom CSS shell.
**Reason:** Prohibited by fixed scope. Defers design-system decisions to Core Site Build.

---

## DEC-006 — Framework-dependent deployment

**Date:** 2026-08-20
**Decision:** Publish as framework-dependent (not self-contained).
**Reason:** CES manages the server and installs the Hosting Bundle. Framework-dependent produces smaller output and is the standard IIS approach.
**Impact:** CES Dev and Production must have the Hosting Bundle for net10.0.

---

## DEC-007 — Private personal GitHub repository as initial remote

**Date:** 2026-08-20
**Decision:** Use `https://github.com/ReginaldCosensIII/usap-website.git` as `origin`.
**Reason:** Repository was pre-created and approved. Personal account is adequate for the current phase.
**Impact:** Ownership is provisional. Transfer requires written approval. See `docs/HANDOFF.md`.

---

## DEC-008 — Repository transfer subject to contract terms and written approval

**Date:** 2026-08-20
**Decision:** Any transfer of the repository to a client-owned or CES organizational account requires written approval from both parties, consistent with the applicable contract terms.
**Reason:** Transfer without explicit agreement could create ownership disputes.
**Impact:** Repository remains under personal account until applicable decisions are made.
**Follow-up:** Revisit at QA & Launch milestone gate.

---

## DEC-009 — Uppercase canonical documentation filenames

**Date:** 2026-08-20
**Decision:** All-uppercase filenames for canonical documentation (e.g., `ARCHITECTURE.md`).
**Reason:** Consistent with workflow-package convention; prominent and conventional for project-governance documents.

---

## DEC-010 — Reference imagery restricted to docs/reference-materials/

**Date:** 2026-08-20
**Decision:** The four reference assets (wireframes, design concept, logo reference) remain in `docs/reference-materials/` only. None may be copied to `wwwroot` or used in production.
**Reason:** The design concept contains unapproved copy. The logo is a low-resolution screenshot. Using these in production creates quality, legal, and brand risks.
**Follow-up:** See `docs/CONTENT_AND_ASSETS.md` for asset details and replacement requirements.

---

## DEC-011 — Local dev certificate vs. IIS certificate

**Date:** 2026-08-20
**Decision:** Development certificate is for localhost only. IIS requires a separate certificate matching the site hostname.
**Reason:** They are fundamentally different certificates. Certificate private keys must never be committed to Git.
**Follow-up:** IIS certificate details in `docs/DEPLOYMENT_RUNBOOK.md` when available.

---

## DEC-012 — CES Dev needs SDK + Hosting Bundle; Production needs Hosting Bundle only

**Date:** 2026-08-20
**Decision:** CES Dev: SDK `10.0.400` + Hosting Bundle. Production: Hosting Bundle only.
**Reason:** Dev may be used for building; Production hosts published output only. No SDK needed to run a published framework-dependent application.
**Follow-up:** Confirm installation when deployment runbook is executed.

---

## DEC-013 — Repository-level NuGet.config to clear stale fallback folder

**Date:** 2026-08-20
**Decision:** Added repository-level `NuGet.config` that clears the fallback package folders list.
**Reason:** `dotnet build` failed because a machine-level NuGet config (`Microsoft.VisualStudio.FallbackLocation.config`) referenced `D:\VS2022\shared\NuGetPackages` from a prior VS installation no longer at that path. Repository-level override is safe, committed, and reproducible. It does not affect package sources.
**Classification:** Defect correction — no scope addition.

---

## DEC-014 — Classic .sln generated with supported --format sln option

**Date:** 2026-08-20 (corrected 2026-08-21)
**Decision:** `USAP.Web.sln` is a classic `.sln` file required for broad Visual Studio 2022 compatibility.
**Generation commands:**
```powershell
dotnet new sln --name USAP.Web --format sln --output . --force
dotnet sln USAP.Web.sln add --in-root src\USAP.Web\USAP.Web.csproj
```
**Reason:** `dotnet new sln` in SDK `10.0.400` creates `.slnx` by default. The `--format sln` option is the supported way to produce the classic format. The initial PF-002 implementation fabricated the file manually; the PF-002-C1 correction regenerated it with the correct command.
**Impact:** The GUID is toolchain-generated (`{43148D99-161F-4813-9C7D-E0536CA95A05}`), not a placeholder.

---

## DEC-015 — Lean documentation model

**Date:** 2026-08-21
**Decision:** The human lead authorized reducing ongoing repository documentation to a practical lean set. Seven documents (CONTENT_INVENTORY, ASSET_REGISTER, CLIENT_CONTENT_REQUEST, WORK_LOG, QA_LOG, PROJECT_DOCUMENTATION_WORKFLOW, IMPLEMENTATION_AGENT_STANDARDS) are consolidated or removed.
**Replacement:** Content inventory, asset status, and client inputs are consolidated into `docs/CONTENT_AND_ASSETS.md`. Time and work tracking remain in the human lead's external spreadsheet. Validation evidence lives primarily in Implementation Agent return reports. `DESIGN_SYSTEM.md` and `REDIRECT_MAP.md` are created only when their respective work begins.
**Retained lean set:** README.md, AGENTS.md, PROJECT_OVERVIEW.md, ARCHITECTURE.md, ROUTES_AND_CONTENT.md, CONTENT_AND_ASSETS.md, DECISION_LOG.md, DEPLOYMENT_RUNBOOK.md, HANDOFF.md.
**Approver:** Human project lead (Reggie).

---

## DEC-016 — flex-column page shell; no overflow-x masking

**Date:** 2026-08-21
**Decision:** `body` uses `min-height: 100vh; display: flex; flex-direction: column`. `main` uses `flex: 1; width: 100%`. `overflow-x: hidden` is not applied.
**Reason:** Ensures the footer reaches the bottom of the viewport on short pages without masking layout defects. Horizontal overflow is addressed through correct widths, not masking.
**Verified:** Manual browser check by Reggie on 2026-08-21 confirmed no horizontal overflow at ≈390 px and ≈768 px.
