using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users
{
    [TestFixture]
    public class WhenGettingAccountDeletion
    {
        [Test]
        [MoqInlineAutoData(AccountDeletionViewModel.RouthPath.WithdrawApplications, RouteNames.AccountDeleteWithDrawApplication, true)]
        [MoqInlineAutoData(AccountDeletionViewModel.RouthPath.ConfirmAccountDeletion, RouteNames.ConfirmAccountDelete, false)]
        public void Then_View_Is_Returned(
            AccountDeletionViewModel.RouthPath journeyPath,
            string pageBackLink,
            bool hasAnyOutstandingApplications,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserController controller)
        {
            var result = controller.AccountDeletion(journeyPath) as ViewResult;
            result.Should().NotBeNull();

            var actualModel = result!.Model as AccountDeletionViewModel;

            actualModel.Should().NotBeNull();
            actualModel!.JourneyPath.Should().Be(journeyPath);
            actualModel.PageBackLink.Should().Be(pageBackLink);
            actualModel.HasAnyOutstandingApplications.Should().Be(hasAnyOutstandingApplications);
        }
    }
}
