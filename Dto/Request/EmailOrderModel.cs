using PBL6_QLBH.Models;

namespace PBL6_QLBH.Dto.Request
{
    public class EmailOrderModel
    {
        public decimal TotalAmount { get; set; }  // Tổng tiền thanh toán
        public IEnumerable<OrderDetail> Orders { get; set; }  // Chi tiết đơn hàng (sản phẩm, số lượng, giá, ...)
        public DateTime? OrderDate { get; set; }  // Ngày đặt hàng
        public string OrderNumber { get; set; }  // Số hóa đơn
        public string CustomerName { get; set; }  // Tên khách hàng
        public string CustomerAddress { get; set; }  // Địa chỉ khách hàng
        public string CustomerPhone { get; set; }  // Số điện thoại khách hàng
        public string CustomerEmail { get; set; }  // Email khách hàng
        public string SellerName { get; set; }  // Tên người bán
        public string SellerAddress { get; set; }  // Địa chỉ người bán
        public string SellerPhone { get; set; }  // Số điện thoại người bán
        public string SellerEmail { get; set; }  // Email người bán
        public decimal? Discount { get; set; }  // Giảm giá
        public decimal? Tax { get; set; }  // Thuế (nếu có)
    }
}
