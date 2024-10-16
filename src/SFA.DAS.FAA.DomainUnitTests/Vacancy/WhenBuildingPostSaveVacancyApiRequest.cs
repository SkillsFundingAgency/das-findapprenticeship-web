using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.SavedVacancies;

namespace SFA.DAS.FAA.Domain.UnitTests.Vacancy
{
    [TestFixture]
    public class WhenBuildingPostSaveVacancyApiRequest
    {
        [Test, AutoData]
        public void Then_Then_Request_Is_Built(
            Guid candidateId,
            string vacancyReference)
        {
            var data = new PostSaveVacancyApiRequestData
            {
                VacancyReference = vacancyReference
            };
            var actual = new PostSaveVacancyApiRequest(candidateId, data);

            actual.PostUrl.Should().Be($"saved-vacancies/{candidateId}/add");
        }
    }
}