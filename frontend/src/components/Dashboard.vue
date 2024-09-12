<template>
  <div class="container mt-4">
    <div class="row">
      <!-- Card para Tempo Gasto Hoje -->
      <div class="col-md-4">
        <div class="card text-center">
          <div class="card-header bg-primary text-white">
            <h5>Tempo Gasto Hoje</h5>
          </div>
          <div class="card-body">
            <p class="card-text">{{ metrics?.totalHoursSpentToday || 'Carregando...' }}</p>
          </div>
        </div>
      </div>

      <!-- Card para Tempo Gasto na Semana -->
      <div class="col-md-4">
        <div class="card text-center">
          <div class="card-header bg-success text-white">
            <h5>Tempo Gasto na Semana</h5>
          </div>
          <div class="card-body">
            <p class="card-text">{{ metrics?.totalHoursSpentThisWeek || 'Carregando...' }}</p>
          </div>
        </div>
      </div>

      <!-- Card para Tempo Gasto no Mês -->
      <div class="col-md-4">
        <div class="card text-center">
          <div class="card-header bg-info text-white">
            <h5>Tempo Gasto no Mês</h5>
          </div>
          <div class="card-body">
            <p class="card-text">{{ metrics?.totalHoursSpentThisMonth || 'Carregando...' }}</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import { getMetrics } from '../services/api/time-trackers/time-trackers-service';
import { MetricsResponse } from '../services/api/time-trackers/types';

export default defineComponent({
  name: 'Dashboard',
  setup() {
    const metrics = ref<MetricsResponse | null>(null);

    onMounted(async () => {
      try {
        const response = await getMetrics();
        metrics.value = response.data;
      } catch (error) {
        console.error('Erro ao carregar os dados das métricas:', error);
      }
    });

    return {
      metrics,
    };
  },
});
</script>
