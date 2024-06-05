namespace SFA.DAS.FAA.Application.Queries.Applications.GetTransferUserData
{
    public record GetTransferUserDataQueryResult
    {
        public string? CandidateFirstName { get; set; }
        public string? CandidateLastName { get; set; }
        public long StartedApplications { get; set; }
        public long SubmittedApplications { get; set; }
        public long SavedApplications { get; set; }
    }
}
