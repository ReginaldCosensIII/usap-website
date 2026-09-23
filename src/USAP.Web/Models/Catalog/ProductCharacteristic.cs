namespace USAP.Web.Models.Catalog;

/// <summary>
/// Represents a discrete, evidence-safe product characteristic or specification label/value pair.
/// </summary>
public record ProductCharacteristic(
    string Label,
    string DisplayValue
);
