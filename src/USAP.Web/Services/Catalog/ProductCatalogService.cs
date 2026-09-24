namespace USAP.Web.Services.Catalog;

using System.Text.RegularExpressions;
using USAP.Web.Models.Catalog;

public class ProductCatalogService : IProductCatalogService
{
    private static readonly Regex SlugRegex = new("^[a-z0-9-]+$", RegexOptions.Compiled);

    private static readonly IReadOnlyList<string> CanonicalFamilyOrder = new[]
    {
        "log-periodic-antennas",
        "portable-transportable-antennas",
        "aperiodic-loop-antennas",
        "nvis-antennas",
        "antenna-rotator-control-systems",
        "tower-systems-accessories"
    };

    private static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> CanonicalFamilyGroupDistribution =
        new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["log-periodic-antennas"] = new[] { "lp-high-power", "lp-1017", "lp-1018ba", "lp-1019", "lp-1112mr" },
            ["portable-transportable-antennas"] = new[] { "v-4213", "lp-1402-1403", "1910" },
            ["aperiodic-loop-antennas"] = new[] { "aperiodic" },
            ["nvis-antennas"] = new[] { "1942" },
            ["antenna-rotator-control-systems"] = new[] { "r3500", "r3501", "r3503", "drc-3", "drc-4" },
            ["tower-systems-accessories"] = new[] { "t-3002" }
        };

    private static readonly HashSet<string> InterimResourceIds = new(StringComparer.OrdinalIgnoreCase)
    {
        "doc-lp-high-power",
        "doc-lp-1018ba",
        "doc-lp-1019",
        "doc-1910-2024",
        "doc-aperiodic",
        "doc-t-3002-oct2016"
    };

    /// <summary>
    /// Required 1942 NVIS configuration identifiers (all 6 published in provisional public catalog).
    /// </summary>
    public static readonly IReadOnlyList<string> Required1942ConfigurationCodes = new[]
    {
        "1942-RT",
        "1942-TA",
        "1942-GM",
        "1942-RT-LP",
        "1942-TA-LP",
        "1942-GM-LP"
    };

    public static readonly IReadOnlyList<string> LowPower1942ConfigurationCodes = new[]
    {
        "1942-RT-LP",
        "1942-TA-LP",
        "1942-GM-LP"
    };

    private static readonly IReadOnlyList<string> ProhibitedTokensAndClaims = new[]
    {
        // Internal governance and research tokens
        "ApprovalStatus",
        "PublicationRecommendation",
        "SpecificationStatus",
        "ConflictHolds",
        "SourceNotes",
        "SourcePresence",
        "CommercialAvailability",
        "ClientConfirmation",
        "HoldDisputedSpecs",

        // Prohibited held claim strings and unconfirmed marketing phrases
        "complete 30-foot mast field system",
        "antenna-only configuration",
        "rapid-deployment",
        "400 W PEP",
        "2 kW PEP",
        "4,300 in-lb",
        "10,000 lb vertical load",
        "9,000 in-lb",
        "23,000 in-lb",
        "23,700 in-lb",
        "60,000 in-lb",
        "20,000 lb vertical load",
        "compatible with the DRC-3",
        "paired with the DRC-4",
        "standalone and PC control",
        "microprocessor-based",
        "rackmount",
        "19-inch",
        "gap-free",
        "no-skip-zone",
        "guaranteed coverage",
        "continuous-rotation capability on compatible rotators",
        "documented for selected rotator systems",
        "50 ohms pressurized",
        "1-5/8 in EIA coaxial flange",
        "Complete field-system package record"
    };

    private readonly IReadOnlyList<ProductFamilyRecord> _families;
    private readonly IReadOnlyList<ProductGroupRecord> _productGroups;
    private readonly IReadOnlyList<ProductResourceRecord> _resources;

    private readonly Dictionary<string, ProductFamilyRecord> _familiesById;
    private readonly Dictionary<string, ProductFamilyRecord> _familiesBySlug;
    private readonly Dictionary<string, ProductGroupRecord> _productGroupsById;
    private readonly Dictionary<string, List<ProductGroupRecord>> _productGroupsByFamilyId;

    public ProductCatalogService()
    {
        var (families, productGroups, resources) = InitializeCatalogData();

        ValidateCatalog(families, productGroups, resources);

        _families = families;
        _productGroups = productGroups;
        _resources = resources;

        _familiesById = _families.ToDictionary(f => f.Id, StringComparer.OrdinalIgnoreCase);
        _familiesBySlug = _families.ToDictionary(f => f.Slug, StringComparer.OrdinalIgnoreCase);
        _productGroupsById = _productGroups.ToDictionary(g => g.Id, StringComparer.OrdinalIgnoreCase);

        _productGroupsByFamilyId = new Dictionary<string, List<ProductGroupRecord>>(StringComparer.OrdinalIgnoreCase);
        foreach (var group in _productGroups)
        {
            if (!_productGroupsByFamilyId.TryGetValue(group.FamilyId, out var groupList))
            {
                groupList = new List<ProductGroupRecord>();
                _productGroupsByFamilyId[group.FamilyId] = groupList;
            }

            groupList.Add(group);
        }
    }

    public IReadOnlyList<ProductFamilyRecord> GetFamilies() => _families;

    public IReadOnlyList<ProductFamilyRecord> GetFeaturedFamilies()
    {
        return _families
            .Where(f => f.DisplayOrder is 1 or 2 or 5 or 6)
            .OrderBy(f => f.DisplayOrder)
            .ToList();
    }

    public ProductFamilyRecord? GetFamilyBySlug(string? slug)
    {
        if (string.IsNullOrWhiteSpace(slug) || !SlugRegex.IsMatch(slug))
        {
            return null;
        }

        return _familiesBySlug.GetValueOrDefault(slug);
    }

    public ProductFamilyRecord? GetFamilyById(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        return _familiesById.GetValueOrDefault(id);
    }

    public IReadOnlyList<ProductGroupRecord> GetProductGroupsByFamily(string? familyId)
    {
        if (string.IsNullOrWhiteSpace(familyId))
        {
            return Array.Empty<ProductGroupRecord>();
        }

        return _productGroupsByFamilyId.TryGetValue(familyId, out var list)
            ? list
            : Array.Empty<ProductGroupRecord>();
    }

    public ProductGroupRecord? GetProductGroupById(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        return _productGroupsById.GetValueOrDefault(id);
    }

    public CatalogAsset GetLandingHeroAsset() => new(
        ImagePath: "/images/products/usap-products-landing-hero-direction-c-candidate-v1.png",
        AltText: "Technical visualization of antenna systems and engineering studies in a blue outdoor landscape.",
        IsPlaceholder: false);

    public IReadOnlyList<ProductResourceRecord> GetApprovedResourcesForGroup(string? groupId)
    {
        if (string.IsNullOrWhiteSpace(groupId))
        {
            return Array.Empty<ProductResourceRecord>();
        }

        return _resources
            .Where(r => string.Equals(r.ProductGroupId, groupId, StringComparison.OrdinalIgnoreCase) && InterimResourceIds.Contains(r.Id))
            .ToList();
    }

    private static void ValidateCatalog(
        IReadOnlyList<ProductFamilyRecord> families,
        IReadOnlyList<ProductGroupRecord> productGroups,
        IReadOnlyList<ProductResourceRecord> resources)
    {
        if (families.Count != 6)
        {
            throw new InvalidOperationException($"Expected exactly 6 canonical product families, found {families.Count}.");
        }

        var familySlugs = families.Select(f => f.Slug).ToList();
        for (var i = 0; i < CanonicalFamilyOrder.Count; i++)
        {
            if (!string.Equals(familySlugs[i], CanonicalFamilyOrder[i], StringComparison.Ordinal))
            {
                throw new InvalidOperationException($"Product family order mismatch at index {i}: expected '{CanonicalFamilyOrder[i]}', found '{familySlugs[i]}'.");
            }
        }

        if (productGroups.Count != 16)
        {
            throw new InvalidOperationException($"Expected exactly 16 product groups, found {productGroups.Count}.");
        }

        var familyGroupMap = productGroups
            .GroupBy(g => g.FamilyId, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Id).ToList(), StringComparer.OrdinalIgnoreCase);

        foreach (var (familyId, expectedGroupIds) in CanonicalFamilyGroupDistribution)
        {
            if (!familyGroupMap.TryGetValue(familyId, out var actualGroupIds))
            {
                throw new InvalidOperationException($"Missing expected groups for family '{familyId}'.");
            }

            if (actualGroupIds.Count != expectedGroupIds.Count)
            {
                throw new InvalidOperationException($"Family '{familyId}' group count mismatch: expected {expectedGroupIds.Count}, found {actualGroupIds.Count}.");
            }

            for (var i = 0; i < expectedGroupIds.Count; i++)
            {
                if (!string.Equals(actualGroupIds[i], expectedGroupIds[i], StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException($"Group mismatch in family '{familyId}' at position {i}: expected '{expectedGroupIds[i]}', found '{actualGroupIds[i]}'.");
                }
            }
        }

        var productGroupMap = productGroups.ToDictionary(g => g.Id, StringComparer.OrdinalIgnoreCase);
        var modelCodeSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var publicModelCodes = new List<string>();
        var namedModelCount = 0;

        foreach (var group in productGroups)
        {
            if (!group.HasPublishedModelNumber)
            {
                if (group.Models.Count != 0)
                {
                    throw new InvalidOperationException($"Group '{group.Id}' declares HasPublishedModelNumber = false but contains {group.Models.Count} models.");
                }
                continue;
            }

            if (group.Models.Count == 0)
            {
                throw new InvalidOperationException($"Group '{group.Id}' has no models defined.");
            }

            foreach (var model in group.Models)
            {
                if (string.IsNullOrWhiteSpace(model.ModelCode))
                {
                    throw new InvalidOperationException($"Product group '{group.Id}' contains a model with an empty ModelCode. Aperiodic must use HasPublishedModelNumber = false instead of empty model codes.");
                }

                if (!modelCodeSet.Add(model.ModelCode))
                {
                    throw new InvalidOperationException($"Product group '{group.Id}' contains duplicate model code '{model.ModelCode}'.");
                }

                namedModelCount++;
                publicModelCodes.Add(model.ModelCode);

                if (model.Characteristics != null)
                {
                    foreach (var c in model.Characteristics)
                    {
                        if (string.IsNullOrWhiteSpace(c.Label) || string.IsNullOrWhiteSpace(c.DisplayValue))
                        {
                            throw new InvalidOperationException($"Model '{model.ModelCode}' in group '{group.Id}' contains empty characteristic label or value.");
                        }
                    }
                }
            }

            // Validate explicitly curated OverviewCharacteristics
            if (group.OverviewCharacteristics != null)
            {
                if (group.OverviewCharacteristics.Count > 3)
                {
                    throw new InvalidOperationException($"Group '{group.Id}' overview characteristics exceed limit of 3 (found {group.OverviewCharacteristics.Count}).");
                }

                var overviewLabels = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var c in group.OverviewCharacteristics)
                {
                    if (string.IsNullOrWhiteSpace(c.Label) || string.IsNullOrWhiteSpace(c.DisplayValue))
                    {
                        throw new InvalidOperationException($"Group '{group.Id}' contains empty overview characteristic label or value.");
                    }
                    if (!overviewLabels.Add(c.Label))
                    {
                        throw new InvalidOperationException($"Group '{group.Id}' contains duplicate overview characteristic '{c.Label}'.");
                    }
                }
            }

            // Validate explicitly curated DetailedCharacteristics
            if (group.DetailedCharacteristics != null)
            {
                var detailedLabels = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var overviewLabels = (group.OverviewCharacteristics ?? Array.Empty<ProductCharacteristic>())
                    .Select(c => c.Label)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                foreach (var c in group.DetailedCharacteristics)
                {
                    if (string.IsNullOrWhiteSpace(c.Label) || string.IsNullOrWhiteSpace(c.DisplayValue))
                    {
                        throw new InvalidOperationException($"Group '{group.Id}' contains empty detailed characteristic label or value.");
                    }
                    if (!detailedLabels.Add(c.Label))
                    {
                        throw new InvalidOperationException($"Group '{group.Id}' contains duplicate detailed characteristic '{c.Label}'.");
                    }
                    if (overviewLabels.Contains(c.Label))
                    {
                        throw new InvalidOperationException($"Group '{group.Id}' contains overlapping characteristic '{c.Label}' in both overview and detailed collections.");
                    }
                }
            }
        }

        // C1 Binding Count Clarification:
        // Must be exactly 30 rendered named identifiers across 15 groups + 1 unnamed Aperiodic group record
        if (namedModelCount != 30)
        {
            throw new InvalidOperationException($"Public projection must contain exactly 30 named model/configuration codes, found {namedModelCount}.");
        }

        // 7. Aperiodic group handling: 0 named models, HasPublishedModelNumber == false
        var aperiodicGroup = productGroupMap["aperiodic"];
        if (aperiodicGroup.Models.Count != 0)
        {
            throw new InvalidOperationException($"Aperiodic group must have 0 models in public projection, found {aperiodicGroup.Models.Count}.");
        }

        if (aperiodicGroup.HasPublishedModelNumber)
        {
            throw new InvalidOperationException("Aperiodic group must set HasPublishedModelNumber to false.");
        }

        if (string.IsNullOrWhiteSpace(aperiodicGroup.ModelNote))
        {
            throw new InvalidOperationException("Aperiodic group must provide an explicit factual ModelNote.");
        }

        // 8. 1942 NVIS configuration validation: all 6 configuration names must be present
        foreach (var required1942Code in Required1942ConfigurationCodes)
        {
            if (!publicModelCodes.Contains(required1942Code, StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Required 1942 configuration identifier '{required1942Code}' is missing from public catalog models.");
            }
        }

        // Low-power 1942 variants must render without unpublished ratings, power, or weight claims
        var nvisGroup = productGroupMap["1942"];
        foreach (var lpCode in LowPower1942ConfigurationCodes)
        {
            var lpModel = nvisGroup.Models.FirstOrDefault(m => string.Equals(m.ModelCode, lpCode, StringComparison.OrdinalIgnoreCase));
            if (lpModel == null)
            {
                throw new InvalidOperationException($"Low-power 1942 model '{lpCode}' not found in NVIS group.");
            }

            if (lpModel.Characteristics != null)
            {
                foreach (var c in lpModel.Characteristics)
                {
                    if (c.Label.Contains("Power", StringComparison.OrdinalIgnoreCase) ||
                        c.Label.Contains("Weight", StringComparison.OrdinalIgnoreCase) ||
                        c.Label.Contains("Gain", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException($"Low-power 1942 model '{lpCode}' must not publish unverified '{c.Label}' characteristic.");
                    }
                }
            }
        }

        // 9. Technical Resource interim links: exactly 6 current interim documents
        if (resources.Count != 6)
        {
            throw new InvalidOperationException($"Current interim technical resources must contain exactly 6 approved records, found {resources.Count}.");
        }

        foreach (var resource in resources)
        {
            if (!InterimResourceIds.Contains(resource.Id))
            {
                throw new InvalidOperationException($"Resource '{resource.Id}' is not in the interim document set.");
            }

            if (!productGroupMap.ContainsKey(resource.ProductGroupId))
            {
                throw new InvalidOperationException($"Resource '{resource.Id}' references unknown product group '{resource.ProductGroupId}'.");
            }

            if (string.IsNullOrWhiteSpace(resource.Title) || string.IsNullOrWhiteSpace(resource.CurrentSourceUrl))
            {
                throw new InvalidOperationException($"Resource '{resource.Id}' has missing Title or CurrentSourceUrl.");
            }
        }

        // 10. AssociatedAsset validation: all 16 product groups must have an intentional AssociatedAsset in C1
        if (productGroups.Count != 16)
        {
            throw new InvalidOperationException($"Expected exactly 16 product groups, found {productGroups.Count}.");
        }

        foreach (var group in productGroups)
        {
            if (group.AssociatedAsset == null)
            {
                throw new InvalidOperationException($"Product group '{group.Id}' must have an AssociatedAsset in C1.");
            }

            if (string.IsNullOrWhiteSpace(group.AssociatedAsset.ImagePath) || !group.AssociatedAsset.ImagePath.StartsWith("/images/products/"))
            {
                throw new InvalidOperationException($"Product group '{group.Id}' has invalid AssociatedAsset ImagePath '{group.AssociatedAsset.ImagePath}'.");
            }

            if (string.IsNullOrWhiteSpace(group.AssociatedAsset.AltText))
            {
                throw new InvalidOperationException($"Product group '{group.Id}' has AssociatedAsset without AltText.");
            }
        }

        // 11. Prohibited governance tokens and held claims scan across all public data
        var allPublicTexts = new List<string>();

        foreach (var f in families)
        {
            allPublicTexts.Add(f.CardTitle);
            allPublicTexts.Add(f.CardSummary);
            allPublicTexts.Add(f.CardEyebrow);
            if (f.SectionEyebrow != null)
            {
                allPublicTexts.Add(f.SectionEyebrow);
            }
            if (f.SectionHeading != null)
            {
                allPublicTexts.Add(f.SectionHeading);
            }
            if (f.SectionIntro != null)
            {
                allPublicTexts.Add(f.SectionIntro);
            }
            if (f.PageTitle != null)
            {
                allPublicTexts.Add(f.PageTitle);
            }
            if (f.MetaDescription != null)
            {
                allPublicTexts.Add(f.MetaDescription);
            }
        }

        foreach (var g in productGroups)
        {
            allPublicTexts.Add(g.Name);
            allPublicTexts.Add(g.ShortDescription);
            allPublicTexts.Add(g.ExpandedIntroduction);
            if (g.ModelNote != null)
            {
                allPublicTexts.Add(g.ModelNote);
            }

            if (g.AllGroupCharacteristics != null)
            {
                foreach (var c in g.AllGroupCharacteristics)
                {
                    allPublicTexts.Add(c.Label);
                    allPublicTexts.Add(c.DisplayValue);
                }
            }

            if (g.AssociatedAsset != null)
            {
                allPublicTexts.Add(g.AssociatedAsset.AltText);
                if (g.AssociatedAsset.Caption != null)
                {
                    allPublicTexts.Add(g.AssociatedAsset.Caption);
                }
            }

            foreach (var m in g.Models)
            {
                allPublicTexts.Add(m.ModelCode);
                if (m.DisplayName != null)
                {
                    allPublicTexts.Add(m.DisplayName);
                }

                if (m.Description != null)
                {
                    allPublicTexts.Add(m.Description);
                }

                if (m.Characteristics != null)
                {
                    foreach (var c in m.Characteristics)
                    {
                        allPublicTexts.Add(c.Label);
                        allPublicTexts.Add(c.DisplayValue);
                    }
                }
            }
        }

        foreach (var r in resources)
        {
            allPublicTexts.Add(r.Title);
        }

        foreach (var text in allPublicTexts)
        {
            foreach (var prohibited in ProhibitedTokensAndClaims)
            {
                if (text.Contains(prohibited, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException($"Prohibited token or held claim detected in public catalog text: '{prohibited}' in '{text}'.");
                }
            }
        }
    }

    private static (
        IReadOnlyList<ProductFamilyRecord> Families,
        IReadOnlyList<ProductGroupRecord> ProductGroups,
        IReadOnlyList<ProductResourceRecord> Resources)
    InitializeCatalogData()
    {
        var families = new List<ProductFamilyRecord>
        {
            new(
                Id: "log-periodic-antennas",
                Slug: "log-periodic-antennas",
                Name: "Log Periodic Antennas",
                DisplayOrder: 1,
                CardEyebrow: "Wideband HF / Tactical / Base Station",
                CardTitle: "Log Periodic Antennas",
                CardSummary: "Broadband directional antenna systems spanning HF through UHF applications.",
                CardAsset: new CatalogAsset(
                    ImagePath: "/images/products/usap-family-card-log-periodic-family-visual-v1.png",
                    AltText: "Log periodic antenna array in an outdoor installation.",
                    IsPlaceholder: false),
                HeroAsset: null,
                Route: "/products/log-periodic-antennas",
                CurrentSourceUrls: new[] { "https://www.usantennaproducts.com/products/log-periodic-antennas/" },
                LegacySourceUrls: Array.Empty<string>(),
                ApprovalStatus: "ProjectSelectedReferenceCandidatePendingApproval",
                SourceNotes: "Primary category on public site. LP-1112MR is primarily classified here; transportable nature noted in group attributes.",
                ConflictHolds: new[]
                {
                    "LP-1001: Input impedance conflict across legacy datasheets (HTML 5 ohms vs PDF 50 ohms)",
                    "LP-1017: Low-frequency radiation angle discrepancy (HTML 3.5 deg vs PDF 35 deg)",
                    "LP-1112MR: Power, gain, connector, and dimension discrepancies between HTML and PDF",
                    "LP-1019: First model naming discrepancy between HTML (LP-1019) and PDF (LP-1019BA)"
                },
                ProductGroupIds: new[] { "lp-high-power", "lp-1017", "lp-1018ba", "lp-1019", "lp-1112mr" },
                ResponsiveHero: new ResponsiveHeroAsset(
                    DesktopImagePath: "/images/products/usap-family-log-periodic-antennas-hero-candidate-a-v2-desktop-preview.png",
                    DesktopWidth: 1536,
                    DesktopHeight: 576,
                    MobileImagePath: "/images/products/usap-family-log-periodic-antennas-hero-candidate-a-v2-mobile-crop.png",
                    MobileWidth: 768,
                    MobileHeight: 768,
                    AltText: "Log periodic antenna array in an outdoor installation.",
                    AssetClassification: "conceptual/reference-grounded family visual",
                    ApprovalStatus: "provisional — USAP review pending"
                ),
                SectionEyebrow: "PRODUCT MODELS & CONFIGURATIONS",
                SectionHeading: "Log Periodic Antenna Models & Configurations",
                SectionIntro: "Review USAP log periodic models, available configurations, published operating characteristics, and associated technical documentation for directional broadband applications across published frequency ranges.",
                PageTitle: "Log Periodic Antennas",
                MetaDescription: "Broadband directional log periodic antenna models, configurations, published operating characteristics, and technical documentation from United States Antenna Products."
            ),
            new(
                Id: "portable-transportable-antennas",
                Slug: "portable-transportable-antennas",
                Name: "Portable & Transportable Antenna Systems",
                DisplayOrder: 2,
                CardEyebrow: "Field Deployable / HF & VHF",
                CardTitle: "Portable & Transportable",
                CardSummary: "Field-deployable antenna systems covering HF and VHF communications.",
                CardAsset: new CatalogAsset(
                    ImagePath: "/images/products/usap-family-card-portable-transportable-placeholder-v1.png",
                    AltText: "Portable antenna system deployed on a guyed field mast.",
                    IsPlaceholder: false),
                HeroAsset: null,
                Route: "/products/portable-transportable-antennas",
                CurrentSourceUrls: new[]
                {
                    "https://www.usantennaproducts.com/products/portable-discone-antenna-system/",
                    "https://www.usantennaproducts.com/products/transportable-hf-antennas/"
                },
                LegacySourceUrls: Array.Empty<string>(),
                ApprovalStatus: "ProjectSelectedPlaceholderPendingApproval",
                SourceNotes: "Approved taxonomy consolidation combining current Portable Discone Antenna System and Transportable HF Antennas categories.",
                ConflictHolds: new[]
                {
                    "V-4213: Model designation vs wind rating inconsistency across datasheets",
                    "LP-1402-1403: Model-specific power rating discrepancy between HTML and PDF",
                    "1910: VSWR and power rating differences between legacy 2016 and preferred 2024 datasheets"
                },
                ProductGroupIds: new[] { "v-4213", "lp-1402-1403", "1910" },
                ResponsiveHero: new ResponsiveHeroAsset(
                    DesktopImagePath: "/images/products/usap-family-portable-transportable-hero-a3s-desktop-1536x576.png",
                    DesktopWidth: 1536,
                    DesktopHeight: 576,
                    MobileImagePath: "/images/products/usap-family-portable-transportable-hero-a3s-mobile-768x768.png",
                    MobileWidth: 768,
                    MobileHeight: 768,
                    AltText: "Field antenna mast deployed in a coastal terrain landscape.",
                    AssetClassification: "source-guided photorealistic product visualization",
                    ApprovalStatus: "provisional — USAP review pending"
                ),
                SectionEyebrow: "PRODUCT MODELS & CONFIGURATIONS",
                SectionHeading: "Portable & Transportable Antenna Models",
                SectionIntro: "Compare documented portable and transportable antenna configurations, including published discone, log periodic, and tactical dipole models for field-oriented applications.",
                PageTitle: "Portable & Transportable Antenna Systems",
                MetaDescription: "Documented portable and transportable antenna configurations including discone, log periodic, and tactical dipole models from United States Antenna Products."
            ),
            new(
                Id: "aperiodic-loop-antennas",
                Slug: "aperiodic-loop-antennas",
                Name: "Aperiodic Loop Antennas",
                DisplayOrder: 3,
                CardEyebrow: "Directional HF Receiving Arrays",
                CardTitle: "Aperiodic Loop Antennas",
                CardSummary: "Receive-oriented directional HF loop arrays for fixed and transportable installations.",
                CardAsset: new CatalogAsset(
                    ImagePath: "/images/products/usap-family-card-aperiodic-loop-element-v1.png",
                    AltText: "Aperiodic loop antenna element mounted on a tripod in a remote installation.",
                    IsPlaceholder: false),
                HeroAsset: null,
                Route: "/products/aperiodic-loop-antennas",
                CurrentSourceUrls: new[] { "https://www.usantennaproducts.com/products/aperiodic-loop-antennas/" },
                LegacySourceUrls: Array.Empty<string>(),
                ApprovalStatus: "ProjectSelectedPlaceholderPendingApproval",
                SourceNotes: "Covers untuned balanced loop arrays. R1 verified single public loop group.",
                ConflictHolds: new[] { "Aperiodic: Complete array geometry, spacing, and preamplifier details require client input" },
                ProductGroupIds: new[] { "aperiodic" },
                ResponsiveHero: new ResponsiveHeroAsset(
                    DesktopImagePath: "/images/products/usap-family-aperiodic-loop-antennas-hero-candidate-a-v1-desktop-preview.png",
                    DesktopWidth: 1536,
                    DesktopHeight: 576,
                    MobileImagePath: "/images/products/usap-family-aperiodic-loop-antennas-hero-candidate-a-v1-mobile-crop.png",
                    MobileWidth: 768,
                    MobileHeight: 768,
                    AltText: "Aperiodic loop antenna element mounted on a tripod in a remote installation.",
                    AssetClassification: "reference-grounded single-element visualization",
                    ApprovalStatus: "provisional — USAP review pending"
                ),
                SectionEyebrow: "SYSTEM CONFIGURATION & TECHNICAL DETAILS",
                SectionHeading: "Aperiodic Loop Antenna System & Configuration Details",
                SectionIntro: "Review the USAP aperiodic loop receiving system, its published operating characteristics, configuration guidance, and available technical documentation for directional reception.",
                PageTitle: "Aperiodic Loop Antennas",
                MetaDescription: "Broadband aperiodic loop receiving antenna system, published operating characteristics, configuration guidance, and technical documentation from United States Antenna Products."
            ),
            new(
                Id: "nvis-antennas",
                Slug: "nvis-antennas",
                Name: "NVIS Antennas",
                DisplayOrder: 4,
                CardEyebrow: "Near Vertical Incidence Skywave",
                CardTitle: "NVIS Antennas",
                CardSummary: "Short-to-medium-range HF antenna systems in rooftop, transportable, and ground-mount configurations.",
                CardAsset: new CatalogAsset(
                    ImagePath: "/images/products/usap-family-card-nvis-1942-family-visual-v1.png",
                    AltText: "Field-deployed NVIS antenna system with a central mast and broad wire footprint.",
                    IsPlaceholder: false),
                HeroAsset: null,
                Route: "/products/nvis-antennas",
                CurrentSourceUrls: new[] { "https://www.usantennaproducts.com/products/nvis-antennas/" },
                LegacySourceUrls: Array.Empty<string>(),
                ApprovalStatus: "ProjectSelectedPlaceholderPendingApproval",
                SourceNotes: "Primary NVIS category. R1 verified Model 1942 series.",
                ConflictHolds: new[] { "1942 series: PDF lists low-power variants not mentioned on HTML page" },
                ProductGroupIds: new[] { "1942" },
                ResponsiveHero: new ResponsiveHeroAsset(
                    DesktopImagePath: "/images/products/usap-family-nvis-antennas-hero-candidate-a-v2-desktop-preview.png",
                    DesktopWidth: 1536,
                    DesktopHeight: 576,
                    MobileImagePath: "/images/products/usap-family-nvis-antennas-hero-candidate-a-v2-mobile-crop.png",
                    MobileWidth: 768,
                    MobileHeight: 768,
                    AltText: "Field-deployed NVIS antenna array configured for high-angle skywave communications.",
                    AssetClassification: "1942-family-level reference-grounded visualization",
                    ApprovalStatus: "provisional — USAP review pending"
                ),
                SectionEyebrow: "PRODUCT MODELS & CONFIGURATIONS",
                SectionHeading: "1942 NVIS Antenna Models & Configurations",
                SectionIntro: "Review the six published 1942 identifiers covering rooftop, transportable, ground-mount, and corresponding low-power configurations for Near Vertical Incidence Skywave applications.",
                PageTitle: "1942 NVIS Antennas",
                MetaDescription: "Published 1942 Near Vertical Incidence Skywave (NVIS) antenna identifiers covering rooftop, transportable, and ground-mount configurations from United States Antenna Products."
            ),
            new(
                Id: "antenna-rotator-control-systems",
                Slug: "antenna-rotator-control-systems",
                Name: "Antenna Rotator & Control Systems",
                DisplayOrder: 5,
                CardEyebrow: "Positioning Hardware & Controllers",
                CardTitle: "Rotator & Control Systems",
                CardSummary: "Medium- to heavy-duty antenna rotators and digital controllers for precise azimuth positioning.",
                CardAsset: new CatalogAsset(
                    ImagePath: "/images/products/usap-family-card-rotator-control-r3500-drc4-a3s-recommended-v1.png",
                    AltText: "Heavy-duty R3500 rotator and tabletop DRC-4 controller shown together in a source-guided technical visualization.",
                    IsPlaceholder: false),
                HeroAsset: null,
                Route: "/products/antenna-rotator-control-systems",
                CurrentSourceUrls: new[] { "https://www.usantennaproducts.com/products/antenna-rotator-control-systems/" },
                LegacySourceUrls: Array.Empty<string>(),
                ApprovalStatus: "ProjectSelectedCandidatePendingApproval",
                SourceNotes: "Covers R3500, R3501, R3503 rotators and DRC-3, DRC-4 controllers.",
                ConflictHolds: new[]
                {
                    "R3500 series: Controller compatibility matrix variance",
                    "DRC-3/DRC-4: Remote-user, software, and Internet-control support claims unverified"
                },
                ProductGroupIds: new[] { "r3500", "r3501", "r3503", "drc-3", "drc-4" },
                ResponsiveHero: new ResponsiveHeroAsset(
                    DesktopImagePath: "/images/products/usap-family-rotator-control-hero-a3s-desktop-1536x576.png",
                    DesktopWidth: 1536,
                    DesktopHeight: 576,
                    MobileImagePath: "/images/products/usap-family-rotator-control-systems-hero-a3s-mobile-768x768.png",
                    MobileWidth: 768,
                    MobileHeight: 768,
                    AltText: "Heavy-duty antenna rotator paired with a tabletop digital controller on an engineering bench.",
                    AssetClassification: "source-guided photorealistic product visualization",
                    ApprovalStatus: "provisional — USAP review pending"
                ),
                SectionEyebrow: "POSITIONING & CONTROL EQUIPMENT",
                SectionHeading: "Antenna Rotator & Controller Models",
                SectionIntro: "Compare USAP antenna rotators and digital controllers using published positioning and control information, drive configurations, and documented system relationships.",
                PageTitle: "Antenna Rotator & Control Systems",
                MetaDescription: "Antenna rotator and controller models, published positioning and control information, and documented system relationships from United States Antenna Products."
            ),
            new(
                Id: "tower-systems-accessories",
                Slug: "tower-systems-accessories",
                Name: "Tower Systems & Accessories",
                DisplayOrder: 6,
                CardEyebrow: "Tower, Mast & Hardware Configurations",
                CardTitle: "Tower Systems & Accessories",
                CardSummary: "Tower, mast, rotation, feedline, and installation configurations for large antenna systems.",
                CardAsset: new CatalogAsset(
                    ImagePath: "/images/products/usap-family-card-tower-systems-nonconfigurational-fallback-v2.png",
                    AltText: "Technical study of tower, mast, rotation, feedline, and installation components.",
                    IsPlaceholder: false),
                HeroAsset: null,
                Route: "/products/tower-systems-accessories",
                CurrentSourceUrls: new[] { "https://www.usantennaproducts.com/products/tower-systems-and-accessories/" },
                LegacySourceUrls: Array.Empty<string>(),
                ApprovalStatus: "ProjectSelectedFallbackPendingApproval",
                SourceNotes: "Covers T-3002 series tower systems and ordering configurations.",
                ConflictHolds: new[] { "T-3002 duplicate public classification consolidated under tower systems" },
                ProductGroupIds: new[] { "t-3002" },
                ResponsiveHero: new ResponsiveHeroAsset(
                    DesktopImagePath: "/images/products/usap-family-tower-systems-hero-a3s-desktop-1536x576.png",
                    DesktopWidth: 1536,
                    DesktopHeight: 576,
                    MobileImagePath: "/images/products/usap-family-tower-systems-hero-a3s-mobile-768x768.png",
                    MobileWidth: 768,
                    MobileHeight: 768,
                    AltText: "Structural lattice tower section with mast hardware and rigging components.",
                    AssetClassification: "source-guided photorealistic product visualization",
                    ApprovalStatus: "provisional — USAP review pending"
                ),
                SectionEyebrow: "SYSTEM MODELS & ACCESSORIES",
                SectionHeading: "T-3002 Tower System Models & Accessories",
                SectionIntro: "Review T-3002 tower-system models and published tower-system configuration, rotation, installation, and accessory information.",
                PageTitle: "T-3002 Tower Systems & Accessories",
                MetaDescription: "T-3002 tower-system models, published structural configurations, rotation specifications, and accessory information from United States Antenna Products."
            )
        };

        var productGroups = new List<ProductGroupRecord>
        {
            // 1. Log Periodic Antennas (5 groups, 8 public named models)
            new(
                Id: "lp-high-power",
                FamilyId: "log-periodic-antennas",
                Name: "High-Power HF Log Periodics",
                DisplayOrder: 1,
                SectionAnchor: "lp-high-power",
                ShortDescription: "Directional HF log-periodic antennas with model-specific frequency coverage from 3 to 40 MHz.",
                ExpandedIntroduction: "The LP-1005, LP-1001, and LP-1002 are directional HF log-periodic arrays for fixed-azimuth or rotatable installations, featuring horizontal polarization and 10–13.5 dBi forward gain.",
                OverviewCharacteristics: new[]
                {
                    new ProductCharacteristic("Polarization", "Horizontal"),
                    new ProductCharacteristic("Forward Gain", "10–13.5 dBi"),
                    new ProductCharacteristic("RF Connector", "1-5/8 in EIA")
                },
                AssociatedAsset: new CatalogAsset(
                    ImagePath: "/images/products/groups/usap-product-group-lp-high-power-source-guided-candidate-a1-v1.png",
                    AltText: "Long-boom high-power log-periodic antenna array in an outdoor installation.",
                    IsPlaceholder: false),
                Models: new[]
                {
                    new ProductModelRecord(
                        ModelCode: "LP-1005",
                        DisplayName: "High-Power HF Log Periodic Antenna",
                        Description: "High-power HF directional array",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Frequency Range", "3.0–30.0 MHz"),
                            new ProductCharacteristic("VSWR", "3:1 (3.0–4.0 MHz), 2:1 (4.0–30.0 MHz)")
                        }
                    ),
                    new ProductModelRecord(
                        ModelCode: "LP-1001",
                        DisplayName: "High-Power HF Log Periodic Antenna",
                        Description: "High-power HF directional array",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Frequency Range", "4.0–30.0 MHz"),
                            new ProductCharacteristic("VSWR", "2:1 nominal")
                        }
                    ),
                    new ProductModelRecord(
                        ModelCode: "LP-1002",
                        DisplayName: "High-Power HF Log Periodic Antenna",
                        Description: "High-power HF directional array",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Frequency Range", "6.0–40.0 MHz"),
                            new ProductCharacteristic("VSWR", "2:1 nominal")
                        }
                    )
                }
            ),
            new(
                Id: "lp-1017",
                FamilyId: "log-periodic-antennas",
                Name: "LP-1017 Log Periodic",
                DisplayOrder: 2,
                SectionAnchor: "lp-1017",
                ShortDescription: "A 6.2–30 MHz commercial HF log-periodic antenna for directional communication applications.",
                ExpandedIntroduction: "LP-1017 is a tower-mounted HF log-periodic array with horizontal polarization. Documented characteristics include 6.2–30 MHz frequency coverage, Type N female connector, 17 elements, and a 37.75-foot boom.",
                AssociatedAsset: new CatalogAsset(
                    ImagePath: "/images/products/groups/usap-product-group-lp-1017-source-guided-candidate-a1-v1.png",
                    AltText: "Tower-mounted long-boom log-periodic antenna array.",
                    IsPlaceholder: false),
                Models: new[]
                {
                    new ProductModelRecord(
                        ModelCode: "LP-1017",
                        DisplayName: "Commercial HF Log Periodic Antenna",
                        Description: "Commercial HF log-periodic directional array",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Frequency Range", "6.2–30.0 MHz"),
                            new ProductCharacteristic("Polarization", "Horizontal"),
                            new ProductCharacteristic("Forward Gain", "8 dBi at 6.2 MHz; 12 dBi at 30 MHz"),
                            new ProductCharacteristic("VSWR", "2.5:1 nominal"),
                            new ProductCharacteristic("RF Connector", "Type N female"),
                            new ProductCharacteristic("Element Count", "17 elements"),
                            new ProductCharacteristic("Boom Length", "37.75 ft (11.5 m)")
                        }
                    )
                }
            ),
            new(
                Id: "lp-1018ba",
                FamilyId: "log-periodic-antennas",
                Name: "LP-1018BA Broadband Log Periodic",
                DisplayOrder: 3,
                SectionAnchor: "lp-1018ba",
                ShortDescription: "A 30–1100 MHz broadband log-periodic antenna for horizontal or vertical orientation, with 8 dB documented gain.",
                ExpandedIntroduction: "LP-1018BA is a broadband VHF/UHF log-periodic array for transmit or receive use. Documented characteristics include 30–1100 MHz coverage, 50-ohm Type N female input, 2:1 nominal VSWR, and horizontal or vertical orientation.",
                AssociatedAsset: new CatalogAsset(
                    ImagePath: "/images/products/groups/usap-product-group-lp-1018ba-source-guided-candidate-a1-v1.png",
                    AltText: "Broadband log-periodic antenna array mounted on a mast.",
                    IsPlaceholder: false),
                Models: new[]
                {
                    new ProductModelRecord(
                        ModelCode: "LP-1018BA",
                        DisplayName: "Broadband Log Periodic Antenna",
                        Description: "Broadband VHF/UHF directional array",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Frequency Range", "30–1100 MHz"),
                            new ProductCharacteristic("Polarization", "Horizontal or vertical as oriented"),
                            new ProductCharacteristic("Forward Gain", "8.0 dB"),
                            new ProductCharacteristic("VSWR", "2.0:1 nominal"),
                            new ProductCharacteristic("Input Impedance", "50 ohms"),
                            new ProductCharacteristic("RF Connector", "Type N female"),
                            new ProductCharacteristic("Boom Length", "16 ft 7 in (5.05 m)")
                        }
                    )
                }
            ),
            new(
                Id: "lp-1019",
                FamilyId: "log-periodic-antennas",
                Name: "LP-1019 Series",
                DisplayOrder: 4,
                SectionAnchor: "lp-1019",
                ShortDescription: "Compact 100–1100 MHz log-periodic antennas in LP-1019BA and LP-1019SS configurations for horizontal or vertical orientation.",
                ExpandedIntroduction: "The LP-1019 series covers compact directional VHF/UHF arrays with a 100–1100 MHz operating range, 8 dB documented gain, and 2:1 nominal VSWR in horizontal or vertical orientation.",
                OverviewCharacteristics: new[]
                {
                    new ProductCharacteristic("Frequency Range", "100–1100 MHz"),
                    new ProductCharacteristic("Forward Gain", "8.0 dB"),
                    new ProductCharacteristic("Polarization", "Horizontal or vertical as oriented")
                },
                DetailedCharacteristics: new[]
                {
                    new ProductCharacteristic("VSWR", "2.0:1 nominal")
                },
                AssociatedAsset: new CatalogAsset(
                    ImagePath: "/images/products/groups/usap-product-group-lp-1019-source-guided-candidate-a1-v2.png",
                    AltText: "Compact log-periodic antenna array mounted on a mast.",
                    IsPlaceholder: false),
                Models: new[]
                {
                    new ProductModelRecord(
                        ModelCode: "LP-1019BA",
                        DisplayName: "Compact Broadband Log Periodic (Aluminum)",
                        Description: "Compact VHF/UHF directional array (aluminum construction)",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Construction", "Aluminum construction"),
                            new ProductCharacteristic("RF Connector", "Type N female")
                        }
                    ),
                    new ProductModelRecord(
                        ModelCode: "LP-1019SS",
                        DisplayName: "Compact Broadband Log Periodic (Stainless Steel)",
                        Description: "Compact VHF/UHF directional array (stainless-steel construction)",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Construction", "Stainless-steel construction")
                        }
                    )
                }
            ),
            new(
                Id: "lp-1112mr",
                FamilyId: "log-periodic-antennas",
                Name: "LP-1112MR Transportable Log Periodic",
                DisplayOrder: 5,
                SectionAnchor: "lp-1112mr",
                ShortDescription: "A transportable, rotatable HF log-periodic antenna system covering 4–30 MHz with a 60-foot support tower.",
                ExpandedIntroduction: "LP-1112MR is a transportable directional HF system with horizontal polarization, manual rotation, and a 60-foot support tower.",
                AssociatedAsset: new CatalogAsset(
                    ImagePath: "/images/products/groups/usap-product-group-lp-1112mr-source-guided-candidate-a1-v2.png",
                    AltText: "Wide transportable log-periodic antenna system deployed on a central field mast.",
                    IsPlaceholder: false),
                Models: new[]
                {
                    new ProductModelRecord(
                        ModelCode: "LP-1112MR",
                        DisplayName: "Transportable Rotatable HF Log Periodic System",
                        Description: "Transportable rotatable HF directional array",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Frequency Range", "4.0–30.0 MHz"),
                            new ProductCharacteristic("System Type", "Transportable HF log-periodic system"),
                            new ProductCharacteristic("Polarization", "Horizontal"),
                            new ProductCharacteristic("Support Structure", "60 ft (18.3 m) tower"),
                            new ProductCharacteristic("Rotation Mode", "Manually operated rotation system"),
                            new ProductCharacteristic("VSWR", "2.5:1 nominal")
                        }
                    )
                }
            ),

            // 2. Portable & Transportable Antenna Systems (3 groups, 6 public named models)
            new(
                Id: "v-4213",
                FamilyId: "portable-transportable-antennas",
                Name: "V-4213 Portable Discone",
                DisplayOrder: 6,
                SectionAnchor: "v-4213",
                ShortDescription: "Portable vertical discone antenna configurations covering 30–88 MHz, represented by equipment package and antenna unit records.",
                ExpandedIntroduction: "The V-4213 group covers omnidirectional vertical discone antennas with 30–88 MHz coverage. Published records distinguish the V-4213AD and V-4213AC configurations.",
                OverviewCharacteristics: new[]
                {
                    new ProductCharacteristic("Frequency Range", "30–88 MHz"),
                    new ProductCharacteristic("Radiation Pattern", "Omnidirectional"),
                    new ProductCharacteristic("Polarization", "Vertical")
                },
                DetailedCharacteristics: new[]
                {
                    new ProductCharacteristic("RF Connector", "Type N")
                },
                AssociatedAsset: new CatalogAsset(
                    ImagePath: "/images/products/groups/usap-product-group-v-4213-source-guided-candidate-a1-v1.png",
                    AltText: "Portable discone antenna system deployed on a sectional field mast.",
                    IsPlaceholder: false),
                Models: new[]
                {
                    new ProductModelRecord(
                        ModelCode: "V-4213AD",
                        DisplayName: "Portable Discone Antenna Equipment Package",
                        Description: "Portable discone equipment-package configuration",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Configuration Type", "Portable discone equipment-package configuration")
                        }
                    ),
                    new ProductModelRecord(
                        ModelCode: "V-4213AC",
                        DisplayName: "Portable Discone Antenna Unit",
                        Description: "Antenna unit record",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Configuration Type", "Antenna unit record")
                        }
                    )
                }
            ),
            new(
                Id: "lp-1402-1403",
                FamilyId: "portable-transportable-antennas",
                Name: "LP-1402 / LP-1403 Transportable Log Periodics",
                DisplayOrder: 7,
                SectionAnchor: "lp-1402-1403",
                ShortDescription: "Transportable VHF log-periodic systems covering 30–76 MHz (LP-1402) and 30–88 MHz (LP-1403), with horizontal or vertical orientation.",
                ExpandedIntroduction: "LP-1402 and LP-1403 are mast-mounted transportable directional arrays with 4.5 dBi documented gain, BNC female input, and 2:1 nominal VSWR in horizontal or vertical orientation.",
                OverviewCharacteristics: new[]
                {
                    new ProductCharacteristic("Polarization", "Horizontal or vertical"),
                    new ProductCharacteristic("Forward Gain", "4.5 dBi"),
                    new ProductCharacteristic("VSWR", "2:1 nominal")
                },
                DetailedCharacteristics: new[]
                {
                    new ProductCharacteristic("RF Connector", "BNC female")
                },
                AssociatedAsset: new CatalogAsset(
                    ImagePath: "/images/products/groups/usap-product-group-lp-1402-1403-source-guided-candidate-a1-v1.png",
                    AltText: "Transportable mast-mounted log-periodic antenna system in a field setting.",
                    IsPlaceholder: false),
                Models: new[]
                {
                    new ProductModelRecord(
                        ModelCode: "LP-1402",
                        DisplayName: "Transportable VHF Log Periodic Antenna (30–76 MHz)",
                        Description: "Transportable VHF log-periodic antenna (7 elements)",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Frequency Range", "30.0–76.0 MHz"),
                            new ProductCharacteristic("Element Count", "7 elements")
                        }
                    ),
                    new ProductModelRecord(
                        ModelCode: "LP-1403",
                        DisplayName: "Transportable VHF Log Periodic Antenna (30–88 MHz)",
                        Description: "Transportable VHF log-periodic antenna (8 elements)",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Frequency Range", "30.0–88.0 MHz"),
                            new ProductCharacteristic("Element Count", "8 elements")
                        }
                    )
                }
            ),
            new(
                Id: "1910",
                FamilyId: "portable-transportable-antennas",
                Name: "1910 Tactical Dipoles",
                DisplayOrder: 8,
                SectionAnchor: "1910",
                ShortDescription: "Transportable HF dipole antenna configurations covering 2–30 MHz with horizontal polarization.",
                ExpandedIntroduction: "The 1910AA and 1910BA are broadband center-fed transportable HF dipole systems with horizontal polarization, 50-ohm Type N inputs, and 2.5:1 nominal VSWR in the 2024 technical documentation.",
                OverviewCharacteristics: new[]
                {
                    new ProductCharacteristic("Frequency Range", "2.0–30.0 MHz"),
                    new ProductCharacteristic("Polarization", "Horizontal"),
                    new ProductCharacteristic("Input Impedance", "50 ohms")
                },
                DetailedCharacteristics: new[]
                {
                    new ProductCharacteristic("RF Connector", "Type N"),
                    new ProductCharacteristic("VSWR", "2.5:1 nominal")
                },
                AssociatedAsset: new CatalogAsset(
                    ImagePath: "/images/products/groups/usap-product-group-1910-source-guided-candidate-a1-v1.png",
                    AltText: "Field-deployed HF wire dipole system supported by a central mast.",
                    IsPlaceholder: false),
                Models: new[]
                {
                    new ProductModelRecord(
                        ModelCode: "1910AA",
                        DisplayName: "Transportable HF Dipole Configuration (1910AA)",
                        Description: "Transportable HF dipole configuration (Model 1910AA)",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Configuration Type", "Transportable HF dipole configuration")
                        }
                    ),
                    new ProductModelRecord(
                        ModelCode: "1910BA",
                        DisplayName: "Transportable HF Dipole Configuration (1910BA)",
                        Description: "Transportable HF dipole configuration (Model 1910BA)",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Configuration Type", "Transportable HF dipole configuration")
                        }
                    )
                }
            ),

            // 3. Aperiodic Loop Antennas (1 group, 0 named models, 1 unnamed group record)
            new(
                Id: "aperiodic",
                FamilyId: "aperiodic-loop-antennas",
                Name: "USAP Aperiodic Loop Antenna",
                DisplayOrder: 9,
                SectionAnchor: "aperiodic",
                ShortDescription: "A receive-oriented directional HF loop-array concept covering 2–32 MHz in fixed and transportable configurations.",
                ExpandedIntroduction: "USAP's aperiodic loop documentation describes untuned balanced loop elements arranged as an end-fire receive array covering 2–32 MHz. Each documented loop element is an aluminum-alloy assembly for tripod or post mounting. No published model number is provided for this antenna system.",
                OverviewCharacteristics: new[]
                {
                    new ProductCharacteristic("Frequency Range", "2.0–32.0 MHz"),
                    new ProductCharacteristic("System Type", "Receive-oriented directional loop array"),
                    new ProductCharacteristic("Array Element", "Welded aluminum-alloy loop element (approx. 10 lb)")
                },
                DetailedCharacteristics: new[]
                {
                    new ProductCharacteristic("Polarization", "Vertical loop plane"),
                    new ProductCharacteristic("Mounting", "Tripod or post mount")
                },
                AssociatedAsset: new CatalogAsset(
                    ImagePath: "/images/products/groups/usap-product-group-aperiodic-single-loop-source-guided-candidate-a1-v1.png",
                    AltText: "One aperiodic loop and preamplifier element on a transportable tripod.",
                    IsPlaceholder: false),
                Models: Array.Empty<ProductModelRecord>(),
                HasPublishedModelNumber: false,
                ModelNote: "No published model number is provided for this antenna system."
            ),

            // 4. NVIS Antennas (1 group, 6 public named configurations: 3 primary + 3 low-power)
            new(
                Id: "1942",
                FamilyId: "nvis-antennas",
                Name: "1942 NVIS Series",
                DisplayOrder: 10,
                SectionAnchor: "1942",
                ShortDescription: "A 2–30 MHz NVIS antenna series with rooftop, transportable, and ground-mount configurations for regional HF communications.",
                ExpandedIntroduction: "The 1942 series provides near-vertical incidence skywave (NVIS) antenna configurations covering 2–30 MHz with a 20-foot mast, stainless-steel elements, and ground radials.",
                OverviewCharacteristics: new[]
                {
                    new ProductCharacteristic("Frequency Range", "2.0–30.0 MHz"),
                    new ProductCharacteristic("Mast Height", "20 ft (6.1 m) mast"),
                    new ProductCharacteristic("RF Connector", "Type N")
                },
                AssociatedAsset: new CatalogAsset(
                    ImagePath: "/images/products/groups/usap-product-group-1942-source-guided-candidate-a1-v1.png",
                    AltText: "Field-deployed NVIS antenna system with a central mast and broad low wire footprint.",
                    IsPlaceholder: false),
                Models: new[]
                {
                    new ProductModelRecord(
                        ModelCode: "1942-RT",
                        DisplayName: "1942 Roof-Top Configuration",
                        Description: "Rooftop mounting configuration",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Configuration", "Rooftop mount"),
                            new ProductCharacteristic("VSWR", "2.5:1 nominal")
                        }
                    ),
                    new ProductModelRecord(
                        ModelCode: "1942-TA",
                        DisplayName: "1942 Transportable Configuration",
                        Description: "Transportable field configuration",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Configuration", "Transportable field mount"),
                            new ProductCharacteristic("VSWR", "1.8:1 nominal")
                        }
                    ),
                    new ProductModelRecord(
                        ModelCode: "1942-GM",
                        DisplayName: "1942 Ground-Mount Configuration",
                        Description: "Ground-mount configuration",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Configuration", "Ground mount"),
                            new ProductCharacteristic("Deployment Area", "50 ft x 50 ft (15.25 m square)"),
                            new ProductCharacteristic("VSWR", "1.8:1 nominal")
                        }
                    ),
                    new ProductModelRecord(
                        ModelCode: "1942-RT-LP",
                        DisplayName: "1942 Low-Power Roof-Top Configuration",
                        Description: "Low-power rooftop configuration",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Configuration", "Low-power rooftop mount")
                        }
                    ),
                    new ProductModelRecord(
                        ModelCode: "1942-TA-LP",
                        DisplayName: "1942 Low-Power Transportable Configuration",
                        Description: "Low-power transportable field configuration",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Configuration", "Low-power transportable field mount")
                        }
                    ),
                    new ProductModelRecord(
                        ModelCode: "1942-GM-LP",
                        DisplayName: "1942 Low-Power Ground-Mount Configuration",
                        Description: "Low-power ground-mount configuration",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Configuration", "Low-power ground mount")
                        }
                    )
                }
            ),

            // 5. Antenna Rotator & Control Systems (5 groups, 5 public named models, 5 associated assets)
            new(
                Id: "r3500",
                FamilyId: "antenna-rotator-control-systems",
                Name: "R3500 Heavy Duty Antenna Rotator",
                DisplayOrder: 11,
                SectionAnchor: "r3500",
                ShortDescription: "A mechanical antenna rotator with continuous or centered rotation options for directional antenna positioning.",
                ExpandedIntroduction: "R3500 is a heavy-duty mechanical rotator for antenna positioning. Documented characteristics include 1.25 RPM maximum rotation speed, ±1° position accuracy, 115 or 230 VAC operation, and an 18 × 23 × 7 inch housing.",
                AssociatedAsset: new CatalogAsset(
                    ImagePath: "/images/products/usap-r3500-drc4-source-guided-relationship-a3s-v1.png",
                    AltText: "Heavy-duty antenna rotator and tabletop controller shown together in a technical studio scene.",
                    IsPlaceholder: false),
                Models: new[]
                {
                    new ProductModelRecord(
                        ModelCode: "R3500",
                        DisplayName: "Heavy-Duty Antenna Rotator",
                        Description: "Heavy-duty mechanical antenna rotator",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Drive Type", "Worm-gear drive"),
                            new ProductCharacteristic("Rotation Speed", "Up to 1.25 RPM"),
                            new ProductCharacteristic("Position Accuracy", "±1°"),
                            new ProductCharacteristic("Housing Dimensions", "18 in W × 23 in L × 7 in H (46 × 58 × 18 cm)"),
                            new ProductCharacteristic("Mast Acceptance", "Accepts 2–3 in mast shaft and up to 1-5/8 in transmission line"),
                            new ProductCharacteristic("Operating Voltage", "115 / 230 VAC")
                        }
                    )
                }
            ),
            new(
                Id: "r3501",
                FamilyId: "antenna-rotator-control-systems",
                Name: "R3501 Universal Rotator System",
                DisplayOrder: 12,
                SectionAnchor: "r3501",
                ShortDescription: "A universal antenna rotator system for directional antenna positioning.",
                ExpandedIntroduction: "R3501 is a mechanical rotator adaptable to top, side, tower, or pole mounting. Documented characteristics include 1 RPM rotation speed, ±1° position accuracy, 115/230 VAC operation, and a 19 × 24 × 14.5 inch housing.",
                AssociatedAsset: new CatalogAsset(
                    ImagePath: "/images/products/groups/usap-product-group-r3501-source-guided-candidate-a1-v1.png",
                    AltText: "Source-guided visualization of an R3501 open-frame antenna rotator assembly.",
                    IsPlaceholder: false),
                Models: new[]
                {
                    new ProductModelRecord(
                        ModelCode: "R3501",
                        DisplayName: "Universal Antenna Rotator",
                        Description: "Universal mechanical antenna rotator",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Equipment Type", "Mechanical antenna rotator"),
                            new ProductCharacteristic("Drive Type", "Gear-and-chain drive"),
                            new ProductCharacteristic("Rotation Speed", "1 RPM"),
                            new ProductCharacteristic("Position Accuracy", "±1°"),
                            new ProductCharacteristic("Housing Dimensions", "19 in W × 24 in L × 14.5 in H"),
                            new ProductCharacteristic("Mast Acceptance", "Accepts 1-1/4–2-1/2 in mast shaft and up to 1-5/8 in transmission line"),
                            new ProductCharacteristic("Operating Voltage", "115 / 230 VAC")
                        }
                    )
                }
            ),
            new(
                Id: "r3503",
                FamilyId: "antenna-rotator-control-systems",
                Name: "R3503 Heavy Duty Rotating System",
                DisplayOrder: 13,
                SectionAnchor: "r3503",
                ShortDescription: "A heavy-duty rotating system engineered for large communication antenna arrays.",
                ExpandedIntroduction: "R3503 is a heavy-duty rotating system engineered for large communication antenna arrays. Documented specifications include up to 1.25 RPM speed, ±2° position accuracy, and 115/230 VAC operation.",
                AssociatedAsset: new CatalogAsset(
                    ImagePath: "/images/products/groups/usap-product-group-r3503-source-guided-candidate-a1-v1.png",
                    AltText: "Source-guided visualization of an R3503 heavy-duty open-frame antenna rotator assembly.",
                    IsPlaceholder: false),
                Models: new[]
                {
                    new ProductModelRecord(
                        ModelCode: "R3503",
                        DisplayName: "Heavy-Duty Antenna Rotating System",
                        Description: "Heavy-duty antenna rotating system",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Equipment Type", "Heavy-duty antenna rotating system"),
                            new ProductCharacteristic("Drive Type", "Gear-and-chain drive"),
                            new ProductCharacteristic("Rotation Speed", "Up to 1.25 RPM"),
                            new ProductCharacteristic("Position Accuracy", "±2°"),
                            new ProductCharacteristic("Operating Voltage", "115 / 230 VAC"),
                            new ProductCharacteristic("Shipping Weight", "Approx. 1,260 lb")
                        }
                    )
                }
            ),
            new(
                Id: "drc-3",
                FamilyId: "antenna-rotator-control-systems",
                Name: "DRC-3 Digital Rotator Controller",
                DisplayOrder: 14,
                SectionAnchor: "drc-3",
                ShortDescription: "An industrial antenna-rotator control enclosure with local manual controls, digital heading display, and continuous-rotation capability.",
                ExpandedIntroduction: "DRC-3 is an industrial antenna-rotator control enclosure featuring local manual controls and digital heading display.",
                AssociatedAsset: new CatalogAsset(
                    ImagePath: "/images/products/usap-drc3-source-guided-product-visual-a3s-v1.png",
                    AltText: "Industrial antenna-rotator control enclosure with display and front-panel controls.",
                    IsPlaceholder: false),
                Models: new[]
                {
                    new ProductModelRecord(
                        ModelCode: "DRC-3",
                        DisplayName: "Industrial Rotator Control Enclosure",
                        Description: "Industrial antenna-rotator control enclosure",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Equipment Classification", "Industrial antenna-rotator control enclosure"),
                            new ProductCharacteristic("Control Features", "Local clockwise and counterclockwise manual controls, digital heading display"),
                            new ProductCharacteristic("Rotation Functions", "360-degree continuous rotation")
                        }
                    )
                }
            ),
            new(
                Id: "drc-4",
                FamilyId: "antenna-rotator-control-systems",
                Name: "DRC-4 Digital Rotator Controller",
                DisplayOrder: 15,
                SectionAnchor: "drc-4",
                ShortDescription: "A horizontal tabletop digital antenna-rotator controller with digital display, rotary dial, and front controls.",
                ExpandedIntroduction: "DRC-4 is a horizontal tabletop digital antenna-rotator controller featuring front-panel controls and digital heading display.",
                AssociatedAsset: new CatalogAsset(
                    ImagePath: "/images/products/usap-drc4-source-guided-product-visual-a3s-v1.png",
                    AltText: "Horizontal tabletop antenna-rotator controller with digital display, rotary dial, and front controls.",
                    IsPlaceholder: false),
                Models: new[]
                {
                    new ProductModelRecord(
                        ModelCode: "DRC-4",
                        DisplayName: "Tabletop Digital Rotator Controller",
                        Description: "Horizontal tabletop digital rotator controller",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Equipment Classification", "Horizontal tabletop digital controller"),
                            new ProductCharacteristic("Control Features", "Digital heading display, rotary dial, front-panel controls"),
                            new ProductCharacteristic("Rotation Functions", "360-degree continuous-rotation control")
                        }
                    )
                }
            ),

            // 6. Tower Systems & Accessories (1 group, 5 public named models, 1 associated asset)
            new(
                Id: "t-3002",
                FamilyId: "tower-systems-accessories",
                Name: "T-3002 RLPA Tower System",
                DisplayOrder: 16,
                SectionAnchor: "t-3002",
                ShortDescription: "A rotatable HF log-periodic tower system with 100-foot and 80-foot dual-guyed or self-supporting configurations.",
                ExpandedIntroduction: "T-3002 is a tower-and-rotating-mast system for large HF log-periodic installations. Documented specifications include dual-guyed and self-supporting structural forms, 100-foot and 80-foot heights, and up to 1.25 RPM rotation.",
                OverviewCharacteristics: new[]
                {
                    new ProductCharacteristic("Rotation Speed", "Up to 1.25 RPM"),
                    new ProductCharacteristic("Operating Voltage", "115 / 230 VAC, 50/60 Hz")
                },
                AssociatedAsset: new CatalogAsset(
                    ImagePath: "/images/products/groups/usap-product-group-t-3002-source-guided-candidate-a1-v1.png",
                    AltText: "Tower-system components including lattice supports, a rotating mast, and a directional antenna.",
                    IsPlaceholder: false),
                Models: new[]
                {
                    new ProductModelRecord(
                        ModelCode: "T-3002",
                        DisplayName: "RLPA Tower System Parent Line",
                        Description: "Rotatable log periodic antenna tower system parent line",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("System Type", "Parent tower-system line"),
                            new ProductCharacteristic("Available Heights", "80 ft or 100 ft overall"),
                            new ProductCharacteristic("Structural Options", "Dual-guyed (0.31-acre installation area) or self-supporting (under 100 sq ft)")
                        }
                    ),
                    new ProductModelRecord(
                        ModelCode: "3002FA",
                        DisplayName: "100 ft Dual-Guyed Configuration",
                        Description: "100 ft dual-guyed tower configuration",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Configuration Type", "Dual-guyed rotating-mast system"),
                            new ProductCharacteristic("Overall Height", "100 ft (30.4 m)"),
                            new ProductCharacteristic("Installation Footprint", "Approx. 0.31 acres")
                        }
                    ),
                    new ProductModelRecord(
                        ModelCode: "3002FB",
                        DisplayName: "80 ft Dual-Guyed Configuration",
                        Description: "80 ft dual-guyed tower configuration",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Configuration Type", "Dual-guyed rotating-mast system"),
                            new ProductCharacteristic("Overall Height", "80 ft (24.4 m) rotating mast (60 ft tower sections)"),
                            new ProductCharacteristic("Installation Footprint", "Approx. 0.31 acres")
                        }
                    ),
                    new ProductModelRecord(
                        ModelCode: "3002SS",
                        DisplayName: "100 ft Self-Supporting Configuration",
                        Description: "100 ft self-supporting tower configuration",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Configuration Type", "Self-supporting rotating-mast system"),
                            new ProductCharacteristic("Overall Height", "100 ft (30.4 m)"),
                            new ProductCharacteristic("Installation Footprint", "Under 100 sq ft")
                        }
                    ),
                    new ProductModelRecord(
                        ModelCode: "3002SS-80",
                        DisplayName: "80 ft Self-Supporting Configuration",
                        Description: "80 ft self-supporting tower configuration",
                        Characteristics: new[]
                        {
                            new ProductCharacteristic("Configuration Type", "Self-supporting rotating-mast system"),
                            new ProductCharacteristic("Overall Height", "80 ft (24.4 m) rotating mast (60 ft tower sections)"),
                            new ProductCharacteristic("Installation Footprint", "Under 100 sq ft")
                        }
                    )
                }
            )
        };

        // 6 interim technical documents (not a permanent ceiling; R2 identified 17 canonical migration documents for later technical documentation scope)
        var resources = new List<ProductResourceRecord>
        {
            new("doc-lp-high-power", "lp-high-power", "LP-1001, LP-1002, LP-1005 High-Power Log Periodic Data Sheet", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/05/1001_1002_1005-data-sheet.pdf"),
            new("doc-lp-1018ba", "lp-1018ba", "LP-1018BA Broadband Log Periodic Data Sheet", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/06/LP_1018BA_data_sheet_revised.pdf"),
            new("doc-lp-1019", "lp-1019", "LP-1019 / LP-1019SS Data Sheet", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/06/1019-and-1019-ss-data-sheet.pdf"),
            new("doc-1910-2024", "1910", "1910AA / 1910BA Data Sheet (February 2024 Revised)", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2024/02/1910AA_1910BA-revised-1.pdf"),
            new("doc-aperiodic", "aperiodic", "USAP Aperiodic Loop Antenna Data Sheet", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/06/USAP-Aperiodic-Loop-Antenna-data-sheet.pdf"),
            new("doc-t-3002-oct2016", "t-3002", "T-3002 RLPA Tower System Data Sheet (October 2016)", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/10/T3002_data_sheet.pdf")
        };

        return (families, productGroups, resources);
    }
}
