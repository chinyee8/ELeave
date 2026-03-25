public class SubmitLeaveDto
{
    public string LeaveType { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Reason { get; set; }
}

public class ApproveLeaveDto
{
    public int RequestID { get; set; }
    // Action must be 'Approved' or 'Rejected'
    public string Action { get; set; } = string.Empty;
    public string? Note { get; set; }
}
