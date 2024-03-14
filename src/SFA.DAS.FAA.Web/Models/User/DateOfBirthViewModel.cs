namespace SFA.DAS.FAA.Web.Models.User;

public class DateOfBirthViewModel : ViewModelBase
{
    public int? Day { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }

    //public DateTime DateOfBirth { get { return new DateTime((int)Year, (int)Month, (int)Day); } }
}
