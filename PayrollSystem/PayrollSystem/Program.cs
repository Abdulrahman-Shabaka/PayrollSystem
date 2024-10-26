using Microsoft.EntityFrameworkCore;
using PayrollSystem.Data;
using PayrollSystem.Interfaces;
using PayrollSystem.Models.Mapping;
using PayrollSystem.Repositories;
using PayrollSystem.Services;
using PayrollSystem.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Allow memory cache
builder.Services.AddMemoryCache();

// Register your services with DI
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<SalaryService>();
builder.Services.AddScoped<IncentiveAndDiscountService>();
builder.Services.AddScoped<AttendanceService>();

// Add the Unit of Work and repository as well
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Configure the DbContext with SQL Server
builder.Services.AddDbContext<PayrollContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    //Apply migrations
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<PayrollContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employee}/{action=Index}/{id?}");

app.Run();
