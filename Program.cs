using BanSach.Components;
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
using BanSach.Components.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMudServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IItemManagement, ItemManagement>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IQNAService, QNAService>();
builder.Services.AddScoped<IBillService, BillService>();
builder.Services.AddScoped<IUserService, UserService>();

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

var app = builder.Build();
app.UseSwaggerUI();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages(); // Ánh xạ Razor Pages

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();