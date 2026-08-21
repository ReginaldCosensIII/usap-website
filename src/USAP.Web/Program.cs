var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages with default conventions.
builder.Services.AddRazorPages();

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
