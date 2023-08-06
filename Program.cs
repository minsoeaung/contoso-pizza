using System.Text;
using System.Text.Json.Serialization;
using Amazon.S3;
using ContosoPizza.Authentication.ApiKey;
using ContosoPizza.Configurations;
using ContosoPizza.Data;
using ContosoPizza.Services;
using ContosoPizza.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

const string origins = "_myLocalReactApp";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: origins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
        }
    );
});

// https://nitishkaushik.com/addmvc-vs-addcontrollerswithviews-vs-addcontrollers-vs-addrazorpages-asp-net-core/
builder.Services.AddMvc()
    .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = ApiVersion.Default;
    options.ApiVersionReader = new HeaderApiVersionReader("X-Version");
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options => { options.GroupNameFormat = "'v'VVV"; });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Contoso API",
        Description = "ASP.NET Core Web API for Contoso",
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "Contoso API",
        Description = "ASP.NET Core Web API for Contoso Version 2",
    });

    options.AddSecurityDefinition("ApiBearerAuth", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter jwt token",
        Name = "Authentication",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiBearerAuth"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<ContosoContext>(opt => opt.UseNpgsql(builder.Configuration["Psql:connectionString"]));

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<MailConfig>(builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<AwsConfig>(builder.Configuration.GetSection("AWS"));
builder.Services.Configure<GoogleConfig>(builder.Configuration.GetSection("Google"));
builder.Services.Configure<SmsConfig>(builder.Configuration.GetSection("Twilio"));
builder.Services.Configure<FacebookConfig>(builder.Configuration.GetSection("Facebook"));

builder.Services.AddScoped<IPizzaService, PizzaService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IApiKeyService, ApiKeyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddTransient<ISmsService, SmsService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddHttpClient();

builder.Services.AddIdentityCore<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ContosoContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(jwtBearerOptions =>
    {
        jwtBearerOptions.SaveToken = true;
        jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero // Without this, token expiration time has additional 5 minutes
        };
    })
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
        ApiKeyDefaults.AuthenticationScheme,
        options => { }
    );

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeMe", policy => policy.RequireUserName("minsoeaung"));
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => options.RouteTemplate = "swagger/{documentName}/swagger.json");
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "Contoso Title";
        options.SwaggerEndpoint($"/swagger/v1/swagger.json", "v1");
        options.SwaggerEndpoint($"/swagger/v2/swagger.json", "v2");
    });
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();

app.UseCors(origins);

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.CreateDbIfNotExists();

app.Run();