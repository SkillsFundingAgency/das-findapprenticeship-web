using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.FAA.Web.Models.User
{
    public class ChangeNameViewModel : ViewModelBase
    {
        [Required(ErrorMessage = "Enter your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter your last name")]
        public string LastName { get; set; }
    }
}
