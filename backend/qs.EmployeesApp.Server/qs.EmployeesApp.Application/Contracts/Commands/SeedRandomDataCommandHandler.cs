using Bogus;
using MediatR;
using qs.EmployeesApp.Application.Contracts.Repositories;
using qs.EmployeesApp.Domain.Entities;

namespace qs.EmployeesApp.Application.Contracts.Commands;

public class SeedRandomDataCommandHandler : IRequestHandler<SeedRandomDataCommand, bool>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IPositionRepository _positionRepository;

    private readonly string[] _positionNames =
    [
        "Data Scientist",
        "Marketing Manager",
        "Financial Analyst",
        "Graphic Designer",
        "Project Manager",
        "Human Resources Specialist",
        "Civil Engineer",
        "Registered Nurse",
        "Sales Representative",
        "Content Writer",
        "UX/UI Designer",
        "Operations Manager",
        "Mechanical Engineer",
        "Customer Service Representative",
        "Business Analyst",
        "Network Administrator",
        "Environmental Scientist",
        "Executive Assistant",
        "Supply Chain Manager",
        "Electrical Engineer",
        "Public Relations Specialist",
        "Teacher",
        "Logistics Coordinator",
        "Cybersecurity Analyst",
        "Product Manager",
        "Social Media Manager",
        "Architect",
        "Pharmacist",
        "Legal Counsel",
        "Machine Learning Engineer",
        "Event Planner",
        "Quality Assurance Tester",
        "Research Scientist",
        "Interior Designer",
        "Construction Manager",
        "Technical Writer",
        "Systems Analyst",
        "Occupational Therapist",
        "Real Estate Agent",
        "Data Engineer",
        "Corporate Trainer",
        "Biomedical Engineer",
        "Investment Banker",
        "Copywriter",
        "Urban Planner",
        "DevOps Engineer",
        "Clinical Psychologist",
        "Procurement Specialist",
        "Animation Artist",
    ];

    private readonly string[] _departmentNames =
    [
        "Marketing",
        "Finance",
        "Operations",
        "Sales",
        "Customer Support",
        "Research and Development",
        "Legal",
        "Engineering",
        "Procurement",
        "Quality Assurance",
        "Public Relations",
        "Supply Chain",
        "Business Development"
    ];

    public SeedRandomDataCommandHandler(IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository, IPositionRepository positionRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _positionRepository = positionRepository;
    }

    public async Task<bool> Handle(SeedRandomDataCommand request, CancellationToken cancellationToken)
    {
        Random random = new Random();

        var existingPositions = await _positionRepository.ListAsync(cancellationToken);

        var notExistingPositions = _positionNames
            .Where(n => !existingPositions.Any(p => p.Name.Equals(n, StringComparison.InvariantCultureIgnoreCase)))
            .Select(x => new Position
            {
                Name = x,
                BaseSalary = (uint)random.Next(5, 120) * 1_000
            }).ToList();

        var newPositions = await _positionRepository.AddRangeAsync(notExistingPositions, cancellationToken);

        var existingDepartments = await _departmentRepository.ListAsync(cancellationToken);

        var notExistingDepartments = _departmentNames
            .Where(n => !existingDepartments.Any(d => d.Name.Equals(n, StringComparison.InvariantCultureIgnoreCase)))
            .Select(x => new Department
            {
                Name = x
            }).ToList();

        var newDepartments = await _departmentRepository.AddRangeAsync(notExistingDepartments, cancellationToken);

        var faker = new Faker("ru");

        var allPositions = existingPositions.UnionBy(newPositions, p => p.Id).ToList();
        var allDepartments = existingDepartments.UnionBy(newDepartments, d => d.Id).ToList();

        var notExistingEmployees = Enumerable.Range(0, request.Count).Select(_ => new Employee
        {
            Name = faker.Name.FullName(),
            DateOfBirth = new DateOnly(random.Next(1930, DateTime.Now.Year - 18), random.Next(1, 13),
                random.Next(1, 28)),
            EmploymentDate = new DateOnly(random.Next(2015, DateTime.Now.Year), random.Next(1, 13), random.Next(1, 28)),
            PositionId = allPositions[random.Next(0, allPositions.Count)].Id,
            DepartmentId = allDepartments[random.Next(0, allDepartments.Count)].Id,
            SalaryAdjustment = random.Next(-3_000, 90_001),
        }).ToList();

        await _employeeRepository.AddRangeAsync(notExistingEmployees, cancellationToken);

        return true;
    }
}