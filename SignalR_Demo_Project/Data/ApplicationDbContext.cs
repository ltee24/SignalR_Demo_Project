using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SignalR_Demo_Project.Models;

namespace SignalR_Demo_Project.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<ChatMessages> ChatMessages { get; set; }
}
