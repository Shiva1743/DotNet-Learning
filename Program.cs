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
builder.Host.UseNLog();

//builder.Services.AddMvcCore();

//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//builder.Logging.AddDebug();


// ASP.NET Core in DI container , AppDbContext register and connect with SQL Server.
builder.Services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDBConnection")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 10;
        options.Password.RequiredUniqueChars = 3;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<AppDbContext>();

//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.Password.RequiredLength = 10;
//    options.Password.RequiredUniqueChars = 3;
//    options.Password.RequireNonAlphanumeric = false;
//});


//builder.Services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>(); // same count value through application
//builder.Services.AddScoped<IEmployeeRepository, MockEmployeeRepository>(); // count will be same withing scoped means incresed till 5
//builder.Services.AddTransient<IEmployeeRepository, MockEmployeeRepository>(); // count will be same as default data bcz every time new intance is created

builder.Services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>(); // count will be same withing scoped means incresed till 5

//builder.Services.AddControllers();
//builder.Services.AddControllersWithViews();

var app = builder.Build();


//app.MapGet("/", () => "Hello World123!");
//app.UseDefaultFiles();


//DefaultFilesOptions defaultfilesOptions = new DefaultFilesOptions();
//defaultfilesOptions.DefaultFileNames.Clear();
//defaultfilesOptions.DefaultFileNames.Add("home.html");
//app.UseDefaultFiles(defaultfilesOptions);


//FileServerOptions fileServerOptions = new FileServerOptions();
//fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
//fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("home.html");
//app.UseFileServer(fileServerOptions); // represents file as defined


//app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    Console.WriteLine("hii ngjg");
    //app.UseDeveloperExceptionPage(); // old code

    app.UseExceptionHandler("/Error");
}
else{

    //app.UseStatusCodePages(); // 404 page not found ape
    //app.UseStatusCodePagesWithRedirects("/Error/{0}"); // 404 page open kare atle statuscode 200 ave but 404 actual error avvi joie

    app.UseExceptionHandler("/Error");
}
app.UseStatusCodePagesWithReExecute("/Error/{0}"); // 404 actual error ave chhe.

//else if (app.Environment.IsStaging() || app.Environment.IsProduction() || app.Environment.Equals("UAT")) {
//    app.UseExceptionHandler("/Error");
//}

// it represents used in replace of UseDefaultFiles and UseStaticFiles and html files like index,default.
//app.UseFileServer();
//app.UseDefaultFiles();


app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.UseMvcWithDefaultRoute(); // old dotnet for homecontoller
//Conventional Routing
//app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();
//app.Run(async (contex) =>
//{
//    //throw new Exception("error message display!!");
//    //await contex.Response.WriteAsync("hi from run cmd");

//    // comes from launchsetting or window's enviroment setting in contol panel if set,
//    // if not set in both then by default its value is production
//    await contex.Response.WriteAsync("hosting environment : " + app.Environment.EnvironmentName);
//});
//app.Use(async(contex,next) => { Console.WriteLine("hii"); await next();  });

app.Run();
