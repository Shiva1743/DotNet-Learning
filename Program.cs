using CoreEmptyProject1.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
//builder.Services.AddMvcCore();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
//builder.Logging.AddDebug();


// ASP.NET Core in DI container , AppDbContext register and connect with SQL Server.
builder.Services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDBConnection")));


//builder.Services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>(); // same count value through application
//builder.Services.AddScoped<IEmployeeRepository, MockEmployeeRepository>(); // count will be same withing scoped means incresed till 5
//builder.Services.AddTransient<IEmployeeRepository, MockEmployeeRepository>(); // count will be same as default data bcz every time new intance is created

builder.Services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>(); // count will be same withing scoped means incresed till 5

//builder.Services.AddControllers();
//builder.Services.AddControllersWithViews();

var app = builder.Build();

//Console.WriteLine("EnvironmentName");
//Console.WriteLine(builder.Environment.EnvironmentName);
//Console.WriteLine("ApplicationName");
//Console.WriteLine(builder.Environment.ApplicationName);
//Console.WriteLine("ContentRootPath");
//Console.WriteLine(builder.Environment.ContentRootPath);


//Console.WriteLine(builder.Configuration["MySettings:Company"]);
//Console.WriteLine(builder.Configuration["MySettings:Version"]);
//Console.WriteLine(builder.Configuration["MyKey"]);

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
    app.UseDeveloperExceptionPage();
}else{
    //app.UseStatusCodePages(); // 404 page not found ape
    //app.UseStatusCodePagesWithRedirects("/Error/{0}"); // 404 page open kare atle statuscode 200 ave but 404 actual error avvi joie
    app.UseStatusCodePagesWithReExecute("/Error/{0}"); // 404 actual error ave chhe.
}
//else if (app.Environment.IsStaging() || app.Environment.IsProduction() || app.Environment.Equals("UAT")) {
//    app.UseExceptionHandler("/Error");
//}

// it represents used in replace of UseDefaultFiles and UseStaticFiles and html files like index,default.
//app.UseFileServer();
//app.UseDefaultFiles();


app.UseStaticFiles();
app.UseRouting();

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
