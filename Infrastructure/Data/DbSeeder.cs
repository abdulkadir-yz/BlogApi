using BlogApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Infrastructure.Data;

public class DbSeeder
{
    // Seed kullanıcılarının ortak geliştirme şifresi (Identity şifre kurallarına uyar)
    private const string DevPassword = "Passw0rd!";

    private readonly ApplicationDbContext _db;
    private readonly UserManager<User> _userManager;

    public DbSeeder(ApplicationDbContext db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task SeedAsync(CancellationToken ct = default)
    {
        // Ensure database exists and is up to date
        await _db.Database.MigrateAsync(ct);

        if (await _db.Users.AnyAsync(ct))
            return;

        // Kullanıcıları UserManager ile oluşturuyoruz ki şifre hash'lensin
        // ve NormalizedEmail dolsun (yoksa /auth/login ile giriş yapılamaz).
        var alice = new User { Name = "Alice", UserName = "alice@mail.com", Email = "alice@mail.com" };
        var bob   = new User { Name = "Bob",   UserName = "bob@mail.com",   Email = "bob@mail.com" };

        await _userManager.CreateAsync(alice, DevPassword);
        await _userManager.CreateAsync(bob, DevPassword);

        _db.Posts.AddRange(
            new Post { Id = Guid.NewGuid(), UserId = alice.Id, Title = "Hello World", Content = "First post",  PublishedAt = DateTimeOffset.UtcNow },
            new Post { Id = Guid.NewGuid(), UserId = bob.Id,   Title = "Notes",       Content = "Second post", PublishedAt = null }
        );
        await _db.SaveChangesAsync(ct);
    }
}
