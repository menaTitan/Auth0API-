using Auth0Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("QuoteDB");

// Add services to the container.

builder.Services.AddControllers();

//1.Choose a JWT library
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = "https://dev-ubbqh8wm.us.auth0.com/";
    options.Audience = "https://localhost:7251/";
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// add the xml format
builder.Services.AddMvc().AddXmlSerializerFormatters();
builder.Services.AddDbContext<QuotesDbContext>(opt => opt.UseSqlServer(connectionString));

//using the response caching 
builder.Services.AddResponseCaching();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 2. Enable authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//using the response caching 
app.UseResponseCaching();

app.Run();
