using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PBL6.Mapping;
using PBL6.Repositories.IRepository;
using PBL6.Repositories.Repository;
using PBL6.Services.IService;
using PBL6.Services.Service;
using PBL6.Services.ServiceImpl;
using PBL6_BackEnd.Config;
using PBL6_BackEnd.Services.Service;
using PBL6_BackEnd.Services.ServiceImpl;
using PBL6_QLBH.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IVnPayService, VnPayService>();

builder.Services.AddScoped<IMomoService, MomoService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<IReviewService,ReviewService>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
builder.Services.Configure<ZaloPayConfig>(builder.Configuration.GetSection(ZaloPayConfig.ConfigName));
builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<ZaloPayConfig>>().Value);
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDB"));
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
