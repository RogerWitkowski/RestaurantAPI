using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models.Dto
{
    public class RegisterUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirsttName { get; set; }
        public string LasttName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public int RoleId { get; set; } = 1;
    }
}