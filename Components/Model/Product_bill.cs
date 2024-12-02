namespace BanSach.Components.Model
{
    public class Product_bill
    {
        public int UserId { get; set; }
        public int ProductBillId { get; set; }
        public int BillId { get; set; }
        public decimal Price { get; set; }
        public int FeaturePId { get; set; }
        public int Quantity { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public List<Product_cart> ProductCarts { get; set; }
        public string PaymentStatus { get; set; } = "Chưa thanh toán";
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending; 
    }

    // Enum trạng thái đơn hàng
    public enum OrderStatus
    {
        Pending,     // Đang chờ xử lý
        Processing,  // Đang xử lý
        Completed,   // Đã hoàn tất
        Cancelled    // Đã hủy
    }
}
