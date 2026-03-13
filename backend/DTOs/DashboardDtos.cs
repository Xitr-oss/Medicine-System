namespace backend.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalMedicines { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
