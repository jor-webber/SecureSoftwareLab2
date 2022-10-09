using System.ComponentModel.DataAnnotations;

namespace Lab1.Models
{
    public class Team
    {
        [Required, Display(Name = "Id")]
        public int Id { get; set; }
        [Required, Display(Name = "Teams Name")]
        public string TeamName { get; set; }
        [Required, Display(Name = "Email"), EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Established Date")]
        public DateTime EstablishedDate { get; set; }
    }
}
