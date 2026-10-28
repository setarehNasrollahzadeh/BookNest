
using BookNest.Data;
using BookNest.Services.Categories;
using Microsoft.EntityFrameworkCore;

namespace BookNest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.



            //...
            //Register BookNestDbContext
            builder.Services.AddDbContext<BookNestDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("BookNestConnection") ?? throw new InvalidOperationException("Connection string" + "'BookNestConnection' not found."));
            });

            //...
            //Register services that we want to use in controllers
            builder.Services.AddScoped<ICategoriesService, CategoriesService>();



            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
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
