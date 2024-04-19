namespace SFA.DAS.FAA.Domain.User;

public class UpdateDateOfBirthRequestData
{
    public required DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
}