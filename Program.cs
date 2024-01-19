using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using webapi.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

#region builder.Services
builder
    .Services
    .AddCors(options =>
    {
        options.AddPolicy(
            name: MyAllowSpecificOrigins,
            policy =>
            {
                policy
                    // .WithOrigins("https://localhost:5173, http://localhost:5173")
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                // .AllowCredentials();
            }
        );
    });

#region  Windows AD 身分認證 (二擇一)
// axios 目前沒試成功
// builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();

// builder
//     .Services
//     .AddAuthorization(options =>
//     {
//         options.FallbackPolicy = options.DefaultPolicy;
//     });
#endregion

#region JWT 身分認證 (二擇一)
builder.Services.AddAuthentication("Bearer").AddJwtBearer();
builder.Services.AddAuthorization();
#endregion

builder
    .Services
    .AddDbContext<YourProjectContext>(
        opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("YourProjectContext"))
    );
builder.Services.AddControllers();

// 自動產生文件、測試網頁 http://ip:port/swagger
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var securityScheme = new OpenApiSecurityScheme()
{
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "JSON Web Token based security",
};

var securityReq = new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] {}
    }
};


var info = new OpenApiInfo()
{
    Version = "v1",
    Title = "JWT with Swagger",
    Description = "要在 value 填入 Bearer {token} (只填toekn不行，前面要自行加上 Bearer和空格))",
};

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", info);
    o.AddSecurityDefinition("Bearer", securityScheme);
    o.AddSecurityRequirement(securityReq);
});
#endregion

#region app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Response to preflight request doesn't pass access control check: Redirect is not allowed for a preflight request.
// app.UseHttpsRedirection(); // 不能打開，否則會出現錯誤

app.UseStaticFiles();

//應用程式通常不需要呼叫 UseRouting 或 UseEndpoints
// app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

// 啟用身分認證
app.UseAuthentication(); // 驗證 who are you

app.UseAuthorization(); // 授權 what can you do

app.MapGet("/", () => "Hello, World!");

app.MapGet("/secret", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}. My secret")
    .RequireAuthorization();

app.MapControllers(); // REST API 的屬性路由

app.Run();
#endregion
