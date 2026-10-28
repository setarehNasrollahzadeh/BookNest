using BookNest.Client.Components;
using BookNest.Client.Services.Categories;

namespace BookNest.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            //...
            // Registers API HttpClient
            builder.Services.AddHttpClient("BookNestApi", client =>
            {
                var apiBaseUrl = builder.Configuration["ApiBaseUrl"];

                if (string.IsNullOrWhiteSpace(apiBaseUrl))
                {
                    throw new InvalidOperationException(
                        "ApiBaseUrl is not configured.");
                }

                client.BaseAddress = new Uri(apiBaseUrl);
            });

            //...
            // Registers CategoriesApiService
            builder.Services.AddScoped<CategoriesApiService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
