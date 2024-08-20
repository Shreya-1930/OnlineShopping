using Microsoft.AspNetCore.Authentication.Cookies;
using OnlineShopping;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddOutputCache();
builder.Services.AddControllersWithViews(
    c => c.Filters.Add(new CustomExceptionFilterAttribute()));
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(c => { c.LoginPath = "/Security/Login"; c.AccessDeniedPath = "/Security/accessdeniedpath"; });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseOutputCache();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=OnlineShop}/{action=Home}/{id?}");

app.Run();
