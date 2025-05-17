using MediatR;

namespace qs.EmployeesApp.Application.Contracts.Commands;

public class DeleteEmployeeCommand : IRequest<bool>
{
    public long Id { get; }

    public DeleteEmployeeCommand(long id)
    {
        Id = id;
    }
}