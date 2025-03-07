using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CatForum.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CatForumContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CatForumContext") ?? throw new InvalidOperationException("Connection string 'CatForumContext' not found.")));
                         // Change from IdentityUser to ApplicationUser           // Change this from true to false
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<CatForumContext>();

// Assignment 1 final commit

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Add this below line
app.MapRazorPages().WithStaticAssets();
app.Run();