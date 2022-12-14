using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Restaurant.DataAccess.DataAccess;
using Restaurant.Models.Dto;

namespace Restaurant.DataAccess.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator(RestaurantDbContext dbContext)
        {
            RuleFor(e => e.Email).NotEmpty().EmailAddress();
            RuleFor(p => p.Password).MinimumLength(10).NotEmpty();
        }
    }
}