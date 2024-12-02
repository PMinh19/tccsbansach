using BanSach.Components.Data;
using BanSach.Components.IService;
using BanSach.Components.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BanSach.Components.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext db;
        private readonly HttpClient http;

        public CartService(AppDbContext _db, HttpClient _http)
        {
            db = _db;
            http = _http;
        }

        public async Task<List<Product_cart>> GetAllPCart()
        {
            return await db.Product_carts.Include(p => p.Product).ToListAsync();
        }

        public async Task<Product_cart> CreatePCart(Product_cart productCart)
        {
            var existingProductCart = await db.Product_carts
                .FirstOrDefaultAsync(pc => pc.UserId == productCart.UserId && pc.ProductId == productCart.ProductId);

            if (existingProductCart != null)
            {
                existingProductCart.Quantity += productCart.Quantity;
                existingProductCart.Updated = DateTime.Now;
            }
            else
            {
                productCart.Created = DateTime.Now;
                productCart.Updated = DateTime.Now;
                db.Product_carts.Add(productCart);
            }

            await db.SaveChangesAsync();
            return productCart;
        }

        public async Task DeletePCart(Product_cart productCart)
        {
            var cartItem = await db.Product_carts
                                    .FirstOrDefaultAsync(pc => pc.UserId == productCart.UserId && pc.ProductId == productCart.ProductId);

            if (cartItem != null)
            {
                db.Product_carts.Remove(cartItem);
                await db.SaveChangesAsync();
            }
        }

        public async Task<Product_bill> PlaceProductBill(Product_bill productBill)
        {
            if (string.IsNullOrEmpty(productBill.PaymentStatus))
            {
                productBill.PaymentStatus = "Chưa thanh toán";
            }

            db.Product_bills.Add(productBill);
            await db.SaveChangesAsync();

            var cartItems = await db.Product_carts.Where(p => p.UserId == productBill.UserId).ToListAsync();
            foreach (var item in cartItems)
            {
                item.IsCheckedOut = true;
            }

            await db.SaveChangesAsync();
            return productBill;
        }

        public async Task<Product_bill> GetProductBillById(int productBillId)
        {
            return await db.Product_bills
                    .Include(pb => pb.ProductCarts)
                    .FirstOrDefaultAsync(pb => pb.ProductBillId == productBillId);
        }

        public async Task<PaymentResult> ProcessPayment(int productBillId, string paymentMethod)
        {
            try
            {
                var productBill = await db.Product_bills.FirstOrDefaultAsync(pb => pb.ProductBillId == productBillId);

                if (productBill == null)
                {
                    return new PaymentResult { IsSuccess = false, Message = "Hóa đơn không tồn tại." };
                }

                if (paymentMethod == "Tiền mặt")
                {
                    productBill.PaymentStatus = "Đã đặt đơn và xác nhận thanh toán bằng tiền mặt khi nhận hàng.";
                }
                else if (paymentMethod == "Ví điện tử")
                {
                    productBill.PaymentStatus = "Đã đặt đơn và chờ thanh toán qua ví điện tử.";
                }
                else
                {
                    return new PaymentResult { IsSuccess = false, Message = "Phương thức thanh toán không hợp lệ." };
                }

                await db.SaveChangesAsync();
                return new PaymentResult { IsSuccess = true, Message = "Thanh toán thành công." };
            }
            catch (Exception ex)
            {
                return new PaymentResult { IsSuccess = false, Message = $"Lỗi trong quá trình thanh toán: {ex.Message}" };
            }
        }

        public async Task<int> SaveShippingAddress(Address address)
        {
            db.Address.Add(address);
            await db.SaveChangesAsync();
            return address.AddressId;
        }

        public async Task<ServiceResponse<int>> CreateBill(int addressId, int userId, decimal totalPrice, string note)
        {
            var response = new ServiceResponse<int>();
            try
            {
                var bill = new Bill
                {
                    AddressId = addressId,
                    TotalPrice = (int)totalPrice,
                    Note = note,
                    Status = 0,
                    Created_at = DateTime.Now
                };

                db.Bill.Add(bill);
                await db.SaveChangesAsync();
                response.Data = bill.BillId;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Lỗi khi tạo hóa đơn: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> LinkBillToProductBill(int billId, List<Product_bill> productBills)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                foreach (var productBill in productBills)
                {
                    productBill.BillId = billId;
                    productBill.Updated = DateTime.Now;
                    db.Product_bills.Update(productBill);
                }

                await db.SaveChangesAsync();
                response.Data = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Lỗi khi liên kết hóa đơn: {ex.Message}";
            }
            return response;
        }

        public async Task SaveAddress(Address address)
        {
            db.Add(address);
            await db.SaveChangesAsync();
        }

        public async Task<bool> UpdateBillAsync(Bill bill)
        {
            try
            {
                await db.Bill.AddAsync(bill);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Không thể cập nhật hóa đơn: " + ex.Message);
            }
        }
    }
}
