namespace ReportesConDapper.Models
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public string EstadoVenta { get; set; }


        // Propiedad para los detalles de la venta
        public List<DetalleVenta> DetallesVenta { get; set; }


        public decimal SubTotal => DetallesVenta.Sum(d => d.TotalProducto);

        public decimal Impuesto => SubTotal * 0.15m; // Suponiendo un ISV del 15%

        public decimal Total => SubTotal + Impuesto;
    }
}
