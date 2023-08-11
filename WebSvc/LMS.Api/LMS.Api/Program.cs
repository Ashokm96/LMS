using LMS.Api.Entities;
using LMS.Api.Models;
using LMS.Api.Repository.Implementation;
using LMS.Api.Repository.Interface;
using LMS.Api.Services.Contract;
using LMS.Api.Services.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var log4netRepository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
log4net.Config.XmlConfigurator.Configure(log4netRepository, new FileInfo("log4net.config"));

builder.Services.AddControllers();
builder.Services.AddCors(options => 
{ 
    options.AddPolicy("CORSPolicy", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((hosts) => true));
});

//Add  Swagger
//builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Learning Management System", Version = "v1" }); });
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


//JWT Authenticaton and Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireRole("Admin");
    });
    options.AddPolicy("UserPolicy", policy =>
    {
        policy.RequireRole("User");
    });
    options.AddPolicy("BothRolesPolicy", policy =>
    policy.RequireAssertion(context =>
        context.User.HasClaim(c =>
            (c.Type == "AdminPolicy" ||
             c.Type == "UserPolicy"))));

});


//Add services to the container.
builder.Services.Configure<DBConfiguration>(builder.Configuration.GetSection("DBConfiguration"));
builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
// Add repositories
builder.Services.AddTransient<ILMSRepository, LMSRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
// Add services
builder.Services.AddTransient<ILMSService, LMSService>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddEndpointsApiExplorer();
builder.Host.ConfigureLogging(logger => {
    logger.ClearProviders();
    logger.AddLog4Net("log4net.config");
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Learning Management System v1"));
}
app.UseCors("CORSPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
