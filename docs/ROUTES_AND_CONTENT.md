# Routes and Content

## Route map

| Route | Page | Status | Notes | Metadata & Indexing |
|---|---|---|---|---|
| `/` | `Pages/Index.cshtml` | Provisional | Section 5 integrates shared `_ProductFamilyCard.cshtml` featuring 4 canonical families via `IProductCatalogService`, linking to `/products/{slug}` with section-level `View All Products` linking to `/products`. | `HF, VHF & UHF Antenna Systems` (index, follow) |
| `/products` | `Pages/Products/Index.cshtml` | Provisional (USAP-CATALOG-001-C2) | Structured 6-family catalog landing page with Direction C hero, unified shared card grid (3-column), Candidate B rotator card, 3 actionable guidance pathways with inline continuation, 6-question FAQ, and closing CTA. | `Antenna Products & Systems` (index, follow) |
| `/products/{familySlug}` | `Pages/Products/Family.cshtml` | Provisional (USAP-CATALOG-002-C1R6) | Six canonical family routes with responsive hero, breadcrumbs, structured product groups, Configuration Support, and standardized closing sequence. Unique titles/meta descriptions and family-specific headings. Validates slugs with HTTP 404 for invalid slugs. | `[Family Name] | United States Antenna Products` (index, follow — production intent) |
| `/technical-resources` | `Pages/TechnicalResources.cshtml` | Complete (USAP-TECHDOC-001) | Direct public access canonical resource library featuring all 17 canonical documents, client-side keyword search, family category filtering with live counts, responsive cards, direct PDF actions, empty state, and 100% progressive enhancement. | `Technical Resources` (index, follow) |
| `/about-us` | `Pages/AboutUs.cshtml` | Provisional | Reference-grounded imagery integrated; shared hero and closing CTA; client approval pending | `About Us` (index, follow) |
| `/contact-us` | `Pages/ContactUs.cshtml` | Complete (FORMS-002) | Shared internal hero and sidebar with verified contact details; supports trusted query context (`reason`, `family`, `group`, `doc`) resolved via `ICtaContextResolver` with visible `.inquiry-context-panel` badge (`product=` does not establish trusted context; arbitrary models enter only via editable field). | `Contact Us` (index, follow) |
| `/request-a-quote` | `Pages/RequestAQuote.cshtml` | Complete (FORMS-002) | Shared internal hero and sidebar; enforces `InquiryType.RequestAQuote` and required `Organization` field; supports query context resolved via `ICtaContextResolver`. | `Request a Quote` (index, follow) |
| `/contact-us/thank-you` | `Pages/ContactUs/ThankYou.cshtml` | Complete (FORMS-002) | Dedicated confirmation route for contact inquiries; displays reference number from TempData; direct navigation guidance state; excluded from sitemap. | `Thank You — Inquiry Received` (noindex, nofollow) |
| `/request-a-quote/thank-you` | `Pages/RequestAQuote/ThankYou.cshtml` | Complete (FORMS-002) | Dedicated confirmation route for quote requests; displays reference number from TempData; direct navigation guidance state; excluded from sitemap. | `Thank You — Quote Request Received` (noindex, nofollow) |
| `/thank-you` | `Pages/ThankYou.cshtml` | Legacy Redirect (FORMS-002) | Backwards-compatibility stub redirecting safely to `/contact-us/thank-you`. | (Redirect) |
| `/not-found` | `Pages/NotFound.cshtml` | Utility | Custom 404 handler returning HTTP 404 | `Page Not Found` (noindex, nofollow) |
| `/Error` | `Pages/Error.cshtml` | Utility | Custom 500 handler returning HTTP 500 | `Something Went Wrong` (noindex, nofollow) |

## About Us content status (C1 reference-grounded integration)

* **Status:** Provisionally complete for the current phase with reference-grounded imagery integrated. Client review and formal approval remain required.
* **Hero Image:** `usap-about-hero-lp1017-clean-v2.png` integrated via `.internal-hero--about`.
* **Section Images:** Manufacturing & Testing Candidate C (`usap-about-manufacturing-rf-interface-lp1019-candidate-c-v2.png`) and Integrated Support (`usap-about-integrated-support-lp1112mr-clean-v2.png`) integrated into approved image frames.
* **Closing CTA:** The shared default closing CTA component (`.closing-cta--default`) is being used until page-specific variants are approved.
* **Public source URLs:**
  * `https://www.usantennaproducts.com/about-usap/`
  * `https://www.usantennaproducts.com/manufacturing/`
  * `https://www.usantennaproducts.com/testing-quality/`

## Contact Us & Request a Quote content status (C1/C2 shared hero & sidebar)

* **Status:** Provisionally complete with shared internal hero and verified contact sidebar. Forms preserved.
* **Hero Image:** `usap-contact-hero-clean-v2.png` integrated via `.internal-hero--contact`.
* **Lead Copy:**
  * Contact Us: Eyebrow: `Contact USAP`, Heading: `Contact United States Antenna Products`, Lead: `Reach our engineering, sales, and support teams to discuss your antenna requirements, project specifications, or existing installations.`
  * Request a Quote: Eyebrow: `Request a Quote`, Heading: `Request a Quote for Your Antenna System`, Lead: `Share your application and system requirements so USAP can evaluate the request and follow up with an appropriate direction.`
* **Shared Sidebar:** Implemented via `_ContactSidebar.cshtml`:
  * Verified public contact information: United States Antenna Products, LLC; 5263 Agro Drive, Frederick, MD 21703; Phone: 240-341-7120 (`tel:+12403417120`); Fax: 240-371-4980; Google Maps directions link.
  * Google Maps iframe query embed: HTTPS API-key-free query `5263 Agro Drive, Frederick, MD 21703` with lazy loading, fullscreen permission, and accessible fallback link.
  * Third-party dependency: Google Maps embed relies on external network connectivity and Google privacy policies.
* **Form Preservation:** PageModels, form bindings, validation attributes, antiforgery tokens, honeypot, reCAPTCHA, submit handlers, and rate limiting remain 100% untouched.

## Product Catalog & Product Family Routes (Checkpoint USAP-CATALOG-002-C1)

* **Landing Page (`/products`):** Full responsive landing page displaying Direction C hero asset (`usap-products-landing-hero-direction-c-candidate-v1.png`), 3×2 responsive card grid (3-column desktop, 2-column tablet, 1-column mobile) powered by dedicated `_ProductFamilyCard.cshtml` with single semantic anchor per card, no nested interactive controls, approved card display titles, full-bleed 16:10 media aspect ratio without side gutters, single Tab stop per card, coordinated hover/focus zoom and arrow feedback, actionable Engineering Guidance section featuring 3 project-lead-selected coordinated card visuals (Deployment & Mobility, Coverage & Propagation, Positioning & Infrastructure; combined wide visual deferred), 6-question Product Selection FAQ (`<details>`/`<summary>`), and closing CTA.
* **Homepage Section 5 Integration (`/`):** Section 5 utilizes dedicated `_HomeProductFamilyCard.cshtml` to feature the 4 authorized canonical families (1. Log Periodic Antennas, 2. Portable & Transportable, 3. Rotator & Control Systems, 4. Tower Systems & Accessories) in a balanced 2×2 desktop/tablet and 1×4 mobile layout. Each card retains 16:10 media presentation, single Tab stop, complete-card click activation (`stretched-link`), and direct family route destination. Section-level `View All Products` button links to `/products`.
* **Six Reserved Canonical Family Routes (Canonical 5 / 3 / 1 / 1 / 5 / 1 distribution, 16 distinct groups):**
  1. `/products/log-periodic-antennas` (5 groups: High-Power HF Log Periodics [LP-1005, LP-1001, LP-1002], LP-1017 Log Periodic, LP-1018BA Broadband Log Periodic, LP-1019 Series [LP-1019BA, LP-1019SS], LP-1112MR Transportable Log Periodic) — 8 public named models.
  2. `/products/portable-transportable-antennas` (3 groups: V-4213 Portable Discone [V-4213AD, V-4213AC], LP-1402 / LP-1403 Transportable Log Periodics [LP-1402, LP-1403], 1910 Tactical Dipoles [1910AA, 1910BA]) — 6 public named models.
  3. `/products/aperiodic-loop-antennas` (1 group: USAP Aperiodic Loop Antenna [unnamed model line; fixed and transportable loop configurations; HasPublishedModelNumber = false; 0 named models]) — 1 group record.
  4. `/products/nvis-antennas` (1 group: 1942 NVIS Series [1942-RT, 1942-TA, 1942-GM, 1942-RT-LP, 1942-TA-LP, 1942-GM-LP]) — 6 public named models. (All six configuration identifiers rendered; low-power variants published without unverified power, weight, or gain ratings).
  5. `/products/antenna-rotator-control-systems` (5 groups: R3500 Heavy Duty Antenna Rotator [R3500], R3501 Universal Rotator System [R3501], R3503 Heavy Duty Rotating System [R3503], DRC-3 Digital Rotator Controller [DRC-3], DRC-4 Digital Rotator Controller [DRC-4]) — 5 public named models. (R3500 copy scoped strictly to verified mechanical rotator facts without unconfirmed DRC-4 bundling or pairing claims).
  6. `/products/tower-systems-accessories` (1 group: T-3002 RLPA Tower System [parent record T-3002 with child configurations 3002FA, 3002FB, 3002SS, 3002SS-80]) — 5 public named models.
  * **Public Catalog Inventory:** Exactly 30 published named models across 15 groups, plus 1 unnamed Aperiodic loop system record (`HasPublishedModelNumber = false`). Total: 31 public catalog records across 16 groups.
* **Breakpoint-Sensitive Family Hero & Relocated Breadcrumbs:** Right-anchored contained image (~65% width on desktop, ~75% on tablet) seamlessly blended with a multi-background overlay combining left-anchored navy text protection and a top-biased vertical dissolve (C1R10 / Candidate D). Zero cropping or letterboxing; readable copy and CTAs across 1920px, 1440px, 1024px, 768px, 767px, and 390px viewports. Semantic breadcrumb navigation (`<nav aria-label="Breadcrumb">`) is positioned in the main content container immediately below the hero, aligned to the top-left edge.
* **Product Group Cards & Always-Visible Overview Presentation (C1R4 Refinement):**
  - Outer semantic `<article class="product-group-card" id="@group.SectionAnchor">` combining an always-visible overview with a nested native `<details class="product-group-disclosure">`.
  - Always-visible overview displays the associated product image (standard cover or contain-style for LP-1112MR, 1910, 1942), product-group name, short summary, expanded introduction (when providing distinct context), configuration/model count badges, technical resource badge, and up to three key group characteristics at all times.
  - The product image and primary group identity remain visible whether the disclosure is collapsed or expanded.
  - Nested native `<details class="product-group-disclosure">` provides progressive disclosure for deeper information:
    - Summary control clearly labeled `View Models & Specifications` / `Hide Models & Specifications` (`View Configuration & Technical Details` / `Hide Configuration & Technical Details` for Aperiodic), accessible with assistive technology context (`for @group.Name`).
    - Expanded content presents additional group characteristics (when total > 3), model cards grid (`.product-models-grid`), Aperiodic system configuration notes, approved interim technical resources with PDF badges, and Contact Engineering inquiry link (`/contact-us`).
  - Model cards (`.product-model-card`) render model code (`<code>`), display name/configuration role, brief description, and model-level specifications (`<dl class="product-specs-list">`).
  - Restrained neutral visual styling: routine red top borders and default navy left accents are removed from ordinary product cards and disclosures, reserved only for intentional emphasis components.
* **Product Selection FAQ Full-Width Layout (C1R4 Refinement):**
  - The `.faq-list` on `/products` spans the full standard `.container` width (`width: 100%`), eliminating awkward narrow-column spacing.
  - Expanded answer copy is constrained internally (`max-width: 75ch`) on paragraph elements to preserve optimal readability line lengths.
* **Canonical Catalog Inventory (30 + 1 Reconciliation):**
  - Exactly 6 families, 16 product groups, 30 named models/configurations, 1 unnamed Aperiodic group record = 31 total catalog records.
  - Erroneous temporary identifiers introduced in C1R3 markdown (`lp-1112`, `lp-3001`, `1925`, `1940`, `r3505`, `r3506`, `1942-1..6`, `R3501-1`, `R3503-1`, `T-3002-30..70`) are explicitly rejected and excluded from public content and documentation.
* **Complete Product-Group Visual Coverage:** All 16 product groups feature intentional visuals: the 3 existing approved-for-provisional-use rotator visuals (`r3500`, `drc-3`, `drc-4`) plus the 13 project-lead-selected A1 source-guided candidates (`lp-high-power`, `lp-1017`, `lp-1018ba`, `lp-1019`, `lp-1112mr`, `v-4213`, `lp-1402-1403`, `1910`, `aperiodic`, `1942`, `r3501`, `r3503`, `t-3002`). Wide-span assets (`lp-1112mr`, `1910`, `1942`) utilize contain-style responsive presentation to prevent aggressive cropping.
* **Family Closing Sequence & Configuration Support Action (C1R5 Refinement):**
  - Sequence order on all family pages: (1) Product Groups -> (2) Configuration Support notice (`.configuration-support-notice`) -> (3) Return to All Product Families (`.family-back-nav`) -> (4) Closing CTA -> (5) Site Footer.
  - Return to All Product Families navigation control is repositioned below Configuration Support and above the closing CTA, retaining secondary `.btn-outline` treatment.
  - Action button inside Configuration Support upgraded to shared primary red button (`.btn-primary`), promoting direct conversion for technical application planning without duplicate button declarations.
* **Header Product-Family Dropdown Navigation (C1R5 Refinement):**
  - Desktop and mobile site headers (`_Header.cshtml`) implement an accessible `<details id="nav-products-dropdown">` / `<summary class="nav-dropdown-toggle">` disclosure menu.
  - Submenu provides direct access to All Products (`/products`) plus the six canonical family routes (`/products/log-periodic-antennas`, `/products/portable-transportable-antennas`, `/products/aperiodic-loop-antennas`, `/products/nvis-antennas`, `/products/antenna-rotator-control-systems`, `/products/tower-systems-accessories`).
  - Products navigation state is highlighted (`is-active`, `aria-current="page"`) across `/products` and all six family routes.
  - Operable via mouse, touch, and keyboard (Enter/Space, Tab, Escape to dismiss and focus summary), with outside-click dismissal on desktop. Mobile drawer allows expanding/collapsing dropdown without unintended menu closure. Fully functional without JavaScript. Footer retains single top-level `/products` link without duplicate family links.
* **Hero Resilience, Height Normalization, Ultra-Wide Composition, and Horizontal Blend Architecture (C1R5/C1R9/C1R10/C1R11/C1R12/C1R13):**
  - Internal-page heroes (`.internal-hero`, `.internal-hero--family`, `.internal-hero--products`) standardize on a shared desktop/tablet baseline (`--internal-hero-min-height: 24.2875rem;` / ~388.6px).
  - Under C1R10, `.internal-hero--about` uses normalized `padding-block: 1.75rem;` (28px) at desktop/tablet widths, achieving exact 388.59px height parity while preserving comfortable breathing room (~28.8px top / ~29.8px bottom).
  - Under C1R11/C1R12, ultra-wide viewports (`@media (min-width: 100rem)` / ~1600px) implement Candidate B right-pinned media (`right: 0`) paired with moderate responsive growth (`min-height: clamp(24.2875rem, 22vw, 28rem);` and `width: clamp(65rem, 59vw, 75rem);`), refined under C1R12 with Candidate H1 softened horizontal text-protection gradient (`0` to `+30rem` solid navy, `+40rem` 0.88, `+50rem` 0.58, `+62rem` 0.24, transparent at `+72rem`). Imagery terminates at the physical right browser edge with zero navy gutter, while full 8:3 source composition emerges smoothly without perceived tonal seams across copy-to-image transitions.
  - Under C1R13-R1, the Rotator & Control Systems hero (`/products/antenna-rotator-control-systems`) incorporates an asset-specific entry mask (`mask-image` over the left 12rem) at `@media (min-width: 100rem)` to dissolve the sharp rectangular left entrance of its technical blueprint asset into the navy hero canvas, with zero impact on other hero routes or standard desktop/tablet/mobile viewports.
  - Height remains `height: auto` without forced overflow clipping, scaling gracefully under browser zoom (100%, 125%, 150%, 200%). Breadcrumbs remain strictly below family heroes.


* **Disputed Specifications Policy:** Specifications on hold are withheld and replaced with restrained engineering guidance. Internal governance metadata (`ConflictHolds`, `SourceNotes`, `ApprovalStatus`, etc.) is never exposed publicly.
* **Canonical Technical Documents:** Under `USAP-TECHDOC-001`, the interim 6 external document links were superseded by the complete 17-document canonical first-party library hosted locally under `wwwroot/documents/technical/`. All 16 product groups reference local canonical paths with zero WordPress dependencies. Groups with multiple canonical documents (e.g. `v-4213`) associate both documents cleanly.
* **Slug Validation & True 404:** Slugs are strictly validated against `IProductCatalogService.GetFamilyBySlug(familySlug)`. Unrecognized slugs return HTTP 404.
* **Legacy Public Redirects:** All 34 known legacy WordPress PDF URLs are permanently redirected (HTTP 301) via `LegacyDocumentRedirectMiddleware` to their respective canonical local documents under `/technical-resources/documents/`. Legacy category/product page redirects remain scheduled for Milestone C3.

## Technical Resources Library (USAP-TECHDOC-001 / USAP-TECHDOC-001-R1 / USAP-TECHDOC-001-R2)

The `/technical-resources` route provides an accessible, searchable, and category-filtered public document library:
* **Direct Public Access:** All 17 canonical documents are directly viewable and downloadable without a lead capture gate.
* **Canonical Document Corpus:** Exactly 17 first-party PDFs migrated from R2 research package, organized by product family.
* **Search & Filter Controls:** Right-aligned contained desktop search toolbar (max-width 32rem) with visible submit button, keyword search across title/family/models/description, and 6 product-family category filter pills plus `All` with real-time document counts. Full width on mobile/tablet.
* **Homepage Deep-Link Taxonomy:** Reconciled entry points supporting `type=data-sheet`, `view=antenna-systems`, `category=antenna-rotator-control-systems`, and `category=tower-systems-accessories`, with an active filter badge and clear action. Redundant reset controls suppressed.
* **Card Footers & Atomic Page Counts:** Restored visual organization with restrained document SVG icon and atomic `nowrap` page indicators (`1 page`, `2 pages`), paired with compact `View Document` and `Download PDF` action buttons fitting on a single row in 3-column desktop layout.
* **Link State Protection:** Explicit `:link`, `:visited`, `:hover`, and `:focus-visible` styling ensures card titles, metadata, and breadcrumbs remain dark navy and branded red, with zero browser-default purple link colors.
* **Branded HTML Document Detail Routes (`/technical-resources/document/{slug}`):** Data-driven detail pages for all 17 canonical documents featuring H1 title, relocated semantic breadcrumbs below hero matching Product Family architecture, metadata, covered models, primary download/open actions, related product group links, and native browser embedded PDF preview (`<object>` / `<iframe>`). Redundant Back button removed.
* **Progressive Enhancement:** 100% functional without JavaScript. Server-side GET filtering provides baseline access, while unobtrusive JavaScript adds instant client-side updates, live region status announcements, and smooth filter resets.
* **Hero Artwork & Controlled Closing CTA:** Hero image `usap-technical-resources-hero-requirements-document-candidate-b-v1.png` is integrated into the shared `.internal-hero` architecture. Standard USAP closing CTA (`.closing-cta--default`) with approved controlled copy ("Ready to Discuss Your Antenna Requirements?") precedes the footer across library and detail routes. Mobile-specific hero derivative remains deferred.


## Forms

Both `/contact-us` and `/request-a-quote` will confirm via `/thank-you` (provisional). Final behavior requires implementation approval in Forms & Search. Antiforgery and server-side validation are required.

## Custom 404

Implemented using a dedicated `/not-found` Razor Page and `UseStatusCodePagesWithReExecute`. Unrecognized routes correctly return a branded page with HTTP 404.

## Legal routes

`/privacy-policy` and `/terms` are not created. Routes may be created once approved legal text is supplied. Placeholder legal pages are not authorized.

## Content status and outstanding inputs

See `docs/CONTENT_AND_ASSETS.md`.

## Search Indexing and Sitemap

A static XML sitemap defines exactly six provisional routes (`/`, `/products`, `/technical-resources`, `/about-us`, `/contact-us`, `/request-a-quote`).

**Excluded routes:**
* `/thank-you` (`noindex, nofollow`).
* `/not-found` (`noindex, nofollow`).
* `/Error` (`noindex, nofollow`).
* Arbitrary placeholder or invalid product-family routes and all PDF/attachment URLs.

**Indexable Product Family Routes (C1R6):**
The six canonical `/products/{familySlug}` routes omit page-level `Robots` metadata, allowing production search engines to index and follow them (`index, follow` default). Non-production environments (development and staging) are protected against indexing globally via the `X-Robots-Tag: noindex, nofollow` HTTP response header middleware in `Program.cs`.

**Important:**
* Invalid product-family slugs (e.g., `/products/this-is-not-real`) return HTTP 404 via `IProductCatalogService.GetFamilyBySlug` validation in `Family.cshtml.cs`.
* The sitemap and structured-data values are provisional and **must be reviewed again before production launch**.

## Product-Family SEO & Heading Register (C1R6)

| Route | Rendered `<title>` | Meta Description | Section Eyebrow | Post-Hero H2 | Production Robots |
|---|---|---|---|---|---|
| `/products/log-periodic-antennas` | `Log Periodic Antennas — United States Antenna Products` | Broadband directional log periodic antenna models, configurations, published operating characteristics, and technical documentation from United States Antenna Products. | PRODUCT MODELS & CONFIGURATIONS | Log Periodic Antenna Models & Configurations | `index, follow` (default) |
| `/products/portable-transportable-antennas` | `Portable & Transportable Antenna Systems — United States Antenna Products` | Documented portable and transportable antenna configurations including discone, log periodic, and tactical dipole models from United States Antenna Products. | PRODUCT MODELS & CONFIGURATIONS | Portable & Transportable Antenna Models | `index, follow` (default) |
| `/products/aperiodic-loop-antennas` | `Aperiodic Loop Antennas — United States Antenna Products` | Broadband aperiodic loop receiving antenna system, published operating characteristics, configuration guidance, and technical documentation from United States Antenna Products. | SYSTEM CONFIGURATION & TECHNICAL DETAILS | Aperiodic Loop Antenna System & Configuration Details | `index, follow` (default) |
| `/products/nvis-antennas` | `1942 NVIS Antennas — United States Antenna Products` | Published 1942 Near Vertical Incidence Skywave (NVIS) antenna identifiers covering rooftop, transportable, and ground-mount configurations from United States Antenna Products. | PRODUCT MODELS & CONFIGURATIONS | 1942 NVIS Antenna Models & Configurations | `index, follow` (default) |
| `/products/antenna-rotator-control-systems` | `Antenna Rotator & Control Systems — United States Antenna Products` | Antenna rotator and controller models, published positioning and control information, and documented system relationships from United States Antenna Products. | POSITIONING & CONTROL EQUIPMENT | Antenna Rotator & Controller Models | `index, follow` (default) |
| `/products/tower-systems-accessories` | `T-3002 Tower Systems & Accessories — United States Antenna Products` | T-3002 tower-system models, published structural configurations, rotation specifications, and accessory information from United States Antenna Products. | SYSTEM MODELS & ACCESSORIES | T-3002 Tower System Models & Accessories | `index, follow` (default) |

## Shared Navigation & Products Hero Baseline Status (C1R8)

- **Sitewide Always-Visible Sticky Header**: Deployed across all pages (`position: sticky; top: 0;`). Kept visible continuously across all scroll directions during normal operation; all scroll-direction hide/reveal logic and idle restoration timers removed per project-lead direction. While mobile navigation scroll lock is active (`.nav-open-lock`), temporarily viewport-pinned via `position: fixed; top: 0; left: 0; right: 0; width: 100%;` to maintain top-of-viewport anchoring while background scrolling is locked.
- **Content-Height Mobile Navigation**: All six top-level controls centered within the mobile drawer; drawer terminates below Request a Quote button + padding; translucent backdrop dismisses navigation.
- **Route Validation**: Verified HTTP 200 across all canonical routes (`/`, `/about-us`, `/technical-resources`, `/contact-us`, `/request-a-quote`, `/products`, and all six family routes), and verified true HTTP 404 for invalid family slugs (e.g., `/products/non-existent-family-xyz`).

## Shared Internal-Hero Foundation & Full-Image Presentation Status (C1R9)

- **Unified Desktop/Tablet Height Baseline**: Reconciled the shared internal hero foundation to establish a shared desktop/tablet minimum-height (`--internal-hero-min-height: 24.2875rem;` / ~388.6px), derived from the product-family hero baseline at 1440×900.
- **Height Parity across Normal Content**: Normal image-backed internal heroes across all routes (`/products`, `/contact-us`, `/request-a-quote`, and all six family routes) resolve to exact height parity (388.6px, 0.0px variance at 1440×900 and 1920×1080).
- **Full-Image Right-Side Media Containment**: Non-family heroes (`/products`, `/about-us`, `/contact-us`, `/request-a-quote`) transition from destructive full-width `object-fit: cover` to bounded right-side media containment (`width: 65%; max-width: 1100px; height: 100%; object-fit: contain; object-position: right center;` at desktop; `width: 75%` at tablet). Full 8:3 source compositions are visible with zero vertical cropping.
- **Fluid Expansion for Longer Content**: Preserves `height: auto` without max-height constraints. Heroes with extensive copy (e.g. `/about-us` at 427.1px) expand naturally without clipping under browser zoom (125%, 200%) or accessibility text scaling.
- **Atmospheric Mobile Fallback**: Compact mobile screens (< 48rem) retain fluid height and display media as an atmospheric background under a 90% navy contrast overlay (`rgba(13, 27, 46, 0.90)`). Zero horizontal overflow across all viewports.

## Redirect map

See `docs/REDIRECT_MAP.md` for the current legacy URL mapping strategy and rules. Redirect implementation remains deferred.
