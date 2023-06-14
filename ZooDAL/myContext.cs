using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooDAL.Entities;

namespace ZooDAL
{
    public class myContext : DbContext
    {
        public myContext(DbContextOptions options) 
            : base(options) { }


        public DbSet<Animal> Animals { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cat1 = new Category { Id=Guid.NewGuid(), Name="Birds" };
            var cat2 = new Category { Id = Guid.NewGuid(), Name = "Mammals" };
            var cat3 = new Category { Id = Guid.NewGuid(), Name = "Reptiles" };

            modelBuilder.Entity<Category>().HasData(cat1, cat2, cat3);
        }
    }
}
