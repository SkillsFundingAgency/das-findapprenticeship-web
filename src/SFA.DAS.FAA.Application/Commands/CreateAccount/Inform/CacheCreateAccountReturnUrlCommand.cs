using MediatR;

namespace SFA.DAS.FAA.Application.Commands.CreateAccount.Inform
{
    public class CacheCreateAccountReturnUrlCommand : IRequest
    {
        public string? ReturnUrl { get; set; }
    }

    public class CacheCreateAccountReturnUrlCommandHandler() : IRequestHandler<CacheCreateAccountReturnUrlCommand>
    {
        public Task Handle(CacheCreateAccountReturnUrlCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
