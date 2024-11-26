using MudBlazor.Services;
using Famicom.Components;
using Blazored.SessionStorage;
using Models.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<TableControllerService>();

// Add MudBlazor services
builder.Services.AddMudServices();
builder.Services.AddBlazoredSessionStorage(); 

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAntiforgery();

var app = builder.Build();
var apiHost = Host.CreateDefaultBuilder()
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<TableControllerApi.Startup>();

        string envPath = Path.Combine(AppContext.BaseDirectory, ".env");
        Env.Load(envPath);
        string tcapiPort = Env.GetString("TCAPI_PORT");
        
        webBuilder.UseUrls("https://localhost:" + tcapiPort);
    }).ConfigureServices(services =>
    {
        services.AddSingleton<ITableControllerService, TableControllerService>(provider => app.Services.GetService<TableControllerService>() ?? throw new InvalidOperationException("TableControllerService not found."));
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

app.UseRouting();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

apiHost.Start();

app.Run();