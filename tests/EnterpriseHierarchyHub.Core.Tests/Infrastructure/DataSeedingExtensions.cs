using Bogus;
using EnterpriseHierarchyHub.Core.Domain;

namespace EnterpriseHierarchyHub.Core.Tests.Infrastructure;

public static class DataSeedingExtensions
{
    public static List<OrganizationUnit> GenerateOrganizationTree(int ceoCount = 1, int vpCount = 2, int deptCount = 3, int employeeCount = 5)
    {
        var faker = new Faker();

        // Generate CEOs
        var ceos = new Faker<OrganizationUnit>()
            .RuleFor(o => o.Id, f => Guid.NewGuid())
            .RuleFor(o => o.Name, f => f.Name.FullName())
            .RuleFor(o => o.Type, f => OrganizationUnitType.CEO)
            .RuleFor(o => o.ParentId, f => (Guid?)null)
            .Generate(ceoCount);

        // Generate VPs under each CEO
        var units = new List<OrganizationUnit>(ceos);
        foreach (var ceo in ceos)
        {
            var vps = new Faker<OrganizationUnit>()
                .RuleFor(o => o.Id, f => Guid.NewGuid())
                .RuleFor(o => o.Name, f => f.Name.FullName())
                .RuleFor(o => o.Type, f => OrganizationUnitType.VP)
                .RuleFor(o => o.ParentId, f => ceo.Id)
                .Generate(vpCount);

            units.AddRange(vps);

            // Generate Departments under each VP
            foreach (var vp in vps)
            {
                var departments = new Faker<OrganizationUnit>()
                    .RuleFor(o => o.Id, f => Guid.NewGuid())
                    .RuleFor(o => o.Name, f => f.Commerce.Department())
                    .RuleFor(o => o.Type, f => OrganizationUnitType.DEPARTMENT)
                    .RuleFor(o => o.ParentId, f => vp.Id)
                    .Generate(deptCount);

                units.AddRange(departments);

                // Generate Employees under each Department
                foreach (var department in departments)
                {
                    var employees = new Faker<OrganizationUnit>()
                        .RuleFor(o => o.Id, f => Guid.NewGuid())
                        .RuleFor(o => o.Name, f => f.Name.FullName())
                        .RuleFor(o => o.Type, f => OrganizationUnitType.EMPLOYEE)
                        .RuleFor(o => o.ParentId, f => department.Id)
                        .Generate(employeeCount);

                    units.AddRange(employees);
                }
            }
        }

        return units;
    }
}