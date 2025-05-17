<script lang="ts">
  import Navbar from '$components/Navbar.svelte';
  import About from '$routes/About.svelte';
  import Employees from '$routes/Employees.svelte';
  import NotFound from "$components/NotFound.svelte";
  import { departments, positions } from '$store/index';
  import { onMount } from 'svelte';
  import { getDepartments, getPositions } from '$services/apiService';
  import { Router } from 'svelte-spa-history-router';

  let appError: string | null = null;

  onMount(async () => {
    try {
      const fetchedDepartments = await getDepartments();
      departments.set(fetchedDepartments);

      const fetchedPositions = await getPositions();
      positions.set(fetchedPositions);
    } catch (error: any) {
      console.error("Failed to load initial data:", error);
      appError = `Не удалось загрузить справочники (отделы/должности): ${error.message || 'Проверьте API'}`;
    }
  });

  const routes = [
    { path: '/', component: About},
    { path: '/about', component: About},
    { path: '/employees', component: Employees},
    { path: '.*', component: NotFound},
  ];

</script>

<Navbar />
<main class="container mt-4">
  {#if appError}
    <div class="alert alert-danger" role="alert">
      <strong>Ошибка приложения:</strong> {appError}
    </div>
  {/if}

  <Router {routes} />
</main>

<style>
  main {
    padding-bottom: 2rem;
  }
</style>