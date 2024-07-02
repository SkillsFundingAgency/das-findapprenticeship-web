using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users
{
    [TestFixture]
    public class WhenGettingSignIntoYourOldAccount
    {
        [Test, MoqAutoData]
        public void Then_View_Is_Returned(
            [Greedy] UserController controller)
        {
            var result = controller.SignInToYourOldAccount();

            using var scope = new AssertionScope();
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
        }
    }
}
