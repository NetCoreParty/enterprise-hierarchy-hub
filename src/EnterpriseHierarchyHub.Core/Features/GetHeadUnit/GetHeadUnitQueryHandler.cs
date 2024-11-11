using EnterpriseHierarchyHub.Core.Domain;
using EnterpriseHierarchyHub.Core.Services;
using MediatR;

namespace EnterpriseHierarchyHub.Core.Features.GetHeadUnit;

public record GetHeadUnitQuery() : IRequest<OrganizationUnit>;

public class GetHeadUnitQueryHandler : IRequestHandler<GetHeadUnitQuery, OrganizationUnit>
{
    private readonly IOrganizationService _organizationService;

    public GetHeadUnitQueryHandler(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    public async Task<OrganizationUnit> Handle(GetHeadUnitQuery request, CancellationToken ct)
    {
        var headUnit = await _organizationService.GetHeadUnit(ct);

        return headUnit ?? throw new KeyNotFoundException("Head node not found.");
    }
}