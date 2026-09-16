# Routes and Content

## Route map

| Route | Page | Status | Notes | Metadata & Indexing |
|---|---|---|---|---|
| `/` | `Pages/Index.cshtml` | Provisional | Section 5 integrates shared `_ProductFamilyCard.cshtml` featuring 4 canonical families via `IProductCatalogService`, linking to `/products/{slug}` with section-level `View All Products` linking to `/products`. | `HF, VHF & UHF Antenna Systems` (index, follow) |
| `/products` | `Pages/Products/Index.cshtml` | Provisional (USAP-CATALOG-001-C2) | Structured 6-family catalog landing page with Direction C hero, unified shared card grid (3-column), Candidate B rotator card, 3 actionable guidance pathways with inline continuation, 6-question FAQ, and closing CTA. | `Antenna Products & Systems` (index, follow) |
| `/products/{familySlug}` | `Pages/Products/Family.cshtml` | Provisional (USAP-CATALOG-001-C2) | Six canonical family routes with responsive `<picture>` hero (1536×576 desktop / 768×768 mobile) using `(max-width: 47.999rem)` boundary, copy-safe overlay, per-family focal positions, audited product groups, and dedicated Rotator hardware visuals section. Slug validation returns HTTP 404 for invalid slugs. | `[Family Name]` (noindex, follow) |
| `/technical-resources` | `Pages/TechnicalResources.cshtml` | Stub | Pending approved documents and organization | `Technical Resources` (index, follow) |
| `/about-us` | `Pages/AboutUs.cshtml` | Provisional | Reference-grounded imagery integrated; shared hero and closing CTA; client approval pending | `About Us` (index, follow) |
| `/contact-us` | `Pages/ContactUs.cshtml` | Provisional | Shared internal hero and sidebar with verified contact details and Google Maps embed; form behavior preserved | `Contact Us` (index, follow) |
| `/request-a-quote` | `Pages/RequestAQuote.cshtml` | Provisional | Shared internal hero and sidebar with verified contact details and Google Maps embed; form behavior preserved | `Request a Quote` (index, follow) |
| `/thank-you` | `Pages/ThankYou.cshtml` | Provisional stub | Form behavior, redirect logic, and copy require approval | `Thank You` (noindex, nofollow) |
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

## Product Catalog & Product Family Routes (Checkpoint USAP-CATALOG-001-C2R)

* **Landing Page (`/products`):** Full responsive landing page refined in Checkpoint `USAP-CATALOG-001-C2R`. Displays Direction C hero asset (`usap-products-landing-hero-direction-c-candidate-v1.png`), 3×2 responsive card grid (3-column desktop, 2-column tablet, 1-column mobile) powered by dedicated `_ProductFamilyCard.cshtml` with approved card display titles, 16:10 media aspect ratio, single Tab stop per card, actionable Engineering Guidance section with subtle dot-grid background and inline SVGs, 6-question Product Selection FAQ (`<details>`/`<summary>`), and closing CTA.
* **Homepage Section 5 Integration (`/`):** Section 5 utilizes dedicated `_HomeProductFamilyCard.cshtml` to feature the 4 authorized canonical families (1. Log Periodic Antennas, 2. Portable & Transportable, 3. Rotator & Control Systems, 4. Tower Systems & Accessories) in a balanced 2×2 desktop/tablet and 1×4 mobile layout. Each card retains 16:10 media presentation, single Tab stop, complete-card click activation (`stretched-link`), and direct family route destination. Section-level `View All Products` button links to `/products`.
* **Six Reserved Canonical Family Routes (Canonical 5 / 3 / 1 / 1 / 5 / 1 distribution, 16 distinct groups):**
  1. `/products/log-periodic-antennas` (5 groups: High-Power HF Log Periodics [LP-1005, LP-1001, LP-1002], LP-1017 Log Periodic, LP-1018BA Broadband Log Periodic, LP-1019 Series [LP-1019BA, LP-1019SS], LP-1112MR Transportable Log Periodic)
  2. `/products/portable-transportable-antennas` (3 groups: V-4213 Portable Discone [V-4213AD, V-4213AC], LP-1402 / LP-1403 Transportable Log Periodics [LP-1402, LP-1403], 1910 Tactical Dipoles [1910AA, 1910BA])
  3. `/products/aperiodic-loop-antennas` (1 group: USAP Aperiodic Loop Antenna [unnamed model line; fixed and transportable loop configurations])
  4. `/products/nvis-antennas` (1 group: 1942 NVIS Series [1942-RT, 1942-TA, 1942-GM; low-power variants 1942-RT-LP, 1942-TA-LP, 1942-GM-LP on conflict hold])
  5. `/products/antenna-rotator-control-systems` (5 groups: R3500 Heavy Duty Rotator with DRC-4 Rotator Control Unit, R3501 Universal Rotator System, R3503 Heavy Duty Rotating System, DRC-3 Digital Rotator Controller, DRC-4 Digital Rotator Controller)
  6. `/products/tower-systems-accessories` (1 group: T-3002 RLPA Tower System [parent record with child configurations 3002FA, 3002FB, 3002SS, 3002SS-80])
* **Breakpoint-Sensitive Family Hero:** Right-anchored contained image (~65% width on desktop, ~75% on tablet) seamlessly blended with a left-anchored navy gradient overlay. Zero cropping or letterboxing; readable copy and CTAs across 1920px, 1440px, 1024px, 768px, 767px, and 390px viewports.
* **Product Group Disclosures:** Implemented as native semantic `<details class="product-group-disclosure">` with `<summary class="product-group-summary">` and inner `.product-group-content` wrapper. Enhanced with zero-dependency CSS/Web Animations API (`disclosure.js`) respecting `prefers-reduced-motion`.
* **Associated Asset Integration:** Rotator product visual assets are rendered directly inside their respective disclosures (`r3500`, `drc-3`, `drc-4`) via the non-destructive `AssociatedAsset` property on `ProductGroupRecord`. The 13 non-rotator groups render no image frames.
* **Neutral Configuration Support Notice:** Replaced previous warning banner with an authoritative, neutral `Configuration Support` panel directing inquiries to `/contact-us`.
* **Disputed Specifications Policy:** Specifications on hold (`HoldDisputedSpecs`) are withheld and replaced with restrained engineering guidance. Internal governance metadata (`ConflictHolds`, `SourceNotes`) is never exposed publicly.
* **Technical Document Allowlist:** Provisional publication restricted to explicit 6-item allowlist (`doc-lp-high-power`, `doc-lp-1018ba`, `doc-lp-1019`, `doc-1910-2024`, `doc-aperiodic`, `doc-t-3002-oct2016`).
* **Slug Validation & True 404:** Slugs are strictly validated against `IProductCatalogService.GetFamilyBySlug(familySlug)`. Unrecognized slugs return HTTP 404.
* **Legacy Public Redirects (Deferred to C3):** Legacy WordPress category and product URLs from the current public site are mapped in the research workbook and will be implemented in Milestone C3.

## Technical Resources scope

Fixed-scope baseline: organized static resource library with approved documents, categories, and organization.
* The selected Technical Resources hero image candidate (`usap-technical-resources-hero-requirements-document-candidate-b-v1.png`) is stored in `wwwroot/images/technical-resources/` but remains strictly unimplemented and unreferenced during C1.
* Advanced search and filtering is separately authorized and is not included in the fixed 48-hour scope.

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
* `/products/{familySlug}` (currently `noindex, follow`).
* `/thank-you` (`noindex, nofollow`).
* `/not-found` (`noindex, nofollow`).
* `/Error` (`noindex, nofollow`).
* Arbitrary placeholder or invalid product-family routes and all PDF/attachment URLs.

**Important:**
* Invalid product-family slugs (e.g., `/products/this-is-not-real`) currently return HTTP 200 OK because `Family.cshtml.cs` blindly binds the route data. This is a known soft-404 defect that remains until the catalog implementation adds slug validation.
* The sitemap and structured-data values are provisional and **must be reviewed again before production launch**.

## Redirect map

See `docs/REDIRECT_MAP.md` for the current legacy URL mapping strategy and rules. Redirect implementation remains deferred.
