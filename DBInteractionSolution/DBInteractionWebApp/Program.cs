using DBInteractionSystem;
using DBInteractionWebApp.Components;
using Microsoft.EntityFrameworkCore;

namespace DBInteractionWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Retrieve the connection string from the appsetting.json
            // The connection string will be passed to the class library using the encapsulated
            //      extension method
            // The connection string will be registered to get access to the database
            var connectionString = builder.Configuration.GetConnectionString("WWDB");


            builder.Services.WestWindExtensionServices(options => options.UseSqlServer(connectionString));


            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
