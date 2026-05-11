using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using MiProyectoWeb.Models;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;  

namespace MiProyectoWeb.Pages
{
    public class DashboardModel : PageModel
    {
        public string Usuario { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;

        private static List<Alimento> inventarioAlimentos = new List<Alimento>()
        {
            new Alimento { Id = 1, Nombre = "Arroz Super Extra (1kg)", Categoria = "Granos", Precio = 1.25, Stock = 50 },
            new Alimento { Id = 2, Nombre = "Leche Entera (1L)", Categoria = "Lácteos", Precio = 0.95, Stock = 30 },
            new Alimento { Id = 3, Nombre = "Atún Real en Aceite", Categoria = "Enlatados", Precio = 1.60, Stock = 40 },
            new Alimento { Id = 4, Nombre = "Aceite de Girasol (1L)", Categoria = "Grasas", Precio = 3.20, Stock = 15 }
        };

        public static List<Alimento> GetInventarioAlimentos() => inventarioAlimentos;

        public static void SetInventarioAlimentos(List<Alimento> value) => inventarioAlimentos = value;

        public List<ItemCarrito> Carrito { get; set; } = new List<ItemCarrito>();
        public double TotalPagar { get; set; }

        [BindProperty]
        public string NuevoNombre { get; set; } = string.Empty;
        [BindProperty]
        public string NuevaCategoria { get; set; } = string.Empty;
        [BindProperty]
        public double NuevoPrecio { get; set; }
        [BindProperty]
        public int NuevoStock { get; set; }

        public IActionResult OnGet()
        {
            Usuario = HttpContext?.Session?.GetString("Usuario") ?? string.Empty;
            Rol = HttpContext?.Session?.GetString("Rol") ?? string.Empty;

            if (string.IsNullOrEmpty(Usuario))
            {
                return RedirectToPage("/Login");
            }

            CargarCarrito();
            return Page();
        }

        public IActionResult OnPostAgregarAlCarrito(int productoId)
        {
            CargarCarrito();
            var producto = GetInventarioAlimentos().FirstOrDefault(a => a.Id == productoId);

            if (producto != null && producto.Stock > 0)
            {
                var item = Carrito.FirstOrDefault(c => c.Alimento.Id == productoId);
                if (item != null)
                {
                    if (item.Cantidad < producto.Stock) item.Cantidad++;
                }
                else
                {
                    Carrito.Add(new ItemCarrito { Alimento = producto, Cantidad = 1 });
                }

                GuardarCarrito();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostVaciarCarrito()
        {
            HttpContext?.Session?.Remove("Carrito");
            return RedirectToPage();
        }

        public IActionResult OnPostFinalizarCompra()
        {
            CargarCarrito();

            foreach (var item in Carrito)
            {
                var productoInventario = GetInventarioAlimentos().FirstOrDefault(a => a.Id == item.Alimento.Id);
                if (productoInventario != null)
                {
                    productoInventario.Stock -= item.Cantidad;
                }
            }

            HttpContext?.Session?.Remove("Carrito");
            TempData["MensajeExito"] = "¡Compra realizada con éxito! Se ha descontado del inventario.";
            return RedirectToPage();
        }

        public IActionResult OnPostAgregarProductoAdmin()
        {
            if (!string.IsNullOrEmpty(NuevoNombre) && NuevoPrecio > 0 && NuevoStock > 0)
            {
                int nuevoId = GetInventarioAlimentos().Any() ? GetInventarioAlimentos().Max(a => a.Id) + 1 : 1;
                GetInventarioAlimentos().Add(new Alimento
                {
                    Id = nuevoId,
                    Nombre = NuevoNombre,
                    Categoria = NuevaCategoria,
                    Precio = NuevoPrecio,
                    Stock = NuevoStock,
                    ImagenUrl = "🛒"
                });
            }
            return RedirectToPage();
        }

        public IActionResult OnPostLogout()
        {
            HttpContext?.Session?.Clear();
            return RedirectToPage("/Login");
        }

        private void CargarCarrito()
        {
            var carritoJson = HttpContext?.Session?.GetString("Carrito");
            if (!string.IsNullOrEmpty(carritoJson))
            {
                Carrito = JsonConvert.DeserializeObject<List<ItemCarrito>>(carritoJson) ?? new List<ItemCarrito>();
                TotalPagar = Carrito.Sum(c => c.Subtotal);
            }
        }

        private void GuardarCarrito()
        {
            var json = JsonConvert.SerializeObject(Carrito);
            HttpContext?.Session?.SetString("Carrito", json);
        }
    }
}

// Copia de seguridad renombrada para evitar conflicto de tipos con Pages\Dashboard.cshtml.cs.
// Este fichero ya no define DashboardModel en el namespace MiProyectoWeb.Pages.
// Si confirmas que todo compila bien, puedes borrar este archivo del proyecto.

namespace MiProyectoWeb.Internal
{
    // Clase backup para evitar duplicados en el namespace principal.
    internal class DashboardModelBackup
    {
        // Mantener aquí cualquier código histórico si lo deseas.
    }
}