using System.Linq;
using System.Threading.Tasks;
using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly OnlinemedicineorderingdbContext _context;

        public DashboardController(OnlinemedicineorderingdbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<DashboardStatsDto>> GetStats()
        {
            var totalMedicines = await _context.Medicines.CountAsync();
            var totalCustomers = await _context.Customers.CountAsync();
            var totalOrders = await _context.Orders.CountAsync();
            var totalRevenue = await _context.Orders
                .Where(o => o.Status != "Cancelled")
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0m;

            var stats = new DashboardStatsDto
            {
                TotalMedicines = totalMedicines,
                TotalCustomers = totalCustomers,
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue
            };

            return Ok(stats);
        }
    }
}
