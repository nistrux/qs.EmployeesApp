using MediatR;

namespace qs.EmployeesApp.Application.Contracts.Commands;

public class SeedRandomDataCommand : IRequest<bool>
{
    public int Count { get; }

    public SeedRandomDataCommand(int count)
    {
        Count = count;
    }
}