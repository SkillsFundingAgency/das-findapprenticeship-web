﻿using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.FAA.Infrastructure.Api;

namespace SFA.DAS.FAA.Application.Commands.CreateAccount.UserName
{
    public class UpdateNameCommandHandler(IApiClient apiClient)
        : IRequestHandler<UpdateNameCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateNameCommand request, CancellationToken cancellationToken)
        {
            var putRequest = new UpdateNameApiRequest(request.CandidateId, new UpdateNameApiRequestData
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            });

            var response = await apiClient.PutWithResponseCode<NullResponse>(putRequest);

            if ((int)response.StatusCode > 300)
            {
                throw new InvalidOperationException();
            }

            return Unit.Value;
        }

    }

}
