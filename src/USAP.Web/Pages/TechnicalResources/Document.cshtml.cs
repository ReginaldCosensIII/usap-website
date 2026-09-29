using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using USAP.Web.Models;
using USAP.Web.Models.Catalog;
using USAP.Web.Services.Catalog;

namespace USAP.Web.Pages.TechnicalResources;

public class DocumentModel : PageModel
{
    private static readonly Dictionary<string, DocumentEnrichmentData> ExperimentalEnrichments = new(StringComparer.OrdinalIgnoreCase)
    {
        ["usap-lp-1001-lp-1002-lp-1005-data-sheet"] = new(
            Overview: "The LP-1001, LP-1002, and LP-1005 series encompasses high-power, rotatable HF log-periodic antenna arrays engineered for directional communications across high-frequency bands. Designed for demanding base-station and transportable communications where multi-frequency coverage and high gain are essential, these arrays provide continuous coverage without requiring external matching networks. This data sheet presents electrical characteristics, mechanical dimensions, and structural specifications across all three covered models.",
            KeyTopics: new[]
            {
                "Electrical specifications detailing operating frequency coverage and RF power-handling capabilities across covered models.",
                "Mechanical boom and element dimensions, structural materials, and operational turning radius.",
                "Directional forward gain profiles, nominal VSWR characteristics, and front-to-back ratios.",
                "Rotator system mounting interfaces and communication tower installation guidelines.",
                "Model comparison matrix distinguishing LP-1001, LP-1002, and LP-1005 parameters."
            }
        ),
        ["usap-lp-1017-data-sheet"] = new(
            Overview: "The LP-1017 is a commercial, lightweight HF log-periodic antenna engineered for directional medium- to long-range communications across 6.2–30.0 MHz. Suitable for broadband automatic link establishment (ALE) and fixed-frequency operations without external tuning networks, the antenna uses tapered tubular aluminum elements to provide directional gain while minimizing array weight and wind drag. This data sheet provides electrical ratings, mechanical dimensions, and takeoff angle performance data.",
            KeyTopics: new[]
            {
                "Electrical specifications covering frequency coverage, power capability, nominal impedance, and VSWR.",
                "Structural parameters including boom length, longest element span, turning radius, and array weight.",
                "Wind-loading ratings and structural survivability information for uniced and radial ice conditions.",
                "Take-off angle reference table documenting vertical radiation angles across operating frequencies and mast heights.",
                "Rotator interface notes and antenna system installation guidance."
            }
        ),
        ["usap-lp-1018ba-data-sheet"] = new(
            Overview: "The LP-1018BA is a broadband log-periodic dipole antenna designed for directional transmission and reception in tactical, surveillance, and monitoring installations across 30–1100 MHz. Offering versatile horizontal or vertical polarization mounting options, the antenna supports point-to-point, ground-to-air, and spectrum monitoring applications. This publication details electrical ratings across extended frequency bands, structural boom configurations, and mounting hardware.",
            KeyTopics: new[]
            {
                "Broadband frequency coverage and electrical specifications across 30 to 1100 MHz.",
                "Forward gain characteristics, front-to-back ratios, and nominal VSWR performance curves.",
                "RF power ratings and input connector configurations across lower and upper frequency bands.",
                "Mechanical construction details, element dimensions, and boom mounting assembly.",
                "Horizontal and vertical polarization mounting orientations and installation options."
            }
        ),
        ["usap-lp-1019-series-data-sheet"] = new(
            Overview: "The LP-1019 series consists of compact, broadband log-periodic antenna arrays engineered for high-frequency point-to-point, ground-to-air, and surveillance applications across 100–1100 MHz. Available in lightweight aluminum (LP-1019BA) and corrosion-resistant stainless steel (LP-1019SS) constructions, these antennas deliver directional gain and broadband impedance matching without complex tuning. The data sheet presents model comparisons, radiation curves, and mounting details.",
            KeyTopics: new[]
            {
                "Frequency range coverage (100–1100 MHz) and broadband electrical characteristics.",
                "Comparison of LP-1019BA (aluminum) and LP-1019SS (stainless steel) material construction.",
                "Directional forward gain, front-to-back ratio, and typical VSWR curves.",
                "Feedline connector interfaces and RF power-handling ratings.",
                "Mechanical dimensions, wind surface area, and mast mounting bracket configurations."
            }
        ),
        ["usap-lp-1112mr-data-sheet"] = new(
            Overview: "The LP-1112MR is a heavy-duty transportable, rotatable log-periodic antenna system designed for tactical medium- and long-range communications across 4.0–30.0 MHz. Engineered for field deployment, the system combines high power-handling capacity with a modular sectional mast and rapid-erection guy structure. This comprehensive publication covers field assembly procedures, electrical characteristics, component inventories, and mechanical specifications.",
            KeyTopics: new[]
            {
                "System specifications covering 4.0–30.0 MHz frequency range and high-power handling ratings.",
                "Modular mast components, structural guy layout, and ground anchoring geometry.",
                "Step-by-step field erection and takedown guidance for mobile deployment teams.",
                "Antenna element assembly, boom configuration, and feedline connection instructions.",
                "Wind load survivability, environmental performance data, and transport case packing list."
            }
        ),
        ["usap-v4213-portable-discone-overview"] = new(
            Overview: "The V-4213 series encompasses omnidirectional portable discone antenna systems designed for broadband field communications, air traffic control, and tactical base operations. Providing vertically polarized broadband coverage without tuning adjustments, the V-4213 can be rapidly deployed on sectional field masts or temporary fixtures. This system overview introduces equipment distinctions, deployment advantages, and general electrical and mechanical characteristics.",
            KeyTopics: new[]
            {
                "Omnidirectional broadband radiation patterns and vertical polarization characteristics.",
                "System architecture and equipment-level distinctions between V-4213 variants.",
                "Core electrical specifications including nominal VSWR, impedance, and connector interface.",
                "Sectional field mast mounting and mechanical support hardware options.",
                "Tactical transport packaging and field deployment workflow."
            }
        ),
        ["usap-v4213ad-v4213ac-data-sheet"] = new(
            Overview: "This technical data sheet provides detailed configuration specifications, connector definitions, and radiation pattern plots for the V-4213AD and V-4213AC portable discone antennas. Focused on equipment-level differentiation, the document outlines specific RF power capabilities, mechanical dimensions, and environmental parameters for each variant to assist system engineers in selecting the appropriate configuration for fixed and mobile communications.",
            KeyTopics: new[]
            {
                "Configuration comparison distinguishing V-4213AD and V-4213AC model variants.",
                "Detailed electrical specification tables covering frequency bandwidth, VSWR, and power ratings.",
                "Radiation pattern plots and azimuth/elevation coverage characteristics.",
                "Mechanical dimensions, element lengths, cone/disc angles, and total system weight.",
                "Environmental operational limits and mounting hardware specifications."
            }
        ),
        ["usap-lp-1402-lp-1403-data-sheet"] = new(
            Overview: "The LP-1402 and LP-1403 are tactical transportable log-periodic antennas engineered for rapid field deployment and directional HF communications across wide operational frequency bands. Designed for mast mounting in expeditionary installations, these arrays deliver broadband gain without requiring external impedance matching devices. This data sheet details electrical performance, mechanical attributes, and transport specifications for both models.",
            KeyTopics: new[]
            {
                "Frequency bandwidth and directional electrical performance parameters.",
                "Model comparison distinguishing LP-1402 and LP-1403 power ratings and dimensions.",
                "Nominal VSWR characteristics, input connector types, and nominal impedance.",
                "Mechanical construction, element folding or detachment for transport, and mast interfaces.",
                "Tactical deployment logistics including transit bag packaging and setup guidance."
            }
        ),
        ["usap-1910aa-1910ba-data-sheet"] = new(
            Overview: "The Model 1910 series consists of lightweight tactical broadband HF dipole antenna systems engineered for rapid field deployment across 2.0–30.0 MHz. Available as the 1910AA and 1910BA models, these center-fed terminated dipoles operate across their entire bandwidth without an antenna tuner. Designed for tactical field deployment, the system supports both high-angle short-range propagation and low-angle communications. This data sheet presents electrical ratings, dimensional parameters, and transport details.",
            KeyTopics: new[]
            {
                "Electrical performance specifications for Model 1910AA and Model 1910BA across 2.0–30.0 MHz.",
                "Tuner-free broadband operation, nominal VSWR curves, and documented typical efficiency.",
                "Radiation pattern and take-off angle characteristics for short-, medium-, and extended-range links.",
                "Mechanical dimensions, wire element materials, mast support structure, and guy anchors.",
                "Tactical packaging, transit bag configurations, and two-person field erection procedures."
            }
        ),
        ["usap-aperiodic-loop-antenna-data-sheet"] = new(
            Overview: "The USAP Aperiodic Loop Antenna is an untuned broadband receive-antenna system designed for omnidirectional or directional array applications where compact footprint and low noise reception are paramount. Utilizing an integrated low-noise preamplifier circuit at the loop base, the system delivers consistent frequency response across HF bands without manual re-tuning. This technical publication outlines loop electrical principles, array configurations, preamplifier specifications, and mechanical mounting.",
            KeyTopics: new[]
            {
                "Operating principles of untuned balanced receiving loops in high-frequency applications.",
                "Broadband preamplifier electrical specifications, dynamic range, and power requirements.",
                "Intermodulation performance, sensitivity metrics, and noise-floor characteristics.",
                "Array layout configurations, multi-element spacing guidelines, and directional phasing.",
                "Mechanical construction, weatherproof housing details, and transportable mounting options."
            }
        ),
        ["usap-1942-nvis-series-data-sheet"] = new(
            Overview: "The 1942 series encompasses Near Vertical Incidence Skywave (NVIS) antenna systems engineered to support tactical regional communications in terrain where line-of-sight propagation is obstructed. Available in rooftop (RT), tactical transportable (TA), and ground-mounted (GM) configurations, these antennas direct RF energy at steep angles to bridge regional communication gaps without relying on ground repeaters. This data sheet details configuration models, power options, and structural layouts.",
            KeyTopics: new[]
            {
                "Principles of Near Vertical Incidence Skywave (NVIS) propagation for terrain-obstructed paths.",
                "Model configurations detailing Rooftop (RT), Transportable (TA), and Ground-Mount (GM) variants.",
                "Power-handling capabilities distinguishing standard and low-power (-LP) model designations.",
                "Electrical specifications including frequency coverage, nominal VSWR, and input impedance.",
                "Mechanical footprint, central mast assembly, guy arrangements, and field deployment procedures."
            }
        ),
        ["usap-r3500-rotator-data-sheet"] = new(
            Overview: "The R3500 is a heavy-duty antenna rotator engineered to support and rotate large directional HF communication arrays on commercial and military communication towers. Featuring a rugged weatherproof housing and high-torque mechanical drive, the rotator provides continuous directional positioning under severe wind and icing conditions. This data sheet outlines mechanical torque ratings, vertical load capacity, mast dimensions, and compatible control unit interfaces.",
            KeyTopics: new[]
            {
                "Mechanical torque specifications including rotating torque, braking torque, and maximum vertical load.",
                "Drive mechanism, motor rating, rotational speed, and precision positioning feedback.",
                "Weatherproof housing construction, seal specifications, and environmental survivability.",
                "Tower mounting configurations, top-mast clamp interfaces, and feedline routing provisions.",
                "Digital rotator controller compatibility and remote interface options."
            }
        ),
        ["usap-r3501-rotator-data-sheet"] = new(
            Overview: "The R3501 Universal Rotator System is engineered for medium- to heavy-duty antenna positioning applications where mechanical versatility, reliable azimuth rotation, and economical installation are required. Designed to mount in various positions on standard communication towers, the unit provides continuous azimuth rotation for directional antenna arrays. This technical document presents operational capabilities, drive train data, and control interface requirements.",
            KeyTopics: new[]
            {
                "Mechanical rotational torque, braking capacity, and vertical load limits for medium-duty arrays.",
                "Drive train configuration, gear reduction, and azimuth rotation speed parameters.",
                "Tower mounting flexibility, platform requirements, and mast clamping dimensions.",
                "Control cable wiring specifications, positional accuracy, and limit-switch operation.",
                "Integration with USAP digital rotator control units for local and remote positioning."
            }
        ),
        ["usap-r3503-rotator-data-sheet"] = new(
            Overview: "The R3503 is a heavy-duty rotating system engineered to rotate and support large directional communication antenna arrays across demanding environmental conditions. Designed for flexible mounting positions on most standard communication towers, the assembly encloses drive components within a rugged, weather-protected housing. Typically deployed alongside a DRC-3 Digital Rotator Controller, it enables both local manual adjustments at the tower base and remote operations via computer interface.",
            KeyTopics: new[]
            {
                "Mechanical torque specifications: rotating torque, braking torque, and vertical load capacity.",
                "Drive train and motor specifications including gear-and-chain drive and horsepower ratings.",
                "Azimuth rotation speed, user-definable limits, and positional accuracy parameters.",
                "Digital rotator control unit integration, software commands, and forward/reverse delay timers.",
                "Operating temperature ratings, shipping metrics, and environmental protection features."
            }
        ),
        ["usap-drc-3-controller-data-sheet"] = new(
            Overview: "The DRC-3 is an industrial antenna-rotator control enclosure engineered to provide accurate, reliable azimuth positioning for heavy-duty antenna rotating systems. Housed in a ruggedized protective enclosure, the DRC-3 features local front-panel controls and digital readouts alongside serial computer interface capabilities for automated remote tracking. This publication details electrical power requirements, interface protocols, enclosure mounting, and operating modes.",
            KeyTopics: new[]
            {
                "Industrial enclosure construction, front-panel indicators, and local control buttons.",
                "Digital azimuth positioning accuracy, heading readout, and preset bearing memory.",
                "Remote computer control interface, command protocol, and software integration options.",
                "Motor drive control outputs, speed regulation, and forward/reverse delay safety timers.",
                "Electrical input power requirements, field wiring terminal blocks, and environmental specs."
            }
        ),
        ["usap-drc-4-controller-data-sheet"] = new(
            Overview: "The DRC-4 is a horizontal tabletop digital antenna-rotator controller designed for communications operations requiring precise azimuth heading selection and intuitive desktop controls. Featuring a high-visibility digital display, rotary heading dial, and programmable memory presets, the unit interfaces with USAP rotator systems for responsive antenna positioning. This data sheet presents electrical characteristics, front-panel features, rear connectivity, and operational modes.",
            KeyTopics: new[]
            {
                "Horizontal tabletop chassis architecture, front-panel rotary dial, and digital display readout.",
                "Azimuth heading control, manual rotary entry, and programmable target presets.",
                "Rotator feedback sensor interface, position calibration, and heading accuracy.",
                "Rear-panel connection terminals, control cable requirements, and power supply specs.",
                "Computer interface ports and remote software command compatibility."
            }
        ),
        ["usap-t-3002-tower-system-data-sheet"] = new(
            Overview: "The T-3002 RLPA Tower System is a complete structural and mechanical support installation engineered specifically for rotating log-periodic antenna arrays. Incorporating heavy-duty lattice tower sections, an internal rotating mast, and an integrated rotator mounting platform, the T-3002 provides a stable foundation for directional HF antenna systems. This technical data sheet presents structural height options, wind loading ratings, mechanical interface dimensions, and foundation requirements.",
            KeyTopics: new[]
            {
                "Lattice tower structural design, section heights, and modular assembly features.",
                "Internal rotating mast assembly, bearing supports, and antenna mounting interfaces.",
                "Rotator mounting platform geometry, torque transfer, and alignment provisions.",
                "Structural wind loading survivability, ice rating parameters, and guy wire configurations.",
                "Foundation specifications, ground installation footprints, and erection safety guidance."
            }
        )
    };

    private readonly IProductCatalogService _catalogService;

    public DocumentModel(IProductCatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    public ProductResourceRecord Document { get; private set; } = null!;
    public ProductFamilyRecord? Family { get; private set; }
    public IReadOnlyList<ProductGroupRecord> RelatedGroups { get; private set; } = Array.Empty<ProductGroupRecord>();
    public DocumentEnrichmentData? Enrichment { get; private set; }
    public CatalogAsset? PrimaryVisual { get; private set; }
    public string? PrimaryGroupTitle { get; private set; }

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

        if (ExperimentalEnrichments.TryGetValue(slug, out var enrichment))
        {
            Enrichment = enrichment;
        }

        var primaryGroup = RelatedGroups.FirstOrDefault();
        if (primaryGroup != null)
        {
            PrimaryVisual = primaryGroup.AssociatedAsset;
            PrimaryGroupTitle = primaryGroup.Name;
        }

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

public record DocumentEnrichmentData(
    string Overview,
    IReadOnlyList<string> KeyTopics
);
