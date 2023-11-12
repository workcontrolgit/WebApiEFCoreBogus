﻿using AutoMapper;
using MediatR;
using OnionApiUpgradeBogus.Application.Interfaces;
using OnionApiUpgradeBogus.Application.Interfaces.Repositories;
using OnionApiUpgradeBogus.Application.Parameters;
using OnionApiUpgradeBogus.Application.Wrappers;
using OnionApiUpgradeBogus.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnionApiUpgradeBogus.Application.Features.Customers.Queries.GetCustomers
{
    public class GetCustomersQuery : QueryParameter, IRequest<PagedResponse<IEnumerable<Entity>>>
    {
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
    }

    public class GetAllCustomersQueryHandler : IRequestHandler<GetCustomersQuery, PagedResponse<IEnumerable<Entity>>>
    {
        private readonly ICustomerRepositoryAsync _positionRepository;
        private readonly IMapper _mapper;
        private readonly IModelHelper _modelHelper;

        public GetAllCustomersQueryHandler(ICustomerRepositoryAsync positionRepository, IMapper mapper, IModelHelper modelHelper)
        {
            _positionRepository = positionRepository;
            _mapper = mapper;
            _modelHelper = modelHelper;
        }

        public async Task<PagedResponse<IEnumerable<Entity>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {

            var validFilter = request;
            //filtered fields security
            if (!string.IsNullOrEmpty(validFilter.Fields))
            {
                //limit to fields in view model
                validFilter.Fields = _modelHelper.ValidateModelFields<GetCustomersViewModel>(validFilter.Fields);
            }
            if (string.IsNullOrEmpty(validFilter.Fields))
            {
                //default fields from view model
                validFilter.Fields = _modelHelper.GetModelFields<GetCustomersViewModel>();
            }
            // query based on filter
            var entityCustomers = await _positionRepository.GetPagedCustomerReponseAsync(validFilter);
            var data = entityCustomers.data;
            RecordsCount recordCount = entityCustomers.recordsCount;
            // response wrapper
            return new PagedResponse<IEnumerable<Entity>>(data, validFilter.PageNumber, validFilter.PageSize, recordCount);
        }
    }
}