using triincom.Application.Services;
using triincom.Core.Interface;
using triincom.DataPersistence;
using triincom.Middlewares;
using triincom.Application;
using triincom.Infrastructure.Repositories;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                builder =>
                {
                    builder.WithOrigins("http://localhost:5173")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
        });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDatabaseContext(builder.Configuration);
        builder.Services.AddMapperServices();

        builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
        builder.Services.AddScoped<IApplicationService, ApplicationService>();

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ResponseFilter>();
        });



        var app = builder.Build();

        app.UseCors("AllowFrontend");
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }


        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.UseMiddleware<ExceptionHandlerMiddleware>();

        app.Run();
    }
}