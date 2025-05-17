export interface DepartmentDto {
    id: number;
    name: string;
}

export interface PositionDto {
    id: number;
    name: string;
    baseSalary: number;
}

export interface EmployeeDto {
    id: number;
    name: string;
    dateOfBirth: string; // format: date "YYYY-MM-DD"
    employmentDate: string; // format: date "YYYY-MM-DD"
    salary: number | null;
    salaryAdjustment: number;
    positionId: number;
    positionName: string;
    departmentId: number;
    departmentName: string;
}

export interface CreateEmployeeDto {
    name: string;
    dateOfBirth: string; // "YYYY-MM-DD"
    employmentDate: string; // "YYYY-MM-DD"
    positionId: number;
    departmentId: number;
    salaryAdjustment: number;
}

export interface UpdateEmployeeDto {
    id: number;
    name: string;
    dateOfBirth: string; // "YYYY-MM-DD"
    employmentDate: string; // "YYYY-MM-DD"
    salaryAdjustment: number;
    positionId: number;
    departmentId: number;
}

export interface EmployeeDtoPaginatedList {
    items: EmployeeDto[];
    pageNumber: number;
    totalPages: number;
    totalCount: number;
}

export interface ApiError {
    message: string;
    status?: number;
    details?: any;
}

export type SortField = 'id' | 'name' | 'dateOfBirth' | 'employmentDate' | 'positionId' | 'departmentId' | 'salary';
export type SortOrder = 'asc' | 'desc';

export type FilterField = 'name' | 'position';