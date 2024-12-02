using BanSach.Components.Data;
using BanSach.Components.IService;
using BanSach.Components.Model;
using Microsoft.EntityFrameworkCore;

namespace BanSach.Components.Services
{
    public class BillService: IBillService
    {
      
            private readonly AppDbContext db;
            public BillService(AppDbContext _db)
            {
                db = _db;
            }
            public async Task<List<Bill>> GetAllBill()
            {
                return await db.Bill.ToListAsync();
            }
        }
}
