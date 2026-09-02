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
        │   ├── css/site.css          design system tokens, layout, components
        │   ├── js/site-navigation.js  dependency-free mobile nav module
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

Mobile navigation uses a CSS disclosure pattern:
- Nav list is visible by default (no-JS fallback).
- JavaScript adds `js-nav-ready` to `<html>`, which CSS uses to reveal the toggle and hide the list.
- The toggle manages `aria-expanded`, `hidden`, and `aria-label` on the nav list ID (`primary-nav-list`).
- Desktop navigation activates at `64rem` (1024px); mobile disclosure applies below that.
- No focus trap, no body-scroll lock, no external library.
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
- **Deferred Validation**: Product-family soft-404 validation remains deferred.

## Deferred decisions

| Decision | Deferred to |
|---|---|
| Static content data format and location | Catalog & Resources |
| SMTP/email service | Forms & Search (Currently simulated in Development with `DevelopmentInquirySubmissionService`; unavailable elsewhere) |
| Analytics (GA4) | Forms & Search |
| Advanced Technical Resources filtering | Separately authorized |
| Legacy URL redirect map | The initial redirect map now exists. Redirect implementation remains deferred until the catalog/resource mapping and IIS review are complete. |

## Forms and Validation

- Forms use native ASP.NET Core `DataAnnotations` and `IValidatableObject` for server-side validation.
- JavaScript provides only progressive enhancement (conditional field visibility). Server validation remains authoritative.
- Submission endpoints (`/contact-us`, `/request-a-quote`) are protected by a POST-only rate limit partitioned by IP.

## IIS deployment

CES Dev: SDK `10.0.400` + ASP.NET Core Hosting Bundle.
Production: Hosting Bundle only (no SDK required for published output).
Details in `docs/DEPLOYMENT_RUNBOOK.md`.
