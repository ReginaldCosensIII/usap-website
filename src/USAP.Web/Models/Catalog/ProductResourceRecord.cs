namespace USAP.Web.Models.Catalog;

public record ProductResourceRecord(
    string Id,
    string ProductGroupId,
    string Title,
    string ResourceType,
    string CurrentSourceUrl,
    string ApprovalStatus
);
