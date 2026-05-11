using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiProyectoWeb.Models
{
    // Clase para representar un ítem en el carrito (coincide con Dashboard.cshtml.cs)
    public class ItemCarrito
    {
        public Alimento Alimento { get; set; } = new Alimento();
        public int Cantidad { get; set; }

        // Propiedad calculada para obtener el subtotal del ítem
        public double Subtotal => Alimento.Precio * Cantidad;
    }
}
