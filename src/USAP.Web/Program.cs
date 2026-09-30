using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.FileProviders;
using System.Threading.RateLimiting;
using USAP.Web.Configuration;
using USAP.Web.Middleware;
using USAP.Web.Models;
using USAP.Web.Services;
using USAP.Web.Services.Catalog;
using USAP.Web.Services.Email;
using USAP.Web.Services.Recaptcha;

var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages with default conventions.
builder.Services.AddRazorPages();

// Integration Options Foundations (FORMS-002, FORMS-003, FORMS-004) - populated via UserSecrets in Dev or IIS Env in Prod
builder.Services.AddOptions<SmtpOptions>()
    .Bind(builder.Configuration.GetSection(SmtpOptions.SectionName))
    .Validate(options => options.IsValid(out _), "SMTP configuration is invalid when Enabled=true.")
    .ValidateOnStart();
builder.Services.AddOptions<RecaptchaOptions>()
    .Bind(builder.Configuration.GetSection(RecaptchaOptions.SectionName))
    .Validate(options => options.IsValid(out _), "reCAPTCHA configuration is invalid when Enabled=true.")
    .ValidateOnStart();
builder.Services.Configure<AnalyticsOptions>(builder.Configuration.GetSection(AnalyticsOptions.SectionName));

builder.Services.AddHttpClient<IRecaptchaAssessmentService, GoogleRecaptchaAssessmentService>(client =>
{
    client.BaseAddress = new Uri("https://recaptchaenterprise.googleapis.com/");
    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddOptions<SiteSettings>()
    .Bind(builder.Configuration.GetSection(SiteSettings.SectionName))
    .Validate(settings =>
    {
        if (string.IsNullOrWhiteSpace(settings.BaseUrl))
        {
            return false;
        }

        if (!Uri.TryCreate(settings.BaseUrl, UriKind.Absolute, out var uri))
        {
            return false;
        }

        if (uri.Scheme != Uri.UriSchemeHttps)
        {
            return false;
        }

        if (string.IsNullOrEmpty(uri.Host))
        {
            return false;
        }

        if (!string.IsNullOrEmpty(uri.UserInfo))
        {
            return false;
        }

        if (!string.IsNullOrEmpty(uri.Query))
        {
            return false;
        }

        if (!string.IsNullOrEmpty(uri.Fragment))
        {
            return false;
        }

        if (uri.AbsolutePath != "/")
        {
            return false;
        }

        if (settings.BaseUrl.EndsWith("/"))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(settings.SiteName))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(settings.OrganizationName))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(settings.DefaultTitle))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(settings.DefaultDescription))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(settings.DefaultSocialImagePath) || !settings.DefaultSocialImagePath.StartsWith("/") || settings.DefaultSocialImagePath.StartsWith("//"))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(settings.DefaultSocialImageAlt))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(settings.Locale))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(settings.ThemeColor))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(settings.TitleSeparator))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(settings.TwitterCard))
        {
            return false;
        }

        return true;
    }, "Critical SiteSettings are missing or invalid.")
    .ValidateOnStart();

builder.Services.AddScoped<ICtaContextResolver, CtaContextResolver>();
builder.Services.AddScoped<IInquiryMessageComposer, InquiryMessageComposer>();
builder.Services.AddScoped<IEmailComposer, EmailComposer>();
builder.Services.AddScoped<ISmtpEmailSender, MailKitSmtpEmailSender>();
builder.Services.AddSingleton<IProductCatalogService, ProductCatalogService>();

var smtpSection = builder.Configuration.GetSection(SmtpOptions.SectionName);
var smtpEnabled = smtpSection.GetValue<bool>("Enabled");

if (smtpEnabled)
{
    builder.Services.AddScoped<IInquirySubmissionService, SmtpInquirySubmissionService>();
}
else if (builder.Environment.IsDevelopment())
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
    // Dedicated Razor Page handler for unexpected server failures (HTTP 500).
    app.UseExceptionHandler("/Error");

    // HSTS is not applied in Development; IIS/production configuration controls HSTS headers.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Permanent HTTP 301 redirects for legacy WordPress technical document URLs
app.UseMiddleware<LegacyDocumentRedirectMiddleware>();

app.UseStaticFiles();

// Serve canonical technical documents under the public /technical-resources/documents route
var technicalDocsPath = Path.Combine(app.Environment.WebRootPath, "documents", "technical");
if (Directory.Exists(technicalDocsPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(technicalDocsPath),
        RequestPath = "/technical-resources/documents"
    });
}

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

app.UseStatusCodePagesWithReExecute("/not-found");

app.UseRouting();

app.UseRateLimiter();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
