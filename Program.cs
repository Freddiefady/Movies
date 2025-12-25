using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Movies.Services;
using Microsoft.OpenApi;
using Movies.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
// Add Global Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
// Add Problem Details support
builder.Services.AddProblemDetails(configure =>
{
    configure.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
    };
});

/**
* "type": "ValidationException",
* "title": "An error occurred",
* "status": 500,
* "detail": "One or more validation errors occurred.",
* "traceId": "23-sdfgjkhjkdfklgndvjnsdifpgbvlskmcods-348623676748hfb9byvb4",
* "requestId": "9gj08n0nv030f83b4"
*/

builder.Services.AddAutoMapper(typeof(Program));

// Dependency Injection for Services
// Created each time they are requested
// Disposed at the end of the request.
builder.Services.AddTransient<IGenresService, GenreService>();
builder.Services.AddTransient<IMoviesService, MovieService>();

builder.Services.AddSwaggerGen(options =>
{
    // Configure Swagger metadata
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Movies Test",
        Version = "v1",
        Description = "This is a sample API for demonstration purposes.",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Freddie Developer",
            Url = new Uri("https://example.com/support")
        },
        License = new OpenApiLicense
        {
            Name = "Use under LICX",
            Url = new Uri("https://example.com/license")
        }
    });
    // Define the security scheme
    options.AddSecurityDefinition("Windows", new OpenApiSecurityScheme
    {
        Name = "Authoriztions",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Enter your JWT key"
    });
    // Define the security requirement
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
                Name = "Bearer",
            },
            Array.Empty<string>()
        }
    });
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyMethod()
);

app.UseAuthorization();
// use built-in exception handler
app.UseExceptionHandler();
// Use the global exception handler middleware
// app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.MapControllers();
app.Run();
