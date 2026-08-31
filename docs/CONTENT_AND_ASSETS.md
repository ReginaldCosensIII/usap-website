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
| 1 | Header / primary navigation | 🟡 Implemented with provisional/current brand assets. Final production logo and brand approval remain pending. |
| 2 | Hero | 🟡 Implemented with provisional copy, video/poster media, and CTA content. Final client review and approval remain pending. |
| 3 | Trust / authority strip | 🟡 Implemented provisionally. Claims and capability statements still require final verification or approval. |
| 4 | Why United States Antenna Products | 🟡 Implemented with provisional/current-public-source content and assets. Final approval remains pending. |
| 5 | Communication applications / industries served | 🟡 Implemented provisionally. Final application language and approval remain pending. |
| 6 | Featured product families | 🟡 Implemented provisionally. Final approved family structure, product data, and imagery remain pending. |
| 7 | Client feedback / testimonials | 🟡 Structural/provisional placeholder only. Approved testimonials, attribution, and publication permission remain pending. |
| 8 | Technical Resources preview | 🟡 Implemented provisionally. Final approved resource titles, descriptions, files, and organization remain pending. |
| 9 | Request Information / Request Quote CTA | 🟡 Implemented provisionally. Final copy approval remains pending. |
| 10 | Footer | 🟡 Implemented provisionally. Final brand assets and approved contact, social, and legal presentation remain pending. |

---

## Page content status

| Page | Route | Status |
|---|---|---|
| Home | `/` | 🟡 Provisional implementation. Final client copy, imagery, branding, and approval remain pending. |
| Products listing | `/products` | 🟡 Stub — up to six approved families pending |
| Product family detail | `/products/{familySlug}` | ❌ All product data pending |
| Technical Resources | `/technical-resources` | ❌ Documents and organization pending |
| About Us | `/about-us` | ❌ Company copy pending |
| Contact Us | `/contact-us` | 🟡 Provisional inquiry form exists. Public contact presentation and production delivery/recipient configuration remain pending. |
| Request a Quote | `/request-a-quote` | 🟡 Provisional inquiry form exists. Production delivery/recipient configuration and final copy remain pending. |
| Thank You | `/thank-you` | 🟡 Provisional confirmation page exists. Final production submission/redirect behavior and copy remain pending. |
| Privacy Policy | Not created | ❌ Legal text required from client |
| Terms of Use | Not created | ❌ Legal text required from client (if applicable) |

### Global / shared

| Item | Status | Notes |
|---|---|---|
| Site name | ✅ | "United States Antenna Products, LLC" |
| Navigation labels | ✅ | Home, Products, Technical Resources, About Us, Contact Us, Request a Quote |
| Production logo | ❌ | SVG or high-resolution PNG required |
| Favicon | ✅ Approved | `favicon.ico`, 20,222 bytes. Source: current public USAP website. Copied into the repository by the human project lead. Reuse authorized by the human project lead. Approved for this implementation. **Not** an original production logo or primary brand-source file. Wired in `_Layout.cshtml` via `<link rel="icon" type="image/x-icon" href="~/favicon.ico" />`. |
| Social preview image | 🟡 Placeholder | **PROVISIONAL — REPLACE BEFORE LAUNCH**. The current homepage hero poster (`/images/homepage/hero/usap-home-hero-poster-lp-1112mr.png`, 1,939,324 bytes, 1915x821) is temporarily reused as the sitewide social-preview image. A dedicated branded 1200x630 social-preview image is required before production launch. Page-specific social images are optional later enhancements. |
| Footer contact details | 🟡 Placeholder | **CURRENT PUBLIC SOURCE — REUSE PENDING CLIENT REVISION**. Phone: `240-341-7120`. Fax: `240-371-4980`. Address: `5263 Agro Drive, Frederick, MD 21703`. Official website: `https://www.usantennaproducts.com/`. Public email remains missing. Reconfirm before production launch. |
| Social links | 🟡 Placeholder | **CURRENT PUBLIC SOURCE — REUSE PENDING CLIENT REVISION**. Facebook: `https://www.facebook.com/usantennaproducts/`. LinkedIn: `https://www.linkedin.com/company/united-states-antenna-products-l-l-c-/`. Reviewed 2026-08-31. Intended for future `Organization.sameAs`. No X/Twitter profile is currently approved. Reconfirm before production launch. |
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
- Publicly displayed business contact information may be stored in source and project documentation when sourced from USAP’s official public website or supplied by CES/USAP.
- Public contact information must be reconfirmed before production launch.
- Private form-recipient addresses, SMTP credentials, API keys, and other operational secrets must never be committed to source or documentation.
- Private operational values must use approved secrets management or environment configuration.
- Legal text must come from the client; CES does not draft or supply legal content.
- Information from the current official USAP website may be reused during the rebuild.
- Current public information remains authoritative for working implementation unless CES or USAP provides a correction.
- Critical contact, legal, domain, and launch information must still be reconfirmed before production deployment.
- Provisional metadata and assets must be replaced or formally accepted before launch.
