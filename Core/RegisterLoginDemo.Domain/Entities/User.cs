using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RegisterLoginDemo.Domain.Entities
{
    public class User : IdentityUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        // Identity creates the rest of the fields.
    }
}