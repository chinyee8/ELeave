var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

// Session stores the JWT token after login
builder.Services.AddSession(opt => {
    opt.IdleTimeout = TimeSpan.FromHours(8);
    opt.Cookie.HttpOnly = true;
    opt.Cookie.IsEssential = true;
});

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Default page = Login
app.MapControllerRoute(name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

