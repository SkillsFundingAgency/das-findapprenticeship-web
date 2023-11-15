namespace SFA.DAS.FAA.Domain.SearchResults;

public class Vacancies
{
    public int vacancyId { get; private set; }
        public string title { get; private set; }

        public string employerName { get; private set; }
        public string vacancyLocation { get; private set; }
        public string vacancyPostCode { get; private set; }
        public string TitleAndLevel { get; private set; }
        public string wage { get; private set; }
        public DateOnly advertClosing { get; private set; }
        public DateOnly postedDate { get; private set; }
}