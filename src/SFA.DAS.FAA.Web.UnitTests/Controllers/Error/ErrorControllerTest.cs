using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Error
{
    [TestFixture]
    public class ErrorControllerTest
    {
        [Test, MoqAutoData]
        public void Then_View_404_Is_Returned([Greedy] ErrorController controller)
        {
            var result = controller.PageNotFound() as ViewResult;

            result.Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public void Then_View_500_Is_Returned([Greedy] ErrorController controller)
        {
            var result = controller.ApplicationError() as ViewResult;

            result.Should().NotBeNull();
        }
    }
}
