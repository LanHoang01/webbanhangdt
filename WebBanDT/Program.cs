using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebBanDT.Models;
using WebBanDT.Repository;

var builder = WebApplication.CreateBuilder(args);

// Contection Db
builder.Services.AddDbContext<DataContect>(options =>
{
	options.UseSqlServer(builder.Configuration["ConnectionStrings:ConnectedDb"]);//cấu hình file
});
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.IsEssential = true;
});


builder.Services.AddIdentity<AppUserModel, IdentityRole>()
	.AddEntityFrameworkStores<DataContect>().AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(options =>
{
	// Password settings.
	options.Password.RequireDigit = true; // kiểu số
	options.Password.RequireLowercase = true;// chữ thường
	options.Password.RequireNonAlphanumeric = false;//ký tự đặc biệt
	options.Password.RequireUppercase = false;//chữ hoa
	options.Password.RequiredLength = 4;

	options.User.RequireUniqueEmail = true;
});


var app = builder.Build();

app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");
app.UseSession();
app.UseStaticFiles();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}




app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();//xác thực
app.UseAuthorization();//xác thực quyền

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "category",
    pattern: "/category/{Slug?}",
	defaults : new {controller="Category",action="Index"});

app.MapControllerRoute(
    name: "brand",
    pattern: "/brand/{Slug?}",
    defaults: new { controller = "Brand", action = "Index" });





app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");


//seeding data
var contect = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContect>();
SeedData.SeedingData(contect);

app.Run();
