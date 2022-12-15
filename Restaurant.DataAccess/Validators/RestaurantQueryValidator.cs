using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Restaurant.Models.Models;

namespace Restaurant.DataAccess.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        private readonly int[] _allowedPageSizes = new[] { 5, 10, 15 };

        public RestaurantQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, content) =>
            {
                if (!_allowedPageSizes.Contains(value))
                {
                    content.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", _allowedPageSizes)}]");
                }
            });
        }
    }
}