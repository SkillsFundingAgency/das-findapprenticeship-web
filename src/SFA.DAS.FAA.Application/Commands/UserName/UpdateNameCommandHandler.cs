using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.FAA.Infrastructure.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Application.Commands.UserName
{
    public class UpdateNameCommandHandler(IApiClient apiClient) 
        : IRequestHandler<UpdateNameCommand>
    {
        public async Task<Unit> Handle(UpdateNameCommand request, CancellationToken cancellationToken)
        {
            var putRequest = new UpdateNameApiRequest(request.FirstName, request.LastName);

            var response = await apiClient.PostWithResponseCode<NullResponse>(putRequest);

            return Unit.Value;
        }

    }

}
