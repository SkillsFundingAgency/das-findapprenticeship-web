﻿using MediatR;

namespace SFA.DAS.FAA.Application.Queries.GetGeoPoint;
public class GetGeoPointQuery : IRequest<GetGeoPointQueryResult>
{
    public string PostCode { get; set; } = null!;
}