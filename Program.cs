using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiProyectoWeb
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// Unifica Windows Forms con el servidor de ASP.NET Core en segundo plano.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if USE_ASP_NET_CORE
            // Si defines la constante de compilación USE_ASP_NET_CORE y agregas
            // las referencias necesarias a ASP.NET Core (y migras a .NET 6+),
            // este código arrancará el servidor web en segundo plano.
            Task.Run(() => StartWebServer());
#endif

            // Tu base principal de Windows Forms (No se toca ni se borra)
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

#if USE_ASP_NET_CORE
        /// <summary>
        /// Inicializa el motor de ASP.NET Core para que tus Razor Pages, sesiones y captcha funcionen.
        /// (Sólo se compila si se define USE_ASP_NET_CORE)
        /// </summary>
        private static void StartWebServer()
        {
            var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder();

            // Configurar servicios necesarios para las Razor Pages y las Sesiones web
            builder.Services.AddRazorPages();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Configurar el puerto web local (ejemplo: http://localhost:5000)
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenLocalhost(5000);
            });

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();

            // Habilitar sesiones
            app.UseSession();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
#endif
    }
}