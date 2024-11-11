using EnterpriseHierarchyHub.Core.Abstractions;
using EnterpriseHierarchyHub.Core.Domain;
using EnterpriseHierarchyHub.Core.Services;
using MediatR;

namespace EnterpriseHierarchyHub.Core.Features.SearchUnits;

public record SearchUnitsQuery(string Name, string Type, int PageNumber, int PageSize) : IRequest<PagedResult<OrganizationUnit>>;

public class SearchUnitsQueryHandler : IRequestHandler<SearchUnitsQuery, PagedResult<OrganizationUnit>>
{

    private readonly IOrganizationService _organizationService;

    public SearchUnitsQueryHandler(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    public Task<PagedResult<OrganizationUnit>> Handle(SearchUnitsQuery request, CancellationToken ct)
        => _organizationService.SearchUnits(
            request.Name,
            request.Type,
            request.PageNumber,
            request.PageSize,
            ct);
}