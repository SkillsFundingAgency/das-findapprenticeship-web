using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.AdditionalQuestion.GetAdditionalQuestion;
using SFA.DAS.FAA.Domain.Apply.GetAdditionalQuestion;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;

[TestFixture]
public class WhenMappingApiResponseWithGetAdditionalQuestionQueryResult
{
    [Test, MoqAutoData]
    public void Map_Returns_Expected_Result(
        GetAdditionalQuestionApiResponse source)
    {
        var result = (GetAdditionalQuestionQueryResult)source;

        using var scope = new AssertionScope();
        result.Id.Should().Be(source.Id);
        result.Answer.Should().Be(source.Answer);
        result.QuestionText.Should().Be(source.QuestionText);
        result.ApplicationId.Should().Be(source.ApplicationId);
    }
}