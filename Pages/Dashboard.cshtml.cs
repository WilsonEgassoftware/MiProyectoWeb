using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using MiProyectoWeb.Models;

namespace MiProyectoWeb.Pages
{
    public class DashboardModel : PageModel
    {
        public string Usuario { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;

        // Datos para los Dropdowns Dinámicos
        public List<string> Categorias { get; set; } = new List<string> { "Granos", "Lácteos", "Enlatados" };

        // Diccionario de Subcategorías/Marcas según la Categoría seleccionada
        private static readonly Dictionary<string, List<string>> SubcategoriasPorCategoria = new Dictionary<string, List<string>>()
        {
            { "Granos", new List<string> { "Arroz Super Extra", "Lenteja Importada", "Garbanzo" } },
            { "Lácteos", new List<string> { "Leche Entera", "Yogurt Natural", "Queso Mozzarella" } },
            { "Enlatados", new List<string> { "Atún Real", "Sardinas Isabel", "Duraznos en Almíbar" } }
        };

        [BindProperty]
        public string NuevoNombre { get; set; } = string.Empty;
        [BindProperty]
        public string CategoriaSeleccionada { get; set; } = string.Empty;
        [BindProperty]
        public string SubcategoriaSeleccionada { get; set; } = string.Empty;
        [BindProperty]
        public double NuevoPrecio { get; set; }
        [BindProperty]
        public int NuevoStock { get; set; }
        
        [BindProperty]
        public string CedulaProveedor { get; set; } = string.Empty; // Dato Sensible a Validar

        public string ErrorMessage { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            Usuario = HttpContext.Session.GetString("Usuario") ?? string.Empty;
            Rol = HttpContext.Session.GetString("Rol") ?? string.Empty;

            if (string.IsNullOrEmpty(Usuario))
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }

        // Endpoint AJAX para cargar las subcategorías dinámicamente sin recargar la página
        public JsonResult OnGetSubcategorias(string categoria)
        {
            if (SubcategoriasPorCategoria.ContainsKey(categoria))
            {
                return new JsonResult(SubcategoriasPorCategoria[categoria]);
            }
            return new JsonResult(new List<string>());
        }

        public IActionResult OnPostAgregarProductoAdmin()
        {
            // 🚨 VALIDACIÓN BACK-END DEL DATO SENSIBLE (CÉDULA)
            if (!MiProyectoWeb.Helpers.CedulaValidator.ValidarCedulaEcuatoriana(CedulaProveedor))
            {
                ErrorMessage = "Error de Servidor: La Cédula del proveedor ingresada no es válida en Ecuador.";
                return Page();
            }

            // Validación de campos obligatorios
            if (string.IsNullOrEmpty(NuevoNombre) || NuevoPrecio <= 0 || NuevoStock <= 0)
            {
                ErrorMessage = "Por favor, llene todos los campos con valores válidos.";
                return Page();
            }

            // Si pasa las validaciones de Back-end, se registra el producto
            int nuevoId = InventarioAlimentos.Any() ? InventarioAlimentos.Max(a => a.Id) + 1 : 1;
            InventarioAlimentos.Add(new Alimento
            {
                Id = nuevoId,
                Nombre = $"{SubcategoriaSeleccionada} - {NuevoNombre}",
                Categoria = CategoriaSeleccionada,
                Precio = NuevoPrecio,
                Stock = NuevoStock,
                ImagenUrl = "🛒"
            });

            TempData["MensajeExito"] = "Producto registrado con éxito tras validar la cédula del proveedor.";
            return RedirectToPage();
        }

        // Simulación de Inventario Global estático
        public static List<Alimento> InventarioAlimentos { get; set; } = new List<Alimento>()
        {
            new Alimento { Id = 1, Nombre = "Arroz Super Extra (1kg)", Categoria = "Granos", Precio = 1.25, Stock = 50, ImagenUrl = "🌾" }
        };
    }
}

namespace MiProyectoWeb.Internal
{
    internal class DashboardModelBackup
    {
        // Copia de seguridad - no contiene la definición pública de DashboardModel
    }
}