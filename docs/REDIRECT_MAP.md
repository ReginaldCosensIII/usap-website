# Redirect Map

**Note:** This file documents proposed launch redirects. No redirect behavior is currently implemented. The likely future implementation location is ASP.NET Core Rewrite Middleware, but that decision remains subject to the complete redirect inventory and IIS deployment review.

## Verified Top-Level Mappings

| Legacy path | Proposed target | Intended status | Implementation state |
|---|---|---|---|
| `/antennas/` | `/products` | 301 | Documented only |
| `/about-usap/` | `/about-us` | 301 | Documented only |
| `/datasheets/` | `/technical-resources` | 301 | Documented only |
| `/request-info/` | `/request-a-quote` | 301 | Documented only |

## Implemented Legacy Document Redirects (USAP-TECHDOC-001)

The following 34 legacy WordPress technical document URLs are actively redirected via permanent HTTP 301 responses using `LegacyDocumentRedirectMiddleware` in ASP.NET Core:

| Legacy URL ID | Legacy WordPress Path | Canonical Destination | Status |
|---|---|---|---|
| PDF-URL-001 | `/wp-content/uploads/2016/05/1001_1002_1005-data-sheet.pdf` | `/technical-resources/documents/log-periodic-antennas/usap-lp-1001-lp-1002-lp-1005-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-002 | `/wp-content/uploads/2016/05/1017-data-sheet-1.pdf` | `/technical-resources/documents/log-periodic-antennas/usap-lp-1017-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-003 | `/wp-content/uploads/2016/05/1017-data-sheet-2.pdf` | `/technical-resources/documents/log-periodic-antennas/usap-lp-1017-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-004 | `/wp-content/uploads/2016/05/1017-data-sheet-3.pdf` | `/technical-resources/documents/log-periodic-antennas/usap-lp-1017-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-005 | `/wp-content/uploads/2016/05/1017-data-sheet-4.pdf` | `/technical-resources/documents/log-periodic-antennas/usap-lp-1017-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-006 | `/wp-content/uploads/2016/05/1017-data-sheet.pdf` | `/technical-resources/documents/log-periodic-antennas/usap-lp-1017-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-007 | `/wp-content/uploads/2016/05/1112-data-sheet-revised.pdf` | `/technical-resources/documents/log-periodic-antennas/usap-lp-1112mr-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-008 | `/wp-content/uploads/2016/05/1112-data-sheet.pdf` | `/technical-resources/documents/log-periodic-antennas/usap-lp-1112mr-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-009 | `/wp-content/uploads/2016/05/1402AC_1403AB.pdf` | `/technical-resources/documents/portable-transportable-antennas/usap-lp-1402-lp-1403-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-010 | `/wp-content/uploads/2016/05/1910AA_1910BA.pdf` | `/technical-resources/documents/portable-transportable-antennas/usap-1910aa-1910ba-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-011 | `/wp-content/uploads/2016/05/4213cutsheet-revised.pdf` | `/technical-resources/documents/portable-transportable-antennas/usap-v4213-portable-discone-overview.pdf` | 301 Permanent (Active) |
| PDF-URL-012 | `/wp-content/uploads/2016/05/4213cutsheet.pdf` | `/technical-resources/documents/portable-transportable-antennas/usap-v4213-portable-discone-overview.pdf` | 301 Permanent (Active) |
| PDF-URL-013 | `/wp-content/uploads/2016/06/1019-and-1019-ss-data-sheet.pdf` | `/technical-resources/documents/log-periodic-antennas/usap-lp-1019-series-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-014 | `/wp-content/uploads/2016/06/1910-data-sheet-revised.pdf` | `/technical-resources/documents/portable-transportable-antennas/usap-1910aa-1910ba-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-015 | `/wp-content/uploads/2016/06/1910-data-sheet.pdf` | `/technical-resources/documents/portable-transportable-antennas/usap-1910aa-1910ba-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-016 | `/wp-content/uploads/2016/06/DRC-3-Data-Sheet-revised.pdf` | `/technical-resources/documents/antenna-rotator-control-systems/usap-drc-3-controller-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-017 | `/wp-content/uploads/2016/06/DRC-3-Data-Sheet.pdf` | `/technical-resources/documents/antenna-rotator-control-systems/usap-drc-3-controller-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-018 | `/wp-content/uploads/2016/06/LP_1018BA_data_sheet.pdf` | `/technical-resources/documents/log-periodic-antennas/usap-lp-1018ba-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-019 | `/wp-content/uploads/2016/06/LP_1018BA_data_sheet_revised.pdf` | `/technical-resources/documents/log-periodic-antennas/usap-lp-1018ba-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-020 | `/wp-content/uploads/2016/06/MODEL-1942-data-sheet-revised.pdf` | `/technical-resources/documents/nvis-antennas/usap-1942-nvis-series-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-021 | `/wp-content/uploads/2016/06/MODEL-1942-data-sheet.pdf` | `/technical-resources/documents/nvis-antennas/usap-1942-nvis-series-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-022 | `/wp-content/uploads/2016/06/Model-V4213-data-sheet-revised.pdf` | `/technical-resources/documents/portable-transportable-antennas/usap-v4213ad-v4213ac-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-023 | `/wp-content/uploads/2016/06/Model-V4213-data-sheet.pdf` | `/technical-resources/documents/portable-transportable-antennas/usap-v4213ad-v4213ac-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-024 | `/wp-content/uploads/2016/06/T-3002-Data-Sheet.pdf` | `/technical-resources/documents/tower-systems-accessories/usap-t-3002-tower-system-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-025 | `/wp-content/uploads/2016/06/USAP-Aperiodic-Loop-Antenna-data-sheet.pdf` | `/technical-resources/documents/aperiodic-loop-antennas/usap-aperiodic-loop-antenna-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-026 | `/wp-content/uploads/2016/07/MODEL-R3500-data-sheet.doc.pdf` | `/technical-resources/documents/antenna-rotator-control-systems/usap-r3500-rotator-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-027 | `/wp-content/uploads/2016/10/DRC-4-Data-Sheet.pdf` | `/technical-resources/documents/antenna-rotator-control-systems/usap-drc-4-controller-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-028 | `/wp-content/uploads/2016/10/MODEL-R3501-data-sheet.doc.pdf` | `/technical-resources/documents/antenna-rotator-control-systems/usap-r3501-rotator-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-029 | `/wp-content/uploads/2016/10/MODEL-R3503-data-sheet.doc.pdf` | `/technical-resources/documents/antenna-rotator-control-systems/usap-r3503-rotator-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-030 | `/wp-content/uploads/2016/10/Model-R3501-CORRECTED-COPY.pdf` | `/technical-resources/documents/antenna-rotator-control-systems/usap-r3501-rotator-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-031 | `/wp-content/uploads/2016/10/T3002_data_sheet.pdf` | `/technical-resources/documents/tower-systems-accessories/usap-t-3002-tower-system-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-032 | `/wp-content/uploads/2019/04/1001_1002_1005-data-sheet.pdf` | `/technical-resources/documents/log-periodic-antennas/usap-lp-1001-lp-1002-lp-1005-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-033 | `/wp-content/uploads/2024/02/1910AA_1910BA-revised-1.pdf` | `/technical-resources/documents/portable-transportable-antennas/usap-1910aa-1910ba-data-sheet.pdf` | 301 Permanent (Active) |
| PDF-URL-034 | `/wp-content/uploads/2024/02/LP_1018BA_data_sheet_revised.pdf` | `/technical-resources/documents/log-periodic-antennas/usap-lp-1018ba-data-sheet.pdf` | 301 Permanent (Active) |

## Canonical Raw-PDF URL Reconciliation (USAP-TECHDOC-001-R1)

To prevent search engines and crawlers from indexing two separate public URLs for identical PDF binaries, the physical static file location `/documents/technical/{family-slug}/{filename}` is canonicalized to the public route `/technical-resources/documents/{family-slug}/{filename}`.

`LegacyDocumentRedirectMiddleware` actively intercepts all requests starting with `/documents/technical/` and issues an immediate HTTP 301 permanent redirect to `/technical-resources/documents/{family-slug}/{filename}`:

| Source Alternate URL Pattern | Destination Canonical URL Pattern | Status | Implementation State |
|---|---|---|---|
| `/documents/technical/{family}/{filename}.pdf` | `/technical-resources/documents/{family}/{filename}.pdf` | 301 Permanent | Active in `LegacyDocumentRedirectMiddleware` |
| `/documents/technical/` (directory root) | `/technical-resources` | 301 Permanent | Active in `LegacyDocumentRedirectMiddleware` |

All 17 canonical PDF paths have been verified: alternate paths return HTTP 301, and canonical paths return HTTP 200 `application/pdf`.


## Deferred Mappings and Policies

* `/contact-us/` already corresponds to the new `/contact-us` route, but canonical trailing-slash behavior must be verified before adding a redirect rule.
* Legacy `/antennas/{product-slug}/` mappings are deferred to Milestone C3.
* Legacy `/antenna-category/{category-slug}/` mappings are deferred to Milestone C3.
* WordPress HTML attachment-page mappings are deferred.
* Query-string attachment URLs are deferred.
* Invalid, duplicate, or low-value URLs may eventually return 404 or 410 rather than redirecting.
* Redirect chains and loops are prohibited.
* All redirect targets must return successful canonical responses (verified HTTP 200).
