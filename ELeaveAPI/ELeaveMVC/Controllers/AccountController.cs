using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _factory;
    private readonly IConfiguration _config;

    public AccountController(IHttpClientFactory f, IConfiguration c)
    { _factory = f; _config = c; }

    // ------------------- LOGIN -------------------
    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var api = new ApiHelper(_factory, _config);
        var (ok, body) = await api.PostAsync("auth/login",
            new { username, password });

        if (!ok)
        {
            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        dynamic? result = JsonConvert.DeserializeObject(body);
        HttpContext.Session.SetString("Token", (string)result!.token);
        HttpContext.Session.SetString("FullName", (string)result!.fullName);
        HttpContext.Session.SetString("Role", (string)result!.role);

        return RedirectToAction("Dashboard", "Leave");
    }

    // ------------------- SIGNUP -------------------
    [HttpGet]
    public IActionResult Signup() => View();

    [HttpPost]
    public async Task<IActionResult> Signup(string fullName, string username, string password)
    {
        var api = new ApiHelper(_factory, _config);
        var (ok, body) = await api.PostAsync("auth/signup",
            new { FullName = fullName, Username = username, Password = password });

        if (!ok)
        {
            ViewBag.Error = "Signup failed. Username might be taken.";
            return View();
        }

        TempData["Message"] = "Signup successful! Please login.";
        return RedirectToAction("Login");
    }

    // ------------------- LOGOUT -------------------

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
