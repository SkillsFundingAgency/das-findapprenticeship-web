using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.FAA.Web.Models.User
{
    public class NameViewModel : ViewModelBase
    {
        [Required(ErrorMessage = "Enter your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter your last name")]
        public string LastName { get; set; }
        public bool? ReturnToConfirmationPage { get; set; }
        public string BackLink { get; set; }
    }
}
