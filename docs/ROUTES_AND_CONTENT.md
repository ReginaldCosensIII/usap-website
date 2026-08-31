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
| `/Error` | `Pages/Error.cshtml` | Minimal stub | Custom error/404 deferred to Core Site Build | `Error` (noindex, nofollow) |

## Technical Resources scope

Fixed-scope baseline: organized static resource library with approved documents, categories, and organization.
**Advanced search and filtering is separately authorized** and is not included in the fixed 48-hour scope.

## Forms

Both `/contact-us` and `/request-a-quote` will confirm via `/thank-you` (provisional). Final behavior requires implementation approval in Forms & Search. Antiforgery and server-side validation are required.

## Custom 404

Deferred to Core Site Build. Unrecognized routes currently return a plain ASP.NET Core error response.

## Legal routes

`/privacy-policy` and `/terms` are not created. Routes may be created once approved legal text is supplied. Placeholder legal pages are not authorized.

## Content status and outstanding inputs

See `docs/CONTENT_AND_ASSETS.md`.

## Redirect map

`docs/REDIRECT_MAP.md` does not exist yet. It will be created when legacy URL analysis begins.
