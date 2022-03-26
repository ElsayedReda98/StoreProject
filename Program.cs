using Microsoft.EntityFrameworkCore;
using StoreProject.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StoreProjectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreProjectContext")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// added
builder.Services.AddMvc();
//added
//builder.Services.AddPaging();

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
