using VacationManagment.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VacationManagment.Data;
using AspNetCoreHero.ToastNotification;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IManageRolesService, ManageRolesService>();
builder.Services.AddScoped<IManageUsersService, ManageUsersService>();

builder.Services.AddDbContext<VacationDbContext>(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("VacationConnection")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<VacationDbContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 5;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("User Policy", policy =>
     {
         policy.RequireClaim("Request Vacation");
         policy.RequireClaim("View Request Vacation");
     
     });
    options.AddPolicy("Admin Policy", policy =>
    {
        policy.RequireClaim("All Things");
        policy.RequireClaim("Request Vacation");
        policy.RequireClaim("View Request Vacation");
    });
});


builder.Services.ConfigureApplicationCookie(Configure =>
{
    Configure.LoginPath = "/User/Login";
    Configure.AccessDeniedPath = "/User/AccessDenied";
});

builder.Services.AddNotyf(x =>
{
    x.DurationInSeconds =10;
    x.IsDismissable = true;
    x.Position = NotyfPosition.TopRight;
});

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

app.UseAuthentication();
app.UseAuthorization(); 

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.Run();
