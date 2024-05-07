using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Extensions;

namespace SFA.DAS.FAA.Web.UnitTests.Extensions
{
    [TestFixture]
    public class ApplicationStatusExtensionTest
    {
        [TestCase(ApplicationStatus.Draft, "Application started")]
        [TestCase(ApplicationStatus.Submitted, "Applied")]
        [TestCase(ApplicationStatus.Withdrawn, "Applied")]
        [TestCase(ApplicationStatus.Successful, "Applied")]
        [TestCase(ApplicationStatus.Unsuccessful, "Applied")]
        public void GetLabel_Returns_Expected_Value(ApplicationStatus status, string expected)
        {
            status.GetLabel().Should().Be(expected);
        }

        [TestCase(ApplicationStatus.Draft, "govuk-tag govuk-tag--yellow govuk-!-margin-bottom-2")]
        [TestCase(ApplicationStatus.Submitted, "govuk-tag govuk-tag--green govuk-!-margin-bottom-2")]
        [TestCase(ApplicationStatus.Withdrawn, "govuk-tag govuk-tag--green govuk-!-margin-bottom-2")]
        [TestCase(ApplicationStatus.Successful, "govuk-tag govuk-tag--green govuk-!-margin-bottom-2")]
        [TestCase(ApplicationStatus.Unsuccessful, "govuk-tag govuk-tag--green govuk-!-margin-bottom-2")]
        public void GetCss_Returns_Expected_Value(ApplicationStatus status, string expected)
        {
            status.GetCssClass().Should().Be(expected);
        }
    }
}
