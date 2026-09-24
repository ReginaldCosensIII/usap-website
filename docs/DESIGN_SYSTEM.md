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

Primitive radius scale:

| Token | Value |
|---|---|
| `--border-radius-sm` | 0.25rem |
| `--border-radius` | 0.375rem |
| `--border-radius-md` | 0.5rem |
| `--border-radius-lg` | 0.75rem |
| `--border-radius-full` | 9999px |

Semantic radius scale (C2R Amendment 11):

| Semantic Alias | Primitive Reference | Intended Surface Context |
|---|---|---|
| `--radius-control` | `var(--border-radius)` (0.375rem) | Buttons, form inputs, interactive controls |
| `--radius-surface` | `var(--border-radius-lg)` (0.75rem) | Product family cards, FAQ panels, disclosure cards |
| `--radius-media` | `var(--border-radius-md)` (0.5rem) | Card media frames, figures, embedded diagrams |
| `--radius-pill` | `var(--border-radius-full)` (9999px) | Badges, tags, status pills |

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

### Shared semantic typography roles

| Semantic role | Tokens | Description |
|---|---|---|
| Section eyebrow | `--eyebrow-font-family`, `--eyebrow-font-size` (`--text-sm`), `--eyebrow-font-weight` (700), `--eyebrow-letter-spacing` (0.05em), `--eyebrow-line-height` (`--leading-tight`), `--eyebrow-color` (`--color-brand-red`) | All-caps section category label used on Home, About, and internal pages |
| Section heading | `--section-heading-font-size` (`clamp(1.75rem, 3vw, 2.25rem)`), `--section-heading-line-height` (1.15), `--section-heading-font-weight` (700), `--section-heading-color` (`--color-brand-navy`) | Primary section title |
| Section lead | `--section-lead-font-size` (1.125rem), `--section-lead-line-height` (1.55), `--section-lead-color` (`--color-text-muted`) | Intro paragraph following section heading |
| Card heading | `--card-heading-font-size` (`clamp(1.2rem, 1.45vw, 1.35rem)`), `--card-heading-line-height` (1.25), `--card-heading-font-weight` (700), `--card-heading-color` (`--color-brand-navy`) | Title inside cards |
| Card body | `--card-body-font-size` (1rem), `--card-body-line-height` (1.5), `--card-body-color` (`--color-neutral-70`, `#4b5563`) | Body copy inside cards |

`.eyebrow` — all-caps, `--text-sm`, bold (700), `letter-spacing: 0.05em`, compact line height, red color.
`.section-heading` — clamp(1.75rem, 3vw, 2.25rem), bold (700), navy, line-height 1.15.

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

Shared card foundation tokens:
- `--card-surface`: white surface (`--color-surface-page`, `#ffffff`).
- `--card-border`: 1px solid `rgba(15, 32, 60, 0.10)`.
- `--card-radius`: `0.75rem` (`--border-radius-lg`).
- `--card-shadow-rest`: resting elevation (`0 1px 2px rgba(15, 32, 60, 0.04), 0 10px 30px rgba(15, 32, 60, 0.08)`).
- `--card-shadow-hover`: shared elevated shadow on hover (`0 4px 6px rgba(15, 32, 60, 0.05), 0 16px 40px rgba(15, 32, 60, 0.12)`).
- `--card-hover-translate-y`: `-4px` (shared vertical lift magnitude).
- `--card-hover-border-color`: `rgba(15, 32, 60, 0.20)` (shared hover border reinforcement).
- `--card-transition-duration`: `220ms` (`--duration-normal`).
- `--card-transition-ease`: `cubic-bezier(0.25, 0.46, 0.45, 0.94)` (`--ease-out`).

### Static, ambient, and interactive card behavior

- **Static informational cards (`.card`):** Receive shared resting surface, border, radius, and resting shadow. Noninteractive cards never use `cursor: pointer`.
- **Ambient informational cards (`.card.card--ambient`):** Applied to noninteractive informational cards (the 50-year heritage card, Manufacturing & Testing feature cards, and Integrated Support process cards) to provide visual elevation aligned with the homepage design:
  - **Shared elevation magnitude:** On hover (`@media (hover: hover) and (pointer: fine)`), standard ambient cards lift by `transform: translateY(var(--card-hover-translate-y))` (`-4px`), expand shadow to `--card-shadow-hover`, and strengthen borders to `--card-hover-border-color`.
  - **Stationary ambient cards (`.card--ambient-stationary`):** Modifier applied to cards where vertical translation is undesirable (e.g., the 50-year Company Overview heritage card). Suppresses translation on fine-pointer hover (`transform: none`). Confirmed: Stationary ambient cards strictly retain full hover shadow (`--card-shadow-hover`), hover border response (`--card-hover-border-color`), and navy left accent integrity (`border-left-color: var(--color-brand-navy)`).
  - **Default cursor:** Strictly maintains `cursor: default`; never uses `cursor: pointer`.
  - **Noninteractive semantics:** Contains no link wrappers, buttons, `tabindex`, ARIA interactive roles, or click handlers.
  - **No artificial keyboard focus:** Does not implement `:focus-within` or artificial focus outlines since these cards expose no interactive actions.
  - **Surface integrity:** Card surface remains white (`var(--card-surface)`); no background or text color inversion.
  - **Accent responses:** Cards with `.card--accent-top-red` subtly transition their red top border to `--color-brand-red-hover` (`#a80d26`). The heritage card with `.card--accent-left-navy` preserves its navy left border (`--color-brand-navy`, `#0d1b2e`).
  - **Device targeting:** Scoped strictly within `@media (hover: hover) and (pointer: fine)` so touch devices do not execute sticky hover states.
  - **Reduced motion:** Under `@media (prefers-reduced-motion: reduce)`, transitions are set to `none` and `transform: none` is enforced on hover (immediate shadow and border-color change allowed as non-motion feedback).
  - **Semantic distinction:** While ambient and interactive cards share elevation tokens (`--card-hover-translate-y`, `--card-shadow-hover`, `--card-hover-border-color`, duration, easing), they remain strictly separated by semantic intent, cursor styling, and focusability.
- **Interactive cards (`.card--interactive` / `.home-card`):** Applied strictly to cards with actionable interactive targets. Hover and `:focus-within` elevate the card by `-4px` and expand the shadow to `--card-shadow-hover`. Under `prefers-reduced-motion: reduce`, translation is completely suppressed and transitions are set to `none`.

### Card modifier classes

| Class | Purpose |
|---|---|
| `.card` | Base static card shell with border, radius, resting shadow |
| `.card--ambient` | Shared homepage-aligned hover elevation (lift + shadow + border + accent shift) for noninteractive informational cards |
| `.card--ambient-stationary` | Suppresses vertical hover translation (`transform: none`) while retaining hover shadow, border strengthening, and accent color feedback |
| `.card--interactive` | Variant enabling hover/focus lift and elevation for actionable cards |
| `.card--accent-top-red` | 3px solid red top accent border |
| `.card--accent-left-navy` | 4px solid navy left accent border |
| `.card-body` | Card content padding (`--space-6`) |
| `.card-img` | 16/9 image within a card |
| `.card-img-placeholder` | Placeholder when image unavailable |
| `.card-product` | Product card hook |
| `.card-resource` | Resource card with icon column layout |
| `.cta-band` | Dark-navy full-width CTA section |
| `.notice` | Status/info notice with left border |

---

## Shared Closing CTA Component

Promoted in C11 from the approved homepage closing CTA into a sitewide reusable design-system component in `src/USAP.Web/wwwroot/css/site.css`.

### Component Anatomy

| Class | Role | Visual & Structural Properties |
|---|---|---|
| `.closing-cta` | Section container | `position: relative`, `overflow: hidden`, `color: var(--color-neutral-00)`, `text-align: center`, `padding-block: clamp(3rem, 6vw, 4.75rem)`. |
| `.closing-cta--default` | Default visual modifier | Sets dark navy fallback (`--color-brand-navy`, `#0f203c`), background image (`url('../images/homepage/closing-cta/usap-home-closing-cta-signal-background.png')`), centered, cover, no-repeat. |
| `.closing-cta-content` / `.closing-cta__content` | Content wrapper | `position: relative`, `z-index: 3`, `max-width: 50rem`, centered `margin-inline: auto`, column layout with `gap: var(--space-4)`, centered alignment. |
| `.closing-cta-content .eyebrow` | Eyebrow label | Category label in `--color-accent-dark-surface` (`#ff4d5f`) providing strong contrast on dark backgrounds. |
| `.closing-cta-content h2` | Section heading | `clamp(2rem, 4vw, 2.5rem)`, `--leading-tight` (1.2), text-wrap balance, white text. |
| `.closing-cta-content p` | Supporting copy | `1.125rem`, line-height `1.6`, `rgba(255, 255, 255, 0.9)`, `max-width: 40rem`. |
| `.closing-cta-actions` / `.closing-cta__actions` | Button actions cluster | Centered button group with `gap: var(--space-3)`. Stacks vertically at mobile widths; renders horizontally at desktop (`@media (min-width: 48rem)`). |

### Shared vs Page-Specific Responsibilities

- **Shared component responsibilities:** Shell layout, section padding (`clamp(3rem, 6vw, 4.75rem)`), dark navy fallback color, default signal background image, text centering, heading and body type scale, high-contrast text color, and responsive action button wrapping.
- **Page-specific responsibilities:** Contextual eyebrow text (e.g. "Contact Our Team" on About), headline, descriptive supporting copy, button destination URLs, and button labels (e.g. Request a Quote / Contact Us on About; Request a Quote / Request Information on Home).

### Extension for Future Page-Specific Variants

To implement a page-specific background or tint without duplicating CTA CSS:
1. Retain the base `.closing-cta` class and its inner markup structure (`.closing-cta-content`, `.closing-cta-actions`).
2. Replace `.closing-cta--default` with a new modifier (e.g. `.closing-cta--products`, `.closing-cta--support`).
3. Define only the variant-specific `background-image`, `background-color`, or overlay gradient in the modifier rule. All typography, padding, alignment, and responsiveness remain inherited from the shared component.


---

## Shared Internal-Page Hero Foundation

Promoted in C1 from the approved About Us hero treatment into a sitewide reusable design-system component in `src/USAP.Web/wwwroot/css/site.css`.

### Component Anatomy

| Class | Role | Visual & Structural Properties |
|---|---|---|
| `.internal-hero` | Landmark `<section>` shell | `position: relative`, full-width, `min-height: 22rem`, flex center, `background-color: var(--color-brand-navy)`, `color: var(--color-text-inverse)`, `padding-block: var(--space-12)`, `overflow: hidden`, `border-bottom: 1px solid var(--color-brand-navy-border)`. |
| `.internal-hero-bg` | Geometric fallback base | `position: absolute`, `inset: 0`, z-index 1, radial/linear gradients providing consistent brand atmosphere when imagery is loading. |
| `.internal-hero-image` | Decorative background image | `position: absolute`, `inset: 0`, z-index 2, `object-fit: cover`, `object-position: center` (customized per modifier), `pointer-events: none`. Rendered via `<img>` with `alt=""`, `fetchpriority="high"`, intrinsic dimensions `width="2048"` and `height="768"`. |
| `.internal-hero-overlay` | Text contrast protection | `position: absolute`, `inset: 0`, z-index 3, gradient from 95% opacity on left to 65% on right, collapsing to solid 90% opacity below 1024px (`64rem`) to guarantee WCAG AAA/AA text contrast across all viewports. `pointer-events: none`. |
| `.internal-hero-content` | Readable content wrapper | `position: relative`, z-index 4, `max-width: 44rem`. |
| `.internal-hero-eyebrow` | Eyebrow category tag | `color: var(--color-accent-dark-surface)` (`#ff4d5f`, 5.34:1 on navy), `margin-bottom: var(--space-2)`. |
| `.internal-hero-title` | Semantic `<h1>` heading | `clamp(2rem, 3.5vw + 0.5rem, 3rem)`, `line-height: var(--leading-tight)`, text-wrap balance, white. |
| `.internal-hero-lead` | Supporting lead text | `font-size: var(--text-md)`, `line-height: var(--leading-relaxed)`, 92% white, `max-width: 38rem`. |
| `.internal-hero-actions` | Optional button cluster | Flex wrap with `gap: var(--space-3)`. Used on About Us; omitted on form pages where the form directly follows. |

### Page-Specific Modifiers & Focal Positions

| Modifier | Applied Route | Asset Reference | Intended Subject / Focal Alignment |
|---|---|---|---|
| `.internal-hero--about` | `/about-us` | `images/about/usap-about-hero-lp1017-clean-v2.png` | `object-position: 70% center` (highlights LP-1017 log-periodic array on right). |
| `.internal-hero--contact` | `/contact-us` | `images/contact-us/usap-contact-hero-communication-signal-candidate-c-v1.png` | `object-position: 65% center` (highlights communication signal bubble while leaving left space for copy). |
| `.internal-hero--quote` | `/request-a-quote` | `images/request-a-quote/usap-quote-hero-technical-planning-candidate-a-v1.png` | `object-position: 60% center` (highlights technical planning documents and calipers). |

### Accessibility Rules for Hero Imagery

- Hero images are strictly decorative backgrounds. They must render with empty `alt=""` and `aria-hidden` attributes or container concealment so screen readers do not announce decorative elements.
- Meaningful content resides exclusively in semantic HTML headings, paragraphs, and links.
- High-contrast CSS gradient overlays are mandatory to ensure text contrast passes WCAG AA/AAA standards across all responsive viewports.

---

## Shared Form-Page Layout & Contact Sidebar

A standardized two-column layout applied to `/contact-us` and `/request-a-quote`.

### Layout Anatomy

- `.form-page-layout`: Flex column on mobile; CSS Grid (`grid-template-columns: minmax(0, 1.7fr) minmax(19rem, 23rem)`) at desktop (`min-width: 64rem`).
- `.form-page-main`: Primary content column hosting the interactive inquiry form (`_InquiryForm.cshtml`). Remains first in DOM and mobile reading order.
- `.contact-sidebar`: Right-hand complementary column (`<aside>`) containing the verified contact-information card and Google Maps embed card. Stacks cleanly below the form on viewports below 1024px.
- Both sidebar cards use the stationary informational card pattern (`.card.card--ambient.card--ambient-stationary.card--accent-left-navy`), avoiding unwanted translation or elevation on hover.

### Verified Contact Information Card

- Semantic `<address>` element formatted with `font-style: normal`.
- Displays official company address: `5263 Agro Drive, Frederick, MD 21703`.
- Phone link formatted with `tel:+12403417120` displaying `240-341-7120`.
- Clearly labeled fax: `240-371-4980`.
- Direct Google Maps directions link with `target="_blank"` and `rel="noopener noreferrer"`.
- No unsupported email addresses, employee names, or office hours are displayed.

### Google Maps Embed Card

- HTTPS API-key-free query embed: `https://maps.google.com/maps?q=5263+Agro+Drive,+Frederick,+MD+21703&...`.
- Wrapped in a responsive 4:3 frame (`.contact-map-frame`) with subtle border and card radius.
- Attributes: `loading="lazy"`, `allowfullscreen`, `referrerpolicy="no-referrer-when-downgrade"`, and descriptive accessible `title`.
- Accessible text fallback link provided directly below the iframe for users who cannot view or load third-party frames.

---

## Product Catalog & Family Card Foundation (Milestone C1 / USAP-CATALOG-001)

Standardized catalog presentation patterns established in Milestone C1 and refined in `USAP-CATALOG-001` in `src/USAP.Web/wwwroot/css/site.css` (Section 6C).

### Products Landing Hero (`.internal-hero--products`)

- **Candidate Asset:** Direction C (`usap-products-landing-hero-direction-c-candidate-v1.png`, 2048 × 768 px).
- **Structure:** Semantic `<img>` rendered within the `.internal-hero-media` wrapper before `.internal-hero-overlay`.
- **Loading & Performance:** Configured with `fetchpriority="high"`, `decoding="async"`, `width="2048"`, and `height="768"` to prevent layout shift and optimize largest contentful paint (LCP).
- **Focal Positioning:** `object-fit: cover; object-position: 75% center;` on desktop viewports (≥64rem / 1024px), shifting to `object-position: 80% center;` on mobile/tablet viewports (<64rem). This shifts the focal antenna array geometry into the right-hand view area, keeping the left ~35% clear for the hero heading, eyebrow, and introductory text.
- **Contrast & Legibility:** Left-to-right navy gradient overlay (`.internal-hero-overlay`) maintains text contrast exceeding WCAG AAA (>7:1) across all breakpoints.
- **Graceful Degradation:** The hero wrapper retains its solid `--color-brand-navy` background, ensuring immediate text contrast and structure if the image is disabled or still loading.

### Six-Family Grid Layout (`.products-family-grid`)

- **Desktop (≥64rem / 1024px):** 3-column CSS Grid (`grid-template-columns: repeat(3, 1fr)`).
- **Tablet (48rem–63.9375rem / 768px–1023px):** 2-column CSS Grid (`grid-template-columns: repeat(2, 1fr)`).
- **Mobile (<48rem / <768px):** 1-column layout (`grid-template-columns: 1fr`).
- **Gap:** Uniform `--space-6` (1.5rem / 24px) spacing across rows and columns.
- **Zero Overflow:** Strictly verified zero horizontal overflow across 390px, 768px, 1024px, 1025px, 1440px, and 1920px viewports.

### Product Family Card (`.product-family-card` / `_ProductFamilyCard.cshtml`)

A shared reusable card component implemented in `Pages/Shared/_ProductFamilyCard.cshtml`, utilized uniformly by the `/products` landing grid (3-column) and the homepage Section 5 featured products grid (4-column):

| Class | Role | Styling & Behavior |
|---|---|---|
| `.product-family-card` | Card shell & primary anchor | Outer element is a single semantic anchor (`<a href="@Model.Route" class="card card--interactive product-family-card">`). Uses restrained neutral border (`var(--card-border)`; red top border and navy left accent removed), flex-column layout, relative positioning, and 100% height for equal row heights. Entire card surface is clickable as a native link without JavaScript. Full-card 3px focus-visible outline (`--focus-ring`). Hover and keyboard focus trigger coordinated image zoom (`scale(1.03)`) and editorial CTA arrow shift. |
| `.product-family-card__media` | 16:10 media frame | Edge-to-edge 16:10 aspect ratio (`aspect-ratio: 16 / 10`), zero inset padding, running flush to top and side edges with `overflow: hidden`. |
| `.product-family-card__img` | Product photography | Edge-to-edge frame coverage with `object-fit: cover; object-position: center; display: block; padding: 0;`. Intrinsic dimensions set to `width="1600" height="1000"` (16:10 native aspect ratio), with `loading="lazy"` and `decoding="async"`. Restrained scale zoom (`scale(1.03)`) on card hover and keyboard focus (suppressed under `prefers-reduced-motion: reduce`). |
| `.product-family-card__placeholder` | Native design placeholder | Deliberate CSS fallback for families without candidate imagery or when imagery fails to load. 16:10 edge-to-edge geometry, dark navy gradient, technical blueprint dot grid (`.card-placeholder-pattern`), antenna SVG wireframe, `PRODUCT IMAGERY IN DEVELOPMENT` status badge, and `Design placeholder — replacement imagery pending USAP review.` caption. |
| `.product-family-card__body` | Content area | Flex-grow column with `--space-6` padding. Equal-height alignment pushes action wrapper to card bottom. |
| `.product-family-card__eyebrow` | Category tag | Uppercase 12px bold label in `--color-text-link` (#004a99). Height governed by `--family-card-eyebrow-min-height`. |
| `.product-family-card__title` | Family name | Semantic `<h3>` heading in `--color-brand-navy` (#0d1b2e) with `--leading-snug`. Height governed by `--family-card-title-min-height`. |
| `.product-family-card__summary` | Family description | Factual, conservative summary without disputed engineering claims. Height governed by `--family-card-summary-min-height`. Flex-grow pushes action link to card footer. |
| `.card-link.product-family-card__link` | Editorial CTA indicator | Semantic non-interactive `<span>` styled as a restrained editorial text-link with visible `Explore Family →` text. Zero nested interactive controls. Visibly responds to card hover and keyboard focus with color shift and directional arrow translation. Accessible family-specific target provided by card anchor (`aria-label="Explore @Model.Family.Name"`). Exactly one Tab stop per card. |

#### Context-Sensitive Height Normalization Tokens

To ensure equal card row heights and bottom CTA alignment across varying column widths without line-clamping, ellipses, clipping, or per-card font-size reductions, context-sensitive CSS custom properties are applied at the container level and reset to `auto` on mobile:

| Token | 3-Column Context (`.products-family-grid` ≥768px) | 4-Column Context (`.home-featured-products .products-grid` ≥1024px) | Mobile Context (<768px) |
|---|---|---|---|
| `--family-card-eyebrow-min-height` | `2.25rem` | `2.75rem` | `auto` |
| `--family-card-title-min-height` | `3.25rem` | `4.25rem` | `auto` |
| `--family-card-summary-min-height` | `4.5rem` | `5.5rem` | `auto` |

### Responsive Picture Hero Component (`.internal-hero--family`)

Family pages (`Pages/Products/Family.cshtml`) implement a responsive `<picture>` hero banner:

- **Non-Overlapping Breakpoint:** `<source media="(max-width: 47.999rem)" ...>` routes viewports below 768px to the 768×768 mobile crop, while viewports at 48rem/768px and above receive the 1536×576 desktop image, agreeing with desktop CSS `@media (min-width: 48rem)`.
- **Loading Performance:** `fetchpriority="high"`, `decoding="async"`, and explicit intrinsic `width` and `height` attributes to eliminate cumulative layout shift.
- **Copy-Safe Contrast Overlay (`.internal-hero-overlay--family`):** Dark gradient overlay (`linear-gradient(to right, rgba(13, 27, 46, 0.88) 0%, rgba(13, 27, 46, 0.72) 40%, rgba(13, 27, 46, 0.25) 75%, transparent 100%)`) ensures WCAG AA text contrast for headings against complex antenna imagery.
- **Per-Family Focal Positions:** Tailored focal alignments (e.g., `center 40%` for Log Periodic, `center 45%` for Portable, `center 42%` for Rotator & Control) to preserve structural antenna and controller geometry across breakpoints.

### Engineering Guidance Section (`.selection-help-section`)

- Background surface `.color-surface-subtle` with top and bottom dividers.
- 3-column desktop grid, 1-column mobile layout.
- Restrained ambient cards without heavy left accent borders (`.card.card--ambient`).
- 3 actionable pathways with coordinated card visuals:
  - **Deployment & Mobility:** `usap-guidance-card-deployment-mobility-a1-v1.png` (800 × 500 px).
  - **Coverage & Propagation:** `usap-guidance-card-coverage-propagation-a1-v1.png` (800 × 500 px).
  - **Positioning & Infrastructure:** `usap-guidance-card-positioning-infrastructure-a1-v1.png` (800 × 500 px).
- Media frames: `.selection-help-card__media` (16:10 aspect ratio, full-bleed to top and sides, `overflow: hidden`, radius `var(--radius-media)`).
- Images: `.selection-help-card__img` (`object-fit: cover`, `loading="lazy"`, `decoding="async"`).
- Combined wide Engineering Guidance visual alternate is deliberately deferred.
- **Single Conversion Destination:** Competing 2-button CTA removed; replaced with an inline continuation notice (`.guidance-continuation`) directing users to `#products-faq` and the existing `#closing-cta-heading` closing CTA.

### Product Selection FAQ Component (`.faq-list`)

- Pure semantic HTML implementation using native `<details>` and `<summary>` elements without JavaScript dependencies.
- Styled surface (`var(--color-surface)`), border (`var(--card-border)`), and border-radius (`var(--radius-md)`).
- Accessible keyboard focus and toggle disclosure with smooth chevron rotation indicator (`transform: rotate(90deg)` on `details[open]`).
- Animation suppressed under `prefers-reduced-motion: reduce`.

### Product Group Disclosures & Presentation Components (USAP-CATALOG-002-C1 / C1R1)

The family page renders 16 semantic `<details class="product-group-disclosure">` accordion items:

| Class | Role | Styling & Behavior |
|---|---|---|
| `.product-group-disclosure` | Disclosure container | Bordered surface (`var(--color-surface)`), rounded corners (`var(--radius-md)`), neutral border (`var(--card-border)`), with margin-bottom between groups. Ordinary groups do not use red top borders or navy left borders. |
| `.product-group-summary` | Interactive header | Semantic `<summary>` element. Focus-visible outline, flex layout with header info, badges, and chevron indicator. Zero nested interactive elements. |
| `.product-group-disclosure__badge` | Count indicator | Pill badge showing configuration count when `HasPublishedModelNumber` is true. Omitted when false (e.g. Aperiodic system configuration). |
| `.product-group-disclosure__intro` | Expanded lead copy | Optional introductory paragraph providing high-level operational and architectural context for the group. |
| `.product-group-disclosure__group-specs` | Group-level characteristics | Semantic `<dl>` list displaying top-level technical characteristics common to all models in the group (e.g., Frequency Range, Power Capacity, Polarization). |
| `.product-models-grid` | Model cards grid | Auto-fit CSS grid (`grid-template-columns: repeat(auto-fit, minmax(280px, 1fr))`) reflowing to 1 column on mobile (≤500px). |
| `.product-model-card` | Model card | Compact card surface with subtle neutral border (no red top or navy left borders), padding (`var(--space-4)`), and flex column layout. Displays model code in `<h4>`, optional display name/role, concise description, and model-specific characteristics. |
| `.product-specs-list` | Key-value spec list | Semantic `<dl class="product-specs-list">` using flex layout pairs (`.product-spec-item`) with uppercase term labels (`.product-spec-item__term`) and high-contrast definitions (`.product-spec-item__desc`). |
| `.product-group-disclosure__note` | Model-less guidance note | Styled callout for groups without discrete model codes (e.g., Aperiodic Loop Antenna), providing procurement and configuration guidance. |
| `.resource-badge` | Document format badge | Small uppercase tag (e.g., `PDF · 1.2 MB`) accompanying allowlisted technical resource links. |
| `.product-group-media` | In-disclosure figure | Visual asset rendered within the disclosure. All 16 product groups feature intentional visual coverage (13 A1 candidates + 3 provisional rotator visuals). |
| `.product-group-disclosure__img--contain` | Contain-style visual presentation | Applied to wide-span antenna systems (`lp-1112mr`, `1910`, `1942`) via `.product-group-disclosure__figure--contain` and `.product-group-disclosure__media--contain`. Renders with `object-fit: contain` on a subtle neutral background with generous padding, preventing misleading tight crops of the antenna footprint. |

### Breadcrumb Navigation (`.breadcrumbs` / `.breadcrumb-trail`)

- Semantic `<nav aria-label="Breadcrumb">` landmark positioned immediately below the hero banner as the first element in the page content area. Removed from inside the dark hero banner.
- Aligned to the main content container (`.container`) at the upper-left edge.
- Rendered on the light page background with high-contrast link styling (`--color-text-link`), neutral slash separators (`/`), and standard visible focus rings (`--focus-ring`).
- Ordered list structure with linked `Home` and `Products` ancestors and current family item as non-linked text with `aria-current="page"`.

### Configuration Support Section (`.configuration-support`)

- Authoritative, neutral notice box directing users to engineering contact for custom configurations, mast integration, and formal specifications. Positioned at the base of each family route.

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

---

## Product Group Card & Always-Visible Overview (C1R4)

**Pattern:** Two-layer component combining an always-visible summary card with a nested progressive-enhancement disclosure.

| Element | Class / Selector | Visual Treatment & Behavior |
|---|---|---|
| Outer card | `.product-group-card` | Neutral border (`1px solid var(--color-neutral-85)`), surface background, rounded corners (`var(--radius-surface)`), subtle shadow. |
| Always-visible overview | `.product-group-overview` | Desktop (≥1024px / 64rem): 2-column grid (`40% 1fr`), 38–42% image width, remaining width for text. Mobile (<1024px): Stacked single column with image on top. |
| Product image | `.product-group-overview__figure`, `.product-group-overview__media` | 16:10 aspect ratio, rounded corners, cover presentation. Contain-style modifier (`--contain`) applied for LP-1112MR, 1910, and 1942 to prevent cropping of wide footprints. |
| Overview content | `.product-group-overview__content` | Heading (`.product-group-overview__heading`), short summary (`.product-group-overview__summary`), metadata badges (`.product-group-badge`), and up to three key characteristics (`.product-specs-list--overview`). |
| Nested disclosure | `.product-group-disclosure` | Native `<details>` element with neutral top border (`1px solid var(--color-neutral-85)`). Zero margin/padding on outer wrapper. |
| Disclosure summary bar | `.product-group-summary` | Full-width button surface (`var(--color-surface-subtle)`), text labels for closed/open states (`View Models & Specifications` / `Hide Models & Specifications`; `View Configuration & Technical Details` / `Hide Configuration & Technical Details` for Aperiodic), accessible name via `aria-label`, rotating chevron icon. |
| Expanded detail content | `.product-group-disclosure__content`, `.product-group-disclosure__inner` | Background surface, padding (`var(--space-6)`), contains additional group characteristics (when > 3), model cards grid (`.product-models-grid`), interim technical resources (`.product-group-disclosure__resources`), and contact action. |

---

## Product Selection FAQ Component (C1R4)

**Pattern:** Full-width native accordion disclosure list.

| Element | Class / Selector | Visual Treatment & Behavior |
|---|---|---|
| Section container | `.faq-section` | Standard `.container` width, neutral section surface. |
| FAQ list | `.faq-list` | Full width (`width: 100%`), vertical stack with `var(--space-3)` gap. Eliminates prior narrow-column constraints. |
| FAQ item | `.faq-item` | Native `<details>` with neutral border (`1px solid var(--color-neutral-90)`), rounded corners (`var(--radius-surface)`). |
| FAQ summary | `.faq-summary` | Interactive trigger with bold title, hover color transition, and plus/minus icon toggle via CSS pseudo-elements. |
| FAQ answer | `.faq-content p` | Constrained internally to `max-width: 75ch` for optimal reading line lengths, while disclosure card spans full container width. |

---

## Product Family Dropdown Navigation (C1R5)

**Pattern:** Semantic progressive-enhancement dropdown in site header partial (`Pages/Shared/_Header.cshtml`).

| Element | Class / Selector | Visual Treatment & Behavior |
|---|---|---|
| Dropdown wrapper | `.nav-item-dropdown` | List item container (`<li>`), relative positioning on desktop, full width on mobile. |
| Native disclosure | `.nav-dropdown` | Native `<details id="nav-products-dropdown">` element, keyboard-operable without JavaScript. |
| Dropdown trigger | `.nav-dropdown-toggle` | Semantic `<summary>` element with text and rotating chevron SVG. Styled identically to primary nav links. Never treated as an anchor (`<a>`), preventing unintended mobile menu dismissal when expanded. |
| Dropdown menu | `.nav-dropdown-menu` | Unordered list (`<ul role="list">`) containing All Products (`/products`) plus the six family routes. |
| Desktop presentation | `@media (min-width: 64rem)` | Absolute dropdown positioned below header (`top: 100%; left: 0;`), min-width 18.5rem, max-width `calc(100vw - 2rem)`, white surface, subtle border and elevation shadow (`box-shadow: 0 10px 25px -5px rgba(0,0,0,0.12)`). Escape key and click-outside dismissal managed by `site-navigation.js`. |
| Mobile presentation | `@media (max-width: 63.9375rem)` | In-flow vertical accordion expansion within mobile navigation drawer. Indented child links (`padding-left: var(--space-10)`), semi-transparent contrast background (`rgba(0,0,0,0.18)`), and borders matching dark navy drawer aesthetic. |
| Dropdown links | `.nav-dropdown-link` | Block anchor elements with left red accent border on hover/focus/active. Full keyboard focus outline (`--focus-ring` on desktop, `--focus-ring-dark` on mobile). |

---

## Family-Page Closing Sequence & Configuration Support (C1R5)

**Sequence Order:**
1. Product Groups List (`.product-groups-list`)
2. Configuration Support Notice (`.configuration-support-notice`)
3. Return to All Product Families Navigation (`.family-back-nav`)
4. Closing CTA (`.family-closing-cta`)
5. Site Footer (`_Footer.cshtml` in layout)

| Element | Class / Selector | Visual Treatment & Behavior |
|---|---|---|
| Support notice card | `.configuration-support-notice` | Subtle surface (`var(--color-surface-subtle)`), thin border (`var(--color-border-subtle)`), rounded corners, flex layout (row on desktop ≥48rem, column on mobile). Margin top `var(--space-8)`, margin bottom `var(--space-6)`. |
| Support notice CTA | `.configuration-support-action .btn-primary` | Standardized to USAP red brand button (`.btn-primary`), establishing a prominent conversion action for tailored application guidance. Reuses core design token without duplicate CSS declarations. |
| Return navigation | `.family-back-nav .btn-outline` | Positioned immediately below Configuration Support and above the closing CTA. Retains secondary back-navigation styling (`.btn-outline`) without competing with primary conversion actions. |

---

## Hero Resilience Architecture (C1R5)

**Pattern:** Content-driven responsive hero container eliminating fixed heights and text clipping across all standard viewports and browser zoom levels (80%, 100%, 125%).

| Element | Rule / Property | Resilience Implementation |
|---|---|---|
| Container height | `min-height: 22rem; height: auto; max-height: none;` | Height is strictly driven by content and vertical padding; no rigid constraints that can cause overflow or text truncation. |
| Responsive padding | `padding-block: clamp(var(--space-8), 4vw, var(--space-12));` | Fluid vertical breathing room adapting smoothly to narrow mobile screens and magnified zoom levels. |
| Typography scaling | `font-size: clamp(1.875rem, 3.25vw + 0.5rem, 2.75rem);` | Balanced heading scale with `overflow-wrap: break-word` and `text-wrap: balance` to prevent line blowout at 125% zoom. |
| Box model sizing | `box-sizing: border-box;` | Uniform box sizing across hero and content wrapper prevents boundary miscalculations. |
| Breadcrumbs position | `.family-breadcrumbs` | Remains positioned strictly in Section 2 below the hero banner. |

---

## Mobile Navigation Overlay & Interaction Contract (C1R6)

**Pattern:** Viewport-overlay mobile navigation drawer anchored beneath the header bar without page reflow.

| Element | Class / Selector | Visual Treatment & Behavior |
|---|---|---|
| Header container | `.site-header-inner` | In mobile (<64rem), `margin-bottom: 0` on brand link and toggle button prevents vertical expansion or layout shift. |
| Fixed overlay drawer | `.js-nav-ready .primary-nav-list` | `position: fixed; top: var(--mobile-header-bottom, 4.25rem); left: 0; right: 0; width: 100%; height: calc(100dvh - var(--mobile-header-bottom, 4.25rem)); overflow-y: auto; overscroll-behavior: contain; z-index: var(--z-index-dropdown, 100);`. Decouples menu from document flow, eliminating downward push of page hero. |
| Viewport scroll lock | `html.nav-open-lock, body.nav-open-lock` | `overflow: hidden !important; overscroll-behavior: none;`. Prevents background body scrolling. JS records `window.scrollY` on open and restores exact scroll position on close. |
| Focus containment | `#main-content, .site-footer, .skip-link` | Receives standard `inert` attribute while menu is open. Navigation controller tracks modified elements to remove only attributes it applied. |
| Centered mobile products control | `.nav-dropdown-toggle--mobile` | `display: flex; align-items: center; justify-content: center; gap: var(--space-2); list-style: none;`. Products label and rotating chevron SVG are centered together as a single visual group. Child dropdown links remain left-aligned (`padding-left: var(--space-10)`). |
| Native disclosure state | `.nav-dropdown-toggle` | Relies on native `<details>/<summary>` state. Static `aria-haspopup` and `aria-expanded` attributes omitted. |

---

## Family-Page Heading Hierarchy & Subordination (C1R6)

**Pattern:** Strict, accessible heading hierarchy ensuring the hero H1 remains the unambiguous primary page title while post-hero sections are clearly subordinate.

| Level | Role / Element | Selector | Visual Treatment & Scale |
|---|---|---|---|
| **H1** | Primary Visible Page Title | `.internal-hero--family .internal-hero-title` | `font-size: clamp(1.875rem, 3.25vw + 0.5rem, 2.75rem); font-weight: 700; color: var(--color-text-inverse);`. Clear, dominant page identifier. |
| **H2** | Family Models & Configurations Section | `.family-groups-header .section-heading` | `font-size: clamp(1.5rem, 2.5vw, 1.875rem); font-weight: 700; color: var(--color-brand-navy);`. Visibly subordinate to the hero H1. Paired with family-specific `.section-eyebrow` (`--color-brand-red`). |
| **H3** | Product Group Title | `.product-group-overview__heading` | `font-size: var(--text-xl); font-weight: 700; color: var(--color-brand-navy);`. |
| **H4** | Model Cards & Subsections | `.product-model-card__title` / Subsection headings | `font-size: var(--text-md); font-weight: 600;`. |
| **H5** | Nested Configurations | When applicable beneath H4 subsections | `font-size: var(--text-base); font-weight: 600;`. |

---

## Shared Always-Visible Sticky Header & Refined Mobile Navigation (C1R8)

**Pattern:** Persistent, always-visible sticky header and content-height mobile navigation overlay with centered top-level controls.

### 1. Always-Visible Sticky Header Component

| Property / Rule | Value | Purpose |
|---|---|---|
| Positioning | `position: sticky; top: 0;` | Keeps header anchored to viewport top and visible at all times during normal page operation. While mobile nav scroll lock is active (`.nav-open-lock`), temporarily viewport-pinned via `position: fixed; top: 0; left: 0; right: 0; width: 100%;` to maintain top-of-viewport anchoring while `html`/`body` overflow is locked. |
| Stacking order | `z-index: var(--z-sticky)` (200); elevated to `var(--z-overlay)` (300) when mobile menu is open | Ensures header and mobile menu remain above standard content and the translucent click-away backdrop. |
| Hide-on-scroll state machine | None (Removed in C1R8) | The scroll-direction auto-hide state machine, transform transitions, and idle timer were completely removed per project-lead direction. |
| Responsive positioning | `--mobile-header-bottom: <header.offsetHeight>px` | Stable offsetHeight measurement used to place mobile overlay directly below the sticky header. |

### 3. Content-Height Mobile Navigation Panel & Backdrop

| Element | Selector | Implementation & Behavior |
|---|---|---|
| Menu panel | `.js-nav-ready .primary-nav-list` | `position: fixed; top: var(--mobile-header-bottom, 4.25rem); left: 0; right: 0; width: 100vw; height: auto; max-height: calc(100dvh - var(--mobile-header-bottom, 4.25rem)); overflow-y: auto; overscroll-behavior: contain;`. Content-driven height ending immediately after Request a Quote button + padding. |
| Click-away backdrop | `.js-nav-ready .nav-backdrop` | `position: fixed; inset: 0; top: var(--mobile-header-bottom); background-color: rgba(0, 0, 0, 0.40); backdrop-filter: blur(2px); z-index: calc(var(--z-overlay) - 1);`. Translucent shade distinguishing unused screen space, dismissing navigation on outside click. |
| Centered top-level controls | `.primary-nav-link`, `.nav-dropdown > summary`, `.nav-cta` | `display: flex; justify-content: center; align-items: center; width: 100%; text-align: center;`. Horizontally centers Home, Products, Technical Resources, About Us, Contact Us, and Request a Quote. |
| Unified Products group | `.nav-dropdown-summary-group` | `display: inline-flex; align-items: center; justify-content: center; gap: var(--space-2);`. Centers Products label and rotating chevron together as a single visual unit. |
| Submenu child links | `.nav-dropdown-link` | Left-aligned and indented (`padding-left: var(--space-10)`) for scanability. |
| Non-displacing active accent | `.primary-nav-link::before`, `.nav-dropdown > summary::before` | Absolutely positioned 3px left bar (`position: absolute; left: 0; top: 0; bottom: 0; width: 3px; background-color: var(--color-accent-dark-surface);`). Zero horizontal displacement of centered text. |

---

## Shared Internal-Hero Foundation & Full-Image Presentation (C1R9)

**Pattern:** Unified desktop/tablet banner baseline height with non-cropping right-side media containment and atmospheric mobile fallback.

### 1. Hero Baseline Geometry Tokens & Rules

| Token / Property | Selector | Value | Visual Purpose |
|---|---|---|---|
| Baseline Desktop Min-Height | `:root` | `--internal-hero-min-height: 24.2875rem;` | Unified baseline height (~388.6px) measured from product-family hero geometry at 1440×900. |
| Fluid Height Contract | `.internal-hero` | `height: auto; min-height: 22rem;` | Content-resilient container sizing allowing natural expansion under longer copy or zoom without clipping. |
| Desktop/Tablet Min-Height | `.internal-hero` (>= 48rem) | `min-height: var(--internal-hero-min-height, 24.2875rem);` | Enforces exact height parity (388.6px ±0.0px) across all standard internal pages where copy fits. |
| Ultra-Wide Hero Min-Height (C1R11) | `.internal-hero` (>= 100rem) | `min-height: clamp(24.2875rem, 22vw, 28rem);` | Moderate responsive height scaling from ~388.6px (1600px) to ~422.4px (1920px), capped at 448px (2560px), allowing contained 8:3 image to expand without dominant height. |
| Standard Vertical Padding | `.internal-hero` | `padding-block: clamp(var(--space-8), 4vw, var(--space-12));` | Fluid vertical rhythm scaling smoothly across viewports. |
| About Us Vertical Padding (C1R10) | `.internal-hero--about` (>= 48rem) | `padding-block: 1.75rem;` (28px) | Normalizes `/about-us` to exact 388.59px baseline height with balanced ~28.8px top / ~29.8px bottom spacing. |

### 2. Media Containment & Overlay System

| Element | Viewport Scope | Implementation | Presentation Behavior |
|---|---|---|---|
| Media Region (`.internal-hero-picture`, `.internal-hero-image`) | Desktop (>= 64rem / 1024px) | `position: absolute; left: auto; right: 0; width: 65%; max-width: 1100px; height: 100%;` | Bounded right-side media slot aligned to right edge, leaving left side unobstructed for copy. |
| Media Region (`.internal-hero-picture`, `.internal-hero-image`) | Tablet (48rem–63.999rem / 768px–1023px) | `position: absolute; left: auto; right: 0; width: 75%; height: 100%;` | Proportional tablet media region providing generous visual area without copy collision. |
| Media Sizing (`.internal-hero-img`, `.internal-hero-image`) | Desktop & Tablet (>= 48rem) | `object-fit: contain; object-position: right center;` | Displays full 8:3 source composition (2048×768 or 1536×576) with zero top/bottom cropping. |
| Top-Biased Contrast Overlay (Candidate D) | Desktop (>= 64rem) & Tablet (48rem–63.999rem) | Multi-background gradient: top-biased vertical dissolve layered over horizontal copy protection | `linear-gradient(to bottom, var(--color-brand-navy) 0%, rgba(13, 27, 46, 0.75) 5%, transparent 18%, transparent 88%, rgba(13, 27, 46, 0.45) 96%, var(--color-brand-navy) 100%)` layered over `linear-gradient(to right, ...)`. Eliminates harsh sky cutoff lines while preserving natural sky, hardware, and copy contrast. |
| Ultra-Wide Media Track (C1R11) | Ultra-Wide (>= 100rem / 1600px) | `position: absolute; left: auto; right: 0; width: clamp(65rem, 59vw, 75rem); max-width: none; height: 100%;` | Right-pinned media slot scaling from 1040px (1600px) to 1132.8px (1920px), capped at 1200px (2560px). Zero right navy gutter; image terminates at physical right browser edge. |
| Ultra-Wide Contrast Overlay (C1R11) | Ultra-Wide (>= 100rem / 1600px) | 2-layer gradient: Candidate D vertical fade layered over relaxed canvas-anchored horizontal copy protection | `linear-gradient(to right, var(--color-brand-navy) 0, var(--color-brand-navy) calc(max(0px, (100vw - 100rem) / 2) + 24rem), rgba(13, 27, 46, 0.72) calc(max(0px, (100vw - 100rem) / 2) + 38rem), rgba(13, 27, 46, 0.25) calc(max(0px, (100vw - 100rem) / 2) + 54rem), transparent calc(max(0px, (100vw - 100rem) / 2) + 68rem))` layered under Candidate D vertical fade. Zero right-to-left gutter gradient. |
| Mobile Media & Overlay | Mobile (< 48rem / 768px) | `object-fit: cover; object-position: center; overlay: rgba(13, 27, 46, 0.90);` | Atmospheric background presentation providing safe contrast for white typography on compact screens. |
