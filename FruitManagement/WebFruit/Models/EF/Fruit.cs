using Core.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace WebFruit.Models.EF
{
    public class Fruit : DbContext
    {
        public Fruit(DbContextOptions<Fruit> options) : base(options)
        {
        }
        public DbSet<Article> Articles{ get; set; }
        public DbSet<Autherized> Autherizeds{ get; set; }
        public DbSet<Category> Categories{ get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
