using Manago.Services.Identity.DbContexts;
using Manago.Services.Identity.Helper;
using Manago.Services.Identity.Initializer;
using Manago.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add Database Connection
builder.Services.AddDbContext<ApplicationDbContexts>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("ProductDB")));
#region Configure Identity Server
// Configuring Identity Server 
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContexts>().AddDefaultTokenProviders();
var identityBuilder = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.Events.RaiseErrorEvents = true;
}).AddInMemoryIdentityResources(SD.IdentityResources)
            .AddInMemoryApiScopes(SD.ApiScopes)
            .AddInMemoryClients(SD.Clients)
            .AddAspNetIdentity<ApplicationUser>();
// Signin Credentials for Development Purpose
identityBuilder.AddDeveloperSigningCredential();

#endregion
builder.Services.AddScoped<IDbInitializer, DbInitializer>();


// Add services to the container.
builder.Services.AddControllersWithViews();






var app = builder.Build();
IConfiguration configuration = app.Configuration;
IWebHostEnvironment environment = app.Environment;

// To Inject the service in Program.cs need to create scope for that and the we can get required service
using var scope = app.Services.CreateScope();
IDbInitializer dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

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
app.UseIdentityServer();
app.UseAuthorization();
// Call method to initalize User Roles
dbInitializer.Initialize();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
