using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WebSocketsChatStudy.Context;

public class ChatContextFactory : IDesignTimeDbContextFactory<ChatContext>
    {
        public ChatContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChatContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ChatDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

            return new ChatContext(optionsBuilder.Options);
        }
    }
