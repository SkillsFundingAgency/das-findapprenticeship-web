using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.GetApprenticeshipVacancy;

namespace SFA.DAS.FAA.Domain.UnitTests.GetApprenticeshipVacancy
{
    public class WhenBuildingGetApprenticeshipVacancyApiRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(string vacancyReference)
        {
            var actual = new GetApprenticeshipVacancyApiRequest(vacancyReference);

            actual.GetUrl.Should().Be($"vacancies/{vacancyReference}");
        }
    }
}
