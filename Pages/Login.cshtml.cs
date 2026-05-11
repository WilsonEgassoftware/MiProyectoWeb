using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace MiProyectoWeb.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        // Mensaje mostrado en la vista en caso de error
        public string ErrorMessage { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            // Si ya hay sesión activa redirige al dashboard
            var usuario = HttpContext?.Session?.GetString("Usuario");
            if (!string.IsNullOrEmpty(usuario))
            {
                return RedirectToPage("/Dashboard");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Usuario y contraseña son obligatorios.";
                return Page();
            }

            // Usuarios de prueba
            if (Username == "admin" && Password == "admin123")
            {
                HttpContext.Session.SetString("Usuario", "admin");
                HttpContext.Session.SetString("Rol", "Admin");
                return RedirectToPage("/Dashboard");
            }

            if (Username == "user" && Password == "user123")
            {
                HttpContext.Session.SetString("Usuario", "user");
                HttpContext.Session.SetString("Rol", "Cliente");
                return RedirectToPage("/Dashboard");
            }

            ErrorMessage = "Credenciales incorrectas.";
            return Page();
        }
    }
}
