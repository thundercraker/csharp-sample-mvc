using System.ComponentModel.DataAnnotations;

namespace EngineerTest.Models.View.Account
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
