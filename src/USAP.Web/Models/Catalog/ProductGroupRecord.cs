namespace USAP.Web.Models.Catalog;

public record ProductGroupRecord(
    string Id,
    string FamilyId,
    string Name,
    int DisplayOrder,
    string SectionAnchor,
    IReadOnlyList<ProductModelRecord> Models,
    string ShortDescription,
    string CurrentSourceUrl,
    string ApprovalStatus,
    string SourceNotes,
    IReadOnlyList<string> ConflictHolds,
    IReadOnlyList<string> ResourceIds,
    string SourcePresence = "Listed on current USAP website",
    string CommercialAvailability = "Not confirmed",
    string SpecificationStatus = "Confirmed from current HTML"
);
