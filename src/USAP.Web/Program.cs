using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using USAP.Web.Services;
using USAP.Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages with default conventions.
builder.Services.AddRazorPages();

builder.Services.AddOptions<SiteSettings>()
    .Bind(builder.Configuration.GetSection(SiteSettings.SectionName))
    .Validate(settings =>
    {
        if (string.IsNullOrWhiteSpace(settings.BaseUrl)) return false;
        if (!Uri.TryCreate(settings.BaseUrl, UriKind.Absolute, out var uri)) return false;
        if (uri.Scheme != Uri.UriSchemeHttps) return false;
        if (string.IsNullOrEmpty(uri.Host)) return false;
        if (!string.IsNullOrEmpty(uri.UserInfo)) return false;
        if (!string.IsNullOrEmpty(uri.Query)) return false;
        if (!string.IsNullOrEmpty(uri.Fragment)) return false;
        if (uri.AbsolutePath != "/") return false;
        if (settings.BaseUrl.EndsWith("/")) return false;

        if (string.IsNullOrWhiteSpace(settings.SiteName)) return false;
        if (string.IsNullOrWhiteSpace(settings.OrganizationName)) return false;
        if (string.IsNullOrWhiteSpace(settings.DefaultTitle)) return false;
        if (string.IsNullOrWhiteSpace(settings.DefaultDescription)) return false;
        if (string.IsNullOrWhiteSpace(settings.DefaultSocialImagePath) || !settings.DefaultSocialImagePath.StartsWith("/") || settings.DefaultSocialImagePath.StartsWith("//")) return false;
        if (string.IsNullOrWhiteSpace(settings.DefaultSocialImageAlt)) return false;
        if (string.IsNullOrWhiteSpace(settings.Locale)) return false;
        if (string.IsNullOrWhiteSpace(settings.ThemeColor)) return false;
        if (string.IsNullOrWhiteSpace(settings.TitleSeparator)) return false;
        if (string.IsNullOrWhiteSpace(settings.TwitterCard)) return false;

        return true;
    }, "Critical SiteSettings are missing or invalid.")
    .ValidateOnStart();

builder.Services.AddScoped<IInquiryMessageComposer, InquiryMessageComposer>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IInquirySubmissionService, DevelopmentInquirySubmissionService>();
}
else
{
    builder.Services.AddScoped<IInquirySubmissionService, UnavailableInquirySubmissionService>();
}

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("InquirySubmission", context =>
    {
        // Only rate limit POST requests
        if (context.Request.Method != HttpMethods.Post)
        {
            return RateLimitPartition.GetNoLimiter("NoLimit");
        }

        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown_ip";
        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            Window = TimeSpan.FromMinutes(1),
            PermitLimit = 5,
            QueueLimit = 0
        });
    });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", token);
    };
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // Custom error handling deferred to Core Site Build task.
    app.UseExceptionHandler("/Error");

    // HSTS is not applied in Development; IIS/production configuration controls HSTS headers.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Apply X-Robots-Tag to prevent indexing in non-production environments.
// Placed after UseStaticFiles to avoid unnecessary header processing on static assets,
// but before UseRouting to ensure all dynamic routes receive the header.
if (!app.Environment.IsProduction())
{
    app.Use(async (context, next) =>
    {
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Append("X-Robots-Tag", "noindex, nofollow");
            return Task.CompletedTask;
        });
        await next(context);
    });
}

app.UseRouting();

app.UseRateLimiter();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
