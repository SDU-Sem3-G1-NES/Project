using MudBlazor.Services;
using Famicom.Components;
using Blazored.SessionStorage;
using Models.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Components;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);


var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
};

builder.Services.AddHttpClient("default", client =>
{

}).ConfigurePrimaryHttpMessageHandler(() => handler);

builder.Services.AddSingleton<TableControllerService>();
builder.Services.AddScoped<LoginStateService>();
builder.Services.AddScoped<UserPermissionService>();
builder.Services.AddHostedService<SubscriberNotifyService>();
builder.Services.AddSingleton<SubscriberUriService>();

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
        services.AddHttpClient();
        services.AddSingleton<ITableControllerService, TableControllerService>(provider => app.Services.GetService<TableControllerService>() ?? throw new InvalidOperationException("TableControllerService not found."));
        services.AddSingleton<ISubscriberUriService, SubscriberUriService>(provider => app.Services.GetService<SubscriberUriService>() ?? throw new InvalidOperationException("SubscriberUriService not found."));

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