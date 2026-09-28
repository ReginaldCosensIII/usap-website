namespace USAP.Web.Middleware;

using Microsoft.AspNetCore.Http;

/// <summary>
/// Provides permanent HTTP 301 redirects for legacy WordPress technical document URLs
/// to their canonical local PDF destinations as established by R2 research.
/// </summary>
public class LegacyDocumentRedirectMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Explicit mapping of 34 legacy public PDF URLs to canonical local documents.
    /// </summary>
    public static readonly IReadOnlyDictionary<string, string> RedirectMap =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // PDF-URL-001 - DOC-LP-HIGH-POWER
            ["/wp-content/uploads/2016/05/1001_1002_1005-data-sheet.pdf"] = "/technical-resources/documents/log-periodic-antennas/usap-lp-1001-lp-1002-lp-1005-data-sheet.pdf",
            // PDF-URL-002 - DOC-LP-1017
            ["/wp-content/uploads/2016/05/1017-data-sheet-1.pdf"] = "/technical-resources/documents/log-periodic-antennas/usap-lp-1017-data-sheet.pdf",
            // PDF-URL-003 - DOC-LP-1017
            ["/wp-content/uploads/2016/05/1017-data-sheet-2.pdf"] = "/technical-resources/documents/log-periodic-antennas/usap-lp-1017-data-sheet.pdf",
            // PDF-URL-004 - DOC-LP-1017
            ["/wp-content/uploads/2016/05/1017-data-sheet-3.pdf"] = "/technical-resources/documents/log-periodic-antennas/usap-lp-1017-data-sheet.pdf",
            // PDF-URL-005 - DOC-LP-1017
            ["/wp-content/uploads/2016/05/1017-data-sheet-4.pdf"] = "/technical-resources/documents/log-periodic-antennas/usap-lp-1017-data-sheet.pdf",
            // PDF-URL-006 - DOC-LP-1017
            ["/wp-content/uploads/2016/05/1017-data-sheet.pdf"] = "/technical-resources/documents/log-periodic-antennas/usap-lp-1017-data-sheet.pdf",
            // PDF-URL-007 - DOC-LP-1112MR
            ["/wp-content/uploads/2016/05/1112-data-sheet-revised.pdf"] = "/technical-resources/documents/log-periodic-antennas/usap-lp-1112mr-data-sheet.pdf",
            // PDF-URL-008 - DOC-LP-1112MR
            ["/wp-content/uploads/2016/05/1112-data-sheet.pdf"] = "/technical-resources/documents/log-periodic-antennas/usap-lp-1112mr-data-sheet.pdf",
            // PDF-URL-009 - DOC-LP-1402-1403
            ["/wp-content/uploads/2016/05/1402AC_1403AB.pdf"] = "/technical-resources/documents/portable-transportable-antennas/usap-lp-1402-lp-1403-data-sheet.pdf",
            // PDF-URL-010 - DOC-1910
            ["/wp-content/uploads/2016/05/1910AA_1910BA.pdf"] = "/technical-resources/documents/portable-transportable-antennas/usap-1910aa-1910ba-data-sheet.pdf",
            // PDF-URL-011 - DOC-V4213-OVERVIEW
            ["/wp-content/uploads/2016/05/4213cutsheet-revised.pdf"] = "/technical-resources/documents/portable-transportable-antennas/usap-v4213-portable-discone-overview.pdf",
            // PDF-URL-012 - DOC-V4213-OVERVIEW
            ["/wp-content/uploads/2016/05/4213cutsheet.pdf"] = "/technical-resources/documents/portable-transportable-antennas/usap-v4213-portable-discone-overview.pdf",
            // PDF-URL-013 - DOC-LP-1019
            ["/wp-content/uploads/2016/06/1019-and-1019-ss-data-sheet.pdf"] = "/technical-resources/documents/log-periodic-antennas/usap-lp-1019-series-data-sheet.pdf",
            // PDF-URL-014 - DOC-1910
            ["/wp-content/uploads/2016/06/1910-data-sheet-revised.pdf"] = "/technical-resources/documents/portable-transportable-antennas/usap-1910aa-1910ba-data-sheet.pdf",
            // PDF-URL-015 - DOC-1910
            ["/wp-content/uploads/2016/06/1910-data-sheet.pdf"] = "/technical-resources/documents/portable-transportable-antennas/usap-1910aa-1910ba-data-sheet.pdf",
            // PDF-URL-016 - DOC-DRC3
            ["/wp-content/uploads/2016/06/DRC-3-Data-Sheet-revised.pdf"] = "/technical-resources/documents/antenna-rotator-control-systems/usap-drc-3-controller-data-sheet.pdf",
            // PDF-URL-017 - DOC-DRC3
            ["/wp-content/uploads/2016/06/DRC-3-Data-Sheet.pdf"] = "/technical-resources/documents/antenna-rotator-control-systems/usap-drc-3-controller-data-sheet.pdf",
            // PDF-URL-018 - DOC-LP-1018BA
            ["/wp-content/uploads/2016/06/LP_1018BA_data_sheet.pdf"] = "/technical-resources/documents/log-periodic-antennas/usap-lp-1018ba-data-sheet.pdf",
            // PDF-URL-019 - DOC-LP-1018BA
            ["/wp-content/uploads/2016/06/LP_1018BA_data_sheet_revised.pdf"] = "/technical-resources/documents/log-periodic-antennas/usap-lp-1018ba-data-sheet.pdf",
            // PDF-URL-020 - DOC-1942
            ["/wp-content/uploads/2016/06/MODEL-1942-data-sheet-revised.pdf"] = "/technical-resources/documents/nvis-antennas/usap-1942-nvis-series-data-sheet.pdf",
            // PDF-URL-021 - DOC-1942
            ["/wp-content/uploads/2016/06/MODEL-1942-data-sheet.pdf"] = "/technical-resources/documents/nvis-antennas/usap-1942-nvis-series-data-sheet.pdf",
            // PDF-URL-022 - DOC-V4213-CONFIG
            ["/wp-content/uploads/2016/06/Model-V4213-data-sheet-revised.pdf"] = "/technical-resources/documents/portable-transportable-antennas/usap-v4213ad-v4213ac-data-sheet.pdf",
            // PDF-URL-023 - DOC-V4213-CONFIG
            ["/wp-content/uploads/2016/06/Model-V4213-data-sheet.pdf"] = "/technical-resources/documents/portable-transportable-antennas/usap-v4213ad-v4213ac-data-sheet.pdf",
            // PDF-URL-024 - DOC-T3002
            ["/wp-content/uploads/2016/06/T-3002-Data-Sheet.pdf"] = "/technical-resources/documents/tower-systems-accessories/usap-t-3002-tower-system-data-sheet.pdf",
            // PDF-URL-025 - DOC-APERIODIC
            ["/wp-content/uploads/2016/06/USAP-Aperiodic-Loop-Antenna-data-sheet.pdf"] = "/technical-resources/documents/aperiodic-loop-antennas/usap-aperiodic-loop-antenna-data-sheet.pdf",
            // PDF-URL-026 - DOC-R3500
            ["/wp-content/uploads/2016/07/MODEL-R3500-data-sheet.doc.pdf"] = "/technical-resources/documents/antenna-rotator-control-systems/usap-r3500-rotator-data-sheet.pdf",
            // PDF-URL-027 - DOC-DRC4
            ["/wp-content/uploads/2016/10/DRC-4-Data-Sheet.pdf"] = "/technical-resources/documents/antenna-rotator-control-systems/usap-drc-4-controller-data-sheet.pdf",
            // PDF-URL-028 - DOC-R3501
            ["/wp-content/uploads/2016/10/MODEL-R3501-data-sheet.doc.pdf"] = "/technical-resources/documents/antenna-rotator-control-systems/usap-r3501-rotator-data-sheet.pdf",
            // PDF-URL-029 - DOC-R3503
            ["/wp-content/uploads/2016/10/MODEL-R3503-data-sheet.doc.pdf"] = "/technical-resources/documents/antenna-rotator-control-systems/usap-r3503-rotator-data-sheet.pdf",
            // PDF-URL-030 - DOC-R3501
            ["/wp-content/uploads/2016/10/Model-R3501-CORRECTED-COPY.pdf"] = "/technical-resources/documents/antenna-rotator-control-systems/usap-r3501-rotator-data-sheet.pdf",
            // PDF-URL-031 - DOC-T3002
            ["/wp-content/uploads/2016/10/T3002_data_sheet.pdf"] = "/technical-resources/documents/tower-systems-accessories/usap-t-3002-tower-system-data-sheet.pdf",
            // PDF-URL-032 - DOC-LP-HIGH-POWER
            ["/wp-content/uploads/2019/04/1001_1002_1005-data-sheet.pdf"] = "/technical-resources/documents/log-periodic-antennas/usap-lp-1001-lp-1002-lp-1005-data-sheet.pdf",
            // PDF-URL-033 - DOC-1910
            ["/wp-content/uploads/2024/02/1910AA_1910BA-revised-1.pdf"] = "/technical-resources/documents/portable-transportable-antennas/usap-1910aa-1910ba-data-sheet.pdf",
            // PDF-URL-034 - DOC-LP-1018BA
            ["/wp-content/uploads/2024/02/LP_1018BA_data_sheet_revised.pdf"] = "/technical-resources/documents/log-periodic-antennas/usap-lp-1018ba-data-sheet.pdf"
        };

    public LegacyDocumentRedirectMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value;
        if (!string.IsNullOrEmpty(path))
        {
            if (RedirectMap.TryGetValue(path, out var target))
            {
                var destination = target + context.Request.QueryString.Value;
                context.Response.StatusCode = StatusCodes.Status301MovedPermanently;
                context.Response.Headers.Location = destination;
                return Task.CompletedTask;
            }

            // Alternate raw-PDF path redirect: /documents/technical/... -> /technical-resources/documents/...
            if (path.StartsWith("/documents/technical/", StringComparison.OrdinalIgnoreCase))
            {
                var canonicalPath = "/technical-resources/documents/" + path["/documents/technical/".Length..];
                var destination = canonicalPath + context.Request.QueryString.Value;
                context.Response.StatusCode = StatusCodes.Status301MovedPermanently;
                context.Response.Headers.Location = destination;
                return Task.CompletedTask;
            }
        }

        return _next(context);
    }
}
