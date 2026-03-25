namespace ELeaveMVC.Models
{
    public class LeaveRequest
    {
        public int RequestID { get; set; }
        public string LeaveType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalDays { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime SubmittedDate { get; set; }
        public string? ApprovalNote { get; set; }
    }

}
