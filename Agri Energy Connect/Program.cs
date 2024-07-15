var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<Agri_Energy_Connect.Models.Poe2Context>();
builder.Services.AddControllersWithViews();
//Add sessions and set the properties of the session.
//THe time out is 20 MIN, so that it doesnt time out too early.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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
//Add use sessions to get the user id
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
