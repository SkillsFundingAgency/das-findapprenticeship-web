using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.FAA.Infrastructure.Api;

namespace SFA.DAS.FAA.Application.Commands.UserName
{
    public class UpdateNameCommandHandler(IApiClient apiClient) 
        : IRequestHandler<UpdateNameCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateNameCommand request, CancellationToken cancellationToken)
        {
            Guid tempCandidateId = Guid.NewGuid();

            var putRequest = new UpdateNameApiRequest(request.FirstName, request.LastName,  tempCandidateId);

            var response = await apiClient.PutWithResponseCode<NullResponse>(putRequest);

            if ((int)response.StatusCode > 300)
            {
                throw new InvalidOperationException();
            }

            return Unit.Value;
        }

    }

}
