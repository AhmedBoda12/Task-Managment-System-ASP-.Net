using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Task_Managment.DAL.Data;
using Task_Managment.DAL.Helpers;
using Task_Managment.DAL.Models;
using Task_Managment.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddBusinessLayerServices();
builder.Services.AddControllers();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultEmailProvider;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddCustomJwtAuthentication(builder.Configuration);
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
        builder.SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});
builder.Services.AddSwaggerGen(o =>
{

//o.SwaggerDoc("v1", new OpenApiInfo
//{
//    Version = "v1",
//    Title = "TaskManagement",
//    Description = "QSIT Backend Workshop ",
//    TermsOfService = new Uri("https://qsitint.com/who-we-are/"),
//    Contact = new OpenApiContact
//    {
//        Name = "Ahmed Aboueelella",
//        Email = "ahmedabouelella98888@gmail.com",
//        Url = new Uri("https://www.linkedin.com/in/ahmedthewalker/")
//    },
//    License = new OpenApiLicense
//    {
//        Name = "License",
//        Url = new Uri("https://opensource.org/license/bsd-1-clause")
//    }
//});
o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
{
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
});
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
var app = builder.Build();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<Task_Managment.Middleware.ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
