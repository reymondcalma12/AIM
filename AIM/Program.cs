using AIM.Data;
using AIM.Services;
using ExternalLogin;
using ExternalLogin.Interfaces;
using ExternalLogin.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(
    o => o.Filters.Add(typeof(ProfileAccessAuth)));

var CentralLoginConnectionString = builder.Configuration.GetConnectionString("CentralLoginConnection")
?? throw new InvalidOperationException("Connection string 'ExternalDbContext' not found.");

builder.Services.AddDbContext<ExternalDbContext>(options =>
    options.UseSqlServer(CentralLoginConnectionString));

// Register the ProfileAccessService and ProfileAccessBackgroundService
builder.Services.AddScoped<ProfileAccessService>();
builder.Services.AddHostedService<ProfileAccessBackgroundService>();

builder.Services.AddDbContext<AimDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AIM_Context"))
);




// SESSION
builder.Services.AddSession(options =>
{
	options.Cookie.Name = "MySessionCookie";
	options.IdleTimeout = TimeSpan.FromDays(7);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IExternalLoginService, ExternalLoginService>();

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


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
