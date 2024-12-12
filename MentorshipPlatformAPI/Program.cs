using MentorshipPlatformAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// JWT Authentication configuration
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
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")  // Corrected URL
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add controllers and authorization services
builder.Services.AddControllers();
builder.Services.AddAuthorization();

// MySQL Database connection configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Apply CORS policy before routing
app.UseCors("AllowSpecificOrigin");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
