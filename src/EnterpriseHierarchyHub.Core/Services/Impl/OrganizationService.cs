using EnterpriseHierarchyHub.Core.Abstractions;
using EnterpriseHierarchyHub.Core.Domain;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EnterpriseHierarchyHub.Core.Services.Impl;

public class OrganizationService : IOrganizationService
{
    private readonly IMongoCollection<OrganizationUnit> _organizationUnits;

    public OrganizationService(IMongoCollection<OrganizationUnit> organizationUnits)
    {
        _organizationUnits = organizationUnits;
    }

    public void EnsureIndexesCreated()
    {
        // !!!(Important note)  Indexes in MongoDB are persistent once created.
        // MongoDB will check if the specified indexes already exist. If the indexes are missing, MongoDB will create them; if they already exist, MongoDB will skip creation.

        // Define an index on the "name" field with case-insensitive collation (optional)
        var nameIndex = new CreateIndexModel<OrganizationUnit>(
            Builders<OrganizationUnit>.IndexKeys.Ascending(u => u.Name),
            new CreateIndexOptions
            {
                Name = "Idx_Name",
                Collation = new Collation("en", strength: CollationStrength.Primary)
            }
        );

        // Define an index on the "type" field
        var typeIndex = new CreateIndexModel<OrganizationUnit>(
            Builders<OrganizationUnit>.IndexKeys.Ascending(u => u.Type),
            new CreateIndexOptions
            {
                Name = "Idx_Type"
            }
        );

        _organizationUnits.Indexes.CreateMany(new[] { nameIndex, typeIndex });
    }

    public async Task<OrganizationUnit> GetHeadUnit(CancellationToken ct)
    {
        var filter = Builders<OrganizationUnit>.Filter.And(
            Builders<OrganizationUnit>.Filter.Eq(u => u.ParentId, null),
            Builders<OrganizationUnit>.Filter.Eq(u => u.Type, OrganizationUnitType.CEO)
        );

        return await _organizationUnits
            .Find(filter)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<OrganizationUnit> GetUnitById(Guid unitId, CancellationToken ct)
    {
        var filter = Builders<OrganizationUnit>.Filter.Eq(u => u.Id, unitId);
        return await _organizationUnits
            .Find(filter)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<PagedResult<OrganizationUnit>> GetChildUnits(Guid? parentId, int pageNumber, int pageSize, CancellationToken ct)
    {
        var filter = parentId == null
            ? Builders<OrganizationUnit>.Filter.Eq(u => u.ParentId, null)
            : Builders<OrganizationUnit>.Filter.Eq(u => u.ParentId, parentId);

        var totalCount = await _organizationUnits.CountDocumentsAsync(filter, cancellationToken: ct);

        var units = await _organizationUnits
            .Find(filter)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(ct);

        return new PagedResult<OrganizationUnit>
        {
            Items = units,
            TotalCount = (int)totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<PagedResult<OrganizationUnit>> SearchUnits(string name, string type, int pageNumber, int pageSize, CancellationToken ct)
    {
        var filters = new List<FilterDefinition<OrganizationUnit>>();

        if (!string.IsNullOrEmpty(name))
        {
            filters.Add(Builders<OrganizationUnit>.Filter.Regex(u => u.Name, new BsonRegularExpression(name, "i")));
        }

        if (!string.IsNullOrEmpty(type))
        {
            filters.Add(Builders<OrganizationUnit>.Filter.Eq(u => u.Type.ToString(), type));
        }

        var combinedFilter = filters.Any()
            ? Builders<OrganizationUnit>.Filter.And(filters)
            : FilterDefinition<OrganizationUnit>.Empty;

        var totalCount = await _organizationUnits.CountDocumentsAsync(combinedFilter, cancellationToken: ct);

        var units = await _organizationUnits
            .Find(combinedFilter)
            .Skip((pageNumber - 1) * pageSize)
        .Limit(pageSize)
            .ToListAsync(ct);

        return new PagedResult<OrganizationUnit>
        {
            Items = units,
            TotalCount = (int)totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}