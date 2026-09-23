namespace USAP.Web.Models.Catalog;

/// <summary>
/// Represents an extensible public-facing technical document record in the current interim product-document set.
/// Contains display data with zero internal approval tokens.
/// </summary>
public record ProductResourceRecord(
    string Id,
    string ProductGroupId,
    string Title,
    string ResourceType,
    string CurrentSourceUrl
);
