namespace ELeaveAPI.Models
{
    public class LeaveBalance
    {
        public Guid BalanceID { get; set; } = Guid.NewGuid(); // <- auto-generate
        public Guid UserID { get; set; }
        public int Year { get; set; } = DateTime.Now.Year;
        public decimal Annual { get; set; } = 14;
        public decimal Sick { get; set; } = 14;
        public decimal Emergency { get; set; } = 3;
        public decimal Unpaid { get; set; } = 0;
        public User? User { get; set; }
    }

}
