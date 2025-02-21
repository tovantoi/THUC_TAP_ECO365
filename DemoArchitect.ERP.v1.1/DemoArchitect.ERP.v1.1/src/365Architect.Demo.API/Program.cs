using _365Architect.Demo.Application.DependencyInjection.Extension;
using _365Architect.Demo.Contract.DependencyInjection.Extensions;
using _365Architect.Demo.Persistence.DependencyInjection.Extensions;
using _365Architect.Demo.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddPresentation();
builder.Environment.AddEnvironmentHelper();
var app = builder.Build();
app.UseGrpcWeb();
app.MapPresentationEndpoint();
app.Run();