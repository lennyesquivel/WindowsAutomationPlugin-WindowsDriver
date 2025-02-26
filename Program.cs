using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "clientSessionId";
    options.IdleTimeout = TimeSpan.FromMinutes(5);
});

builder.WebHost.UseKestrel(options => {
    options.Listen(IPAddress.Loopback, 5000);
    //options.Listen(IPAddress.Loopback, 5001, listenOptions =>
    //{
    //    listenOptions.UseHttps("WindowsAutomationPlugin.pfx", "verysecurepassword");
    //});
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSession();

app.Run();
