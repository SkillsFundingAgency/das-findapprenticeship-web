using MediatR;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Commands.EmploymentLocations.Update
{
    public record UpdateEmploymentLocationsCommand : IRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public List<Guid> SelectedAddressIds { get; set; } = new();
        public SectionStatus EmploymentLocationSectionStatus { get; init; }
    }
}