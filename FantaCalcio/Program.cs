using FantaCalcio.Data;
using FantaCalcio.Models;
using FantaCalcio.Services;
using FantaCalcio.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var ConnString = builder.Configuration.GetConnectionString("AppDb")!;
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(ConnString));


// definizione delle regole di autenticazione
string key = builder.Configuration["Jwt:Key"]!;
var bytesKey = System.Text.Encoding.UTF8.GetBytes(key);

builder.Services
    .AddAuthentication(opt => {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opt => { // configurazione delle caratteristiche del token JWT
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(bytesKey)
        };
    });

builder.Services.AddAuthorization();


//CONFIGURAZIONE Services 
builder.Services.AddScoped<IGiocatoreService,GiocatoriService>();
builder.Services.AddScoped<IModalitaService, ModalitaService>();
builder.Services.AddScoped<ITipoAstaService, TipoAstaService>();
builder.Services.AddScoped<IRuoloService, RuoloService>();
builder.Services.AddScoped<IRuoloMantraService, RuoloMantraService>();
builder.Services.AddScoped<IAstaService, AstaService>();




var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

