using Microsoft.EntityFrameworkCore;
using WebSocketsChatStudy.Models;

namespace WebSocketsChatStudy.Context;

public class Context : DbContext
{
    public DbSet<User> Users { get; set; }
}
