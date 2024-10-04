using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Equivalency;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetSavedVacancies;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models.SavedVacancies;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SavedVacancies
{
    [TestFixture]
    public class WhenGettingIndex
    {
        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Query_Is_Called_And_Index_Returned(
            Guid candidateId,
            GetSavedVacanciesQueryResult result,
            [Frozen] Mock<IDateTimeService> dateTimeService,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SavedVacanciesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetSavedVacanciesQuery>(c =>
                    c.CandidateId.Equals(candidateId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
            
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new(CustomClaims.CandidateId, candidateId.ToString())
            }));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var actual = await controller.Index() as ViewResult;

            actual.Should().NotBeNull();
            var expected = IndexViewModel.Map(result, dateTimeService.Object, SortOrder.ClosingSoonest);
            var actualModel = actual.Model as IndexViewModel;
            actualModel.Should()
                .BeEquivalentTo(expected,
                    o => o.Excluding(x => x.SortOrder)
                        .Excluding(x => x.SortOrderOptions));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Query_Is_Called_With_DeletedVacancy_Reference_And_Index_Returned(
            string vacancyReference,
            Guid candidateId,
            GetSavedVacanciesQueryResult result,
            [Frozen] Mock<IDateTimeService> dateTimeService,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SavedVacanciesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetSavedVacanciesQuery>(c =>
                    c.CandidateId.Equals(candidateId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new(CustomClaims.CandidateId, candidateId.ToString())
            }));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var actual = await controller.Index(vacancyReference) as ViewResult;

            actual.Should().NotBeNull();
            var expected = IndexViewModel.Map(result, dateTimeService.Object, SortOrder.ClosingSoonest);
            var actualModel = actual.Model as IndexViewModel;
            actualModel.Should()
                .BeEquivalentTo(expected,
                    o => o.Excluding(x => x.SortOrder)
                        .Excluding(x => x.SortOrderOptions));

            actualModel!.DeletedVacancy.Should().NotBeNull();
            actualModel!.DeletedVacancy.Should().BeEquivalentTo(result.DeletedVacancy);
        }
    }
}
