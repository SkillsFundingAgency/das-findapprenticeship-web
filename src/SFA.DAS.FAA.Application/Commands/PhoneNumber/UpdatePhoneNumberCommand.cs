﻿using MediatR;

namespace SFA.DAS.FAA.Application.Commands.PhoneNumber;
public class UpdatePhoneNumberCommand : IRequest<Unit>
{
    public string GovUkIdentifier { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}