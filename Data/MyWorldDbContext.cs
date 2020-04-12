using Microsoft.EntityFrameworkCore;
using SqlDistributedCache.API.Entities;
namespace SqlDistributedCache.API.Data
{
    public class MyWorldDbContext : DbContext
    {
        public MyWorldDbContext(DbContextOptions<MyWorldDbContext> options) : base(options)
        {

        }

        public DbSet<Gadgets> Gadgets { get; set; }
    }
}