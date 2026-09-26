# Architecture

## Runtime and SDK

| Item | Value |
|---|---|
| Target framework | `net10.0` |
| SDK (pinned) | `10.0.400` (`global.json`, `rollForward: latestPatch`, `allowPrerelease: false`) |
| Deployment model | Framework-dependent |
| Hosting | IIS via ASP.NET Core Module (ANCM) |

## Technology choices

- Server-rendered ASP.NET Core 10 Razor Pages — no SPA, no database, no CMS.
- No Bootstrap, jQuery, or frontend build pipeline.
- Static structured data files for product and resource content (format finalized in Catalog & Resources milestone).
- Repository-level `NuGet.config` clears a stale machine-level VS fallback folder reference. See DEC-013.

## Solution

`USAP.Web.sln` is a classic `.sln` file generated with:

```powershell
dotnet new sln --name USAP.Web --format sln --output . --force
dotnet sln USAP.Web.sln add --in-root src\USAP.Web\USAP.Web.csproj
```

The `--format sln` option in SDK 10.0.400 produces the required classic format (the default is `.slnx`). See DEC-014.

## Project structure

```
usap-website/
├── .editorconfig
├── .gitattributes        SVG text; no *.sln merge=union
├── .gitignore
├── AGENTS.md
├── NuGet.config
├── README.md
├── USAP.Web.sln
├── global.json
├── docs/
│   ├── reference-materials/  (internal only; not served publicly)
│   └── *.md
└── src/
    └── USAP.Web/
        ├── Middleware/
        │   └── LegacyDocumentRedirectMiddleware.cs   HTTP 301 legacy PDF redirects
        ├── Models/
        │   └── Catalog/
        │       ├── ProductFamilyRecord.cs
        │       ├── ProductGroupRecord.cs
        │       └── ProductResourceRecord.cs          Canonical technical document model
        ├── Pages/
        │   ├── Products/     /products and /products/{familySlug}
        │   ├── Shared/
        │   │   ├── _Layout.cshtml        root layout (skip link, partials, deferred JS)
        │   │   ├── _Header.cshtml        shared header with brand region and primary nav
        │   │   ├── _Footer.cshtml        shared footer with link columns and copyright
        │   │   ├── _Seo.cshtml
        │   │   └── _StructuredData.cshtml
        │   ├── Index.cshtml          /
        │   ├── AboutUs.cshtml        /about-us
        │   ├── ContactUs.cshtml      /contact-us
        │   ├── RequestAQuote.cshtml  /request-a-quote
        │   ├── TechnicalResources.cshtml  /technical-resources
        │   ├── ThankYou.cshtml       /thank-you (provisional)
        │   └── Error.cshtml
        ├── Properties/launchSettings.json
        ├── wwwroot/
        │   ├── css/
        │   │   ├── site.css          design system tokens, layout, components
        │   │   └── technical-resources.css   library grid, filters, and cards
        │   ├── js/
        │   │   ├── site-navigation.js  dependency-free mobile nav module
        │   │   ├── disclosure.js       native accessible disclosure controller
        │   │   └── technical-resources.js    client-side search and category filtering
        │   ├── documents/
        │   │   └── technical/        17 canonical first-party PDF binaries
        │   ├── robots.txt
        │   └── sitemap.xml
        ├── appsettings.json
        ├── appsettings.Development.json
        └── Program.cs
```

## Configuration

| Context | Approach |
|---|---|
| Base | `appsettings.json` — no secrets |
| Development | `appsettings.Development.json` — logging only |
| Local secrets | `dotnet user-secrets` |
| Production | IIS environment variables |
| Certificates | IIS HTTPS bindings — never in source control |

## CSS architecture

Single organized stylesheet `site.css` with 12 labeled sections:
Tokens → Reset → Typography → Layout primitives → Links/buttons → Cards/surfaces →
Form controls → Header/navigation → Footer → Utilities → Responsive → Reduced-motion.

`body`: `min-height: 100vh; min-height: 100dvh; display: flex; flex-direction: column`
`main`: `flex: 1; width: 100%`
No `overflow-x: hidden`. Layout integrity maintained through correct widths.

All design tokens are CSS custom properties in the `:root` block. See `docs/DESIGN_SYSTEM.md`.

## Shared partial structure

`_Layout.cshtml` includes `_Header.cshtml` and `_Footer.cshtml` via `Html.PartialAsync`.
Navigation JavaScript (`site-navigation.js`) is loaded as an external deferred script via
`<script src="~/js/site-navigation.js" defer>` before closing `</body>`, with
`asp-append-version` for cache-busting. It does not block page rendering.

## Progressive-enhancement navigation

Mobile navigation uses a CSS disclosure and fixed overlay pattern:
- Nav list is visible by default (no-JS fallback).
- JavaScript adds `js-nav-ready` to `<html>`, which CSS uses to reveal the toggle and hide the list.
- The toggle manages `aria-expanded`, `hidden`, and `aria-label` on the nav list ID (`primary-nav-list`).
- Desktop navigation activates at `64rem` (1024px); mobile overlay applies below that.
- **Overlay Positioning (C1R6):** When opened, the navigation list is fixed immediately below the header (`position: fixed`, `top: var(--mobile-header-bottom, 4.25rem)`, `height: calc(100dvh - var(--mobile-header-bottom, 4.25rem))`, `overscroll-behavior: contain`), eliminating page reflow or downward push of hero content.
- **Background Scroll-Lock & State Restoration (C1R6):** Opening the mobile navigation locks the viewport via `nav-open-lock` on `html` and `body` (`overflow: hidden !important; overscroll-behavior: none`), capturing `window.scrollY` and restoring it exactly upon closure across repeated cycles without layout drift.
- **Focus Containment via `inert` (C1R6):** Standard `inert` attributes are applied to background page landmarks (`#main-content`, `.site-footer`, `.skip-link`) while open, preventing tab focus leak. A dedicated tracking array ensures only attributes applied by the navigation controller are removed on close, preserving external component state.
- **Native Disclosure Semantics (C1R6):** The Products submenu `<details>/<summary>` disclosure relies on native platform state; redundant `aria-haspopup` and `aria-expanded` attributes are omitted.
- See `docs/DESIGN_SYSTEM.md` — Mobile navigation interaction contract.

## Metadata and SEO Architecture

- **SiteSettings**: Strongly typed global settings bound to `appsettings.json` (e.g., BaseUrl, DefaultTitle).
- **SeoMetadata**: Strongly typed per-page metadata model passed via `ViewData[SeoMetadata.ViewDataKey]`.
- **Shared Partial (`_Seo.cshtml`)**: Extracts settings and metadata to safely render canonical URLs, Open Graph, Twitter/X cards, theme color, and title. Safe encoding is handled by Razor.
- **Canonical generation strategy**: Canonical URLs combine the configured `SiteSettings.BaseUrl` and `SeoMetadata.CanonicalPath` (or current request path), ignoring query strings and `Host` headers.
- **Non-production indexing protection**: An environment-aware middleware in `Program.cs` adds `X-Robots-Tag: noindex, nofollow` to all non-production responses.
- **Sitemap**: Static XML architecture (`wwwroot/sitemap.xml`) selected over dynamic generation. Contains exactly six provisional top-level routes and must be reviewed before production launch.
- **Robots.txt**: Static architecture (`wwwroot/robots.txt`). Explicitly declares the sitemap. Non-production safety relies on the `X-Robots-Tag` header instead of complex `robots.txt` generation.
- **Structured Data**: A shared `_StructuredData.cshtml` safely serializes foundational `Organization` and `WebSite` JSON-LD graphs via `System.Text.Json`. It is rendered once in the `<head>` of `_Layout.cshtml`.
- **Page-level schema hook**: An optional `@await RenderSectionAsync("StructuredData", required: false)` exists in `_Layout.cshtml` `<head>` for future page-specific structured data extensions (e.g., `Product`, `BreadcrumbList`).
- **Redirects**: Redirect implementation remains deferred. Initial legacy URL rules are documented in `docs/REDIRECT_MAP.md`.

## Error Handling Architecture

- **Dedicated 404 Page**: A separate `/not-found` Razor Page handles HTTP 404 (Not Found) responses using `app.UseStatusCodePagesWithReExecute("/not-found")`.
- **Dedicated 500 Page**: The `/Error` Razor Page is retained exclusively for unexpected server failures using `app.UseExceptionHandler("/Error")`.
- **Status Preservation**: HTTP status codes are explicitly preserved.
- **Canonical Omission**: Error pages omit canonical and `og:url` tags to prevent indexing errors.
- **Slug Validation**: Product-family routes validate slugs strictly via `IProductCatalogService.GetFamilyBySlug`, returning true HTTP 404 for invalid slugs.

## Catalog Presentation Projection Architecture (USAP-CATALOG-002-C1)

- **Presentation Projection Separation**: The catalog architecture establishes a clean boundary between public presentation models and internal research governance tracking. Raw research governance fields (`ApprovalStatus`, `PublicationRecommendation`, `SpecificationStatus`, `ConflictHolds`, `SourceNotes`, `SourcePresence`, `CommercialAvailability`, `ClientConfirmation`, `HoldDisputedSpecs`, `confidence score`) are excluded from page-facing models and rendered output.
- **Public Domain Models (`USAP.Web.Models.Catalog`)**:
  - `ProductCharacteristic`: Compact key-value record `(string Label, string DisplayValue)` for safe technical characteristics and operational limits.
  - `ProductModelRecord`: Projection containing `(string ModelCode, string? DisplayName, string? Description, IReadOnlyList<ProductCharacteristic>? Characteristics)`.
  - `ProductGroupRecord`: Projection containing `(string Id, string FamilyId, string Name, int DisplayOrder, string SectionAnchor, string ShortDescription, string? ExpandedIntroduction, CatalogAsset? AssociatedAsset, IReadOnlyList<ProductModelRecord> Models, IReadOnlyList<ProductCharacteristic>? GroupCharacteristics, bool HasPublishedModelNumber, string? ModelNote)`.
  - `ProductResourceRecord`: Extensible resource model `(string Id, string ProductGroupId, string Title, string DocumentType, string CurrentSourceUrl)`.
- **Public Catalog Inventory & Safeguards (30 + 1)**:
  - Public projection renders all 30 named model/configuration identifiers from current site records across 15 product groups, plus 1 unnamed Aperiodic loop system record (`HasPublishedModelNumber = false`). Total: 31 catalog records across 16 groups.
  - All six 1942 NVIS configuration identifiers are published (`1942-RT`, `1942-TA`, `1942-GM`, `1942-RT-LP`, `1942-TA-LP`, `1942-GM-LP`). The three low-power variants render strictly with conservative configuration-level role descriptions, omitting unverified power, weight, gain, or coverage claims.
  - R3500 presentation is scoped strictly to non-conflicting mechanical rotator facts, with no unconfirmed DRC-4 bundling, pairing, or commercial availability claims.
- **Complete Visual Coverage Architecture**:
  - All 16 product groups feature intentional visuals: the 3 approved-for-provisional-use rotator visuals (`r3500`, `drc-3`, `drc-4`) and the 13 project-lead-selected A1 source-guided candidates. LP-1112MR, 1910, and 1942 employ contain-style responsive behavior to preserve their complete footprints without aggressive cropping.
  - The Products landing page (`/products`) integrates 3 coordinated Engineering Guidance card visuals (Deployment & Mobility, Coverage & Propagation, Positioning & Infrastructure); the wide combined alternate visual remains deferred.
- **Interim Product Documents & Extensible Resource Model**:
  - Six current interim product-page resource links are wired (`doc-lp-high-power`, `doc-lp-1018ba`, `doc-lp-1019`, `doc-1910-2024`, `doc-aperiodic`, `doc-t-3002-oct2016`). The data architecture supports multiple documents per group and is designed to accommodate the 17 canonical PDFs identified in R2 for planned local migration under `USAP-TECHDOC-001`, without imposing a permanent six-document limit or classifying the remaining PDFs as rejected.
- **Always-Visible Product-Group Overview & Nested Disclosure Architecture (C1R4)**:
  - Product-group sections are structured as an outer semantic `<article class="product-group-card">` containing an always-visible overview (`.product-group-overview`) and a nested native `<details class="product-group-disclosure">`.
  - The overview displays the product-group visual, heading, short summary, metadata badges, and up to three key group characteristics at all times, ensuring product imagery and primary identity remain visible when collapsed.
  - The nested disclosure houses deeper model cards (`.product-models-grid`), additional group characteristics, interim technical resources, and engineering contact links, toggled by a clear native summary control (`View Models & Specifications` / `Hide Models & Specifications`; `View Configuration & Technical Details` / `Hide Configuration & Technical Details` for Aperiodic).
  - Progressive enhancement via `disclosure.js` operates seamlessly on the nested disclosure without requiring JavaScript for core access.
- **Product Selection FAQ Layout Refinement (C1R4)**:
  - `.faq-list` occupies the full standard `.container` width (`width: 100%`), eliminating narrow-column alignment constraints.
  - Answer copy is constrained internally (`max-width: 75ch`) on paragraph elements to preserve optimal readability line lengths.
- **Deterministic Startup Validation**:
  - `ProductCatalogService` validates deterministic catalog invariants at instantiation (exact 6 canonical families in order, exact 16 product groups matching canonical family distribution, exactly 30 unique named public model codes, Aperiodic 0-model handling, presence of all 6 1942 codes with safe low-power characteristics, exact 6 interim resource records, all 16 group visuals present with valid paths and alt text, and exhaustive scanning for prohibited governance tokens and held claim strings). Any violation throws `InvalidOperationException` and halts startup immediately.

## Header Navigation & Dropdown Architecture (USAP-CATALOG-002-C1R5)

- **Semantic Disclosure Baseline**: The site header partial (`Pages/Shared/_Header.cshtml`) implements a progressive-enhancement dropdown for the Products menu item using native `<details class="nav-dropdown" id="nav-products-dropdown">` and `<summary class="primary-nav-link nav-dropdown-toggle">`.
- **Accessible Structure**: Zero nested anchors; the summary toggle contains only text and a presentational SVG chevron. Child anchors inside `.nav-dropdown-menu` point to All Products (`/products`) and the six canonical family routes.
- **Client Enhancement & State Synchronization**:
  - `site-navigation.js` synchronizes the `aria-expanded` attribute on the summary toggle with the `<details>` open/closed state.
  - On desktop (≥1024px / 64rem), outside clicks and `Escape` key presses dismiss the dropdown and return focus to the summary toggle.
  - On mobile (<1024px / 64rem), expanding or collapsing the Products dropdown operates within the mobile navigation drawer without triggering drawer dismissal. Selection of any child navigation link closes the drawer.
  - Fully accessible to keyboard navigation, screen readers, mouse, and touch. Retains complete native functionality when JavaScript is unavailable.
- **Active Navigation Area Highlighting**: The Products navigation area is highlighted (`is-active` and `aria-current="page"`) when viewing `/products` or any of the six family routes (`/products/{familySlug}`).

## Hero Scaling & Resilience Architecture (USAP-CATALOG-002-C1R5)

- **Content-Driven Dimensions**: Hero containers (`.internal-hero`, `.internal-hero--family`, `.internal-hero--products`) avoid rigid height or max-height constraints. Total height is determined by content volume and vertical padding.
- **Multi-Scale Viewport Support**: Responsive vertical padding `clamp(var(--space-8), 4vw, var(--space-12))`, fluid headings `clamp(1.875rem, 3.25vw + 0.5rem, 2.75rem)`, and `overflow-wrap: break-word` ensure zero text clipping or horizontal overflow across extreme viewports (390px to 2560px) and browser zoom levels (80%, 100%, 125%).
- **Breadcrumbs Segregation**: Family-page breadcrumbs remain strictly located in Section 2 below the hero element, preserving content separation.

## Mobile Navigation Overlay, Explicit Curation & Family SEO Architecture (USAP-CATALOG-002-C1R6)

- **Mobile Navigation Fixed Overlay**:
  - The opened navigation list is decoupled from normal document flow (`position: fixed`, `top: var(--mobile-header-bottom, 4.25rem)`, `height: calc(100dvh - var(--mobile-header-bottom, 4.25rem))`, `overscroll-behavior: contain`), eliminating layout reflow and downward shift of hero and page content.
  - Dynamically updates `--mobile-header-bottom` on open, window resize, and orientation change using `getBoundingClientRect().bottom`.
  - Body and document root are locked against background scrolling via `nav-open-lock` (`overflow: hidden !important; overscroll-behavior: none`). Scroll position (`window.scrollY`) is recorded on open and restored precisely upon close across multiple cycles without scroll drift.
  - Interactive background landmarks (`#main-content`, `.site-footer`, `.skip-link`) receive the HTML5 `inert` attribute while open to trap focus in the navigation overlay. The controller tracks only elements it altered, removing `inert` cleanly upon closure without interfering with external components.
  - Centered mobile Products disclosure control (`justify-content: center; gap: var(--space-2)`) groups the label and rotating chevron SVG while maintaining left-aligned, indented child family links.
  - Redundant `aria-haspopup="true"` and `aria-expanded="false"` attributes removed from the native `<summary>` control; state is managed natively by `<details>`.

- **Explicit Overview-Characteristic Curation**:
  - `ProductGroupRecord` separates group characteristics into canonical `OverviewCharacteristics` (0 to 3 items) and `DetailedCharacteristics` (remaining items), completely removing runtime `Take(3)` and `Skip(3)` slicing from Razor markup.
  - `ProductCatalogService.ValidateCatalog` deterministically enforces:
    - Maximum of 3 overview characteristics per product group;
    - Non-empty label and value strings;
    - Unique characteristic labels within each product group;
    - Zero overlap between overview and detailed collections;
    - Exhaustive scanning of family SEO titles, descriptions, eyebrows, headings, and intros for prohibited governance tokens.

- **Family Page SEO & Visible Heading Hierarchy**:
  - Hero H1 remains the primary, most prominent page title (`clamp(1.875rem, 3.25vw + 0.5rem, 2.75rem)`).
  - Post-hero section heading uses a visibly subordinate H2 (`clamp(1.5rem, 2.5vw, 1.875rem)` in `.family-groups-header .section-heading`) with family-specific eyebrow and conservative supporting copy.
  - Logical heading outline: H1 (Family Title) -> H2 (Family Models & Configurations) -> H3 (Product Group) -> H4 (Subsections / Models) -> H5 (nested configurations if applicable).
  - Indexable production intent: Family pages omit `Robots` in page metadata, adopting the production-intent `"index, follow"` default. Non-production environments remain safeguarded via middleware `X-Robots-Tag: noindex, nofollow`.

## Always-Visible Sticky Header & Mobile Navigation Architecture (USAP-CATALOG-002-C1R8)

- **Always-Visible Sticky Header Architecture**:
  - Following project-lead direction in C1R8, the shared `.site-header` is standard on persistent `position: sticky; top: 0;` and remains visible at all times during normal page operation.
  - While the mobile navigation scroll lock is active (`.nav-open-lock`), CSS temporarily viewport-pins the header with `position: fixed; top: 0; left: 0; right: 0; width: 100%;` so it remains pinned at `top: 0` while `html`/`body` overflow is locked mid-page.
  - The superseded scroll-direction auto-hide state machine (downward hide, upward reveal, idle timer, rAF scroll sampling, and transform transitions) has been completely removed from `site.css` and `site-navigation.js`.
  - Zero cumulative layout shift (CLS = 0) and instantaneous visibility across all scrolling directions without scroll event listeners.
  - Stacking order uses `z-index: var(--z-sticky)` (200), elevating to `z-index: var(--z-overlay)` (300) when mobile navigation is open.

- **Content-Height Mobile Navigation Drawer & Translucent Backdrop**:
  - Replaced the forced full-viewport height with a content-driven panel (`height: auto; max-height: calc(100dvh - var(--mobile-header-bottom))`).
  - In collapsed state, the drawer terminates neatly below the Request a Quote CTA + bottom padding, eliminating unnecessary blank navy space and revealing the page surface beneath.
  - When expanded or on short-height displays, the panel caps at `max-height` and activates momentum scrolling (`overflow-y: auto; overscroll-behavior: contain`).
  - A separate click-away backdrop (`#nav-backdrop`, 40% translucent black with subtle blur) covers the remaining screen below the header and dismisses the navigation on tap.
  - When opened, `.site-header` elevates its stacking context (`z-index: var(--z-overlay)`), ensuring header controls and drawer remain fully interactive above the backdrop.
  - Top-level controls (Home, Products, Tech Resources, About, Contact, Quote) are horizontally centered. The active route indicator uses an absolute left accent bar (`::before`), guaranteeing zero label displacement (sub-pixel centering difference <= 0.01px).

- **C1R6 Characteristic Audit Resolution**:
  - Discovered and verified that the contradiction between C1R6 documentation and its audit report was caused by an audit-script selector/extraction bug in C1R6 tooling.
  - The active codebase, domain records, service implementation, and Razor DOM output are completely intact and accurately project the approved curated characteristics (3 overview for lp-high-power, lp-1019, v-4213, lp-1402-1403, 1910, aperiodic, 1942; 2 for t-3002; detailed specs in disclosures; empty groups empty; zero Take/Skip slicing).

## Shared Internal-Hero Geometry and Full-Image Presentation Architecture (USAP-CATALOG-002-C1R9)

- **Shared Desktop/Tablet Baseline Height**:
  - Reconciled the shared internal hero foundation to establish a unified desktop/tablet height baseline (`--internal-hero-min-height: 24.2875rem;` / ~388.6px), measured directly from the product-family hero baseline at 1440×900.
  - Normal image-backed heroes across all routes (`/products`, `/contact-us`, `/request-a-quote`, and all six `/products/{familySlug}` routes) resolve to exact height parity (388.6px, 0.0px variance at 1440×900 and 1920×1080).
  - Preserved fluid content resilience (`height: auto; min-height: ...;` without max-height constraints). Heroes with longer content (such as `/about-us` at 427.1px) expand naturally without clipping under normal viewports, browser zoom (125%, 200%), or accessibility text scaling.

- **Full-Image Right-Side Media Containment**:
  - Superseded the prior destructive full-width `object-fit: cover` presentation on non-family heroes (`/products`, `/about-us`, `/contact-us`, `/request-a-quote`).
  - Both `.internal-hero-picture` (family pages) and standalone `.internal-hero-image` elements use a bounded right-side media region at desktop (`width: 65%; max-width: 1100px; right: 0; left: auto; height: 100%; object-fit: contain; object-position: right center;`) and tablet (`width: 75%;`).
  - Eliminates vertical cover cropping: the entire 8:3 source-image composition (2048×768 on non-family pages, 1536×576 on family pages) is visible without stretching or distortion.
  - Shared gradient overlay (`linear-gradient(to right, var(--color-brand-navy) 0%, var(--color-brand-navy) 35%, rgba(13, 27, 46, 0.85) 55%, rgba(13, 27, 46, 0.40) 80%, transparent 100%)`) provides solid navy protection for left-side copy while seamlessly blending into the contained right-side imagery.

- **Responsive Mobile Behavior (< 48rem / 768px)**:
  - Mobile maintains content-driven vertical expansion (`min-height: 22rem; height: auto`).
  - Media layers render as an atmospheric background (`object-fit: cover; object-position: center center;`) behind a 90% navy overlay (`rgba(13, 27, 46, 0.90)`), ensuring maximum legibility of white typography while preserving zero horizontal overflow.
  - Confirmed as a stable foundation; dedicated 1:1 mobile crop assets for non-family heroes deferred to future asset enhancements.

## Final Internal-Hero Height Normalization and Top-Biased Edge-Fade Architecture (USAP-CATALOG-002-C1R10)

- **About Us Height Normalization to Shared Baseline**:
  - Normalized `.internal-hero--about` at desktop/tablet widths (`@media (min-width: 48rem)`) to `padding-block: 1.75rem;` (28px top and bottom).
  - Eliminates the prior ~38.5px height variance, resolving `/about-us` to exactly 388.59px (~388.6px) at normal desktop viewports, achieving complete height parity with the other 9 image-backed internal heroes.
  - Maintains `height: auto` and `--internal-hero-min-height: 24.2875rem;`. If zoom or narrow text wrapping increases content height, the hero expands naturally without clipping.
  - Symmetrical breathing room of ~28.8px above the eyebrow and ~29.8px below the CTA button maintains clean visual balance without modifying copy or typography.

- **Top-Biased Edge-Fade Architecture (Candidate D)**:
  - Upgraded `.internal-hero-overlay` and `.internal-hero-overlay--family` at desktop (`>= 64rem`) and tablet (`48rem - 63.999rem`) to use a multi-background gradient composition.
  - Layers a vertical top-biased dissolve (`to bottom, var(--color-brand-navy) 0%, rgba(13, 27, 46, 0.75) 5%, transparent 18%, transparent 88%, rgba(13, 27, 46, 0.45) 96%, var(--color-brand-navy) 100%`) over the established horizontal text-protection gradient.
  - Refined 5% top navy band dissolving by 18% eliminates the sharp horizontal color boundary between bright sky imagery and the surrounding navy hero container without unnecessarily darkening natural sky or upper antenna structures.
  - Lighter bottom dissolve preserves ground hardware and terrain detail while softening the lower pixel transition.
  - Central 70% of the image height (18% to 88%) remains 100% unshaded and visually intact. Full 8:3 source imagery remains contained and non-cropped.

## Ultra-Wide Right-Anchored Hero Composition Architecture (USAP-CATALOG-002-C1R11)

- **Evidence-Backed ~1590px Crossover & Ultra-Wide Breakpoint**:
  - The C1R11-P1 geometry study proved that 8:3 source imagery at the normalized usable hero height of ~387.59px displays at exactly 1033.58px width (~1034px). With a desktop media track allocated at 65vw, the crossover from width-limited to height-limited presentation occurs at $\frac{1033.58\text{px}}{0.65} \approx 1590.13\text{px}$.
  - Established `@media (min-width: 100rem)` (~1600px) as the controlling ultra-wide internal-hero breakpoint.
  - Below 100rem (verified at 1440×900 and 1366×768), all desktop, tablet, and mobile presentations remain 100% unchanged from the accepted C1R10-R2 baseline.
- **Right-Pinned Media with Moderate Responsive Hero Growth (Candidate B)**:
  - Following physical wide monitor review, the initial centered 100rem canvas and right navy gutter were superseded in favor of right-edge pinning (`right: 0`) combined with moderate, capped responsive hero growth:
    - `.internal-hero`: `min-height: clamp(24.2875rem, 22vw, 28rem);` (scales from ~388.6px at 1600px to ~422.4px at 1920px, capped at 448.0px at 2560px).
    - `.internal-hero-picture, .internal-hero-image`: `right: 0; width: clamp(65rem, 59vw, 75rem); max-width: none;` (scales from 1040px at 1600px to 1132.8px at 1920px, capped at 1200px at 2560px).
  - Visible imagery terminates cleanly at the physical browser right edge (`visibleImageRight == viewportWidth`) with zero right-side navy gutter.
  - Responsive hero growth allows the full 8:3 source composition to scale proportionately, extending visible imagery significantly farther left (+90.1px at 1920px, +158.4px at 2560px compared to fixed-height Candidate A) to eliminate the empty center chasm while maintaining an elegant, non-dominant hero height.
- **Relaxed Ultra-Wide Horizontal Copy-Protection Overlay (C1R11 / C1R12 Refinement)**:
  - Anchors gradient color stops to the centered 100rem design canvas via `--ultra-wide-gutter: max(0px, (100vw - 100rem) / 2)`:
    `linear-gradient(to right, var(--color-brand-navy) 0, var(--color-brand-navy) calc(max(0px, (100vw - 100rem) / 2) + 30rem), rgba(13, 27, 46, 0.88) calc(max(0px, (100vw - 100rem) / 2) + 40rem), rgba(13, 27, 46, 0.58) calc(max(0px, (100vw - 100rem) / 2) + 50rem), rgba(13, 27, 46, 0.24) calc(max(0px, (100vw - 100rem) / 2) + 62rem), transparent calc(max(0px, (100vw - 100rem) / 2) + 72rem))`
  - Under C1R12, the horizontal transition was softened to Candidate H1: solid navy extends to `gutter + 30rem` (covering copy start to 640px at 1920px), maintains high opacity (88% at `+40rem` / 800px where image enters) to eliminate visible tonal seam lines under the heading, dissolves through `+50rem` (58%) and `+62rem` (24%), and reaches full transparency by `+72rem` (1312px) before main antenna hardware subjects.
  - Layered under the accepted Candidate D vertical top/bottom fade (`linear-gradient(to bottom, navy 0%, 75% navy 5%, transparent 18%, transparent 88%, 45% navy 96%, navy 100%)`).
  - Completely eliminates the rejected right-to-left navy gutter fade. Ensures 100% solid navy contrast behind typography while allowing graphic details to emerge smoothly without dark bands or seams.
- **Rotator & Control Systems Route-Specific Image-Entry Mask (USAP-CATALOG-002-C1R13-R1)**:
  - While the shared Candidate H1 overlay cleanly blends photographic hero imagery (which features natural outdoor vignetting), the Rotator & Control Systems asset (`usap-family-rotator-control-hero-a3s-desktop-1536x576.png`) features a high-contrast mechanical schematic grid drawn over a uniform dark-blue studio field that starts abruptly with a sharp vertical boundary at $x=0$.
  - At ultra-wide displays ($\ge 100\text{rem}$ / $1600\text{px}+$ viewports), this hard rectangular edge produced a perceptible contrast step against the solid navy canvas.
  - Implemented Candidate R2: a route-specific horizontal alpha mask applied directly to `.internal-hero--family-antenna-rotator-control-systems .internal-hero-picture` over its left $12\text{rem}$ ($192\text{px}$):
    `-webkit-mask-image` / `mask-image`: `linear-gradient(to right, transparent 0, rgba(0, 0, 0, 0.55) 4rem, rgba(0, 0, 0, 0.85) 8rem, #000 12rem)`.
  - Dissolves the actual image pixels to zero opacity at the asset boundary, allowing the blueprint schematic to emerge organically from the `#0d1b2e` hero canvas without any contrast step, without dimming rotator or controller hardware, and without secondary veil lines.
  - Scoped strictly to `@media (min-width: 100rem)`. Sub-100rem viewports (where image enters under 100% solid navy) and all other nine hero routes remain $100\%$ untouched. Targeted via the existing semantic `.internal-hero--family-antenna-rotator-control-systems` class without any Razor markup modifications.
- **Uniform Shared System & Scope Boundaries**:
  - Applied uniformly across all 10 image-backed internal heroes (`/products`, `/about-us`, `/contact-us`, `/request-a-quote`, and all 6 product-family routes).
  - Preserves full `object-fit: contain; object-position: right center;` with zero cropping, distortion, or source asset changes.
  - Homepage hero remains separate and unmodified. Technical Resources hero will inherit this shared architecture upon eventual implementation.

- **Technical Resources Architecture & Document Migration (USAP-TECHDOC-001 / USAP-TECHDOC-001-R1 / USAP-TECHDOC-001-R2)**:
  - **Direct Public Access**: Implemented direct public access to locally hosted technical PDFs without lead capture gating per project-lead directive (DEC-042).
  - **Local Canonical Document Hosting & Preferred Public URL**: 17 verified first-party canonical PDFs hosted under `src/USAP.Web/wwwroot/documents/technical/{family-slug}/{filename}`. Established `/technical-resources/documents/{family-slug}/{filename}` as the singular preferred public raw-PDF URL. Alternate `/documents/technical/...` paths issue permanent HTTP 301 redirects to the preferred path via `LegacyDocumentRedirectMiddleware` to prevent dual indexable URLs for identical binaries (DEC-043).
  - **PDF Byte Preservation & Metadata Policy**: Strict byte-for-byte fidelity with the verified R2 source package is maintained across all 17 PDFs. Embedded metadata/title anomalies in historical PDF binaries are not modified in binary files; title clarity is established purely via HTML document-detail views, `<title>` tags, and structured metadata. Evaluation of metadata-normalized publication derivatives or a controlled viewer is deferred after baseline commit (DEC-044).
  - **Branded HTML Document-Detail Experience**: Implemented reusable data-driven Razor Page `/technical-resources/document/{slug}` with hero heading, relocated semantic breadcrumbs below hero matching Product Family architecture, covered model badges, format/page metadata, direct PDF actions, streamlined native browser embedded PDF viewing (`<object>` / `<iframe>`) with clean fallback, related product group links, and standard closing CTA.
  - **Metadata Model & Service**: `ProductResourceRecord` encapsulates complete technical metadata (`Id`, `Slug`, `DetailUrl`, `Title`, `LocalPdfPath`, `FamilySlug`, `FamilyName`, `ProductGroupIds`, `ModelCodes`, `DocumentType`, `Description`, `PageCount`, `SortOrder`, `LegacySourceUrls`, `SearchText`). `ProductCatalogService` provides `GetTechnicalDocumentBySlug(slug)` alongside family and group resource queries.
  - **Homepage Deep-Link Taxonomy & Preset Views**: Four homepage preview cards deep-link to structured views: `type=data-sheet` (15 data sheets), `view=antenna-systems` (11 antenna documents), `category=antenna-rotator-control-systems` (5 documents), and `category=tower-systems-accessories` (1 document), accompanied by an active filter banner with clear action. Redundant reset buttons are suppressed when preset banner is visible.
  - **Search Toolbar & Hero Artwork**: Toolbar features a right-aligned contained layout on desktop (max-width 32rem) with visible submit button, balancing against left-aligned family filters. Full-width responsive layout on mobile/tablet. Hero graphic `usap-technical-resources-hero-requirements-document-candidate-b-v1.png` is integrated into the shared `.internal-hero` architecture.
  - **Card Footer & Link State System**: Restored atomic, nowrap page-count indicators (`1 page`, `2 pages`) with restrained document SVG icon, paired with compact `View Document` and `Download PDF` actions. Explicit `:link`, `:visited`, `:hover`, and `:focus-visible` styling guarantees zero default browser purple link colors across card titles, metadata, and detail navigation.
  - **Permanent Legacy Document Redirects**: `LegacyDocumentRedirectMiddleware` provides HTTP 301 permanent redirects for all 34 known legacy `/wp-content/uploads/...pdf` URLs, preserving bookmarks and query strings.
  - **Progressive Enhancement & SEO Foundation**: Fully usable without JavaScript via server-side GET handling. Unique server-rendered HTML `<title>`, description, H1, and contextual product links serve as indexable search engine content.

## Deferred decisions

| Decision | Deferred to |
|---|---|
| Static content data format and location | Catalog & Resources (Completed for products and technical documents) |
| SMTP/email service | Forms & Search (Currently simulated in Development with `DevelopmentInquirySubmissionService`; unavailable elsewhere) |
| Analytics (GA4) | Forms & Search |
| Advanced Technical Resources filtering | Separately authorized (basic search, family category filters, and homepage preset views implemented) |
| Legacy page redirect map | Technical document redirects (34 URLs) are fully implemented via `LegacyDocumentRedirectMiddleware`. Legacy WordPress category and product URLs remain deferred to Milestone C3. |
| Internal hero text width and placement on ultra-wide displays | Visual Enhancements (On larger/wider desktop monitors, internal hero copy remains relatively constrained toward the left despite increased space; layout expansion deferred to later visual-enhancement workstream) |
| Metadata-normalized PDF publication derivatives or controlled viewer | Post-Baseline Enhancement (Evaluate after baseline TECHDOC feature commit) |
| Document-detail hero image visual treatment | Post-Baseline Enhancement (Evaluate after baseline TECHDOC stabilization) |

## Forms and Validation

- Forms use native ASP.NET Core `DataAnnotations` and `IValidatableObject` for server-side validation.
- JavaScript provides only progressive enhancement (conditional field visibility). Server validation remains authoritative.
- Submission endpoints (`/contact-us`, `/request-a-quote`) are protected by a POST-only rate limit partitioned by IP.

## IIS deployment

CES Dev: SDK `10.0.400` + ASP.NET Core Hosting Bundle.
Production: Hosting Bundle only (no SDK required for published output).
Details in `docs/DEPLOYMENT_RUNBOOK.md`.
