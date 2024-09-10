<template>
    <div>
        <div class="filters">
            <label for="collaboratorFilter" class="form-label">Filtrar por colaborador</label>
            <select id="collaboratorFilter" class="form-control" v-model="selectedCollaboratorId">
                <option value="">Todos</option>
                <option v-for="collaborator in collaborators" :key="collaborator.id" :value="collaborator.id">
                    {{ collaborator.name }}
                </option>
            </select>
        </div>

        <table class="table mt-3">
            <thead>
                <tr>
                    <th>Colaborador</th>
                    <th>Início</th>
                    <th>Fim</th>
                    <th>Projeto</th>
                    <th>Ações</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="tracker in timeTrackers" :key="tracker.id">
                    <td>{{ tracker.collaboratorName }}</td>
                    <td>{{ formatDate(tracker.startDate) }}</td>
                    <td>{{ tracker.endDate ? formatDate(tracker.endDate) : 'Em andamento' }}</td>
                    <td>{{ tracker.projectName }}</td>
                    <td>
                        <!-- Botão para parar o rastreamento, visível apenas se o rastreamento estiver em andamento -->
                        <button v-if="!tracker.endDate" class="btn btn-danger btn-sm" @click="stopTracking(tracker.id)">
                            Parar
                        </button>
                    </td>
                </tr>
            </tbody>
        </table>

        <div class="pagination">
            <button @click="previousPage" :disabled="page === 1">Anterior</button>
            <span>Página {{ page }} de {{ totalPages }}</span>
            <button @click="nextPage" :disabled="page === totalPages">Próxima</button>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted, watch, computed } from 'vue';
import { getTimeTrackerByTaskId, stopTimeTracker } from '../../services/api/time-trackers/time-trackers-service';
import { TimeTracker } from '../../services/api/time-trackers/types';
import { Collaborator } from '../../services/api/collaborators/types';
import { getCollaborators } from '../../services/api/collaborators/collaboratorsService';
import { useToast } from 'vue-toastification';

export default defineComponent({
    name: 'TimeTrackersTable',
    props: {
        taskId: {
            type: String,
            required: true,
        },
    },
    emits: ['ongoing-tracker'],
    setup(props, { emit }) {
        const timeTrackers = ref<TimeTracker[]>([]);
        const collaborators = ref<Collaborator[]>([]);
        const selectedCollaboratorId = ref('');
        const page = ref(1);
        const pageSize = ref(10);
        const totalPages = ref(1);
        const toast = useToast();

        const hasOngoingTracker = computed(() => {
            return timeTrackers.value.some(tracker => !tracker.endDate);
        });

        watch(hasOngoingTracker, (newVal) => {
            emit('ongoing-tracker', newVal);
        });

        const fetchTimeTrackers = async () => {
            const response = await getTimeTrackerByTaskId({
                page: page.value,
                pageSize: pageSize.value,
                collaboratorId: selectedCollaboratorId.value,
            }, props.taskId);

            timeTrackers.value = response.data.items;
            totalPages.value = Math.ceil(response.data.totalRecords / pageSize.value);
        };

        const fetchCollaborators = async () => {
            const response = await getCollaborators();
            collaborators.value = response.data;
        };

        const previousPage = () => {
            if (page.value > 1) {
                page.value--;
                fetchTimeTrackers();
            }
        };

        const nextPage = () => {
            if (page.value < totalPages.value) {
                page.value++;
                fetchTimeTrackers();
            }
        };

        const stopTracking = async (trackerId: string) => {
            try {
                await stopTimeTracker(trackerId);
                toast.success('Rastreamento parado com sucesso.');
                fetchTimeTrackers();
            } catch (error) {
                toast.error('Erro ao parar o rastreamento.');
                console.error('Erro ao parar o rastreamento:', error);
            }
        };

        const formatDate = (date: string) => {
            const localDate = new Date(date);
            localDate.setHours(localDate.getHours() - 3);
            return localDate.toLocaleString();
        };


        watch(selectedCollaboratorId, () => {
            fetchTimeTrackers();
        });

        onMounted(() => {
            fetchTimeTrackers();
            fetchCollaborators();
        });

        return {
            timeTrackers,
            collaborators,
            selectedCollaboratorId,
            page,
            totalPages,
            previousPage,
            nextPage,
            formatDate,
            stopTracking,
        };
    },
});
</script>

<style scoped>
.filters {
    margin-bottom: 1rem;
}

.pagination {
    display: flex;
    justify-content: space-between;
    margin-top: 1rem;
}
</style>