using System.ComponentModel.DataAnnotations;

namespace RoboticVacuumCleaner.Web.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Введите имя")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно содержать от 2 до 50 символов")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите фамилию")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Фамилия должна содержать от 2 до 50 символов")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите email")]
        [EmailAddress(ErrorMessage = "Введите корректный email")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Введите корректный номер телефона")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
        public string Password { get; set; } = string.Empty;
    }

    

    

    
}