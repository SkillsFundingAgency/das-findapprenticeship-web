using AutoFixture.NUnit3;
using CreateAccount.GetCandidateDateOfBirth;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenGettingDateOfBirth
{
    [Test]
    [MoqInlineAutoData(null, RouteNames.UserName, "Date of birth – Find an apprenticeship – GOV.UK", "Create an account", "Date of birth", "Continue")]
    [MoqInlineAutoData(UserJourneyPath.CreateAccount, RouteNames.UserName, "Date of birth – Find an apprenticeship – GOV.UK", "Create an account", "Date of birth", "Continue")]
    [MoqInlineAutoData(UserJourneyPath.ConfirmAccountDetails, RouteNames.ConfirmAccountDetails, "Date of birth – Find an apprenticeship – GOV.UK", "Create an account", "Date of birth", "Continue")]
    [MoqInlineAutoData(UserJourneyPath.AccountFound, RouteNames.ConfirmAccountDetails, "Date of birth – Find an apprenticeship – GOV.UK", "", "Date of birth", "Continue")]
    [MoqInlineAutoData(UserJourneyPath.Settings, RouteNames.Settings, "Change your date of birth – Find an apprenticeship – GOV.UK", "", "Change your date of birth", "Save")]

    public async Task Then_View_Is_Returned(
        UserJourneyPath journeyPath,
        string pageBackLink,
        string pageTitle,
        string pageCaption,
        string pageHeading,
        string pageCtaButtonLabel,
        string govIdentifier,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new(ClaimTypes.NameIdentifier, govIdentifier),
                        new(CustomClaims.CandidateId, candidateId.ToString()),
                    }))

            }
        };

        mediator.Setup(x => x.Send(It.IsAny<GetCandidateDateOfBirthQuery>(), CancellationToken.None))
            .ReturnsAsync(new GetCandidateDateOfBirthQueryResult { DateOfBirth = null });

        var result = await controller.DateOfBirth(journeyPath) as ViewResult;

        result.Should().NotBeNull();

        var actualModel = result!.Model as DateOfBirthViewModel;

        actualModel!.PageTitle.Should().Be(pageTitle);
        actualModel.PageCaption.Should().Be(pageCaption);
        actualModel.PageHeading.Should().Be(pageHeading);
        actualModel.PageCtaButtonLabel.Should().Be(pageCtaButtonLabel);
        actualModel.JourneyPath.Should().Be(journeyPath);
        actualModel.BackLink.Should().Be(pageBackLink);
        mediator.Verify(x => x.Send(It.IsAny<GetCandidateDateOfBirthQuery>(), CancellationToken.None), Times.Once());
    }
}
