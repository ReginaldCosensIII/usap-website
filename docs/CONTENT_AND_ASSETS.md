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
| Product family detail | `/products/{familySlug}` | 🟡 Minimal route placeholders with shared hero, breadcrumbs, and audited group listing implemented (Milestone C1). True 404 for invalid slugs. Full specifications and datasheets deferred to C2. |
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
7. **Technical Document Publication Policy (C2R):** Publication is default-deny. Only 6 provisional/preferred provisional records are exposed: `doc-lp-high-power`, `doc-lp-1018ba`, `doc-lp-1019`, `doc-1910-2024`, `doc-aperiodic`, `doc-t-3002-oct2016`. All conflicting or candidate-only documents remain withheld. Current first-party PDF links remain a provisional dependency requiring migration prior to WordPress decommissioning.

### Deferred Individual-Product Asset Matrix (16 Product Groups — C2R1)

The individual-product asset strategy cleanly isolates deployed provisional assets from deferred future research and generation. Exactly three Rotator groups currently have associated visuals deployed; the remaining 13 product groups require a dedicated, separate asset-research and generation checkpoint.

**Optional Imagery Architecture:** Disclosures containing an associated asset (`AssociatedAsset != null`) render the `<figure>` with responsive media; disclosures without an associated asset render a clean text-first layout with zero blank media containers, zero generic silhouettes, and zero placeholder frames. No backlog or governance terminology is exposed in public page markup.

| Family | Group ID | Product Group Name | Associated Asset Currently Available | First-Party PDF / Public Source for Future Research | Asset Review & Generation Status |
|---|---|---|---|---|---|
| Log Periodic Antennas | `lp-high-power` | High Power Log Periodic Antennas | None | `LP-High-Power.pdf` | Separate asset-research and generation checkpoint required |
| Log Periodic Antennas | `lp-1017` | LP-1017 Series Log Periodic Antennas | None | `LP-1017.pdf` | Separate asset-research and generation checkpoint required |
| Log Periodic Antennas | `lp-1018ba` | LP-1018BA Directional Log Periodic Antenna | None | `LP-1018BA.pdf` | Separate asset-research and generation checkpoint required |
| Log Periodic Antennas | `lp-1019` | LP-1019 Series Log Periodic Antennas | None | `LP-1019.pdf` | Separate asset-research and generation checkpoint required |
| Log Periodic Antennas | `lp-1112mr` | LP-1112MR Tactical Log Periodic Antenna | None | `LP-1112MR.pdf`, current public site visual | Separate asset-research and generation checkpoint required |
| Portable & Transportable | `v-4213` | V-4213 Portable Discone Antenna System | None | `V-4213.pdf` | Separate asset-research and generation checkpoint required |
| Portable & Transportable | `lp-1402-1403` | LP-1402 & LP-1403 Tactical Mast-Mounted Antennas | None | `LP-1402-1403.pdf` | Separate asset-research and generation checkpoint required |
| Portable & Transportable | `1910` | 1910 Transportable Log Periodic Antenna System | None | `1910-OCT-2016.pdf`, `1910-OCT-2024.pdf` | Separate asset-research and generation checkpoint required |
| Aperiodic Loop Antennas | `aperiodic` | Aperiodic Loop Antenna Systems | None | `Aperiodic-Loop.pdf` | Separate asset-research and generation checkpoint required |
| NVIS Antennas | `1942` | 1942 Series NVIS Antenna System | None | `1942-NVIS.pdf` | Separate asset-research and generation checkpoint required |
| Rotator & Control Systems | `r3500` | R3500 Heavy-Duty Antenna Rotator Series | `/images/products/usap-rotator-hardware-r3500-drc4-bench-source-guided-v1.png` | First-party rotator catalog & engineering datasheets | Available (source-guided technical visualization deployed) |
| Rotator & Control Systems | `r3501` | R3501 Medium-Duty Antenna Rotator | None | First-party rotator catalog & engineering specs | Separate asset-research and generation checkpoint required |
| Rotator & Control Systems | `r3503` | R3503 Elevation Rotator | None | First-party rotator catalog & engineering specs | Separate asset-research and generation checkpoint required |
| Rotator & Control Systems | `drc-3` | DRC-3 Industrial Digital Rotator Controller | `/images/products/usap-rotator-controller-drc3-large-enclosure-candidate-a-v1.png` | First-party DRC-3 manual & enclosure photo | Available (source-guided technical visualization deployed) |
| Rotator & Control Systems | `drc-4` | DRC-4 Tabletop Digital Rotator Controller | `/images/products/usap-rotator-controller-drc4-tabletop-front-detail-v1.png` | First-party DRC-4 datasheet & manual | Available (source-guided technical visualization deployed) |
| Tower Systems & Accessories | `t-3002` | T-3002 Heavy Duty Tower Systems & Accessories | None | `T-3002-OCT2016.pdf` | Separate asset-research and generation checkpoint required |

### Current-Site Catalog Policy Alignment & Metadata Separation (QA2 / C2)

- **Binding Authority:** Until USAP provides a controlled product list, the current public USAP website is the authority for which products belong in the catalog. Every product or configuration listed on the current site remains cataloged.
- **Four-Dimensional Catalog Metadata:** The catalog architecture cleanly distinguishes:
  1. *Source presence:* Listed on current USAP website (all 16 groups, 30 named models, 1 unnamed line).
  2. *Commercial availability:* Not confirmed (no assumption of current manufacturing, stocking, or purchase availability).
  3. *Client approval:* Pending (formal USAP client review required before production launch).
  4. *Specification status:* Confirmed from current HTML / Provisional / HoldDisputedSpecs.
- **Tone & Section Language:** All public product and group titles use natural names consistent with current first-party pages (the archival word `Reference` or `References` has been completely eliminated). Section eyebrow is `PRODUCT CATALOG`, heading is `Products and Models in This Family`, and model label is `Models and Configurations:`.
- **Shared Availability Notice:**
  > *"Product information below is based on USAP’s current public website and linked technical documents. Contact USAP engineering to confirm availability, configuration, compatibility, and final specifications for your application."*
- **Preservation of Technical Resources for Later Lifecycle Classification:**
  All 19 discovered product datasheets and revisions remain cataloged and linked to their respective product groups. The catalog is architected to support future lifecycle reclassification (e.g., active vs legacy/support) without deleting product pages, specifications, downloads, revision history, or support documentation.
- **Disputed Technical Values & Published Relationships:**
  - R3500/DRC-4 and DRC-3 with R3501/R3503 are presented as current-site published relationships without guaranteeing current purchase availability.
  - Conflicting specifications (LP-1001 impedance, LP-1017 angle, LP-1112MR power/gain, V-4213 wind rating, LP-1402/1403 power, 1910 datasheets, 1942 low-power variants) remain internally tracked in `ConflictHolds` and project documentation. Provisional display follows current HTML while omitting misleading disputed values.
- **Architectural Isolation:** All asset paths, dimensions, alt text, and classifications are centralized in `ProductCatalogService.cs`. The Razor templates bind to `@Model.HeroAsset` and `@family.Asset`, ensuring assets can be swapped without touching page markup or layout styles.
- **Native Placeholder Fallback:** The native CSS design placeholder markup (`.product-family-card__placeholder` with dot grid, antenna wireframe, and `PRODUCT IMAGERY IN DEVELOPMENT` badge) is preserved in `Index.cshtml` as an explicit fallback.

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
