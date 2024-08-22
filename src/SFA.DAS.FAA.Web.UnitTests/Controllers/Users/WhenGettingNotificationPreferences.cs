using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;
using SFA.DAS.FAA.Application.Queries.User.GetCandidatePreferences;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenGettingNotificationPreferences
{
    [Test]
    [MoqInlineAutoData(null, RouteNames.PhoneNumber, "Get reminders about your unfinished applications – Find an apprenticeship – GOV.UK", "Create an account", "Get reminders about your unfinished applications", "Continue")]
    [MoqInlineAutoData(UserJourneyPath.CreateAccount, RouteNames.PhoneNumber, "Get reminders about your unfinished applications – Find an apprenticeship – GOV.UK", "Create an account", "Get reminders about your unfinished applications", "Continue")]
    [MoqInlineAutoData(UserJourneyPath.ConfirmAccountDetails, RouteNames.ConfirmAccountDetails, "Get reminders about your unfinished applications – Find an apprenticeship – GOV.UK", "Create an account", "Get reminders about your unfinished applications", "Continue")]
    [MoqInlineAutoData(UserJourneyPath.Settings, RouteNames.Settings, "Change if you get reminders about your unfinished applications – Find an apprenticeship – GOV.UK", "", "Change if you get reminders about your unfinished applications", "Save")]
    [MoqInlineAutoData(UserJourneyPath.AccountFound, RouteNames.AccountFoundTermsAndConditions, "Get reminders about your unfinished applications – Find an apprenticeship – GOV.UK", "", "Get reminders about your unfinished applications", "Continue")]
    public async Task Then_View_Is_Returned(
        UserJourneyPath journeyPath,
        string pageBackLink,
        string pageTitle,
        string pageCaption,
        string pageHeading,
        string pageCtaButtonLabel,
        Guid candidateId,
        GetCandidatePreferencesQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString())
                    }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<GetCandidatePreferencesQuery>(x => x.CandidateId == candidateId)
            , It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.NotificationPreferences(journeyPath) as ViewResult;

        result.Should().NotBeNull();
        var actualModel = result!.Model as NotificationPreferencesViewModel;

        actualModel.Should().NotBeNull();
        actualModel!.PageTitle.Should().Be(pageTitle);
        actualModel.PageCaption.Should().Be(pageCaption);
        actualModel.PageHeading.Should().Be(pageHeading);
        actualModel.PageCtaButtonLabel.Should().Be(pageCtaButtonLabel);
        actualModel.JourneyPath.Should().Be(journeyPath);
        actualModel.BackLink.Should().Be(pageBackLink);
    }
}
