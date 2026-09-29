namespace USAP.Web.Services;

using USAP.Web.Models;
using USAP.Web.Services.Catalog;

public class CtaContextResolver : ICtaContextResolver
{
    private readonly IProductCatalogService _catalogService;
    public CtaContextResolver(IProductCatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    public CtaContext Resolve(string? reason, string? family, string? group, string? doc)
    {
        // 1. Resolve contact intent
        var intent = reason?.Trim().ToLowerInvariant() switch
        {
            "product-information" => InquiryType.ProductInformation,
            "engineering-support" => InquiryType.EngineeringSupport,
            "technical-documentation" => InquiryType.TechnicalDocumentation,
            "request-a-quote" => InquiryType.RequestAQuote,
            _ => InquiryType.GeneralInquiry
        };

        string? familySlug = null;
        string? familyName = null;
        string? groupId = null;
        string? groupName = null;
        string? documentSlug = null;
        string? documentTitle = null;
        string? modelCode = null;
        string? displayCategory = null;
        string? displayTitle = null;
        string? suggestedProduct = null;
        string? contextSummary = null;

        // 2. Validate technical document context if provided
        if (!string.IsNullOrWhiteSpace(doc))
        {
            var cleanDoc = doc.Trim();
            var docRecord = _catalogService.GetTechnicalDocumentBySlug(cleanDoc);
            if (docRecord != null)
            {
                documentSlug = docRecord.Slug;
                documentTitle = docRecord.Title;
                familySlug = docRecord.FamilySlug;
                familyName = docRecord.FamilyName;

                if (docRecord.ModelCodes.Count == 1)
                {
                    modelCode = docRecord.ModelCodes[0];
                    suggestedProduct = docRecord.ModelCodes[0];
                }
                else
                {
                    suggestedProduct = docRecord.Title;
                }

                displayCategory = "Technical Document";
                displayTitle = docRecord.Title;
                contextSummary = $"Technical Document: {docRecord.Title} ({docRecord.Slug})";

                if (intent == InquiryType.GeneralInquiry)
                {
                    intent = InquiryType.TechnicalDocumentation;
                }
            }
        }

        // 3. Validate product group context if not already established by document
        if (displayTitle == null && !string.IsNullOrWhiteSpace(group))
        {
            var cleanGroup = group.Trim();
            var groupRecord = _catalogService.GetProductGroupById(cleanGroup);
            if (groupRecord != null)
            {
                groupId = groupRecord.Id;
                groupName = groupRecord.Name;
                suggestedProduct = groupRecord.Name;
                displayCategory = "Product Group";
                displayTitle = groupRecord.Name;
                contextSummary = $"Product Group: {groupRecord.Name} (ID: {groupRecord.Id})";

                var fam = _catalogService.GetFamilyById(groupRecord.FamilyId);
                if (fam != null)
                {
                    familySlug = fam.Slug;
                    familyName = fam.Name;
                }

                if (intent == InquiryType.GeneralInquiry)
                {
                    intent = InquiryType.ProductInformation;
                }
            }
        }

        // 4. Validate family context ONLY if not already established by higher-priority document or group
        if (displayTitle == null && !string.IsNullOrWhiteSpace(family))
        {
            var cleanFamily = family.Trim();
            var familyRecord = _catalogService.GetFamilyBySlug(cleanFamily);
            if (familyRecord != null)
            {
                familySlug = familyRecord.Slug;
                familyName = familyRecord.Name;
                displayCategory = "Product Family";
                displayTitle = familyRecord.Name;
                suggestedProduct = familyRecord.Name;
                contextSummary = $"Product Family: {familyRecord.Name}";

                if (intent == InquiryType.GeneralInquiry)
                {
                    intent = InquiryType.ProductInformation;
                }
            }
        }

        // 6. Generic Engineering Support context if reason set but no specific item
        if (intent == InquiryType.EngineeringSupport && displayTitle == null)
        {
            displayCategory = "Engineering & Requirements Support";
            displayTitle = "Application & System Requirements";
            contextSummary = "Engineering & Requirements Support";
        }

        return new CtaContext(
            Intent: intent,
            FamilySlug: familySlug,
            FamilyName: familyName,
            GroupId: groupId,
            GroupName: groupName,
            DocumentSlug: documentSlug,
            DocumentTitle: documentTitle,
            ModelCode: modelCode,
            DisplayCategory: displayCategory,
            DisplayTitle: displayTitle,
            SuggestedProductOfInterest: suggestedProduct,
            ContextSummary: contextSummary
        );
    }
}
