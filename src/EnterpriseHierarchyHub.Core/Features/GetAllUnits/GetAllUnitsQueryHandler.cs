using EnterpriseHierarchyHub.Core.Abstractions;
using EnterpriseHierarchyHub.Core.Domain;
using EnterpriseHierarchyHub.Core.Services;
using MediatR;

namespace EnterpriseHierarchyHub.Core.Features.GetAllUnits;

public record GetAllUnitsQuery(Guid? ParentId, int PageNumber, int PageSize) : IRequest<PagedResult<OrganizationUnit>>;

public class GetAllUnitsQueryHandler : IRequestHandler<GetAllUnitsQuery, PagedResult<OrganizationUnit>>
{
    private readonly IOrganizationService _organizationService;

    public GetAllUnitsQueryHandler(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    public Task<PagedResult<OrganizationUnit>> Handle(GetAllUnitsQuery request, CancellationToken cancellationToken)
        => _organizationService.GetChildUnits(request.ParentId, request.PageNumber, request.PageSize, cancellationToken);
}