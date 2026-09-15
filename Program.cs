
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using FluentValidation;
using FluentValidation.AspNetCore;
using WebApplication20260914.Data;
using WebApplication20260914.Configuration;
using WebApplication20260914.Controllers.Validators;
using WebApplication20260914.Masterdata.Repository;
using WebApplication20260914.Masterdata.Service;
using WebApplication20260914.Promotion.Repository;
using WebApplication20260914.Promotion.Service;

namespace WebApplication20260914
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<IValidateOptions<SqlServerOptions>, SqlServerOptionsValidator>();
            builder.Services.AddSingleton<IValidateOptions<RabbitMqOptions>, RabbitMqOptionsValidator>();

            builder.Services
                .AddOptions<SqlServerOptions>()
                .Bind(builder.Configuration.GetSection(SqlServerOptions.SectionName))
                .ValidateOnStart();

            builder.Services
                .AddOptions<RabbitMqOptions>()
                .Bind(builder.Configuration.GetSection(RabbitMqOptions.SectionName))
                .ValidateOnStart();

            builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                var sqlServerOptions = serviceProvider.GetRequiredService<IOptions<SqlServerOptions>>().Value;
                options.UseSqlServer(sqlServerOptions.ConnectionString);
            });

            // Add services to the container.

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddControllers();
            builder.Services.AddValidatorsFromAssemblyContaining<RabbitMqPublishRequestDtoValidator>();

            builder.Services.AddScoped<IFamilyRepository, FamilyRepository>();
            builder.Services.AddScoped<IItemRepository, ItemRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
            builder.Services.AddScoped<IPromotionParticipantRepository, PromotionParticipantRepository>();

            builder.Services.AddScoped<IFamilyService, FamilyService>();
            builder.Services.AddScoped<IItemService, ItemService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IPromotionService, PromotionService>();
            builder.Services.AddScoped<IPromotionParticipantService, PromotionParticipantService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
