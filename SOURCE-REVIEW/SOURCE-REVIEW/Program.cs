using review.application.DependencyInjection.Extension;
using review.Contract.DependencyInjection.Extensions;
using review.Persistence.DependencyInjection.Extensions;
using review.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddPresentation();
builder.Environment.AddEnvironmentHelper();
var app = builder.Build();
app.UseGrpcWeb();
app.MapPresentationEndpoint();
app.Run();