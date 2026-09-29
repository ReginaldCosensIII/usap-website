namespace USAP.Web.Services;

using USAP.Web.Models;

public interface ICtaContextResolver
{
    /// <summary>
    /// Safely resolves and validates incoming query parameters against canonical catalog and document data.
    /// </summary>
    CtaContext Resolve(string? reason, string? family, string? group, string? doc);
}
