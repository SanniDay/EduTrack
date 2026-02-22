using EduTrack.Helpers;
using EduTrack.Interfaces;
using EduTrack.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// =============================
// Add Services
// =============================

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<DbHelper>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();

// =============================
// Authentication (Cookie)
// =============================

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

// =============================
// Build App
// =============================

var app = builder.Build();

// =============================
// Global Error Handling
// =============================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");  // Production error page
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage(); // Detailed error in development
}

// Handle status codes (404, 403, etc.)
app.UseStatusCodePagesWithReExecute("/Home/StatusCode", "?code={0}");

// =============================
// Middleware Pipeline
// =============================

app.UseHttpsRedirection();
app.UseStaticFiles();     // Required for CSS/JS
app.UseRouting();

app.UseAuthentication();  // Must come before Authorization
app.UseAuthorization();

// =============================
// Routing
// =============================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();