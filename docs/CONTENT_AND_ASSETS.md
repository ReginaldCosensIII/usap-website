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
| About Us | `/about-us` | 🟡 Provisionally complete with reference-grounded imagery integrated; uses shared hero and closing CTA. Final client approval, company history, and manufacturing details pending. |
| Contact Us | `/contact-us` | 🟡 Shared hero and two-column form/sidebar layout implemented. Verified public contact details and Google Maps embed added. Form behavior preserved. Copy remains provisional. |
| Request a Quote | `/request-a-quote` | 🟡 Shared hero and two-column form/sidebar layout implemented. Verified public contact details and Google Maps embed added. Form behavior preserved. Copy remains provisional. |
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

## Shared Design Assets

| Asset | Path | Dimensions / Size / Hash | Notes |
|---|---|---|---|
| Light-surface technical overlay | `src/USAP.Web/wwwroot/images/shared/backgrounds/usap-light-surface-technical-signal-overlay-v1.png` | 1983x793, 511351 bytes, SHA256: B85D9E10639BB6138666647E69D1709EE124A581A12187077B95F182FBE116C1 | Approved transparent background composite. Currently deployed only on `/not-found` and `/Error` (desktop, ≥1025 px). **Not approved for Contact Us or Request a Quote.** |
| Calibration marks V2 | `src/USAP.Web/wwwroot/images/homepage/decorative/usap-calibration-marks-v2.svg` | SVG, SHA256: 48CFD82707231B744760B2B529601EA0293C6A27F9C23BBCAADFB3FD86C6EAE8 | Approved optional decorative asset. Not deployed on any page. Available for possible future use. Must not be revised or versioned without approval. |

**Design Rules for Textures:**
- Texture must remain decorative, low contrast, nonessential, and strictly subordinate to content.
- Do not use page-wide graph-paper grids or place target-like rings directly around headings.
- The approved composite PNG is deployed on the 404 and 500 error pages only. It is not approved for Contact Us, Request a Quote, or any other interactive form page.
- Signal-arcs V2 SVG was rejected and must not be used or restored.
- Reusable standalone SVG texture modules are deferred to a dedicated future visual-asset pass, if time permits after substantial page work is complete.
- Experimental SVG and raster modules were excluded because they had not reached the required visual quality.

---

## Internal-Page Asset Review Package & Promoted Assets (C1)

Canonical archive preserved in repository: `project-input/USAP_Internal_Page_Asset_Review_Package_2026-09-11.zip` (19,112,932 bytes).
SHA-256: `3BB2A04AAED355C9DF3209F49C3A92D57E946F12FB1D968868F6363904391267`

### Promoted Assets in `wwwroot`

| Intended Use | Relative Path in `src/USAP.Web/wwwroot/` | Dimensions | SHA-256 | Accuracy Classification & Status |
|---|---|---|---|---|
| About Us Hero | `images/about/usap-about-hero-lp1017-clean-v2.png` | 2048 × 768 | `D6D1C8F8B0515818B924FC3AD32BCC90D47BDD02B15DE9C154C2F7037D4D6557` | Reference-grounded visualization (LP-1017 family). Production-intent review asset pending USAP engineering approval. |
| About Us: Manufacturing & Testing | `images/about/usap-about-manufacturing-rf-interface-lp1019-candidate-c-v2.png` | 1536 × 1024 | `99F8873AA732B6B90611F976E8E8A795EBCE00A0EE581A7FADF3B866081E4033` | Reference-grounded visualization (Candidate C: LP-1019 RF interface). Grounded in published specs. Does not depict real facility or measured test. |
| About Us: Integrated Support | `images/about/usap-about-integrated-support-lp1112mr-clean-v2.png` | 1536 × 1024 | `87EB5F2A6BA0AA7A938CECF590D52193C87E3C0CA914CBC77303E868A7A79423` | Reference-grounded visualization (LP-1112MR transportable antenna system). Pending USAP product-owner approval. |
| Contact Us Hero (Revised) | `images/contact-us/usap-contact-hero-communication-signal-candidate-c-v1.png` | 2048 × 768 | `B020AD39E6A8CEFD15C584DCA9B77503070CDE8FD0C33B63C6A556F645CD71E4` | Conceptual communication illustration (Candidate C revision). Medical-appearing ECG/EKG waveform replaced with clean communication signal bars. Production asset duplicated in `project-input/USAP_Contact_Hero_Revision_2026-09-12/`. Pending USAP visual approval. |
| Request a Quote Hero | `images/request-a-quote/usap-quote-hero-technical-planning-candidate-a-v1.png` | 2048 × 768 | `6269BE6639F6CCB31A214CC05D7EF9EF587A57A782244778CBAD181261C53FBE` | Conceptual communication illustration (Candidate A: technical planning). Does not depict specific customer contract or quote. |
| Technical Resources Hero | `images/technical-resources/usap-technical-resources-hero-requirements-document-candidate-b-v1.png` | 2048 × 768 | `03FE67FA5A836A472A4C19C172B44AAA154CE282088155E135027FEC01CEF48A` | Conceptual communication illustration (Candidate B). **Store only**: not referenced or loaded during C1/C2. |

### Contact Us Hero Revision and Provenance (C2)

The Contact Us hero image was revised in C2 to address potential misinterpretation of the illuminated waveform in the original Candidate C:
1. **Original Archived Version:** Preserved inside the unchanged September 11 archive `project-input/USAP_Internal_Page_Asset_Review_Package_2026-09-11.zip` (`SHA-256: C6F70E4B0CA756EB16486B194BACED5B9412529E196C6D0A5DF253E5BE588E88`). It contains a jagged waveform that could be mistaken for an ECG/EKG or medical monitoring telemetry display.
2. **Revised Production Version:** Deployed in `src/USAP.Web/wwwroot/images/contact-us/usap-contact-hero-communication-signal-candidate-c-v1.png` (`SHA-256: B020AD39E6A8CEFD15C584DCA9B77503070CDE8FD0C33B63C6A556F645CD71E4`). It preserves the dark blue/navy lighting, overall composition, glass speech bubble, and right focal weighting of Candidate C, but replaces the medical-looking waveform with three clean, nonmedical communication signal bars.
3. **Preserved Runtime Reference:** The filename was kept identical so that zero Razor markup or CSS selector changes were required.
4. **Supplemental Provenance:** A byte-for-byte copy is preserved in `project-input/USAP_Contact_Hero_Revision_2026-09-12/` with matching checksum and explanatory README.
5. **Approval Status:** Remains pending formal USAP/client visual approval.

### Alternate Candidates Retained Exclusively in Archive

The following exploratory candidates are preserved exclusively inside `project-input/USAP_Internal_Page_Asset_Review_Package_2026-09-11.zip` and are not deployed to `wwwroot`:
- Manufacturing Candidate A: `usap-about-manufacturing-structural-inspection-aperiodic-loop-candidate-a-v2.png`
- Manufacturing Candidate B: `usap-about-manufacturing-rotation-mechanism-r3500-candidate-b-v2.png`
- Manufacturing comparison sheet: `usap-about-manufacturing-candidates-comparison-v2.png`
- Contact Us Candidate A: `usap-contact-hero-support-headset-candidate-a-v1.png`
- Contact Us Candidate B: `usap-contact-hero-telephone-receiver-candidate-b-v1.png`
- Request a Quote Candidate C: `usap-quote-hero-custom-engineering-candidate-c-v1.png`

### Superseded Provisional About Assets Removed

The following provisional placeholders were completely replaced and deleted from source control in C1:
- `src/USAP.Web/wwwroot/images/about/usap-about-hero-provisional-v1.png`
- `src/USAP.Web/wwwroot/images/about/usap-about-manufacturing-testing-provisional-v1.jpg`
- `src/USAP.Web/wwwroot/images/about/usap-about-integrated-support-provisional-v1.jpg`

---

## About Us copy & asset status (C1 reference-grounded integration)

Copy on `/about-us` is strictly anchored to published factual statements from the current public USAP website as provisional migration-source authority.

* **Status:** Provisionally complete for current phase with reference-grounded imagery integrated. Client review and formal approval remain required.
* **Hero Image:** `usap-about-hero-lp1017-clean-v2.png` integrated via `.internal-hero--about`.
* **Manufacturing Image:** Candidate C (`usap-about-manufacturing-rf-interface-lp1019-candidate-c-v2.png`) integrated into Section 3 intro frame.
* **Integrated Support Image:** `usap-about-integrated-support-lp1112mr-clean-v2.png` integrated into Section 4 intro frame.
* **Closing CTA:** The shared default closing CTA component (`.closing-cta--default`) with signal background image is adopted until page-specific variants are approved.
* **Public source URLs:**
  * `https://www.usantennaproducts.com/about-usap/`
  * `https://www.usantennaproducts.com/antenna-category/rotator-systems/`
  * `https://www.usantennaproducts.com/antenna-category/digital-rotator-controller/`
  * `https://www.usantennaproducts.com/antenna-category/tower-systems-accessories/`
* **Supported factual statements implemented:**
  * Military, government, and commercial sectors served.
  * System offerings include rotator systems, digital rotator controllers, tower systems, and related accessories.
  * Rotator control capabilities support antenna operation from locations with a secure internet connection.
  * Integrated services cover requirements evaluation, system construction, installation design, and installation.
  * Customer support includes phone assistance and manuals/software for purchased systems without additional charges.
  * Dedicated, personalized service as a core business principle.
* **Missing client information (requires client input before final expansion):**
  * Founding date and company origin
  * Founder or leadership history
  * Ownership milestones
  * Major company milestones
  * Facility and manufacturing details
  * Testing and quality-control processes
  * Certifications or standards
  * Approved government, military, or commercial project examples
  * Approved customer history or case studies
  * These items must not be invented and remain pending client delivery.

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
