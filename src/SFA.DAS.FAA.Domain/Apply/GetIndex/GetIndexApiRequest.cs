using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.GetIndex
{
    public class GetIndexApiRequest : IGetApiRequest
    {
        private readonly string _vacancyReference;
        private readonly string _emailAddress;

        public GetIndexApiRequest(string vacancyReference, string emailAddress)
        {
            _vacancyReference = vacancyReference;
            _emailAddress = emailAddress;
        }

        public string GetUrl => $"vacancies/{_vacancyReference}/apply?applicantEmailAddress={_emailAddress}";
    }
}
