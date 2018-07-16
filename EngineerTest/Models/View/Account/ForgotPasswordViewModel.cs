using System.ComponentModel.DataAnnotations;

namespace EngineerTest.Models.View.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
