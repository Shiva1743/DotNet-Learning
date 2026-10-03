// logs storig => F:\1ACSharpProjects\CoreEmptyProject1\bin\Debug\net8.0\dotnetlogs

using CoreEmptyProject1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role"));
});

builder.Host.UseNLog();

// ASP.NET Core in DI container , AppDbContext register and connect with SQL Server.
builder.Services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDBConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 10;
    options.Password.RequiredUniqueChars = 3;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>(); // count will be same withing scoped means incresed till 5

var app = builder.Build();



//app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    Console.WriteLine("hii ngjg");
    app.UseDeveloperExceptionPage();
    //app.UseExceptionHandler("/Error");
}
else{
    app.UseExceptionHandler("/Error");
}
app.UseStatusCodePagesWithReExecute("/Error/{0}"); // 404 actual error ave chhe.

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
