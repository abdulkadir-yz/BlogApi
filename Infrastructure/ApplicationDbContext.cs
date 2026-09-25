// Infrastructure/ApplicationDbContext.cs
using BlogApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Users DbSet'i IdentityDbContext'ten geliyor, burada sadece kendi entity'lerimiz var.
    public DbSet<Post> Posts => Set<Post>();
}
