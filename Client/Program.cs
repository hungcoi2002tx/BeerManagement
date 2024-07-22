using Client.Middleware;
using Client.WebRequests;
using Share.Ultils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddAutoMapperConfig();

#region config http client
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ICustomHttpClient, CustomHttpClient>();
builder.Services.AddSingleton<Logger>();
#endregion
#region Add session
builder.Services.AddDistributedMemoryCache(); //using local storage to save session
builder.Services.AddSession(x =>
{
    x.IdleTimeout = TimeSpan.FromSeconds(100000000); //settime out for session
    x.Cookie.HttpOnly = true; //Security sesion cookies
    x.Cookie.IsEssential = true;
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.UseWhen(context =>
        context.Request.Path != "/login" &&
        context.Request.Path != "/page400" &&
        context.Request.Path != "/page403" &&
        context.Request.Path != "/page404" &&
        context.Request.Path != "/page500" &&
        context.Request.Path != "/page503",
        appBuilder =>
        {
            appBuilder.UseMiddleware<CheckTokenMiddleware>();
        });

app.MapGet("/", async context =>
{
	context.Response.Redirect("/Login");
});

app.Run();
