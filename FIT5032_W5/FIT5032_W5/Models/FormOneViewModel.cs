using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace FIT5032_W5.Models
{
    public class FormOneViewModel
    {
        [Required(ErrorMessage ="First Name is required.")]
        [MinLength(2, ErrorMessage = "First Name must have a minimum length of 2")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
