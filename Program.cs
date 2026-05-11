using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MiProyectoWeb
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // 1. Arranca el servidor web integrado de fondo en HTTPS (Puerto 5001)
            Task.Run(() => StartWebServer());

            // 2. Tu interfaz de Windows Forms clásica libre de errores
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void StartWebServer()
        {
            // Usamos WebHostBuilder que es 100% nativo y compatible con tu versión de .NET Framework
            var host = new WebHostBuilder()
                .UseKestrel(options =>
                {
                    options.ListenLocalhost(5000); // Puerto HTTP
                    options.ListenLocalhost(5001, listenOptions =>
                    {
                        listenOptions.UseHttps(); // 👈 Habilita HTTPS de forma segura
                    });
                })
                .ConfigureServices(services =>
                {
                    services.AddRazorPages();
                    services.AddDistributedMemoryCache();
                    services.AddSession(options =>
                    {
                        options.IdleTimeout = TimeSpan.FromMinutes(20);
                        options.Cookie.HttpOnly = true;
                        options.Cookie.IsEssential = true;
                    });
                })
                .Configure(app =>
                {
                    app.UseHttpsRedirection(); // Forzar redirección HTTPS
                    app.UseStaticFiles();
                    app.UseRouting();
                    app.UseSession();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapRazorPages();
                    });
                })
                .Build();

            host.Run();
        }
    }
}