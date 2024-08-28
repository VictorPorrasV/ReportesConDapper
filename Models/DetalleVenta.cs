namespace ReportesConDapper.Models
{
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }

        // Propiedad de navegación para el producto
        public Producto IdProductoNavigation { get; set; }


        public decimal TotalProducto => Cantidad * IdProductoNavigation.Precio;


    }
}
