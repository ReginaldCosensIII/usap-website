# Routes and Content

## Route map

| Route | Page | Status | Notes | Metadata & Indexing |
|---|---|---|---|---|
| `/` | `Pages/Index.cshtml` | Provisional | Pending client copy, design, imagery | `HF, VHF & UHF Antenna Systems` (index, follow) |
| `/products` | `Pages/Products/Index.cshtml` | Stub | Up to six approved product families — pending client data | `Antenna Products & Systems` (index, follow) |
| `/products/{familySlug}` | `Pages/Products/Family.cshtml` | Stub | Pending product data per family | **PROVISIONAL**: `Antenna Product Family` (noindex, follow). Must be replaced when catalog content is implemented. Valid slugs will become indexable later. |
| `/technical-resources` | `Pages/TechnicalResources.cshtml` | Stub | Pending approved documents and organization | `Technical Resources` (index, follow) |
| `/about-us` | `Pages/AboutUs.cshtml` | Stub | Pending client copy and imagery | `About Us` (index, follow) |
| `/contact-us` | `Pages/ContactUs.cshtml` | Provisional | Pending client copy and imagery | `Contact Us` (index, follow) |
| `/request-a-quote` | `Pages/RequestAQuote.cshtml` | Provisional | Pending client copy and imagery | `Request a Quote` (index, follow) |
| `/thank-you` | `Pages/ThankYou.cshtml` | Provisional stub | Form behavior, redirect logic, and copy require approval | `Thank You` (noindex, nofollow) |
| `/not-found` | `Pages/NotFound.cshtml` | Utility | Custom 404 handler returning HTTP 404 | `Page Not Found` (noindex, nofollow) |
| `/Error` | `Pages/Error.cshtml` | Utility | Custom 500 handler returning HTTP 500 | `Something Went Wrong` (noindex, nofollow) |

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
