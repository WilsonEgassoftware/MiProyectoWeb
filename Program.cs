// 🚨 ALIAS DE COMPATIBILIDAD CRUCIAL PARA .NET FRAMEWORK CLÁSICO
using WebApplication = Microsoft.AspNetCore.Builder.WebApplication;

using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

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
            // 1. Arranca el servidor web ASP.NET Core en segundo plano usando HTTPS (Puerto 5001)
            Task.Run(() => StartWebServer());
                
            // 2. Tu base principal de Windows Forms (No se toca ni se borra)
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        /// <summary>
        /// Inicializa el motor de ASP.NET Core para que tus Razor Pages, sesiones y validaciones funcionen.
        /// Configurado para correr de forma segura bajo HTTPS.
        /// </summary>
        private static void StartWebServer()
        {
            // Gracias al alias de la línea 2, el compilador sabe exactamente qué clase usar aquí
            var builder = WebApplication.CreateBuilder();

            // Configurar servicios necesarios para las Razor Pages y las Sesiones web
            builder.Services.AddRazorPages();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Configurar Kestrel para escuchar en HTTP (5000) y de forma segura en HTTPS (5001)
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenLocalhost(5000); // Puerto HTTP estándar
                options.ListenLocalhost(5001, listenOptions =>
                {
                    listenOptions.UseHttps(); // 👈 Activa la capa de encriptación SSL/TLS de forma segura
                });
            });

            var app = builder.Build();

            // Forzar redirección automática de HTTP a HTTPS
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            // Habilitar sesiones
            app.UseSession();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}