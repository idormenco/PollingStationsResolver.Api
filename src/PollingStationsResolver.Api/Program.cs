global using FastEndpoints;
global using FastEndpoints.Security;
using FastEndpoints.Swagger;
using PollingStationsResolver.Api.Services.Credentials;
using PollingStationsResolver.Api.Services.Parser;
using PollingStationsResolver.Domain;
using System.Text;
using PollingStationsResolver.Api.HangfireJobs;
using PollingStationsResolver.Geocoding;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // set this for Excel parser

var builder = WebApplication.CreateBuilder();


builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc();
builder.Services.AddApplicationDomain(builder.Configuration);
builder.Services.AddApplicationAuth(builder.Configuration);
builder.Services.AddSingleton<IExcelParser, ExcelParser>();

builder.Services.AddLocationServices(builder.Configuration);
builder.Services.AddHangfireJobs();

// todo check this registration
builder.Services.AddSingleton<PollingStationsResolver.Api.Features.ImportedPollingStation.ResponseMapper>();
builder.Services.AddSingleton<PollingStationsResolver.Api.Features.PollingStation.ResponseMapper>();

builder.WebHost.ConfigureKestrel(o =>
{
    o.Limits.MaxRequestBodySize = 1073741824; //set to max allowed file size of your system
});

var app = builder.Build();
app.UseDefaultExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireJobs(builder.Configuration);

app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
});
app.UseSwaggerGen();
app.Run();
