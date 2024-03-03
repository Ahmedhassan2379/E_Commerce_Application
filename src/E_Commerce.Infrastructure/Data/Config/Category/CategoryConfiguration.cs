using E_Commerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Config
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(80);
            builder.HasData(
                new Category { Id = 1, Name = "Laptop", Description = "this is laptops part" },
                new Category { Id = 2, Name = "Mobile", Description = "this is mobiles part" },
                new Category { Id = 3, Name = "Watch", Description = "this is watchs part" }
                );
        }
    }
}
