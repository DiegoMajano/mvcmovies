using Microsoft.EntityFrameworkCore;
using MVCPeliculas.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var rawConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = NormalizePostgresConnectionString(rawConnectionString);

builder.Services.AddDbContext<PeliculasDbContext>(item =>
    item.UseNpgsql(connectionString)
);
static string NormalizePostgresConnectionString(string? raw)
{
    if (string.IsNullOrWhiteSpace(raw)) return raw ?? string.Empty;

    if (raw.StartsWith("postgres://") || raw.StartsWith("postgresql://"))
    {
        var uri = new Uri(raw);
        var userInfo = uri.UserInfo.Split(':', 2);
        var builder = new Npgsql.NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.Port > 0 ? uri.Port : 5432,
            Username = Uri.UnescapeDataString(userInfo[0]),
            Password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "",
            Database = uri.AbsolutePath.TrimStart('/'),
            SslMode = Npgsql.SslMode.Require,
            TrustServerCertificate = true
        };
        return builder.ConnectionString;
    }

    return raw;
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PeliculasDbContext>();
    var maxRetries = 10;
    for (int i = 1; i <= maxRetries; i++)
    {
        try
        {
            db.Database.Migrate();
            break;
        }
        catch (Exception ex) when (i < maxRetries)
        {
            Console.WriteLine($"Esperando a que la base de datos este lista (intento {i}/{maxRetries})... {ex.Message}");
            Thread.Sleep(3000);
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
