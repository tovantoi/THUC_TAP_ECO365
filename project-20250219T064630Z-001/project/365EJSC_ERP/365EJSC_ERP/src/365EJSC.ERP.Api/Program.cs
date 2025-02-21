using _365EJSC.ERP.Application.DependencyInjection.Extension;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Persistence.DependencyInjection.Extensions;
using _365EJSC.ERP.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddPresentation();

builder.Environment.AddEnvironmentHelper();
var app = builder.Build();

app.UseGrpcWeb();
app.MapPresentationEndpoint();
app.Run();