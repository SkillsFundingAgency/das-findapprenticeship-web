using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.GetNhsApprenticeshipVacancy;

namespace SFA.DAS.FAA.Domain.UnitTests.GetNhsApprenticeshipVacancy
{
    [TestFixture]
    public class WhenBuildingGetNhsApprenticeshipVacancyApiRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(string vacancyReference)
        {
            var actual = new GetNhsApprenticeshipVacancyApiRequest(vacancyReference);

            actual.GetUrl.Should().Be($"vacancies/nhs/{vacancyReference}");
        }
    }
}