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
