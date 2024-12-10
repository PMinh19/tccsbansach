﻿using BanSach.Components;
using MudBlazor.Services;
using Microsoft.EntityFrameworkCore;
using BanSach.Components.Data;
using Microsoft.Extensions.DependencyInjection;
using BanSach.Components.Services;
using BanSach.Components.IService;
using Microsoft.Extensions.Options;
using BanSach.Components.Services.AuthService;

using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BanSach.Components.Services.QNAService;


using Microsoft.AspNetCore.Authentication.Cookies;


using Microsoft.AspNetCore.Authentication.Cookies;
using PayPalCheckoutSdk.Orders;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMudServices();
builder.Services.AddScoped<IItemManagement, ItemManagement>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IQNAService, QNAService>();
builder.Services.AddScoped<IBillService, BillService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDiscountServicecs, DiscountService>();
builder.Services.AddScoped<AdminAccountService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddOptions();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddAuthorizationCore();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddControllersWithViews()
               .AddSessionStateTempDataProvider();
builder.Services.AddSession();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Chỉ sử dụng với HTTPS
});


var app = builder.Build();


app.UseSwaggerUI();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseSwagger();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
// Add UseAntiforgery middleware here after Authentication and Authorization
app.UseAntiforgery();

// Add routing and endpoints
app.MapRazorPages(); // Để ánh xạ Razor Pages (Blazor)
app.MapControllers(); // Để ánh xạ các API Endpoint
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(); // Để ánh xạ Razor Components
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var adminAccountService = services.GetRequiredService<AdminAccountService>();
    var adminExists = context.Users.Any(u => u.Email == "adminboss@gmail.com"); if (!adminExists)
    {
        adminAccountService.CreateAdminAccount().Wait();
    }
}
app.Run();
