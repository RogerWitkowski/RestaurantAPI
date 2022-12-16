using Restaurant.Utility.StaticDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.Models
{
    public class RestaurantQuery
    {
        public string? SearchPhrase { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SortBy { get; set; }
        public SortDirectionStaticDetails SortDirection { get; set; }
    }
}