using FitnessHere.DAL.DTOs;
using FitnessHere.DAL.EF;
using FitnessHere.DAL.Entities;


//using FitnessHere.DAL.EF;
//using FitnessHere.DAL.Entities;
using FitnessHere.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

IConfiguration conf = builder.Configuration;

string connStr = conf.GetConnectionString("FitnessDatabase");
connStr = connStr.Replace("|DbDir|", builder.Environment.ContentRootPath);


builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSingleton<IRepository<MemberDTO>>(
    s => new AdoNetMemberRepository(connStr));


builder.Services.AddSingleton(new AdoNetFiltering(connStr));


builder.Services.AddSingleton(new AdoNetImport(connStr));

builder.Services.AddDbContext<FitnessDatabaseContext>(options =>
    options.UseSqlServer(connStr)
    .UseLoggerFactory(
        LoggerFactory.Create(builder =>
            builder.AddConsole().AddDebug()
        )
     )
);

builder.Services.AddScoped<IRepository<Trainer>, EfTrainerRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Member}/{action=Index}/{id?}");

app.Run();
