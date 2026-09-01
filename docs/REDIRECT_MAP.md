# Redirect Map

**Note:** This file documents proposed launch redirects. No redirect behavior is currently implemented. The likely future implementation location is ASP.NET Core Rewrite Middleware, but that decision remains subject to the complete redirect inventory and IIS deployment review.

## Verified Top-Level Mappings

| Legacy path | Proposed target | Intended status | Implementation state |
|---|---|---|---|
| `/antennas/` | `/products` | 301 | Documented only |
| `/about-usap/` | `/about-us` | 301 | Documented only |
| `/datasheets/` | `/technical-resources` | 301 | Documented only |
| `/request-info/` | `/request-a-quote` | 301 | Documented only |

## Deferred Mappings and Policies

* `/contact-us/` already corresponds to the new `/contact-us` route, but canonical trailing-slash behavior must be verified before adding a redirect rule.
* Legacy `/antennas/{product-slug}/` mappings are deferred.
* Legacy `/antenna-category/{category-slug}/` mappings are deferred.
* WordPress attachment-page and PDF mappings are deferred.
* Query-string attachment URLs are deferred.
* Invalid, duplicate, or low-value URLs may eventually return 404 or 410 rather than redirecting.
* Final redirect rules require the approved product-family and resource inventories.
* Redirect chains and loops are prohibited.
* Redirect targets must return successful canonical responses.
