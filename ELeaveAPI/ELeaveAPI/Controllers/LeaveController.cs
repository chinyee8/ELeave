using ELeaveAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LeaveController : ControllerBase
{
    private readonly AppDbContext _db;
    public LeaveController(AppDbContext db) => _db = db;

    // Helper: gets the logged-in user's ID from the JWT token
    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // GET api/leave/my-requests  — employee sees own requests
    [HttpGet("my-requests")]
    public async Task<IActionResult> MyRequests()
    {
        var list = await _db.LeaveRequests
            .Where(r => r.UserID == GetUserId())
            .Include(r => r.Approver)
            .OrderByDescending(r => r.SubmittedDate)
            .ToListAsync();
        return Ok(list);
    }

    // POST api/leave/submit  — employee submits a new request
    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromBody] SubmitLeaveDto dto)
    {
        var days = (decimal)(dto.EndDate - dto.StartDate).Days + 1;
        var req = new LeaveRequest
        {
            UserID = GetUserId(),
            LeaveType = dto.LeaveType,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            TotalDays = days,
            Reason = dto.Reason
        };
        _db.LeaveRequests.Add(req);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Request submitted", req.RequestID });
    }

    // GET api/leave/pending  — manager sees all pending requests
    [HttpGet("pending")]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> Pending()
    {
        var list = await _db.LeaveRequests
            .Where(r => r.Status == "Pending")
            .Include(r => r.User)
            .OrderBy(r => r.SubmittedDate)
            .ToListAsync();
        return Ok(list);
    }

    // POST api/leave/approve  — manager approves or rejects
    [HttpPost("approve")]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> Approve([FromBody] ApproveLeaveDto dto)
    {
        var req = await _db.LeaveRequests.FindAsync(dto.RequestID);
        if (req == null) return NotFound(new { message = "Request not found" });

        req.Status = dto.Action;
        req.ApprovedBy = GetUserId();
        req.ApprovalDate = DateTime.Now;
        req.ApprovalNote = dto.Note;

        await _db.SaveChangesAsync();
        return Ok(new { message = $"Request {dto.Action} successfully" });
    }

    // GET api/leave/balance  — employee sees own leave balance
    [HttpGet("balance")]
    public async Task<IActionResult> Balance()
    {
        var bal = await _db.LeaveBalances
            .FirstOrDefaultAsync(b =>
                b.UserID == GetUserId() &&
                b.Year == DateTime.Now.Year);
        return Ok(bal);
    }
}
