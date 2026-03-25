using ELeaveMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class LeaveController : Controller
{
    private readonly IHttpClientFactory _factory;
    private readonly IConfiguration _config;

    public LeaveController(IHttpClientFactory f, IConfiguration c)
    { _factory = f; _config = c; }

    private ApiHelper GetApi()
    {
        var api = new ApiHelper(_factory, _config);
        var token = HttpContext.Session.GetString("Token") ?? string.Empty;
        api.SetToken(token);
        return api;
    }

    public async Task<IActionResult> Dashboard()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            return RedirectToAction("Login", "Account");

        var api = GetApi();
        var requests = await api.GetAsync<List<LeaveRequest>>("leave/my-requests");
        var balance = await api.GetAsync<dynamic>("leave/balance");

        ViewBag.FullName = HttpContext.Session.GetString("FullName");
        ViewBag.Balance = balance;
        return View(requests ?? new List<LeaveRequest>());
    }

    [HttpGet]
    public IActionResult Submit() => View();

    [HttpPost]
    public async Task<IActionResult> Submit(
        string leaveType, DateTime startDate,
        DateTime endDate, string? reason)
    {
        var api = GetApi();
        var (ok, _) = await api.PostAsync("leave/submit",
            new { leaveType, startDate, endDate, reason });

        TempData["Message"] = ok
            ? "Leave request submitted successfully!"
            : "Error submitting request. Please try again.";
        TempData["IsError"] = (!ok).ToString();

        return RedirectToAction("Dashboard");
    }
}
