using EsyaCart.Data.Context;
using EsyaCart.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EsyaCart.Pages.Admin
{
    public class OrderListModel : PageModel
    {
        public readonly AppDbContext _context;

        public OrderListModel(AppDbContext context)
        {
            _context = context;
        }

        public List<AllOrderDetails> AllOrders { get; set; }

        public async Task OnGet()
        {
            AllOrders = await (from Orders in _context.Orders
                               join OrderItems in _context.OrderItems
                              on Orders.Order_id equals OrderItems.Order_Id
                               select new AllOrderDetails
                               {
                                   OrderItems_Id = OrderItems.OrderItems_Id,
                                   Product_Id = OrderItems.Product_Id,
                                   Customer_Id= Orders.Customer_Id,
                                   Quantity = OrderItems.Quantity,
                                   OrderedDate = Orders.OrderedDate,
                                   Price = OrderItems.Price,
                                   TotalPrice = Orders.TotalPrice,
                                   isDelivered = Orders.isDelivered
                               }).ToListAsync();
        }


    }
    public class AllOrderDetails
    {
        public int OrderItems_Id { get; set; }

        public int Product_Id { get; set; }

        public int Quantity { get; set; }
        public double Price { get; set; }

        public double TotalPrice { get; set; }
        public DateTime OrderedDate { get; set; }
        public int Customer_Id { get; set; } 
        public bool isDelivered { get; set; } = true;

    }
}
