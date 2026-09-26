using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using USAP.Web.Models;
using USAP.Web.Models.Catalog;
using USAP.Web.Services.Catalog;

namespace USAP.Web.Pages.TechnicalResources;

public class DocumentModel : PageModel
{
    private readonly IProductCatalogService _catalogService;

    public DocumentModel(IProductCatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    public ProductResourceRecord Document { get; private set; } = null!;
    public ProductFamilyRecord? Family { get; private set; }
    public IReadOnlyList<ProductGroupRecord> RelatedGroups { get; private set; } = Array.Empty<ProductGroupRecord>();

    public IActionResult OnGet(string slug)
    {
        var doc = _catalogService.GetTechnicalDocumentBySlug(slug);
        if (doc == null)
        {
            return NotFound();
        }

        Document = doc;
        Family = _catalogService.GetFamilyBySlug(doc.FamilySlug);
        RelatedGroups = doc.ProductGroupIds
            .Select(id => _catalogService.GetProductGroupById(id))
            .Where(g => g != null)
            .Cast<ProductGroupRecord>()
            .ToList();

        ViewData[SeoMetadata.ViewDataKey] = new SeoMetadata
        {
            Title = $"{Document.Title} | Technical Resources",
            Description = Document.Description,
            CanonicalPath = $"/technical-resources/document/{Document.Slug}",
            Robots = "index, follow",
            OpenGraphType = "article"
        };

        return Page();
    }
}
