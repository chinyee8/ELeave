namespace ELeaveAPI.Models
{
    public class LeaveRequest
    {
        public Guid RequestID { get; set; } = Guid.NewGuid();
        public Guid UserID { get; set; }
        public string LeaveType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalDays { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; } = "Pending";
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? ApprovalNote { get; set; }
        public DateTime SubmittedDate { get; set; } = DateTime.Now;
        // Navigation — filled when we Include() related data
        public User? User { get; set; }
        public User? Approver { get; set; }
    }

}
