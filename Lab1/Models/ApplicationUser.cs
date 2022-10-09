using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace Lab1.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Birth Date"), AllowNull]
        public string BirthDate { get; set; }
    }
}
