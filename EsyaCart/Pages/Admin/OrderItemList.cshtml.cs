using EsyaCart.Data.Context;
using EsyaCart.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EsyaCart.Pages.Admin
{
    public class OrderItemListModel : PageModel
    {
        public readonly AppDbContext _context;

        public OrderItemListModel(AppDbContext context)
        {
            _context = context;
        }
        public int Order_Id { get; set; }

        public List<Orders> orders { get; set; } = new List<Orders>();
        public List<OrderItems> items { get; set; } = new List<OrderItems>();
        public void OnGet(int Order_id)
        {
            items = _context.OrderItems.ToList();
            var order_item = _context.Orders.Find(Order_id);
            Order_Id = order_item.Order_id;
        }
    }
}
