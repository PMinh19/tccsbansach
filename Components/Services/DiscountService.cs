using BanSach.Components.Data;
using BanSach.Components.IService;
using BanSach.Components.Model;
using Microsoft.EntityFrameworkCore;

namespace BanSach.Components.Services
{
    public class DiscountService: IDiscountServicecs
    {
        private readonly AppDbContext db;
        public DiscountService(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<List<Discount>> GetAllItem()
        {
            return await db.Discounts.ToListAsync();
        }
        public async Task<Discount> CreateItem(Discount Discount)
        {
            // Thêm Discount vào cơ sở dữ liệu
            db.Discounts.Add(Discount);
            await db.SaveChangesAsync();
            return Discount; // Trả về đối tượng Discount đã thêm
        }

        public async Task DeleteItem(Discount Discount)
        {
            db.Discounts.Remove(Discount);
            await db.SaveChangesAsync();
        }

        public async Task EditItem(Discount Discount)
        {
            db.Discounts.Update(Discount);
            await db.SaveChangesAsync();
        }
        public async Task<Discount?> GetItemById(int DiscountId)
        {
            var Discount = await db.Discounts.FirstOrDefaultAsync(p => p.DiscountId == DiscountId);
            if (Discount == null)
            {
                return null;
            }

            return Discount;
        }
    }
}
