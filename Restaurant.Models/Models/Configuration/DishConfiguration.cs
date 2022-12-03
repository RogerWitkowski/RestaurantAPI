using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Models.Models;

namespace Restaurant.Models.Models.Configuration
{
    internal class DishConfiguration : IEntityTypeConfiguration<Dish>
    {
        public void Configure(EntityTypeBuilder<Dish> enityTypeBuilder)
        {
            enityTypeBuilder.Property(n => n.Name).IsRequired();
            enityTypeBuilder.Property(p => p.Price).HasColumnType("decimal(18,2)");
        }
    }
}