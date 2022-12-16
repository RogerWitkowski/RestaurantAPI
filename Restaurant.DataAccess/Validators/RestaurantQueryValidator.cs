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
        private readonly string[] _allowedSortByColumnName = { nameof(Models.Models.Restaurant.Name), nameof(Models.Models.Restaurant.Description), nameof(Models.Models.Restaurant.Category), nameof(Models.Models.Restaurant.Address.City), };

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

            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || _allowedSortByColumnName.Contains(value))
                .WithMessage($"Sort By is optional, or must be in [{string.Join(",", _allowedSortByColumnName)}]");
        }
    }
}