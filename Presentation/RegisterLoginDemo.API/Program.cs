using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RegisterLoginDemo.Application.Abstraction.Service;
using RegisterLoginDemo.Application.Concrete.Repository;
using RegisterLoginDemo.Application.Data;
using RegisterLoginDemo.Domain.Abstraction.Repository;
using RegisterLoginDemo.Domain.Entities;
using RegisterLoginDemo.Infrastructure.Concrete.Service;
using RegisterLoginDemo.Persistance.Concrete;

namespace RegisterLoginDemo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<LoginDbContext>(opt => opt.UseSqlServer(connectionString,
            b => b.MigrationsAssembly("RegisterLoginDemo.Application")));

            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<LoginDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddHangfire(config => config.UseSqlServerStorage(connectionString));

            builder.Services.AddSingleton<ISmsService, SmsService>();
            builder.Services.AddSingleton<IEmailService, EmailService>();

            builder.Services.AddScoped<CustomizedUserManager<User>>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IVerificationCodeService, VerificationCodeService>();
            builder.Services.AddScoped<IVerificationCodeRepository, VerificationCodeRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHangfireDashboard();

            app.UseHangfireServer();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}