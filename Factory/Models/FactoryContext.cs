
using Microsoft.EntityFrameworkCore;

namespace Factory.Models
{
  public class FactoryContext : DbContext
  {
    //public DbSet<Item> Items { get; set; }

    public FactoryContext(DbContextOptions options) : base(options) { }
  }
}