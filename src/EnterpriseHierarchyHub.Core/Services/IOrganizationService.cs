using EnterpriseHierarchyHub.Core.Abstractions;
using EnterpriseHierarchyHub.Core.Domain;

namespace EnterpriseHierarchyHub.Core.Services;

public interface IOrganizationService
{
    void EnsureIndexesCreated();

    Task<OrganizationUnit?> GetHeadUnit(CancellationToken ct = default);

    Task<OrganizationUnit?> GetUnitById(Guid unitId, CancellationToken ct = default);

    Task<PagedResult<OrganizationUnit>> GetChildUnits(Guid? parentId, int pageNumber, int pageSize, CancellationToken ct = default);

    Task<PagedResult<OrganizationUnit>> SearchUnits(string name, string type, int pageNumber, int pageSize, CancellationToken ct = default);
}