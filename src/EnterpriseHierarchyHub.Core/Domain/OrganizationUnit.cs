namespace EnterpriseHierarchyHub.Core.Domain;

public class OrganizationUnit
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public OrganizationUnitType Type { get; set; }

    public Guid? ParentId { get; set; }

    public UnitDetails Details { get; set; } = new();

    public List<OrganizationUnit> Subunits { get; set; } = new();
}

public class UnitDetails
{
    public PersonDetails Person { get; set; } = new();

    public DepartmentDetails Department { get; set; } = new();
}

public class PersonDetails
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public PositionType Position { get; set; }
}

public class DepartmentDetails
{
    public string Name { get; set; } = string.Empty;
}