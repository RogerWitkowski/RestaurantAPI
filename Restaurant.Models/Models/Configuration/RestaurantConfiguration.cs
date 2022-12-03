using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurant.Models.Models.Configuration
{
    internal class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> entityTypeBuilder)
        {
            entityTypeBuilder.Property(n => n.Name).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(d => d.Description).IsRequired().HasMaxLength(100);
        }
    }
}