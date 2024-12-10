using BanSach.Components.Data;
using BanSach.Components.IService;
using BanSach.Components.Model;
using Microsoft.EntityFrameworkCore;

namespace BanSach.Components.Services
{
    public class BillService : IBillService
    {

        private readonly AppDbContext db;
        public BillService(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<List<Product_bill>> GetAllProduct_bill()
        {
            return await db.Product_bills.ToListAsync();
        }
        public async Task DeleteProduct_bill(Product_bill Product_bill)
        {
            db.Product_bills.Remove(Product_bill);
            await db.SaveChangesAsync();
        }

        public async Task EditProduct_bill(Product_bill Product_bill)
        {
            db.Product_bills.Update(Product_bill);
            await db.SaveChangesAsync();
        }
        public async Task<List<Bill>> GetAllbill()
        {
            return await db.Bill.ToListAsync();
        }
        public async Task Deletbill(Bill bill)
        {
            db.Bill.Remove(bill);
            await db.SaveChangesAsync();
        }

        public async Task Editbill(Bill  bill)
        {
            db.Bill.Update(bill);
            await db.SaveChangesAsync();
        }
        public async Task<List<Delivery>> GetAllDelivery()
        {
            return await db.Deliveries.ToListAsync();
        }
        public async Task<Delivery?> GetDeById(int DeId)
        {
            var d = await db.Deliveries.FirstOrDefaultAsync(p => p.DeliveryId == DeId);
            if (d == null)
            {
                return null;
            }

            return d;
        }
    }
}
