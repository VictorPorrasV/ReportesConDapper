using Dapper;
using Microsoft.Data.SqlClient;
using ReportesConDapper.Interfaces;
using ReportesConDapper.Models;
using System.Data;

namespace ReportesConDapper.Repositorio
{
    public class VentaRepositorio : IVenta
    {
        private readonly string connectionString;

        public VentaRepositorio(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task<IEnumerable<Venta>> GetAllVentas()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return await db.QueryAsync<Venta>("SELECT * FROM Ventas");
            }
        }

        public async Task<Venta> GetVentaConDetallesAsync(int id)
        {
            var query = @"
        SELECT v.*, dv.*, p.* 
        FROM Ventas v
        INNER JOIN DetallesVenta dv ON v.IdVenta = dv.IdVenta
        INNER JOIN Productos p ON dv.IdProducto = p.IdProducto
        WHERE v.IdVenta = @Id;
    ";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var ventaDict = new Dictionary<int, Venta>();

                var result = await db.QueryAsync<Venta, DetalleVenta, Producto, Venta>(
                    query,
                    (venta, detalleVenta, producto) =>
                    {
                        if (!ventaDict.TryGetValue(venta.IdVenta, out var currentVenta))
                        {
                            currentVenta = venta;
                            currentVenta.DetallesVenta = new List<DetalleVenta>();
                            ventaDict.Add(currentVenta.IdVenta, currentVenta);
                        }

                        detalleVenta.IdProductoNavigation = producto;
                        currentVenta.DetallesVenta.Add(detalleVenta);
                        return currentVenta;
                    },
                    new { Id = id },
                    splitOn: "IdVenta,IdProducto");

                return result.FirstOrDefault();
            }
        }

    
    }
}
