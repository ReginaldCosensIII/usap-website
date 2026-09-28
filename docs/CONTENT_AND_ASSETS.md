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
| Products listing | `/products` | ✅ Structured 6-family catalog landing page implemented with A2F provisional candidate assets (USAP-CATALOG-001). Detail pages deferred to C2. |
| Product family detail | `/products/{familySlug}` | ✅ Evidence-safe product detail presentation implemented across all 6 family routes and 16 groups with always-visible overviews, nested native disclosures, and local canonical technical document links (USAP-CATALOG-002-C1/TECHDOC-001). All 16 product groups feature intentional visual coverage and local document integration. |
| Technical Resources | `/technical-resources` | ✅ Complete (USAP-TECHDOC-001 / USAP-TECHDOC-001-R1). Direct public access library hosting 17 canonical PDFs, client-side search, family category filtering, preset deep links, and branded HTML document detail experience (`/technical-resources/document/{slug}`) with embedded native PDF preview and closing CTA. |
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

## Product Catalog & Product Family Content & Assets (USAP-CATALOG-001)

Documented in `USAP-CATALOG-001` (incorporating canonical C1/C1A/C1B foundation and A2F candidate asset integration). Establishes the authoritative repository-native catalog baseline, promoted provisional candidate assets, and conservative copy guidelines.

### A2F Product Catalog Review Package Intake

Canonical archive preserved in repository root (gitignored):
`project-inputs/USAP_Product_Catalog_A2F_Review_Package_2026-09-15.zip` (10,950,498 bytes).
SHA-256: `0831c27bb1a66bf4ef2841a92fae94eb2c1316fcf1d1d3a9be2cab546eabced5`
Archive structure: 54 total entries containing hero candidates (Directions A, B, C), family-card visual candidates, comparison sheets, and internal review derivatives. Verified byte-identical and integrity-checked on intake.

### Promoted Provisional Candidate Assets in `wwwroot/images/products/`

All seven promoted assets were integrated into `src/USAP.Web/wwwroot/images/products/` without alteration from their canonical review package files. Each asset retains byte-identical hash preservation against `HASH_PRESERVATION_REGISTER.csv`:

| Asset / Role | Relative Path in `src/USAP.Web/wwwroot/` | Dimensions | Size | SHA-256 | Classification | Approved Alt Text | Prohibited Claims & Restrictions | Replacement Requirements |
|---|---|---|---|---|---|---|---|---|
| Products Landing Hero (Direction C) | `images/products/usap-products-landing-hero-direction-c-candidate-v1.png` | 2048 × 768 | 1,946,738 B | `E9F75AABACA46D8395A1B9CC5C998B469EFC55791EEBA7A08926320A96071EAD` | conceptual family visual | Technical visualization of antenna systems and engineering studies in a blue outdoor landscape. | Do not present as an exact depiction of a specific USAP installation, measured test range, or proprietary USAP design. | Replace if client provides approved high-resolution hero photography. |
| Log Periodic Antennas Card | `images/products/usap-family-card-log-periodic-family-visual-v1.png` | 1600 × 1000 | 1,387,964 B | `03B191AB25844A40C0FD5B8045FFD264327734BB265BA5DFFCF3305EA0A9CF7F` | family-level conceptual/reference-grounded visual | Log periodic antenna array in an outdoor installation. | Do not identify as a photograph of LP-1017 or any specific named USAP model (LP-1001, LP-1002, LP-1005, LP-1018BA, LP-1019, LP-1112MR). | Replace when USAP provides approved product photography. |
| Portable & Transportable Card | `images/products/usap-family-card-portable-transportable-placeholder-v1.png` | 1600 × 1000 | 1,773,360 B | `DDF0E92CC340520A441162607911BAA755A74610DB5087090C437A966A30E947` | conceptual family placeholder | Portable antenna system deployed on a guyed field mast. | Do not identify as V-4213, LP-1402, LP-1403, or 1910 hardware. | Replace when USAP provides approved product photography. |
| Aperiodic Loop Antennas Card | `images/products/usap-family-card-aperiodic-loop-element-v1.png` | 1600 × 1000 | 2,098,672 B | `9CF71EA8DF4DDD1126FFA8F5C80D64304647EF2B82F21517553C9C945398E03F` | reference-grounded single-element visualization | Aperiodic loop antenna element on a transportable tripod. | Do not identify as a complete multi-element array installation or a specific named model. | Replace when USAP provides approved product photography. |
| NVIS Antennas Card | `images/products/usap-family-card-nvis-1942-family-visual-v1.png` | 1600 × 1000 | 2,102,661 B | `839A933F73EA1B3F77FE6B2459342F36A5B9211BAE45F7C459D677E0209ACDA0` | 1942-family-level reference-grounded visualization | Field-deployed NVIS antenna system with a central mast and broad wire footprint. | Do not claim depiction of exact 1942-RT, 1942-TA, or 1942-GM field hardware. | Replace when USAP provides approved product photography. |
| Antenna Rotator & Control Systems Card | `images/products/usap-family-card-rotator-control-lower-risk-placeholder-v2.png` | 1600 × 1000 | 1,888,491 B | `5B0C8F2B71EF2CFEEF7ED81D091A79329B482AA23345B5616F75663C6378B682` | reference-grounded rotator with illustrative controller study | Heavy-duty antenna rotator with a translucent tabletop controller study. | Do not present as DRC-3, DRC-4, R3500, R3501, or R3503 production hardware, or imply a confirmed bundle/pairing. | Replace when USAP provides approved product photography. |
| Tower Systems & Accessories Card | `images/products/usap-family-card-tower-systems-nonconfigurational-fallback-v2.png` | 1600 × 1000 | 2,006,234 B | `32A2CAC6B2DC9B6227D21295801CCEF3EDF1E2DBC8D562419DA02FCE565C09C8` | design fallback — non-configurational family study | Technical study of tower, mast, rotation, feedline, and installation components. | Do not represent as T-3002 or specific 3002FA/FB/SS hardware configuration. | Replace when USAP provides approved product photography. |

### Candidate Selection Rationale & Alternatives Retained Exclusively in Archive

1. **Products Landing Hero:** Direction C (`usap-products-landing-hero-direction-c-candidate-v1.png`) was selected because its left ~35% provides a clean, deep-navy gradient zone with high text contrast for the hero heading, eyebrow, and introductory text, while the right ~65% displays an intricate antenna array and technical blueprint background. Direction A and Direction B placed high-contrast antenna geometry across the left third, degrading text readability.
2. **Antenna Rotator & Control Systems:** Candidate B (`usap-family-card-rotator-control-lower-risk-placeholder-v2.png`) was selected as a lower-risk conceptual indicator. Candidate A was rejected because it depicted specific, unverified rotator chassis features. Candidate C was rejected due to excessive abstract minimalism.
3. **Tower Systems & Accessories:** Candidate B (`usap-family-card-tower-systems-nonconfigurational-fallback-v2.png`) was selected as a generalized structural mast/tower illustration. Candidate A was rejected because it purported to depict a specific 3002SS hardware configuration that has not been confirmed by client engineering.
4. **Archived-Only Assets:** Direction A, Direction B, Rotator Candidates A and C, Tower Candidate A, all comparison sheets, and internal review derivatives remain strictly within the intake ZIP archive and must never be served or tracked in `wwwroot`.

### Promoted Assets from A3 and A3S Archives (Checkpoint USAP-CATALOG-001-C2)

Under Checkpoint USAP-CATALOG-001-C2, 16 approved asset derivatives were promoted into `src/USAP.Web/wwwroot/images/products/` from the verified `project-input/USAP_Product_Catalog_A3_Family_Hero_Review_Package_2026-09-15.zip` and `project-input/USAP_Product_Catalog_A3S_Production_Quality_Regeneration_2026-09-16.zip` source archives.

Every promoted file was copied without re-encoding, preserving exact byte counts and SHA-256 hashes. Domain records in `ProductCatalogService.cs` assign explicit asset classifications without generic defaults, and all promoted assets carry the provisional approval status `provisional — USAP review pending`.

#### Complete C2 Promoted Asset Register & Traceability

| Role / Target Path in `src/USAP.Web/wwwroot/` | Source Archive Path | Dimensions | Size (Bytes) | SHA-256 Checksum | Explicit Classification | Approved Alt Text | Approval Status |
|---|---|---|---|---|---|---|---|
| `images/products/usap-family-card-rotator-control-r3500-drc4-a3s-recommended-v1.png` | A3S `usap-product-catalog-a3s/family-cards/usap-family-card-rotator-control-r3500-drc4-a3s-recommended-v1.png` | 1600 × 1000 | 2,551,119 | `AC7C826572FE585C409036C4D0566229B948684B3858D383883C9C1C5A804D76` | source-guided photorealistic product visualization | Heavy-duty antenna rotator with a digital rotator controller on an engineering bench. | provisional — USAP review pending |
| `images/products/usap-family-log-periodic-antennas-hero-candidate-a-v2-desktop-preview.png` | A3 `usap-product-catalog-a3/log-periodic-antennas/desktop/usap-family-log-periodic-antennas-hero-candidate-a-v2-desktop-preview.png` | 1536 × 576 | 893,358 | `0BA2204D408A4D8B2E437846942D1F5EDC90F1B39EDD55F166003632DBF5355E` | conceptual/reference-grounded family visual | High-power HF log periodic antenna installation in an open field setting. | provisional — USAP review pending |
| `images/products/usap-family-log-periodic-antennas-hero-candidate-a-v2-mobile-crop.png` | A3 `usap-product-catalog-a3/log-periodic-antennas/mobile/usap-family-log-periodic-antennas-hero-candidate-a-v2-mobile-crop.png` | 768 × 768 | 638,090 | `D89517E347B2D5C9461AF4C095FAF858A5E42A59095AED8E2B0632FF251688D0` | conceptual/reference-grounded family visual | Close-up view of log periodic antenna elements and boom structure. | provisional — USAP review pending |
| `images/products/usap-family-portable-transportable-hero-a3s-desktop-1536x576.png` | A3S `usap-product-catalog-a3s/family-heroes/portable-transportable-antennas/desktop/usap-family-portable-transportable-hero-a3s-desktop-1536x576.png` | 1536 × 576 | 1,072,125 | `73F8CC6FD728EA4742314FC6047CF4749C7D2C2F67CCA96BABF2D6D1B98910A8` | source-guided photorealistic product visualization | Portable and transportable tactical antenna system deployed in a field environment. | provisional — USAP review pending |
| `images/products/usap-family-portable-transportable-hero-a3s-mobile-768x768.png` | A3S `usap-product-catalog-a3s/family-heroes/portable-transportable-antennas/mobile/usap-family-portable-transportable-hero-a3s-mobile-768x768.png` | 768 × 768 | 766,967 | `A6A2E594B0138403B6E67520910753DBEE7A0AF2409CC1F3A978AA49A39E214C` | source-guided photorealistic product visualization | Tactical portable antenna mast and hub assembly in a field setting. | provisional — USAP review pending |
| `images/products/usap-family-aperiodic-loop-antennas-hero-candidate-a-v1-desktop-preview.png` | A3 `usap-product-catalog-a3/aperiodic-loop-antennas/desktop/usap-family-aperiodic-loop-antennas-hero-candidate-a-v1-desktop-preview.png` | 1536 × 576 | 1,067,436 | `1D131D8425D14C7153BD5DE3E337BD5BFF72F97E2CE362019C37FBFD703F4B5D` | reference-grounded single-element visualization | Aperiodic loop receiving antenna element deployed in a quiet monitoring environment. | provisional — USAP review pending |
| `images/products/usap-family-aperiodic-loop-antennas-hero-candidate-a-v1-mobile-crop.png` | A3 `usap-product-catalog-a3/aperiodic-loop-antennas/mobile/usap-family-aperiodic-loop-antennas-hero-candidate-a-v1-mobile-crop.png` | 768 × 768 | 844,502 | `DE6DA702204DEF2B49FEE0B020A575C9D6F5A14E72A74684C2D6C4DBFE9F74C8` | reference-grounded single-element visualization | Compact aperiodic loop antenna element mounted on a portable support. | provisional — USAP review pending |
| `images/products/usap-family-nvis-antennas-hero-candidate-a-v2-desktop-preview.png` | A3 `usap-product-catalog-a3/nvis-antennas/desktop/usap-family-nvis-antennas-hero-candidate-a-v2-desktop-preview.png` | 1536 × 576 | 1,078,517 | `2F3A924F959CB0107BCEA10A0A4A8757E18B98A12F39B2DA678703AC39CA9A60` | 1942-family-level reference-grounded visualization | 1942 series NVIS antenna installation configured for regional high-angle HF communications. | provisional — USAP review pending |
| `images/products/usap-family-nvis-antennas-hero-candidate-a-v2-mobile-crop.png` | A3 `usap-product-catalog-a3/nvis-antennas/mobile/usap-family-nvis-antennas-hero-candidate-a-v2-mobile-crop.png` | 768 × 768 | 812,182 | `98AC134C2904596F7C452EC1EBE42015526237F95F14F3170B7D53E69DBA0F6B` | 1942-family-level reference-grounded visualization | Central mast and radiator feedpoint geometry of an NVIS antenna system. | provisional — USAP review pending |
| `images/products/usap-family-rotator-control-hero-a3s-desktop-1536x576.png` | A3S `usap-product-catalog-a3s/family-heroes/rotator-control-systems/desktop/usap-family-rotator-control-hero-a3s-desktop-1536x576.png` | 1536 × 576 | 1,215,960 | `17E35BACC4233BBA412CA82E3424DD8F3967D8A8BF92EAE6A2494E9D2F778C2C` | source-guided photorealistic product visualization | Heavy-duty antenna rotator unit and rack-mount digital control equipment. | provisional — USAP review pending |
| `images/products/usap-family-rotator-control-systems-hero-a3s-mobile-768x768.png` | A3S `usap-product-catalog-a3s/family-heroes/rotator-control-systems/mobile/usap-family-rotator-control-systems-hero-a3s-mobile-768x768.png` | 768 × 768 | 943,383 | `2A6584442A38D9305E7FE24D2524A253371169A926AC2CC3A39CB1FB5380DF85` | source-guided photorealistic product visualization | Digital rotator controller unit front panel and controls. | provisional — USAP review pending |
| `images/products/usap-family-tower-systems-hero-a3s-desktop-1536x576.png` | A3S `usap-product-catalog-a3s/family-heroes/tower-systems-accessories/desktop/usap-family-tower-systems-hero-a3s-desktop-1536x576.png` | 1536 × 576 | 1,110,766 | `2A38F547A2438D014796F92F90C38CA1BF503265309FB8CC941AAFE4E78497CB` | source-guided photorealistic product visualization | Commercial antenna tower installation with rotatable LP array and feedline integration. | provisional — USAP review pending |
| `images/products/usap-family-tower-systems-hero-a3s-mobile-768x768.png` | A3S `usap-product-catalog-a3s/family-heroes/tower-systems-accessories/mobile/usap-family-tower-systems-hero-a3s-mobile-768x768.png` | 768 × 768 | 858,515 | `AD3C05CF8C196CEF33493157B76307FE3FE5F158DC4E82606F3F9BB67F3354D6` | source-guided photorealistic product visualization | Tower mast lattice structure and rotational hardware interface. | provisional — USAP review pending |
| `images/products/usap-r3500-drc4-source-guided-relationship-a3s-v1.png` | A3S `usap-product-catalog-a3s/product-visuals/rotator-control/usap-r3500-drc4-source-guided-relationship-a3s-v1.png` | 1600 × 1000 | 2,286,839 | `3EA85E13ED17635D9773EEF34D8DCC828F4ECB61F131241B48B5E4A46585708A` | source-guided photorealistic product visualization | R3500 heavy-duty rotator unit with paired DRC-4 digital control unit on an engineering bench. | provisional — USAP review pending |
| `images/products/usap-drc3-source-guided-product-visual-a3s-v1.png` | A3S `usap-product-catalog-a3s/product-visuals/rotator-control/usap-drc3-source-guided-product-visual-a3s-v1.png` | 1600 × 1000 | 1,966,126 | `F78F5720EF3E061F935FED86C0C61002EC570FBB22539C31DC3B294AF7D2B47B` | source-guided photorealistic product visualization | DRC-3 digital rotator controller front panel with LED position readout and tactile controls. | provisional — USAP review pending |
| `images/products/usap-drc4-source-guided-product-visual-a3s-v1.png` | A3S `usap-product-catalog-a3s/product-visuals/rotator-control/usap-drc4-source-guided-product-visual-a3s-v1.png` | 1600 × 1000 | 2,051,887 | `4F3636E7F8409D8C8D20ACAFB561CC0837B0A887888D2E722AB8262D6497D6A5` | source-guided photorealistic product visualization | DRC-4 digital rotator controller front panel with graphical display and precision controls. | provisional — USAP review pending |

#### Selection Decisions & Rejections

1. **Rotator & Control Family Card:** Candidate B (`usap-family-card-rotator-control-r3500-drc4-a3s-recommended-v1.png`) is promoted into service. Candidate A is strictly rejected because it purports to depict unverified rotator internal mechanics.
2. **Family Hero Composition:** Breakpoint-sensitive right-anchored contained image (~65% desktop, ~75% tablet) blended with left-anchored navy gradient overlay. Renders 1536×576 desktop and 768×768 mobile crop via semantic `<picture>` with `(max-width: 47.999rem)` boundary. Zero cropping or white letterboxing across all viewports.
3. **Dedicated Card Partials (16:10 Aspect Ratio):** `_HomeProductFamilyCard.cshtml` and `_ProductFamilyCard.cshtml` preserve approved 16:10 media presentation, single keyboard Tab stop, and `.stretched-link` activation.
4. **Relocated Rotator Product Visuals (C2R):** The 3 product-specific visuals (DRC-3, DRC-4, and R3500/DRC-4 relationship) are integrated directly into their respective group disclosures via the non-destructive `AssociatedAsset` property on `ProductGroupRecord`, eliminating the standalone visual section.
5. **Strict Terminology Compliance:**
   - DRC-3: "Large industrial antenna-rotator control enclosure with display and control components." (Prohibits tabletop, wall-mounted, cabinet-mounted, mast-mounted, and defense claims).
   - DRC-4: "Tabletop antenna-rotator controller with digital display, rotary dial, and front controls." (Prohibits rackmount, rack-mount, 19-inch, and unverified precision claims).
   - R3500/DRC-4: "Heavy-duty R3500 rotator and tabletop DRC-4 controller shown together in a source-guided technical visualization." (No bundle or availability guarantee).
6. **DRC-4 Controller Face Branding Limitation:** The horizontal controller-face wordmark depicted on the DRC-4 front panel is source-derived and provisional. It is not an approved official alternate logo lockup and remains slated for replacement when official vector or high-resolution brand artwork is supplied by USAP.
7. **Technical Document Publication Policy (Interim C1 Link Set vs. Migration):** The 6 technical documents linked on product pages (`doc-lp-high-power`, `doc-lp-1018ba`, `doc-lp-1019`, `doc-1910-2024`, `doc-aperiodic`, `doc-t-3002-oct2016`) constitute the current interim C1 product-page link set (`InterimResourceIds`); they do not represent a permanent authorization ceiling. Under the R2 technical documentation review, 17 canonical technical documents were selected for subsequent migration and local hosting prior to WordPress decommissioning. The remaining canonical documents outside C1 are not rejected merely because they are outside the interim C1 release.

### Complete Product-Group Visual Coverage Matrix (16 Product Groups — C1 Finalized)

All 16 product groups feature intentional visual coverage in C1. The project lead selected all 13 A1 product-group candidates and 3 coordinated Engineering Guidance card visuals for provisional implementation. Asset selection represents project-lead authorization for implementation and does not represent formal USAP approval.

**Responsive Visual Treatments:**
- **Standard cover layout:** Deployed across groups where subject remains accurately recognizable at full crop.
- **Wide-span contain layout:** Deployed for `lp-1112mr`, `1910`, and `1942` (`.product-group-disclosure__figure--contain`, `.product-group-disclosure__img--contain`) to preserve the central antenna system identity and full footprint without aggressive cropping.
- **Engineering Guidance visuals:** 3 coordinated card images deployed on `/products` (Deployment & Mobility, Coverage & Propagation, Positioning & Infrastructure). The combined wide guidance visual alternate remains deferred.

| Family | Group ID | Product Group Name | Associated Asset File | Dimensions & SHA-256 | Approved Safe Alt Text | Selection Status |
|---|---|---|---|---|---|---|
| Log Periodic | `lp-high-power` | High Power Log Periodic Antennas | `images/products/groups/usap-product-group-lp-high-power-source-guided-candidate-a1-v1.png` | 1600 × 1000 PNG<br>`699e6c992e498051929f4fe9fe7205f1b12f0a71e8a39af331216788f29be0ab` | Long-boom high-power log-periodic antenna array in an outdoor installation. | Project-lead selected; USAP approval pending |
| Log Periodic | `lp-1017` | LP-1017 Series Log Periodic Antennas | `images/products/groups/usap-product-group-lp-1017-source-guided-candidate-a1-v1.png` | 1600 × 1000 PNG<br>`c3f542d458e4c3525b90263813e4700363f2a90009a90b003228bfc5c5aae26e` | Tower-mounted long-boom log-periodic antenna array. | Project-lead selected; USAP approval pending |
| Log Periodic | `lp-1018ba` | LP-1018BA Directional Log Periodic Antenna | `images/products/groups/usap-product-group-lp-1018ba-source-guided-candidate-a1-v1.png` | 1600 × 1000 PNG<br>`997ffdf51065b6a8104385c50f0fb04a5b569e3a69b88867c6845c133f73b5e6` | Broadband log-periodic antenna array mounted on a mast. | Project-lead selected; USAP approval pending |
| Log Periodic | `lp-1019` | LP-1019 Series Log Periodic Antennas | `images/products/groups/usap-product-group-lp-1019-source-guided-candidate-a1-v2.png` | 1600 × 1000 PNG<br>`c37b7c40e42e203f6cc01172816772d67db1ab3f3c44d5973a719cda118cb790` | Compact log-periodic antenna array mounted on a mast. | Project-lead selected corrected v2; USAP approval pending |
| Log Periodic | `lp-1112mr` | LP-1112MR Tactical Log Periodic Antenna | `images/products/groups/usap-product-group-lp-1112mr-source-guided-candidate-a1-v2.png` | 1600 × 1000 PNG<br>`baf591379f8c48c36ae0b485534e4eb095d19e33d92c71a4866aa04bf76c30af` | Wide transportable log-periodic antenna system deployed on a central field mast. | Project-lead selected corrected v2 (contain mode); USAP approval pending |
| Portable | `v-4213` | V-4213 Portable Discone Antenna System | `images/products/groups/usap-product-group-v-4213-source-guided-candidate-a1-v1.png` | 1600 × 1000 PNG<br>`68e5f91946ff0be0b375b76c7bce7c3b90bfa43cb0365b950786b5d6490ee6f1` | Portable discone antenna system deployed on a sectional field mast. | Project-lead selected; USAP approval pending |
| Portable | `lp-1402-1403` | LP-1402 & LP-1403 Tactical Mast-Mounted Antennas | `images/products/groups/usap-product-group-lp-1402-1403-source-guided-candidate-a1-v1.png` | 1600 × 1000 PNG<br>`e8a19d6f2ed72b2e912ba3e44f872647ba33b481e21c041f9820f6fba05ea694` | Transportable mast-mounted log-periodic antenna system in a field setting. | Project-lead selected; USAP approval pending |
| Portable | `1910` | 1910 Transportable Log Periodic Antenna System | `images/products/groups/usap-product-group-1910-source-guided-candidate-a1-v1.png` | 1600 × 1000 PNG<br>`001f8ed06a63c946f0ec8cc21ba58a948eca7a9187bc714b244c1c412ac565f1` | Field-deployed HF wire dipole system supported by a central mast. | Project-lead selected (contain mode); USAP approval pending |
| Aperiodic | `aperiodic` | Aperiodic Loop Antenna Systems | `images/products/groups/usap-product-group-aperiodic-single-loop-source-guided-candidate-a1-v1.png` | 1600 × 1000 PNG<br>`aad1a11581fd6791cdac37701d9e5fb661d535afe3ca26d1a469a91466cd3334` | One aperiodic loop and preamplifier element on a transportable tripod. | Project-lead selected; USAP approval pending |
| NVIS | `1942` | 1942 Series NVIS Antenna System | `images/products/groups/usap-product-group-1942-source-guided-candidate-a1-v1.png` | 1600 × 1000 PNG<br>`96a6769f7d6adaaf5486f1f15fa9c6e3bd9a3bc65bd973f674b02ded076cf908` | Field-deployed NVIS antenna system with a central mast and broad low wire footprint. | Project-lead selected (contain mode); USAP approval pending |
| Rotator | `r3500` | R3500 Heavy-Duty Antenna Rotator Series | `images/products/usap-r3500-drc4-source-guided-relationship-a3s-v1.png` | 1600 × 1000 PNG<br>`3ea85e13ed17635d9773eef34d8dcc828f4ecb61f131241b48b5e4a46585708a` | Heavy-duty antenna rotator and tabletop controller shown together in a technical studio scene. | Approved for provisional use |
| Rotator | `r3501` | R3501 Universal Rotator System | `images/products/groups/usap-product-group-r3501-source-guided-candidate-a1-v1.png` | 1600 × 1000 PNG<br>`a8b160992914ed7baaa15c8e5b831db743d3eb579cfa1599d3cdc2049a614a04` | Source-guided visualization of an R3501 open-frame antenna rotator assembly. | Project-lead selected; USAP approval pending |
| Rotator | `r3503` | R3503 Heavy Duty Rotating System | `images/products/groups/usap-product-group-r3503-source-guided-candidate-a1-v1.png` | 1600 × 1000 PNG<br>`8f7804c2c074ee001cf0e9482a4b6a28781ae1a0b4ce3a04fb9c2ed92ad16dbe` | Source-guided visualization of an R3503 heavy-duty open-frame antenna rotator assembly. | Project-lead selected; USAP approval pending |
| Rotator | `drc-3` | DRC-3 Industrial Digital Rotator Controller | `images/products/usap-drc3-source-guided-product-visual-a3s-v1.png` | 1600 × 1000 PNG<br>`44047a064106567fe7bc02146e25785a9a8385077227d8cece4d989f6b9bcfa4` | Industrial antenna-rotator control enclosure with display and front-panel controls. | Approved for provisional use |
| Rotator | `drc-4` | DRC-4 Tabletop Digital Rotator Controller | `images/products/usap-drc4-source-guided-product-visual-a3s-v1.png` | 1600 × 1000 PNG<br>`5b306b6b7da255a297746595ee5d92e59e2eb420313cf7f7ae8cae3ef816a7bb` | Horizontal tabletop antenna-rotator controller with digital display, rotary dial, and front controls. | Approved for provisional use |
| Tower | `t-3002` | T-3002 RLPA Tower System | `images/products/groups/usap-product-group-t-3002-source-guided-candidate-a1-v1.png` | 1600 × 1000 PNG<br>`515ad9d510996b8301d9cdefc109f1bcd12048807fcf389eda0130cae74cbaa5` | Tower-system components including lattice supports, a rotating mast, and a directional antenna. | Project-lead selected; USAP approval pending |

### Safe Public Scope & Content Reconciliation (USAP-CATALOG-002-C1 Finalized)

- **Model & Identifier Reconciliation (30 + 1 Target Achieved):**
  - Public catalog projection contains exactly 30 published named models across 15 groups, plus 1 unnamed Aperiodic loop system record (`HasPublishedModelNumber = false`). Total: 31 catalog records across 16 groups.
  - All 6 current-site 1942 NVIS configurations are public: `1942-RT`, `1942-TA`, `1942-GM`, `1942-RT-LP`, `1942-TA-LP`, `1942-GM-LP`. The three low-power variants are identified conservatively by configuration role only, omitting unverified power, weight, gain, or coverage ratings.
  - R3500 presentation strictly uses non-conflicting mechanical rotator characteristics, omitting unverified bundle, pairing, or commercial availability claims.
- **Visual Asset Deployment Status:**
  - Complete 16-group visual coverage deployed with verified SHA-256 hashes and dimensions.
  - 3 coordinated Engineering Guidance card visuals deployed on `/products`. The combined wide visual alternate remains deferred.
- **Interim Technical Documents vs. Future Technical Documentation Migration:**
  - 6 interim technical resource links are currently wired into the product pages:
    - `doc-lp-high-power`: `1001_1002_1005-data-sheet.pdf`
    - `doc-lp-1018ba`: `LP_1018BA_data_sheet_revised.pdf`
    - `doc-lp-1019`: `1019-and-1019-ss-data-sheet.pdf`
    - `doc-1910-2024`: `1910AA_1910BA-revised-1.pdf`
    - `doc-aperiodic`: `USAP-Aperiodic-Loop-Antenna-data-sheet.pdf`
    - `doc-t-3002-oct2016`: `T3002_data_sheet.pdf`
  - R2 identified 34 current PDF URL records representing 32 unique binaries and selected 17 canonical PDFs for eventual local migration under `USAP-TECHDOC-001`. The interim 6-link set does not represent an authorization ceiling or a rejection of the remaining 11 canonical documents. Public vs. lead-gated distribution status for all technical documents remains client-pending.
- **Purged Governance and Unsupported Terms:**
  - Prohibited governance tokens (`ApprovalStatus`, `PublicationRecommendation`, `SpecificationStatus`, `ConflictHolds`, `SourceNotes`, `SourcePresence`, `CommercialAvailability`, `ClientConfirmation`, `HoldDisputedSpecs`, `confidence score`, `provisional research record`) are excluded from public output.
  - Prohibited unverified claims purged: "without skip zones in mountainous terrain", "gap-free", "no-skip-zone", "guaranteed coverage", "complete 30-foot mast field system", "rapid-deployment", "400 W PEP", "compatible with the DRC-3", "paired with the DRC-4", "rackmount", "19-inch", and unconfirmed torque/weight figures.

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
- [x] Approved PDF/data sheet set with category classification and distribution restrictions: Reconciled and migrated under `USAP-TECHDOC-001` (17 canonical PDFs, direct public access, 0 WordPress dependencies).

---

## Canonical Technical Documents Register (USAP-TECHDOC-001)

Exactly 17 first-party canonical PDF binaries are deployed locally under `src/USAP.Web/wwwroot/documents/technical/`. All 17 files are verified byte-for-byte and hash-for-hash against the R2 manifest.

| Stable ID | Document Title | Product Family | Models Covered | Type | Pages | Local Path | Verified SHA-256 |
|---|---|---|---|---|---:|---|---|
| `DOC-LP-HIGH-POWER` | LP-1001, LP-1002 and LP-1005 High-Power HF Log Periodic Antennas | Log Periodic Antennas | LP-1001, LP-1002, LP-1005 | Data sheet | 2 | `/technical-resources/documents/log-periodic-antennas/usap-lp-1001-lp-1002-lp-1005-data-sheet.pdf` | `dc3c05e4195b1c465a120a16e5d01cbf0adf183efeff97cdfe8791b8cb48bdc6` |
| `DOC-LP-1017` | LP-1017 Commercial HF Log Periodic Antenna | Log Periodic Antennas | LP-1017 | Data sheet | 2 | `/technical-resources/documents/log-periodic-antennas/usap-lp-1017-data-sheet.pdf` | `bd53cbecb83d07633ee680bb1c9e28280395ef1eaa3093af08fcf039cc0faa25` |
| `DOC-LP-1018BA` | LP-1018BA Broadband Log Periodic Antenna | Log Periodic Antennas | LP-1018BA | Data sheet | 2 | `/technical-resources/documents/log-periodic-antennas/usap-lp-1018ba-data-sheet.pdf` | `f2bde447af31610e4b6468103752c385dbc0b8411862f638f3de163449d855d8` |
| `DOC-LP-1019` | LP-1019BA and LP-1019SS Log Periodic Antennas | Log Periodic Antennas | LP-1019BA, LP-1019SS | Data sheet | 2 | `/technical-resources/documents/log-periodic-antennas/usap-lp-1019-series-data-sheet.pdf` | `ef7b63deca466633d9dbd4ea4701f62eb3463dfcb59e7ba2e410fc5527e809d7` |
| `DOC-LP-1112MR` | LP-1112MR Transportable Log Periodic Antenna | Log Periodic Antennas | LP-1112MR | Data sheet | 4 | `/technical-resources/documents/log-periodic-antennas/usap-lp-1112mr-data-sheet.pdf` | `34941809f48c79a9c4b9cf06d855322c14332cc29b1b751f6248bfefc1c373c7` |
| `DOC-V4213-OVERVIEW` | V-4213 Portable Discone Antenna System Overview | Portable & Transportable Antenna Systems | V-4213AD, V-4213AC | Product overview | 2 | `/technical-resources/documents/portable-transportable-antennas/usap-v4213-portable-discone-overview.pdf` | `9f9a8f6cd977ea26506606892f3e238b3f5e5b8d14241e460d854997ce4d5435` |
| `DOC-V4213-CONFIG` | V-4213AD and V-4213AC Configuration Data | Portable & Transportable Antenna Systems | V-4213AD, V-4213AC | Data sheet | 3 | `/technical-resources/documents/portable-transportable-antennas/usap-v4213ad-v4213ac-data-sheet.pdf` | `4dc60b97ee758bf0f15689edb5c19c9807de25dbf14736c0522b4079fb45856f` |
| `DOC-LP-1402-1403` | LP-1402 and LP-1403 Transportable Log Periodic Antennas | Portable & Transportable Antenna Systems | LP-1402, LP-1403 | Data sheet | 2 | `/technical-resources/documents/portable-transportable-antennas/usap-lp-1402-lp-1403-data-sheet.pdf` | `ef5d461276f562ee309fb53f54c41d8f6f792889feffe7415869e4bb28dfa4e1` |
| `DOC-1910` | 1910AA and 1910BA Broadband HF Dipole Antennas | Portable & Transportable Antenna Systems | 1910AA, 1910BA | Data sheet | 2 | `/technical-resources/documents/portable-transportable-antennas/usap-1910aa-1910ba-data-sheet.pdf` | `302b5e704a07d257965c9ec29332311b41633d97cd9675c6eb98f0a8bd9e8c2c` |
| `DOC-APERIODIC` | USAP Aperiodic Loop Antenna | Aperiodic Loop Antennas | None published | Technical overview | 2 | `/technical-resources/documents/aperiodic-loop-antennas/usap-aperiodic-loop-antenna-data-sheet.pdf` | `130622310670dd102bcdf1fc9ee0196efb9d04055d87731c8440d40f57e88e17` |
| `DOC-1942` | 1942 NVIS Antenna Systems | NVIS Antennas | 1942-RT, 1942-TA, 1942-GM, 1942-RT-LP, 1942-TA-LP, 1942-GM-LP | Data sheet | 2 | `/technical-resources/documents/nvis-antennas/usap-1942-nvis-series-data-sheet.pdf` | `ae1fdc8f5645116488e8f5d517b14e9fe8ece75bc0f22c2f131763bac0b29ede` |
| `DOC-R3500` | R3500 Heavy-Duty Antenna Rotator | Antenna Rotator & Control Systems | R3500 | Data sheet | 2 | `/technical-resources/documents/antenna-rotator-control-systems/usap-r3500-rotator-data-sheet.pdf` | `a1918ed9bc544dfb645aeee0d05fddc1dfb016b26b1cf6c3510a06ea7e13bfee` |
| `DOC-R3501` | R3501 Universal Antenna Rotator System | Antenna Rotator & Control Systems | R3501 | Data sheet | 1 | `/technical-resources/documents/antenna-rotator-control-systems/usap-r3501-rotator-data-sheet.pdf` | `a908b7f06a924c329ab8de328ef846912257d02e1213ef9d90719399b911051e` |
| `DOC-R3503` | R3503 Heavy-Duty Rotating System | Antenna Rotator & Control Systems | R3503 | Data sheet | 1 | `/technical-resources/documents/antenna-rotator-control-systems/usap-r3503-rotator-data-sheet.pdf` | `69b6a0916a47bbe5937ab2b96360688f08bb8a6c27ddc42dd6e49a4c2b9a8142` |
| `DOC-DRC3` | DRC-3 Digital Rotator Controller | Antenna Rotator & Control Systems | DRC-3 | Data sheet | 3 | `/technical-resources/documents/antenna-rotator-control-systems/usap-drc-3-controller-data-sheet.pdf` | `6e500af890467c7cf92e2b7885a1aaadaf1cb13aa7608abc3a2f4f5652ca7e24` |
| `DOC-DRC4` | DRC-4 Digital Rotator Controller | Antenna Rotator & Control Systems | DRC-4 | Data sheet | 2 | `/technical-resources/documents/antenna-rotator-control-systems/usap-drc-4-controller-data-sheet.pdf` | `7fd777bd7321c604040f99d3f0c5c58c7ac206127f6940ed81e7c4f431de1219` |
| `DOC-T3002` | T-3002 RLPA Tower System | Tower Systems & Accessories | T-3002, 3002FA, 3002FB, 3002SS, 3002SS-80 | Data sheet | 3 | `/technical-resources/documents/tower-systems-accessories/usap-t-3002-tower-system-data-sheet.pdf` | `d94cb65015149a2c5e58540f22f1fb5c0e4c952a2d371333248c16c88f4399a9` |

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
