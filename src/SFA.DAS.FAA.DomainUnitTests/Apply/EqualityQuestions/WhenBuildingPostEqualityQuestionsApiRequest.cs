using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.EqualityQuestions;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.EqualityQuestions
{
    public class WhenBuildingPostEqualityQuestionsApiRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(Guid applicationId, UpdateEqualityQuestionsModel data)
        {
            var actual = new PostEqualityQuestionsApiRequest(applicationId, data);

            actual.PostUrl.Should().Be($"applications/{applicationId}/equalityQuestions");
            ((UpdateEqualityQuestionsModel)actual.Data).Should().BeEquivalentTo(data);
        }
    }
}
