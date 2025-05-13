using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.SavedVacancies;

namespace SFA.DAS.FAA.Domain.UnitTests.Vacancy
{
    [TestFixture]
    public class WhenBuildingPostDeleteSavedVacancyApiRequest
    {
        [Test, AutoData]
        public void Then_Then_Request_Is_Built(
            Guid candidateId,
            string vacancyId,
            bool deleteAllByVacancyReference)
        {
            var data = new PostDeleteSavedVacancyApiRequestData
            {
                VacancyId = vacancyId,
                DeleteAllByVacancyReference = deleteAllByVacancyReference
            };
            var actual = new PostDeleteSavedVacancyApiRequest(candidateId, data);

            actual.PostUrl.Should().Be($"saved-vacancies/{candidateId}/delete");
        }
    }
}