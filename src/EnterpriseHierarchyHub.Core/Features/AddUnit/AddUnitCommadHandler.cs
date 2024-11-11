using EnterpriseHierarchyHub.Core.Domain;
using MediatR;

namespace EnterpriseHierarchyHub.Core.Features.AddUnit;

public record AddUnitCommand(OrganizationUnit Unit) : IRequest<OrganizationUnit>;

public class AddUnitCommadHandler : IRequestHandler<AddUnitCommand, OrganizationUnit>
{
    public Task<OrganizationUnit> Handle(AddUnitCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}