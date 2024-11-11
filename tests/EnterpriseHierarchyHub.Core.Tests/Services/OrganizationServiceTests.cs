using EnterpriseHierarchyHub.Core.Abstractions;
using EnterpriseHierarchyHub.Core.Domain;
using EnterpriseHierarchyHub.Core.Services;
using EnterpriseHierarchyHub.Core.Tests.Infrastructure;
using Moq;

namespace EnterpriseHierarchyHub.Core.Tests.Services;

public class OrganizationServiceTests
{
    private readonly Mock<IOrganizationService> _organizationServiceMock;

    public OrganizationServiceTests()
    {
        _organizationServiceMock = new Mock<IOrganizationService>();
    }

    [Fact]
    public async Task GetUnitById_ShouldReturnCorrectUnit()
    {
        // Arrange
        var testData = DataSeedingExtensions.GenerateOrganizationTree();
        var testUnit = testData.First();
        _organizationServiceMock.Setup(service => service.GetUnitById(testUnit.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(testUnit);

        // Act
        var result = await _organizationServiceMock.Object.GetUnitById(testUnit.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testUnit.Id, result.Id);
        Assert.Equal(testUnit.Name, result.Name);
    }

    [Fact]
    public async Task GetHeadUnit_ShouldReturnParentUnit()
    {
        // Arrange
        var testData = DataSeedingExtensions.GenerateOrganizationTree();
        var childUnit = testData.FirstOrDefault(u => u.ParentId != null);
        var parentUnit = testData.FirstOrDefault(u => u.Id == childUnit.ParentId);

        _organizationServiceMock.Setup(service => service.GetHeadUnit(It.IsAny<CancellationToken>()))
            .ReturnsAsync(parentUnit);

        // Act
        var result = await _organizationServiceMock.Object.GetHeadUnit();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(parentUnit.Id, result.Id);
    }

    [Fact]
    public async Task SearchUnits_ShouldReturnFilteredResults()
    {
        // Arrange
        var testData = DataSeedingExtensions.GenerateOrganizationTree();
        var nameToSearch = "Sales";
        var expectedTypeEnum = OrganizationUnitType.DEPARTMENT;
        var typeToSearch = expectedTypeEnum.ToString();
        var expectedResults = new PagedResult<OrganizationUnit>()
        {
            Items = testData.Where(unit => unit.Name.Contains(nameToSearch) && unit.Type == expectedTypeEnum).ToList(),
            PageNumber = 1,
            PageSize = 1,
            TotalCount = testData.Count,
        };

        _organizationServiceMock.Setup(service => service.SearchUnits(nameToSearch, typeToSearch, 1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResults);

        // Act
        var result = await _organizationServiceMock.Object.SearchUnits(nameToSearch, typeToSearch, 1, 10);

        // Assert
        Assert.NotEmpty(result.Items);
        Assert.All(result.Items, unit => Assert.Contains(nameToSearch, unit.Name));
        Assert.All(result.Items, unit => Assert.Equal(expectedTypeEnum, unit.Type));
    }
}