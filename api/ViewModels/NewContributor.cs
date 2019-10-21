using System.ComponentModel.DataAnnotations;

namespace api.ViewModels
{
    public class NewContributor
    {
        [Required]
        public string username { get; set; }

        [Required]
        [MinLength(8)]
        public string password { get; set; }

        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }
  }
}
