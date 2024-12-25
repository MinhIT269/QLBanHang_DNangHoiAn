var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(30);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});

//Đăng nhập 
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/User/Auth/Login"; // Đường dẫn trang login
        options.ExpireTimeSpan = TimeSpan.FromHours(2); // Thời gian hết hạn cookie
        options.SlidingExpiration = true; // Gia hạn cookie nếu người dùng còn hoạt động
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

string wwwroot = app.Environment.WebRootPath;
Rotativa.AspNetCore.RotativaConfiguration.Setup(wwwroot, "rotativa");

app.Run();