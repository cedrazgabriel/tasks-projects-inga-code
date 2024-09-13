<template>
    <div>
        <div>
            <button class="btn btn-success me-2" @click="startTracking" :disabled="isTracking">Iniciar</button>
        </div>
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
                <!-- Substituímos o timeTrackers por paginatedTimeTrackers -->
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
import { defineComponent, ref, onMounted, watch } from 'vue';
import { getTimeTrackerByTaskId, initTimeTracker, stopTimeTracker } from '../../services/api/time-trackers/time-trackers-service';
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
        onTimeTrackerStoped: {
            type: Function,
            required: true,
        },
    },
    emits: ['time-tracker-started'],
    setup(props) {
        const timeTrackers = ref<TimeTracker[]>([]);
        const collaborators = ref<Collaborator[]>([]);
        const selectedCollaboratorId = ref('');
        const page = ref(1);
        const pageSize = ref(10);
        const totalPages = ref(1);
        const toast = useToast();
        const isTracking = ref(false);


        const previousPage = () => {
            if (page.value > 1) {
                page.value--;
            }
        };

        const nextPage = () => {
            if (page.value < totalPages.value) {
                page.value++;
            }
        };

        // Adiciona watch para atualizar os dados quando a página mudar
        watch([page, selectedCollaboratorId], () => {
            fetchTimeTrackers();
        });

        const stopTracking = async (trackerId: string) => {
            try {
                const request = {
                    endTime: new Date().toISOString(),
                };

                await stopTimeTracker(trackerId, request);
                toast.success('Rastreamento parado com sucesso.');
                fetchTimeTrackers();
                props.onTimeTrackerStoped();
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

        const fetchTimeTrackers = async () => {
            const requestParams = {
                page: page.value,
                pageSize: pageSize.value,
                collaboratorId: selectedCollaboratorId.value === "" ? undefined : selectedCollaboratorId.value,
            };

            const response = await getTimeTrackerByTaskId(requestParams, props.taskId);

            timeTrackers.value = response.data.items;
            totalPages.value = Math.ceil(response.data.totalRecords / pageSize.value);

            isTracking.value = timeTrackers.value.some(tracker => tracker.endDate === null);
        };

        const fetchCollaborators = async () => {
            const response = await getCollaborators();
            collaborators.value = response.data;
        };

        const startTracking = async () => {
            isTracking.value = true;

            try {
                const timeZoneId = Intl.DateTimeFormat().resolvedOptions().timeZone;
                const payload = {
                    timeZoneId,
                    taskId: props.taskId || ''
                };

                await initTimeTracker(payload);

                toast.success('Rastreamento iniciado com sucesso.');
                fetchTimeTrackers();
            } catch (error) {
                console.error('Erro ao iniciar o rastreamento de tempo:', error);
                toast.error('Erro ao iniciar o rastreamento de tempo');
            }
        };

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
            isTracking,
            startTracking,
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
