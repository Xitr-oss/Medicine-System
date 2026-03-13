using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly OnlinemedicineorderingdbContext _context;

        public OrdersController(OnlinemedicineorderingdbContext context)
        {
            _context = context;
        }

        // POST: api/orders
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<ActionResult<OrderDto>> PlaceOrder([FromBody] PlaceOrderDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int customerId))
            {
                return Unauthorized();
            }

            // Calculate total amount
            decimal totalAmount = 0;
            var orderItems = new List<Orderitem>();

            foreach (var itemDto in dto.Items)
            {
                var medicine = await _context.Medicines.FindAsync(itemDto.MedicineId);
                if (medicine == null || medicine.Stock < itemDto.Quantity)
                {
                    return BadRequest($"Medicine with ID {itemDto.MedicineId} is out of stock or does not exist.");
                }

                // Deduct stock
                medicine.Stock -= itemDto.Quantity;

                var lineTotal = medicine.Price * itemDto.Quantity;
                totalAmount += lineTotal;

                orderItems.Add(new Orderitem
                {
                    MedicineId = itemDto.MedicineId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = medicine.Price
                });
            }

            var order = new Order
            {
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = "Pending",
                Notes = dto.Notes,
                Orderitems = orderItems
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Order placed successfully", OrderId = order.Id });
        }

        // GET: api/orders/my-orders
        [Authorize(Roles = "Customer")]
        [HttpGet("my-orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetMyOrders()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int customerId))
            {
                return Unauthorized();
            }

            var orders = await _context.Orders
                .Include(o => o.Orderitems)
                .ThenInclude(oi => oi.Medicine)
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var result = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                Notes = o.Notes,
                Items = o.Orderitems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    MedicineId = oi.MedicineId,
                    MedicineName = oi.Medicine.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            });

            return Ok(result);
        }

        // GET: api/orders
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Orderitems)
                .ThenInclude(oi => oi.Medicine)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var result = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer.Name,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                Notes = o.Notes,
                Items = o.Orderitems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    MedicineId = oi.MedicineId,
                    MedicineName = oi.Medicine.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            });

            return Ok(result);
        }

        // PUT: api/orders/{id}/status
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto dto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = dto.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
