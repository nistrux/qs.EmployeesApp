<script lang="ts">
    import { createEventDispatcher, onMount } from 'svelte';
    import type { EmployeeDto, CreateEmployeeDto, UpdateEmployeeDto, DepartmentDto, PositionDto, ApiError } from '$types';
    import { createEmployee, updateEmployee } from '$services/apiService';

    export let employee: EmployeeDto | null = null;
    export let departments: DepartmentDto[] = [];
    export let positions: PositionDto[] = [];

    const dispatch = createEventDispatcher();

    let formData: Omit<CreateEmployeeDto, 'id'> & { id?: number } = {
        id: undefined,
        name: '',
        dateOfBirth: '',
        employmentDate: '',
        positionId: 0,
        departmentId: 0,
        salaryAdjustment: 0,
    };

    let selectedPosition: PositionDto | null = null;
    let baseSalaryDisplay: number = 0;
    let totalCalculatedSalary: number = 0;

    let isLoading = false;
    let error: string | null = null;
    let formTitle = '';

    onMount(() => {
        if (employee) {
            formTitle = 'Редактировать сотрудника';
            formData = {
                id: employee.id,
                name: employee.name,
                dateOfBirth: employee.dateOfBirth.split('T')[0],
                employmentDate: employee.employmentDate.split('T')[0],
                positionId: employee.positionId,
                departmentId: employee.departmentId,
                salaryAdjustment: employee.salaryAdjustment || 0,
            };
            const pos = positions.find(p => p.id === employee.positionId);
            if (pos) {
                selectedPosition = pos;
                baseSalaryDisplay = pos.baseSalary;
            }

        } else {
            formTitle = 'Создать нового сотрудника';
            formData = {
                name: '',
                dateOfBirth: '',
                employmentDate: '',
                positionId: positions.length > 0 ? positions[0].id : 0,
                departmentId: departments.length > 0 ? departments[0].id : 0,
                salaryAdjustment: 0,
            };
            if (positions.length > 0 && formData.positionId > 0) {
                const pos = positions.find(p => p.id === formData.positionId);
                if (pos) {
                    selectedPosition = pos;
                    baseSalaryDisplay = pos.baseSalary;
                }
            } else {
                selectedPosition = null;
                baseSalaryDisplay = 0;
            }
        }
        calculateTotalSalary();
    });

    function handlePositionChange(event: Event) {
        const target = event.target as HTMLSelectElement;
        const newPositionId = parseInt(target.value);
        formData.positionId = newPositionId;
        selectedPosition = positions.find(p => p.id === newPositionId) || null;
        baseSalaryDisplay = selectedPosition ? selectedPosition.baseSalary : 0;
        calculateTotalSalary();
    }

    function handleSalaryAdjustmentChange() {
        calculateTotalSalary();
    }

    function calculateTotalSalary() {
        totalCalculatedSalary = (selectedPosition ? selectedPosition.baseSalary : 0) + (Number(formData.salaryAdjustment) || 0);
    }


    async function handleSubmit() {
        isLoading = true;
        error = null;

        if (!formData.name || !formData.dateOfBirth || !formData.employmentDate || !formData.positionId || !formData.departmentId) {
            error = "Пожалуйста, заполните все обязательные поля (ФИО, даты, должность, отдел).";
            isLoading = false;
            return;
        }
        if (Number(formData.salaryAdjustment) < -3000) {
            error = "Надбавка не может быть меньше -3000";
            isLoading = false;
            return;
        }
        if (Number(formData.salaryAdjustment) > 90000) {
            error = "Надбавка не может быть больше 90000";
            isLoading = false;
            return;
        }

        try {
            if (employee && formData.id) {
                const updateData: UpdateEmployeeDto = {
                    id: formData.id,
                    name: formData.name,
                    dateOfBirth: formData.dateOfBirth,
                    employmentDate: formData.employmentDate,
                    positionId: formData.positionId,
                    departmentId: formData.departmentId,
                    salaryAdjustment: Number(formData.salaryAdjustment) || 0,
                };
                await updateEmployee(formData.id, updateData);
            } else {
                const createData: CreateEmployeeDto = {
                    name: formData.name,
                    dateOfBirth: formData.dateOfBirth,
                    employmentDate: formData.employmentDate,
                    positionId: formData.positionId,
                    departmentId: formData.departmentId,
                    salaryAdjustment: Number(formData.salaryAdjustment) || 0,
                };
                await createEmployee(createData);
            }
            dispatch('save');
        } catch (e: any) {
            console.error("Failed to save employee:", e);
            const apiError = e as ApiError;
            if (apiError.details && typeof apiError.details === 'object' && apiError.details.errors) {
                const validationErrors = Object.entries(apiError.details.errors)
                    .map(([field, messages]) => `${field}: ${(messages as string[]).join(', ')}`)
                    .join('; ');
                error = `Ошибка валидации: ${validationErrors}`;
            } else {
                error = `Не удалось сохранить сотрудника: ${apiError.message || 'Ошибка API'}`;
            }
        } finally {
            isLoading = false;
        }
    }

    function closeModal() {
        dispatch('close');
    }

    $: {
        if (formData.positionId && positions.length > 0) {
            selectedPosition = positions.find(p => p.id === formData.positionId) || null;
            baseSalaryDisplay = selectedPosition ? selectedPosition.baseSalary : 0;
        } else {
            selectedPosition = null;
            baseSalaryDisplay = 0;
        }
        calculateTotalSalary();
    }

</script>

<div class="modal fade show d-block" tabindex="-1" role="dialog" aria-labelledby="employeeModalLabel" aria-modal="true">
    <div class="modal-dialog modal-lg modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="employeeModalLabel">{formTitle}</h5>
                <button type="button" class="btn-close" on:click={closeModal} aria-label="Close" disabled={isLoading}></button>
            </div>
            <div class="modal-body">
                {#if error}
                    <div class="alert alert-danger" role="alert">{error}</div>
                {/if}
                <form on:submit|preventDefault={handleSubmit}>
                    <div class="mb-3">
                        <label for="employeeName" class="form-label">Ф.И.О.</label>
                        <input type="text" class="form-control" id="employeeName" bind:value={formData.name} required disabled={isLoading}>
                    </div>
                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label for="dateOfBirth" class="form-label">Дата рождения</label>
                            <input type="date" class="form-control" id="dateOfBirth" bind:value={formData.dateOfBirth} required disabled={isLoading}>
                        </div>
                        <div class="col-md-6 mb-3">
                            <label for="employmentDate" class="form-label">Дата устройства на работу</label>
                            <input type="date" class="form-control" id="employmentDate" bind:value={formData.employmentDate} required disabled={isLoading}>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label for="departmentId" class="form-label">Отдел</label>
                            <select class="form-select" id="departmentId" bind:value={formData.departmentId} required disabled={isLoading}>
                                {#if departments.length === 0}
                                    <option value="" disabled selected>Загрузка отделов...</option>
                                {/if}
                                {#each departments as dept (dept.id)}
                                    <option value={dept.id}>{dept.name}</option>
                                {/each}
                            </select>
                        </div>
                        <div class="col-md-6 mb-3">
                            <label for="positionId" class="form-label">Должность</label>
                            <select class="form-select" id="positionId" bind:value={formData.positionId} on:change={handlePositionChange} required disabled={isLoading}>
                                {#if positions.length === 0}
                                    <option value="" disabled selected>Загрузка должностей...</option>
                                {/if}
                                {#each positions as pos (pos.id)}
                                    <option value={pos.id}>{pos.name}</option>
                                {/each}
                            </select>
                        </div>
                    </div>
                    <div class="row align-items-end">
                        <div class="col-md-4 mb-3">
                            <label for="baseSalary" class="form-label">Базовая ставка (MDL)</label>
                            <input type="number" class="form-control" id="baseSalary" value={baseSalaryDisplay} readonly disabled>
                        </div>
                        <div class="col-md-4 mb-3">
                            <label for="salaryAdjustment" class="form-label">Надбавка (MDL)</label>
                            <input type="number" class="form-control" id="salaryAdjustment" bind:value={formData.salaryAdjustment} min="-3000" max="90000" step="1" on:input={handleSalaryAdjustmentChange} disabled={isLoading}>
                        </div>
                        <div class="col-md-4 mb-3">
                            <p class="form-control-plaintext"><strong>Итоговая зарплата: {new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'MDL', minimumFractionDigits: 0 }).format(totalCalculatedSalary)}</strong></p>
                        </div>
                    </div>

                    <div class="modal-footer mt-3">                        
                        <button type="submit" class="btn btn-primary" disabled={isLoading}>
                            {#if isLoading}
                                <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                                Сохранение...
                            {:else}
                                Сохранить
                            {/if}
                        </button>
                        <button type="button" class="btn btn-secondary" on:click={closeModal} disabled={isLoading}>Отмена</button>
                    </div>
                </form>
            </div>
        </div>
    </div>
</div>
<div class="modal-backdrop fade show d-block"></div>

<style>
    .modal.d-block {
        z-index: 1055;
    }
    .modal-backdrop.d-block {
        z-index: 1050;
    }
</style>