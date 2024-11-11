using EsyaCart.Data.Context;
using EsyaCart.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsyaCart.Pages.Admin
{
    public class AdminHomePageModel : PageModel
    {
        private readonly AppDbContext _context;

        public AdminHomePageModel(AppDbContext context) { _context = context; }

        public int UserCount { get; set; }
        public int VenderCount { get; set; }
        public List<string> Days { get; set; } = new List<string>();
        public List<double> TotalSales { get; set; } = new List<double>();
        public List<int> GroupedOrderCount { get; set; } = new List<int>();

        public async Task OnGetAsync()
        {
            UserCount = await _context.UserDetails.CountAsync();
            VenderCount = await _context.VendorDetails.CountAsync();

            var salesData = await _context.Orders
                .Where(o => o.OrderedDate >= DateTime.Now.AddDays(-7))
                .ToListAsync();

            var groupedSalesData = salesData
                .GroupBy(o => o.OrderedDate.DayOfWeek)
                .Select(g => new
                {
                    Day = g.Key.ToString(),
                    TotalSales = g.Sum(o => o.TotalPrice)
                })
                .ToList();

            var groupedOrders = salesData
                .GroupBy(o => o.OrderedDate.DayOfWeek)
                .Select(g => new
                {
                    Day = g.Key.ToString(),
                    TotalOrderCount = g.Count()
                })
                .ToList();

            Days = Enum.GetNames(typeof(DayOfWeek)).ToList();
            foreach (var day in Days)
            {
                var sales = groupedSalesData.FirstOrDefault(s => s.Day == day)?.TotalSales ?? 0;
                TotalSales.Add(sales);

                var orders = groupedOrders.FirstOrDefault(s => s.Day == day)?.TotalOrderCount ?? 0;
                GroupedOrderCount.Add(orders);
            }
        }
    }
}
