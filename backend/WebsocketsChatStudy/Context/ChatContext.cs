using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebSocketsChatStudy.Models;

namespace WebSocketsChatStudy.Context;

public class ChatContext : IdentityDbContext<User, IdentityRole, string>
{
    public ChatContext(DbContextOptions<ChatContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatContext).Assembly);
    }

    public DbSet<User> Users { get; set; }
}
