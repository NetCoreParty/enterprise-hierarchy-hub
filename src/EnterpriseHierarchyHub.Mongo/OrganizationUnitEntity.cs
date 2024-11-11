using EnterpriseHierarchyHub.Core.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace EnterpriseHierarchyHub.Mongo;

public class OrganizationUnitEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrganizationUnitType Type { get; set; }

    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid? ParentId { get; set; }

    public UnitDetailsEntity Details { get; set; } = new();

    public List<OrganizationUnitEntity> Subunits { get; set; } = new();
}

public class UnitDetailsEntity
{
    public PersonDetailsEntity Person { get; set; } = new();

    public DepartmentDetailsEntity Department { get; set; } = new();
}

public class PersonDetailsEntity
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    [BsonElement("position")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PositionType? Position { get; set; }
}

public class DepartmentDetailsEntity
{
    public string Name { get; set; } = string.Empty;
}