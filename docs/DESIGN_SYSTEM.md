# Design System

Provisional design system for the USAP website rebuild.
All values and visual direction documented here are provisional and replaceable without
requiring structural changes to the HTML or CSS architecture.

**Status:** Approved for development use. Official brand assets, typography, color system,
and logo not yet delivered by client. See `docs/CONTENT_AND_ASSETS.md`.

---

## Provisional visual direction

The design concept reference (internal use only — `docs/reference-materials/`) suggests:

| Element | Provisional direction |
|---|---|
| Primary dark color | Deep navy |
| Accent / action color | Strong red |
| Body surfaces | White / very light neutral |
| Body text | Dark, high-contrast neutral |
| Typography | System font stack — no external font requests |

All values are centralized in the `:root` block at the top of `site.css` and can be changed in one place when official brand assets are supplied.

---

## Token groups

All tokens are CSS custom properties defined in `src/USAP.Web/wwwroot/css/site.css`.

### Brand and semantic colors

| Token | Provisional value | Purpose |
|---|---|---|
| `--color-brand-navy` | `#0d1b2e` | Primary dark brand color — headings, header, footer |
| `--color-brand-navy-mid` | `#16293f` | Mobile nav panel background, slightly lighter navy |
| `--color-brand-navy-border` | `#1e3450` | Nav and footer separator lines |
| `--color-brand-red` | `#c8102e` | Accent — CTAs, active-route indicator, eyebrow text |
| `--color-brand-red-hover` | `#a80d26` | Red hover state |
| `--color-brand-red-active` | `#8c0b1f` | Red pressed state |
| `--color-text-default` | `#141a21` (neutral-10) | Primary body text |
| `--color-text-muted` | `#525a65` (neutral-40) | Secondary / supporting text |
| `--color-text-inverse` | `#ffffff` | Text on dark backgrounds |
| `--color-text-link` | `#004a99` | Body link color |
| `--color-focus` | `#0057b8` | Focus ring color |
| `--color-error` | `#b91c1c` | Error state |
| `--color-success` | `#147a3b` | Success state |
| `--color-warning` | `#b45309` | Warning state |

### Neutral scale

Eight neutrals from `--color-neutral-100` (white) to `--color-neutral-10` (near-black).
One dark-surface accent: `--color-accent-dark-surface: #ff4d5f` (approximately 5.34:1 on `#0d1b2e`).

### Typography

| Token | Value |
|---|---|
| `--font-family-base` | `system-ui, -apple-system, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif` |
| `--font-family-mono` | `ui-monospace, "Cascadia Code", "Fira Code", "Courier New", Courier, monospace` |

No external font requests. No Google Fonts, Adobe Fonts, or CDN font loading.

### Fluid type scale

`clamp(min, preferred vw, max)` — scales smoothly between viewport sizes.

| Token | Range |
|---|---|
| `--text-xs` | 0.75–0.875rem |
| `--text-sm` | 0.833–0.9375rem |
| `--text-base` | 1–1.125rem |
| `--text-md` | 1.2–1.375rem |
| `--text-lg` | 1.44–1.75rem |
| `--text-xl` | 1.728–2.25rem |
| `--text-2xl` | 2.074–2.875rem |
| `--text-3xl` | 2.488–3.625rem |

### Spacing scale (4px base)

`--space-1` (4px) through `--space-24` (96px).

### Container widths

| Token | Value | Use |
|---|---|---|
| `--container-content` | 72rem (1152px) | Standard content container |
| `--container-wide` | 90rem (1440px) | Wide / full-bleed sections |
| `--container-padding` | 1.5rem → 2.5rem (responsive) | Inline padding on containers |

### Section spacing

| Token | Value |
|---|---|
| `--section-gap-sm` | 3rem |
| `--section-gap` | 4rem |
| `--section-gap-lg` | 6rem |

### Borders and radii

| Token | Value |
|---|---|
| `--border-radius-sm` | 0.25rem |
| `--border-radius` | 0.375rem |
| `--border-radius-md` | 0.5rem |
| `--border-radius-lg` | 0.75rem |

### Shadows

`--shadow-sm`, `--shadow`, `--shadow-md`, `--shadow-lg` — light rgba shadows for card and elevation.

### Focus ring

`--focus-ring: 3px solid #0057b8` (blue, approximately 6.87:1 on white) — light/white surfaces.
`--focus-ring-dark: 3px solid #ffffff` (white, approximately 17.31:1 on navy) — dark/navy surfaces.
`--focus-ring-offset: 2px` / `--focus-ring-offset-dark: 3px` (for dark backgrounds)

Light-surface ring: used on page body, desktop header, desktop nav links.
Dark-surface ring: used on mobile/navy header (brand, toggle, nav links, CTA) and footer.
Visible on all interactive elements via `:focus-visible`. Applied globally and per-component.

### Motion

`--duration-fast` (120ms), `--duration-normal` (220ms), `--duration-slow` (400ms).
Easing: `--ease-out` and `--ease-in-out`.
All transitions suppressed under `prefers-reduced-motion: reduce`.

### Z-index layers

`--z-below (-1)` → `--z-raised (10)` → `--z-dropdown (100)` → `--z-sticky (200)` → `--z-modal (400)` → `--z-toast (500)`.

---

## Breakpoints

Breakpoints are explicit numeric values in CSS `@media` queries.
**CSS custom properties cannot be used inside media query conditions.**

| Name | Min-width | Target |
|---|---|---|
| base | 0 | Mobile (≈390px) |
| sm | 30rem (480px) | Larger phones, landscape |
| md | 48rem (768px) | Tablets — mobile disclosure at this size |
| lg | 64rem (1024px) | Small desktops — desktop nav and white header activate here |
| xl | 80rem (1280px) | Standard desktops |
| 2xl | 90rem (1440px) | Wide screens |

Mobile navigation collapses below **64rem (1024px)** — `max-width: 63.9375rem`.
Tablet (768px) uses mobile disclosure navigation.

---

## Typography scale in use

```css
h1 { font-size: var(--text-3xl); }  /* 2.488–3.625rem */
h2 { font-size: var(--text-2xl); }
h3 { font-size: var(--text-xl); }
h4 { font-size: var(--text-lg); }
h5 { font-size: var(--text-md); }
h6 { font-size: var(--text-base); }
```

`.eyebrow` — all-caps, `--text-xs`, `letter-spacing: 0.1em`, red color.
`.section-heading` — `--text-2xl`, bold, navy.

---

## Layout primitives

| Class | Purpose |
|---|---|
| `.container` | Standard content container (72rem max) |
| `.container-wide` | Wide container (90rem max) |
| `.section` | `padding-block: 4rem` |
| `.section-sm` | `padding-block: 3rem` |
| `.section-lg` | `padding-block: 6rem` |
| `.grid` | CSS grid with `--grid-gap` |
| `.grid-2 / .grid-3 / .grid-4` | Fixed-column grids |
| `.grid-auto-fit / -sm / -lg` | Responsive auto-fit grids |
| `.stack / .stack-sm / .stack-lg` | Vertical rhythm via `> * + *` margin |
| `.cluster` | Horizontal flex with gap and wrap |
| `.sr-only` | Accessible visually hidden text |
| `.skip-link` | Skip navigation link (visible on focus) |

---

## Buttons

| Class | Use |
|---|---|
| `.btn.btn-primary` | Primary CTA — red fill |
| `.btn.btn-secondary` | Secondary — navy fill |
| `.btn.btn-outline` | Outline — navy border |
| `.btn.btn-inverse` | On dark backgrounds — white border/text |
| `.btn-sm` | Small modifier |
| `.btn-lg` | Large modifier |

All buttons: `:hover`, `:active`, `:focus-visible`, `:disabled` states defined.

---

## Links

`.link-emphasis` — bold, no underline, appends → arrow, underline on hover.
Standard `<a>`: link blue, visited purple, navy on hover.

---

## Cards and content surfaces

| Class | Purpose |
|---|---|
| `.card` | Base card shell with border, shadow, hover lift |
| `.card-body` | Card content padding |
| `.card-img` | 16/9 image within a card |
| `.card-img-placeholder` | Placeholder when image unavailable |
| `.card-product` | Product card hook |
| `.card-resource` | Resource card with icon column layout |
| `.cta-band` | Dark-navy full-width CTA section |
| `.notice` | Status/info notice with left border |

---

## Form control foundations

| Class | Purpose |
|---|---|
| `.form-group` | Label + control + help text column |
| `.form-label` | Visible label |
| `.form-required` | Red asterisk for required fields |
| `.form-control` | Input, select, textarea base |
| `.form-help` | Help text below control |
| `.form-error` | Validation error message |

Error state: `aria-invalid="true"` on `.form-control` applies red border + shadow.

---

## Header and navigation

### Structure

`_Header.cshtml` partial — included by `_Layout.cshtml`.

```
<header class="site-header">
  <div class="container">
    <div class="site-header-inner">
      <a class="site-brand">         ← logo region (text-only placeholder)
      <button class="nav-toggle">   ← mobile toggle (JS-revealed)
      <nav class="primary-nav">
        <ul id="primary-nav-list">  ← nav items + Request a Quote CTA
```

### Active route indication

- Current page: `.is-active` class + `aria-current="page"` attribute.
- Products section is active for both `/products` and `/products/{familySlug}`.
- Active indicator: bold weight + navy text + red bottom border (desktop) / `--color-accent-dark-surface` (#ff4d5f, approximately 5.34:1 on navy) left border (mobile).
- **Not color-only:** underline/border provides a non-color visual distinction.

### Header surface

| Breakpoint | Background | Text | Focus ring |
|---|---|---|---|
| Mobile/tablet (<1024px) | `--color-brand-navy` | white / neutral-80 | `--focus-ring-dark` (white) |
| Desktop (≥1024px) | `--color-neutral-100` (white) | navy / neutral-40 | `--focus-ring` (blue) |
### Logo region

- Text-only provisional treatment while production logo is pending.
- Structure accepts a `<img>` replacement without requiring header reconstruction.
- Production logo must be an SVG or high-resolution PNG supplied by client.
- Logo screenshot from reference materials must not appear in the public shell.

---

## Mobile navigation interaction contract

**Pattern:** Accessible disclosure — not a modal dialog.

| Behavior | Implementation |
|---|---|
| No-JS fallback | Nav list visible by default; toggle hidden until `js-nav-ready` class added to `<html>` |
| JS-active | Toggle revealed by `.js-nav-ready .nav-toggle { display: flex }` CSS rule |
| Open/close | Toggle `aria-expanded` + `hidden` attribute on `#primary-nav-list` |
| Escape | Closes menu and returns focus to toggle |
| Click outside | Closes menu (document click listener) |
| Nav link click | Closes menu |
| Resize to desktop | Removes `hidden`, resets `aria-expanded` to false |
| No focus trap | Disclosure — Escape and natural tab order serve navigation |
| No body-scroll lock | None |
| Touch target | Toggle button is 2.75rem × 2.75rem (~44px) |
| Reduced motion | `transition-duration: 0.01ms` via `prefers-reduced-motion` media query |
| Toggle label | `aria-label` updated dynamically: "Open navigation menu" / "Close navigation menu" |

---

## Footer

`_Footer.cshtml` partial — included by `_Layout.cshtml`.

Four-column layout (brand + 3 link groups) at desktop; single column at mobile.
Current-year copyright generated via `@DateTime.Now.Year`.

**Approved content only** — no address, phone, email, social links, Privacy Policy, Terms,
or any destination without a current approved route.

No placeholder tagline. Company name is in the brand column and the sole copyright statement is in the footer-bottom row.

---

## Accessibility expectations

- `:focus-visible` on all interactive elements — keyboard users always see where focus is.
- Skip link: first focusable element, visually hidden until focused, jumps to `#main-content`.
- `aria-current="page"` on the active navigation link.
- Mobile toggle: `aria-expanded` reflects open/closed state; `aria-controls` references nav list.
- Footer `<h2>` column headings are semantic (accessible) — not `<p>` or `<div>`.
- No color-only status indicators.
- All heading levels follow a single logical hierarchy per page.

---

## How future tasks should use this system

1. **Extend tokens in the `:root` block** — do not hard-code hex or rem values in component CSS.
2. **Use existing layout primitives** (`.container`, `.section`, `.grid-*`, `.stack`, `.cluster`) before adding new ones.
3. **Build new components in the appropriate CSS section** — labeled sections prevent drift.
4. **Active-route logic** is centralized in `_Header.cshtml` — do not duplicate it in individual pages.
5. **New navigation destinations** require a new route and content before being added to the footer/nav.
6. **Replace the logo** by updating the `.site-brand-text` content in `_Header.cshtml` with an `<img>` — the surrounding structure is already production-ready.
7. **Breakpoints are in CSS** — do not add media queries using custom properties.
8. **Reduced-motion** is handled globally — individual components do not need to re-implement it.
