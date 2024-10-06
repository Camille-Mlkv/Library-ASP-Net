using WEB_253502_Melikava.UI;
using WEB_253502_Melikava.UI.Extensions;
using WEB_253502_Melikava.UI.Services.API;
using WEB_253502_Melikava.UI.Services.BookService;
using WEB_253502_Melikava.UI.Services.FileService;
using WEB_253502_Melikava.UI.Services.GenreService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.RegisterCustomServices();
UriData.ApiUri = builder.Configuration["UriData:ApiUri"];
builder.Services.AddHttpClient<IBookService, ApiBookService>(opt => opt.BaseAddress = new Uri(UriData.ApiUri));
builder.Services.AddHttpClient<IGenreService,ApiGenreService>(opt=>opt.BaseAddress=new Uri(UriData.ApiUri));
builder.Services.AddHttpClient<IFileService, ApiFileService>(opt =>opt.BaseAddress = new Uri($"{UriData.ApiUri}Files"));

builder.Services.AddRazorPages();
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

app.MapRazorPages();

app.Run();
