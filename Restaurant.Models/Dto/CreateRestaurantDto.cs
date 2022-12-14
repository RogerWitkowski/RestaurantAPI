using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.Dto
{
    public class CreateRestaurantDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }
        public string Category { get; set; }
        public bool HasDelivery { get; set; }

        [EmailAddress]
        public string ContactEmail { get; set; }

        [Phone]
        public string ContactNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string Country { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        public string Street { get; set; }
        public string PostalCode { get; set; }
    }
}