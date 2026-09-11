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

---

## DEC-017 — Provisional semantic design tokens, system fonts, and progressive-enhancement disclosure navigation

**Date:** 2026-08-21 (updated 2026-08-23)
**Decision:** Design system implemented with provisional semantic CSS custom property tokens centralized in `site.css`. System font stack used. Mobile navigation implemented as a CSS disclosure with a small dependency-free, external, deferred JavaScript file.
**Reason:**
- **Provisional tokens:** Official brand assets (colors, typography, logo) are not yet supplied. Centralizing values in `:root` allows a complete visual update from one location when assets arrive, without HTML or layout restructuring.
- **System fonts:** Zero external font requests. Eliminates a latency dependency, a privacy concern, and a branding decision that should be made with official assets.
- **Disclosure navigation:** A progressive-enhancement disclosure (`aria-expanded` + `hidden` attribute) avoids a modal focus trap, body-scroll locking, and any external library dependency. Navigation works without JavaScript. The deferred external script (`site-navigation.js`) adds enhancement only.
- **No third-party CSS or JS:** No framework or library lock-in. All code is auditable and replaceable.
**Impact:**
- Desktop navigation and white header activate at `64rem` (1024px). Tablet (768px) and below use the mobile disclosure pattern on a navy header.
- Two focus-ring tokens: `--focus-ring` (blue, approximately 6.87:1 on white) for light surfaces; `--focus-ring-dark` (white, approximately 17.31:1 on navy) for dark/navy surfaces.
- Mobile active-route indicator uses `--color-accent-dark-surface: #ff4d5f` (approximately 5.34:1 on navy) for a left border that independently meets 3:1 contrast.
- All visual token values and the navigation JS contract are documented in `docs/DESIGN_SYSTEM.md`. Replacing provisional values or extending the system in subsequent milestones does not require HTML restructuring.
**Classification:** Fixed scope — Planning & Foundation / Design System and Responsive Foundation.

---

## DEC-018 — Reusable Head and Metadata Foundation

**Date:** 2026-08-31
**Decision:**
- Canonical production origin confirmed as `https://www.usantennaproducts.com`.
- Canonical URLs must use the configured production origin rather than incoming Host headers.
- Option B selected: strongly typed `SeoMetadata` passed through ViewData and rendered by a shared partial (`_Seo.cshtml`).
- The current hero poster is accepted only as a provisional default social image.
- A final branded 1200x630 social image remains required.
- Twitter/X handles are omitted until approved.
- Advanced structured data remains deferred.
- **C16A Addendum:** Placeholder product-family routes are `noindex, follow`. They must remain unindexed until catalog data and slug validation (returning true 404s for invalid slugs) are implemented.
**Reason:** C16 Foundation implementation and C16A Corrective Review.

---

## DEC-019 - Search Indexing and Structured Data Foundation

**Date:** 2026-08-31
**Decision:**
- Static sitemap architecture selected over dynamic generation due to the limited number of current static routes.
- Static robots.txt selected. Existing non-production header retained.
- Shared Organization and WebSite JSON-LD added via _StructuredData.cshtml.
- Logo omitted from structured data pending final production asset.
- Product/breadcrumb schemas deferred.
- Redirect map documented (docs/REDIRECT_MAP.md) but redirects not yet implemented.
- Sitemap and structured-data values require launch reconfirmation.
**Reason:** USAP-SEO-002-C17 Search Indexing implementation.

---

## DEC-020 — Custom 404 and Production Error Handling

**Date:** 2026-09-01
**Decision:**
- Dedicated `/not-found` Razor Page created to handle HTTP 404 responses via `app.UseStatusCodePagesWithReExecute("/not-found")`.
- Existing `/Error` Razor Page retained exclusively for HTTP 500 unexpected server failures via `app.UseExceptionHandler("/Error")`.
- HTTP status codes are explicitly preserved.
- Canonical and `og:url` tags are omitted on error responses to prevent indexing issues.
- `Organization` and `WebSite` JSON-LD intentionally remain sitewide.
- Product slug validation and legacy redirects remain deferred.
- The error pages share a transparent light-surface technical overlay composite PNG as a restrained background texture.
**Reason:** Separates 404 marketing experience from 500 technical support experience, avoiding recursive error loops and making `PageModel` logic simpler.

---

## DEC-021 — Form-page background texture experiment rejected; branch scope reduced

**Date:** 2026-09-02
**Decision:**
- A controlled visual trial applied the approved composite PNG (`usap-light-surface-technical-signal-overlay-v1.png`) to the Contact Us and Request a Quote page surfaces as a desktop-only background texture.
- Multiple scaling and positioning variants (`cover`, `100% auto` + `center top`, `100% auto` + `center center`) were reviewed.
- The treatment was rejected. The 1983 × 793 asset does not suit tall, form-focused layouts at any tested position or scale. At all variants, the artwork introduced visual noise without improving page hierarchy or usability.
- The implementation was removed before push or merge. The form pages retain their existing clean presentation.
- `ContactUs.cshtml`, `RequestAQuote.cshtml`, and `site.css` are identical to `main`.
- A possible form-page redesign may be considered later as a separate task. That future task must not assume that a background texture is the correct solution.
- The calibration-marks V2 SVG (`usap-calibration-marks-v2.svg`, SHA256: `48CFD82707231B744760B2B529601EA0293C6A27F9C23BBCAADFB3FD86C6EAE8`) is retained as an approved optional decorative asset. It is not deployed on any page.
- Signal-arcs V2 SVG remains rejected and must not be used, restored, or recreated.
- Substantial internal-page work (products, resources, about) takes priority over further decorative texture experimentation.
**Classification:** Experiment closed without scope addition. No functional change.
