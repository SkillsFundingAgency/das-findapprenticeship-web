﻿using MediatR;

namespace SFA.DAS.FAA.Application.Commands.UserAddress;
public class UpdateAddressCommand : IRequest<Unit>
{
    public Guid CandidateId { get; set; }
    public string Email { get; set; }
    public string Thoroughfare { get; set; }
    public string Organisation { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string Postcode { get; set; }
}