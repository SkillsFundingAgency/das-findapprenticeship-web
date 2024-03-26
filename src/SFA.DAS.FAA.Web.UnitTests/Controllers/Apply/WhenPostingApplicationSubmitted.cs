using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply;
public class WhenPostingApplicationSubmitted
{
    [Test, MoqAutoData]
    public async Task And_User_Selected_Yes_Then_Redirect_To_Equality_Flow(
        ApplicationSubmittedViewModel model,
        [Greedy] ApplyController controller)
    {
        model.AnswerEqualityQuestions = true;

        var result = await controller.ApplicationSubmitted(model) as RedirectToRouteResult;

        result.RouteName.Should().Be(RouteNames.ApplyApprenticeship.EqualityFlow);
    }

    [Test, MoqAutoData]
    public async Task And_User_Selected_No_Then_Redirect_To_Their_Applications(
        ApplicationSubmittedViewModel model,
        [Greedy] ApplyController controller)
    {
        model.AnswerEqualityQuestions = false;

        var result = await controller.ApplicationSubmitted(model) as RedirectToRouteResult;

        result.RouteName.Should().Be(RouteNames.UserProfile.YourApplications);
    }
}
