using GFLtestFolders.Data;
using GFLtestFolders.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddMvc().AddRazorRuntimeCompilation();

var app = builder.Build();

// RUN MIGRATIONS
await using var scope = app.Services.CreateAsyncScope();
await using var context = scope.ServiceProvider.GetService<DataContext>();
await context.Database.MigrateAsync();

await InsertStandardFolderStructureAsync(context);

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


async Task InsertStandardFolderStructureAsync(DataContext context)
{
    if (!context.FolderDirectories.Any())
    {
        List<FolderDirectory> data = new List<FolderDirectory>()
        {
            new() {Title = "Creating Digital Images"},
            new() {Title = "Resources", ParentId = 1}, // 2
            new() {Title = "Evidence", ParentId = 1},
            new() {Title = "Graphic Products", ParentId = 1}, // 4
            new() {Title = "Primary Sources", ParentId = 2},
            new() {Title = "Secondary Sources", ParentId = 2},
            new() {Title = "Process", ParentId = 4},
            new() {Title = "Final Product", ParentId = 4}
        };

        await context.FolderDirectories.AddRangeAsync(data);
        await context.SaveChangesAsync();
    }
}