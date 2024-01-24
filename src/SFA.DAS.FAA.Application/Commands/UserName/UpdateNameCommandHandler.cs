using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Application.Commands.UserName
{
    public class UpdateNameCommandHandler(IApiClient apiClient) 
        : IRequestHandler<UpdateNameCommand, UpdateNameCommandResult>
    {
        public async Task <UpdateNameCommandHandler> Handle(UpdateNameCommand request, CancellationToken cancellationToken = default)
        {
            var patchRequest = new UpdateNameApiRequest(request.FirstName, request.LastName);

            var response = await apiClient.PatchWithResponseCode; //Not sure what the response should be 

            return new UpdateNameCommandResult
            {
                //not sure what the result should be either
            };
        }
    }

}
