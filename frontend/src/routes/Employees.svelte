<script lang="ts">
    import { onMount, createEventDispatcher } from 'svelte';
    import type { EmployeeDto, EmployeeDtoPaginatedList, SortField, SortOrder, FilterField } from '$types';
    import {getEmployees, deleteEmployee, seedEmployees, getDepartments, getPositions} from '$services/apiService';
    import EmployeeModal from '$components/EmployeeModal.svelte';
    import DeleteConfirmModal from '$components/DeleteConfirmModal.svelte';
    import TableSpinner from '$components/TableSpinner.svelte';
    import {departments, departments as storeDepartments, positions, positions as storePositions} from '$store/index';
    import SeedDataModal from "$components/SeedDataModal.svelte";

    let employeesList: EmployeeDto[] = [];
    let isLoading = true;
    let error: string | null = null;

    // Pagination
    let currentPage = 1;
    const pageSizes: number[] = [3, 5, 10, 15, 20, 25, 50];
    let pageSize: number = pageSizes[2];
    let totalPages = 1;
    let totalCount = 0;

    // Sorting
    let currentSortField: SortField = 'id';
    let currentSortOrder: SortOrder = 'asc';

    // Filtering
    let activeFilterField: FilterField | 'none' = 'none';
    let filterValue = '';

    const filterFieldsOptions: { value: FilterField | 'none'; label: string }[] = [
        { value: 'none', label: 'Без фильтра' },
        { value: 'name', label: 'Ф.И.О.' },
        { value: 'position', label: 'Должность' },
    ];

    // Modals
    let showEmployeeModal = false;
    let editingEmployee: EmployeeDto | null = null;
    let showDeleteModal = false;
    let employeeToDelete: EmployeeDto | null = null;
    let showSeedModal = false;
    let seedLoading = false;
    let seedError: string | null = null;
    let seedSuccessMessage: string | null = null;

    const dispatch = createEventDispatcher();

    async function fetchEmployees() {
        isLoading = true;
        error = null;
        try {
            const effectiveFilterField = activeFilterField === 'none' ? undefined : activeFilterField;
            const effectiveFilterValue = activeFilterField === 'none' || !filterValue.trim() ? undefined : filterValue.trim();

            const data: EmployeeDtoPaginatedList = await getEmployees(
                currentPage,
                pageSize,
                currentSortField,
                currentSortOrder,
                effectiveFilterField,
                effectiveFilterValue
            );
            employeesList = data.items || [];
            totalPages = data.totalPages;
            totalCount = data.totalCount;
        } catch (e: any) {
            console.error("Failed to fetch employees:", e);
            error = `Не удалось загрузить сотрудников: ${e.message || 'Ошибка API'}`;
            employeesList = [];
        } finally {
            isLoading = false;
        }
    }

    onMount(() => {
        fetchEmployees();
    });

    function handleSort(field: SortField) {
        if (currentSortField === field) {
            currentSortOrder = currentSortOrder === 'asc' ? 'desc' : 'asc';
        } else {
            currentSortField = field;
            currentSortOrder = 'asc';
        }
        currentPage = 1;
        fetchEmployees();
    }

    function handlePageSizeChange(event: Event) {
        const target = event.target as HTMLSelectElement;
        pageSize = parseInt(target.value);
        currentPage = 1;
        fetchEmployees();
    }

    function applyFilter() {
        currentPage = 1;
        fetchEmployees();
    }

    function clearFilter() {
        activeFilterField = 'none';
        filterValue = '';
        applyFilter();
    }

    function handlePageChange(newPage: number) {
        if (newPage >= 1 && newPage <= totalPages && newPage !== currentPage) {
            currentPage = newPage;
            fetchEmployees();
        }
    }

    function openCreateModal() {
        editingEmployee = null;
        showEmployeeModal = true;
    }

    function openEditModal(employee: EmployeeDto) {
        editingEmployee = { ...employee };
        showEmployeeModal = true;
    }

    function openDeleteModal(employee: EmployeeDto) {
        employeeToDelete = employee;
        showDeleteModal = true;
    }

    function openSeedModal() {
        seedError = null;
        seedSuccessMessage = null;
        showSeedModal = true;
    }

    function closeEmployeeModal() {
        showEmployeeModal = false;
        editingEmployee = null;
    }

    function closeDeleteModal() {
        showDeleteModal = false;
        employeeToDelete = null;
    }

    function closeSeedModal() {
        showSeedModal = false;
    }

    async function handleEmployeeSaved() {
        closeEmployeeModal();
        await fetchEmployees();
    }

    async function confirmDelete() {
        if (employeeToDelete) {
            isLoading = true;
            try {
                await deleteEmployee(employeeToDelete.id);
                closeDeleteModal();
                // Если удалили последнего на странице, и это не первая страница, перейдем на предыдущую
                if (employeesList.length === 1 && currentPage > 1) {
                    currentPage--;
                }
                await fetchEmployees();
            } catch (e: any) {
                console.error("Failed to delete employee:", e);
                error = `Не удалось удалить сотрудника: ${e.message || 'Ошибка API'}`;
            } finally {
                isLoading = false;
            }
        }
    }

    async function handleSeedConfirm(event: CustomEvent<{ count: number }>) {
        const { count } = event.detail;
        seedLoading = true;
        seedError = null;
        seedSuccessMessage = null;
        try {
            const result = await seedEmployees(count);
            seedSuccessMessage = `Успешно создано ${count} тестовых сотрудников.`;
            closeSeedModal();
            await fetchEmployees();
            //fetch additional data after seed
            const fetchedDepartments = await getDepartments();
            departments.set(fetchedDepartments);

            const fetchedPositions = await getPositions();
            positions.set(fetchedPositions);
        } catch (e: any) {
            console.error("Failed to seed employees:", e);
            seedError = `Не удалось создать тестовые данные: ${e.message || 'Ошибка API'}`;
        } finally {
            seedLoading = false;
        }
    }

    function formatDate(dateString: string | null | undefined): string {
        if (!dateString) return 'N/A';
        try {
            const date = new Date(dateString);
            if (isNaN(date.getTime())) return 'Неверная дата';
            return date.toLocaleDateString('ru-RU');
        } catch (e) {
            return 'Неверная дата';
        }
    }

    function formatSalary(salary: number | null | undefined): string {
        if (salary === null || salary === undefined) return 'N/A';
        return new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'MDL', minimumFractionDigits: 0 }).format(salary);
    }

    $: if (activeFilterField === 'none' && filterValue !== '') {
        filterValue = '';
        //applyFilter
    }

</script>

<div class="container mt-3 text-start">
    <h2 class="mb-4">Список сотрудников</h2>

    {#if error}
        <div class="alert alert-danger" role="alert">
            {error}
        </div>
    {/if}

    <div class="row filter-controls mb-3 align-items-end">
        <div class="col-md-3 col-sm-6 mb-2">
            <label for="filterFieldSelect" class="form-label">Фильтровать по:</label>
            <select id="filterFieldSelect" class="form-select" bind:value={activeFilterField}>
                {#each filterFieldsOptions as option}
                    <option value={option.value}>{option.label}</option>
                {/each}
            </select>
        </div>
        <div class="col-md-3 col-sm-6 mb-2">
            <label for="filterValueInput" class="form-label">Значение:</label>
            <input type="text" id="filterValueInput" class="form-control" bind:value={filterValue} disabled={activeFilterField === 'none'} placeholder={activeFilterField !== 'none' ? 'Введите значение...' : ''}>
        </div>
        <div class="col-md-auto col-sm-12 mb-2">
            <button class="btn btn-primary me-2 w-sm-100 mb-sm-1" on:click={applyFilter} disabled={activeFilterField === 'none' && !filterValue.trim()}><i class="fas fa-filter"></i> Применить</button>
            <button class="btn btn-secondary w-sm-100" on:click={clearFilter}><i class="fas a"></i> Сбросить</button>
        </div>
        <div class="col-md-auto ms-md-auto text-md-end col-sm-12 mb-2">
            <button class="btn btn-success w-sm-100" on:click={openCreateModal}><i class="fa-solid fa-user-plus"></i> Создать</button>
            <button class="btn btn-primary w-sm-100" on:click={openSeedModal} aria-label="seed data"><i class="fa-solid fa-seedling"></i></button>
        </div>
    </div>


    {#if isLoading && !employeesList.length}
        <TableSpinner />
    {:else if !employeesList.length && !isLoading}
        <div class="alert alert-info">Сотрудники не найдены. Попробуйте изменить фильтры или создать первого сотрудника.</div>
    {:else}
        <div class="table-responsive">
            <table class="table table-striped table-hover table-bordered">
                <thead class="table-dark">
                <tr>
                    <th class="sortable" on:click={() => handleSort('name')}>Ф.И.О {#if currentSortField === 'name'}<i class="fas {currentSortOrder === 'asc' ? 'fa-sort-up' : 'fa-sort-down'}"></i>{/if}</th>
                    <th class="sortable" on:click={() => handleSort('departmentId')}>Отдел {#if currentSortField === 'departmentId'}<i class="fas {currentSortOrder === 'asc' ? 'fa-sort-up' : 'fa-sort-down'}"></i>{/if}</th>
                    <th class="sortable" on:click={() => handleSort('positionId')}>Должность {#if currentSortField === 'positionId'}<i class="fas {currentSortOrder === 'asc' ? 'fa-sort-up' : 'fa-sort-down'}"></i>{/if}</th>
                    <th class="sortable" on:click={() => handleSort('dateOfBirth')}>Дата рождения {#if currentSortField === 'dateOfBirth'}<i class="fas {currentSortOrder === 'asc' ? 'fa-sort-up' : 'fa-sort-down'}"></i>{/if}</th>
                    <th class="sortable" on:click={() => handleSort('employmentDate')}>Дата устройства {#if currentSortField === 'employmentDate'}<i class="fas {currentSortOrder === 'asc' ? 'fa-sort-up' : 'fa-sort-down'}"></i>{/if}</th>
                    <th class="sortable" on:click={() => handleSort('salary')}>Заработная плата {#if currentSortField === 'salary'}<i class="fas {currentSortOrder === 'asc' ? 'fa-sort-up' : 'fa-sort-down'}"></i>{/if}</th>
                    <th>Действия</th>
                </tr>
                </thead>
                <tbody>
                {#each employeesList as employee (employee.id)}
                    <tr>
                        <td>{employee.name}</td>
                        <td>{employee.departmentName}</td>
                        <td>{employee.positionName}</td>
                        <td>{formatDate(employee.dateOfBirth)}</td>
                        <td>{formatDate(employee.employmentDate)}</td>
                        <td>{formatSalary(employee.salary)}</td>
                        <td class="action-buttons">
                            <button class="btn btn-sm btn-primary" aria-label="Редактировать" on:click={() => openEditModal(employee)} title="Редактировать">
                                <i class="fa-solid fa-pencil"></i>
                            </button>
                            <button class="btn btn-sm btn-danger" aria-label="delete" on:click={() => openDeleteModal(employee)} title="Удалить">
                                <i class="fas fa-trash"></i>
                            </button>
                        </td>
                    </tr>
                {/each}
                </tbody>
            </table>
        </div>

        <div class="pagination-controls mt-4">
            <div class="page-size-selector mb-2 mb-md-0">
                <label for="pageSizeSelect" class="form-label me-2">Записей на странице:</label>
                <select id="pageSizeSelect" class="form-select form-select-sm" style="width: auto;" bind:value={pageSize} on:change={handlePageSizeChange}>
                    {#each pageSizes as size}
                        <option value={size}>{size}</option>
                    {/each}
                </select>
            </div>

            {#if totalPages > 1}
                <nav aria-label="Page navigation">
                    <ul class="pagination justify-content-center mb-0">
                        <li class="page-item" class:disabled={currentPage === 1}>
                            <button class="page-link" on:click={() => handlePageChange(1)} disabled={currentPage === 1} aria-label="First">
                                <span aria-hidden="true"><i class="fa-solid fa-angles-left"></i></span>
                            </button>
                        </li>
                        <li class="page-item" class:disabled={currentPage === 1}>
                            <button class="page-link" on:click={() => handlePageChange(currentPage - 1)} disabled={currentPage === 1} aria-label="Previous">
                                <span aria-hidden="true"><i class="fa-solid fa-angle-left"></i></span>
                            </button>
                        </li>

                        {#if currentPage > 2}
                            <li class="page-item"><button class="page-link" on:click={() => handlePageChange(currentPage - 2)}>{currentPage - 2}</button></li>
                        {/if}
                        {#if currentPage > 1}
                            <li class="page-item"><button class="page-link" on:click={() => handlePageChange(currentPage - 1)}>{currentPage - 1}</button></li>
                        {/if}

                        <li class="page-item active" aria-current="page">
                            <span class="page-link">{currentPage}</span>
                        </li>

                        {#if currentPage < totalPages}
                            <li class="page-item"><button class="page-link" on:click={() => handlePageChange(currentPage + 1)}>{currentPage + 1}</button></li>
                        {/if}
                        {#if currentPage < totalPages - 1}
                            <li class="page-item"><button class="page-link" on:click={() => handlePageChange(currentPage + 2)}>{currentPage + 2}</button></li>
                        {/if}


                        <li class="page-item" class:disabled={currentPage === totalPages}>
                            <button class="page-link" on:click={() => handlePageChange(currentPage + 1)} disabled={currentPage === totalPages} aria-label="Next">
                                <span aria-hidden="true"><i class="fa-solid fa-angle-right"></i></span>
                            </button>
                        </li>
                        <li class="page-item" class:disabled={currentPage === totalPages}>
                            <button class="page-link" on:click={() => handlePageChange(totalPages)} disabled={currentPage === totalPages} aria-label="Last">
                                <span aria-hidden="true"><i class="fa-solid fa-angles-right"></i></span>
                            </button>
                        </li>
                    </ul>
                </nav>
            {/if}
            <div class="text-muted ms-md-2 mt-2 mt-md-0">Стр. {currentPage} из {totalPages} (Всего: {totalCount})</div>
        </div>
    {/if}
</div>

{#if showEmployeeModal}
    <EmployeeModal
            employee={editingEmployee}
            departments={$storeDepartments}
            positions={$storePositions}
            on:close={closeEmployeeModal}
            on:save={handleEmployeeSaved}
    />
{/if}

{#if showDeleteModal && employeeToDelete}
    <DeleteConfirmModal
            employeeName={employeeToDelete.name}
            on:close={closeDeleteModal}
            on:confirm={confirmDelete}
    />
{/if}

{#if showSeedModal}
    <SeedDataModal
            isLoading={seedLoading}
            on:close={closeSeedModal}
            on:confirm={handleSeedConfirm}
    />
{/if}