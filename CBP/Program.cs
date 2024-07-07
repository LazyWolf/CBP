using CBP.Data;
using CBP.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<IDataContext, DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CBP")));
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services
    .AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<IDataContext>();
    if (!dbContext.Database.CanConnect())
    {
        throw new NotImplementedException("Cannot connect to DataContext");
    }
}

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
