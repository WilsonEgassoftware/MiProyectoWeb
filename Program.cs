using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; // 👈 ESTA LÍNEA FALTABA

namespace MiProyectoWeb
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Arranca el servidor web en segundo plano
            Task.Run(() => StartWebServer());

            // Inicia Windows Forms
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void StartWebServer()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(options =>
                    {
                        options.ListenLocalhost(5000);

                        options.ListenLocalhost(5001, listenOptions =>
                        {
                            listenOptions.UseHttps();
                        });
                    });

                    webBuilder.ConfigureServices(services =>
                    {
                        services.AddRazorPages();

                        services.AddDistributedMemoryCache();

                        services.AddSession(options =>
                        {
                            options.IdleTimeout = TimeSpan.FromMinutes(20);
                            options.Cookie.HttpOnly = true;
                            options.Cookie.IsEssential = true;
                        });
                    });

                    webBuilder.Configure(app =>
                    {
                        app.UseHttpsRedirection();
                        app.UseStaticFiles();
                        app.UseRouting();
                        app.UseSession();

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapRazorPages();
                        });
                    });
                })
                .Build();

            host.Run();
        }
    }
}