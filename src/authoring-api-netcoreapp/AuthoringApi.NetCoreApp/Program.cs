using AuthoringApi.NetCoreApp.Communication;
using AuthoringApi.NetCoreApp.Configuration;
using AuthoringApi.NetCoreApp.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers(options => options.ReturnHttpNotAcceptable = true)
    .AddXmlDataContractSerializerFormatters()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddTransient<IClientLogger, SignalRClientLogger>();
builder.Services.AddSingleton<ITokenManager, IdServerTokenManager>();
builder.Services.AddTransient<IAuthoringApiService, SitecoreAuthoringApiService>();
builder.Services.AddTransient<IAuthoringApiConfiguration, SitecoreAuthoringApiConfiguration>();
builder.Services.AddTransient<IManagementApiService, SitecoreManagementApiService>();

builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseStaticFiles();

app.MapHub<ClientProgressHub>("/progressHub");

app.Run();
