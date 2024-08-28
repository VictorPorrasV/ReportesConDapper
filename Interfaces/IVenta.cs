using ReportesConDapper.Models;

namespace ReportesConDapper.Interfaces
{
    public interface IVenta
    {
        Task <IEnumerable<Venta>> GetAllVentas();
        Task<Venta> GetVentaConDetallesAsync(int id);

    }
}
