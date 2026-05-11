using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore; // 👈 Referencia directa al helper nativo de ASP.NET Core
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
            // 🚀 Solución definitiva: Usamos 'WebHost.CreateDefaultBuilder'
            // Este método inicializa automáticamente el servidor Kestrel, las rutas de IIS y la configuración
            // de forma nativa sin requerir declarar manualmente la clase 'WebHostBuilder'.
            var host = WebHost.CreateDefaultBuilder()
                .UseKestrel(options =>
                {
                    options.ListenLocalhost(5000); // Puerto HTTP estándar
                    options.ListenLocalhost(5001, listenOptions =>
                    {
                        listenOptions.UseHttps(); // Habilita HTTPS de forma segura
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