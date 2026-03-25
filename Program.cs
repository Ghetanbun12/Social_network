using Microsoft.EntityFrameworkCore;

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

var app = builder.Build();

// 🔥 Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// 🔥 Middleware
app.UseHttpsRedirection();

app.UseAuthorization();

// 🔥 Map Controller
app.MapControllers();

app.Run();