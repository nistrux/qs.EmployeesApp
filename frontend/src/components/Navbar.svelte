<script lang="ts">
    import { link } from 'svelte-spa-history-router';
</script>

<nav class="navbar navbar-expand-lg navbar-dark bg-dark">
    <div class="container-fluid">
        <a class="navbar-brand" href="/" use:link>EmployeesApp</a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav">
                <li class="nav-item">
                    <a class="nav-link" aria-current="page" href="/about" use:link >О компании </a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" aria-current="page" href="/employees" use:link > Сотрудники </a>
                </li>
            </ul>
        </div>
    </div>
</nav>

<style>
    /*.nav-link.active {
        font-weight: bold;
        !* color: #fff; *!
    }*/
</style>