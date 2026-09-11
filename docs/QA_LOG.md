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
