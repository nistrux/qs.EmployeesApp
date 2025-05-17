<script lang="ts">
    import { createEventDispatcher } from 'svelte';

    export let isLoading: boolean = false;

    const dispatch = createEventDispatcher();

    export let countInput: number = 10;
    let error: string | null = null;

    function validateAndConfirm() {
        if (countInput === null || countInput === undefined || countInput <= 0) {
            error = "Количество должно быть целым числом больше 0";
            return;
        }
        error = null;
        dispatch('confirm', { count: countInput });
    }

    function closeModal() {
        dispatch('close');
    }
</script>

<div class="modal fade show d-block" tabindex="-1" role="dialog" aria-labelledby="seedDataModalLabel" aria-modal="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="seedDataModalLabel">Заполнить БД тестовыми данными</h5>
                <button type="button" class="btn-close" on:click={closeModal} aria-label="Close" disabled={isLoading}></button>
            </div>
            <div class="modal-body">
                {#if error}
                    <div class="alert alert-warning" role="alert">{error}</div>
                {/if}
                <form on:submit|preventDefault={validateAndConfirm}>
                    <div class="mb-3">
                        <label for="seedCount" class="form-label">Количество создаваемых сотрудников:</label>
                        <input
                                type="number"
                                class="form-control"
                                id="seedCount"
                                bind:value={countInput}
                                min="1"
                                step="1"
                                required
                                disabled={isLoading}
                        >
                    </div>
                    <p class="form-text">Это действие добавит указанное количество случайно сгенерированных сотрудников а так-же должности и департаменты в базу данных.</p>
                </form>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" on:click={validateAndConfirm} disabled={isLoading || countInput <= 0}>
                    {#if isLoading}
                        <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                        Создание...
                    {:else}
                        <i class="fas fa-database"></i> Создать
                    {/if}
                </button>
                <button type="button" class="btn btn-secondary" on:click={closeModal} disabled={isLoading}>Отмена</button>
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
    .alert-warning {
        color: #664d03;
        background-color: #fff3cd;
        border-color: #ffecb5;
    }
</style>