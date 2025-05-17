import type {
    DepartmentDto,
    PositionDto,
    EmployeeDto,
    CreateEmployeeDto,
    UpdateEmployeeDto,
    EmployeeDtoPaginatedList,
    ApiError,
    SortField,
    SortOrder,
    FilterField
} from '$types';

const BASE_URL = '/api';

async function handleResponse<T>(response: Response): Promise<T> {
    if (!response.ok) {
        let errorDetails;
        try {
            errorDetails = await response.json();
        } catch (e) {
            errorDetails = await response.text();
        }
        const error: ApiError = {
            message: `API Error: ${response.statusText} (Status: ${response.status})`,
            status: response.status,
            details: errorDetails,
        };
        console.error('API Error:', error);
        throw error;
    }
    if (response.status === 204) {
        return null as T;
    }
    return response.json() as Promise<T>;
}

export async function getDepartments(): Promise<DepartmentDto[]> {
    const response = await fetch(`${BASE_URL}/Departments`);
    return handleResponse<DepartmentDto[]>(response);
}

export async function getPositions(): Promise<PositionDto[]> {
    const response = await fetch(`${BASE_URL}/Positions`);
    return handleResponse<PositionDto[]>(response);
}

export async function getEmployees(
    pageNumber: number = 1,
    pageSize: number = 10,
    sortField?: SortField,
    sortOrder?: SortOrder,
    filterField?: FilterField,
    filterValue?: string
): Promise<EmployeeDtoPaginatedList> {
    const params = new URLSearchParams({
        PageNumber: pageNumber.toString(),
        PageSize: pageSize.toString(),
    });
    if (sortField) params.append('SortField', sortField);
    if (sortOrder) params.append('SortOrder', sortOrder);
    if (filterField && filterValue) {
        params.append('FilterField', filterField);
        params.append('FilterValue', filterValue);
    }

    const response = await fetch(`${BASE_URL}/Employees?${params.toString()}`);
    return handleResponse<EmployeeDtoPaginatedList>(response);
}

export async function createEmployee(employeeData: CreateEmployeeDto): Promise<EmployeeDto> {
    const response = await fetch(`${BASE_URL}/Employees`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(employeeData),
    });
    return handleResponse<EmployeeDto>(response);
}

export async function updateEmployee(id: number, employeeData: UpdateEmployeeDto): Promise<void> {
    const response = await fetch(`${BASE_URL}/Employees/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(employeeData),
    });
    await handleResponse<void>(response);
}

export async function deleteEmployee(id: number): Promise<void> {
    const response = await fetch(`${BASE_URL}/Employees/${id}`, {
        method: 'DELETE',
    });
    await handleResponse<void>(response);
}

export async function getEmployeeById(id: number): Promise<EmployeeDto> {
    const response = await fetch(`${BASE_URL}/Employees/${id}`);
    return handleResponse<EmployeeDto>(response);
}

export async function seedEmployees(count: number): Promise<{ message: string } | any> {
    try {
        const response = await fetch(`${BASE_URL}/Employees/seed?count=${count}`, {
            method: 'POST',
        });

        if (response.status === 201) {
            return { message: `Successfully seeded ${count} employees.` };
        }

        if (!response.ok) {
            return handleResponse<any>(response);
        }
    } catch (error) {
        console.error("Error seeding employees:", error);
        throw new Error(`Failed to seed employees: ${error instanceof Error ? error.message : String(error)}`);
    }
}
