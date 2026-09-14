
using Microsoft.Extensions.Options;
using WebApplication20260914.Configuration;

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

            // Add services to the container.

            builder.Services.AddControllers();
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
