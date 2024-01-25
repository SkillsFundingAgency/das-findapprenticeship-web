using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.FAA.Web.Models.User
{
    public class NameViewModel
    {
        [FromForm]
        [Required(ErrorMessage = "Enter your first name")]
        public string FirstName { get; set; }
        [FromForm]
        [Required(ErrorMessage = "Enter your last name")]
        public string LastName { get; set; }
    }
}
