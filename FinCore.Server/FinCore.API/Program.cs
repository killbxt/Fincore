using FinCore.Data.Context;
using FinCore.Data.Repositories.CardsRepository;
using FinCore.Data.Repositories.ClientsRepository;
using FinCore.Data.Repositories.TransactionsRepository;
using FinCore.Services.AuthsService;
using FinCore.Services.CardsService;
using FinCore.Services.ClientsService;
using FinCore.Services.TransactionsService;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace FinCore.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<FinCoreDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FincoreDbConnectionString")));


            builder.Services.AddScoped<IClientRepository, ClientRepository>();
            builder.Services.AddScoped<ILogger<ClientRepository>, Logger<ClientRepository>>();

            builder.Services.AddScoped<ICardRepository, CardRepository>();
            builder.Services.AddScoped<ILogger<CardRepository>, Logger<CardRepository>>();

            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<ILogger<TransactionRepository>, Logger<TransactionRepository>>();

            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<ICardService, CardService>();

            builder.Services.AddScoped<ILogger<IClientService>, Logger<ClientService>>();
            builder.Services.AddScoped<ILogger<ITransactionRepository>, Logger<TransactionRepository>>();
            builder.Services.AddScoped<ILogger<ICardRepository>, Logger<CardRepository>>();

            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                   };
               });

            builder.Services.AddCors(c => c.AddPolicy("fincorepolicy", policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
            }));


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.UseCors("fincorepolicy");
            app.Run();
        }
    }
}
