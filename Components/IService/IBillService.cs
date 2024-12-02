using BanSach.Components.Model;

namespace BanSach.Components.IService
{
    public interface IBillService
    {
        Task<List<Bill>> GetAllBill();
    }
}
