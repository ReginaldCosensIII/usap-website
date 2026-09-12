# Routes and Content

## Route map

| Route | Page | Status | Notes | Metadata & Indexing |
|---|---|---|---|---|
| `/` | `Pages/Index.cshtml` | Provisional | Pending client copy, design, imagery | `HF, VHF & UHF Antenna Systems` (index, follow) |
| `/products` | `Pages/Products/Index.cshtml` | Stub | Up to six approved product families — pending client data | `Antenna Products & Systems` (index, follow) |
| `/products/{familySlug}` | `Pages/Products/Family.cshtml` | Stub | Pending product data per family | **PROVISIONAL**: `Antenna Product Family` (noindex, follow). Must be replaced when catalog content is implemented. Valid slugs will become indexable later. |
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
  * `https://www.usantennaproducts.com/antenna-category/rotator-systems/`
  * `https://www.usantennaproducts.com/antenna-category/digital-rotator-controller/`
  * `https://www.usantennaproducts.com/antenna-category/tower-systems-accessories/`
* **Company Overview narrative:**
  * Lead: `"United States Antenna Products, LLC develops antenna and control solutions for military, government, and commercial communication requirements."`
  * Supporting paragraph 1: `"Beyond its antenna products, USAP offers rotator systems, digital rotator controllers, tower systems, and related accessories. Its published rotator-control capabilities support antenna operation from locations with a secure internet connection."`
  * Supporting paragraph 2: `"Support for purchased systems includes phone assistance and access to manuals and software without additional charges. USAP describes dedicated, personalized service as a core part of its business."`
* **Integrated Support narrative:**
  * Lead: `"USAP supports complete antenna projects rather than limiting its role to individual products. Its integrated services extend from evaluating customer requirements through system construction, installation design, and installation."`
* **Missing client information (requires client input before final expansion):**
  * Founding date and company origin
  * Founder or leadership history
  * Ownership milestones
  * Major company milestones
  * Facility and manufacturing details
  * Testing and quality-control processes
  * Certifications or standards
  * Approved government, military, or commercial project examples
  * Approved customer history or case studies
  * These items must not be invented and remain pending client delivery.

## Contact Us and Request a Quote content status (C1/C2 checkpoints)

* **Status:** Shared internal hero and two-column responsive form/sidebar layout implemented.
* **Hero Imagery:**
  * Contact Us: Revised conceptual hero Candidate C (`usap-contact-hero-communication-signal-candidate-c-v1.png`, SHA-256: `B020AD39E6A8CEFD15C584DCA9B77503070CDE8FD0C33B63C6A556F645CD71E4`) integrated via `.internal-hero--contact`. Replaces medical-appearing ECG/EKG waveform with clean communication signal bars. Provenance copy in `project-input/USAP_Contact_Hero_Revision_2026-09-12/`. Pending USAP visual approval.
  * Request a Quote: Conceptual hero Candidate A (`usap-quote-hero-technical-planning-candidate-a-v1.png`) integrated via `.internal-hero--quote`.
* **Hero Content:** Provisional migration copy applied.
  * Contact Us: Eyebrow: `Contact USAP`, Heading: `Contact United States Antenna Products`, Lead: `Connect with USAP about antenna products, technical support, or general questions.`
  * Request a Quote: Eyebrow: `Request a Quote`, Heading: `Request a Quote for Your Antenna System`, Lead: `Share your application and system requirements so USAP can evaluate the request and follow up with an appropriate direction.`
* **Shared Sidebar:** Implemented via `_ContactSidebar.cshtml`:
  * Verified public contact information: United States Antenna Products, LLC; 5263 Agro Drive, Frederick, MD 21703; Phone: 240-341-7120 (`tel:+12403417120`); Fax: 240-371-4980; Google Maps directions link.
  * Google Maps iframe query embed: HTTPS API-key-free query `5263 Agro Drive, Frederick, MD 21703` with lazy loading, fullscreen permission, and accessible fallback link.
  * Third-party dependency: Google Maps embed relies on external network connectivity and Google privacy policies.
* **Form Preservation:** PageModels, form bindings, validation attributes, antiforgery tokens, honeypot, reCAPTCHA, submit handlers, and rate limiting remain 100% untouched.

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
