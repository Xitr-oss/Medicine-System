using System;
using System.Collections.Generic;

namespace backend.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }

        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }

    public class OrderItemDto
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public string? MedicineName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class PlaceOrderDto
    {
        public string? Notes { get; set; }
        public List<PlaceOrderItemDto> Items { get; set; } = new List<PlaceOrderItemDto>();
    }

    public class PlaceOrderItemDto
    {
        public int MedicineId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateOrderStatusDto
    {
        public string Status { get; set; } = null!;
    }
}
