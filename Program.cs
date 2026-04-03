using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SocialNetwork.Services;
using SocialNetwork.Services.Follow;

var builder = WebApplication.CreateBuilder(args);

// 🔥 Add Controller (backend chuẩn)
builder.Services.AddControllers();

// 🔥 Swagger (test API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔥 MySQL Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<IFollowService, FollowService>();

var app = builder.Build();

// 🔥 Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// 🔥 Middleware
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

// 🔥 Map Controller
app.MapControllers();

app.Run();