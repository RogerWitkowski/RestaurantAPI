using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurant.Models.Models.Configuration
{
    internal class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> entityTypeBuilder)
        {
            entityTypeBuilder.Property(n => n.City).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(s => s.Street).IsRequired().HasMaxLength(50);
        }
    }
}