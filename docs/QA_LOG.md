# QA Log — Runtime and Quality Assurance Record

This document records runtime defects, environmental conditions, diagnostic findings, and verification outcomes for the USAP website rebuild.

---

## QA-001 — Transient `BadImageFormatException` (0x8007000B) during Development Server Execution

* **Date Reported:** 2026-09-09
* **Milestone / Task:** USAP-ABOUT-001-C7 / USAP-ABOUT-001-C8
* **Impacted Routes:** `/about-us` (transient HTTP 500)
* **Status:** Resolved (diagnosed as transient compilation/binary mismatch caused by unsafe file operations during active server execution; verified clean in C8).

### 1. Incident Description and Stack Trace

During the C7 implementation pass, an HTTP request to `https://localhost:7012/about-us` returned an HTTP 500 Internal Server Error with the following exception:

```text
System.BadImageFormatException: An attempt was made to load a program with an incorrect format. (0x8007000B)
   at System.Signature.Init(ObjectHandleOnStack _this, Void* pCorSig, Int32 cCorSig, RuntimeFieldHandleInternal fieldHandle, RuntimeMethodHandleInternal methodHandle)
   at System.Signature.Init(ObjectHandleOnStack _this, Void* pCorSig, Int32 cCorSig, RuntimeFieldHandleInternal fieldHandle, RuntimeMethodHandleInternal methodHandle)
   at System.Signature..ctor(Void* pCorSig, Int32 cCorSig, RuntimeType declaringType)
   at System.Reflection.RuntimePropertyInfo.get_Signature()
   at System.Reflection.RuntimePropertyInfo.EqualsSig(RuntimePropertyInfo target)
   at System.RuntimeType.RuntimeTypeCache.MemberInfoCache`1.PopulateProperties(Filter filter)
   at System.RuntimeType.RuntimeTypeCache.MemberInfoCache`1.GetListByName(String name, Span`1 utf8Name, MemberListType listType, CacheType cacheType)
   at System.RuntimeType.RuntimeTypeCache.MemberInfoCache`1.Populate(String name, MemberListType listType, CacheType cacheType)
   at Microsoft.Extensions.Internal.PropertyActivator`1.GetActivatableProperties(Type type, Type activateAttributeType, Boolean includeNonPublic)
   at Microsoft.AspNetCore.Mvc.Razor.RazorPagePropertyActivator..ctor(Type pageType, Type declaredModelType, IModelMetadataProvider metadataProvider, PropertyValueAccessors propertyValueAccessors)
   at Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.DefaultPageFactoryProvider.CreatePageFactory(CompiledPageActionDescriptor actionDescriptor)
   at Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.PageActionInvokerCache.CreateCacheEntry(CompiledPageActionDescriptor compiledActionDescriptor, FilterItem[] cachedFilters)
   at Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.PageActionInvokerCache.GetCachedResult(ActionContext actionContext)
   at Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.PageRequestDelegateFactory.<>c__DisplayClass13_0.<CreateRequestDelegate>b__0(HttpContext context)
   at Microsoft.AspNetCore.Routing.EndpointMiddleware.Invoke(HttpContext httpContext)
   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
   at Microsoft.AspNetCore.Diagnostics.StatusCodePagesMiddleware.Invoke(HttpContext context)
   at Program.<>c.<<<Main>$>b__0_2>d.MoveNext()
```

### 2. Root Cause Analysis

1. **Active Development Server File Lock:**
   The user development server was actively running `USAP.Web.exe` from `bin\Debug\net10.0\USAP.Web.exe`. In this state, the host executable and in-memory assemblies are locked by the operating system.
2. **Unsafe Manual Binary Copy:**
   During the C7 session, manual file copy commands were executed:
   ```powershell
   Copy-Item "src\USAP.Web\obj\Debug\net10.0\USAP.Web.dll" "src\USAP.Web\bin\Debug\net10.0\USAP.Web.dll"
   Copy-Item "src\USAP.Web\obj\Debug\net10.0\USAP.Web.pdb" "src\USAP.Web\bin\Debug\net10.0\USAP.Web.pdb"
   ```
   Overwriting a partially loaded managed assembly while ASP.NET Core Razor runtime compilation / metadata discovery was executing created mismatched metadata tokens between the runtime type system and the disk assembly, triggering `BadImageFormatException (0x8007000B)` when dynamic activator reflection inspected page properties.

### 3. Prohibited Actions Established

To prevent recurrence:
* Never manually copy or move DLL, PDB, EXE, or generated Razor artifacts between `obj` and `bin`.
* Never replace compiled output while an application process is running.
* Never touch file timestamps to force runtime recompilation.
* Never clean or delete `bin` or `obj` directories while a process is holding an active lock.
* Never terminate user-owned development processes.
* Do not add production workaround code to mask development build output corruption.

### 4. Recovery and Verification Evidence

1. **User Dev Server Restart (PID 35896):**
   The user restarted the development server at `bin\Debug\net10.0\USAP.Web.exe` (PID 35896, listening on HTTP 5296 and HTTPS 7012).
   - Direct verification via `curl -k -I https://localhost:7012/about-us` confirmed `HTTP/1.1 200 OK`.
   - Content inspection confirmed the rendered HTML contains the synchronized C7 copy:
     `United States Antenna Products, LLC develops antenna and control solutions for military, government, and commercial communication requirements.`
2. **Agent-Owned Clean Release Verification (Port 5099):**
   During C8, an isolated Release build was compiled (`dotnet build USAP.Web.sln --configuration Release`) and launched on `http://127.0.0.1:5099`:
   - `curl -s -I http://127.0.0.1:5099/about-us` returned `HTTP/1.1 200 OK`.
   - The returned body verified clean rendering with zero exceptions and the approved C7 text.
   - The agent-owned process was immediately terminated upon verification.
3. **Defect Resolution Status:**
   Resolved. The defect does not recur under normal clean build and restart workflows.

---

## QA-002 — USAP-ABOUT-001-C9 Verification & Runtime Integrity Log

* **Date:** 2026-09-09
* **Milestone / Task:** USAP-ABOUT-001-C9 (Ambient Card Response and Source-Backed Copy Expansion)
* **Scope:** Verification of `.card--ambient` modifier, expanded Company Overview copy, revised Integrated Support introduction, and Release build runtime integrity.
* **Server Verification:**
  * User-owned Debug process (PID 35896) preserved untouched.
  * Agent-owned Release server compiled cleanly (`dotnet build USAP.Web.sln --configuration Release --no-restore`) and verified on isolated port `http://127.0.0.1:5099`.
  * Verified HTTP 200 on all primary routes (`/`, `/about-us`, `/products`, `/technical-resources`, `/contact-us`, `/request-a-quote`).
  * Confirmed rendered `/about-us` body contains target phrases:
    - `"Beyond its antenna products, USAP offers rotator systems..."`
    - `"USAP supports complete antenna projects rather than limiting its role..."`
  * Zero runtime exceptions encountered.
* **Ambient Card Verification:**
  * Informational cards (`.card--ambient`) retain stationary posture (`transform: none`), default cursor, and subtle navy bloom (`--card-shadow-ambient`).
  * Red top accent subtly transitions to `--color-brand-red-hover` while left navy border remains `--color-brand-navy`.

---

## QA-003 — USAP-ABOUT-001-C10 Verification: Stronger, Homepage-Aligned Ambient Card Elevation

* **Date:** 2026-09-09
* **Milestone / Task:** USAP-ABOUT-001-C10 (Stronger, Homepage-Aligned Ambient Card Elevation)
* **Scope:** Alignment of informational `.card--ambient` hover elevation with the homepage shared tokens (`--card-hover-translate-y: -4px`, `--card-shadow-hover`, `--card-hover-border-color`).
* **Implementation & Verification:**
  * Cleaned up unused `--card-shadow-ambient` and `--card-ambient-border-color` tokens.
  * `.card--ambient` now transitions `transform`, `box-shadow`, and `border-color` using shared timing tokens.
  * Verified hover activation is strictly scoped to fine-pointer devices (`@media (hover: hover) and (pointer: fine)`).
  * Verified reduced motion disables transition and enforces `transform: none` on hover.
  * Confirmed `cursor: default` and noninteractive markup on all About cards.
  * Verified Release server on isolated port 5099 with HTTP 200 on all 6 primary routes.

---

## QA-004 — USAP-ABOUT-001-C11 Verification: Stationary Heritage Card and Shared Closing CTA

* **Date:** 2026-09-10
* **Milestone / Task:** USAP-ABOUT-001-C11 (Stationary Heritage Card and Shared Closing CTA)
* **Scope:**
  * Added `.card--ambient-stationary` modifier to keep the 50-year Company Overview card stationary (`transform: none`) on hover while preserving hover shadow (`--card-shadow-hover`), hover border response (`--card-hover-border-color`), and navy left accent (`border-left-color: var(--color-brand-navy)`).
  * Maintained full `-4px` vertical lift and elevation on the remaining seven About informational cards.
  * Promoted the homepage Section 8 closing CTA to a reusable shared design component (`.closing-cta`, `.closing-cta--default`) in `src/USAP.Web/wwwroot/css/site.css`.
  * Applied the shared closing CTA component to `/about-us`, reusing the homepage background asset (`usap-home-closing-cta-signal-background.png`) without modifying the asset or changing About Us copy or destinations.
  * Removed obsolete closing CTA styling from `homepage.css` and `about-us.css`.
* **Runtime & Build Verification:**
  * User-owned Debug process preserved untouched.
  * Release build compiled cleanly: `dotnet build USAP.Web.sln --configuration Release --no-restore` (exit code 0).
  * `dotnet format USAP.Web.sln --verify-no-changes --no-restore` checked (exit code 1 pre-existing on `Program.cs` solely).
  * `git diff --check` passed cleanly with 0 whitespace errors.
  * Release server launched on isolated port `http://127.0.0.1:5099`.
  * Verified HTTP 200 on all 6 primary routes (`/`, `/about-us`, `/products`, `/technical-resources`, `/contact-us`, `/request-a-quote`).
* **Computed Style Verification:**
  * Heritage card (`.about-heritage-card`):
    - Resting: `transform: matrix(1, 0, 0, 1, 0, 0)`, `box-shadow: rgba(15, 32, 60, 0.04) 0px 1px 2px 0px, rgba(15, 32, 60, 0.08) 0px 10px 30px 0px`, `border-color: rgba(15, 32, 60, 0.1)`, `border-left-color: rgb(13, 27, 46)`, `cursor: default`.
    - Hovered: `transform: none`, `box-shadow: rgba(15, 32, 60, 0.05) 0px 4px 6px 0px, rgba(15, 32, 60, 0.12) 0px 16px 40px 0px`, `border-color: rgba(15, 32, 60, 0.2)`, `border-left-color: rgb(13, 27, 46)`, `cursor: default`.
  * Representative smaller card (`.about-feature-card`):
    - Hovered: `transform: matrix(1, 0, 0, 1, 0, -4)` (elevates by -4px).
  * Shared Closing CTA (`Index.cshtml` vs `AboutUs.cshtml`):
    - `background-image`: both resolve to `usap-home-closing-cta-signal-background.png`.
    - `background-color`: both `rgb(15, 32, 60)` (`--color-brand-navy`).
    - `background-size`: both `cover`.
    - `background-position`: both `50% 50%` (`center`).
    - `padding-block`: both `clamp(3rem, 6vw, 5rem)` (80px at 1440px viewport).
    - `text-align`: both `center`.
* **Asset Safeguards:**
  * Protected homepage PNG `usap-home-application-commercial-industrial.png` SHA-256 confirmed byte-for-byte identical: `8665ABC4ED755509938DCB12E3825B1301DB76533EB4C8BD813530DD69D579E3`.
  * Closing CTA background PNG `usap-home-closing-cta-signal-background.png` SHA-256 confirmed byte-for-byte identical: `396B7AAE1CE34B5A05781E1FC00F0D754F53A962DBFA9812E9DFF38968B317B9`.
  * Zero images generated, modified, or duplicated.

---

## QA-005 — USAP-CONTACT-ASSETS-001-C1 Verification: Shared Heroes, Contact Sidebars, and Reference-Grounded Assets

* **Date:** 2026-09-11
* **Milestone / Task:** USAP-CONTACT-ASSETS-001-C1 (Shared Internal Heroes, Contact Sidebars, Maps, and About Asset Integration)
* **Scope:**
  * Verification of canonical review package (`USAP_Internal_Page_Asset_Review_Package_2026-09-11.zip`, SHA-256: `3BB2A04AAED355C9DF3209F49C3A92D57E946F12FB1D968868F6363904391267`) in `project-input/`.
  * Promotion and byte-for-byte hash verification of 6 authorized image assets in `wwwroot/images/`.
  * Removal of 3 superseded provisional About assets from `wwwroot/images/about/`.
  * Implementation and computed-style verification of shared `.internal-hero` component in `site.css` across `/about-us`, `/contact-us`, and `/request-a-quote`.
  * Implementation of shared responsive two-column layout (`.form-page-layout`) and `_ContactSidebar.cshtml` partial housing verified public USAP contact details and HTTPS Google Maps embed.
  * Verification that Technical Resources hero asset is stored in `wwwroot` but completely unreferenced and unrequested at runtime.
  * Strict preservation of all form models, handlers, validation attributes, antiforgery tokens, honeypot fields, and submission behavior.
* **Archive & Asset Hash Evidence:**
  * Package: `project-input/USAP_Internal_Page_Asset_Review_Package_2026-09-11.zip` -> `3BB2A04AAED355C9DF3209F49C3A92D57E946F12FB1D968868F6363904391267` (MATCH).
  * Promoted Assets:
    1. About Hero: `usap-about-hero-lp1017-clean-v2.png` -> `D6D1C8F8B0515818B924FC3AD32BCC90D47BDD02B15DE9C154C2F7037D4D6557` (MATCH).
    2. About Manufacturing: `usap-about-manufacturing-rf-interface-lp1019-candidate-c-v2.png` -> `99F8873AA732B6B90611F976E8E8A795EBCE00A0EE581A7FADF3B866081E4033` (MATCH).
    3. About Integrated Support: `usap-about-integrated-support-lp1112mr-clean-v2.png` -> `87EB5F2A6BA0AA7A938CECF590D52193C87E3C0CA914CBC77303E868A7A79423` (MATCH).
    4. Contact Hero: `usap-contact-hero-communication-signal-candidate-c-v1.png` -> `C6F70E4B0CA756EB16486B194BACED5B9412529E196C6D0A5DF253E5BE588E88` (MATCH).
    5. Quote Hero: `usap-quote-hero-technical-planning-candidate-a-v1.png` -> `6269BE6639F6CCB31A214CC05D7EF9EF587A57A782244778CBAD181261C53FBE` (MATCH).
    6. Technical Resources Hero (store only): `usap-technical-resources-hero-requirements-document-candidate-b-v1.png` -> `03FE67FA5A836A472A4C19C172B44AAA154CE282088155E135027FEC01CEF48A` (MATCH).
* **Runtime Safety & Process Verification:**
  * User-owned Debug server (PID 13528 on ports 5296 / 7012) preserved untouched.
  * Agent-owned Release server compiled cleanly (`dotnet build USAP.Web.sln --configuration Release --no-restore`) and verified on isolated port `http://127.0.0.1:5099`.
  * Verified HTTP 200 OK on all 6 primary routes (`/`, `/about-us`, `/products`, `/technical-resources`, `/contact-us`, `/request-a-quote`).
* **Broken Images & Asset Request Verification:**
  * Zero 404s for any promoted or active asset on `/about-us`, `/contact-us`, `/request-a-quote`, `/products`, `/technical-resources`.
  * Technical Resources hero confirmed unreferenced by grep and DOM inspection.
  * All 3 superseded provisional About assets confirmed deleted from disk and absent from all runtime markups and CSS.
* **Shared Hero Computed Style Evidence:**
  * About Hero: `background-color: rgb(13, 27, 46)`, `min-height: 352px (22rem)`, `title font-size: 48px`, `lead font-size: 20px`, `image object-fit: cover`, `image object-position: 70% 50%`.
  * Contact Hero: `background-color: rgb(13, 27, 46)`, `min-height: 352px (22rem)`, `title font-size: 48px`, `lead font-size: 20px`, `image object-fit: cover`, `image object-position: 65% 50%`.
  * Quote Hero: `background-color: rgb(13, 27, 46)`, `min-height: 352px (22rem)`, `title font-size: 48px`, `lead font-size: 20px`, `image object-fit: cover`, `image object-position: 60% 50%`.
* **Form Integrity & Security Verification:**
  * PageModel handlers (`ContactUsModel`, `RequestAQuoteModel`, `InquiryPageModelBase`) unchanged.
  * `_InquiryForm.cshtml` bindings, validation attributes, field-level spans, validation summary, antiforgery token, honeypot field, and submit button 100% identical and preserved.
  * Google Maps embed: HTTPS, query `5263 Agro Drive, Frederick, MD 21703`, `loading="lazy"`, `allowfullscreen`, `referrerpolicy="no-referrer-when-downgrade"`, descriptive accessible title, and accessible direct Google Maps link.
* **Responsive & Horizontal Overflow Verification:**
  * Tested viewports: 390×844, 768×1024, 1024×768, 1025×768, 1440×900, 1920×1080.
  * Zero horizontal overflow on `/about-us`, `/contact-us`, and `/request-a-quote` across all viewports.
* **Build & Code Formatting:**
  * `dotnet build USAP.Web.sln --configuration Release --no-restore`: 0 Errors, 0 Warnings (Exit code 0).
  * `dotnet format USAP.Web.sln --verify-no-changes --no-restore`: Only pre-existing `IDE0011` brace warnings in `Program.cs` reported. Zero format warnings in new/modified files.
  * `git diff --check`: Clean (0 whitespace/conflict errors).

---

## QA-006 — USAP-CONTACT-ASSETS-001-C2 Verification: Revised Contact Hero, Provenance, and Corrected QA Evidence

* **Date:** 2026-09-12
* **Milestone / Task:** USAP-CONTACT-ASSETS-001-C2 (Revised Contact Hero Validation and QA Evidence Corrections)
* **Scope:**
  * Protection, inspection, and verification of user-supplied revised Contact Us hero image `src/USAP.Web/wwwroot/images/contact-us/usap-contact-hero-communication-signal-candidate-c-v1.png`.
  * Visual inspection confirming removal of medical/ECG waveform and replacement with clean horizontal communication signal bars within speech bubble.
  * Verification of supplemental provenance directory `project-input/USAP_Contact_Hero_Revision_2026-09-12/` containing byte-for-byte duplicate, `CHECKSUMS.sha256`, and `README.md`.
  * Verification that canonical archive `project-input/USAP_Internal_Page_Asset_Review_Package_2026-09-11.zip` remains byte-for-byte unchanged (`3BB2A04AAED355C9DF3209F49C3A92D57E946F12FB1D968868F6363904391267`).
  * Confirmation that zero Razor markup or CSS selector changes were required.
  * Correction of C1 broken-image test defect: implemented scroll-into-view, lazy-loading decode, and `naturalWidth > 0` validation across all 6 core routes (`c2_local_image_loading_evidence.json`).
  * Creation of genuine browser Tab-key navigation evidence targeting contact info links on `/contact-us` (`c2_keyboard_focus_evidence.png`, `c2_keyboard_focus_evidence.json`).
  * Release build, format verification, and visual regression testing across all 6 required viewports.
* **Hero Asset Checksums & Provenance:**
  * Original Archived Candidate C: `project-input/USAP_Internal_Page_Asset_Review_Package_2026-09-11.zip` -> `C6F70E4B0CA756EB16486B194BACED5B9412529E196C6D0A5DF253E5BE588E88` (preserved in archive).
  * Revised Production Candidate C: `src/USAP.Web/wwwroot/images/contact-us/usap-contact-hero-communication-signal-candidate-c-v1.png` -> `B020AD39E6A8CEFD15C584DCA9B77503070CDE8FD0C33B63C6A556F645CD71E4` (2048 × 768 px, 1,671,032 bytes, sRGB PNG).
  * Supplemental Provenance Copy: `project-input/USAP_Contact_Hero_Revision_2026-09-12/usap-contact-hero-communication-signal-candidate-c-v1.png` -> `B020AD39E6A8CEFD15C584DCA9B77503070CDE8FD0C33B63C6A556F645CD71E4` (BYTE-IDENTICAL MATCH).
  * Archive: `project-input/USAP_Internal_Page_Asset_Review_Package_2026-09-11.zip` -> `3BB2A04AAED355C9DF3209F49C3A92D57E946F12FB1D968868F6363904391267` (BYTE-IDENTICAL UNCHANGED).
* **Corrected Local Image Loading Evidence:**
  * All local `<img>` elements scrolled into view, decoded, and verified with `complete === true` and `naturalWidth > 0` across `/`, `/about-us`, `/products`, `/technical-resources`, `/contact-us`, and `/request-a-quote`.
  * Zero broken local images.
* **Genuine Keyboard Focus Evidence:**
  * Real browser Tab-key sequence dispatched from top of document to `.contact-phone-link`.
  * `document.activeElement` confirmed as `<a class="contact-phone-link" href="tel:+12403417120">240-341-7120</a>`.
  * Computed focus outline: `3px solid rgb(0, 87, 184)` with `outline-offset: 2px`.
* **Runtime Verification:**
  * Clean Release build (0 errors, 0 warnings).
  * Isolated Release server verified on port 5099 with HTTP 200 on all 6 routes.
  * Zero horizontal overflow across all 6 viewports (390×844, 768×1024, 1024×768, 1025×768, 1440×900, 1920×1080).

---

## QA-007 — USAP-PRODUCTS-001-C1 Verification: Structured Six-Family Catalog, Responsive Products Landing Page, and Family Route Placeholders

* **Date:** 2026-09-12
* **Milestone / Task:** USAP-PRODUCTS-001-C1 (Product Catalog Foundation & Landing Page Implementation)
* **Scope:**
  * Implementation of strongly-typed catalog domain models and `ProductCatalogService` singleton in DI.
  * Enforcement and execution of 8 automated validation rules asserting catalog integrity at startup/construction.
  * Delivery of production-ready `/products` landing page using shared `.internal-hero`, 3-col/2-col/1-col responsive family card grid, source-constrained selection guidance, and shared closing CTA.
  * Reservation of 6 canonical family routes via `Pages/Products/Family.cshtml` with shared `.internal-hero`, breadcrumb trail, provisional warning notice, and representative product groups.
  * Elimination of soft-404 defect: slug validation against `IProductCatalogService` returns HTTP 404 for unrecognized slugs.
  * Zero exposure of disputed specifications on the landing page (held in metadata `ConflictHolds`).
  * Preservation of Contact Us and Request a Quote form models, handlers, validation attributes, antiforgery tokens, honeypot, reCAPTCHA, and submission behavior.
  * Verification that Technical Resources hero asset remains store-only and unreferenced.
* **Catalog Validation Test Results (Milestone C1/C1A):**
  * Test suite executed via standalone test runner (`scratch/CatalogValidationTests`):
    1. Family count != 6 (tested 5 and 7 families) -> Threw `Catalog must define exactly 6 families` [PASS].
    2. Out of order family sequence -> Threw `Family at position 1 must be 'log-periodic-antennas'` [PASS].
    3. Wrong group count in catalog (15 groups instead of 16) -> Threw `Catalog must define exactly 16 distinct product groups` [PASS].
    4. Group misassignment / swap (tested assigning LP-1402-1403 to Log Periodic Antennas instead of LP-1018ba) -> Threw `Family 'log-periodic-antennas' group mismatch at index 2: expected 'lp-1018ba', found 'lp-1402-1403'` [PASS].
    5. Wrong per-family group distribution (tested 6 in LP instead of 5) -> Threw `Family 'log-periodic-antennas' must contain exactly 5 groups` [PASS].
    6. Duplicate group ID / cross-family duplicate -> Threw `Duplicate product group ID detected: 'lp-high-power'` [PASS].
    7. Unknown family referenced by product group -> Threw `references unknown family 'unknown-family-xyz'` [PASS].
    8. Unknown product group referenced by resource -> Threw `references unknown product group 'non-existent-group'` [PASS].
    9. Named models count != 30 -> Threw `Catalog must contain exactly 30 named model/configuration codes` [PASS].
    10. T-3002 child configuration missing -> Threw `T-3002 group must contain configuration child model '3002FB'` [PASS].
    11. Live service instantiation -> 6 families in approved canonical order, 16 product groups distributed 5 / 3 / 1 / 1 / 5 / 1, exact ordered group IDs per family verified, LP-1112MR primarily in Log Periodic, LP-1402-1403 in Portable & Transportable, 1910 in Portable & Transportable, APERIODIC sole Aperiodic group, 5 rotators/controllers in Rotator & Control, T-3002 sole Tower group with 4 child configurations, 30 named models + 1 unnamed aperiodic line, conservative family summaries verified [PASS].
* **HTTP Route Status Verification (Isolated Release Server, Port 5099):**
  * `/`: HTTP 200 OK
  * `/products`: HTTP 200 OK
  * `/products/log-periodic-antennas`: HTTP 200 OK
  * `/products/portable-transportable-antennas`: HTTP 200 OK
  * `/products/aperiodic-loop-antennas`: HTTP 200 OK
  * `/products/nvis-antennas`: HTTP 200 OK
  * `/products/antenna-rotator-control-systems`: HTTP 200 OK
  * `/products/tower-systems-accessories`: HTTP 200 OK
  * `/products/invalid-route-slug`: HTTP 404 Not Found (re-executed via `/not-found`) [PASS]
  * `/about-us`: HTTP 200 OK
  * `/contact-us`: HTTP 200 OK
  * `/request-a-quote`: HTTP 200 OK
  * `/technical-resources`: HTTP 200 OK
* **DOM & Catalog Card Inspection:**
  * Exactly 6 family cards rendered in approved display order:
    1. Log Periodic Antennas (`/products/log-periodic-antennas`) - Media: Image (`usap-home-featured-product-lp-1017.png`)
    2. Portable & Transportable Antenna Systems (`/products/portable-transportable-antennas`) - Media: Image (`usap-home-featured-product-transportable-hf.png`)
    3. Aperiodic Loop Antennas (`/products/aperiodic-loop-antennas`) - Media: Deliberate Native Placeholder (`.product-family-card__placeholder`)
    4. NVIS Antennas (`/products/nvis-antennas`) - Media: Deliberate Native Placeholder (`.product-family-card__placeholder`)
    5. Antenna Rotator & Control Systems (`/products/antenna-rotator-control-systems`) - Media: Image (`usap-home-featured-product-rotator-controls.png`)
    6. Tower Systems & Accessories (`/products/tower-systems-accessories`) - Media: Deliberate Native Placeholder (`.product-family-card__placeholder`)
  * Exactly one clear link per card with accessible `.sr-only` family name extension.
  * 3 selection-help cards verified under semantic heading hierarchy (`h1` -> `h2` -> `h3`).
  * Shared closing CTA rendered with Request a Quote and Contact Engineering buttons.
* **Genuine Keyboard Focus Verification:**
  * Dispatched browser Tab sequence from document top to first family card link.
  * Tab sequence: `#1 .skip-link` -> `#2 .site-brand` -> `#3-#7 nav links` -> `#8 .nav-cta` -> `#9 .product-family-card__link`.
  * Target link: `document.activeElement` confirmed as `<a class="btn btn-secondary btn-sm product-family-card__link" href="/products/log-periodic-antennas">`.
  * `matchesFocusVisible: true`.
  * Computed outline: `3px solid rgb(0, 87, 184)` with `outline-offset: 2px`.
  * Captured full viewport evidence (`07_family_card_keyboard_focus_viewport_1440x900.png`) and card-crop evidence (`07_family_card_keyboard_focus_card_crop.png`).
* **Responsive & Horizontal Overflow Verification:**
  * Tested viewports: 390×844, 768×1024, 1024×768, 1025×768, 1440×900, 1920×1080.
  * `document.documentElement.scrollWidth <= window.innerWidth` across all viewports on `/products` and family routes. Zero horizontal overflow detected.
* **Local Image & Media Decoding Verification:**
  * Total images on `/products`: 4 (Brand logo SVG + 3 approved product PNG cutouts).
  * All images decoded with `complete === true` and positive natural dimensions. Zero broken local images.
* **Reduced Motion & No-JS Usability:**
  * `prefers-reduced-motion: reduce`: Computed transition durations on cards and buttons suppressed to `0.00001s` (`1e-05s`).
  * JavaScript execution disabled: Exactly 6 cards, all links, and content render and navigate without JavaScript.
* **Console & Network Errors:**
  * Zero browser console errors.
  * Zero unhandled network failures.
* **Build & Code Formatting Evidence:**
  * `dotnet build USAP.Web.sln --configuration Release --no-restore`: 0 Errors, 0 Warnings (Exit code 0).
  * `dotnet format USAP.Web.sln --verify-no-changes --no-restore`: Only pre-existing `IDE0011` warnings in `Program.cs` reported. Zero warnings in new/modified files.
  * `git diff --check`: Clean (0 whitespace errors).

---

## QA-008 — USAP-PRODUCTS-001-C1B Verification: Product Catalog Visual and Semantic Refinement

* **Date:** 2026-09-13
* **Milestone / Task:** USAP-PRODUCTS-001-C1B (Product Catalog Visual and Semantic Refinement)
* **Scope:**
  * Relocation of breadcrumbs out of `.internal-hero--family` to the top of the main content area directly below the hero on all six product-family routes. Semantic `<nav aria-label="Breadcrumb">` with ordered list, light-background styling, linked Home & Products ancestors, and non-linked `aria-current="page"` current family.
  * Entire surface of all six `/products` family cards made clickable via semantic `.stretched-link` pattern: exactly one real anchor per card (`.product-family-card__link`), one logical Tab stop per card, no JavaScript click handlers.
  * Replaced filled button CTA (`.btn.btn-secondary.btn-sm`) with restrained editorial card link (`.card-link`) displaying `Explore Family →`, aligned at the bottom of the card body, with accessible family-specific label (`aria-label="Explore @family.Name"`).
  * Enforced uniform card content alignment: CSS grid/flexbox equal-height cards per row, bottom-aligned CTAs across varying title and summary lengths, natural responsive wrapping.
  * Edge-to-edge 16:10 media frames (`aspect-ratio: 16 / 10`, `overflow: hidden`, `object-fit: cover`, `object-position: center`) without inset padding or gutters; identical frame geometry for images and native placeholders.
  * Accent restraint: removed red top border (`card--accent-top-red`) from ordinary family cards; removed repeated navy left border (`card--accent-left-navy`) from product-group cards on family pages.
  * Corrected native placeholder terminology: badge updated to `PRODUCT IMAGERY IN DEVELOPMENT`, caption updated to `Design placeholder — replacement imagery pending USAP review.`
  * Removed internal checkpoint and milestone terminology (`Checkpoint C1`, `Milestone C1`, `Milestone C2`, `provisional`) from public pages; replaced with restrained public `Product Information` notice linking to USAP engineering.
  * Full-card focus-visible outline: `3px solid rgb(0, 87, 184)` with `outline-offset: 2px` via `:has(.product-family-card__link:focus-visible)`.
  * Candidate image alt text: Replaced exact-model assertions with concise family-level descriptions ("Log periodic antenna array in a field installation.", "Transportable HF antenna system in a field setting.", "Antenna rotator and digital controller components.").
  * Selection-assistance copy: Audited cards on `/products` against canonical research and workbook. Replaced draft copy with source-constrained text and removed unsubstantiated terms ("long-term continuous duty", "terrain obstacle mitigation", "low-noise", "precision", "microprocessor", "modular aluminum").
  * Preservation of canonical C1A baseline: exactly 6 families, 16 groups distributed `5 / 3 / 1 / 1 / 5 / 1`, 30 named models + 1 unnamed, existing validation rules, true HTTP 404 on invalid slugs.
  * Preservation of Contact Us and Request a Quote form behavior, security controls, and Technical Resources store-only asset status.
* **Catalog Validation Test Results (Milestone C1/C1A Baseline Preserved):**
  * All 11 positive and negative catalog integrity tests pass in `CatalogValidationTests`:
    1. Family count assertion [PASS]
    2. Family sequence assertion [PASS]
    3. Distinct product group count assertion (16 groups) [PASS]
    4. Group assignment integrity assertion [PASS]
    5. Group distribution assertion (5 / 3 / 1 / 1 / 5 / 1) [PASS]
    6. Duplicate group ID detection [PASS]
    7. Unknown family reference detection [PASS]
    8. Unknown product group reference detection [PASS]
    9. Named models count assertion (30 named models) [PASS]
    10. T-3002 child configuration validation [PASS]
    11. Live service instantiation and conservative summaries verification [PASS]
* **HTTP Route Status Verification (Isolated Release Server, Port 5099):**
  * `/`: HTTP 200 OK
  * `/products`: HTTP 200 OK
  * `/products/log-periodic-antennas`: HTTP 200 OK
  * `/products/portable-transportable-antennas`: HTTP 200 OK
  * `/products/aperiodic-loop-antennas`: HTTP 200 OK
  * `/products/nvis-antennas`: HTTP 200 OK
  * `/products/antenna-rotator-control-systems`: HTTP 200 OK
  * `/products/tower-systems-accessories`: HTTP 200 OK
  * `/products/invalid-route-slug`: HTTP 404 Not Found (executed via custom `/not-found` handler) [PASS]
  * `/about-us`: HTTP 200 OK
  * `/contact-us`: HTTP 200 OK
  * `/request-a-quote`: HTTP 200 OK
  * `/technical-resources`: HTTP 200 OK
* **Breadcrumb Placement & Semantic Audit:**
  * Zero breadcrumbs found inside `.internal-hero--family` across all six family pages.
  * Exactly one semantic breadcrumb found in the main content container directly below the hero on each family page.
  * Breadcrumb container: `<nav class="breadcrumb-trail" aria-label="Breadcrumb">`.
  * Hierarchy: `<ol class="breadcrumb-trail__list">` with 3 items:
    1. `<li><a class="breadcrumb-trail__link" href="/">Home</a></li>`
    2. `<li><a class="breadcrumb-trail__link" href="/products">Products</a></li>`
    3. `<li><span class="breadcrumb-trail__current" aria-current="page">[Family Name]</span></li>`
* **Card Clickability, Links, and Tab-Stop Audit:**
  * Exactly 1 real anchor per family card (`.product-family-card__link`). Zero buttons inside cards.
  * Single Tab stop per card confirmed during keyboard navigation.
  * Stretched-link hit testing confirmed: clicking representative coordinates within media frame, eyebrow, heading, summary, body whitespace, and CTA all resolve to target family link.
  * CTA rendered as editorial `.card-link` with `aria-label="Explore [Family Name]"`.
  * Desktop row CTA bottom alignment verified: Row 1 cards bottom edge at 1256px, Row 2 cards bottom edge at 1823px across all cards in each row.
* **Media Frame Dimensions & Fit Evidence:**
  * Computed aspect ratio on all family-card media frames: 16:10 (`aspect-ratio: 16 / 10`, computed 388 × 242.5 px at 1440px viewport).
  * Media frames run edge-to-edge to top, left, and right borders of card (`overflow: hidden`, 0 inset padding).
  * Native design placeholder frames compute to identical dimensions as image frames.
  * Computed image styles: `object-fit: cover`, `object-position: center`, `display: block`.
* **Accent Restraint Evidence:**
  * Normal product-family cards: `border-top`: `1px solid rgba(15, 32, 60, 0.1)` (no red top border).
  * Normal product-group cards: `border-left`: `1px solid rgba(15, 32, 60, 0.1)` (no navy left border).
  * Deliberate notices: `.product-info-notice` retains informational amber accent border.
* **Placeholder & Milestone Terminology Verification:**
  * Placeholder badge text: `PRODUCT IMAGERY IN DEVELOPMENT`.
  * Placeholder caption text: `Design placeholder — replacement imagery pending USAP review.`
  * Zero occurrences of `Checkpoint C1`, `C1A`, `C1B`, `Milestone C1`, or `Milestone C2` in rendered HTML across all pages.
* **Genuine Keyboard Focus Verification:**
  * Tab navigation to family card link activates full-card focus ring: `outline: 3px solid rgb(0, 87, 184)` with `outline-offset: 2px` via `:has(.product-family-card__link:focus-visible)`.
  * CTA displays active focus underline and arrow shift.
* **Responsive Viewport & Overflow Verification:**
  * Tested viewports: 390×844, 768×1024, 1024×768, 1025×768, 1440×900, 1920×1080.
  * Zero horizontal overflow detected on any page across all viewports (`scrollWidth <= innerWidth`).
* **Console & Network Errors:**
  * Zero browser console errors across all routes.
  * Zero unhandled network failures across all routes.
* **Reduced Motion & No-JS Usability:**
  * `prefers-reduced-motion: reduce`: Transition durations suppressed to `0.00001s` (`1e-05s`), `transform: none` enforced.
  * JavaScript execution disabled: Full card grid, images, placeholders, and links render and navigate cleanly.
* **Build & Code Formatting Evidence:**
  * `dotnet build USAP.Web.sln --configuration Release --no-restore`: 0 Errors, 0 Warnings (Exit code 0).
  * Isolated Debug build: 0 Errors, 0 Warnings (Exit code 0). Standard Debug build notes PID 7976 locking `USAP.Web.exe` (user-owned server preserved).
  * `dotnet format USAP.Web.sln --verify-no-changes --no-restore`: Verified; zero warnings in new or modified files. Only pre-existing `IDE0011` warnings in `Program.cs`.
  * `git diff --check`: Clean (0 whitespace errors).

---

## QA-009 — Task USAP-CATALOG-001 Verification: Products Landing Page & A2F Candidate Asset Integration

* **Date:** 2026-09-15
* **Task / Milestone:** USAP-CATALOG-001 (Products Landing Page and Reusable Catalog Foundation)
* **Scope & Implementation Summary:**
  * Intake and cryptographic integrity verification of canonical review package archive: `project-inputs/USAP_Product_Catalog_A2F_Review_Package_2026-09-15.zip` (10,950,498 bytes, SHA-256: `0831c27bb1a66bf4ef2841a92fae94eb2c1316fcf1d1d3a9be2cab546eabced5`).
  * Promotion of seven project-selected candidate assets into `src/USAP.Web/wwwroot/images/products/` with byte-identical hash preservation verified against `HASH_PRESERVATION_REGISTER.csv`:
    1. Products landing hero: Direction C (`usap-products-landing-hero-direction-c-candidate-v1.png`, 2048 × 768 px, 1,946,738 B, SHA-256: `E9F75AABACA46D8395A1B9CC5C998B469EFC55791EEBA7A08926320A96071EAD`)
    2. Log Periodic Antennas: Reference-grounded visual (`usap-family-card-log-periodic-family-visual-v1.png`, 1600 × 1000 px, 1,387,964 B, SHA-256: `03B191AB25844A40C0FD5B8045FFD264327734BB265BA5DFFCF3305EA0A9CF7F`)
    3. Portable & Transportable Antenna Systems: Stylized placeholder (`usap-family-card-portable-transportable-placeholder-v1.png`, 1600 × 1000 px, 1,773,360 B, SHA-256: `DDF0E92CC340520A441162607911BAA755A74610DB5087090C437A966A30E947`)
    4. Aperiodic Loop Antennas: Element visual (`usap-family-card-aperiodic-loop-element-v1.png`, 1600 × 1000 px, 2,098,672 B, SHA-256: `9CF71EA8DF4DDD1126FFA8F5C80D64304647EF2B82F21517553C9C945398E03F`)
    5. NVIS Antennas: Family visual (`usap-family-card-nvis-1942-family-visual-v1.png`, 1600 × 1000 px, 2,102,661 B, SHA-256: `839A933F73EA1B3F77FE6B2459342F36A5B9211BAE45F7C459D677E0209ACDA0`)
    6. Antenna Rotator & Control Systems: Lower-risk indicator Candidate B (`usap-family-card-rotator-control-lower-risk-placeholder-v2.png`, 1600 × 1000 px, 1,888,491 B, SHA-256: `5B0C8F2B71EF2CFEEF7ED81D091A79329B482AA23345B5616F75663C6378B682`)
    7. Tower Systems & Accessories: Generalized tower fallback Candidate B (`usap-family-card-tower-systems-nonconfigurational-fallback-v2.png`, 1600 × 1000 px, 2,006,234 B, SHA-256: `32A2CAC6B2DC9B6227D21295801CCEF3EDF1E2DBC8D562419DA02FCE565C09C8`)
  * Catalog service extension: `IProductCatalogService.GetLandingHeroAsset()` and implementation in `ProductCatalogService.cs`. Family asset metadata updated with new image paths, classifications, and approved alt text.
  * Products landing page Razor integration: `@Model.HeroAsset` wired to semantic `<img>` in `.internal-hero--products` with `width="2048" height="768"`, `fetchpriority="high"`, and `decoding="async"`. Family cards wired to `width="1600" height="1000"`, `loading="lazy"`, `decoding="async"`. Fallback placeholder markup preserved.
  * Responsive CSS styling: Added `.internal-hero--products .internal-hero-image { object-position: 75% center; }` on desktop and `object-position: 80% center;` at `<64rem` (mobile/tablet), ensuring antenna elements display clearly on the right while the left gradient guarantees WCAG AAA text contrast.
* **Catalog Validation Suite Results (11 Positive & Negative Tests Pass):**
  * Console test runner executed on Release build of `CatalogValidationTests`:
    1. Live Catalog Initialization & Structure [PASS]
    2. Negative: Family count < 6 (5 families) [PASS]
    3. Negative: Family count > 6 (7 families) [PASS]
    4. Negative: Out of order family sequence [PASS]
    5. Negative: Wrong group count in catalog (15 instead of 16) [PASS]
    6. Negative: Misassigned group reference (LP-1402-1403 in LP) [PASS]
    7. Negative: Wrong per-family group distribution (6 in LP, 2 in Portable) [PASS]
    8. Negative: Duplicate group ID detection [PASS]
    9. Negative: Product group references unknown family [PASS]
    10. Negative: Resource references unknown product group [PASS]
    11. Negative: Named models count != 30 [PASS]
    12. Negative: T-3002 child configuration missing [PASS]
* **Static Asset HTTP Delivery (Port 5099):**
  * All 7 promoted images returned HTTP 200 OK with `Content-Type: image/png` and exact byte counts matching source files.
* **HTTP Route Status Verification (Isolated Release Server, Port 5099):**
  * `/`: HTTP 200 OK
  * `/products`: HTTP 200 OK
  * `/products/log-periodic-antennas`: HTTP 200 OK
  * `/products/portable-transportable-antennas`: HTTP 200 OK
  * `/products/aperiodic-loop-antennas`: HTTP 200 OK
  * `/products/nvis-antennas`: HTTP 200 OK
  * `/products/antenna-rotator-control-systems`: HTTP 200 OK
  * `/products/tower-systems-accessories`: HTTP 200 OK
  * `/products/invalid-route-slug`: HTTP 404 Not Found (executed via custom `/not-found` handler) [PASS]
  * `/about-us`: HTTP 200 OK
  * `/contact-us`: HTTP 200 OK
  * `/request-a-quote`: HTTP 200 OK
  * `/technical-resources`: HTTP 200 OK
* **DOM Structure & Visual Audit (Headless Chrome CDP):**
  * Hero image element: `.internal-hero--products .internal-hero-image` confirmed present with correct src, alt, width (2048), height (768), `fetchpriority="high"`, and `decoding="async"`.
  * Six product-family cards confirmed present, each containing media frame (`aspect-ratio: 16 / 10`), card image with width (1600), height (1000), `loading="lazy"`, `decoding="async"`, and single `.stretched-link` anchor.
  * Selection guidance section (`.selection-help-section`) and closing CTA (`.closing-cta`) confirmed present.
* **Image Decoding Evidence:**
  * All 7 images evaluated via browser CDP: `complete === true`, `naturalWidth` and `naturalHeight` match intrinsic source dimensions (Hero: 2048×768; Cards: 1600×1000). Zero rendering errors or broken image icons.
* **Responsive Viewport & Overflow Audit:**
  * Viewports tested: 390×844, 768×1024, 1024×768, 1025×768, 1440×900, 1920×1080.
  * Zero horizontal overflow detected on any viewport (`document.documentElement.scrollWidth <= window.innerWidth`).
  * Hero focal positioning verified: `80% 50%` on mobile/tablet (390px, 768px); `75% 50%` on desktop (1024px, 1025px, 1440px, 1920px).
* **Keyboard Focus & Navigation Audit:**
  * Natural sequential Tab navigation successfully traversed all 6 family cards.
  * Each card link activation applied full-card focus ring (`outline: 3px solid rgb(0, 87, 184)`) via `:has(.product-family-card__link:focus-visible)`.
  * Exactly 1 Tab stop per card confirmed.
* **Accessibility, Reduced Motion & Console Hygiene:**
  * `prefers-reduced-motion: reduce`: Transition durations suppressed to `1e-05s`.
  * Browser console errors: 0.
  * Network failures: 0.
* **Build & Code Hygiene Verification:**
  * `dotnet build USAP.Web.sln --configuration Release --no-restore`: 0 Errors, 0 Warnings (Exit code 0).
  * `dotnet format USAP.Web.sln --verify-no-changes --no-restore`: Clean in new/modified code; actual exit code 1 due to 19 pre-existing baseline `IDE0011` brace warnings in `Program.cs`.
  * `git diff --check`: Clean (0 whitespace errors).

---

## QA-010 — Checkpoint USAP-CATALOG-001-QA1 Verification: Evidence-Safety, Screenshot Repair, and Review Package Portability

* **Date:** 2026-09-15
* **Task / Milestone:** Checkpoint USAP-CATALOG-001-QA1 (Evidence-Safety and Review-Package Repair)
* **Scope & Implementation Summary:**
  * **Products Landing Page Preservation:** Verified that `/products` layout, 6-card order, responsive styles, focal positions, and approved A2F imagery remain visually unchanged.
  * **Exact A2F Alt Text Preservation:** Verified live in DOM and `ProductCatalogService.cs`:
    1. Products Hero: `Technical visualization of antenna systems and engineering studies in a blue outdoor landscape.`
    2. Log Periodic: `Log periodic antenna array in an outdoor installation.`
    3. Portable & Transportable: `Portable antenna system deployed on a guyed field mast.`
    4. Aperiodic Loop: `Aperiodic loop antenna element on a transportable tripod.`
    5. NVIS: `Field-deployed NVIS antenna system with a central mast and broad wire footprint.`
    6. Rotator & Control: `Heavy-duty antenna rotator with a translucent tabletop controller study.`
    7. Tower Systems: `Technical study of tower, mast, rotation, feedline, and installation components.`
  * **A2F Classifications Aligned:** Aligned all records to:
    - Products hero: `conceptual family visual`
    - Log Periodic: `family-level conceptual/reference-grounded visual`
    - Portable & Transportable: `conceptual family placeholder`
    - Aperiodic Loop: `reference-grounded single-element visualization`
    - NVIS: `1942-family-level reference-grounded visualization`
    - Rotator & Control: `reference-grounded rotator with illustrative controller study`
    - Tower Systems: `design fallback — non-configurational family study`
    - Confirmed Log Periodic is not identified as an LP-1017 photograph and no candidate is represented as exact product photography.
  * **Evidence-Safe Family Route Shells:**
    - Active disclaimer notice wired to all 6 family routes in `Pages/Products/Family.cshtml`:
      > *"The models and configurations below are references from USAP’s published materials. Current availability, specifications, compatibility, and supported configurations require confirmation from USAP engineering."*
    - Section header updated to: `PUBLISHED PRODUCT REFERENCES` / `Models Referenced in This Family` with label `Published Model References:`.
    - Qualified all 16 groups and 30 models in `ProductCatalogService.cs` to remove unsubstantiated current-status assertions.
    - All 6 families, 16 group IDs, and 30 canonical model codes preserved intact.
  * **Browser Evidence & Prohibited Term Audit:**
    - Headless Chrome audit evaluated all 6 family routes (`family_pages_status_notice_audit.json`). Verified 100% presence of status notice, correct section headers, and 0 occurrences of prohibited terms ("currently available", "current product", "currently offered", "currently supported", "approved product", "available now").
  * **Input-Directory Hygiene:**
    - `USAP_Product_Catalog_A2F_Review_Package_2026-09-15.zip`: verified in `project-inputs/` (57,753,091 bytes).
    - `USAP_Product_Catalog_A3_Family_Hero_Review_Package_2026-09-15.zip`: verified in `project-input/` (34,654,781 bytes).
    - Both directories exist on disk and are strictly ignored in `.gitignore`. Retaining both entries documented as mandatory. A3 archive remains unextracted and unintegrated.
  * **Review Screenshot Repairs:**
    - `02_six_family_grid_1440px.png`: Repaired with expanded document bounding rect; full 6-card grid (3x2) captured with 0 blank clipping (1072x1103 px).
    - `04_six_family_grid_768px.png`: Repaired with expanded document bounding rect; full 6-card grid (2x3) captured with 0 blank clipping (720x1570 px).
    - `07_family_card_keyboard_focus_card_crop.png`: Repaired with centered active card and 15px outline padding; complete 3px focus outline visible (371x567 px).
    - Full-page screenshots for all 6 family routes (`06a` through `06f`) regenerated showing evidence-safe copy.
  * **Exact Command Results & Formatter Disclosure:**
    - `dotnet build USAP.Web.sln --configuration Release --no-restore`: Exit code 0 (0 errors, 0 warnings).
    - `dotnet format USAP.Web.sln --verify-no-changes --no-restore`: Actual exit code 1. Exactly 19 pre-existing baseline `IDE0011` brace warnings on `if` statements in `Program.cs` lines 16-35. Zero format warnings introduced in any new or modified file.
    - `git diff --check`: Exit code 0 (Clean, 0 whitespace errors).
    - `git status --short --branch`: Exit code 0 (`## feat/product-catalog-and-family-pages`, 100% unstaged and uncommitted).
  * **Portable Review Package:**
    - Created `USAP-CATALOG-001-QA1-review-package.zip` (18,518,622 bytes, SHA-256: `7C036448B82042E262338F1D67A6CD0A90852A47720E518D12AC6A6601798677`).
    - Staged outside repository root in artifact directory.
    - Verified 100% forward-slash entry paths (`/`) for cross-platform portability.
    - Validated ZIP integrity via CRC `testzip()` and extraction.
    - Manifest verified: 36 covered files (excluding `MANIFEST.txt` itself), 0 missing, 0 extra, 0 mismatched.
    - Textual source review snapshot included (`source_changes.diff`).

---

## QA-011 — Checkpoint USAP-CATALOG-001-QA2 Verification: Current-Site Catalog Policy Alignment, Natural Product Titles, and A3 Hash Resolution

* **Date:** 2026-09-15
* **Task / Milestone:** Checkpoint USAP-CATALOG-001-QA2 (Current-Site Catalog Policy Alignment)
* **Scope & Implementation Summary:**
  * **Current-Site Catalog Policy Alignment:**
    - In accordance with project lead decision, current public USAP website is recognized as authority for catalog inclusion pending client list.
    - Retained all 16 canonical groups, 30 named models, and 1 unnamed line across the 6 families.
    - Removed repeated word `Reference` or `References` from all 16 public product group titles.
    - Restored natural first-party product names and concise, first-party grounded descriptions without repetitive boilerplate.
    - Stated current-site published relationships (R3500/DRC-4 pairing, DRC-3 with R3501/R3503) without converting them into guarantees of current purchase availability.
  * **Public Section Language Replacement:**
    - Eyebrow: `PRODUCT CATALOG`
    - Heading: `Products and Models in This Family`
    - Lead: `The following product groups and configurations are cataloged for <familyName>:`
    - Model List Label: `Models and Configurations:`
    - Shared Notice: `Product information below is based on USAP’s current public website and linked technical documents. Contact USAP engineering to confirm availability, configuration, compatibility, and final specifications for your application.`
  * **Four-Dimensional Catalog Metadata Separation:**
    - Domain records (`ProductGroupRecord`, `ProductModelRecord`) cleanly distinguish:
      1. *Source presence:* Listed on current USAP website
      2. *Commercial availability:* Not confirmed
      3. *Client approval:* Pending
      4. *Specification status:* Confirmed from current HTML / Provisional / HoldDisputedSpecs
    - Public display contains zero "legacy" or "historical" badges.
  * **Technical Resource Preservation for Lifecycle Classification:**
    - All 19 discovered datasheets and revisions preserved in catalog records.
    - Architectural capability for future lifecycle classification documented without deleting documentation or pages.
  * **A3 Archive Hash Discrepancy Resolution:**
    - Read-only hash recalculation on `project-input/USAP_Product_Catalog_A3_Family_Hero_Review_Package_2026-09-15.zip`:
      - Length: `34,654,781` bytes
      - SHA-256: `D5C60786BABE6533DA5DB1A30947649F621D038B410DDB7D7DD4DF41A96B4E79`
    - Confirmed repository copy is the exact validated archive; previous `61a5ac...` was a typographical error in prior reporting.
    - ZIP CRC integrity passed. The archive contains 46 total ZIP entries: 10 directory entries and 36 regular files. Of the 36 regular files, 35 are covered by `usap-product-catalog-a3/SHA256_MANIFEST.txt`, plus the manifest file itself. Archive remains unextracted and unintegrated.
  * **Automated Browser Audit Results (`family_pages_status_notice_audit.json`):**
    - Evaluated all 6 family routes in headless Chrome CDP:
      1. Log Periodic: Notice=PASS, Eyebrow=PASS, Heading=PASS, ModelsLabel=PASS, NoRefTitle=PASS, ProhibitedWords=[] [PASS]
      2. Portable & Transportable: Notice=PASS, Eyebrow=PASS, Heading=PASS, ModelsLabel=PASS, NoRefTitle=PASS, ProhibitedWords=[] [PASS]
      3. Aperiodic Loop: Notice=PASS, Eyebrow=PASS, Heading=PASS, ModelsLabel=PASS, NoRefTitle=PASS, ProhibitedWords=[] [PASS]
      4. NVIS: Notice=PASS, Eyebrow=PASS, Heading=PASS, ModelsLabel=PASS, NoRefTitle=PASS, ProhibitedWords=[] [PASS]
      5. Rotator & Control: Notice=PASS, Eyebrow=PASS, Heading=PASS, ModelsLabel=PASS, NoRefTitle=PASS, ProhibitedWords=[] [PASS]
      6. Tower Systems: Notice=PASS, Eyebrow=PASS, Heading=PASS, ModelsLabel=PASS, NoRefTitle=PASS, ProhibitedWords=[] [PASS]
    - Total cataloged groups across routes: 16
    - Total cataloged models across routes: 31 (30 named + 1 unnamed)
  * **Repaired & Regenerated Review Screenshots:**
    - `02_six_family_grid_1440px.png` (1072×1103 px): Full 6-card grid in 3×2 layout visible with 0 blank clipping.
    - `04_six_family_grid_768px.png` (720×1570 px): Full 6-card grid in 2×3 layout visible with 0 blank clipping.
    - `07_family_card_keyboard_focus_card_crop.png` (371×567 px): Active card centered with 15px padding, full 3px outline visible.
    - `06a`–`06f`: All 6 family routes captured at full scrollHeight displaying natural catalog titles and updated notice.
    - `01`, `03`, `05`, `07_viewport`, `08_responsive_*`: Full suite captured across 6 viewports.
  * **Commands & Formatter Verification:**
    - `dotnet build USAP.Web.sln --configuration Release --no-restore`: Clean (Exit Code 0).
    - `dotnet run` on CatalogValidationTests: 12 / 12 verification scenarios pass (1 positive catalog initialization/structure scenario, 11 negative integrity scenarios) (Exit Code 0).
    - `dotnet format USAP.Web.sln --verify-no-changes --no-restore`: Actual exit code 1. Exactly 19 pre-existing baseline `IDE0011` brace warnings in `Program.cs` lines 16-35. Zero warnings in new/modified code.
    - `git diff --check`: Clean (Exit Code 0).
    - `git status --short --branch`: The branch is correct (`feat/product-catalog-and-family-pages`) and the working tree contains the expected unstaged and uncommitted catalog changes; the staging area is empty.
  * **Portable Review Package Metrics:**
    - Package: `USAP-CATALOG-001-QA2R-review-package.zip`
    - Purpose: External review evidence package staged and verified in the artifact directory.
    - Contents: Review screenshots (including repaired 02, 04, 07 crop, and 06a–06f), machine-readable audit and test evidence, build and formatting results, asset hash registers, untracked files register, git status, and complete source snapshot.
    - Source Snapshot: `source_changes.diff` includes the complete tracked git diff and synthetic `/dev/null` sections for all 7 untracked catalog C# source files.
    - Untracked Files Register: `UNTRACKED_FILES_REGISTER.txt` enumerates all 14 untracked files (5 models, 2 services, 7 PNG assets) with sizes, SHA-256 hashes, and classifications.
    - Integrity: 100% forward-slash paths, ZIP CRC `testzip()` passed with 0 corruptions, 100% manifest match upon clean extraction.
    - Package Metrics: Final outer archive size, SHA-256 checksum, manifest-covered file count, and total entry count are reported in the external return report to avoid circular checksum dependencies.


---

## QA-012 — Checkpoint USAP-CATALOG-001-C2 Verification: Family Asset Integration, Card Unification, Guided Selection, and FAQ Completion

* **Date:** 2026-09-16
* **Task / Milestone:** Checkpoint USAP-CATALOG-001-C2 (Family Asset Integration, Card Unification, Guided Selection, and FAQ Completion)
* **Scope & Implementation Summary:**
  * **Asset Promotion & Domain Integration:**
    - Promoted 16 approved asset derivatives from A3 and A3S into `src/USAP.Web/wwwroot/images/products/` with 100% byte-identical hash preservation against source archives:
      - 1 Family Card: Rotator & Control Candidate B (`usap-family-card-rotator-control-r3500-drc4-a3s-recommended-v1.png`). Strictly rejected Candidate A.
      - 6 Desktop Family Heroes (1536×576) & 6 Mobile Family Heroes (768×768): Log Periodic (A3 Candidate A V2), Portable & Transportable (A3S), Aperiodic Loop (A3 Candidate A V1), NVIS (A3 Candidate A V2), Rotator & Control (A3S), Tower Systems & Accessories (A3S).
      - 3 Dedicated Rotator Visuals (1600×1000): DRC-3, DRC-4, and R3500/DRC-4 relationship.
    - Domain model updated with explicit `ResponsiveHeroAsset` classifications without defaults.
    - Approval status across all promoted assets: `provisional — USAP review pending`.
  * **Responsive Hero `<picture>` & Non-Overlapping Breakpoint:**
    - `<picture>` element with `<source media="(max-width: 47.999rem)" ...>` avoids 768px layout collisions with desktop `@media (min-width: 48rem)`.
    - Programmatic browser audit verified selected resource: 390px and 767px select 768×768 mobile crop; 768px, 1024px, and 1440px select 1536×576 desktop image. At 768px, image choice and layout breakpoint strictly agree.
    - Restrained copy-safe left overlay (`.internal-hero-overlay--family`) and per-family focal positions preserved.
  * **Card Unification & Homepage Section 5 Integration:**
    - Created shared partial component `Pages/Shared/_ProductFamilyCard.cshtml` implementing stretched-link single Tab-stop pattern.
    - Homepage Section 5 updated via `IProductCatalogService.GetFeaturedFamilies()` displaying 4 canonical families linking to `/products/{slug}`, with section-level `View All Products` linking to `/products`.
    - Obsolete `.product-card` and `.product-media` CSS rules audited and removed from `homepage.css`.
    - Context-sensitive min-height custom properties (`--family-card-eyebrow-min-height`, `--family-card-title-min-height`, `--family-card-summary-min-height`) tuned for 3-column (2.25/3.25/4.5rem) and 4-column (2.75/4.25/5.5rem) contexts, with mobile reset to `auto`.
    - Equal row heights, bottom-aligned CTAs, no line-clamping, no ellipses, no clipping verified.
    - Computed style audit confirms 100% parity across borders, radii, shadows, media ratios, typography, hover elevation, and focus rings.
  * **Actionable Engineering Guidance & FAQ:**
    - Reworked Guidance section into 3 actionable pathways (Deployment & Mobility, Coverage & Propagation, Positioning & Infrastructure) with cross-family links.
    - Replaced duplicate 2-button CTA with a restrained inline continuation notice linking to `#products-faq` and `#closing-cta-heading`.
    - Implemented 6-question Product Selection FAQ on `/products` using native semantic `<details>`/`<summary>` without JavaScript. Suppressed marker rotation under `prefers-reduced-motion`.
  * **Dedicated Rotator Product Visuals Section:**
    - 3 `<figure>`/`<figcaption>` cards on `/products/antenna-rotator-control-systems` with single restrained section note.
  * **DRC-4 Branding Limitation Documented:**
    - The horizontal controller-face wordmark is source-derived and provisional. It is not an approved official alternate logo lockup. Documented for future replacement when vector brand artwork is supplied.
  * **Automated Browser & Regression Route Coverage (10 routes verified):**
    - HTTP 200 OK: `/`, `/products`, `/products/log-periodic-antennas`, `/products/portable-transportable-antennas`, `/products/aperiodic-loop-antennas`, `/products/nvis-antennas`, `/products/antenna-rotator-control-systems`, `/products/tower-systems-accessories`, `/about-us`, `/technical-resources`, `/contact-us`, `/request-a-quote`.
    - HTTP 404 Not Found: `/products/invalid-slug` (verified via custom 404 handler).
    - Zero console errors across all routes.
    - Zero network 4xx/5xx failures.
    - Zero horizontal overflow across 6 viewports (390, 767, 768, 1024, 1440, 1920).
    - Exactly 1 Tab stop per card, hit-testing validated.
    - Complete absence of A3R or superseded asset requests verified.
  * **Commands & Formatter Verification:**
    - `dotnet restore USAP.Web.sln`: Clean (Exit Code 0).
    - `dotnet build USAP.Web.sln --configuration Debug --no-restore`: Clean (Exit Code 0, 0 warnings, 0 errors).
    - `dotnet build USAP.Web.sln --configuration Release --no-restore`: Clean (Exit Code 0, 0 warnings, 0 errors).
    - `dotnet format USAP.Web.sln --verify-no-changes --no-restore`: Actual exit code 1. Exactly 19 pre-existing baseline `IDE0011` brace warnings in `Program.cs` lines 16-35. Zero warnings introduced in any new or modified file.
    - `dotnet run` on CatalogValidationTests: 18 / 18 scenarios pass (12 baseline scenarios + 6 new C2 responsive hero and featured family contract scenarios) (Exit Code 0).
    - `git diff --check`: Clean (Exit Code 0).
    - `git status --short --branch`: Clean (`## feat/product-catalog-and-family-pages`, 100% unstaged and uncommitted).
  * **Review Package & Artifacts:**
    - Portable review package `USAP-CATALOG-001-C2-review-package.zip` generated in artifact directory.
    - 17 review screenshots captured.
    - Source snapshot `source_changes.diff` includes tracked git diff and synthetic `/dev/null` sections for untracked source files.
    - Untracked files register, asset hash registers, audit summaries, and build results included.
    - 100% forward-slash paths, CRC `testzip()` passed with 0 corruptions, 100% manifest match upon clean extraction.

---

## QA-013 — USAP-CATALOG-001-C2R / C2R1 / C2R2 Verification: Product Catalog Alignment, Compact Rhythm, Rich Summaries, and Evidence Remediation

* **Date:** 2026-09-16
* **Milestone / Task:** USAP-CATALOG-001-C2R / USAP-CATALOG-001-C2R1 / USAP-CATALOG-001-C2R2 (Catalog Card Alignment, Compact Rhythm, Product Disclosure Summary, Support Placement, and Evidence Remediation)
* **Scope & Implementation:**
  * Strict preservation of domain model architecture (`ProductFamilyRecord`, `ProductGroupRecord`, `ProductModelRecord`, `ProductResourceRecord`). `ProductModelRecord.cs` restored to checkpoint version.
  * Addition of non-destructive `DisplayTitle` fallback property on `ProductFamilyRecord` and `AssociatedAsset` on `ProductGroupRecord`.
  * Dedicated card partial strategy: `_HomeProductFamilyCard.cshtml` (2×2 desktop/tablet, 1×4 mobile) and `_ProductFamilyCard.cshtml` (3×2 desktop, 2×3 tablet, 1×6 mobile) with 16:10 aspect ratio and single Tab-stop click activation.
  * Compact Catalog Card Rhythm & Shared Reservations (C2R2): Replaced oversized C2R1 vertical reservations (`2.25lh`, `2.4lh`, `5lh` producing ~626px cards) with the compact, content-driven component contract on multi-column breakpoints (`48rem` and `64rem`):
    - `--family-card-eyebrow-min-height: 2lh;` (reserves exactly 2 lines for eyebrow)
    - `--family-card-title-min-height: 2lh;` (reserves exactly 2 lines for heading)
    - `--family-card-summary-min-height: auto;` (allows summary to take natural content height)
    - Bottom CTA alignment preserved across cards in each row using card body flex layout and `margin-top: auto` on `.product-family-card__action-wrapper`. Surplus space is not distributed via `justify-content: space-between`.
  * Live Card Compaction & Alignment Verification:
    - 1440px Desktop: Row 1 reduced from 626.05px to 526.75px (-99.30px reduction); Row 2 reduced from 626.05px to 553.00px (-73.05px reduction), both exceeding the >= 60px height reduction requirement.
    - Row 1: Summary top (1106.20px / 1106.17px / 1106.17px) aligned within 0.0313px (<= 1px requirement); CTA top (1178.70px) aligned within 0.0000px; card heights equal (526.75px).
    - Row 2: Summary top (1656.92px / 1656.95px / 1656.95px) aligned within 0.0313px; CTA top (1755.70px) aligned within 0.0000px; card heights equal (553.00px).
    - NVIS summary top (1656.92px) aligns with Rotator (1656.95px) and Tower (1656.95px) within 0.0313px.
    - 768px Tablet (2-column): Pairs 1 and 2 reduced from 586.20px to 511.73px (-74.47px reduction); Pair 3 reduced from 586.20px to 536.67px (-49.53px reduction). Summary tops and CTA tops aligned within <= 0.0625px across all pairs.
    - 390px Mobile (single-column): All vertical reservations remain `auto`, natural content height preserved with zero horizontal overflow.
  * Conservative Public Copy for 1942 NVIS Series (C2R2): Removed unsupported "gap-free" claim from NVIS collapsed short description in `ProductCatalogService.cs`. Applied restrained public wording: "Near Vertical Incidence Skywave (NVIS) HF antenna systems covering 2–30 MHz for short-to-medium-range communications in roof-top, transportable, and ground-mount configurations." Zero operational performance guarantees or unverified claims.
  * Rich Collapsed Product Summaries (C2R1): Redesigned `<summary class="product-group-summary">` to expose `<h3>` heading, concise public description, configuration count badge, approved technical document badge (rendered only when approved documents exist), and visible "View details" action with disclosure indicator. Zero nested interactive elements inside `<summary>`.
  * Expanded disclosure body preserves models, allowed specifications, approved technical document links, Contact Engineering CTA, and associated visuals (for the 3 rotator groups). Clean text layout with zero empty media containers or placeholders for the other 13 groups. Duplicate description removed from body.
  * Configuration Support Placement (C2R1): Moved to the bottom of the family-page content flow: (1) Product catalog heading -> (2) Product disclosures -> (3) Return to All Product Families -> (4) Configuration Support panel -> (5) Shared closing CTA.
  * Robust Reversible Disclosure Animation (C2R1): Progressive enhancement in `disclosure.js` starts transitions from current rendered height and opacity without jumping on rapid repeated clicks. All inline styles and animations clean up on completion. `prefers-reduced-motion: reduce` toggles natively and immediately without animation.
  * Approved card display titles verified across all 6 families; canonical `Name` preserved for `<h1>`, breadcrumbs, and records.
  * Strict terminology compliance: DRC-3 ("Large industrial antenna-rotator control enclosure with display and control components.") and DRC-4 ("Tabletop antenna-rotator controller with digital display, rotary dial, and front controls.") with all prohibited claims purged.
  * Relocation of rotator visual assets into respective group disclosures (`r3500`, `drc-3`, `drc-4`).
  * Withholding of disputed specifications (`HoldDisputedSpecs`) and zero public exposure of governance fields.
  * Technical document default-deny allowlist (6 approved documents: `doc-lp-high-power`, `doc-lp-1018ba`, `doc-lp-1019`, `doc-1910-2024`, `doc-aperiodic`, `doc-t-3002-oct2016`).
  * Deferred individual-product asset matrix documented in `CONTENT_AND_ASSETS.md` covering all 16 groups.
* **Catalog Validation Suite (20 / 20 Scenarios Passing):**
  * Execution: `dotnet run --project "scratch/CatalogValidationTests/CatalogValidationTests.csproj" --configuration Release --no-restore` (Exit Code 0).
  * Assertions verified:
    1. Canonical 6 families in order, 16 groups distributed 5/3/1/1/5/1, 30 named models + 1 unnamed line record, summaries verified.
    2. Exact 4 featured homepage families in order.
    3. DisplayTitle fallback property and exact card titles across all 6 families.
    4. Canonical names preserved untouched.
    5. AssociatedAsset mapping: attached to r3500, drc-3, drc-4; null on other 13 groups.
    6. Exact DRC-3 and DRC-4 descriptions; zero prohibited terms found.
    7. Approved technical documents allowlist returns exact 6 approved records, excludes unapproved records.
    8. 13 negative validation tests (family counts, group counts, model counts, swaps, invalid heroes, invalid asset paths) throw expected exceptions.
* **Rapid Toggle & Animation Robustness Verification:**
  * Rapid 4-click activation test executed: initialOpen = false, finalOpen = false, expectedOpen = false, passed = true.
  * Post-animation cleanup: `inlineHeightClean = true`, `inlineOverflowClean = true`.
  * Reduced motion test (`prefers-reduced-motion: reduce`): `reducedMotionHandled = true` (instantaneous toggle without animation).
* **Family Disclosures & Content Audit:**
  * All 16 product groups audited with exact matching DOM selectors:
    * Non-null group titles: 16/16.
    * Concise collapsed summaries present: 16/16.
    * Content wrappers present: 16/16.
    * Models & configurations counted: 16/16.
    * Approved resources counted: 16/16.
    * Associated images present: exactly 3 (`r3500`, `drc-3`, `drc-4`); exactly 0 for remaining 13 groups.
    * Zero nested interactive elements inside `<summary>`: 16/16.
    * Leaked governance fields (`ConflictHolds`, `SourceNotes`, `HoldDisputedSpecs`, `ProjectSelected...`, `provisional — USAP review pending`): 0 violations.
    * Configuration Support placement order valid on all 6 family pages: TRUE.
* **Focused Browser Screenshots Captured (C2R2):**
  * Pre-capture verification: scroll into view, bounding rect materially visible, layout stabilized, image decode complete (`complete && naturalWidth > 0`), `currentSrc` verified.
  * Focused inventory:
    * `01_catalog_grid_compact_1440px.png`: Full `/products` grid at 1440px showing compact layout.
    * `02_catalog_row2_nvis_rotator_tower_alignment_1440px.png`: Second card row at 1440px, clearly showing NVIS, Rotator, and Tower alignment.
    * `03_catalog_grid_768px.png`: Full `/products` grid at 768px showing balanced 2-column layout and pairwise alignment.
    * `04_catalog_cards_mobile_390px.png`: Product cards at 390px confirming mobile remains compact and unchanged.
    * `05_catalog_card_keyboard_focus_compact.png`: Prominent keyboard focus outline (`outline: 3px solid rgb(0, 87, 184)`) on focused card link after compaction.
* **Live DOM Horizontal Overflow Audit (48 Live DOM Measurements):**
  * Measured directly from live browser DOM via Chrome DevTools Protocol (`document.documentElement.scrollWidth`, `document.documentElement.clientWidth`, `document.body.scrollWidth`, `document.body.clientWidth`).
  * Tested across all 6 viewports: 390, 767, 768, 1024, 1440, 1920 px.
  * Tested across all 8 required routes: `/`, `/products`, `/products/log-periodic-antennas`, `/products/portable-transportable-antennas`, `/products/aperiodic-loop-antennas`, `/products/nvis-antennas`, `/products/antenna-rotator-control-systems`, `/products/tower-systems-accessories`.
  * Calculation: `hasHorizontalOverflow = documentElement.scrollWidth > documentElement.clientWidth || body.scrollWidth > body.clientWidth`.
  * Result: 48 / 48 tests passed (0 overflow failures, `c2r2_overflow_evidence.json`).
* **Console & Network Logs:**
  * Console errors: 0 (`c2r2_console_errors.json`).
  * Network failures (4xx/5xx): 0 (`c2r2_network_failures.json`).
* **Route Status Verification:**
  * 12 HTTP-200 routes and 1 HTTP-404 route verified passing (`c2r2_route_status_results.json`).
* **Packaged Diff Encoding Verification (C2R2):**
  * Subprocess Git output captured as raw binary bytes and written without intermediate CP-1252 decoding.
  * Corrupted characters scan (U+00C3, U+00E2, U+00C2, U+FFFD): Exactly 0 instances found in modified repository files and generated `source_changes.diff`.
* **Build & Code Quality:**
  * `dotnet build USAP.Web.sln --configuration Release`: 0 Errors, 0 Warnings (Exit code 0).
  * `dotnet build USAP.Web.sln --configuration Debug -p:OutputPath="..."`: 0 Errors, 0 Warnings (Exit code 0).
  * `dotnet format whitespace USAP.Web.sln --verify-no-changes --no-restore`: Clean (Exit code 0).
  * `dotnet format USAP.Web.sln --verify-no-changes --no-restore`: Exit code 1 (attributed strictly to baseline `IDE0011` brace warnings in `Program.cs` lines 16-35; zero formatting issues in modified files).
  * `git diff --check`: Clean (0 whitespace errors).

---

## QA-014 — Checkpoint USAP-CATALOG-002-C1 / C1R1 Verification: Evidence-Safe Product Detail Presentation, 30+1 Reconciliation, and Full Visual Coverage

* **Date:** 2026-09-17 (Recovered & Completed 2026-09-18)
* **Milestone / Task:** USAP-CATALOG-002-C1 / C1R1 (Interrupted Corrective Implementation Recovery and Completion)
* **Authority & Preflight Hashes:**
  * Controlling implementation prompt: `project-inputs/USAP-CATALOG-002-C1-EVIDENCE-SAFE-PRODUCT-DETAIL-PRESENTATION-IMPLEMENTATION-PROMPT.md`
    - Line count: 885 lines (Verified).
    - SHA-256: `710767E03947CDA5323F52A6FCB6AFE126009E6504B8CE21FF8B70D1BD96EC2B` (Verified).
  * `USAP-CATALOG-002-R2-research-package.zip`: `3E6A4C00B5B9D5A51165869582A767380C79E1363C7A9C5B7ECA2B0B55AD296F` (Verified).
  * `USAP-CATALOG-002-R2-canonical-document-source-package.zip`: `28D66FDD365DF4289C1390F6332C9B8256FB7A25D5C1C70248C43AEA3ED0A529` (Verified).
  * `USAP-CATALOG-002-A1-Final-Review-Package-2026-09-17.zip`: `D8C11C85BD42AD220C79C48C657B33D47A4E88B39F618DF72D98840EC5E94BCD` (Verified).
* **Build and Formatting Verification:**
  * `dotnet restore USAP.Web.sln`: 0 Warnings, 0 Errors (Exit code 0).
  * `dotnet build USAP.Web.sln --configuration Release --no-restore`: 0 Warnings, 0 Errors (Exit code 0).
  * `dotnet format whitespace USAP.Web.sln --verify-no-changes --no-restore`: Clean (Exit code 0).
  * `dotnet format USAP.Web.sln --verify-no-changes --no-restore`: Modified files completely clean (Exit code 1 attributable only to pre-existing baseline `IDE0011` brace warnings in `Program.cs` lines 16-35).
  * `git diff --check`: Clean (0 whitespace or character encoding errors).
* **Deterministic Catalog Invariant Verification:**
  * `ProductCatalogService.ValidateCatalog()` verified at startup:
    1. Exact 6 canonical families in order (`log-periodic-antennas`, `portable-transportable-antennas`, `aperiodic-loop-antennas`, `nvis-antennas`, `antenna-rotator-control-systems`, `tower-systems-accessories`).
    2. Exact 16 product groups matching 5/3/1/1/5/1 distribution.
    3. Exactly 30 published named models across 15 groups.
    4. Exactly 1 unnamed Aperiodic group record (`HasPublishedModelNumber = false`, empty `Models` list, procurement note rendered). Total catalog records = 31.
    5. All 6 1942 NVIS configurations published (`1942-RT`, `1942-TA`, `1942-GM`, `1942-RT-LP`, `1942-TA-LP`, `1942-GM-LP`), with low-power variants retaining conservative configuration-level role identification only without disputed power, weight, or coverage ratings.
    6. Non-empty group characteristics on all 16 groups.
    7. Valid non-empty model codes with valid characteristic pairs on all 30 models.
    8. Exactly 16 product-group visual assets wired with matching files on disk and conservative alt text.
    9. Exactly 3 Engineering Guidance card visuals wired on `/products` (combined wide visual absent).
    10. Exactly 6 interim technical documents allowlisted.
    11. Zero prohibited governance tokens found in metadata (`HoldDisputedSpecs`, `ConflictHolds`, `SourceNotes`, `ApprovalStatus`, `PublicationRecommendation`, `SpecificationStatus`, `CommercialAvailability`, `ClientConfirmation`, confidence scores).
* **Route Verification (Isolated Release Server, Port 5299):**
  * `/`: HTTP 200 OK
  * `/products`: HTTP 200 OK
  * `/products/log-periodic-antennas`: HTTP 200 OK
  * `/products/portable-transportable-antennas`: HTTP 200 OK
  * `/products/aperiodic-loop-antennas`: HTTP 200 OK
  * `/products/nvis-antennas`: HTTP 200 OK
  * `/products/antenna-rotator-control-systems`: HTTP 200 OK
  * `/products/tower-systems-accessories`: HTTP 200 OK
  * `/about-us`: HTTP 200 OK
  * `/technical-resources`: HTTP 200 OK
  * `/contact-us`: HTTP 200 OK
  * `/request-a-quote`: HTTP 200 OK
  * `/products/non-existent-family`: HTTP 404 Not Found (executed via custom `/not-found` page)
* **DOM Content & Presentation Audit:**
  * Product family cards: Exactly 6 family cards, each constructed with an outer `<a>` anchor and inner `<span class="card-link">` (zero nested buttons or links).
  * Product group disclosures: Exactly 16 across 6 families.
  * Model cards (`.product-model-card`): Exactly 30 named model cards rendered across 15 groups.
  * Aperiodic loop group renders 0 model cards and displays the dedicated procurement / system configuration note.
  * 1942 NVIS group displays all 6 configuration codes (`1942-RT`, `1942-TA`, `1942-GM`, `1942-RT-LP`, `1942-TA-LP`, `1942-GM-LP`).
  * Contain-style visual presentation verified active for `lp-1112mr`, `1910`, and `1942`.
  * Technical document links: Exactly 6 interim links rendered across catalog pages.
  * Breadcrumbs: Verified on all 6 family pages positioned in `<nav aria-label="Breadcrumb">` at the top-left of the main content container below the hero.
  * Engineering Guidance visuals: Exactly 3 card images rendered on `/products`; combined wide visual absent.
  * Prohibited phrases and tokens: Exactly 0 occurrences in rendered HTML.
* **Responsive & Accessibility Verification:**
  * Tested viewports: 390×844, 768×1024, 1024×768, 1025×768, 1440×900, 1920×1080.
  * Zero horizontal overflow on any route across all viewports (`scrollWidth <= innerWidth`).
  * Native `<details>`/`<summary>` disclosures expand and collapse cleanly without JavaScript.
  * Focus-visible outlines (3px solid `#0057b8`) visible on keyboard Tab navigation across family cards, disclosure headers, and technical links.
  * All animations suppressed under `prefers-reduced-motion: reduce`.

---

## QA-015 — Checkpoint USAP-CATALOG-002-C1R2 Verification: Final Compliance Repair, Exact A1 Derivatives, and Evidence Correction

* **Date:** 2026-09-18
* **Milestone / Task:** USAP-CATALOG-002-C1R2 (Final Compliance Repair and Evidence Correction)
* **Authority & Preflight Hashes:**
  * Controlling implementation prompt: `project-inputs/USAP-CATALOG-002-C1-EVIDENCE-SAFE-PRODUCT-DETAIL-PRESENTATION-IMPLEMENTATION-PROMPT.md`
    - Line count: 885 lines (Verified).
    - SHA-256: `710767E03947CDA5323F52A6FCB6AFE126009E6504B8CE21FF8B70D1BD96EC2B` (Verified).
* **Deficiencies Addressed & Compliance Repairs:**
  1. **Layout Viewport Rectification (CDP Device Metrics Override):**
     - Formally acknowledged and documented that the C1R1 390px test had an actual layout width of 980px due to Chrome headless fallback under `mobile: True`.
     - Rectified using Chrome DevTools Protocol `Emulation.setDeviceMetricsOverride` with `--hide-scrollbars` and `mobile: False`.
     - Verified exact layout width equality: `window.innerWidth === requestedWidth` and `document.documentElement.clientWidth === requestedWidth` across all 6 viewports (390, 768, 1024, 1025, 1440, 1920 px).
     - Live measured results across 8 routes (48/48 test runs passed): 390px measured layout width = exactly 390px, scrollWidth = 390px (0px horizontal overflow).
  2. **Restoration of Prohibited Family-Card and Family-Hero Changes:**
     - Restored pre-C1 verified asset paths and accurate alt text:
       - Aperiodic family card: `/images/products/usap-family-card-aperiodic-loop-element-v1.png`
       - Aperiodic desktop hero: `/images/products/usap-family-aperiodic-loop-antennas-hero-candidate-a-v1-desktop-preview.png`
       - Aperiodic mobile hero: `/images/products/usap-family-aperiodic-loop-antennas-hero-candidate-a-v1-mobile-crop.png`
       - NVIS family card: `/images/products/usap-family-card-nvis-1942-family-visual-v1.png`
       - NVIS desktop hero: `/images/products/usap-family-nvis-antennas-hero-candidate-a-v2-desktop-preview.png`
       - NVIS mobile hero: `/images/products/usap-family-nvis-antennas-hero-candidate-a-v2-mobile-crop.png`
       - Rotator/Controller family card: `/images/products/usap-family-card-rotator-control-r3500-drc4-a3s-recommended-v1.png`
     - Automated live image audit across `/products` and all 6 family routes verified `img.complete && img.naturalWidth > 0 && img.naturalHeight > 0` for every rendered image (39/39 passed; 0 broken images).
  3. **A1 Master Asset Register & Derivative Verification:**
     - Verified all 16 Section 9.7 assets (13 product groups and 3 Engineering Guidance cards) on disk are byte-for-byte identical to the verified A1 master package:
       - Preserved original filenames and destinations.
       - Exact 1600 × 1000 px dimensions preserved.
       - 100% SHA-256 hash match against Section 9.7 specifications.
       - Preserved the 3 existing rotator assets (`r3500`, `drc-3`, `drc-4`).
       - Asset hash register generated matching Section 9.7 requirements.
  4. **Public-Copy Violations Purged:**
     - DRC-4: Purged "continuous-rotation capability on compatible rotators" and compatibility claims. Rotation characteristic set to "360-degree continuous-rotation control".
     - DRC-3: Purged "documented for selected rotator systems".
     - T-3002: Removed "50 ohms pressurized" feedline, "1-5/8 in EIA coaxial flange", and introductory feedline claims.
     - V-4213: Replaced "Complete field-system package record" with "Portable discone equipment-package configuration" on `V-4213AD`.
     - Full automated scan of catalog data and views confirms 0 occurrences of prohibited terms.
  5. **Documentation Parity & Terminology Consistency:**
     - `docs/DECISION_LOG.md`: Corrected DEC-032 product inventory to exact public projection: Log Periodic: 8, Portable: 6, Aperiodic: 0 (+1 unnamed record), NVIS: 6, Rotator: 5, Tower: 5 (Total: 30 named identifiers + 1 unnamed record = 31 records).
     - `docs/CONTENT_AND_ASSETS.md`: Removed obsolete default-deny/withheld paragraph; established consistent policy distinguishing the 6 interim C1 product-page links from the planned 17 canonical document migration under `USAP-TECHDOC-001`.
     - Terminology renamed: `ApprovedResourceIds` -> `InterimResourceIds`. `ProductResourceRecord` documented as an extensible public-facing record in the current interim product-document set.
* **Build and Formatting Verification:**
  * `dotnet build USAP.Web.sln --configuration Release --no-restore`: 0 Errors, 0 Warnings (Exit code 0).
  * `dotnet format whitespace USAP.Web.sln --verify-no-changes --no-restore`: Clean (Exit code 0).
  * `dotnet format USAP.Web.sln --verify-no-changes --no-restore`: Clean on all modified files (Exit code 1 attributable only to baseline `IDE0011` warnings in `Program.cs`).
  * `git diff --check`: Clean (0 whitespace/encoding errors).
* **Review Package:**
  * `USAP-CATALOG-002-C1R2-final-compliance-review-package.zip` generated in artifact directory.
  * Contains exact viewport evidence JSON, image integrity audit, Section 9.7 asset hash register CSV, build outputs, source diff, and complete review screenshots demonstrating all required states and components.

---

## QA-016 — Checkpoint USAP-CATALOG-002-C1R4 Verification: Product Overview Disclosure, Full-Width FAQ Layout, and Inventory Reconciliation

* **Date:** 2026-09-18
* **Milestone / Task:** USAP-CATALOG-002-C1R4 (Product Overview Disclosure and FAQ Layout Refinement)
* **Authority & Guiding Inputs:**
  * Controlling implementation prompt: `project-inputs/USAP-CATALOG-002-C1-EVIDENCE-SAFE-PRODUCT-DETAIL-PRESENTATION-IMPLEMENTATION-PROMPT.md` (885 lines, SHA-256: `710767E03947CDA5323F52A6FCB6AFE126009E6504B8CE21FF8B70D1BD96EC2B`).
  * Binding C1R4 clarifications approved by project lead:
    1. Aperiodic badges and disclosure wording: omit model-count badge; preserve Technical Document badge; disclosure labeled `View Configuration & Technical Details` / `Hide Configuration & Technical Details`.
    2. Meaningful always-visible overview copy: short summary + distinct expanded introduction (non-duplicative); expanded area focuses on models/specs/resources/engineering guidance.
    3. Key-characteristic selection: up to 3 safe group-level characteristics in intended order; no model-specific, disputed, or filler values.
    4. Unique accessible disclosure names: assistive technology receives full group context (`View Models & Specifications for [Group Title]`) while preserving native state announcement.
    5. Progressive enhancement: native `<details>/<summary>` baseline; `.product-group-disclosure` retained for `disclosure.js`; respects `prefers-reduced-motion`.
    6. FAQ layout: full standard container width (`width: 100%`); internal answer copy constrained to `max-width: 75ch`; shared alignment with section content.
    7. Mechanically verified inventory: 30+1 canonical inventory (6 families, 16 groups, 30 named models/configurations, 1 unnamed Aperiodic group = 31 records); exact set equality; zero invalid C1R3 dummy identifiers.
    8. Image metrics separation: separate reporting of total rendered `<img>` occurrences, unique image URLs, loaded occurrences, and broken occurrences.
    9. Server & final-build order: record agent server PID, shut down cleanly before final Release build, exit code 0 required.
    10. Complete evidence package: collapsed & expanded desktop and 390px comparisons; complete FAQ width in container context.
* **Layout & Style Refinements Verified:**
  * `/products` FAQ Section:
    - `.faq-list` occupies full container width (`width: 100%`).
    - `.faq-content p` constrained to `max-width: 75ch` for reading comfort.
    - Border, focus outline, and padding aligned consistently at 1440px desktop, 768px tablet, and 390px mobile.
  * Family Pages Product Group Card (`Pages/Products/Family.cshtml` & `site.css`):
    - Outer `<article class="product-group-card" id="@group.SectionAnchor">`.
    - Always-visible `.product-group-overview`: 2-column desktop grid (image ~40% width, min 280px / max 420px; text flex-1), stacked mobile column.
    - Product group image remains visible at all times, including when disclosure is collapsed.
    - Overview displays badges, heading, safe short summary, distinct introduction copy, and up to 3 safe group-level key characteristics in `.product-group-overview__specs`.
    - Nested `<details class="product-group-disclosure">`: contains model cards, remaining specs, technical resources, and Contact Engineering guidance.
    - Accessible name on summary: `aria-label="View Models &amp; Specifications for @group.Title"`.
    - Aperiodic group customized: no model-count badge; disclosure labeled `View Configuration & Technical Details`.
* **Inventory Reconciliation:**
  * Canonical 30+1 verified across catalog data and live DOM:
    - Log Periodic: 8 models across 5 groups.
    - Portable: 6 models across 3 groups.
    - Aperiodic: 0 named models (+1 unnamed record) across 1 group.
    - NVIS: 6 models across 1 group.
    - Rotator: 5 models across 4 groups.
    - Tower: 5 models across 2 groups.
    - Total: 30 named model configurations + 1 unnamed record = 31 records.
  * Zero invalid C1R3 identifiers present.
* **Responsive & Horizontal Overflow Verification:**
  * Tested across 6 standard viewports: 390×844, 768×1024, 1024×768, 1025×768, 1440×900, 1920×1080.
  * Chrome DevTools Protocol verified exact layout width: `document.documentElement.clientWidth === requestedWidth`.
  * Zero horizontal overflow on `/products` and all 6 family routes.
* **Review Package:**
  * `USAP-CATALOG-002-C1R4-product-overview-and-faq-review-package.zip`.

---

## QA-017 — Checkpoint USAP-CATALOG-002-C1R5 Verification: Product Navigation, Configuration Support, Hero Resilience, and Asset Reliability Refinement

* **Date:** 2026-09-18
* **Milestone / Task:** USAP-CATALOG-002-C1R5 (Product Navigation, Configuration Support, Hero Resilience, and Asset Reliability Refinement)
* **Scope & Implementation Summary:**
  * **Family-Page Closing Sequence:**
    - Established strict sequence across all 6 family routes: (1) product groups -> (2) Configuration Support card -> (3) Return to All Product Families -> (4) closing CTA -> (5) footer.
    - Moved `Return to All Product Families` below Configuration Support and above closing CTA. Retained `.btn-outline` treatment.
    - Updated Configuration Support CTA to shared `.btn-primary` using USAP red brand color with zero duplicate CSS button rules.
  * **Product Navigation (Desktop & Mobile):**
    - Semantic `<details id="nav-products-dropdown">` and `<summary class="nav-dropdown-toggle">` implemented in `_Header.cshtml`.
    - Menu contains All Products (`/products`) plus all 6 family routes (`log-periodic-antennas`, `portable-transportable-antennas`, `aperiodic-loop-antennas`, `nvis-antennas`, `antenna-rotator-control-systems`, `tower-systems-accessories`).
    - Active route highlighting: Products highlighted on `/products` and all 6 family routes.
    - Not hover-only; accessible via keyboard (Enter/Space to toggle, Escape to dismiss and return focus to summary, Tab navigation), click outside dismissal on desktop.
    - Mobile menu: toggling Products dropdown does not close the mobile drawer. Clicking any anchor closes the drawer.
    - Usable without JavaScript as native HTML disclosure.
  * **Hero Resilience & Scaling Correction:**
    - Content-driven height architecture verified across viewports (390×844, 768×1024, 1024×768, 1440×900, 1920×1080, 2560×1440) and zoom levels (80%, 100%, 125%).
    - Fluid padding `clamp(var(--space-8), 4vw, var(--space-12))`, `overflow-wrap: break-word`, `box-sizing: border-box`.
    - Mechanical bounding box assertions verified: `contentRect.bottom <= heroRect.bottom + 1` across all viewports and zoom levels with 0 clipping.
    - Breadcrumbs remain positioned below family heroes in Section 2.
  * **Broken Product-Family Image Investigation & Verification:**
    - Investigated Aperiodic family card: asset file `usap-family-card-aperiodic-loop-element-v1.png` exists on disk (2,098,672 bytes) and returns HTTP 200 with Content-Type `image/png`.
    - Discovered root cause of prior reported broken screenshot: legacy pre-C1R2 screenshot artifact (`06_products_landing_1920x1080.png`) showing old alt text `"mounted on a field tripod"` was present in review staging, exacerbated by lazy loading in headless browser captures.
    - Dual verification enforced: HTTP 200 response + content-type AND browser DOM completion (`naturalWidth > 0 && naturalHeight > 0`).
  * **Review Package:**
    - `USAP-CATALOG-002-C1R5-navigation-hero-and-support-review-package.zip`.

---

## QA-018 — Checkpoint USAP-CATALOG-002-C1R6 Verification: Mobile Navigation Overlay, Family SEO Content, Accessibility, and Final Refinement

* **Date:** 2026-09-18
* **Milestone / Task:** USAP-CATALOG-002-C1R6 (Mobile Navigation Overlay, Family SEO Content, Accessibility, and Final Refinement)
* **Scope & Implementation Summary:**
  * **Mobile Navigation Fixed Overlay:**
    - Refactored opened mobile navigation to fixed viewport overlay (`position: fixed`, `top: var(--mobile-header-bottom, 4.25rem)`, `height: calc(100dvh - var(--mobile-header-bottom, 4.25rem))`, `overscroll-behavior: contain`).
    - Bounding-box assertions verify zero vertical displacement or reflow of `.site-header` or `.internal-hero` when mobile menu opens or closes.
    - Viewport background scroll-lock verified: `nav-open-lock` applies to `html` and `body`; `window.scrollY` saved on open and restored identically across 3 repeated open/close cycles without layout drift.
    - Focus containment verified: HTML5 `inert` applied to `#main-content`, `.site-footer`, and `.skip-link` when open, and cleanly restored on close.
    - Short viewport (375×500) internal overlay scrolling verified.
  * **Mobile Navigation Products Control Centering & Native Semantics:**
    - Centered mobile Products disclosure control (`.nav-dropdown-toggle--mobile`) with `justify-content: center` and `gap: var(--space-2)`. Label and rotating chevron SVG are visually centered as a unit while child links remain left-aligned.
    - Native `<details>/<summary>` semantics: Redundant `aria-haspopup` and `aria-expanded` removed from `<summary>`; native keyboard operation (<kbd>Enter</kbd>/<kbd>Space</kbd>) and screen reader announcements preserved.
  * **Product-Family SEO, Eyebrows, Headings, and Introductions:**
    - Replaced generic post-hero intro with family-specific eyebrows, H2s, and conservative R2-backed introductory paragraphs across all 6 family routes.
    - Hero H1 verified as primary, dominant page title (`clamp(1.875rem, 3.25vw + 0.5rem, 2.75rem)`); post-hero H2 is visibly subordinate (`clamp(1.5rem, 2.5vw, 1.875rem)` in `.family-groups-header`).
    - Unique `<title>` and `<meta name="description">` assigned per family. No page-level `noindex` applied; indexable production intent confirmed (`index, follow` default; global non-production protection via `X-Robots-Tag: noindex, nofollow`).
  * **Explicit Overview-Characteristic Curation:**
    - Replaced runtime `Take(3)` and `Skip(3)` slicing with canonical `OverviewCharacteristics` (0 to 3 items) and `DetailedCharacteristics` on `ProductGroupRecord`.
    - Deterministic startup validation in `ProductCatalogService` validates count <= 3, non-empty labels/values, label uniqueness within group, zero overlap between overview and detailed collections, and scans family SEO strings for prohibited tokens.
  * **Technical Document Status Confirmation:**
    - Exactly 17 canonical PDFs identified in R2; 6 provisionally linked in C1; canonical migration deferred to `USAP-TECHDOC-001`; public vs lead-gated status is client-pending.
  * **Scratch Handling Confirmation:**
    - `scratch/audit_and_package_c1r5.py` left untouched in repository root per instructions; new C1R6 automation executed exclusively in external brain scratch storage.
  * **Review Package:**
  * `USAP-CATALOG-002-C1R6-mobile-navigation-and-family-seo-review-package.zip`.

---

## QA-019 — Checkpoint USAP-CATALOG-002-C1R7 Verification: Header Interaction, Mobile Navigation, Products Hero Investigation, and C1R6 Recovery Verification

* **Date:** 2026-09-18
* **Milestone / Task:** USAP-CATALOG-002-C1R7 (Header Interaction, Mobile Navigation, Products Hero Investigation, and C1R6 Recovery Verification)
* **Scope & Implementation Summary:**
  * **Characteristic Audit Contradiction Resolution:**
    - Diagnosed the reported C1R6 audit finding (0 overview / 0 detailed characteristics across 16 groups).
    - Verified `ProductGroupRecord.cs`, `ProductCatalogService.cs`, and `Family.cshtml`: all 16 groups have intact, approved, curated characteristics (`lp-high-power`, `lp-1019`, `v-4213`, `lp-1402-1403`, `1910`, `aperiodic`, `1942`, `t-3002`).
    - Identified root cause as an audit script extraction defect (checking obsolete card selectors or pre-expansion DOM state). The underlying catalog implementation and data model were 100% correct; zero source data modifications required.
  * **Mobile Header Alignment & Centering:**
    - Logo remains left-aligned; mobile menu toggle positioned at far right of container (`margin-left: auto`).
    - Top-level mobile navigation links (Home, Products + chevron disclosure summary, Technical Resources, About Us, Contact Us, Request a Quote) verified horizontally centered using rendered DOM geometry (control center matches text center within 0.01px).
    - Non-displacing active state implemented via absolute `::before` accent bar, ensuring active route does not shift text centering.
    - Product family sub-links within expanded Products dropdown remain left-aligned and indented for readability.
  * **Content-Height Mobile Navigation Overlay & Translucent Backdrop:**
    - Mobile drawer refactored to content-driven height: `height: auto; max-height: calc(100dvh - var(--mobile-header-bottom, 4.25rem));`.
    - Terminated immediately after Request a Quote button plus padding (368.58px height on 390×844 viewport, leaving page visible beneath).
    - Subtly translucent click-away backdrop (`#nav-backdrop`, `rgba(0, 0, 0, 0.40)`) covers remainder of screen and dismisses menu on click without visual confusion.
    - On short viewports (375×500) and expanded Products submenu, max-height engages and internal scrolling operates smoothly (`overscroll-behavior: contain`).
    - Background page scroll-lock and HTML5 `inert` focus containment verified; 0px scroll drift across 3 consecutive open/close cycles.
  * **Premium Auto-Hiding Sticky Header:**
    - Implemented `position: sticky; top: 0;` auto-hiding header with hardware-accelerated `translate3d(0, -100%, 0)` transition (220ms ease).
    - Downward scroll beyond threshold hides header; upward scroll immediately restores it; ~200ms scroll idle restores it when scrolling stops.
    - Auto-hide is automatically suspended when mobile menu is open, desktop Products dropdown is open, or keyboard focus is inside `.site-header`.
    - Opening mobile menu while header is scrolled off-screen restores header smoothly before opening drawer.
    - `prefers-reduced-motion: reduce` completely removes transform animation (`transition: none`) while preserving visibility and functionality.
  * **Main Products Hero Investigation (Mandatory Investigation Only):**
    - Executed exhaustive markup, CSS cascade, breakpoint matrix, and computed style analysis across viewports (1366×768, 1440×900, 1536×864, 1920×1080, 2560×1440).
    - Proved 100% invariance in CSS rules, selectors, computed min-height, height (369.8px), padding, background-size, and background-position.
    - Mathematical crop analysis proved visual differences across desktop displays are the natural geometric consequence of `object-fit: cover` scaling an 8:3 source image against wider container aspect ratios (5.2:1 at 1920px crops 175px from top; 6.9:1 at 2560px crops 295px from top).
    - Strictly adhered to binding no-change boundary: 0 markup, style, or asset modifications to `/products` hero or family heroes.
  * **Post-Header Verification of Products Hero:**
    - Verified hero bounding box after header implementation: `width: 1440px`, `height: 369.797px`, `padding: 48px`, matching baseline measurements exactly.
  * **Build, Formatting, & Route Verification:**
    - `dotnet restore USAP.Web.sln` (exit code 0).
    - `dotnet build USAP.Web.sln --configuration Release --no-restore` (exit code 0, 0 warnings, 0 errors).
    - `dotnet format whitespace USAP.Web.sln --verify-no-changes --no-restore` (exit code 0).
    - `git diff --check` (exit code 0).
    - All canonical routes verified HTTP 200; `/products/non-existent-family-xyz` verified true HTTP 404.
    - Zero horizontal overflow across all 11 viewports (320px to 2560px).
* **Review Package:**
  * `USAP-CATALOG-002-C1R7-header-navigation-and-products-hero-investigation-review-package.zip`.

---

## QA-020 — Checkpoint USAP-CATALOG-002-C1R8 Verification: Always-Visible Sticky Header Reconciliation and Focused Navigation QA

* **Date:** 2026-09-22
* **Milestone / Task:** USAP-CATALOG-002-C1R8 (Always-Visible Sticky Header Reconciliation and Focused Navigation QA)
* **Scope & Implementation Summary:**
  * **Always-Visible Sticky Header Architecture:**
    - Reconciled `.site-header` to `position: sticky; top: 0;` persistent behavior per latest project-lead direction.
    - Completely removed all auto-hide JavaScript machinery from `site-navigation.js`: directional scroll constants (`DELTA_DOWN`, `DELTA_UP`, `TOP_THRESHOLD`), idle restoration timer (`IDLE_DELAY`), scroll history (`lastScrollY`), rAF frame handler (`handleScrollFrame`), scroll event listener (`onScroll`), suspension check (`isSuspended`), and reveal helpers (`showHeader`, `hideHeader`).
    - Completely removed auto-hide CSS from `site.css`: `.site-header.is-hidden`, negative translate transform, `will-change: transform`, and transform transition. Removed obsolete inline style overrides from `openNav()` and transform/transition overrides from `.nav-open-lock .site-header`.
    - Maintained stacking context: `.site-header` uses `z-index: var(--z-sticky)` (200), elevated to `var(--z-overlay)` (300) when mobile menu is open.
  * **Preserved C1R7/C1R7R1 Mobile Navigation Architecture:**
    - Mobile header alignment: Logo left-aligned; toggle far right (`margin-inline-start: auto`); full container width utilized.
    - Centered top-level controls: Home, Products (label + rotating chevron centered as one unit via `.nav-dropdown-summary-group`), Technical Resources, About Us, Contact Us, Request a Quote.
    - Left-aligned indented submenu links (`padding-left: var(--space-10)`).
    - Non-displacing active-route indicator via absolute left accent bar (`::before`).
    - Content-height drawer: `height: auto; max-height: calc(100dvh - var(--mobile-header-bottom)); overflow-y: auto; overscroll-behavior: contain`.
    - Stable header offset measurement: `--mobile-header-bottom` computed directly from stable `header.offsetHeight`.
    - Translucent click-away backdrop (`#nav-backdrop`, `rgba(0, 0, 0, 0.40)`) with outside tap dismissal.
    - Background page scroll lock (`html.nav-open-lock, body.nav-open-lock`) and HTML5 `inert` background focus containment with zero scroll drift across repeated open/close cycles.
    - Initial enhanced-menu collapse does not perform scroll restoration, preserving fragment URLs and browser-restored scroll states.
    - Desktop breakpoint transition (>=1024px) cleanly tears down open mobile state, backdrop, scroll-lock, and inert attributes.
  * **Desktop Products Disclosure:**
    - Native `<details>/<summary>` disclosure preserved with outside click and Escape dismissal, returning focus to `<summary>`.
  * **Products Hero & Catalog Invariance:**
    - Zero modifications to `/products` landing hero, family heroes, catalog data, characteristics, product assets, or routing.
  * **Build, Formatting, & Route Verification:**
    - `dotnet restore USAP.Web.sln` (exit code 0).
    - `dotnet build USAP.Web.sln --configuration Release --no-restore` (exit code 0, 0 warnings, 0 errors).
    - `dotnet format whitespace USAP.Web.sln --verify-no-changes --no-restore` (exit code 0).
    - `git diff --check` (exit code 0).
* **Review Package:**
  * `USAP-CATALOG-002-C1R8-always-visible-sticky-header-reconciliation-review-package.zip`.

---

## QA-021 — Checkpoint USAP-CATALOG-002-C1R9 Verification: Shared Internal-Hero Geometry and Full-Image Presentation Reconciliation

* **Date:** 2026-09-22
* **Milestone / Task:** USAP-CATALOG-002-C1R9 (Shared Internal-Hero Geometry and Full-Image Presentation Reconciliation)
* **Scope & Implementation Summary:**
  * **Shared Baseline Height & Height Parity:**
    - Established `--internal-hero-min-height: 24.2875rem;` (~388.6px) derived directly from product-family hero geometry at 1440×900.
    - Verified exact height parity (388.6px ±0.0px) across all 9 normal internal pages (`/products`, `/contact-us`, `/request-a-quote`, and all six `/products/{familySlug}` routes) at 1440×900 and 1920×1080.
    - Confirmed fluid content resilience (`height: auto; min-height: ...;` without max-height constraints). `/about-us` expands naturally to 427.1px to accommodate its longer copy without clipping.
  * **Full-Image Right-Side Media Containment:**
    - Superseded destructive full-container `object-fit: cover` on non-family heroes (`/products`, `/about-us`, `/contact-us`, `/request-a-quote`).
    - Unified `.internal-hero-image` with `.internal-hero-picture` family treatment: bounded right-side media region (`width: 65%; max-width: 1100px; height: 100%; right: 0; left: auto; object-fit: contain; object-position: right center;` at >= 64rem; `width: 75%;` at tablet).
    - Verified zero vertical cropping: 100% of 8:3 source compositions (2048×768 non-family, 1536×576 family) are visible without stretching or distortion.
    - Standardized gradient overlay (`linear-gradient(to right, var(--color-brand-navy) 0%, var(--color-brand-navy) 35%, rgba(13, 27, 46, 0.85) 55%, rgba(13, 27, 46, 0.40) 80%, transparent 100%)`) providing solid navy protection for left copy and smooth fade into right-side imagery.
  * **Content Resilience & Zoom Testing:**
    - Tested representative heroes under 100%, 125%, and 200% zoom.
    - Confirmed all titles, leads, and action buttons remain visible and fully interactive with zero text clipping or container overflow.
  * **Responsive & Mobile Verification:**
    - Tested across 9 viewports: `1440×900`, `1920×1080`, `1366×768`, `2560×1440`, `1024×768`, `768×1024`, `390×844`, `375×667`, `375×500`.
    - Confirmed zero horizontal overflow (`scrollWidth <= innerWidth`) on every route and viewport.
    - On mobile (< 48rem), heroes retain fluid height with atmospheric background media under 90% navy overlay (`rgba(13, 27, 46, 0.90)`).
  * **Header Architecture Precision & Static Boundaries:**
    - Verified header remains sticky at `top: 0` during normal operation, and viewport-pinned with `position: fixed` while mobile navigation scroll lock is active.
    - Confirmed zero changes to homepage hero/video, header sizing, logo assets, product-family source assets, catalog data/models, technical documents, or backend logic.
  * **Build, Formatting, & Route Verification:**
    - `git diff --check` (exit code 0).
    - `dotnet restore USAP.Web.sln` (exit code 0).
    - `dotnet build USAP.Web.sln --configuration Release --no-restore` (exit code 0, 0 warnings, 0 errors).
    - `dotnet format whitespace USAP.Web.sln --verify-no-changes --no-restore` (exit code 0).
    - `dotnet format USAP.Web.sln --verify-no-changes --no-restore` (reported known pre-existing baseline IDE0011 brace warnings in `Program.cs` lines 16–35, zero in C1R9 changes).
* **Review Package:**
  * `USAP-CATALOG-002-C1R9-shared-internal-hero-reconciliation-review-package.zip`.

---

## QA-022 — Checkpoint USAP-CATALOG-002-C1R10 Verification: Final Internal-Hero Height and Edge-Fade Reconciliation

* **Date:** 2026-09-23
* **Milestone / Task:** USAP-CATALOG-002-C1R10 (Final Internal-Hero Height and Edge-Fade Reconciliation)
* **Scope & Implementation Summary:**
  * **About Us Height Normalization:**
    - Normalized `.internal-hero--about` to `padding-block: 1.75rem;` (28px top/bottom) under `@media (min-width: 48rem)`.
    - Confirmed `/about-us` resolves to exactly 388.59px (~388.6px) at 1440×900, 1366×768, 1920×1080, and 2560×1440, matching the shared `--internal-hero-min-height` baseline.
    - Verified content breathing room: 28.75px top space above eyebrow and 29.75px bottom space below CTA button.
    - Verified zero modifications to About text block width (704px), lead max-width (608px), typography, line heights, CTA dimensions, or copy.
  * **Top-Biased Edge Fade (Selected Candidate D):**
    - Implemented multi-background gradient on `.internal-hero-overlay, .internal-hero-overlay--family` at desktop (`>= 64rem`) and tablet (`48rem - 63.999rem`).
    - Top dissolve (5% navy band at 75%, clear by 18%) eliminates hard sky edge while preserving natural upper sky and antenna structures; bottom dissolve (88% to 96% light fade, navy at 100%) preserves terrain and hardware bases.
    - Central 70% of image height (18% to 88%) remains completely unshaded and crisp.
    - Horizontal text-protection gradient remains 100% effective for left copy contrast.
  * **Cross-Page Desktop QA (10 Routes @ 1440×900):**
    - Tested all 10 image-backed routes: `/products`, `/about-us`, `/contact-us`, `/request-a-quote`, and all 6 family routes (`/products/log-periodic-antennas`, `/products/portable-transportable-antennas`, `/products/aperiodic-loop-antennas`, `/products/nvis-antennas`, `/products/antenna-rotator-control-systems`, `/products/tower-systems-accessories`).
    - All 10 routes resolved to exact baseline: 388.59px height, zero horizontal overflow (`scrollWidth <= innerWidth`).
  * **Tablet & Responsive QA:**
    - Verified 1024×768: All routes resolve to 388.59px. About content height is 321.28px with 33.16px top / 34.16px bottom space.
    - Verified 768×1024: All routes resolve to 388.59px. About content height is 255.86px with 65.86px top / 66.88px bottom space.
    - Zero horizontal overflow across all viewports.
  * **Mobile Regression QA (< 48rem):**
    - Verified 390×844 and 375×667 across representative routes (`/products`, `/about-us`, `/contact-us`, `/request-a-quote`, `/products/log-periodic-antennas`).
    - Verified `object-fit: cover` and atmospheric overlay preserved exactly as established in C1R9 with zero regression and zero horizontal overflow.
  * **Browser Zoom Verification:**
    - Tested `/about-us` under 100%, 125%, 150%, and 200% zoom.
    - 100% & 125%: Resolves to 388.59px without clipping.
    - 150% & 200%: Hero container scales and expands naturally without text clipping, hidden CTA, or forced fixed-height overflow.
  * **Build, Formatting, & Static Validation:**
    - `git diff --check` (exit code 0).
    - `dotnet restore USAP.Web.sln` (exit code 0).
    - `dotnet build USAP.Web.sln --configuration Release --no-restore` (exit code 0, 0 warnings, 0 errors).
    - `dotnet format whitespace USAP.Web.sln --verify-no-changes --no-restore` (exit code 0).
    - Static boundaries verified: zero changes to homepage hero/video, header behavior/logo, catalog data, product facts, or backend logic.
* **Review Package:**
  * `USAP-CATALOG-002-C1R10-final-internal-hero-reconciliation-review-package.zip`.

---

## QA-023 — Checkpoint USAP-CATALOG-002-C1R11 Verification: Final Right-Anchored Ultra-Wide Hero Composition Reconciliation

* **Date:** 2026-09-23
* **Milestone / Task:** USAP-CATALOG-002-C1R11 (Right-Anchored Ultra-Wide Hero Composition Reconciliation)
* **Scope & Implementation Summary:**
  * **Ultra-Wide Breakpoint Architecture (`@media (min-width: 100rem)`):**
    - Established 100rem (~1600px) as the controlling ultra-wide breakpoint, derived from the mathematical crossover threshold where contained 8:3 source imagery reaches maximum width in a 65vw track ($1033.58 / 0.65 \approx 1590.13\text{px}$).
    - Below 100rem (verified at 1440×900 and 1366×768), all hero metrics, padding, media dimensions, and visual presentations remain 100% byte-for-byte or pixel-equivalent to the accepted C1R10-R2 baseline.
  * **Right-Pinned Media with Moderate Responsive Growth (Candidate B):**
    - Replaced the initially prototyped centered canvas and right navy gutter with Candidate B:
      - `.internal-hero`: `min-height: clamp(24.2875rem, 22vw, 28rem);`.
      - `.internal-hero-picture, .internal-hero-image`: `right: 0; width: clamp(65rem, 59vw, 75rem); max-width: none;`.
      - `.internal-hero-picture .internal-hero-img, .internal-hero-image`: `object-fit: contain; object-position: right center;`.
    - Contained imagery terminates cleanly at the physical browser right edge (`visibleImageRight == viewportWidth`) with zero right-side navy gutter.
    - Responsive height growth allows the full 8:3 image to scale up and reach significantly farther left into the viewport (~796px at 1920px, ~1368px at 2560px; +90.1px and +158.4px farther left than Candidate A), eliminating the empty center chasm.
  * **Relaxed Ultra-Wide Horizontal Copy-Protection Overlay (C1R11 / C1R12 Refinement):**
    - Replaced the right-edge gutter dissolve with the canvas-anchored relaxed horizontal gradient:
      `linear-gradient(to right, var(--color-brand-navy) 0, var(--color-brand-navy) calc(max(0px, (100vw - 100rem) / 2) + 30rem), rgba(13, 27, 46, 0.88) calc(max(0px, (100vw - 100rem) / 2) + 40rem), rgba(13, 27, 46, 0.58) calc(max(0px, (100vw - 100rem) / 2) + 50rem), rgba(13, 27, 46, 0.24) calc(max(0px, (100vw - 100rem) / 2) + 62rem), transparent calc(max(0px, (100vw - 100rem) / 2) + 72rem))`
    - Under C1R12, finalized horizontal transition to Candidate H1: extends solid navy to `gutter + 30rem` (covering copy start to 640px at 1920px), retains 88% opacity at `+40rem` (where image enters at x=796px at 1920px) to completely eliminate the visible tonal seam beside and beneath headings, gently dissolves through `+50rem` (58%) and `+62rem` (24%), and reaches full transparency by `+72rem` (1312px) before main antenna hardware subjects.
    - Layered under the accepted Candidate D vertical top/bottom fade (`linear-gradient(to bottom, navy 0%, 75% navy 5%, transparent 18%, transparent 88%, 45% navy 96%, navy 100%)`).
    - Guarantees 100% solid navy contrast behind typography while allowing graphic details to emerge smoothly without dark bands or tonal seams. Zero right-to-left gutter gradient.
  * **Wide-Screen Desktop QA Across Viewports:**
    - 1600×900: Hero height 388.6px, media width 1040.0px, visible image 566.4px–1600.0px, right gutter 0.0px (`right: 0`).
    - 1920×1080: Hero height 422.4px, media width 1132.8px, media left/right 787.2px / 1920.0px, visible image 796.3px–1920.0px, right gutter 0.0px (`right: 0`).
    - 2560×1440: Hero height 448.0px, media width 1200.0px, media left/right 1360.0px / 2560.0px, visible image 1368.0px–2560.0px, right gutter 0.0px (`right: 0`).
    - Zero horizontal overflow (`scrollWidth <= innerWidth`) on all viewports.
  * **All-Ten-Route Ultra-Wide QA (1920×1080 & 2560×1440):**
    - Tested all 10 image-backed routes: `/products`, `/about-us`, `/contact-us`, `/request-a-quote`, and all 6 product-family routes.
    - Confirmed zero horizontal overflow, zero right navy gutter, full 8:3 source image containment (no cropping, no stretching), zero text/image collision, and identical shared behavior across all 10 routes.
  * **Real Local-Browser Serving Verification:**
    - Verified that the running ASP.NET Core application on `localhost` directly serves the updated production CSS from `site.css` with `min-height: clamp(...)` and `right: 0;` without any runtime injection required.
  * **Build, Formatting, & Static Validation:**
    - `git diff --check` (exit code 0).
    - `dotnet restore USAP.Web.sln` (exit code 0).
    - `dotnet build USAP.Web.sln --configuration Release --no-restore` (exit code 0, 0 warnings, 0 errors).
    - `dotnet format whitespace USAP.Web.sln --verify-no-changes --no-restore` (exit code 0).
* **Review Package:**
  * `USAP-CATALOG-002-C1R11-R2-final-right-anchored-ultrawide-review-package.zip`.

---

## 2026-09-24 — USAP-CATALOG-002-C1R13-R1: Rotator & Control Systems Route-Specific Mask Implementation QA

* **Scope & Implementation Summary:**
  * **Approved Candidate R2 Mask Integration:**
    - Implemented route-specific CSS image mask on `.internal-hero--family-antenna-rotator-control-systems .internal-hero-picture` under `@media (min-width: 100rem)` in `site.css`:
      `-webkit-mask-image` / `mask-image`: `linear-gradient(to right, transparent 0, rgba(0, 0, 0, 0.55) 4rem, rgba(0, 0, 0, 0.85) 8rem, #000 12rem)`.
    - Eliminates the perceptible vertical seam caused by the hard rectangular left boundary of the rotator technical schematic asset without affecting hardware clarity.
  * **Preservation of Shared Architecture & Breakpoints:**
    - Preserved Candidate D top/bottom fade, Candidate H1 horizontal copy-protection blend, right-anchored media track, and zero right gutter.
    - Zero style leakage to other nine hero routes; standard desktop (1366, 1440) and tablet/mobile viewports remain 100% unaffected.
  * **Target Framework Verification:**
    - Confirmed `src/USAP.Web/USAP.Web.csproj` specifies `<TargetFramework>net10.0</TargetFramework>`.
    - Validated build outputs target `net10.0`.
  * **Build, Formatting, & Static Validation:**
    - `git diff --check` (exit code 0).
    - `dotnet restore USAP.Web.sln` (exit code 0).
    - `dotnet build USAP.Web.sln --configuration Release --no-restore` (exit code 0, 0 warnings, 0 errors).
    - `dotnet format whitespace USAP.Web.sln --verify-no-changes --no-restore` (exit code 0).
* **Review Package:**
  * `USAP-CATALOG-002-C1R13-R1-rotator-mask-production-review-package.zip`.

---

## 2026-09-24 — USAP-TECHDOC-001: Technical Resources Library and Canonical PDF Migration QA

* **Scope & Implementation Summary:**
  * **Canonical Document Migration:**
    - Exactly 17 first-party canonical PDF binaries copied into `src/USAP.Web/wwwroot/documents/technical/{family-slug}/{filename}`.
    - Verified 17 of 17 PDF binaries against R2 `MANIFEST.txt`: 100% byte count and SHA-256 hash match.
    - Zero superseded or duplicate binaries deployed to `wwwroot`.
  * **Direct Public Access & Technical Resources Route (`/technical-resources`):**
    - Upgraded `/technical-resources` to a fully responsive, semantic public library.
    - Direct public access model implemented: direct view and download actions for all 17 PDFs without a lead capture gate.
    - Keyword search input filters documents across title, model numbers, product family, description, and search tokens.
    - Category filter bar features 6 product family categories plus `All` with accurate live document counts.
    - Accessible no-results empty state with filter reset action.
    - Live region (`aria-live="polite"`) announces visible document counts to assistive technology.
    - 100% progressive enhancement: library is completely functional without JavaScript (via server GET handling), and enhanced with instant client-side updates when JavaScript is enabled.
  * **Product Catalog Document Reconciliation:**
    - Replaced all 6 interim WordPress-hosted links across the catalog with local canonical PDF paths.
    - Reconciled all 16 product groups to their respective canonical documents.
    - Supported multi-document groups (`v-4213` associates both `DOC-V4213-OVERVIEW` and `DOC-V4213-CONFIG`). All other 15 groups associate their singular canonical document.
    - Total catalog document links: exactly 17 links across all 6 family pages.
    - Zero external WordPress links remain anywhere in application markup.
  * **Legacy Document Redirects (HTTP 301):**
    - Implemented `LegacyDocumentRedirectMiddleware` in ASP.NET Core pipeline before `UseStaticFiles`.
    - Tested all 34 known legacy `/wp-content/uploads/...pdf` URLs from `USAP-CATALOG-002-R2-LEGACY-DOCUMENT-REDIRECT-MATRIX.csv`.
    - 34 of 34 URLs verified returning HTTP 301 Moved Permanently with Location header matching the expected canonical destination.
    - Query string preservation verified (e.g. `?src=test&ref=manual` passes through intact).
  * **Dual Static File Serving:**
    - Configured `PhysicalFileProvider` for `/technical-resources/documents` alongside default `/documents/technical`.
    - All 17 canonical PDFs verified returning HTTP 200 OK and `Content-Type: application/pdf` under `/technical-resources/documents/...`.
    - All 17 canonical PDFs verified returning HTTP 200 OK and `Content-Type: application/pdf` under `/documents/technical/...`.
  * **Responsive & Accessibility Verification:**
    - Tested desktop (1440×900, 1280×800), tablet (768×1024), and mobile (390×844) viewports.
    - Zero horizontal overflow (`scrollWidth <= innerWidth`) on all viewports.
    - Fully keyboard-operable search input, clear button, and category pills with ArrowLeft / ArrowRight navigation.
    - Visible focus rings (`--color-focus`, 2px/3px outline) verified on all interactive elements.
    - Reduced motion preferences honored with transitions and transforms suppressed.
  * **Build, Formatting, & Static Validation:**
    - `git diff --check`: 0 issues (exit code 0).
    - `dotnet restore USAP.Web.sln`: Success (exit code 0).
    - `dotnet build USAP.Web.sln --configuration Release --no-restore`: 0 warnings, 0 errors (exit code 0).
    - `dotnet format whitespace USAP.Web.sln --verify-no-changes --no-restore`: Clean (exit code 0).

---

## 2026-09-25 — USAP-TECHDOC-001-R1: Technical Resources Presentation, Document Detail Experience, SEO Foundation, Homepage Resource Routing, Evidence Repair, and Runtime Hygiene QA

* **Scope & Implementation Summary:**
  * **Runtime Target & Preflight Reconciliation:**
    - Reconciled runtime baseline: .NET SDK `10.0.400`, `net10.0` target framework in `USAP.Web.csproj` and `global.json`.
    - Confirmed all build outputs and verification logs cleanly show `net10.0`.
  * **Development-Server Ownership & Process Hygiene:**
    - Inspected existing runtime state: user-owned dev server processes identified (PIDs 24992, 33552, 34168). All user-owned processes preserved; zero termination or replacement.
    - Automated verification executed on isolated Agent-owned test server on dedicated port `5396` (PIDs 24692, 21840).
    - Confirmed complete cleanup: all Agent-owned processes terminated and port 5396 closed (`Agent-owned development server cleanup: PASS`).
  * **Search Toolbar Refinement:**
    - Refined desktop and wide-desktop search form to a contained, left-aligned container (`max-width: 52rem; width: 100%`) rather than stretching across the entire 72rem content grid.
    - Integrated a prominent "Search" submit button with a clear visual gap (`--space-2`), maintaining full keyboard and no-JS form submission support.
    - Mobile and tablet layouts retain fluid full-width behavior with zero horizontal overflow.
  * **Homepage Deep-Link Reconciliation & Active Preset Filter Banner:**
    - Reconciled the 4 homepage Technical Resources preview cards to structured, query-driven entry points:
      - Product Data Sheets: `/technical-resources?type=data-sheet` (derives count dynamically from `DocumentType == "Data Sheet"`).
      - Antenna Systems: `/technical-resources?view=antenna-systems` (covers all 4 antenna families: Log Periodic, Portable/Transportable, Aperiodic Loop, NVIS).
      - Rotators & Controls: `/technical-resources?category=antenna-rotator-control-systems`.
      - Tower Systems & Accessories: `/technical-resources?category=tower-systems-accessories`.
      - Browse Technical Resources CTA: `/technical-resources` (unfiltered, all 17 documents).
    - Added an active filter notification banner (`.resources-active-filter-banner`) displaying the active preset (e.g., "Filtered by: Product Data Sheets") with an accessible "Clear filter" reset link.
  * **Technical Resources Hero Artwork Integration:**
    - Integrated project asset `usap-technical-resources-hero-requirements-document-candidate-b-v1.png` onto `/technical-resources` using the established `.internal-hero` architecture and Candidate D gradient overlay system.
    - Verified desktop, ultra-wide (1600px+ / 1920px), tablet, and mobile viewports. Text contrast and responsive media containment verified.
  * **Closing CTA Integration:**
    - Added standard site closing CTA (`.closing-cta.closing-cta--default`) immediately preceding the footer on `/technical-resources` and all `/technical-resources/document/{slug}` detail pages.
    - Links to Contact Us (`/contact-us`) and Request a Quote (`/request-a-quote`) verified.
  * **HTML Technical Document Detail Experience (`/technical-resources/document/{slug}`):**
    - Built a reusable, data-driven Razor Page resolving all 17 canonical documents by URL slug.
    - Features hierarchical breadcrumbs, metadata badges (type, file size, pages), covered models/configurations, safe public description, native PDF `<object>`/`<iframe>` viewer with download fallback, related product family card, and closing CTA.
    - Fully server-rendered and SEO-indexable (unique `<title>`, meta description, canonical link, H1).
    - Card primary actions on `/technical-resources` updated to "View Document" navigating to the detail page, while retaining a direct "Download PDF" action.
    - Product group disclosure resource links updated to detail pages with direct download secondary links.
  * **Canonical Raw-PDF URL Reconciliation & Redirects:**
    - Preferred public raw-PDF route confirmed: `/technical-resources/documents/{family-slug}/{filename}`.
    - Configured `LegacyDocumentRedirectMiddleware` to issue HTTP 301 permanent redirects for `/documents/technical/...` -> `/technical-resources/documents/...`.
    - Tested all 17 canonical raw PDFs: 17/17 preferred paths return HTTP 200 `application/pdf`; 17/17 alternate paths return HTTP 301.
    - Tested all 34 legacy WordPress PDF URLs: 34/34 return HTTP 301 to canonical destination.
  * **PDF Metadata & Source Fidelity Policy:**
    - Zero modification of PDF binaries. All 17 canonical files remain byte-for-byte identical to R2 source package (17/17 SHA-256 matches).
  * **Review-Package Evidence Repair:**
    - Completely regenerated `CANONICAL_PDF_MIGRATION_REGISTER.md`, `LEGACY_REDIRECT_QA_MATRIX.md`, `VALIDATION_OUTPUTS.md`, and `CHANGED_FILE_REGISTER.md` with real, verified data—eliminating all malformed `$()`, PowerShell object dumps, and unresolved variables.
  * **Build, Formatting, & Static Validation:**
    - `git diff --check`: 0 issues (exit code 0).
    - `dotnet restore USAP.Web.sln`: Success (exit code 0).
    - `dotnet build USAP.Web.sln --configuration Release --no-restore`: 0 warnings, 0 errors (exit code 0, targeting `net10.0`).
    - `dotnet format whitespace USAP.Web.sln --verify-no-changes --no-restore`: Clean (exit code 0).

---

## 2026-09-25 — USAP-TECHDOC-001-R2: Technical Resources UI Cleanup and Baseline Stabilization QA

* **Scope & Implementation Summary:**
  * **Desktop Search Toolbar Right Alignment:**
    - Refined `.resources-search-form` on desktop to align to the right edge (`margin-left: auto; margin-right: 0; max-width: 32rem; width: 100%`), balancing against the left-aligned family filter pills.
    - Verified responsive behavior on tablet (768px) and mobile (390px) where full width is naturally preserved.
  * **Card Footer & Atomic Page Count Restoration:**
    - Restored restrained document SVG icon and atomic nowrap page count indicator (`1 page`, `2 pages`, etc.) in `.resource-card__meta`.
    - Enforced `white-space: nowrap` across page text and meta container to prevent numbers and words from splitting across multiple lines.
    - Compacted button padding and typography so footers fit on a single row in 3-column desktop layout.
  * **Link State Protection & Visited Styling:**
    - Explicitly styled `:link`, `:visited`, `:hover`, and `:focus-visible` on `.resource-card__title-link`, `.breadcrumbs__link`, `.doc-property__link`, `.doc-viewer-card__note a`, and `.doc-related-card__link`.
    - Confirmed zero browser-default purple link colors leak into the card grid or detail experience.
  * **Document-Detail Breadcrumbs & Header Cleanup:**
    - Relocated breadcrumbs on `/technical-resources/document/{slug}` to appear directly below the hero header.
    - Reused shared `.breadcrumbs` architecture from Product Family pages, eliminating browser-default numbered list styling.
    - Removed redundant low-contrast `Back to Resource Library` button from detail hero.
    - Streamlined viewer card header to remove adjacent duplicate new-tab button.
  * **Closing CTA Copy Alignment:**
    - Replaced guidance copy with the approved USAP Closing Inquiry CTA across `/technical-resources` and all detail routes:
      - Heading: `Ready to Discuss Your Antenna Requirements?`
      - Copy: `Share your application requirements, product questions, or system needs. USAP can help you evaluate available antenna and related equipment options.`
      - Actions: `Request a Quote` and `Request Information`.
  * **Preset Filter Simplification:**
    - Suppressed redundant `Reset filters` link when active preset banner is displayed, ensuring a single clean status indicator and reset action.
  * **Source Review Patch Generation:**
    - Created clean `SOURCE_DIFF.patch` capturing all textual modifications (excluding binary PDFs) for Architect inspection.
  * **Build, Formatting, & Static Validation:**
    - `git diff --check`: 0 issues (exit code 0).
    - `dotnet restore USAP.Web.sln`: Success (exit code 0).
    - `dotnet build USAP.Web.sln --configuration Release --no-restore`: 0 warnings, 0 errors (exit code 0, targeting `net10.0`).
    - `dotnet format whitespace USAP.Web.sln --verify-no-changes --no-restore`: Clean (exit code 0).

---

## Deferred Enhancement — Thank-You Page Visual/Content Refinement

**Classification:** DEFERRED / NON-BLOCKING / ENHANCEMENT PHASE
**Date Logged:** 2026-09-30 (USAP-FORMS-004-R1)
**Target Routes:** `/contact-us/thank-you` and `/request-a-quote/thank-you`
**Status:** Documented for future refinement pass; zero markup or CSS modified during FORMS-004.

### Logged Items:
A. **Supporting Copy:**
   - Current success-page supporting text feels too long and dense.
   - Text measure/width is too constrained on wide viewports.
   - Later enhancement will simplify the copy and improve comfortable, readable line length.

B. **Typography:**
   - Perform a dedicated typography and hierarchy pass across both Thank-You pages.
   - Reassess visual weight, sizing, and spacing of eyebrow, H1, lead/supporting copy, section labels, reference card, and overall vertical rhythm.

C. **Duplicate Success Messaging:**
   - Current Contact success state contains redundant success language (e.g. eyebrow `Inquiry Received` positioned adjacent to body copy conveying the same receipt confirmation).
   - Later enhancement will eliminate the redundant layer and establish a single clear success communication hierarchy.
   - Apply equivalent review to the Quote confirmation page (`/request-a-quote/thank-you`).

D. **Reference-Number Component Corner Radii:**
   - The left colored accent border on the reference-number component currently terminates into sharp points at the top-left and bottom-left edges while outer card corners are rounded.
   - Reconcile left corner radii so the component displays consistent rounded corners around the entire card surface.
   - Align styling with the accepted email reference-number component.

E. **Reference-Number Branding:**
   - Reassess whether the current green success accent should remain as an intentional status color or be reconciled more closely with the authoritative USAP navy (`#0d1b2e`) and red (`#c8102e`) design system tokens.
   - Decision deferred to the later visual enhancement milestone.

F. **Scope Boundary:**
   - Refinements will apply consistently across `/contact-us/thank-you` and `/request-a-quote/thank-you`.
   - All Thank-You templates (`ContactUs/ThankYou.cshtml`, `RequestAQuote/ThankYou.cshtml`, `confirmation.css`, and related PageModels) remained strictly untouched during FORMS-004.
