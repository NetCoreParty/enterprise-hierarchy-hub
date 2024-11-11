using EnterpriseHierarchyHub.Core.Domain;
using EnterpriseHierarchyHub.Core.Services;
using MediatR;

namespace EnterpriseHierarchyHub.Core.Features.GetUnitById;

public record GetUnitByIdQuery(Guid Id) : IRequest<OrganizationUnit>;

public class GetUnitByIdHandler : IRequestHandler<GetUnitByIdQuery, OrganizationUnit>
{
    private readonly IOrganizationService _organizationService;

    public GetUnitByIdHandler(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    public async Task<OrganizationUnit> Handle(GetUnitByIdQuery request, CancellationToken ct)
    {
        var unit = await _organizationService.GetUnitById(request.Id, ct);
        return unit ?? throw new KeyNotFoundException("Organization unit not found.");
    }
}