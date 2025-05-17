<script lang="ts">
    import { createEventDispatcher } from 'svelte';

    export let employeeName: string = '';
    export let isLoading: boolean = false;

    const dispatch = createEventDispatcher();

    function confirm() {
        dispatch('confirm');
    }

    function close() {
        dispatch('close');
    }
</script>

<div class="modal fade show d-block" tabindex="-1" role="dialog" aria-labelledby="deleteModalLabel" aria-modal="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="deleteModalLabel">Подтверждение удаления</h5>
                <button type="button" class="btn-close" on:click={close} aria-label="Close" disabled={isLoading}></button>
            </div>
            <div class="modal-body">
                <p>Вы уверены, что хотите удалить сотрудника <strong>{employeeName}</strong>?</p>
                <p>Это действие нельзя отменить.</p>
            </div>
            <div class="modal-footer">                
                <button type="button" class="btn btn-danger" on:click={confirm} disabled={isLoading}>
                    {#if isLoading}
                        <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                        Удаление...
                    {:else}
                        Удалить
                    {/if}
                </button>
                <button type="button" class="btn btn-secondary" on:click={close} disabled={isLoading}>Отмена</button>
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