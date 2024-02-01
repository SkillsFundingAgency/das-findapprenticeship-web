using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetWorkHistories;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Apply
{
    [TestFixture]
    public class JobViewModelTests
    {
        [Test, MoqAutoData]
        public void Map_Returns_Expected_Result(
            GetJobsQueryResult.Job source)
        {
            var result = (JobsViewModel.Job)source;

            using var scope = new AssertionScope();
            result.Id.Should().Be(source.Id);
            result.JobTitle.Should().Be(source.JobTitle);
            result.Description.Should().Be(source.Description);
            result.Employer.Should().Be(source.Employer);
            result.JobDates.Should().Be($"{source.StartDate:MMMM yyyy} to {source.EndDate:MMMM yyyy}");
        }

        [Test, MoqAutoData]
        public void When_EndDate_Is_Null_Map_Returns_Expected_Result(
            GetJobsQueryResult.Job source)
        {
            source.EndDate = null;
            var result = (WorkHistoryViewModel)source;

            using var scope = new AssertionScope();
            result.Id.Should().Be(source.Id);
            result.JobTitle.Should().Be(source.JobTitle);
            result.Description.Should().Be(source.Description);
            result.Employer.Should().Be(source.Employer);
            result.JobDates.Should().Be($"{source.StartDate:MMMM yyyy} onwards");
        }
    }
}
