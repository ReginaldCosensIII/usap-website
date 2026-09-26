using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using USAP.Web.Models.Catalog;
using USAP.Web.Services.Catalog;

namespace USAP.Web.Pages;

public class TechnicalResourcesModel : PageModel
{
    private static readonly HashSet<string> AntennaFamilySlugs = new(StringComparer.OrdinalIgnoreCase)
    {
        "log-periodic-antennas",
        "portable-transportable-antennas",
        "aperiodic-loop-antennas",
        "nvis-antennas"
    };

    private readonly IProductCatalogService _catalogService;

    public TechnicalResourcesModel(IProductCatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [BindProperty(SupportsGet = true)]
    public string? Q { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Category { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Family { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Type { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? View { get; set; }

    public IReadOnlyList<ProductResourceRecord> AllDocuments { get; private set; } = Array.Empty<ProductResourceRecord>();
    public IReadOnlyList<ProductResourceRecord> FilteredDocuments { get; private set; } = Array.Empty<ProductResourceRecord>();
    public IReadOnlyList<ResourceCategoryItem> Categories { get; private set; } = Array.Empty<ResourceCategoryItem>();

    public string? ActiveFilterLabel { get; private set; }
    public string ActiveFamily { get; private set; } = "all";
    public int TotalDocumentCount => AllDocuments.Count;

    public void OnGet()
    {
        AllDocuments = _catalogService.GetAllTechnicalDocuments();

        // Build product family category counts
        var categories = new List<ResourceCategoryItem>
        {
            new("all", "All", AllDocuments.Count)
        };

        var families = _catalogService.GetFamilies();
        foreach (var family in families)
        {
            var count = AllDocuments.Count(d => string.Equals(d.FamilySlug, family.Slug, StringComparison.OrdinalIgnoreCase));
            categories.Add(new(family.Slug, family.Name, count));
        }
        Categories = categories;

        // Reconcile family / category parameter
        var selectedCategory = !string.IsNullOrWhiteSpace(Category) ? Category : Family;
        if (!string.IsNullOrWhiteSpace(selectedCategory) && !string.Equals(selectedCategory, "all", StringComparison.OrdinalIgnoreCase))
        {
            ActiveFamily = selectedCategory.ToLowerInvariant();
        }

        var docs = AllDocuments.AsEnumerable();

        // 1. Predefined umbrella view (e.g. view=antenna-systems)
        if (string.Equals(View, "antenna-systems", StringComparison.OrdinalIgnoreCase))
        {
            docs = docs.Where(d => AntennaFamilySlugs.Contains(d.FamilySlug));
            ActiveFilterLabel = "Antenna Systems";
        }
        // 2. Structured resource type (e.g. type=data-sheet)
        else if (string.Equals(Type, "data-sheet", StringComparison.OrdinalIgnoreCase))
        {
            docs = docs.Where(d => string.Equals(d.DocumentType, "Data sheet", StringComparison.OrdinalIgnoreCase));
            ActiveFilterLabel = "Product Data Sheets";
        }
        // 3. Normal category / family filtering
        else if (ActiveFamily != "all")
        {
            docs = docs.Where(d => string.Equals(d.FamilySlug, ActiveFamily, StringComparison.OrdinalIgnoreCase));
        }

        // 4. Keyword search
        if (!string.IsNullOrWhiteSpace(Q))
        {
            var terms = Q.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            docs = docs.Where(d => terms.All(term =>
                d.Title.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                d.FamilyName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                d.ModelCodes.Any(m => m.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                d.Description.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                d.SearchText.Contains(term, StringComparison.OrdinalIgnoreCase)
            ));
        }

        FilteredDocuments = docs.ToList();
    }

    public record ResourceCategoryItem(string Slug, string Name, int Count);
}
