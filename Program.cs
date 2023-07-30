using System.Text;
using System.Text.Json.Serialization;
using ContosoPizza.Authentication.ApiKey;
using ContosoPizza.Configurations;
using ContosoPizza.Data;
using ContosoPizza.Services;
using ContosoPizza.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

const string myAllowSpecificOrigins = "_allowAll";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: myAllowSpecificOrigins,
        policy => { policy.WithOrigins().AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }
    );
});

var config = builder.Configuration;

// AddMvc covers everything.
// https://nitishkaushik.com/addmvc-vs-addcontrollerswithviews-vs-addcontrollers-vs-addrazorpages-asp-net-core/
builder.Services.AddMvc()
    .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
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
            new string[] { }
        }
    });
});

builder.Services.AddDbContext<ContosoContext>(opt =>
    opt.UseNpgsql("Host=localhost;Database=contoso_pizza;Username=msa;Password=vcrn;Include Error Detail=true"));

builder.Services.AddScoped<IPizzaService, PizzaService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IApiKeyService, ApiKeyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IMailService, MailService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<JwtConfig>(config.GetSection("JwtSettings"));
builder.Services.Configure<MailConfig>(config.GetSection("MailSettings"));

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
            ValidIssuer = config["JwtSettings:Issuer"],
            ValidAudience = config["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
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

builder.Services.AddAuthorization();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.CreateDbIfNotExists();

app.Run();