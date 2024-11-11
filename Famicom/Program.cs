using MudBlazor.Services;
using Famicom.Components;
using TableControllerApi;
using Models.Services;
using DataAccess;

var builder = WebApplication.CreateBuilder(args);

var serviceCollection = new ServiceCollection();
builder.Services.AddSingleton<TableControllerService>();

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();
var apiHost = Host.CreateDefaultBuilder()
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<TableControllerApi.Startup>();
        webBuilder.UseUrls("http://localhost:4488");
    }).ConfigureServices(services =>
    {
        services.AddSingleton<TableControllerService>(provider => app.Services.GetService<TableControllerService>() ?? throw new InvalidOperationException("TableControllerService not found."));
    }).Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

apiHost.Start();

app.Run();