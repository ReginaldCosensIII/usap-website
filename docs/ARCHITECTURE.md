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
        │   ├── Shared/       _Layout.cshtml, _Navigation.cshtml
        │   ├── Index.cshtml          /
        │   ├── AboutUs.cshtml        /about-us
        │   ├── ContactUs.cshtml      /contact-us
        │   ├── RequestAQuote.cshtml  /request-a-quote
        │   ├── TechnicalResources.cshtml  /technical-resources
        │   ├── ThankYou.cshtml       /thank-you (provisional)
        │   └── Error.cshtml
        ├── Properties/launchSettings.json
        ├── wwwroot/css/site.css   (minimal structural shell)
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

## CSS shell

`body`: `min-height: 100vh; display: flex; flex-direction: column`
`main`: `flex: 1; width: 100%`
No `overflow-x: hidden`. Layout integrity maintained through proper widths.

## Deferred decisions

| Decision | Deferred to |
|---|---|
| Static content data format and location | Catalog & Resources |
| Custom 404/error handling | Core Site Build |
| Design tokens, typography, color system | Core Site Build |
| SMTP/email service | Forms & Search |
| Analytics (GA4), sitemap, robots.txt | Forms & Search |
| Advanced Technical Resources filtering | Separately authorized |
| Legacy URL redirect map | When redirect mapping begins |

## IIS deployment

CES Dev: SDK `10.0.400` + ASP.NET Core Hosting Bundle.
Production: Hosting Bundle only (no SDK required for published output).
Details in `docs/DEPLOYMENT_RUNBOOK.md`.
