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

---

## DEC-022 — Shared Internal-Page Hero Foundation, Contact Sidebar, Maps Embed, and Reference-Grounded Asset Promotion

**Date:** 2026-09-11
**Decision:**
- Promoted the About Us hero structure and styling into a unified shared design-system component (`.internal-hero`) in `site.css`, supporting semantic landmarks, intrinsic background image rendering, shared contrast overlay protection, and page-specific focal position modifiers (`.internal-hero--about`, `.internal-hero--contact`, `.internal-hero--quote`).
- Applied the shared hero foundation across `/about-us`, `/contact-us`, and `/request-a-quote`.
- Promoted six approved assets from `USAP_Internal_Page_Asset_Review_Package_2026-09-11.zip` into `src/USAP.Web/wwwroot/images/` (3 About Us reference-grounded assets, 1 Contact Us conceptual hero, 1 Request a Quote conceptual hero, and 1 store-only Technical Resources hero).
- Completely removed three superseded provisional About assets from `wwwroot/images/about/` after verifying zero remaining runtime references.
- Implemented a shared two-column responsive form-page layout (`.form-page-layout`) with desktop grid and mobile column stack, keeping the primary form first in DOM and accessibility order.
- Created `_ContactSidebar.cshtml` partial housing verified public USAP contact details (address, phone, fax, directions link) and an HTTPS API-key-free Google Maps embed with lazy loading and accessible fallback link.
- Strictly preserved all form-processing, PageModel logic, validation attributes, antiforgery tokens, honeypot fields, reCAPTCHA integration, rate limiting, and email dispatch services.
- Preserved alternate candidates exclusively in `project-input/USAP_Internal_Page_Asset_Review_Package_2026-09-11.zip`.
- Maintained the Technical Resources hero asset as store-only, deferred from runtime presentation until authorized.
**Reason:** Checkpoint USAP-CONTACT-ASSETS-001-C1 to establish unified internal-page layout foundations and integrate review-grounded visual assets.

---

## DEC-023 — Contact Us Hero Revision: Nonmedical Signal Treatment and Provenance Preservation

**Date:** 2026-09-12
**Decision:**
- Replaced the Contact Us hero production asset `src/USAP.Web/wwwroot/images/contact-us/usap-contact-hero-communication-signal-candidate-c-v1.png` with a revised rendering that removes the electronic signal waveform resembling an ECG/EKG or medical telemetry monitor.
- Replaced the waveform with clean, nonmedical communication signal bars inside the speech bubble while preserving the overall glass/chrome speech bubble composition, dark industrial lighting, and right focal weighting.
- Preserved the identical production filename (`usap-contact-hero-communication-signal-candidate-c-v1.png`) and runtime path, requiring zero changes to Razor markup or CSS styles.
- Preserved the original archive `project-input/USAP_Internal_Page_Asset_Review_Package_2026-09-11.zip` (SHA-256: `3BB2A04AAED355C9DF3209F49C3A92D57E946F12FB1D968868F6363904391267`) untouched as a historical source record containing the superseded waveform asset.
- Created a supplemental provenance directory `project-input/USAP_Contact_Hero_Revision_2026-09-12/` containing a byte-for-byte copy of the revised asset, `CHECKSUMS.sha256` (`B020AD39E6A8CEFD15C584DCA9B77503070CDE8FD0C33B63C6A556F645CD71E4`), and explanatory `README.md`.
- Maintained status as pending USAP/client visual approval.
**Reason:** Checkpoint USAP-CONTACT-ASSETS-001-C2 to eliminate medical misinterpretation while preserving unified design-system styling and runtime stability.

---

## DEC-024 — Six-Family Product Catalog Taxonomy, Canonical 16-Group Distribution, Repository-Native Catalog Service, and Responsive Landing Page Foundation in Milestone C1

**Date:** 2026-09-12
**Decision:**
- Approved six-family taxonomy established in exact canonical display order with exact 5 / 3 / 1 / 1 / 5 / 1 product-group distribution (16 distinct groups total):
  1. `Log Periodic Antennas` (`/products/log-periodic-antennas`): 5 groups (`lp-high-power`, `lp-1017`, `lp-1018ba`, `lp-1019`, `lp-1112mr`). Conservative summary: "Broadband directional antenna systems spanning HF through UHF applications."
  2. `Portable & Transportable Antenna Systems` (`/products/portable-transportable-antennas`): 3 groups (`v-4213`, `lp-1402-1403`, `1910`). Conservative summary: "Field-deployable antenna systems covering HF and VHF communications."
  3. `Aperiodic Loop Antennas` (`/products/aperiodic-loop-antennas`): 1 group (`aperiodic`, unnamed model line). Conservative summary: "Receive-focused fixed and transportable loop-array solutions."
  4. `NVIS Antennas` (`/products/nvis-antennas`): 1 group (`1942`, covering models 1942-RT, 1942-TA, 1942-GM, with low-power variants on conflict hold). Conservative summary: "Near Vertical Incidence Skywave antenna configurations for HF communications."
  5. `Antenna Rotator & Control Systems` (`/products/antenna-rotator-control-systems`): 5 groups (`r3500`, `r3501`, `r3503`, `drc-3`, `drc-4`). Conservative summary: "Mechanical rotators and digital control systems for directional antenna installations."
  6. `Tower Systems & Accessories` (`/products/tower-systems-accessories`): 1 group (`t-3002`, covering parent and child ordering configurations 3002FA, 3002FB, 3002SS, 3002SS-80). Conservative summary: "Tower, mast, rotation, feedline, and installation configurations for large antenna systems."
- LP-1112MR is primarily classified under Log Periodic Antennas, preserving public catalog convention, with transportable characteristics captured in metadata. LP-1402/LP-1403 is classified under Portable & Transportable Antenna Systems.
- Catalog contains exactly 30 named model/configuration codes and 1 unnamed Aperiodic Loop product-line record.
- Three active family card imagery cutouts are designated as "project-selected reference-grounded candidate — USAP approval pending", with native placeholder frames maintained for the remaining three families.
- Implemented strongly typed domain models (`CatalogAsset`, `ProductFamilyRecord`, `ProductGroupRecord`, `ProductModelRecord`, `ProductResourceRecord`) and singleton service `ProductCatalogService` registered in DI via `IProductCatalogService`.
- Strict startup/construction validation implemented enforcing exact 6 families, unique IDs/slugs, sequential display order (1..6), valid slug regex, exact per-family group distribution (5/3/1/1/5/1), ordered group ID matching, 30 named models + 1 unnamed, and valid family/product/resource relationships.
- Replaced provisional `/products` scaffold with production-ready landing page utilizing shared `.internal-hero`, 3-col/2-col/1-col responsive card grid, source-constrained selection-help section, and shared closing CTA.
- Reserved six canonical family routes via `Pages/Products/Family.cshtml` using shared `.internal-hero` placeholders, breadcrumbs, provisional notice banner, and audited product groups; invalid slugs return HTTP 404 (eliminating previous soft-404 gap).
- Full product detail pages and specification tables deferred to Milestone C2. Disputed specifications (LP-1001 impedance, LP-1017 radiation angle, V-4213 wind rating, etc.) are strictly excluded from landing page copy.
- Public site legacy redirects and PDF migration deferred to Milestone C3.
**Reason:** Milestone USAP-PRODUCTS-001-C1 structured catalog foundation, canonical 16-group research inventory alignment, and landing page implementation.

---

## DEC-025 — Product Catalog Visual and Semantic Refinements: Relocated Breadcrumbs, Stretched-Link Entire Card Activation, Restrained Editorial CTA, 16:10 Edge-to-Edge Media Frames, Accent Restraint, and Public Guidance Notice (Milestone C1B)

**Date:** 2026-09-13
**Decision:**
- **Relocated Breadcrumbs:** Removed breadcrumb navigation from inside the dark `.internal-hero--family` banner across all six product-family routes. Rendered semantic `<nav aria-label="Breadcrumb">` directly below the hero as the first element in the page-content area, aligned with `.container`, styled for light page backgrounds, with high-contrast links, neutral `/` separators, visible 3px focus rings, and non-linked current family item (`aria-current="page"`).
- **Entire Family Card Clickable:** Implemented accessible `.stretched-link` pattern (`.stretched-link::after` absolute overlay with `z-index: 2`) on each landing page family card. The entire visible card surface (media frame, eyebrow, heading, summary, body padding, visible CTA) activates the family route without JavaScript event handlers, preserving native browser navigation, single Tab stop per card, context menus, and destination display in status bar.
- **Editorial Card Link CTA:** Replaced filled button styling (`.btn.btn-secondary.btn-sm`) on family cards with a restrained editorial text-link and arrow pattern (`Explore Family →`). Aligned at the bottom of the card body via flexbox (`margin-top: auto`), with accessible family-specific name (`aria-label="Explore @family.Name"`). Visibly responds to card hover and keyboard focus with color shift and directional arrow translation.
- **Uniform Card Alignment:** Standardized card body flexbox layout ensuring equal row heights and bottom-aligned CTAs across varied title and summary text lengths without text truncation or line clamping.
- **16:10 Edge-to-Edge Media Frames:** Removed inset white padding and side gutters from family-card media frames. Set `aspect-ratio: 16 / 10` with `overflow: hidden` and `object-fit: cover; object-position: center; display: block; padding: 0;`. Applied identical geometry to native design placeholders. Maintained subtle image scale zoom (`scale(1.03)`) on card hover and keyboard focus, fully suppressed under `prefers-reduced-motion: reduce`.
- **Accent Restraint:** Removed red top borders (`card--accent-top-red`) from normal family cards and repeated navy left borders (`card--accent-left-navy`) from product-group cards. Reserved accent borders strictly for intentionally designated notice and guidance components (such as the engineering selection help cards and product information notices). Catalog cards use neutral borders (`var(--card-border)`), white surfaces, and subtle elevation.
- **Deliberate Design Placeholder Terminology:** Distinguished native CSS design placeholders from production photography and candidate imagery. Updated visible badges to `PRODUCT IMAGERY IN DEVELOPMENT` and captions to `Design placeholder — replacement imagery pending USAP review.`
- **Public Guidance Notice:** Completely eliminated internal milestone and checkpoint terminology (`Checkpoint C1`, `Milestone C1`, `Milestone C2`, `provisional route reservation`) from rendered public pages. Replaced with restrained `Product Information` notice advising visitors that detailed specifications, datasheets, dimension diagrams, and radiation patterns are being audited for publication, providing a direct link to USAP engineering.
- **Asset Discipline:** Confirmed that the separately generated A2 asset package was deliberately not imported for C1B, preserving current repository imagery and native design placeholders to evaluate visual and semantic foundations independently.
**Reason:** Checkpoint USAP-PRODUCTS-001-C1B visual and semantic refinement.

---

## DEC-026 — Candidate Image Alt Text and Source-Constrained Selection Guidance Refinement (Milestone C1B Follow-Up)

**Date:** 2026-09-13
**Decision:**
- **Candidate-Image Alt Text:** Replaced exact-model assertions on active family card images with concise, visually descriptive family-level alt text that accurately describes what is visually present without asserting unapproved product identity:
  - Log Periodic: "Log periodic antenna array in a field installation."
  - Portable & Transportable: "Transportable HF antenna system in a field setting."
  - Rotator & Control: "Antenna rotator and digital controller components."
- **Source-Constrained Selection-Assistance Copy:** Audited the three selection-assistance cards on `/products` against canonical research inputs. Replaced draft copy with conservative, source-constrained descriptions:
  - Deployment & Mobility: "Compare field-deployable antenna configurations with fixed base-station installations based on transportation, setup, and operating requirements."
  - Radiation Profile: "Compare directional log periodic, omnidirectional, NVIS, and receive-focused loop-array configurations based on the intended communications requirement."
  - Integration & Hardware: "Consider mechanical rotators, digital controllers, tower or mast support, feedline, and installation hardware as part of the complete antenna system."
  - Specifically removed unsubstantiated claims: "long-term continuous duty", "terrain obstacle mitigation", "low-noise", "precision", "microprocessor", and "modular aluminum".
- **Preserved C1/C1A/C1B Foundations:** Retained all six families, 16 groups (5/3/1/1/5/1), 30 named models, 11 validation tests, 13 route statuses (including true invalid-slug 404), stretched-link card activation, breadcrumb placement, and unstaged Git discipline.
**Reason:** Checkpoint USAP-PRODUCTS-001-C1B follow-up review corrections.

---

## DEC-027 — Products Landing Page Hero Direction C and Six Family-Card Candidate Asset Integration (Task USAP-CATALOG-001)

**Date:** 2026-09-15
**Decision:**
- **Selected A2F Candidate Asset Set:** Promoted seven project-selected assets from `project-inputs/USAP_Product_Catalog_A2F_Review_Package_2026-09-15.zip` into `src/USAP.Web/wwwroot/images/products/` with byte-identical hash preservation:
  1. Products Landing Hero: Direction C (`usap-products-landing-hero-direction-c-candidate-v1.png`, 2048 × 768 px).
  2. Log Periodic Antennas: Reference-grounded family visual (`usap-family-card-log-periodic-family-visual-v1.png`, 1600 × 1000 px).
  3. Portable & Transportable Antenna Systems: Stylized placeholder visual (`usap-family-card-portable-transportable-placeholder-v1.png`, 1600 × 1000 px).
  4. Aperiodic Loop Antennas: Reference-grounded element visual (`usap-family-card-aperiodic-loop-element-v1.png`, 1600 × 1000 px).
  5. NVIS Antennas: Reference-grounded family visual (`usap-family-card-nvis-1942-family-visual-v1.png`, 1600 × 1000 px).
  6. Antenna Rotator & Control Systems: Conceptual lower-risk indicator Candidate B (`usap-family-card-rotator-control-lower-risk-placeholder-v2.png`, 1600 × 1000 px).
  7. Tower Systems & Accessories: Structural mast/tower illustration Candidate B (`usap-family-card-tower-systems-nonconfigurational-fallback-v2.png`, 1600 × 1000 px).
- **Candidate Selection Rationale:**
  - Hero Direction C selected over Directions A and B because its left ~35% provides a deep-navy gradient area ensuring WCAG AAA text contrast for the hero title, eyebrow, and introductory text, while the right ~65% showcases antenna geometry and blueprint motifs.
  - Rotator Candidate B selected over Candidate A (rejected for depicting unverified specific chassis/hardware features) and Candidate C (rejected for excessive abstraction).
  - Tower Candidate B selected over Candidate A (rejected for depicting an unverified specific 3002SS hardware configuration).
- **Provisional Status & Replacement Contract:** All seven assets are classified as provisional review candidates pending formal USAP product-owner visual approval. None represent certified or verified production hardware photography. They are architecturally decoupled in `IProductCatalogService` so they can be replaced by client-approved photography without requiring markup or layout changes.
- **Archive Integrity:** Unselected candidate directions (Hero A/B, Rotator A/C, Tower A), comparison sheets, and internal review derivatives remain strictly within the ignored intake archive and are not tracked or served in `wwwroot`.
- **Preserved Foundations:** The authorized C1/C1A/C1B taxonomy (6 families, 16 groups, 30 models), 16:10 edge-to-edge geometry, stretched-link navigation, breadcrumb placement, and native CSS design placeholder fallbacks are 100% preserved.
**Reason:** Task USAP-CATALOG-001 Products landing page and provisional asset integration.

---

## DEC-028 — Evidence-Safe Family Route Shells, A2F Classification Alignment, Input-Directory Hygiene, and QA Evidence Repair (Checkpoint USAP-CATALOG-001-QA1)

**Date:** 2026-09-15
**Decision:**
- **Evidence-Safe Family Route Shells:**
  - Added shared public disclaimer notice across all six family routes in `Pages/Products/Family.cshtml`:
    *"The models and configurations below are references from USAP’s published materials. Current availability, specifications, compatibility, and supported configurations require confirmation from USAP engineering."*
  - Replaced section eyebrow with `PUBLISHED PRODUCT REFERENCES`, heading with `Models Referenced in This Family`, and model label with `Published Model References:`.
  - Audited and qualified all 16 product group titles, descriptions, and model configuration strings in `ProductCatalogService.cs` to remove unsubstantiated current-status claims, unverified controller pairings (R3500/DRC-4), unverified controller support (DRC-3/DRC-4), unverified wind/capacity claims (R3503, V-4213), and single-element vs array confusion (Aperiodic Loop).
  - Preserved all 6 families, 16 canonical group IDs, and 30 canonical model codes.
- **A2F Asset Classification Alignment:**
  - Aligned all documentation and return reporting to exact A2F classifications:
    1. Products hero: `conceptual family visual`
    2. Log Periodic: `family-level conceptual/reference-grounded visual`
    3. Portable & Transportable: `conceptual family placeholder`
    4. Aperiodic Loop: `reference-grounded single-element visualization`
    5. NVIS: `1942-family-level reference-grounded visualization`
    6. Rotator & Control: `reference-grounded rotator with illustrative controller study`
    7. Tower Systems: `design fallback — non-configurational family study`
  - Preserved approved live alt texts byte-for-byte in `ProductCatalogService.cs`. Confirmed the Log Periodic asset is not identified as an LP-1017 photograph and no candidate is represented as exact product photography.
- **Input-Directory Hygiene & Archive Locations:**
  - Located `USAP_Product_Catalog_A2F_Review_Package_2026-09-15.zip` in `project-inputs/` (plural, 57,753,091 bytes).
  - Located `USAP_Product_Catalog_A3_Family_Hero_Review_Package_2026-09-15.zip` in `project-input/` (singular, 34,654,781 bytes).
  - Both directories exist on disk and are strictly ignored in `.gitignore`. Retaining both rules is mandatory due to historical intake directory naming conventions across milestones. A3 archive remains unextracted and unintegrated.
- **Review Evidence Repair:**
  - Repaired defective screenshots: `02_six_family_grid_1440px.png` and `04_six_family_grid_768px.png` (expanded viewport height and enabled `captureBeyondViewport` to capture complete 6-card grids without blank clipping); `07_family_card_keyboard_focus_card_crop.png` (centered active card bounding rect with 15px padding to fully show the 3px focus outline).
  - Captured browser audit verifying status notice visibility and absence of prohibited current-status terms across all 6 family routes.
- **Review Package Portability:**
  - Generated portable ZIP `USAP-CATALOG-001-QA1-review-package.zip` outside repository with forward-slash entry paths, SHA-256 manifest, separately stated file count, textual source diff snapshot (`source_changes.diff`), and verified archive integrity.
**Reason:** Conditional architectural review findings remediation for USAP-CATALOG-001.

---

## DEC-029 — Current-Site Catalog Policy Alignment, Natural Product Titles, Four-Dimensional Metadata Separation, and A3 Hash Resolution (Checkpoint USAP-CATALOG-001-QA2)

**Date:** 2026-09-15
**Decision:**
- **Binding Catalog Policy (Current-Site Authority):**
  - Directed by project lead decision: until USAP provides a controlled product list, the current public USAP website is the authority for which products belong in the catalog.
  - Every product or configuration listed on the current USAP website remains included in the catalog (all 16 groups, 30 named models, 1 unnamed line).
  - Products are not assumed to be currently manufactured, stocked, or available for purchase, nor are they labeled retired, discontinued, legacy, or obsolete unless explicitly stated by USAP or the current site.
  - Current HTML product pages control public product naming and general descriptions.
- **Natural Public Catalog Tone & Section Language:**
  - Removed repeated archival word `Reference` or `References` from all 16 public product group titles.
  - Standardized on natural first-party product names: `High-Power HF Log Periodics`, `LP-1017 Log Periodic`, `LP-1018BA Broadband Log Periodic`, `LP-1019 Series`, `LP-1112MR Transportable Log Periodic`, `V-4213 Portable Discone`, `LP-1402 / LP-1403 Transportable Log Periodics`, `1910 Tactical Dipoles`, `USAP Aperiodic Loop Antenna`, `1942 NVIS Series`, `R3500 Heavy Duty Rotator with DRC-4 Rotator Control Unit`, `R3501 Universal Rotator System`, `R3503 Heavy Duty Rotating System`, `DRC-3 Digital Rotator Controller`, `DRC-4 Digital Rotator Controller`, `T-3002 RLPA Tower System`.
  - Replaced section eyebrow with `PRODUCT CATALOG`, heading with `Products and Models in This Family`, lead with `The following product groups and configurations are cataloged for <familyName>:`, and model list label with `Models and Configurations:`.
  - Rewrote shared notice across all six family routes to:
    > *"Product information below is based on USAP’s current public website and linked technical documents. Contact USAP engineering to confirm availability, configuration, compatibility, and final specifications for your application."*
  - Replaced repetitive "Published catalog reference for..." descriptions with concise, useful descriptions grounded in current HTML product pages.
  - Maintained documented published relationships: R3500/DRC-4 pairing and DRC-3 compatibility with R3501/R3503 presented as current-site published relationships without purchase guarantees.
- **Four-Dimensional Catalog Metadata Separation:**
  - Distinct dimensions established in domain models (`ProductGroupRecord`, `ProductModelRecord`):
    1. *Source presence:* Listed on current USAP website
    2. *Commercial availability:* Not confirmed
    3. *Client approval:* Pending
    4. *Specification status:* Confirmed from current HTML / Provisional / HoldDisputedSpecs
  - No legacy or historical badges are rendered publicly at this time.
- **Preservation of Technical Resources for Later Lifecycle Classification:**
  - All 19 discovered datasheets and revisions remain cataloged and linked to their respective product groups.
  - Documented future lifecycle capability allowing products to transition to legacy/support classification without deleting product pages, specifications, downloads, revision history, or support documentation.
- **A3 Archive Hash Verification & Discrepancy Resolution:**
  - Conducted read-only inspection of `project-input/USAP_Product_Catalog_A3_Family_Hero_Review_Package_2026-09-15.zip`.
  - Recalculated outer SHA-256: `D5C60786BABE6533DA5DB1A30947649F621D038B410DDB7D7DD4DF41A96B4E79`.
  - Byte length: `34,654,781` bytes.
  - Confirmed the repository copy matches the expected archive hash exactly. The previously reported hash (`61a5ac...`) was an erroneous record in earlier return materials.
  - ZIP CRC integrity passed. The archive contains 46 total ZIP entries: 10 directory entries and 36 regular files. Of the 36 regular files, 35 are covered by `usap-product-catalog-a3/SHA256_MANIFEST.txt`, plus the manifest file itself. Archive remains strictly unextracted, unmodified, and unintegrated pending human lead authorization.
- **Review Package & Validation:**
  - Portable review package `USAP-CATALOG-001-QA2R-review-package.zip` generated for external verification.
  - Integrity: 100% forward-slash paths, ZIP CRC test passed, 100% manifest verification passed on clean extraction.
  - Final package metrics (exact byte size, SHA-256 checksum, manifest-covered file count, and total entry count) are reported in the external return report to avoid circular checksum dependencies.
**Reason:** Binding catalog-governance policy alignment from human project lead.


---

## DEC-030 — Family Asset Integration, Card Unification, Guided Selection, and FAQ Completion (Checkpoint USAP-CATALOG-001-C2)

**Date:** 2026-09-16
**Decision:**
- **Promotion of 16 Assets from A3 and A3S Archives:**
  - Integrated 16 approved asset derivatives into `src/USAP.Web/wwwroot/images/products/`:
    1. Card Asset: Promoted Rotator & Control Candidate B (`usap-family-card-rotator-control-r3500-drc4-a3s-recommended-v1.png`). Strictly rejected A3S Candidate A.
    2. Desktop & Mobile Family Heroes (6 pairs, 1536×576 desktop and 768×768 mobile):
       - Log Periodic: Retained A3 Candidate A V2 (`usap-family-log-periodic-antennas-hero-candidate-a-v2-desktop-preview.png`, `usap-family-log-periodic-antennas-hero-candidate-a-v2-mobile-crop.png`).
       - Portable & Transportable: Promoted A3S (`usap-family-portable-transportable-hero-a3s-desktop-1536x576.png`, `usap-family-portable-transportable-hero-a3s-mobile-768x768.png`).
       - Aperiodic Loop: Retained A3 Candidate A V1 (`usap-family-aperiodic-loop-antennas-hero-candidate-a-v1-desktop-preview.png`, `usap-family-aperiodic-loop-antennas-hero-candidate-a-v1-mobile-crop.png`).
       - NVIS: Retained A3 Candidate A V2 (`usap-family-nvis-antennas-hero-candidate-a-v2-desktop-preview.png`, `usap-family-nvis-antennas-hero-candidate-a-v2-mobile-crop.png`).
       - Rotator & Control: Promoted A3S (`usap-family-rotator-control-hero-a3s-desktop-1536x576.png`, `usap-family-rotator-control-systems-hero-a3s-mobile-768x768.png`).
       - Tower Systems & Accessories: Promoted A3S (`usap-family-tower-systems-hero-a3s-desktop-1536x576.png`, `usap-family-tower-systems-hero-a3s-mobile-768x768.png`).
    3. Dedicated Rotator Hardware Visuals (3 assets, 1600×1000):
       - Promoted A3S DRC-3 (`usap-drc3-source-guided-product-visual-a3s-v1.png`), DRC-4 (`usap-drc4-source-guided-product-visual-a3s-v1.png`), and R3500/DRC-4 relationship study (`usap-r3500-drc4-source-guided-relationship-a3s-v1.png`).
  - Strict source-to-public traceability: All 16 assets were verified programmatically against source archive paths and copied without re-encoding, preserving exact byte counts and SHA-256 hashes.
- **Explicit Domain Model Classifications (No Misleading Defaults):**
  - Created `ResponsiveHeroAsset` record requiring explicit `AssetClassification` and `ApprovalStatus`:
    - Log Periodic hero: `conceptual/reference-grounded family visual`
    - Aperiodic Loop hero: `reference-grounded single-element visualization`
    - NVIS hero: `1942-family-level reference-grounded visualization`
    - Portable & Transportable hero: `source-guided photorealistic product visualization`
    - Rotator & Control hero: `source-guided photorealistic product visualization`
    - Tower Systems & Accessories hero: `source-guided photorealistic product visualization`
    - Rotator product-specific visuals: `source-guided photorealistic product visualization`
  - Approval status across all promoted assets: `provisional — USAP review pending`.
- **Responsive `<picture>` Implementation & Non-Overlapping Breakpoint:**
  - Implemented `<picture>` in `Pages/Products/Family.cshtml` using `<source media="(max-width: 47.999rem)" ...>` to align cleanly with desktop CSS `@media (min-width: 48rem)` without an overlapping 48rem/768px collision.
  - Browser verification at 390px, 767px, 768px, 1024px, 1440px confirmed exact agreement: 768px loads the 1536×576 desktop image in desktop layout; 767px and below load the 768×768 mobile crop.
  - Preserved copy-safe left overlay and per-family focal positions in `site.css`.
- **Unified Product Family Card & Homepage Section 5 Integration:**
  - Created `Pages/Shared/_ProductFamilyCard.cshtml` implementing the stretched-link single Tab-stop pattern.
  - Homepage Section 5 updated to inject `IProductCatalogService` and display 4 canonical featured families (Log Periodic, Portable & Transportable, Rotator & Control, Tower Systems & Accessories) linking directly to `/products/{slug}`, while preserving the section-level `View All Products` action routing to `/products`.
  - Audited and retired obsolete `.product-card` and `.product-media` CSS rules in `homepage.css`.
  - Context-sensitive card custom properties (`--family-card-eyebrow-min-height`, `--family-card-title-min-height`, `--family-card-summary-min-height`) tuned separately for 3-column (`.products-family-grid`) and 4-column (`.home-featured-products .products-grid`) layouts.
  - Automated DOM and computed-style audits confirmed 100% parity across borders, radii, shadows, media ratios, typography, hover elevation, focus rings, and bottom-aligned CTAs, with complete equal-row-height alignment and mobile `auto` height reset.
- **Actionable Engineering Guidance & Lead-Gen Streamlining:**
  - Refactored Engineering Guidance into 3 actionable pathways: Deployment & Mobility, Coverage & Propagation, Positioning & Infrastructure.
  - Broadened Deployment & Mobility to reference multiple families (Portable, Log Periodic, Tower Systems).
  - Replaced duplicate 2-button lead-gen CTA with a restrained inline continuation notice (`.guidance-continuation`) linking to `#products-faq` and `#closing-cta-heading`, preserving a single clear primary lead-conversion destination.
- **Native Product Selection FAQ:**
  - Implemented 6-question FAQ on `/products` using native semantic `<details>` and `<summary>` without JavaScript.
  - Suppressed marker rotation under `prefers-reduced-motion`.
- **Dedicated Rotator Product Visuals Section:**
  - Implemented 3-card figure grid on `/products/antenna-rotator-control-systems` highlighting DRC-3, DRC-4, and R3500/DRC-4 relationship.
  - Added single restrained section note explaining images are source-guided visualizations based on published USAP materials, avoiding repetitive disclaimers under each visual.
- **DRC-4 Branding Limitation Documented:**
**Reason:** Checkpoint USAP-CATALOG-001-C2 approved scope execution and implementation amendments.

---

## DEC-031: Product Catalog Presentation, Alignment, and Architecture Refinement (C2R / C2R1 / C2R2)

**Date:** 2026-09-16
**Status:** Approved by Human Project Lead
**Deciders:** Human Project Lead (Reggie, CES), System Engineer / Planning Agent, Implementation Agent
**Context:** Checkpoint `USAP-CATALOG-001-C2` established a solid data foundation (canonical 5/3/1/1/5/1 family hierarchy, 16 product groups, 30 named models, slug routing with 404 validation). However, independent project lead and stakeholder review identified presentation, governance, and architectural refinements required before client review. These requirements were formalized in the approved `USAP-CATALOG-001-C2R`, `USAP-CATALOG-001-C2R1`, and `USAP-CATALOG-001-C2R2` implementation instructions.
**Decisions:**
1. **Catalog Model Architecture Preservation:**
   - Preserved existing positional record signatures in `ProductFamilyRecord`, `ProductGroupRecord`, `ProductModelRecord`, and `ProductResourceRecord`.
   - Added computed fallback property `DisplayTitle => string.IsNullOrWhiteSpace(CardTitle) ? Name : CardTitle;` on `ProductFamilyRecord`.
   - Added non-destructive optional property `CatalogAsset? AssociatedAsset = null` to `ProductGroupRecord`.
2. **Authorized Homepage Family Selection:**
   - Restored the 4 authorized homepage families: 1. Log Periodic Antennas, 2. Portable & Transportable, 3. Rotator & Control Systems, 4. Tower Systems & Accessories.
   - Restored balanced 2×2 desktop/tablet and 1×4 mobile layout using dedicated `_HomeProductFamilyCard.cshtml`.
3. **Dedicated Card-Partial Architecture:**
   - Implemented `Pages/Shared/_HomeProductFamilyCard.cshtml` for the homepage 2×2 grid and `Pages/Shared/_ProductFamilyCard.cshtml` for the `/products` 3×2 grid.
   - Both components preserve 16:10 media aspect ratio, single keyboard Tab stop, complete-card click activation via `.stretched-link`, and zero interactive element nesting.
4. **Compact Catalog Card Rhythm & Shared Component Reservations (C2R2):**
   - Replaced oversized C2R1 vertical reservations (`2.25lh`, `2.4lh`, `5lh` producing ~626px cards) with the compact, content-driven component contract at multi-column breakpoints (`48rem` and `64rem`):
     - `--family-card-eyebrow-min-height: 2lh;` (reserves exactly 2 lines for eyebrow)
     - `--family-card-title-min-height: 2lh;` (reserves exactly 2 lines for heading)
     - `--family-card-summary-min-height: auto;` (allows summary to take its natural content height)
     - Bottom CTA alignment preserved across cards in each row using the card body flex layout and `margin-top: auto` on `.product-family-card__action-wrapper`. Surplus space is not distributed via `justify-content: space-between`.
   - Verified live before/after card measurements:
     - 1440px Desktop: Row 1 reduced from 626.05px to 526.75px (-99.30px reduction); Row 2 reduced from 626.05px to 553.00px (-73.05px reduction). Both rows exceed the >= 60px height reduction requirement.
     - Row 1: Summary top (1106.20px / 1106.17px / 1106.17px) aligned within 0.0313px (<= 1px requirement); CTA top (1178.70px) aligned within 0.0000px; card heights equal (526.75px).
     - Row 2: Summary top (1656.92px / 1656.95px / 1656.95px) aligned within 0.0313px; CTA top (1755.70px) aligned within 0.0000px; card heights equal (553.00px).
     - NVIS summary top (1656.92px) aligns with Rotator (1656.95px) and Tower (1656.95px) within 0.0313px.
     - 768px Tablet (2-column): Pairs 1 and 2 reduced from 586.20px to 511.73px (-74.47px reduction); Pair 3 reduced from 586.20px to 536.67px (-49.53px reduction). Summary tops and CTA tops aligned within <= 0.0625px across all pairs.
     - 390px Mobile (single-column): All vertical reservations remain `auto`, natural content height preserved with zero horizontal overflow.
5. **Rich Collapsed Product Summaries (C2R1):**
   - Redesigned `<summary class="product-group-summary">` to communicate rich context in collapsed state: product-group heading (`<h3>`), concise short description, configuration count badge, technical document indicator (rendered only when approved documents exist), and a visible "View details" action with disclosure indicator.
   - Zero interactive elements nested within `<summary>`.
   - Expanded body retains models, configurations, approved technical documents, associated visual (if present), and Contact Engineering CTA, while removing duplicate description text.
6. **Configuration Support Placement (C2R1):**
   - Moved Configuration Support panel to the bottom of the family-page content flow: (1) Product catalog heading -> (2) Product disclosures -> (3) Return to All Product Families -> (4) Configuration Support panel -> (5) Shared closing CTA.
7. **Robust Reversible Disclosure Animation (C2R1):**
   - Progressive enhancement script `disclosure.js` starts opening and closing transitions from current rendered height and opacity without jumping on rapid repeated clicks.
   - All inline styles and animations clean up on completion, preserving natural responsive layout and zoom.
   - `prefers-reduced-motion: reduce` toggles natively and immediately without animation. Applies identically to product disclosures and FAQ items.
8. **Approved Card Display Titles:**
   - Card titles explicitly configured: `Log Periodic Antennas`, `Portable & Transportable`, `Aperiodic Loop Antennas`, `NVIS Antennas`, `Rotator & Control Systems`, `Tower Systems & Accessories`.
   - Canonical `Name` property preserved untouched for `<h1>`, breadcrumbs, and internal catalog records.
9. **Source-Supported Rotator & Controller Terminology:**
   - DRC-3 description: `Large industrial antenna-rotator control enclosure with display and control components.` Prohibited claims (tabletop, wall-mounted, cabinet-mounted, mast-mounted, defense systems) purged.
   - DRC-4 description: `Tabletop antenna-rotator controller with digital display, rotary dial, and front controls.` Prohibited claims (rackmount, 19-inch, precision controller) purged.
   - R3500/DRC-4 relationship: `Heavy-duty R3500 rotator and tabletop DRC-4 controller shown together in a source-guided technical visualization.`
10. **Product Group Disclosures & Associated Asset Relocation:**
    - Replaced standalone rotator hardware visual section by moving assets directly into their respective group disclosures (`r3500`, `drc-3`, `drc-4`) via `AssociatedAsset`. The 13 non-rotator groups render no blank media slots or placeholder frames.
    - Specifications on hold (`HoldDisputedSpecs`) withheld from public display; internal governance fields (`ConflictHolds`, `SourceNotes`) never exposed.
11. **Technical Document Publication Allowlist:**
    - Enforced default-deny allowlist policy via `IProductCatalogService.GetApprovedResourcesForGroup(groupId)`.
    - Provisional publication limited to 6 approved records: `doc-lp-high-power`, `doc-lp-1018ba`, `doc-lp-1019`, `doc-1910-2024`, `doc-aperiodic`, `doc-t-3002-oct2016`.
12. **Breakpoint-Sensitive Hero Presentation:**
    - Right-anchored image containment (~65% width on desktop, ~75% on tablet) with left-anchored navy gradient overlay. Zero cropping or white letterboxing across 1920px, 1440px, 1024px, 768px, 767px, and 390px viewports.
13. **Deferred Individual-Product Asset Matrix (C2R1):**
    - Established documented 16-group asset status in `CONTENT_AND_ASSETS.md` isolating the 3 deployed rotator visuals from the 13 groups requiring separate asset research and generation.
14. **Conservative Public Copy for 1942 NVIS Series (C2R2):**
    - Removed unsupported "gap-free" claim from NVIS collapsed short description in `ProductCatalogService.cs`. Applied restrained public wording: "Near Vertical Incidence Skywave (NVIS) HF antenna systems covering 2–30 MHz for short-to-medium-range communications in roof-top, transportable, and ground-mount configurations." Zero operational performance guarantees or unverified claims introduced.
**Reason:** Binding implementation amendments of USAP-CATALOG-001-C2R, USAP-CATALOG-001-C2R1, and USAP-CATALOG-001-C2R2 approved execution plan.

---

## DEC-032 — USAP-CATALOG-002-C1 / C1R1: Corrective Evidence-Safe Product Detail Presentation & Provisional Asset Integration

**Date:** 2026-09-17 (Corrected 2026-09-18)
**Decision:**
1. **Public Presentation Projection Architecture & Clean Governance Separation:**
   - Introduced `ProductCharacteristic` key-value model `(Label, DisplayValue)` to cleanly project technical attributes without leaking raw governance tags.
   - Preserved pure public view models for `ProductModelRecord` and `ProductGroupRecord`. Purged all internal research/governance tokens (`HoldDisputedSpecs`, `ConflictHolds`, `SourceNotes`, `ApprovalStatus`, `PublicationRecommendation`, `SpecificationStatus`, `CommercialAvailability`, `ClientConfirmation`, confidence scores) from the public API and Razor templates.
2. **Product Inventory & Reconciliation (30 Named Identifiers + 1 Unnamed Record = 31 Records):**
   - The public catalog reconciles to 6 families, 16 groups, 30 named models/configurations, and 1 unnamed Aperiodic group record, totaling 31 records:
     - Log Periodic: 8 (`LP-1005`, `LP-1001`, `LP-1002`, `LP-1017`, `LP-1018BA`, `LP-1019BA`, `LP-1019SS`, `LP-1112MR`)
     - Portable & Transportable: 6 (`V-4213AD`, `V-4213AC`, `LP-1402`, `LP-1403`, `1910AA`, `1910BA`)
     - Aperiodic: 0 named models plus 1 unnamed group record
     - NVIS: 6 (`1942-RT`, `1942-TA`, `1942-GM`, `1942-RT-LP`, `1942-TA-LP`, `1942-GM-LP`). All 6 published; the 3 low-power variants are presented with conservative configuration-level role identification only, strictly omitting disputed power, weight, gain, or coverage claims.
     - Rotator & Controller: 5 (`R3500`, `R3501`, `R3503`, `DRC-3`, `DRC-4`)
     - Tower: 5 (`T-3002`, `3002FA`, `3002FB`, `3002SS`, `3002SS-80`)
   - Total: 30 named identifiers + 1 unnamed Aperiodic record.
3. **Intentional Product-Group Visual Coverage Across All 16 Groups:**
   - Deployed intentional visual coverage to all 16 product groups using the 3 provisional rotator visuals plus 13 project-lead-selected A1 candidates from `project-inputs/USAP-CATALOG-002-A1-Final-Review-Package-2026-09-17.zip`.
   - Asset selection is provisional project-lead authorization for implementation, pending formal USAP client approval.
   - Implemented contain-style responsive presentation (`.product-group-disclosure__img--contain`) for wide-footprint groups (`lp-1112mr`, `1910`, `1942`) to prevent misleading tight cropping.
4. **Engineering Guidance Visual Integration:**
   - Implemented 3 coordinated card visuals for the Engineering Guidance pathways on `/products`: Deployment & Mobility (`usap-guidance-card-deployment-mobility-a1-v1.png`), Coverage & Propagation (`usap-guidance-card-coverage-propagation-a1-v1.png`), and Positioning & Infrastructure (`usap-guidance-card-positioning-infrastructure-a1-v1.png`).
   - The combined wide Guidance visual alternate was deliberately deferred.
5. **Products Landing-Page Card Semantic & Visual Refinement:**
   - Converted the product family card outer element to a single semantic anchor (`<a>`).
   - Zero nested interactive controls (`<span class="card-link">` replaces inner link).
   - Removed routine red top borders and navy left borders from ordinary catalog cards.
   - Coordinated hover/focus zoom (`scale(1.03)`) and inline editorial CTA reaction (`Explore Family →`).
6. **Breadcrumb Placement:**
   - Breadcrumbs removed from family-page heroes.
   - Semantic breadcrumbs placed at the top-left of the main content container immediately below the hero.
7. **R3500 and DRC-4 Conservative Relationship Wording:**
   - Conservative wording applied: R3500 and DRC-4 are described based on published references without claiming mandatory pairing, bundled supply, confirmed compatibility, or current commercial availability.
8. **Technical Document Policy (Interim 6 Links vs Planned 17 Migration):**
   - Retained 6 interim product-page resource links (`doc-lp-high-power`, `doc-lp-1018ba`, `doc-lp-1019`, `doc-1910-2024`, `doc-aperiodic`, `doc-t-3002-oct2016`) without treating 6 as a permanent ceiling or classifying R2's 17 canonical PDFs as rejected.
9. **Deterministic Catalog Startup Validation:**
   - Service constructor strictly validates 6 families, 16 groups (5/3/1/1/5/1), 30 named models, 1 unnamed Aperiodic line, all 6 1942 identifiers, 16 group visuals, 6 interim docs, and scans for prohibited governance terms.
**Reason:** Binding execution of finalized 885-line USAP-CATALOG-002-C1 implementation prompt and C1R1 recovery instructions.

---

## DEC-033 — USAP-CATALOG-002-C1R4: Product Overview Disclosure & FAQ Layout Refinement

**Date:** 2026-09-18
**Decision:**
1. **Always-Visible Product-Group Overview Structure:**
   - Re-architected all 16 product-group presentations into an outer `<article class="product-group-card" id="@group.SectionAnchor">` containing an always-visible overview (`.product-group-overview`) and a nested native `<details class="product-group-disclosure">`.
   - Product imagery remains visible at all times, resolving the issue where closing disclosures hid product visuals.
   - Overview displays product image (cover or contain-style for LP-1112MR, 1910, 1942), group heading, short summary, expanded introduction (when providing distinct context), configuration count badge (omitted for Aperiodic), technical document indicator, and up to three key group characteristics.
2. **Accessible Nested Details Disclosure Control:**
   - Disclosure summary bar spans the full width at the base of the overview.
   - Text labels explicitly communicate state: `View Models & Specifications` / `Hide Models & Specifications` (`View Configuration & Technical Details` / `Hide Configuration & Technical Details` for Aperiodic).
   - Augmented with accessible name context (`for @group.Name`) for assistive technology.
   - Nested disclosure content contains additional group characteristics (when > 3), model cards grid, Aperiodic configuration note, approved interim technical documents, and Contact Engineering link.
   - Native progressive enhancement via `disclosure.js` preserved with zero JavaScript dependency.
3. **Product Selection FAQ Layout Alignment:**
   - Expanded `.faq-list` to occupy the full standard `.container` width (`width: 100%`), eliminating awkward narrow-column spacing.
   - Constrained answer copy internally to `max-width: 75ch` on paragraph elements to preserve optimal readability line lengths while disclosure cards span full container width.
4. **Canonical Catalog Inventory Reconciliation:**
   - Mechanically reconciled the exact 30+1 inventory (6 families, 16 groups, 30 named models/configurations, 1 unnamed Aperiodic record = 31 total records).
   - Erroneous temporary identifiers introduced in C1R3 markdown (`lp-1112`, `lp-3001`, `1925`, `1940`, `r3505`, `r3506`, `1942-1..6`, `R3501-1`, `R3503-1`, `T-3002-30..70`) are explicitly rejected and excluded from public content and documentation.
**Reason:** Binding implementation of project lead's product-page presentation refinements in USAP-CATALOG-002-C1R4.

---

## DEC-034 — USAP-CATALOG-002-C1R5: Product Navigation, Configuration Support, Hero Resilience, and Asset Reliability Refinement

**Date:** 2026-09-18
**Decision:**
1. **Family-Page Closing Sequence & Primary Action:**
   - Established strict sequence across all product-family pages: (1) product-group content -> (2) Configuration Support card -> (3) Return to All Product Families navigation -> (4) closing CTA -> (5) footer.
   - Moved `Return to All Product Families` (`.family-back-nav`) below `<aside class="configuration-support-notice">` and above the closing CTA, preserving its secondary outline treatment (`.btn-outline`).
   - Standardized the CTA in Configuration Support to use USAP's primary red brand button (`.btn-primary`) rather than secondary styling, reusing the existing design-system class with zero duplicate button declarations.
2. **Accessible Product-Family Navigation (Desktop & Mobile):**
   - Implemented semantic disclosure dropdown for Products in site header partial (`_Header.cshtml`) using native `<details class="nav-dropdown" id="nav-products-dropdown">` and `<summary class="primary-nav-link nav-dropdown-toggle">`.
   - Menu includes All Products (`/products`) plus all six family routes (`log-periodic-antennas`, `portable-transportable-antennas`, `aperiodic-loop-antennas`, `nvis-antennas`, `antenna-rotator-control-systems`, `tower-systems-accessories`).
   - Fully accessible across mouse, touch, keyboard, and screen readers without hover dependence. Usable without JavaScript.
   - Enhanced via `site-navigation.js` for desktop click-outside dismissal, Escape-key dismissal with focus return to summary, link-click closure, and `aria-expanded` state synchronization.
   - Mobile navigation drawer preserves independent dropdown expansion/collapse without closing the mobile navigation drawer unexpectedly.
   - Direct access to `/products` preserved via first submenu item. Top-level Products footer link retained without duplicate family links.
3. **Hero Resilience and Multi-Scale Responsive Scaling:**
   - Unified hero architecture across `.internal-hero` and family hero variants to ensure height is driven purely by content and vertical padding.
   - Added `box-sizing: border-box`, responsive vertical padding `clamp(var(--space-8), 4vw, var(--space-12))`, `overflow-wrap: break-word`, and fluid title sizing `clamp(1.875rem, 3.25vw + 0.5rem, 2.75rem)` to eliminate text clipping across 80%, 100%, and 125% zoom levels and viewports from 390px to 2560px.
   - Ensured breadcrumbs remain strictly below family-page heroes.
4. **Asset Reliability and Image Verification Architecture:**
   - Confirmed Aperiodic family card image file `usap-family-card-aperiodic-loop-element-v1.png` is committed, intact on disk (2,098,672 bytes), and returns HTTP 200 with MIME type `image/png`.
   - Discovered root cause of prior screenshot defect: legacy screenshot artifact from pre-C1R2 with obsolete alt text `"mounted on a field tripod"` was lingering in review staging, exacerbated by browser lazy loading during headless captures.
   - Established dual-verification requirement in automated audit: every image verified via both HTTP response/content-type and browser DOM completion (`naturalWidth > 0 && naturalHeight > 0`).
**Reason:** Binding requirements of USAP-CATALOG-002-C1R5.

---

## DEC-035 — USAP-CATALOG-002-C1R6: Mobile Navigation Overlay, Family SEO Content, Centered Disclosure, and Characteristic Curation

**Date:** 2026-09-18
**Decision:**
1. **Mobile Navigation Overlay Architecture:**
   - Refactored opened mobile navigation to operate as an overlay (`position: fixed`, `top: var(--mobile-header-bottom, 4.25rem)`, `width: 100vw`, `height: calc(100dvh - var(--mobile-header-bottom, 4.25rem))`, `overscroll-behavior: contain`) positioned immediately below the header.
   - Opening or closing the navigation causes zero document reflow: the visible mobile header, hero, and main content remain at their exact scroll and visual positions without shifting.
   - Background scrolling is locked via `html.nav-open-lock, body.nav-open-lock { overflow: hidden !important; overscroll-behavior: none; }`. The page scroll position is recorded at open and restored at close with zero scroll drift across repeated cycles.
   - Focus is safely contained without dialog role mutation by applying standards-based `inert` to `#main-content`, `.site-footer`, and `.skip-link` while the overlay is open, and clearing only those applied attributes on close or breakpoint resize.
2. **Centered Mobile Products Disclosure Control:**
   - Updated mobile `<summary class="nav-dropdown-toggle">` from `justify-content: space-between` to `justify-content: center; gap: var(--space-2)`.
   - The "Products" label and chevron form a single, unified visual group centered within the control, preserving chevron rotation on open. Submenu links retain indented, left-aligned layout for readability.
3. **Native Disclosure Accessibility Cleanup:**
   - Removed static `aria-haspopup="true"` and `aria-expanded="false"` from the native `<summary>`, and eliminated redundant JavaScript listeners that manually synchronized `aria-expanded`.
   - The native HTML5 `<details>/<summary>` element handles disclosure state and accessibility semantics naturally without JavaScript, eliminating out-of-sync ARIA states.
4. **Product-Family Heading Hierarchy and Evidence-Safe SEO Content:**
   - Replaced generic post-hero copy on all six family pages with family-specific eyebrows, H2 section headings, and factual supporting paragraphs derived strictly from R2 research.
   - Heading hierarchy strictly enforced: page-level `<h1>` (hero family name) -> subordinate `<h2>` (family models/configurations section) -> `<h3>` (product groups) -> `<h4>`/`<h5>` (model/configuration details).
   - Audited and assigned unique page titles, meta descriptions, and canonical terms across all six family routes.
   - Omitted page-level `noindex` so production-intent metadata allows indexing and following by default.
5. **Explicit Overview-Characteristic Curation:**
   - Replaced generic Razor `Take(3)` and `Skip(3)` slicing with explicit curated properties: `OverviewCharacteristics` (0 to 3 group-level facts) and `DetailedCharacteristics` (remaining safe group-level facts) on `ProductGroupRecord`.
   - Each technical fact has exactly one canonical stored definition, eliminating duplicate copies or competing sources. Startup validation mechanically enforces non-overlap, label uniqueness, and the 3-value limit.
6. **Technical Document Status Policy:**
   - Maintained accurate factual status: 17 canonical PDFs identified in R2 for planned local migration, 6 provisionally linked, canonical migration deferred to `USAP-TECHDOC-001`, public vs lead-gated decision client-pending.
**Reason:** Binding requirements of USAP-CATALOG-002-C1R6.

---

## DEC-036 — USAP-CATALOG-002-C1R7: Header Interaction, Mobile Navigation Refinement, Characteristic Audit Resolution, and Products Hero Baseline Investigation

**Date:** 2026-09-18
**Decision:**
1. **Auto-Hiding Sticky Header State Machine:**
   - Standardized the shared site header (`.site-header`) on `position: sticky; top: 0;` with smooth, compositor-only transform-based hide/show transitions (`transform: translate3d(0, -100%, 0)`).
   - Driven by a single unified scroll listener with `requestAnimationFrame` throttling, a directional downward delta threshold (6px), upward restoration delta threshold (4px), and a top-of-page immunity threshold (50px).
   - Implemented an idle restoration timer (~200ms) that smoothly restores the header to view whenever scrolling ceases.
   - Auto-hiding is suspended whenever: (1) the mobile menu is open, (2) the desktop Products dropdown is open, (3) the header or any child element has keyboard/focus within, or (4) skip-link navigation is triggered.
   - Opening the mobile menu while the header is hidden immediately restores the header before opening the overlay.
   - Under `prefers-reduced-motion: reduce`, sliding transitions are completely suppressed (`transition: none !important`), ensuring instantaneous state changes.
2. **Content-Height Mobile Navigation Panel & Translucent Backdrop:**
   - Redesigned the mobile navigation drawer (`#primary-nav-list`) from a forced full-viewport height (`height: calc(100dvh - ...)`) to a content-driven overlay (`height: auto; max-height: calc(100dvh - var(--mobile-header-bottom))`).
   - In collapsed state, the navy panel ends cleanly below the Request a Quote button with intentional bottom padding, eliminating unnecessary navy empty space and showing the page surface beneath.
   - When the Products submenu is expanded or on short-height viewports (e.g. 375×500), the panel expands up to its maximum available height and activates internal momentum scrolling (`overflow-y: auto; overscroll-behavior: contain`).
   - Added a separate, subtle click-away backdrop (`#nav-backdrop`, `background-color: rgba(0, 0, 0, 0.40)`) that covers the remaining viewport beneath the header, visually distinct from the navy panel, dismissing the menu on outside tap.
   - Background scroll lock (`html.nav-open-lock, body.nav-open-lock`) and focus containment via `inert` are strictly preserved with zero scroll-position drift across repeated cycles.
3. **Centered Top-Level Mobile Navigation & Alignment Geometry:**
   - Corrected mobile header layout: the logo is anchored to the left container edge, the hamburger toggle is anchored to the far right container edge (`margin-inline-start: auto`), and `.primary-nav` uses `display: contents` under `js-nav-ready`, utilizing the full available container width.
   - Horizontally centered every top-level navigation item: Home, Products (label + chevron centered as a single unit via `.nav-dropdown-summary-group`), Technical Resources, About Us, Contact Us, and Request a Quote.
   - Submenu child links remain left-aligned and indented (`padding-left: var(--space-10)`) for scanability.
   - Redesigned the active-route indicator as an absolutely positioned left accent bar (`::before { position: absolute; left: 0; width: 3px; }`), completely eliminating asymmetric horizontal borders or padding and ensuring zero horizontal displacement of centered labels (measured difference <= 0.01px).
4. **C1R6 Characteristic Audit Contradiction Resolution:**
   - Investigated the C1R6 audit report contradiction (which reported 0 overview and 0 detailed characteristics despite code claims).
   - Confirmed this was strictly an audit-script selector/extraction defect in C1R6 tooling.
   - Domain models, Razor rendering templates (`Family.cshtml`), and live catalog data (`ProductCatalogService.cs`) are 100% intact and actively serving the approved mapping (3 overview for lp-high-power, lp-1019, v-4213, lp-1402-1403, 1910, aperiodic, 1942; 2 for t-3002; detailed specs inside disclosures; empty groups intentionally empty; zero Razor Take/Skip slicing; zero disputed claims).
5. **Main Products Hero Baseline Investigation & Binding No-Change Compliance:**
   - Completed a comprehensive cascade, breakpoint, and viewport audit of `.internal-hero--products` across 1366×768, 1440×900, 1536×864, 1920×1080, and 2560×1440 viewports.
   - Established that markup, CSS rules, typography, vertical padding (48px), and computed hero height (369.8px) are 100% identical across all desktop viewports. No desktop breakpoint difference exists.
   - Established that the perceived differences between laptop displays and desktop monitors are caused by: (1) mathematical `object-fit: cover` scaling of an 8:3 image inside an increasingly wide container (container aspect ratio grows from 3.69:1 at 1366px to 5.19:1 at 1920px and 6.92:1 at 2560px, forcing top crop to increase from 71px to 175px and 295px); and (2) relative proportion of viewport height (370px occupies 48% of a 768p screen vs 34% of a 1080p screen).
   - Complied with the binding no-change boundary: zero edits made to `/products` hero markup, CSS, image, overlay, padding, or sizing.
**Reason:** Binding requirements of USAP-CATALOG-002-C1R7.

---

## DEC-037 — USAP-CATALOG-002-C1R8: Always-Visible Sticky Header Reconciliation and Focused Navigation QA

**Date:** 2026-09-22
**Decision:**
1. **Always-Visible Sticky Header Pivot:**
   - In accordance with latest explicit project-lead direction, reconciled `.site-header` to remain `position: sticky; top: 0;` and visible at all times.
   - Completely removed the superseded C1R7 scroll-direction auto-hide state machine from `site-navigation.js` and `site.css`:
     - Removed `DELTA_DOWN`, `DELTA_UP`, `TOP_THRESHOLD`, `IDLE_DELAY`, and `lastScrollY`.
     - Removed rAF scroll handler (`handleScrollFrame`), scroll event listener (`onScroll`), idle restoration timer, and `isSuspended()`.
     - Removed `.site-header.is-hidden` CSS rule, `translate3d(0, -100%, 0)` transform, `will-change: transform`, and header transform transition.
     - Removed header `focusin`, skip-link click, and Products-dropdown `toggle` reveal logic whose sole purpose was restoring hidden headers.
     - Removed forced transform/transition overrides from `openNav()` (`header.style.transition = 'none'`, `header.style.transform = 'none'`, reflow trigger) and obsolete overrides from `.nav-open-lock .site-header`.
2. **Preservation of Valid Navigation & Layout Architecture:**
   - Retained stable `offsetHeight` measurement on `.site-header` for computing `--mobile-header-bottom`.
   - Retained content-height mobile navigation drawer (`height: auto; max-height: calc(100dvh - var(--mobile-header-bottom)); overflow-y: auto`).
   - Retained translucent click-away backdrop (`#nav-backdrop`) with outside tap dismissal.
   - Retained background scroll locking (`html.nav-open-lock, body.nav-open-lock`) and HTML5 `inert` background focus containment with zero scroll drift.
   - Retained initial enhanced-menu collapse without scroll restoration, ensuring fragment navigation and browser-restored scroll states are never overwritten.
   - Retained centered top-level mobile navigation links, centered Products label+chevron unit, and left-aligned indented submenu links.
   - Retained native HTML5 `<details>/<summary>` desktop Products disclosure with outside click and Escape dismissal.
3. **Strict Scope and Hero Invariance:**
   - Zero modifications to `/products` landing hero, family heroes, catalog data, characteristics, product assets, or routing.
**Reason:** Latest binding project-lead direction superseding the prior auto-hide header behavior under USAP-CATALOG-002-C1R8.

---

## DEC-038 — USAP-CATALOG-002-C1R9: Shared Internal-Hero Geometry and Full-Image Presentation Reconciliation

**Date:** 2026-09-22
**Decision:**
1. **Shared Desktop/Tablet Banner Baseline Height:**
   - Standardized the internal-page hero foundation (`.internal-hero`) on a shared desktop/tablet minimum-height token: `--internal-hero-min-height: 24.2875rem;` (~388.6px), measured directly from the product-family hero baseline at 1440×900.
   - Enforced exact height parity (388.6px, 0.0px variance at 1440×900 and 1920×1080) across all standard image-backed internal heroes where content fits (`/products`, `/contact-us`, `/request-a-quote`, and all six `/products/{familySlug}` routes).
   - Retained fluid content resilience (`height: auto; min-height: ...;` without max-height constraints). Heroes with longer content (e.g. `/about-us` at 427.1px) expand naturally without clipping under normal viewports, browser zoom (125%, 200%), or accessibility text scaling.
2. **Full-Image Right-Side Media Containment:**
   - Superseded the prior destructive full-width `object-fit: cover` presentation on non-family heroes (`/products`, `/about-us`, `/contact-us`, `/request-a-quote`).
   - Unified `.internal-hero-image` with the established `.internal-hero-picture` family treatment: bounded right-side media region at desktop (`width: 65%; max-width: 1100px; height: 100%; right: 0; left: auto; object-fit: contain; object-position: right center;`) and tablet (`width: 75%;`).
   - Completely eliminates vertical cover cropping, ensuring the full 8:3 source composition is visible without stretching or distortion.
   - Standardized the shared gradient overlay (`linear-gradient(to right, var(--color-brand-navy) 0%, var(--color-brand-navy) 35%, rgba(13, 27, 46, 0.85) 55%, rgba(13, 27, 46, 0.40) 80%, transparent 100%)`), guaranteeing high-contrast solid navy protection under copy while smoothly blending into the contained right-side media.
3. **Responsive Mobile Behavior (< 48rem / 768px):**
   - Maintained content-driven fluid height (`min-height: 22rem; height: auto`) without rigid vertical constraints.
   - Configured non-family media layers as an atmospheric background (`object-fit: cover; object-position: center;`) under a 90% navy overlay (`rgba(13, 27, 46, 0.90)`), guaranteeing white typography readability without horizontal overflow.
   - Documented dedicated 1:1 mobile derivative creation for non-family assets as a future enhancement candidate.
4. **Header Architecture Precision Documentation:**
   - Clarified that the shared header is `position: sticky; top: 0;` during normal page operation, and is temporarily viewport-pinned with `position: fixed; top: 0; left: 0; right: 0; width: 100%;` while `.nav-open-lock` is active to maintain top-of-viewport anchoring while background scrolling is locked.
5. **Strict Scope & Invariance:**
   - Zero changes to homepage hero, homepage video, header sizing, logo assets, product-family source assets, catalog service/records, product facts, technical documents, routes, or backend logic.
**Reason:** Binding requirements of USAP-CATALOG-002-C1R9 to standardize internal hero height and eliminate destructive image cropping.

---

## DEC-039 — USAP-CATALOG-002-C1R10: Final Internal-Hero Height Normalization and Top-Biased Edge-Fade Reconciliation

**Date:** 2026-09-23
**Decision:**
1. **About Us Height Normalization to Shared Baseline:**
   - Normalized `.internal-hero--about` at desktop/tablet widths (`@media (min-width: 48rem)`) to `padding-block: 1.75rem;` (28px top and bottom).
   - Resolves `/about-us` to exactly 388.59px (~388.6px) at normal desktop presentation (1440×900, 1366×768, 1920×1080, 2560×1440), achieving 100% height parity with all other internal image-backed heroes where copy fits.
   - Retained fluid content resilience (`height: auto; min-height: var(--internal-hero-min-height, 24.2875rem);`). Preserved full content width (704px), lead max-width (608px), typography, line heights, button dimensions, and copy without forced overflow clipping.
   - Vertically centered content layout provides symmetrical, comfortable breathing room (~28.8px top space above eyebrow and ~29.8px bottom space below CTA).
2. **Top-Biased Image-Edge Vertical Fade (Candidate D Selection):**
   - Implemented the project-lead/Architect-selected Candidate D visual treatment across all image-backed internal heroes (`.internal-hero-overlay, .internal-hero-overlay--family`) at desktop (`>= 64rem`) and tablet (`48rem - 63.999rem`).
   - Combined multi-background gradient layers a vertical top-biased dissolve over the existing horizontal text-protection gradient:
     `linear-gradient(to bottom, var(--color-brand-navy) 0%, rgba(13, 27, 46, 0.75) 5%, transparent 18%, transparent 88%, rgba(13, 27, 46, 0.45) 96%, var(--color-brand-navy) 100%)`.
   - Refined 5% top navy band dissolving by 18% eliminates the abrupt color cutoff between bright sky and surrounding navy without unnecessarily darkening upper sky or antenna mast structures.
   - Lighter bottom dissolve (transparent until 88%, 45% navy at 96%) preserves lower equipment and terrain detail while softening the lower pixel cutoff.
   - Retains 100% untouched source color and contrast across the central 70% vertical image region (from 18% to 88%).
3. **Preservation of Contained Right-Side Media Architecture:**
   - Retained C1R9 non-cropping architecture: `object-fit: contain; object-position: right center;` on right-side bounded media (`width: 65%; max-width: 1100px;` desktop, `width: 75%;` tablet).
   - Zero image cropping, stretching, distortion, or alteration of source media files.
4. **Mobile & Asset Scope Boundary:**
   - Mobile (< 48rem) retains content-driven fluid height, atmospheric cover imagery, and 90% navy overlay with zero horizontal overflow. Dedicated 1:1 mobile derivative generation for non-family pages remains explicitly deferred to future asset work.
   - Technical Resources hero implementation remains deferred.
**Reason:** Production implementation of project-lead/Architect-selected internal-hero height normalization and refined Candidate D top-fade treatment under USAP-CATALOG-002-C1R10/R2.

---

## DEC-040 — USAP-CATALOG-002-C1R11: Right-Anchored Ultra-Wide Hero Composition Reconciliation

**Date:** 2026-09-23
**Decision:**
1. **Ultra-Wide Breakpoint Selection (`@media (min-width: 100rem)`):**
   - Established 100rem (~1600px) as the controlling ultra-wide internal-hero breakpoint, mathematically justified by the crossover threshold where contained 8:3 source imagery reaches maximum width in a 65vw track ($1033.58 / 0.65 \approx 1590.13\text{px}$).
   - Below 100rem, the accepted C1R10-R2 desktop, tablet, and mobile hero geometry remains 100% unchanged.
2. **Right-Pinned Media with Moderate Responsive Hero Growth (Candidate B Selection):**
   - Following physical desktop monitor review by the project lead, the initial centered 100rem canvas and right navy gutter were rejected as unsuited for production.
   - Selected Candidate B: right-pinned media (`right: 0`) paired with moderate, capped responsive growth:
     - `.internal-hero`: `min-height: clamp(24.2875rem, 22vw, 28rem);`.
     - `.internal-hero-picture, .internal-hero-image`: `right: 0; width: clamp(65rem, 59vw, 75rem); max-width: none;`.
     - `.internal-hero-picture .internal-hero-img, .internal-hero-image`: `object-fit: contain; object-position: right center;`.
   - The contained 8:3 image terminates cleanly at the physical browser right edge (`visibleImageRight == viewportWidth`) with zero right-side navy gutter.
   - Moderate height growth (~422px at 1920px, capped at 448px at 2560px) allows the full 8:3 image to scale up and extend significantly farther left into the viewport (~796px at 1920px, ~1368px at 2560px; +90.1px and +158.4px farther left than Candidate A), bridging the empty center chasm while maintaining an elegant, non-dominant hero height.
3. **Relaxed Ultra-Wide Horizontal Copy-Protection Overlay (C1R11 / C1R12 Refinement):**
   - Replaced the rejected right-edge gutter dissolve with the canvas-anchored relaxed horizontal gradient:
     `linear-gradient(to right, var(--color-brand-navy) 0, var(--color-brand-navy) calc(max(0px, (100vw - 100rem) / 2) + 30rem), rgba(13, 27, 46, 0.88) calc(max(0px, (100vw - 100rem) / 2) + 40rem), rgba(13, 27, 46, 0.58) calc(max(0px, (100vw - 100rem) / 2) + 50rem), rgba(13, 27, 46, 0.24) calc(max(0px, (100vw - 100rem) / 2) + 62rem), transparent calc(max(0px, (100vw - 100rem) / 2) + 72rem))`
   - Under C1R12, finalized the transition to Candidate H1: solid navy extends 6rem farther right (to `gutter + 30rem`), maintaining 88% opacity at `+40rem` (where image enters at x=796px at 1920px), completely eliminating the perceptible vertical tonal seam under headings on wide monitors while ensuring antenna subjects past `+72rem` remain clear and unshaded.
   - Layered under the accepted Candidate D vertical top/bottom fade (`linear-gradient(to bottom, navy 0%, 75% navy 5%, transparent 18%, transparent 88%, 45% navy 96%, navy 100%)`).
   - Guarantees 100% solid contrast behind typography (max-width 44rem) while allowing graphic details to emerge smoothly without tonal seams or artificial dark bands. Zero right-to-left gutter fade.
4. **Uniform Cross-Route Architecture & Invariance:**
   - Applied uniformly across all 10 image-backed internal heroes (`/products`, `/about-us`, `/contact-us`, `/request-a-quote`, and all 6 product-family routes).
   - Preserves complete 8:3 source imagery containment with zero cropping, distortion, or source asset modification.
   - Homepage hero remains separate and unmodified. Technical Resources hero will inherit this shared architecture upon eventual implementation.
**Reason:** Production implementation of project-lead-selected Candidate B right-anchored ultra-wide hero composition under USAP-CATALOG-002-C1R11/R2.

---

## DEC-041 — USAP-CATALOG-002-C1R13-R1: Rotator & Control Systems Hero Route-Specific Asset-Entry Mask

**Date:** 2026-09-24
**Decision:**
1. **Asset-Driven Route-Specific Exception (Candidate R2 Selection):**
   - Implemented a route-specific CSS image mask on `.internal-hero--family-antenna-rotator-control-systems .internal-hero-picture` under `@media (min-width: 100rem)`:
     `-webkit-mask-image` / `mask-image`: `linear-gradient(to right, transparent 0, rgba(0, 0, 0, 0.55) 4rem, rgba(0, 0, 0, 0.85) 8rem, #000 12rem)`.
   - Fades the source image's alpha channel from 0% to 100% over the left 12rem (192px) of the media region, dissolving the sharp rectangular left entrance of the technical blueprint asset into the underlying `#0d1b2e` hero canvas without any contrast step.
2. **Preservation of Shared Architecture & Invariance:**
   - Global shared hero architecture remains 100% unchanged across the other nine internal heroes.
   - Candidate D vertical top/bottom fade remains controlling.
   - Candidate H1 horizontal copy-protection blend remains controlling.
   - Ultra-wide geometry (height clamp, width clamp, right pinning, zero right gutter, `object-fit: contain`) remains 100% unchanged.
   - No behavior changes below 100rem (verified at 1440×900 and 1366×768 where image enters under solid navy).
   - Mobile and tablet responsive cover behavior remains 100% untouched.
3. **No Razor Markup Modifications:**
   - Isolated cleanly using the pre-existing semantic family class rendered by `Family.cshtml` (`.internal-hero--family-antenna-rotator-control-systems`).
4. **Deferred Ultra-Wide Copy Layout Enhancement:**
   - Retained project-lead observation that internal hero copy on larger/wider monitors should later receive additional horizontal space; deferred to the future visual-enhancement workstream.
**Reason:** Production implementation of project-lead/Architect-selected Candidate R2 mask following the USAP-CATALOG-002-C1R13 prototype study to eliminate the Rotator hero asset's hard entrance seam.

---

## DEC-042 — USAP-TECHDOC-001: Technical Resources Library and Canonical PDF Migration

**Date:** 2026-09-24
**Decision:**
1. **Canonical Technical Document Migration:**
   - Migrated exactly 17 canonical first-party technical PDF binaries from the verified R2 source package into the application under `src/USAP.Web/wwwroot/documents/technical/{family-slug}/{filename}`.
   - All 17 files verified byte-for-byte and SHA-256 identical to the R2 manifest. No binary modification or superseded duplicates deployed.
2. **Direct Public Access Model:**
   - Adopted direct public access for all technical documentation as directed by the project lead. Visitors can directly view and download canonical PDFs without a lead capture gate.
3. **Application Document Architecture:**
   - Extended `ProductResourceRecord` to represent full canonical technical document metadata (`Id`, `Title`, `LocalPdfPath`, `FamilySlug`, `FamilyName`, `ProductGroupIds`, `ModelCodes`, `DocumentType`, `Description`, `PageCount`, `SortOrder`, `LegacySourceUrls`, `SearchText`).
   - Maintained backwards-compatible accessors (`ProductGroupId`, `ResourceType`, `CurrentSourceUrl`) for existing product views.
   - Updated `IProductCatalogService` and `ProductCatalogService` to register all 17 canonical documents and provide retrieval by group (`GetApprovedResourcesForGroup`) and family (`GetTechnicalDocumentsByFamily`), plus full library retrieval (`GetAllTechnicalDocuments`).
4. **Product Catalog Document Reconciliation:**
   - Reconciled all 16 product groups to reference local canonical documents, eliminating all external legacy WordPress PDF links.
   - Supported multi-document association on product groups where R2 established multiple canonical documents (`v-4213` associates both `DOC-V4213-OVERVIEW` and `DOC-V4213-CONFIG`). All other 15 groups associate their singular canonical document.
   - Preserved all factual constraints and prohibited token rules.
5. **Permanent Legacy Document Redirects (HTTP 301):**
   - Implemented `LegacyDocumentRedirectMiddleware` in ASP.NET Core request pipeline to handle all 34 known legacy `/wp-content/uploads/...pdf` URLs.
   - Mapped each legacy URL directly to its canonical destination (`/technical-resources/documents/{family-slug}/{filename}`), preserving query strings.
   - Configured static file serving at `/technical-resources/documents` mapped to disk path `wwwroot/documents/technical`, ensuring both `/technical-resources/documents/...` and `/documents/technical/...` return HTTP 200 OK.
6. **Technical Resources Library UI & Progressive Enhancement:**
   - Built a semantic, accessible resource library on `/technical-resources` with keyword search, category filter pills (`All` + 6 canonical product families with live counts), responsive cards, empty state, and filter reset.
   - Guaranteed 100% progressive enhancement: library is completely visible and filterable without JavaScript, with instant client-side enhancement when JavaScript is enabled.

---

## DEC-043 — USAP-TECHDOC-001-R1: Document Detail Experience, Canonical PDF Canonicalization, Homepage Deep-Link Taxonomy, and Hero Integration

**Date:** 2026-09-25
**Decision:**
1. **Branded HTML Document-Detail Route Architecture:**
   - Implemented a reusable, data-driven Razor Page (`/technical-resources/document/{slug}`) mapping all 17 canonical documents via `GetTechnicalDocumentBySlug(slug)`.
   - Displays breadcrumb navigation, H1 document title, product family link, covered model badges, format/page metadata, direct PDF download and open actions, and related product group links.
   - Incorporates native browser PDF viewing using a semantic `<object data="..." type="application/pdf">` / `<iframe>` container with clean fallback for unsupported devices.
2. **Technical Resources Library Card & Product Action Alignment:**
   - Changed library card primary viewing action from opening raw PDF directly to the branded HTML detail page (`View Document`).
   - Retained explicit `Download PDF` action pointing directly to the local canonical PDF binary.
   - Aligned product family group disclosure links to navigate to the branded detail page while offering direct PDF downloads.
3. **Canonical Raw-PDF URL Reconciliation:**
   - Established `/technical-resources/documents/{family-slug}/{filename}` as the singular preferred public raw-PDF destination.
   - Configured `LegacyDocumentRedirectMiddleware` to issue HTTP 301 permanent redirects from alternate `/documents/technical/...` paths to `/technical-resources/documents/...` to eliminate dual indexable URLs for identical binaries.
4. **PDF Byte Preservation & Metadata Policy:**
   - Reaffirmed strict policy: PDF binaries must remain byte-for-byte identical to the verified first-party R2 source package.
   - Prohibited editing internal PDF binary metadata to change legacy/imperfect embedded document titles (e.g. Word cut sheet titles).
   - Solved visitor-facing and SEO title clarity purely through branded HTML document detail pages, descriptive HTML `<title>` tags, clean public filenames, and structured catalog metadata.
5. **Homepage Deep-Link Taxonomy & Preset Filter UX:**
   - Reconciled the four homepage Technical Resources preview cards against structured metadata:
     - `Product Data Sheets`: links to `/technical-resources?type=data-sheet`, dynamically filtering to documents where `DocumentType == "Data sheet"`.
     - `Antenna Systems`: links to `/technical-resources?view=antenna-systems`, dynamically filtering to the 4 canonical antenna families (`log-periodic-antennas`, `portable-transportable-antennas`, `aperiodic-loop-antennas`, `nvis-antennas`).
     - `Rotators & Controls`: links to `/technical-resources?category=antenna-rotator-control-systems`.
     - `Tower Systems & Accessories`: links to `/technical-resources?category=tower-systems-accessories`.
     - `Browse Technical Resources`: links to `/technical-resources` with all 17 documents visible.
   - Implemented an active filter banner (`Filtered by: [Label] [Clear]`) that visually communicates active presets and provides an instant clear action.
6. **Search Toolbar Desktop Containment:**
   - Refined the search toolbar to form a contained, left-aligned group on desktop (max-width ~52rem) with a clear visual gap between the search input and visible Search submit button.
7. **Hero Artwork & Closing CTA Integration:**
   - Integrated existing hero graphic `usap-technical-resources-hero-requirements-document-candidate-b-v1.png` into `/technical-resources` using the shared `.internal-hero` architecture. Verified readable contrast and responsive behavior; recorded mobile derivative as a deferred visual enhancement item.
   - Added standard USAP closing CTA (`.closing-cta.closing-cta--default`) before footer on `/technical-resources` and document detail views directing visitors to Contact Us and Request a Quote.
8. **Document Detail SEO Foundation:**
   - Provided unique `<title>`, unique meta description, canonical URL, and OpenGraph metadata for all 17 document detail routes based strictly on approved R2 safe summary content. Raw PDF text extraction is not dumped automatically to prevent publishing status-sensitive historical details.

---

## DEC-044 — USAP-TECHDOC-001-R2: Technical Resources UI Cleanup, Card Footer Restoration, Breadcrumb Relocation, and Baseline Stabilization

**Date:** 2026-09-25
**Decision:**
1. **Desktop Search Toolbar Right Alignment:**
   - Refined desktop and wide-desktop toolbar layout so the contained search group (`max-width: 32rem; width: 100%`) aligns to the right side of the main content container (`margin-left: auto; margin-right: 0`), balancing cleanly against the left-aligned product family filter pills.
   - Retained responsive full-width behavior on tablet and mobile viewports (`max-width: 100%; margin-left: 0`) with zero horizontal overflow.
2. **Technical Document Card Footer & Atomic Page Count Restoration:**
   - Restored the visual organization of the earlier card footer: `[document icon] 2 pages    [View Document] [Download PDF]`.
   - Guaranteed that page indicators (`1 page`, `2 pages`, etc.) remain strictly on one line as an atomic flex item (`white-space: nowrap`), preventing unwanted line wrapping between numbers and the word "page/pages".
   - Compacted action button proportions (`View Document` and `Download PDF`) so footers fit on a single row in standard 3-column desktop grid configurations.
3. **Card-Title and UI Link State Protection:**
   - Explicitly styled `:link`, `:visited`, `:hover`, `:focus-visible`, and `:active` states on card title links, document property metadata links, breadcrumbs, viewer notes, and related group cards.
   - Normal and visited states strictly preserve intended dark navy / branded link colors, completely preventing browser-default purple link colors from leaking into the UI.
4. **Document-Detail Breadcrumb Architecture Repair:**
   - Relocated breadcrumbs on `/technical-resources/document/{slug}` to appear immediately below the hero, adhering to the approved breadcrumb hierarchy and styling from Product Family pages.
   - Reused shared `.breadcrumbs` classes, eliminating browser-default numbered list markers.
5. **Detail Hero Action Streamlining:**
   - Removed the redundant and low-contrast `Back to Resource Library` button from the document-detail hero; return navigation is now handled cleanly by the relocated breadcrumbs.
   - Preserved clear primary `Download PDF` and secondary `Open PDF in New Tab` actions in the hero, while streamlining the embedded viewer header to remove adjacent button duplication.
6. **Closing CTA Content Alignment:**
   - Replaced custom guidance copy with the approved, controlled USAP Closing Inquiry CTA across both `/technical-resources` and all `/technical-resources/document/{slug}` routes:
     - Heading: `Ready to Discuss Your Antenna Requirements?`
     - Copy: `Share your application requirements, product questions, or system needs. USAP can help you evaluate available antenna and related equipment options.`
     - Primary Action: `Request a Quote` (`/request-a-quote`).
     - Secondary Action: `Request Information` (`/contact-us`).
7. **Preset Filter UI Simplification:**
   - Suppressed redundant "Reset filters" link when active preset filter banner (`Filtered by: [Label] Clear filter`) is showing, providing a single clear status indicator and reset action.
8. **Explicitly Deferred Post-Baseline Items:**
   - Evaluated embedded PDF title anomalies (e.g. Word cut sheet titles) and reaffirmed byte-for-byte binary preservation for this baseline; deferred evaluation of metadata-normalized publication derivatives or a controlled/custom viewer after baseline commit.
   - Explicitly deferred image-backed hero exploration on document-detail pages until after baseline stabilization.

---

## DEC-045 — USAP-FORMS-002: CTA Context Routing, Two-Form Architecture, Dedicated Success Flows, and Integration Configuration Foundation

**Date:** 2026-09-29
**Decision:**
1. **Preserve Exactly Two Dedicated Inquiry Forms:**
   - Retained `/contact-us` (for informational, technical, engineering, and general inquiries) and `/request-a-quote` (for commercial pricing, quantity, and requirements). Both reuse `_InquiryForm.cshtml` partial.
2. **Context Propagation and Canonical Separation:**
   - Implemented `ICtaContextResolver` (`CtaContextResolver`) to deterministically resolve incoming query strings (`reason`, `family`, `group`, `product`, `doc`) against canonical domain models via `IProductCatalogService`.
   - Strictly separated trusted canonical source context (`SourceContextCategory`, `SourceContextTitle`, `SourceContextSummary`) from visitor-editable form inputs (`ProductOfInterest`).
   - Implemented visible, accessible `.inquiry-context-panel` on forms to reassure visitors without masquerading as an editable input.
3. **Canonical 5-Intent Model & Safe Engineering Support Wording:**
   - Canonical intents: `GeneralInquiry`, `ProductInformation`, `RequestAQuote`, `EngineeringSupport`, `TechnicalDocumentation`.
   - `EngineeringSupport` display name standardized to `Engineering & Requirements Support` with safe boundaries (application requirements, equipment options, suitability, mounting), explicitly omitting claims of custom mechanical/RF engineering services, custom manufacturing, or guaranteed integration.
4. **Form Field Validation & Progressive Enhancement:**
   - `Organization` is strictly REQUIRED for `/request-a-quote` (both server-side and client indicator) and remains OPTIONAL for `/contact-us`.
   - `Phone` remains conditional (required only when Phone preferred).
   - Form remains 100% usable without JavaScript.
5. **Accessible Offscreen Honeypot:**
   - Replaced contradictory `.sr-only` class with `.form-honeypot` (offscreen absolute positioning, zero opacity, `pointer-events: none`, `tabindex="-1"`, `aria-hidden="true"`). Silent diversion on submission preserved.
6. **Dedicated Success Flows:**
   - Created dedicated Razor Pages `/contact-us/thank-you` and `/request-a-quote/thank-you` with `noindex, nofollow` and sitemap exclusion.
   - Preserved POST-Redirect-GET and reference-number display via `TempData`. Direct navigation renders clean guidance state without triggering conversion events.
   - Legacy `/thank-you` redirects safely to `/contact-us/thank-you`.
7. **Integration Configuration Foundations & Zero-Secrets Rule:**
   - Added options classes: `SmtpOptions`, `RecaptchaOptions`, `AnalyticsOptions`.
   - Superseded legacy reCAPTCHA v2 Checkbox recommendation in favor of score-based Google Cloud reCAPTCHA website integration.
   - Superseded GTM in favor of Direct GA4 (`gtag.js`).
   - Binding rule: No credentials in `appsettings*.json`. Secrets managed via .NET User Secrets in Dev and IIS Environment Variables in Production.

---

## DEC-046 — USAP-FORMS-002-R1: Thank-You Visual Reconciliation, Identifier-Based Context Revalidation, and Claim-Safety Reconciliation

**Date:** 2026-09-29
**Decision:**
1. **Identifier-Based Context Persistence & Server Revalidation:**
   - Client form carries only minimal, untrusted context identifiers (`ContextReason`, `ContextFamily`, `ContextGroup`, `ContextDoc`).
   - Client-posted display strings (`SourceContextCategory`, `SourceContextTitle`, `SourceContextSummary`) are explicitly treated as untrusted and not posted as hidden fields.
   - On every POST, `ICtaContextResolver` re-resolves and validates submitted identifiers against canonical catalog records before redisplaying context or composing notification messages.
2. **Product Query Parameter Trust Boundary:**
   - Removed `product` query parameter handling from `ICtaContextResolver`. Untrusted query strings cannot establish trusted canonical model context.
   - Arbitrary visitor product/model entries are accommodated exclusively through the existing visitor-editable `Product / Model of Interest` form control.
3. **404-Aligned Thank-You Visual Architecture:**
   - Replaced custom cards on dedicated thank-you pages with a centered utility page composition (`confirmation.css`) visually aligned with `/not-found`.
   - Reused the light-surface technical signal overlay with restrained visual intensity (0.09 opacity, ~half of the 404 page's 0.18 opacity).
   - Standardized green reference card, paired CTA buttons (`Return to Homepage`, `Explore Products`), and a 404-matching `Helpful Links` section with arrow hover micro-animations.
   - Removed all inline presentation styles from Razor views.
4. **Context-Aware Helpful Links:**
   - Helpful Links on Thank-You pages are dynamically derived from server-revalidated canonical context passed via `TempData` identifiers (up to 3 links, e.g. Return to Document, Related Product Family, Browse Technical Resources, Request a Quote). Direct navigation renders safe fallback links.
5. **Claim-Safety & Response Time Copy Reconciliation:**
   - Eliminated all unsupported response-time and turnaround promises (e.g. 1–2 business days, fixed turnaround windows).
   - Corrected product-family closing section copy from "custom engineering requirements" to "application and system requirements".
6. **Program.cs Formatter Verification:**
   - Confirmed multiline brace formatting in `Program.cs` is strictly required by the active solution analyzer rule `IDE0011: Add braces to 'if' statement`. Reversion to single-line causes `dotnet format --verify-no-changes` to fail with 19 warnings. Retained to preserve clean static verification.

---

## DEC-047 — USAP-FORMS-002-R2: 404 Background Treatment Parity, Mobile QA Repair, Canonical Helpful-Link Routing, and Visitor Confirmation Email Decision Recording

**Date:** 2026-09-29
**Decision:**
1. **Exact 404 Background Treatment Parity:**
   - Superseded the R1 reduced-opacity (0.09) decision. The confirmation page wrapper intentionally reuses the exact background texture treatment from `/not-found`:
     - Exact asset: `usap-light-surface-technical-signal-overlay-v1.png`.
     - Exact opacity: `0.18`.
     - Exact background sizing: `cover`.
     - Exact background positioning: `center`.
     - Exact repeat behavior: `no-repeat`.
     - Exact wrapper behavior: `min-height: clamp(34rem, 54vh, 44rem)` with `isolation: isolate;`.
     - Compacted vertical padding in `.confirmation-container` (`var(--space-8)` desktop) so active confirmation wrapper height (~608px) closely matches 404 wrapper height (594px), eliminating texture distortion, over-cropping, and asset magnification.
   - `/not-found` (`error.css`) remains visually untouched and pixel-equivalent.
2. **Canonical Technical Document Helpful Link Routing:**
   - Corrected Thank-You page Helpful Link generation from `/technical-resources/{slug}` to the canonical branded HTML route `/technical-resources/document/{slug}` across both `ContactUs/ThankYou.cshtml.cs` and `RequestAQuote/ThankYou.cshtml.cs`.
   - Confirmed Product Group Helpful Links point to `/products/{family}#{group.SectionAnchor}`, matching stable article element IDs on Family pages.
3. **Mobile QA Repair & Live Browser Testing:**
   - Identified root cause of R1 screenshot discrepancy: static HTML files saved from responses and opened without the live web server failed to load stylesheets and `site-navigation.js`, preventing the progressive `.js-nav-ready` class from attaching. This caused the desktop navigation `<ul>` to expand the document body to ~800px width inside a 390px window.
   - Refined mobile confirmation CSS (`confirmation.css`):
     - Fluid monospace sizing `clamp(1rem, 4.5vw, 1.25rem)` on `.confirmation-reference-code`.
     - Controlled mobile card padding (`var(--space-3) var(--space-4)`) and `overflow-wrap: anywhere; word-break: normal;` allowing `REQ-20260929-XXXXXX` (19 chars) to render cleanly on a single line at 320px.
     - Mobile container padding reduced to `var(--space-4)` at ≤30rem and `var(--space-3)` at ≤22.5rem.
   - Verified live browser POST-Redirect-GET flow at 1440px, 1024px, 768px, 390px, 375px, 360px, and 320px with zero horizontal overflow (`scrollWidth <= innerWidth`).
4. **Visitor Confirmation Email Architecture (Planned for FORMS-003):**
   - Superseded prior planning decision to omit visitor confirmation emails. Legitimate accepted submissions will receive an automated confirmation email containing the same reference number shown on the Thank-You page.
   - Implementation deferred to FORMS-003 alongside internal SMTP delivery.
   - Thank-You page copy in R2 remains strictly truthful (no claim of sent email until delivery exists).
   - Planned FORMS-003 submission contract distinguishes `InquiryAccepted`, `InternalNotificationSent`, and `VisitorConfirmationSent`.
   - Conditional UI display: Thank-You page displays confirmation email notice only when visitor delivery succeeds; displays fallback guidance ("keep this reference number for your records") if visitor delivery fails, ensuring inquiries are never lost due to visitor SMTP issues.
   - Abuse protection: Operates strictly behind antiforgery, honeypot, rate limiting, and future score-based reCAPTCHA. Visitor has zero control over sender, headers, subject structure, body content, or arbitrary routing.

---

## DEC-048 — USAP-FORMS-002-R3: Contact/Quote Workflow Boundary Enforcement, Honeypot Silent-Success Hardening, and Explicit Submission-State Tracking

**Date:** 2026-09-29
**Decision:**
1. **Binding Contact vs. Quote Workflow Boundary:**
   - `/contact-us` permits only 4 inquiry types: `GeneralInquiry`, `ProductInformation`, `EngineeringSupport`, `TechnicalDocumentation`.
   - In `_InquiryForm.cshtml`, the inquiry-type `<select>` explicitly filters out `RequestAQuote`.
   - Quote Details fieldset (`EstimatedQuantity`, `DesiredTimeline`, `IntendedApplication`) is wrapped in `@if (Model.Type == InquiryType.RequestAQuote)`, omitting quote inputs entirely from `/contact-us` HTML (both JS and No-JS safe).
   - On GET `/contact-us?reason=request-a-quote`, issues an immediate 302 redirect to `/request-a-quote`, preserving only valid trusted canonical context identifiers (`family`, `group`, `doc`). Arbitrary query parameters and display strings are discarded.
   - On POST `/contact-us`, forged submissions attempting `Input.Type == RequestAQuote` are strictly rejected via ModelState error ("Please use the Request a Quote form for quote requests."), never dispatched to `IInquirySubmissionService`, and never converted to `GeneralInquiry`.
   - `/request-a-quote` remains locked server-side to `RequestAQuote` with `Organization/Company` required. Forged POSTs attempting non-quote types are overridden to `RequestAQuote` and validated under quote rules.
2. **Honeypot Silent-Success Hardening:**
   - When honeypot (`Input.Website`) is populated, submission service is bypassed (0 backend calls, 0 emails dispatched, 0 PII stored in `TempData`).
   - A synthetic reference code (`REQ-yyyyMMdd-XXXXXX`) is generated matching the canonical public reference format.
   - Redirects via PRG 302 to the dedicated Thank-You page, rendering the standard confirmation layout, reference card, and canonical helpful links so bots cannot detect rejection.
3. **Explicit Submission-State Architecture:**
   - Introduced explicit server-controlled state tracking in `TempData`:
     - `IsGenuineSubmission`: `true` for genuine submissions, `false` for honeypot diversions, `null` for direct navigation.
     - `SubmissionDisplayState`: `"GenuineSuccess"` vs `"HoneypotDiversion"` vs `null`.
     - `HasDisplaySuccess`: `true` when a reference number is present.
     - `IsGenuine`: `true` only when `IsGenuineSubmission == true`.
   - Direct navigation to `/contact-us/thank-you` or `/request-a-quote/thank-you` renders the neutral status view (`Inquiry Status`, `Quote Request Status`) without a reference card or conversion eligibility.
4. **Future Analytics & SMTP Safety Gates:**
   - Binding requirement for future GA4 integration: `generate_lead` and conversion tracking must gate strictly on `IsGenuineSubmission == true`. Honeypot diversions must NEVER fire conversion events.
   - Binding requirement for FORMS-003: internal notification emails and visitor confirmation emails are dispatched strictly when `IsGenuineSubmission == true`. Zero emails are sent for honeypot diversions.

---

## DEC-049 — USAP-FORMS-002-R4: Route-Authoritative Workflow Presentation, Canonical Context Precedence, and Final Checkpoint Authorization

**Date:** 2026-09-29
**Decision:**
1. **Route-Authoritative Workflow Presentation via `IsQuoteWorkflow`:**
   - Decoupled `_InquiryForm.cshtml` form structure from visitor-bound `Input.Type`. Introduced server-controlled property `IsQuoteWorkflow` on `InquiryPageModelBase` (`false` by default on Contact, overridden to `true` on `RequestAQuoteModel`).
   - Structural form controls evaluate `isQuoteWorkflow`:
     - Organization required indicator (`*`) and `aria-required="true"` render only on Quote workflow.
     - Inquiry Type renders locked static badge on Quote workflow; renders filtered 4-option dropdown on Contact workflow.
     - Quote Details fieldset (`EstimatedQuantity`, `DesiredTimeline`, `IntendedApplication`) renders only when `isQuoteWorkflow == true`.
   - On forged Contact POST (`Input.Type=RequestAQuote` or undefined type), the server rejects submission, normalizes `Input.Type` to a safe Contact type, clears the attacker's attempted value from ModelState (`ModelState.Remove`), and adds the user-safe validation error. The redisplayed page remains visually and structurally a Contact form.
2. **Canonical Context Precedence (`document > group > family`):**
   - Implemented strict hierarchical precedence in `CtaContextResolver`:
     1. Valid Technical Document (`doc`): Authoritative for document metadata and derived canonical family (`docRecord.FamilySlug`). Conflicting `group` and `family` parameters are ignored.
     2. Valid Product Group (`group`): Authoritative for group metadata and derived canonical family (`groupRecord.FamilyId`). Conflicting `family` parameters are ignored.
     3. Valid Product Family (`family`): Authoritative only when neither a valid document nor group has established context.
     4. Invalid Fallback: Invalid higher-priority parameters fall through to valid lower-priority parameters (e.g. invalid doc + valid group -> group context; invalid doc/group + valid family -> family context; all invalid -> generic context).
   - Explicit GET redirect `/contact-us?reason=request-a-quote` forwards only the minimum highest-priority canonical identifier (`doc`, `group`, or `family`).
3. **Confirmation Background Asset Path & Options Property Corrections:**
   - Corrected documentation and evidence to record the actual asset location `wwwroot/images/shared/backgrounds/usap-light-surface-technical-signal-overlay-v1.png` (CSS relative `../images/shared/backgrounds/...`).
   - Reconciled review package documentation with actual source options properties (`AnalyticsOptions`, `RecaptchaOptions`, `SmtpOptions`).
4. **Final Checkpoint Authorization:**
   - Authorized final local Git checkpoint commit `feat(forms): implement contextual inquiry and quote workflows` upon passing full build, formatting, and route verification gates. Push and merge remain strictly deferred.

---

## DEC-050 — USAP-FORMS-003-C1: MailKit SMTP Transport, Internal Notification, Visitor Confirmation Email, and Delivery-Aware Success State

**Date:** 2026-09-30
**Decision:**
1. **MailKit / MimeKit Provider-Neutral SMTP Transport:**
   - Added MailKit 4.18.1 (`net10.0`) package dependency, adhering to repository package conventions.
   - Introduced `ISmtpEmailSender` abstraction and `MailKitSmtpEmailSender` implementation utilizing `MailKit.Net.Smtp.SmtpClient`.
   - Strict TLS Certificate Validation: Certificate validation is never bypassed, disabled, or weakened (`CheckCertificateRevocation = true`).
   - Maps `SecurityMode` safely via `SmtpOptions.ResolveSecureSocketOptions()` (`StartTls` for IONOS dev port 587, `SslOnConnect` for port 465, `StartTlsWhenAvailable`, `Auto`, `None`).
   - Sockets and sessions are disconnected and disposed cleanly using asynchronous APIs (`ConnectAsync`, `AuthenticateAsync`, `SendAsync`, `DisconnectAsync`).
2. **Configuration Validation & Zero Secrets Rule:**
   - Configuration contracts: `Smtp:Enabled`, `Smtp:Host`, `Smtp:Port`, `Smtp:SecurityMode`, `Smtp:Username`, `Smtp:Password`, `Smtp:FromAddress`, `Smtp:FromName`, `Smtp:NotificationRecipient`.
   - Zero secrets stored in source control; dev secrets managed via .NET User Secrets; stage/prod via IIS Environment Variables.
   - Robust startup validation via `SmtpOptions.IsValid(out string? error)` ensures valid Host, Port, Username, Password, FromAddress, and NotificationRecipient when `Smtp:Enabled == true`. Application starts cleanly when SMTP is disabled.
3. **Internal-First Delivery Order & Delivery-Aware Submission Result:**
   - Evolved `IInquirySubmissionService` contract to return `InquirySubmissionResult` with `InquiryAccepted`, `InternalNotificationSent`, `VisitorConfirmationSent`, and authoritative `ReferenceNumber`.
   - Sequential delivery pipeline:
     1. Validate input and re-resolve canonical context.
     2. Generate authoritative inquiry reference number (`REQ-yyyyMMdd-XXXXXX`).
     3. Compose and dispatch internal notification to `Smtp:NotificationRecipient` with `Reply-To` set to visitor's email.
     4. Gate: Only if internal notification succeeds, compose and dispatch visitor confirmation email.
     5. Return delivery-aware result.
   - Failure Semantics:
     - Case A (Both succeed): Thank-You page renders confirmation email sent notice ("A confirmation email containing this reference number has been sent to the email address you provided.").
     - Case B (Internal succeeds, visitor fails): Inquiry accepted, submission not lost. Thank-You page displays fallback guidance ("Your submission has been received. Please keep this reference number for your records."). Sanitized warning logged.
     - Case C (Internal fails): Inquiry rejected, no visitor email attempted, no success redirect. Redisplays form with safe visitor error ("We couldn't send your request at this time. Please try again.") while preserving submitted inputs. Zero SMTP exceptions or credentials exposed.
     - Case D (Honeypot): Zero SMTP calls, silent diversion with synthetic reference number, zero claim of email sent.
     - Case E (Direct Navigation): Zero SMTP calls, neutral status view.
4. **Controlled Subject Policy (Zero Arbitrary Text Injection):**
   - Subjects composed strictly from controlled server tokens: `[DEV] USAP Website — {Controlled Inquiry Type} — {Optional Canonical Context} — {Reference}` for internal, and `[DEV] United States Antenna Products — {Inquiry Received / Quote Request Received} — {Reference}` for visitor.
   - Arbitrary visitor-authored free text (`ProductOfInterest`, `Name`, `Organization`, `Message`, `IntendedApplication`) is strictly barred from subjects, preventing CRLF and header injection.
5. **Dual Multipart/Alternative Email Body Standards:**
   - Both HTML and plain-text versions generated using MimeKit `BodyBuilder`.
   - HTML: Clean industrial styling, 600px centered table container, system font stack, USAP navy headers (`#0d1b2e`), high contrast, text-based branding, zero external dependencies/JS/tracking pixels.
   - Non-production environment marker: Obvious `DEVELOPMENT / TEST` banner rendered at top and bottom in non-Production environments; suppressed in Production.
   - Visitor confirmation emails display a clean Submission Summary without echoing sensitive free-text `Message` or `IntendedApplication` fields, and without turnaround or availability promises.
   - Plain-text versions contain complete substantive data with clean ASCII formatting.
6. **Logging & Privacy Audit:**
   - Operational logs record reference number, controlled inquiry type, delivery stage, and sanitized outcome.
   - Strictly excluded from logs: SMTP passwords, secrets, full email bodies, visitor messages, intended applications, visitor email addresses as routine data, and phone numbers.
7. **Strict Scope Boundary:**
   - reCAPTCHA and GA4 remain deferred to future checkpoints. No commit, push, or merge during this checkpoint.

---

## DEC-051 — USAP-FORMS-003-R1: SMTP Security Hardening, Canonical Reference Consistency, Branded Email Refinement, and Failure Semantics

**Date:** 2026-09-30
**Decision:**
1. **Canonical Reference Number Consistency (`InquiryReferenceGenerator`):**
   - Consolidated inquiry reference generation across genuine (`SmtpInquirySubmissionService`, `DevelopmentInquirySubmissionService`) and honeypot (`InquiryPageModelBase`) pathways into `InquiryReferenceGenerator.Generate()`.
   - Canonical format: `REQ-yyyyMMdd-XXXXXX` (19 characters, e.g. `REQ-20260930-B2582E`).
   - Ensures honeypot diversion cannot be distinguished from genuine submissions by prefix, length, or entropy.
   - Byte-for-byte consistency verified across internal subject/body, visitor subject/body, Thank-You pages, and structured logs.
2. **Strict SMTP Transport Security Hardening:**
   - Restricted supported SMTP `SecurityMode` values to `StartTls` and `SslOnConnect` (case-insensitive aliases: `starttls`, `sslonconnect`, `ssl`).
   - Insecure modes (`None`, `Auto`, `StartTlsWhenAvailable`) are strictly rejected and fail validation.
   - Certificate validation remains strict (`CheckCertificateRevocation = true`). Provider-neutral transport; no hardcoded IONOS logic.
3. **True Startup SMTP Options Validation:**
   - Enforced `.ValidateOnStart()` on `SmtpOptions` in `src/USAP.Web/Program.cs`.
   - Validates that when `Smtp:Enabled == true`, Host, Port, Username, Password, FromAddress, NotificationRecipient, and SecurityMode are valid.
   - Validates mailbox addresses using `MimeKit.MailboxAddress.TryParse` (rejecting crude string checks).
   - Allows clean application startup when `Smtp:Enabled == false` without credentials.
4. **Sanitized Failure Logging Correction:**
   - Eliminated raw `Exception` object logging across SMTP pipeline.
   - Normal operational failure logs record only: reference number, controlled inquiry type, delivery stage, and exception type name (`ex.GetType().Name`).
   - Zero SMTP credentials, command responses, full exception stacks, visitor messages, or PII exposed to logs or visitors.
5. **Cancellation Semantics:**
   - Updated `SmtpInquirySubmissionService` to catch and rethrow `OperationCanceledException` when cancellation is requested on the supplied token (`catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) { throw; }`).
   - Request cancellation is no longer misclassified as an ordinary SMTP delivery failure.
6. **Email Branding Refinement (Website-Aligned) [Superseded in part by DEC-052]:**
   - Header: Clean white surface with all-red typographic USAP brand mark (`UNITED STATES ANTENNA PRODUCTS`) in uppercase bold sans-serif with tracked spacing, followed by a mandatory 3px USAP red (`#c8102e`) horizontal rule. *(Superseded by DEC-052: R3 dark navy header `#0d1b2e`, red eyebrow `USAP WEBSITE`, and 21px bold white brand mark).*
   - Accent Rebalancing: Replaced light-blue accents with USAP red (`#c8102e`), USAP dark navy (`#0d1b2e`), and neutral borders (`#e2e8f0`).
   - Multiline Normalization: Removed `white-space: pre-wrap` where `<br>` conversion occurred, eliminating double-spacing artifacts.
   - Footer: Dark navy (`#0d1b2e`) branded footer with 3px red accent rule and verified public contact info from `_ContactSidebar.cshtml` (5263 Agro Drive, Frederick, MD 21703; Phone: 240-341-7120; Fax: 240-371-4980; Canonical domain link: `https://www.usantennaproducts.com/`). *(Superseded in part by DEC-052: canonical domain corrected to usantennaproducts.com).*
   - Public Contact Info Integrity: Zero invented emails, business hours, department names, or turnaround promises.
   - Scope Discipline: The actual website footer and site-wide logo remain completely untouched.
7. **Logo Asset Limitation & Alternate Preview:**
   - Evaluated `wwwroot/images/brand/usap-logo.svg`: found to contain an embedded 180x102 JPEG on an opaque white rect background, lacking transparent vector master qualities.
   - Selected typographic brand mark as the authoritative production template implementation.
   - Preserved logo-based variation strictly as alternate sanitized previews in `optional-logo-variant/`.
8. **Git Boundary:**
   - Strict uncommitted working tree preserved. No reset, no stash, no commit, no push, no merge. No index mutation used for diff generation.

---

## DEC-052 — USAP-FORMS-003-R3: Final Email Visual Enrichment, Eyebrow Branding, Canonical Domain Reconciliation, and Logo Asset Enhancement Deferral

**Date:** 2026-09-30
**Decision:**
1. **Final Email Header Architecture (Website-Aligned Red Eyebrow & Brand Hierarchy):**
   - Established the accepted dark navy (`#0d1b2e`) header foundation with a bottom 3px USAP red (`#c8102e`) accent rule.
   - Red Eyebrow Accent: Added `USAP WEBSITE` (`#c8102e`, 11px, font-weight 700, uppercase, letter-spacing 1.2px, line-height 1.2, margin-bottom 6px) above the company name, matching the website's restrained red eyebrow design language.
   - Typographic Brand Lockup: `UNITED STATES<br />ANTENNA PRODUCTS` rendered in high-contrast solid white (`#ffffff`, 21px, font-weight 800, line-height 1.2, letter-spacing 1.2px, uppercase). System email-safe font stack ensures reliable rendering without web font downloads.
   - Subordinate Functional Descriptors: Subdued cool neutral (`#cbd5e1`, 12px, font-weight 600, uppercase, letter-spacing 0.8px, margin-top 8px) for email type identification (`NEW WEBSITE INQUIRY`, `NEW QUOTE REQUEST`, `INQUIRY CONFIRMATION`, `QUOTE REQUEST CONFIRMATION`).
   - Development Badge: Positioned with 12px top margin beneath the descriptor; automatically omitted in Production.
   - Operational Header Padding: Balanced compact dimensions (`26px 32px 22px 32px`) preventing excessive vertical height.
2. **Body Accent & Section Eyebrow Refinement:**
   - Reference Callout: Preserved light neutral card with 4px red left border; converted reference label (`REFERENCE NUMBER` / `QUOTE REQUEST REFERENCE`) to USAP red eyebrow treatment (`#c8102e`, 11px, font-weight 700, uppercase, letter-spacing 0.6px).
   - Section Eyebrows: Refined `SUBMISSION SUMMARY`, `CANONICAL WEBSITE CONTEXT`, `VISITOR CONTACT DETAILS`, `VISITOR-PROVIDED REQUEST DETAILS`, and `MESSAGE / PROJECT REQUIREMENTS` into restrained USAP red eyebrows (`#c8102e`, 11px, font-weight 700, uppercase, letter-spacing 0.8px) with subtle bottom border.
3. **Canonical Domain Reconciliation (Zero Stale Domain in Output):**
   - Verified that all generated email outputs (both HTML and plain-text across all 4 message types) use the authoritative public domain: `https://www.usantennaproducts.com/` (display text: `www.usantennaproducts.com`).
   - Confirmed zero occurrences of stale domain `usantenna.com` in current generated email outputs.
   - Actual website Contact sidebar and footer remain completely untouched in this branch.
4. **Production Typographic Lockup & Future Professional Logo Asset Deferral:**
   - Evaluated existing raster-in-SVG asset (`usap-logo.svg`) and confirmed that embedded JPEG on opaque white background is unsuitable for production email headers.
   - Production email remains purely typographic, lightweight, and independent of image loading or CID attachments.
   - Deferred Enhancement: A future high-quality transparent / true-vector USAP logo asset may be proposed to CES and USAP stakeholders during the first stakeholder feedback round as a separately authorized branding enhancement. The template structure allows future substitution as a localized header-template edit.
5. **Final Review Gate & Git Boundary:**
   - Maintained full cumulative C1 + R1 + R2 + R3 working tree without committing, pushing, merging, or mutating the Git index.
   - Complete 19-item review package and SHA-256 validated archive prepared for Human Project Lead review.
