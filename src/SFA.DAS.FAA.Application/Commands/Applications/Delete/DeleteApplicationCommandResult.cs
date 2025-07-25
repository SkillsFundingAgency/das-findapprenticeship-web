namespace SFA.DAS.FAA.Application.Commands.Applications.Delete;

public class DeleteApplicationCommandResult
{
    public bool Success { get; set; }
    public string? VacancyTitle { get; set; }
    public string? EmployerName { get; set; }
}