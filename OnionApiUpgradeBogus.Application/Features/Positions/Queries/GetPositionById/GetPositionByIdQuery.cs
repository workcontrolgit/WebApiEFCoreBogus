using MediatR;
using OnionApiUpgradeBogus.Application.Exceptions;
using OnionApiUpgradeBogus.Application.Interfaces.Repositories;
using OnionApiUpgradeBogus.Application.Wrappers;
using OnionApiUpgradeBogus.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnionApiUpgradeBogus.Application.Features.Positions.Queries.GetPositionById
{
    public class GetPositionByIdQuery : IRequest<Response<Position>>
    {
        public Guid Id { get; set; }

        public class GetPositionByIdQueryHandler : IRequestHandler<GetPositionByIdQuery, Response<Position>>
        {
            private readonly IPositionRepositoryAsync _positionRepository;

            public GetPositionByIdQueryHandler(IPositionRepositoryAsync positionRepository)
            {
                _positionRepository = positionRepository;
            }

            public async Task<Response<Position>> Handle(GetPositionByIdQuery query, CancellationToken cancellationToken)
            {
                var position = await _positionRepository.GetByIdAsync(query.Id);
                if (position == null) throw new ApiException($"Position Not Found.");
                return new Response<Position>(position);
            }
        }
    }
}