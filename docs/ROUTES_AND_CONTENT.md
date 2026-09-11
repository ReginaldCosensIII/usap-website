# Routes and Content

## Route map

| Route | Page | Status | Notes | Metadata & Indexing |
|---|---|---|---|---|
| `/` | `Pages/Index.cshtml` | Provisional | Pending client copy, design, imagery | `HF, VHF & UHF Antenna Systems` (index, follow) |
| `/products` | `Pages/Products/Index.cshtml` | Stub | Up to six approved product families — pending client data | `Antenna Products & Systems` (index, follow) |
| `/products/{familySlug}` | `Pages/Products/Family.cshtml` | Stub | Pending product data per family | **PROVISIONAL**: `Antenna Product Family` (noindex, follow). Must be replaced when catalog content is implemented. Valid slugs will become indexable later. |
| `/technical-resources` | `Pages/TechnicalResources.cshtml` | Stub | Pending approved documents and organization | `Technical Resources` (index, follow) |
| `/about-us` | `Pages/AboutUs.cshtml` | Provisional | Provisional migration copy sourced from public website; client approval and final imagery pending | `About Us` (index, follow) |
| `/contact-us` | `Pages/ContactUs.cshtml` | Provisional | Pending client copy and imagery | `Contact Us` (index, follow) |
| `/request-a-quote` | `Pages/RequestAQuote.cshtml` | Provisional | Pending client copy and imagery | `Request a Quote` (index, follow) |
| `/thank-you` | `Pages/ThankYou.cshtml` | Provisional stub | Form behavior, redirect logic, and copy require approval | `Thank You` (noindex, nofollow) |
| `/not-found` | `Pages/NotFound.cshtml` | Utility | Custom 404 handler returning HTTP 404 | `Page Not Found` (noindex, nofollow) |
| `/Error` | `Pages/Error.cshtml` | Utility | Custom 500 handler returning HTTP 500 | `Something Went Wrong` (noindex, nofollow) |

## About Us content status (C11 provisional completion)

* **Status:** Provisionally complete for the current phase. Existing public-source company content has been used without unnecessary repetition. Client review and formal approval remain required.
* **Closing CTA:** The shared default closing CTA component (`.closing-cta--default`) is being used until page-specific variants are approved.
* **Provisional Assets:** Provisional About images remain scheduled for replacement; they are development placeholders only and are not production-approved. Additional section texture accents remain deferred.
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

## Technical Resources scope

Fixed-scope baseline: organized static resource library with approved documents, categories, and organization.
**Advanced search and filtering is separately authorized** and is not included in the fixed 48-hour scope.

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
