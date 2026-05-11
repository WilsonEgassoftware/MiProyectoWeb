using System;

namespace MiProyectoWeb.Models
{
    // Modelo público para ser usado desde las Razor Pages
    public class Alimento
    {
        // Nombres de propiedades alineados con el uso en Dashboard.cshtml.cs
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public double Precio { get; set; }
        public int Stock { get; set; }
        public string ImagenUrl { get; set; } = string.Empty;
    }
}
