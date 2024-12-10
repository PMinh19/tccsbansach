using BanSach.Components.Model;

namespace BanSach.Components.IService
{
    public interface IBillService
    {
        Task<List<Product_bill>> GetAllProduct_bill();
        Task DeleteProduct_bill(Product_bill Product_bill);
        Task EditProduct_bill(Product_bill Product_bill);
        Task<List<Bill>> GetAllbill();

        Task Deletbill(Bill bill);
        Task Editbill(Bill bill);
        Task<Delivery?> GetDeById(int DeId);
        Task<List<Delivery>> GetAllDelivery();
    }
}
