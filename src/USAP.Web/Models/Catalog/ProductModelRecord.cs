namespace USAP.Web.Models.Catalog;

public record ProductModelRecord(
    string ModelCode,
    string Configuration,
    string SpecificationsSummary,
    string ApprovalStatus,
    string SourcePresence = "Listed on current USAP website",
    string CommercialAvailability = "Not confirmed",
    string SpecificationStatus = "Confirmed from current HTML"
);
