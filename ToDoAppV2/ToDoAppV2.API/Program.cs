
using ToDoAppV2.Application.Interfaces;
using ToDoAppV2.Application.Services;
using ToDoAppV2.Infrastructure.Data;
using ToDoAppV2.Infrastructure.Repositories;

namespace ToDoAppV2.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
                });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
            builder.Services.AddScoped<IToDoItemService, ToDoItemService>();

            builder.Services.AddInMemoryDataBase();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                SeedDatabase(dbContext);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowReactApp");
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
        public static void SeedDatabase(AppDbContext dbContext)
        {
            if (!dbContext.ToDoItems.Any())
            {
                dbContext.ToDoItems.AddRange(
                    new Domain.Entities.ToDoItem { Task = "Complete ASP.NET project", IsCompleted = false },
                    new Domain.Entities.ToDoItem { Task = "Read book", IsCompleted = false }
                    );
            }

            dbContext.SaveChanges();
        }


    }

    
}
