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

        _families = families.OrderBy(f => f.DisplayOrder).ToList();
        _productGroups = productGroups.OrderBy(p => p.DisplayOrder).ToList();
        _resources = resources.ToList();

        _familiesById = _families.ToDictionary(f => f.Id, StringComparer.OrdinalIgnoreCase);
        _familiesBySlug = _families.ToDictionary(f => f.Slug, StringComparer.OrdinalIgnoreCase);
        _productGroupsById = _productGroups.ToDictionary(p => p.Id, StringComparer.OrdinalIgnoreCase);

        _productGroupsByFamilyId = _families.ToDictionary(
            f => f.Id,
            _ => new List<ProductGroupRecord>(),
            StringComparer.OrdinalIgnoreCase);

        foreach (var group in _productGroups)
        {
            if (_productGroupsByFamilyId.TryGetValue(group.FamilyId, out var list))
            {
                list.Add(group);
            }
        }
    }

    public IReadOnlyList<ProductFamilyRecord> GetFamilies() => _families;

    public ProductFamilyRecord? GetFamilyBySlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return null;
        }

        return _familiesBySlug.GetValueOrDefault(slug);
    }

    public ProductFamilyRecord? GetFamilyById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        return _familiesById.GetValueOrDefault(id);
    }

    public IReadOnlyList<ProductGroupRecord> GetProductGroupsByFamily(string familyId)
    {
        if (string.IsNullOrWhiteSpace(familyId))
        {
            return Array.Empty<ProductGroupRecord>();
        }

        return _productGroupsByFamilyId.TryGetValue(familyId, out var list)
            ? list
            : Array.Empty<ProductGroupRecord>();
    }

    public ProductGroupRecord? GetProductGroupById(string id)
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

    private static readonly IReadOnlyList<string> FeaturedFamilyIds = new[]
    {
        "log-periodic-antennas",
        "portable-transportable-antennas",
        "antenna-rotator-control-systems",
        "tower-systems-accessories"
    };

    public IReadOnlyList<ProductFamilyRecord> GetFeaturedFamilies() =>
        FeaturedFamilyIds.Select(id => _familiesById[id]).ToList();

    public static void ValidateCatalog(
        IReadOnlyList<ProductFamilyRecord> families,
        IReadOnlyList<ProductGroupRecord> productGroups,
        IReadOnlyList<ProductResourceRecord> resources)
    {
        // 1. Family count must be exactly 6
        if (families.Count != 6)
        {
            throw new InvalidOperationException($"Catalog must define exactly 6 families, found {families.Count}.");
        }

        // 2. Family IDs and Slugs must be unique
        var familyIdSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var familySlugSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var family in families)
        {
            if (!familyIdSet.Add(family.Id))
            {
                throw new InvalidOperationException($"Duplicate family ID detected: '{family.Id}'.");
            }

            if (!familySlugSet.Add(family.Slug))
            {
                throw new InvalidOperationException($"Duplicate family slug detected: '{family.Slug}'.");
            }

            if (string.IsNullOrWhiteSpace(family.Slug) || !SlugRegex.IsMatch(family.Slug))
            {
                throw new InvalidOperationException($"Family '{family.Id}' has an invalid route slug '{family.Slug}'.");
            }

            if (string.IsNullOrWhiteSpace(family.CardTitle))
            {
                throw new InvalidOperationException($"Family '{family.Id}' is missing required CardTitle.");
            }

            if (string.IsNullOrWhiteSpace(family.CardSummary))
            {
                throw new InvalidOperationException($"Family '{family.Id}' is missing required CardSummary.");
            }

            if (string.IsNullOrWhiteSpace(family.CardEyebrow))
            {
                throw new InvalidOperationException($"Family '{family.Id}' is missing required CardEyebrow.");
            }

            if (family.CardAsset == null)
            {
                throw new InvalidOperationException($"Family '{family.Id}' is missing required CardAsset.");
            }

            if (!family.CardAsset.IsPlaceholder)
            {
                if (string.IsNullOrWhiteSpace(family.CardAsset.ImagePath))
                {
                    throw new InvalidOperationException($"Family '{family.Id}' has non-placeholder CardAsset without ImagePath.");
                }

                if (string.IsNullOrWhiteSpace(family.CardAsset.AltText))
                {
                    throw new InvalidOperationException($"Family '{family.Id}' has non-placeholder CardAsset without AltText.");
                }
            }

            if (family.ResponsiveHero == null)
            {
                throw new InvalidOperationException($"Family '{family.Id}' is missing required ResponsiveHero.");
            }

            if (string.IsNullOrWhiteSpace(family.ResponsiveHero.DesktopImagePath) || !family.ResponsiveHero.DesktopImagePath.StartsWith("/images/products/"))
            {
                throw new InvalidOperationException($"Family '{family.Id}' has invalid DesktopImagePath '{family.ResponsiveHero.DesktopImagePath}'.");
            }

            if (family.ResponsiveHero.DesktopWidth != 1536 || family.ResponsiveHero.DesktopHeight != 576)
            {
                throw new InvalidOperationException($"Family '{family.Id}' has invalid desktop dimensions ({family.ResponsiveHero.DesktopWidth}x{family.ResponsiveHero.DesktopHeight}), expected 1536x576.");
            }

            if (string.IsNullOrWhiteSpace(family.ResponsiveHero.MobileImagePath) || !family.ResponsiveHero.MobileImagePath.StartsWith("/images/products/"))
            {
                throw new InvalidOperationException($"Family '{family.Id}' has invalid MobileImagePath '{family.ResponsiveHero.MobileImagePath}'.");
            }

            if (family.ResponsiveHero.MobileWidth != 768 || family.ResponsiveHero.MobileHeight != 768)
            {
                throw new InvalidOperationException($"Family '{family.Id}' has invalid mobile dimensions ({family.ResponsiveHero.MobileWidth}x{family.ResponsiveHero.MobileHeight}), expected 768x768.");
            }

            if (string.IsNullOrWhiteSpace(family.ResponsiveHero.AltText))
            {
                throw new InvalidOperationException($"Family '{family.Id}' has empty ResponsiveHero AltText.");
            }

            if (string.IsNullOrWhiteSpace(family.ResponsiveHero.AssetClassification))
            {
                throw new InvalidOperationException($"Family '{family.Id}' has empty ResponsiveHero AssetClassification.");
            }

            if (string.IsNullOrWhiteSpace(family.ResponsiveHero.ApprovalStatus))
            {
                throw new InvalidOperationException($"Family '{family.Id}' has empty ResponsiveHero ApprovalStatus.");
            }
        }

        // 3. Display order must be 1..6 without duplicates or gaps
        var orderedFamilies = families.OrderBy(f => f.DisplayOrder).ToList();
        for (var i = 0; i < orderedFamilies.Count; i++)
        {
            var expectedOrder = i + 1;
            if (orderedFamilies[i].DisplayOrder != expectedOrder)
            {
                throw new InvalidOperationException(
                    $"Family display orders are invalid. Expected {expectedOrder}, found {orderedFamilies[i].DisplayOrder} on family '{orderedFamilies[i].Id}'.");
            }

            if (!string.Equals(orderedFamilies[i].Id, CanonicalFamilyOrder[i], StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"Family at position {expectedOrder} must be '{CanonicalFamilyOrder[i]}', but found '{orderedFamilies[i].Id}'.");
            }
        }

        // 4. Product group count must be exactly 16 distinct groups
        if (productGroups.Count != 16)
        {
            throw new InvalidOperationException($"Catalog must define exactly 16 distinct product groups, found {productGroups.Count}.");
        }

        var productGroupMap = new Dictionary<string, ProductGroupRecord>(StringComparer.OrdinalIgnoreCase);
        var primaryFamilyByGroup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var group in productGroups)
        {
            if (!familyIdSet.Contains(group.FamilyId))
            {
                throw new InvalidOperationException($"Product group '{group.Id}' references unknown family '{group.FamilyId}'.");
            }

            if (!productGroupMap.TryAdd(group.Id, group))
            {
                throw new InvalidOperationException($"Duplicate product group ID detected: '{group.Id}'.");
            }

            if (primaryFamilyByGroup.TryGetValue(group.Id, out var existingFamily))
            {
                throw new InvalidOperationException(
                    $"Product group '{group.Id}' belongs to multiple primary families: '{existingFamily}' and '{group.FamilyId}'.");
            }

            primaryFamilyByGroup[group.Id] = group.FamilyId;
        }

        // 5. Per-family group distribution must match canonical 5 / 3 / 1 / 1 / 5 / 1 mapping exactly
        foreach (var family in orderedFamilies)
        {
            if (!CanonicalFamilyGroupDistribution.TryGetValue(family.Id, out var expectedGroupIds))
            {
                throw new InvalidOperationException($"No canonical group distribution registered for family '{family.Id}'.");
            }

            var actualGroupIds = family.ProductGroupIds;
            if (actualGroupIds.Count != expectedGroupIds.Count)
            {
                throw new InvalidOperationException(
                    $"Family '{family.Id}' must contain exactly {expectedGroupIds.Count} groups, but contains {actualGroupIds.Count}.");
            }

            for (var g = 0; g < expectedGroupIds.Count; g++)
            {
                if (!string.Equals(actualGroupIds[g], expectedGroupIds[g], StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        $"Family '{family.Id}' group mismatch at index {g}: expected '{expectedGroupIds[g]}', found '{actualGroupIds[g]}'.");
                }
            }

            // Verify groups assigned to this family in productGroups list match family.ProductGroupIds
            var matchingGroups = productGroups
                .Where(p => string.Equals(p.FamilyId, family.Id, StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p.DisplayOrder)
                .Select(p => p.Id)
                .ToList();

            if (matchingGroups.Count != expectedGroupIds.Count)
            {
                throw new InvalidOperationException(
                    $"Product groups assigned to family '{family.Id}' count {matchingGroups.Count}, expected {expectedGroupIds.Count}.");
            }

            for (var g = 0; g < expectedGroupIds.Count; g++)
            {
                if (!string.Equals(matchingGroups[g], expectedGroupIds[g], StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        $"Product groups assigned to family '{family.Id}' at index {g} is '{matchingGroups[g]}', expected '{expectedGroupIds[g]}'.");
                }
            }
        }

        // 6. Explicit canonical business assertions
        // LP-1112MR belongs primarily to Log Periodic
        if (!string.Equals(productGroupMap["lp-1112mr"].FamilyId, "log-periodic-antennas", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("LP-1112MR must belong primarily to Log Periodic Antennas ('log-periodic-antennas').");
        }

        // LP-1402-1403 belongs to Portable & Transportable and NOT Log Periodic
        if (!string.Equals(productGroupMap["lp-1402-1403"].FamilyId, "portable-transportable-antennas", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("LP-1402-1403 must belong to Portable & Transportable Antenna Systems ('portable-transportable-antennas'), not Log Periodic.");
        }

        if (CanonicalFamilyGroupDistribution["log-periodic-antennas"].Contains("lp-1402-1403", StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("LP-1402-1403 must not be assigned to Log Periodic Antennas.");
        }

        // 1910 belongs to Portable & Transportable
        if (!string.Equals(productGroupMap["1910"].FamilyId, "portable-transportable-antennas", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("1910 Tactical Dipoles must belong to Portable & Transportable Antenna Systems ('portable-transportable-antennas').");
        }

        // APERIODIC is the only Aperiodic Loop group
        if (!string.Equals(productGroupMap["aperiodic"].FamilyId, "aperiodic-loop-antennas", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("APERIODIC must belong to Aperiodic Loop Antennas ('aperiodic-loop-antennas').");
        }

        var aperiodicGroups = productGroups.Where(p => string.Equals(p.FamilyId, "aperiodic-loop-antennas", StringComparison.OrdinalIgnoreCase)).ToList();
        if (aperiodicGroups.Count != 1 || !string.Equals(aperiodicGroups[0].Id, "aperiodic", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("APERIODIC must be the sole product group under Aperiodic Loop Antennas.");
        }

        // All five R3500/R3501/R3503/DRC-3/DRC-4 groups belong to Rotator & Control
        var expectedRotatorGroups = new[] { "r3500", "r3501", "r3503", "drc-3", "drc-4" };
        foreach (var rotId in expectedRotatorGroups)
        {
            if (!productGroupMap.TryGetValue(rotId, out var rotGroup) ||
                !string.Equals(rotGroup.FamilyId, "antenna-rotator-control-systems", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Group '{rotId}' must belong to Antenna Rotator & Control Systems ('antenna-rotator-control-systems').");
            }
        }

        // T-3002 is the only Tower group and its configurations remain children
        var towerGroups = productGroups.Where(p => string.Equals(p.FamilyId, "tower-systems-accessories", StringComparison.OrdinalIgnoreCase)).ToList();
        if (towerGroups.Count != 1 || !string.Equals(towerGroups[0].Id, "t-3002", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("T-3002 must be the sole product group under Tower Systems & Accessories ('tower-systems-accessories').");
        }

        var t3002 = productGroupMap["t-3002"];
        var expectedT3002Models = new[] { "T-3002", "3002FA", "3002FB", "3002SS", "3002SS-80" };
        var actualT3002Models = t3002.Models.Select(m => m.ModelCode).ToList();
        foreach (var expectedModel in expectedT3002Models)
        {
            if (!actualT3002Models.Contains(expectedModel, StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"T-3002 group must contain configuration child model '{expectedModel}'.");
            }
        }

        // 7. Model and configuration inventory checks: exactly 30 named models + 1 unnamed aperiodic loop line
        var namedModelCount = 0;
        var unnamedModelCount = 0;

        foreach (var group in productGroups)
        {
            foreach (var model in group.Models)
            {
                if (string.IsNullOrWhiteSpace(model.ModelCode))
                {
                    unnamedModelCount++;
                }
                else
                {
                    namedModelCount++;
                }
            }
        }

        if (namedModelCount != 30)
        {
            throw new InvalidOperationException($"Catalog must contain exactly 30 named model/configuration codes, found {namedModelCount}.");
        }

        if (unnamedModelCount != 1)
        {
            throw new InvalidOperationException($"Catalog must contain exactly 1 unnamed Aperiodic Loop product-line record, found {unnamedModelCount}.");
        }

        // 8. Resource references an existing canonical product group
        foreach (var resource in resources)
        {
            if (!productGroupMap.ContainsKey(resource.ProductGroupId))
            {
                throw new InvalidOperationException($"Resource '{resource.Id}' references unknown product group '{resource.ProductGroupId}'.");
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
                SourceNotes: "Primary category on public site. LP-1112MR is primarily classified here; transportable nature noted in group attributes. A2F candidate visual pending USAP review.",
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
                )
            ),
            new(
                Id: "portable-transportable-antennas",
                Slug: "portable-transportable-antennas",
                Name: "Portable & Transportable Antenna Systems",
                DisplayOrder: 2,
                CardEyebrow: "Rapid Deployment / HF & VHF",
                CardTitle: "Portable & Transportable Antenna Systems",
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
                SourceNotes: "Approved taxonomy consolidation combining current Portable Discone Antenna System and Transportable HF Antennas categories. A2F placeholder pending USAP review.",
                ConflictHolds: new[]
                {
                    "V-4213: Model designation vs wind rating inconsistency (60 vs 81 mph) across datasheets",
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
                    AltText: "Field-deployed tactical antenna system on a tripod mast.",
                    AssetClassification: "source-guided photorealistic product visualization",
                    ApprovalStatus: "provisional — USAP review pending"
                )
            ),
            new(
                Id: "aperiodic-loop-antennas",
                Slug: "aperiodic-loop-antennas",
                Name: "Aperiodic Loop Antennas",
                DisplayOrder: 3,
                CardEyebrow: "Directional Receiving Arrays",
                CardTitle: "Aperiodic Loop Antennas",
                CardSummary: "Receive-focused fixed and transportable loop-array solutions.",
                CardAsset: new CatalogAsset(
                    ImagePath: "/images/products/usap-family-card-aperiodic-loop-element-v1.png",
                    AltText: "Aperiodic loop antenna element on a transportable tripod.",
                    IsPlaceholder: false),
                HeroAsset: null,
                Route: "/products/aperiodic-loop-antennas",
                CurrentSourceUrls: new[] { "https://www.usantennaproducts.com/products/aperiodic-loop-antennas/" },
                LegacySourceUrls: Array.Empty<string>(),
                ApprovalStatus: "ProjectSelectedReferenceCandidatePendingApproval",
                SourceNotes: "Public site category includes unnamed aperiodic loop antenna. A2F single-element reference candidate pending USAP review.",
                ConflictHolds: new[] { "No published model number; configuration options subject to USAP confirmation" },
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
                )
            ),
            new(
                Id: "nvis-antennas",
                Slug: "nvis-antennas",
                Name: "NVIS Antennas",
                DisplayOrder: 4,
                CardEyebrow: "Near Vertical Incidence Skywave",
                CardTitle: "NVIS Antennas",
                CardSummary: "Near Vertical Incidence Skywave antenna configurations for HF communications.",
                CardAsset: new CatalogAsset(
                    ImagePath: "/images/products/usap-family-card-nvis-1942-family-visual-v1.png",
                    AltText: "Field-deployed NVIS antenna system with a central mast and broad wire footprint.",
                    IsPlaceholder: false),
                HeroAsset: null,
                Route: "/products/nvis-antennas",
                CurrentSourceUrls: new[] { "https://www.usantennaproducts.com/products/nvis-antennas/" },
                LegacySourceUrls: Array.Empty<string>(),
                ApprovalStatus: "ProjectSelectedReferenceCandidatePendingApproval",
                SourceNotes: "Dedicated NVIS product category covering 1942 series. A2F 1942-family visual pending USAP review.",
                ConflictHolds: new[] { "1942: Low-power variant availability and weight-unit discrepancies across datasheets" },
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
                )
            ),
            new(
                Id: "antenna-rotator-control-systems",
                Slug: "antenna-rotator-control-systems",
                Name: "Antenna Rotator & Control Systems",
                DisplayOrder: 5,
                CardEyebrow: "Mechanical Rotators & Digital Controllers",
                CardTitle: "Antenna Rotator & Control Systems",
                CardSummary: "Mechanical rotators and digital control systems for directional antenna installations.",
                CardAsset: new CatalogAsset(
                    ImagePath: "/images/products/usap-family-card-rotator-control-r3500-drc4-a3s-recommended-v1.png",
                    AltText: "Heavy-duty R3500 antenna rotator paired with a rackmount DRC-4 digital controller.",
                    IsPlaceholder: false),
                HeroAsset: null,
                Route: "/products/antenna-rotator-control-systems",
                CurrentSourceUrls: new[]
                {
                    "https://www.usantennaproducts.com/products/antenna-rotator-systems/",
                    "https://www.usantennaproducts.com/products/digital-rotator-controller/"
                },
                LegacySourceUrls: Array.Empty<string>(),
                ApprovalStatus: "ProjectSelectedCandidateBPendingApproval",
                SourceNotes: "Approved taxonomy consolidation combining current Antenna Rotator Systems and Digital Rotator Controller categories. A3S Candidate B recommended visual pending USAP review.",
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
                    AltText: "Heavy-duty antenna rotator paired with a rackmount digital controller on an engineering bench.",
                    AssetClassification: "source-guided photorealistic product visualization",
                    ApprovalStatus: "provisional — USAP review pending"
                )
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
                SourceNotes: "Covers T-3002 series tower systems and ordering configurations. A2F non-configurational fallback visual pending USAP review.",
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
                )
            )
        };

        var productGroups = new List<ProductGroupRecord>
        {
            // 1. Log Periodic Antennas (5 groups)
            new(
                Id: "lp-high-power",
                FamilyId: "log-periodic-antennas",
                Name: "High-Power HF Log Periodics",
                DisplayOrder: 1,
                SectionAnchor: "lp-high-power",
                Models: new[]
                {
                    new ProductModelRecord("LP-1005", "High-power HF directional array", "3–30 MHz; 25 kW average / 50 kW PEP; horizontal polarization", "Provisional", SpecificationStatus: "Confirmed from current HTML"),
                    new ProductModelRecord("LP-1001", "High-power HF directional array", "4–30 MHz; 25 kW average / 50 kW PEP; horizontal polarization", "HoldDisputedSpecs", SpecificationStatus: "HoldDisputedSpecs"),
                    new ProductModelRecord("LP-1002", "High-power HF directional array", "6–40 MHz; 25 kW average / 50 kW PEP; horizontal polarization", "Provisional", SpecificationStatus: "Confirmed from current HTML")
                },
                ShortDescription: "Fixed-station directional HF log periodic antennas engineered for high-power communication systems spanning 3–40 MHz across LP-1005, LP-1001, and LP-1002 configurations.",
                CurrentSourceUrl: "https://www.usantennaproducts.com/antennas/models-lp-1005-lp-1001-lp-1002/",
                ApprovalStatus: "ClientConfirmation",
                SourceNotes: "HTML shows LP-1001 input impedance as 5 ohms; PDF shows 50 ohms.",
                ConflictHolds: new[] { "LP-1001 input impedance conflict between HTML (5 ohms) and PDF (50 ohms)" },
                ResourceIds: new[] { "doc-lp-high-power" },
                SpecificationStatus: "Confirmed from current HTML"
            ),
            new(
                Id: "lp-1017",
                FamilyId: "log-periodic-antennas",
                Name: "LP-1017 Log Periodic",
                DisplayOrder: 2,
                SectionAnchor: "lp-1017",
                Models: new[]
                {
                    new ProductModelRecord("LP-1017", "Wideband HF directional array", "6.2–30 MHz; 2 kW average / 4 kW PEP; horizontal polarization", "HoldDisputedSpecs", SpecificationStatus: "HoldDisputedSpecs")
                },
                ShortDescription: "Wideband directional HF log periodic antenna operating across 6.2–30 MHz, engineered for tactical and base-station communications with horizontal polarization.",
                CurrentSourceUrl: "https://www.usantennaproducts.com/antennas/model-lp-1017/",
                ApprovalStatus: "ClientConfirmation",
                SourceNotes: "Low-frequency vertical radiation angle discrepancy: HTML says 3.5 deg, PDF says 35 deg.",
                ConflictHolds: new[] { "Low-frequency radiation angle discrepancy between HTML (3.5 deg) and PDF (35 deg)" },
                ResourceIds: new[] { "doc-lp-1017" },
                SpecificationStatus: "Confirmed from current HTML"
            ),
            new(
                Id: "lp-1018ba",
                FamilyId: "log-periodic-antennas",
                Name: "LP-1018BA Broadband Log Periodic",
                DisplayOrder: 3,
                SectionAnchor: "lp-1018ba",
                Models: new[]
                {
                    new ProductModelRecord("LP-1018BA", "Broadband VHF/UHF directional array", "30–1100 MHz; horizontal or vertical orientation; 8 dB gain; 2:1 nominal VSWR", "Provisional", SpecificationStatus: "Confirmed from current HTML")
                },
                ShortDescription: "Broadband VHF/UHF log periodic antenna covering 30–1100 MHz, designed for wideband monitoring and communications with horizontal or vertical polarization mounting.",
                CurrentSourceUrl: "https://www.usantennaproducts.com/antennas/models-lp-1018ba/",
                ApprovalStatus: "Provisional",
                SourceNotes: "HTML and PDF core facts agree; pending USAP approval before publication.",
                ConflictHolds: Array.Empty<string>(),
                ResourceIds: new[] { "doc-lp-1018ba" },
                SpecificationStatus: "Confirmed from current HTML"
            ),
            new(
                Id: "lp-1019",
                FamilyId: "log-periodic-antennas",
                Name: "LP-1019 Series",
                DisplayOrder: 4,
                SectionAnchor: "lp-1019",
                Models: new[]
                {
                    new ProductModelRecord("LP-1019BA", "Broadband VHF/UHF log periodic", "100–1100 MHz; horizontal or vertical orientation; 8 dB gain; 2:1 nominal VSWR", "Provisional", SpecificationStatus: "Confirmed from current HTML"),
                    new ProductModelRecord("LP-1019SS", "Stainless steel VHF/UHF log periodic", "100–1100 MHz; stainless steel construction; horizontal or vertical orientation", "Provisional", SpecificationStatus: "Confirmed from current HTML")
                },
                ShortDescription: "Compact VHF/UHF log periodic directional antennas covering 100–1100 MHz, offering standard (LP-1019BA) and stainless-steel (LP-1019SS) construction options.",
                CurrentSourceUrl: "https://www.usantennaproducts.com/antennas/models-lp-1019-lp-1019ss/",
                ApprovalStatus: "ClientConfirmation",
                SourceNotes: "HTML title shortens first model to LP-1019; table and PDF designate it as LP-1019BA.",
                ConflictHolds: new[] { "First model naming discrepancy between HTML (LP-1019) and table/PDF (LP-1019BA)" },
                ResourceIds: new[] { "doc-lp-1019" },
                SpecificationStatus: "Confirmed from current HTML"
            ),
            new(
                Id: "lp-1112mr",
                FamilyId: "log-periodic-antennas",
                Name: "LP-1112MR Transportable Log Periodic",
                DisplayOrder: 5,
                SectionAnchor: "lp-1112mr",
                Models: new[]
                {
                    new ProductModelRecord("LP-1112MR", "Transportable rotatable HF directional array", "4–30 MHz; rotatable directional array; horizontal polarization", "HoldDisputedSpecs", SpecificationStatus: "HoldDisputedSpecs")
                },
                ShortDescription: "Transportable rotatable HF directional log periodic antenna array designed for tactical deployment across 4–30 MHz with horizontal polarization.",
                CurrentSourceUrl: "https://www.usantennaproducts.com/antennas/model-lp-1112mr/",
                ApprovalStatus: "HoldDisputedSpecs",
                SourceNotes: "Primarily classified under Log Periodic Antennas. Transportable nature noted in group attributes. Discrepancies in average power, gain, connector, and dimensions between HTML and PDF.",
                ConflictHolds: new[] { "Discrepancies in average power (5 kW vs 7.5 kW), gain (12 vs 11.5 dBi), connector, and dimensions between HTML and PDF" },
                ResourceIds: new[] { "doc-lp-1112mr" },
                SpecificationStatus: "HoldDisputedSpecs"
            ),

            // 2. Portable & Transportable Antenna Systems (3 groups)
            new(
                Id: "v-4213",
                FamilyId: "portable-transportable-antennas",
                Name: "V-4213 Portable Discone",
                DisplayOrder: 6,
                SectionAnchor: "v-4213",
                Models: new[]
                {
                    new ProductModelRecord("V-4213AD", "Complete mast and antenna system (30 ft mast)", "30–88 MHz; vertical polarization; 100 W average / 200 W PEP; complete 30 ft mast system", "HoldDisputedSpecs", SpecificationStatus: "HoldDisputedSpecs"),
                    new ProductModelRecord("V-4213AC", "Antenna only (standard 2-inch pipe mount)", "30–88 MHz; vertical polarization; 100 W average / 200 W PEP; antenna-only configuration", "HoldDisputedSpecs", SpecificationStatus: "HoldDisputedSpecs")
                },
                ShortDescription: "Omnidirectional VHF discone antenna system covering 30–88 MHz, available in a complete 30-foot mast field system (V-4213AD) or antenna-only configuration (V-4213AC).",
                CurrentSourceUrl: "https://www.usantennaproducts.com/antennas/models-v-4213-and-v-4213ac/",
                ApprovalStatus: "HoldDisputedSpecs",
                SourceNotes: "Two active public pages and PDFs conflict on model designation and 60 vs 81 mph wind rating.",
                ConflictHolds: new[] { "Model designation and wind rating conflict (60 vs 81 mph) between published datasheets" },
                ResourceIds: new[] { "doc-v-4213-revised", "doc-v-4213-legacy" },
                SpecificationStatus: "HoldDisputedSpecs"
            ),
            new(
                Id: "lp-1402-1403",
                FamilyId: "portable-transportable-antennas",
                Name: "LP-1402 / LP-1403 Transportable Log Periodics",
                DisplayOrder: 7,
                SectionAnchor: "lp-1402-1403",
                Models: new[]
                {
                    new ProductModelRecord("LP-1402", "Transportable VHF directional antenna (7 elements)", "30–76 MHz; 7 elements; horizontal or vertical polarization; 4.5 dBi gain", "HoldDisputedSpecs", SpecificationStatus: "HoldDisputedSpecs"),
                    new ProductModelRecord("LP-1403", "Transportable VHF directional antenna (8 elements)", "30–88 MHz; 8 elements; horizontal or vertical polarization; 4.5 dBi gain", "HoldDisputedSpecs", SpecificationStatus: "HoldDisputedSpecs")
                },
                ShortDescription: "Transportable directional VHF log periodic antennas designed for tactical field deployment across 30–76 MHz (LP-1402) and 30–88 MHz (LP-1403) with mast mounting hardware.",
                CurrentSourceUrl: "https://www.usantennaproducts.com/antennas/models-lp1403-and-lp1402/",
                ApprovalStatus: "HoldDisputedSpecs",
                SourceNotes: "HTML and PDF swap model-specific power ratings (65/130 W vs 75/150 W).",
                ConflictHolds: new[] { "Model-specific power rating discrepancy between HTML and PDF" },
                ResourceIds: new[] { "doc-lp-1402-1403" },
                SpecificationStatus: "HoldDisputedSpecs"
            ),
            new(
                Id: "1910",
                FamilyId: "portable-transportable-antennas",
                Name: "1910 Tactical Dipoles",
                DisplayOrder: 8,
                SectionAnchor: "1910",
                Models: new[]
                {
                    new ProductModelRecord("1910AA", "Tactical HF dipole system (400 W PEP)", "2–30 MHz; horizontal polarization; 200 W average / 400 W PEP; under 15 min 2-person setup", "Provisional", SpecificationStatus: "Confirmed from current HTML"),
                    new ProductModelRecord("1910BA", "Tactical HF dipole system (2 kW PEP)", "2–30 MHz; horizontal polarization; 1 kW average / 2 kW PEP; under 15 min 2-person setup", "Provisional", SpecificationStatus: "Confirmed from current HTML")
                },
                ShortDescription: "Rapid-deployment tactical HF dipole antenna systems covering 2–30 MHz, engineered for field communications in 400 W PEP (1910AA) and 2 kW PEP (1910BA) power ratings.",
                CurrentSourceUrl: "https://www.usantennaproducts.com/antennas/models-1910aa-1910ba/",
                ApprovalStatus: "Provisional",
                SourceNotes: "Consolidates duplicate 2016 and 2024 public entries; preferred source is 2024 revised datasheet.",
                ConflictHolds: new[] { "VSWR and power rating differences between legacy 2016 and preferred 2024 datasheets" },
                ResourceIds: new[] { "doc-1910-2024", "doc-1910-2016" },
                SpecificationStatus: "Confirmed from current HTML"
            ),

            // 3. Aperiodic Loop Antennas (1 group)
            new(
                Id: "aperiodic",
                FamilyId: "aperiodic-loop-antennas",
                Name: "USAP Aperiodic Loop Antenna",
                DisplayOrder: 9,
                SectionAnchor: "aperiodic",
                Models: new[]
                {
                    new ProductModelRecord(string.Empty, "Fixed and transportable HF receiving loop configurations", "2–32 MHz; receive-focused directional end-fire loop array; vertical loop plane", "ClientConfirmation", SpecificationStatus: "Confirmed from current HTML")
                },
                ShortDescription: "Receive-oriented HF directional end-fire loop antenna system covering 2–32 MHz, engineered for fixed-station and transportable installations requiring directional receiving performance.",
                CurrentSourceUrl: "https://www.usantennaproducts.com/antennas/usap-aperiodic-loop-antenna/",
                ApprovalStatus: "ClientConfirmation",
                SourceNotes: "No published model number; fixed and transportable descriptions are variants/characteristics, not separate product groups. PDF references a full catalog pending retrieval.",
                ConflictHolds: new[] { "No published model number; configuration options subject to USAP confirmation" },
                ResourceIds: new[] { "doc-aperiodic" },
                SpecificationStatus: "Confirmed from current HTML"
            ),

            // 4. NVIS Antennas (1 group)
            new(
                Id: "1942",
                FamilyId: "nvis-antennas",
                Name: "1942 NVIS Series",
                DisplayOrder: 10,
                SectionAnchor: "1942",
                Models: new[]
                {
                    new ProductModelRecord("1942-RT", "Roof-top mounting configuration (20 ft mast)", "2–30 MHz; 1 kW average / 2 kW PEP; 20 ft mast roof-top configuration", "Provisional", SpecificationStatus: "Confirmed from current HTML"),
                    new ProductModelRecord("1942-TA", "Transportable field configuration", "2–30 MHz; 1 kW average / 2 kW PEP; transportable field setup under 30 min", "Provisional", SpecificationStatus: "Confirmed from current HTML"),
                    new ProductModelRecord("1942-GM", "Ground-mount configuration", "2–30 MHz; 1 kW average / 2 kW PEP; ground mount setup under 120 min", "Provisional", SpecificationStatus: "Confirmed from current HTML"),
                    new ProductModelRecord("1942-RT-LP", "Low-power roof-top configuration", "Low-power configuration named only in PDF", "HoldDisputedSpecs", SpecificationStatus: "HoldDisputedSpecs"),
                    new ProductModelRecord("1942-TA-LP", "Low-power transportable configuration", "Low-power configuration named only in PDF", "HoldDisputedSpecs", SpecificationStatus: "HoldDisputedSpecs"),
                    new ProductModelRecord("1942-GM-LP", "Low-power ground-mount configuration", "Low-power configuration named only in PDF", "HoldDisputedSpecs", SpecificationStatus: "HoldDisputedSpecs")
                },
                ShortDescription: "Near Vertical Incidence Skywave (NVIS) HF antenna systems covering 2–30 MHz, providing gap-free short-to-medium range communications in roof-top, transportable, and ground-mount configurations.",
                CurrentSourceUrl: "https://www.usantennaproducts.com/antennas/models-1943-rt-1942-ta-1942-gm/",
                ApprovalStatus: "ClientConfirmation",
                SourceNotes: "Legacy URL slug typo says 1943. 1942-RT-LP, 1942-TA-LP, and 1942-GM-LP are PDF-only low-power configurations on conflict hold pending confirmation.",
                ConflictHolds: new[] { "Low-power variant availability and weight-unit discrepancies across published datasheets" },
                ResourceIds: new[] { "doc-1942" },
                SpecificationStatus: "Confirmed from current HTML"
            ),

            // 5. Antenna Rotator & Control Systems (5 groups)
            new(
                Id: "r3500",
                FamilyId: "antenna-rotator-control-systems",
                Name: "R3500 Heavy Duty Rotator with DRC-4 Rotator Control Unit",
                DisplayOrder: 11,
                SectionAnchor: "r3500",
                Models: new[]
                {
                    new ProductModelRecord("R3500", "Medium-to-heavy-duty rotator unit paired with DRC-4", "4,300 in-lb rotating torque; 10,000 lb vertical load capacity; worm gear drive", "ClientConfirmation", SpecificationStatus: "Confirmed from current HTML")
                },
                ShortDescription: "Heavy-duty antenna rotator unit paired with the DRC-4 digital controller unit, providing 4,300 in-lb rotating torque and 10,000 lb vertical load capacity for directional antenna positioning.",
                CurrentSourceUrl: "https://www.usantennaproducts.com/antennas/model-r3500/",
                ApprovalStatus: "ClientConfirmation",
                SourceNotes: "Confirm current DRC 4 bundle and product naming.",
                ConflictHolds: new[] { "DRC 4 bundle compatibility and product naming confirmation" },
                ResourceIds: new[] { "doc-r3500" },
                SpecificationStatus: "Confirmed from current HTML"
            ),
            new(
                Id: "r3501",
                FamilyId: "antenna-rotator-control-systems",
                Name: "R3501 Universal Rotator System",
                DisplayOrder: 12,
                SectionAnchor: "r3501",
                Models: new[]
                {
                    new ProductModelRecord("R3501", "Universal rotator unit compatible with DRC-3", "9,000 in-lb rotating torque; 23,000 in-lb braking torque; 1,000 lb vertical load", "ClientConfirmation", SpecificationStatus: "Confirmed from current HTML")
                },
                ShortDescription: "Universal antenna rotator system engineered for 9,000 in-lb rotating torque and 23,000 in-lb braking torque, compatible with the DRC-3 digital rotator controller.",
                CurrentSourceUrl: "https://www.usantennaproducts.com/antennas/model-r3501/",
                ApprovalStatus: "ClientConfirmation",
                SourceNotes: "Confirm controller compatibility and current configuration.",
                ConflictHolds: new[] { "Controller compatibility and current configuration confirmation" },
                ResourceIds: new[] { "doc-r3501" },
                SpecificationStatus: "Confirmed from current HTML"
            ),
            new(
                Id: "r3503",
                FamilyId: "antenna-rotator-control-systems",
                Name: "R3503 Heavy Duty Rotating System",
                DisplayOrder: 13,
                SectionAnchor: "r3503",
                Models: new[]
                {
                    new ProductModelRecord("R3503", "Heavy-duty rotating system compatible with DRC-3", "23,700 in-lb rotating torque; 60,000 in-lb braking torque; 20,000 lb vertical load", "ClientConfirmation", SpecificationStatus: "Confirmed from current HTML")
                },
                ShortDescription: "High-capacity antenna rotating system engineered for 23,700 in-lb rotating torque, 60,000 in-lb braking torque, and 20,000 lb vertical load capacity, compatible with the DRC-3 controller.",
                CurrentSourceUrl: "https://www.usantennaproducts.com/antennas/model-r3503/",
                ApprovalStatus: "ClientConfirmation",
                SourceNotes: "Confirm current DRC compatibility and software support.",
                ConflictHolds: new[] { "DRC compatibility and software support confirmation" },
                ResourceIds: new[] { "doc-r3503" },
                SpecificationStatus: "Confirmed from current HTML"
            ),
            new(
                Id: "drc-3",
                FamilyId: "antenna-rotator-control-systems",
                Name: "DRC-3 Digital Rotator Controller",
                DisplayOrder: 14,
                SectionAnchor: "drc-3",
                Models: new[]
                {
                    new ProductModelRecord("DRC-3", "Digital rotator controller for R3501 and R3503 systems", "Microprocessor-based controller for R3501 and R3503 rotator systems", "HoldDisputedSpecs", SpecificationStatus: "HoldDisputedSpecs")
                },
                ShortDescription: "Microprocessor-based digital rotator controller designed for precise azimuth control and position readout with USAP R3501 and R3503 rotator systems.",
                CurrentSourceUrl: "https://www.usantennaproducts.com/antennas/drc-3/",
                ApprovalStatus: "HoldDisputedSpecs",
                SourceNotes: "Do not publish OS/software, remote-user, or Internet-control claims until USAP confirms current support and security.",
                ConflictHolds: new[] { "Operating system, software support, and remote-user/Internet-control claims unverified" },
                ResourceIds: new[] { "doc-drc-3" },
                SpecificationStatus: "HoldDisputedSpecs"
            ),
            new(
                Id: "drc-4",
                FamilyId: "antenna-rotator-control-systems",
                Name: "DRC-4 Digital Rotator Controller",
                DisplayOrder: 15,
                SectionAnchor: "drc-4",
                Models: new[]
                {
                    new ProductModelRecord("DRC-4", "Digital rotator controller unit paired with R3500", "Standalone and PC control digital rotator controller paired with R3500", "HoldDisputedSpecs", SpecificationStatus: "HoldDisputedSpecs")
                },
                ShortDescription: "Digital rotator controller unit paired with the R3500 rotator, supporting standalone front-panel operation and external PC interface control.",
                CurrentSourceUrl: "https://www.usantennaproducts.com/antennas/drc-4/",
                ApprovalStatus: "HoldDisputedSpecs",
                SourceNotes: "Do not publish software, serial/USB, or remote-access compatibility claims until confirmed.",
                ConflictHolds: new[] { "Software, serial/USB, and remote-access compatibility claims unverified" },
                ResourceIds: new[] { "doc-drc-4" },
                SpecificationStatus: "HoldDisputedSpecs"
            ),

            // 6. Tower Systems & Accessories (1 group)
            new(
                Id: "t-3002",
                FamilyId: "tower-systems-accessories",
                Name: "T-3002 RLPA Tower System",
                DisplayOrder: 16,
                SectionAnchor: "t-3002",
                Models: new[]
                {
                    new ProductModelRecord("T-3002", "Rotatable log periodic antenna tower system", "80 ft or 100 ft rotatable log periodic antenna tower system; 115/230 VAC rotation", "Provisional", SpecificationStatus: "Confirmed from current HTML"),
                    new ProductModelRecord("3002FA", "100 ft dual-guyed tower configuration", "100 ft tower; dual-guyed configuration with rotation and feedline hardware", "Provisional", SpecificationStatus: "Confirmed from current HTML"),
                    new ProductModelRecord("3002FB", "80 ft dual-guyed tower configuration", "80 ft tower; dual-guyed configuration with rotation and feedline hardware", "Provisional", SpecificationStatus: "Confirmed from current HTML"),
                    new ProductModelRecord("3002SS", "100 ft self-supporting tower configuration", "100 ft tower; self-supporting configuration with rotation and feedline hardware", "Provisional", SpecificationStatus: "Confirmed from current HTML"),
                    new ProductModelRecord("3002SS-80", "80 ft self-supporting tower configuration", "80 ft tower; self-supporting configuration with rotation and feedline hardware", "Provisional", SpecificationStatus: "Confirmed from current HTML")
                },
                ShortDescription: "Rotatable log periodic antenna tower system providing 80 ft and 100 ft heights in dual-guyed and self-supporting structural configurations with rotation and feedline hardware.",
                CurrentSourceUrl: "https://www.usantennaproducts.com/antennas/model-t-3002/",
                ApprovalStatus: "Provisional",
                SourceNotes: "Consolidates duplicate public entries. The four ordering configurations (3002FA, 3002FB, 3002SS, 3002SS-80) are children of T-3002, not separate product groups.",
                ConflictHolds: Array.Empty<string>(),
                ResourceIds: new[] { "doc-t-3002-oct2016", "doc-t-3002-jun2016" },
                SpecificationStatus: "Confirmed from current HTML"
            )
        };

        var resources = new List<ProductResourceRecord>
        {
            new("doc-v-4213-revised", "v-4213", "V-4213 Data Sheet (May 2016 Revised)", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/05/4213cutsheet-revised.pdf", "CandidateOnly"),
            new("doc-lp-1402-1403", "lp-1402-1403", "LP-1402 / LP-1403 Data Sheet", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/05/1402AC_1403AB.pdf", "ConflictHold"),
            new("doc-1910-legacy", "1910", "Model 1910 Data Sheet (May 2016)", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/05/1910AA_1910BA.pdf", "SupersededCandidate"),
            new("doc-lp-1017", "lp-1017", "LP-1017 Data Sheet", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/05/1017-data-sheet-4.pdf", "ConflictHold"),
            new("doc-lp-1112mr", "lp-1112mr", "LP-1112MR Data Sheet", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/05/1112-data-sheet-revised.pdf", "ConflictHold"),
            new("doc-lp-high-power", "lp-high-power", "LP-1001, LP-1002, LP-1005 High-Power Log Periodic Data Sheet", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/05/1001_1002_1005-data-sheet.pdf", "Provisional"),
            new("doc-lp-1018ba", "lp-1018ba", "LP-1018BA Broadband Log Periodic Data Sheet", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/06/LP_1018BA_data_sheet_revised.pdf", "Provisional"),
            new("doc-v-4213-legacy", "v-4213", "Model V-4213 Data Sheet (June 2016)", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/06/Model-V4213-data-sheet-revised.pdf", "CandidateOnly"),
            new("doc-lp-1019", "lp-1019", "LP-1019 / LP-1019SS Data Sheet", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/06/1019-and-1019-ss-data-sheet.pdf", "Provisional"),
            new("doc-1910-2024", "1910", "1910AA / 1910BA Data Sheet (February 2024 Revised)", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2024/02/1910AA_1910BA-revised-1.pdf", "PreferredProvisional"),
            new("doc-t-3002-jun2016", "t-3002", "T-3002 Data Sheet (June 2016)", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/06/T-3002-Data-Sheet.pdf", "DuplicateCandidate"),
            new("doc-drc-3", "drc-3", "DRC 3 Digital Rotator Controller Data Sheet", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/06/DRC-3-Data-Sheet-revised.pdf", "ConflictHold"),
            new("doc-aperiodic", "aperiodic", "USAP Aperiodic Loop Antenna Data Sheet", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/06/USAP-Aperiodic-Loop-Antenna-data-sheet.pdf", "Provisional"),
            new("doc-1942", "1942", "Model 1942 Data Sheet", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/06/MODEL-1942-data-sheet-revised.pdf", "ConflictHold"),
            new("doc-r3500", "r3500", "Model R3500 Data Sheet", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/07/MODEL-R3500-data-sheet.doc.pdf", "ConflictHold"),
            new("doc-t-3002-oct2016", "t-3002", "T-3002 RLPA Tower System Data Sheet (October 2016)", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/10/T3002_data_sheet.pdf", "PreferredProvisional"),
            new("doc-r3501", "r3501", "Model R3501 Data Sheet", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/10/Model-R3501-CORRECTED-COPY.pdf", "ConflictHold"),
            new("doc-drc-4", "drc-4", "DRC 4 Digital Rotator Controller Data Sheet", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/10/DRC-4-Data-Sheet.pdf", "ConflictHold"),
            new("doc-r3503", "r3503", "Model R3503 Data Sheet", "Datasheet", "https://www.usantennaproducts.com/wp-content/uploads/2016/10/MODEL-R3503-data-sheet.doc.pdf", "ConflictHold")
        };

        return (families, productGroups, resources);
    }
}
