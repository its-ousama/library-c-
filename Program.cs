using Microsoft.EntityFrameworkCore;
using BOOK_API.Data;
using BOOK_API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configure AppDbContext to use PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); // Switched to UseNpgsql

// Register Service classes for Dependency Injection (Required for project structure)
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<BookService>();

var app = builder.Build();

// run migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Note: The Migrate call must be executed *after* you have run `dotnet ef migrations add [MigrationName]`
    // context.Database.Migrate(); 
    Console.WriteLine("Database ready");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("API is running");

app.Run();