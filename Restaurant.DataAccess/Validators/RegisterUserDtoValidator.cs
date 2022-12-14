using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Restaurant.DataAccess.DataAccess;
using Restaurant.Models.Dto;

namespace Restaurant.DataAccess.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(RestaurantDbContext dbContext)
        {
            RuleFor(e => e.Email)
                .NotEmpty()
                .EmailAddress()
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.AnyAsync(u => u.Email == value);
                    if (emailInUse.Result)
                    {
                        context.AddFailure("Email", "That email is taken!");
                    }
                });

            RuleFor(p => p.Password)
                .MinimumLength(10);

            RuleFor(cp => cp.ConfirmPassword)
                .Equal(p => p.Password);
        }
    }
}