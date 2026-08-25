using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using USAP.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages with default conventions.
builder.Services.AddRazorPages();

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

app.UseRouting();

app.UseRateLimiter();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
