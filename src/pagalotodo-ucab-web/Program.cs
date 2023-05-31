using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;
using UCABPagaloTodoWeb.Models.CurrentUser;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("PagaloTodoApi", c =>
{
    c.BaseAddress = new Uri("https://localhost:5001");
    // c.DefaultReque?stHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentUser.GetUser()?.Token);
// });
}).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
