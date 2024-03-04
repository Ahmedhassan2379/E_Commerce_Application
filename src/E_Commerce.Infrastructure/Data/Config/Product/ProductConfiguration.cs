using E_Commerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(80);
            builder.Property(c => c.Price).HasColumnType("decimal(18,2)");
            //builder.HasOne(x => x.category).WithMany().HasForeignKey(x=>x.CategoryId);
            builder.HasData(
                new Product { Id = 1, Name = "One", Description = "One Item", Price = 2000, CategoryId = 1 ,ProductPicture="https://"},
                new Product { Id = 2, Name = "Two", Description = "Two Item", Price = 2000, CategoryId = 2, ProductPicture = "https://" },
                new Product { Id = 3, Name = "Three", Description = "Three Item", Price = 2000, CategoryId = 3, ProductPicture = "https://" }
                );
        }
    }
}
