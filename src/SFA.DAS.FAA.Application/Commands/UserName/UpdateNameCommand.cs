using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Application.Commands.UserName
{
    public class UpdateNameCommand : IRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
