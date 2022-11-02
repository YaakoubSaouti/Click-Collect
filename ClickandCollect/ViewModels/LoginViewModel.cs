using System.ComponentModel.DataAnnotations;

namespace ClickandCollect.ViewModels
{
    public class LoginViewModel
    {
        [StringLength(45, MinimumLength = 5, ErrorMessage = "The username has to be between 5 and 45 characters.")]
        [Required(ErrorMessage = "Requiered field")]
        public string Username { get; set; }

        [StringLength(45, ErrorMessage = "The password cannot be more than 45 characters.")]
        [Required(ErrorMessage = "Requiered field")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[.@$!%*#?&=])[A-Za-z\d@$!%*#?&.=]{8,}$",
        ErrorMessage = "The password must contain one letter, one special character and one number and has to be at least 8 characters.")]
        public string Password { get; set; }
    }
}
