using _365EJSC.ERP.Api.DependencyInjection.Extensions;
using _365EJSC.ERP.Api.DependencyInjection.Options;
using _365EJSC.ERP.Application.DependencyInjection.Extension;
using _365EJSC.ERP.Contract.Constants;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Middlewares;
using _365EJSC.ERP.Persistence.DependencyInjection.Extensions;
using _365EJSC.ERP.Presentation.Abstractions;
using Asp.Versioning;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var serviceName = "365EJSC API ERP";
//register controllers
builder.Services.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly());
//register api configuration
builder.Services.AddSingleton(new ApiConfig { Name = serviceName });
//Configure swagger
builder.Services.ConfigureOptions<SwaggerConfigureOptions>();

//Configure api versioning
builder.Services.AddApiVersioning(
        options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new QueryStringApiVersionReader());
        })
    .AddMvc()
    .AddApiExplorer(
        options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Environment.AddEnvironmentHelper();
builder.Services.AddProblemDetails();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.UseApiLayerSwagger();
//app.UseHsts();
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapPresentation();

app.Run();