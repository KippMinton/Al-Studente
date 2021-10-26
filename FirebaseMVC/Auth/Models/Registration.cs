using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AlStudente.Auth.Models
{
    public class Registration
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
 
        [Required]
        [Compare(nameof(Password))]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
