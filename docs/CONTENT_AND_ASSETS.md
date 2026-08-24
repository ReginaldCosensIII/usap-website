# Content and Assets

Tracks content status, reference assets, and outstanding client inputs.
Validation evidence and time records are in the human lead's external tracking; they do not belong here.

---

## Status definitions

| Symbol | Meaning |
|---|---|
| ✅ Approved | Client-supplied or contractually authorized; verified |
| 🟡 Placeholder | Development placeholder in place; production content missing |
| ❌ Missing | Required; not yet received |
| N/A | Not applicable to this page/item |

---

## Homepage — provisional section order

The section order is derived from the wireframe references and planning discussion.
Wireframes are structural and responsive references. The high-fidelity design concept is provisional design direction.
**Copy, imagery, claims, contact information, product names, testimonials, and social links shown in the design concept are not approved.**

| # | Section | Status |
|---|---|---|
| 1 | Header / primary navigation | 🟡 Structural shell; production logo, branding, mobile menu pending |
| 2 | Hero | ❌ Headline, subheadline, CTA, imagery pending |
| 3 | Trust / authority strip | ❌ Certifications, affiliations, or trust indicators pending (verified facts only) |
| 4 | Why United States Antenna Products | ❌ Value proposition and differentiators pending |
| 5 | Communication applications / industries served | ❌ Application areas pending verified client content |
| 6 | Featured product families | ❌ Family names, descriptions, imagery pending (up to six approved families) |
| 7 | Client feedback / testimonials | ❌ Testimonials with attribution and publication permission pending |
| 8 | Technical Resources preview | ❌ Resource titles and descriptions pending |
| 9 | Request Information / Request Quote CTA | ❌ Copy and CTA text pending |
| 10 | Footer | 🟡 Company name placeholder; contact, social, legal links pending |

---

## Page content status

| Page | Route | Status |
|---|---|---|
| Home | `/` | 🟡 Stub |
| Products listing | `/products` | 🟡 Stub — up to six approved families pending |
| Product family detail | `/products/{familySlug}` | ❌ All product data pending |
| Technical Resources | `/technical-resources` | ❌ Documents and organization pending |
| About Us | `/about-us` | ❌ Company copy pending |
| Contact Us | `/contact-us` | ❌ Form, contact details pending |
| Request a Quote | `/request-a-quote` | ❌ Form, recipients pending |
| Thank You | `/thank-you` | 🟡 Provisional stub — form behavior undefined |
| Privacy Policy | Not created | ❌ Legal text required from client |
| Terms of Use | Not created | ❌ Legal text required from client (if applicable) |

### Global / shared

| Item | Status | Notes |
|---|---|---|
| Site name | ✅ | "United States Antenna Products, LLC" |
| Navigation labels | ✅ | Home, Products, Technical Resources, About Us, Contact Us, Request a Quote |
| Production logo | ❌ | SVG or high-resolution PNG required |
| Favicon | ✅ Approved | `favicon.ico`, 20,222 bytes. Source: current public USAP website. Copied into the repository by the human project lead. Reuse authorized by the human project lead. Approved for this implementation. **Not** an original production logo or primary brand-source file. Wired in `_Layout.cshtml` via `<link rel="icon" type="image/x-icon" href="~/favicon.ico" />`. |
| Footer contact details | ❌ | Address, phone, email pending |
| Social links | ❌ | URLs and display permission pending |
| Copyright phrasing | 🟡 | Phrasing not yet approved |

---

## Reference assets

All four files are in `docs/reference-materials/`. They are **not served publicly** and must not be copied to `wwwroot`.

| Filename | Dimensions | Source | Status | Permitted use | Replacement |
|---|---|---|---|---|---|
| `wireframe-desktop.png` | 721 × 2048 px | Workflow package | Structural and responsive reference — not a production asset | Internal layout and responsive planning only | Yes — production design required |
| `wireframe-mobile.png` | 118 × 2048 px | Workflow package | Structural and responsive reference — not a production asset | Internal layout and responsive planning only | Yes — production design required |
| `homepage-design-concept.png` | 941 × 1672 px | Workflow package | **Provisional design direction only.** Displayed copy, imagery, claims, contact info, product names, testimonials, and social links are **not approved**. | Internal design direction reference only | Yes — approved final design required |
| `logo-reference.png` | 220 × 135 px | Workflow package | Low-resolution screenshot. Not production-ready. Must not be traced, upscaled, or presented as an approved logo. May only be used as a documented temporary placeholder if explicitly authorized. | Internal brand reference only | Yes — production SVG or high-resolution PNG required from client |

---

## Outstanding client inputs

**Priority 1 — before Core Site Build:**

- [ ] Original SVG or high-resolution PNG logo and brand standards (colors, typography, usage rules)
- [ ] Final company positioning, value proposition, About Us copy, history, location/manufacturing facts
- [ ] Verified certifications, affiliations, or claims with supporting documentation
- [ ] Business address, phone(s), email(s) for public display, business hours
- [ ] Approved social profile URLs
- [ ] Contact form and quote form recipient email addresses (supplied separately through secrets management)
- [ ] Final navigation label and sitemap approval

**Priority 2 — before Catalog & Resources:**

- [ ] Final count and grouping of product families approved by client (up to six)
- [ ] For each family: name, slug, descriptions, model names, specifications, frequency ranges, applications, accessories, related products
- [ ] Original product photography or permission to reuse current-site imagery
- [ ] Approved PDF/data sheet set with category classification and any distribution restrictions

**Priority 3 — before Forms & Search:**

- [ ] SMTP provider/configuration owner (credentials supplied via secrets management, not here)
- [ ] GA4 Measurement ID
- [ ] Google Search Console access

**Priority 4 — before QA & Launch:**

- [ ] Approved Privacy Policy and Terms of Use text (CES does not draft legal content)
- [ ] Testimonials with attribution and written publication permission
- [ ] Approved hero and page imagery (licensed or owned)
- [ ] Current URL list for redirect preservation
- [ ] IIS/DNS/SSL contacts and server access for CES Dev and Production
- [ ] Production HTTPS certificate or procurement plan
- [ ] Deployment workflow decision

---

## Rules

- No engineering specifications, performance claims, military/government relationships, or customer references may be added without written client approval.
- Contact details and form recipients are stored in secrets management, never in source code or documentation.
- Legal text must come from the client; CES does not draft or supply legal content.
