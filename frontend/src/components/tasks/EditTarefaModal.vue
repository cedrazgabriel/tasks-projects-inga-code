<template>
  <div class="modal fade show" style="display: block;" tabindex="-1" aria-labelledby="editProjectModalLabel"
    aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="editProjectModalLabel">
            <!-- Mostrar "Carregando..." enquanto as informações da tarefa estão sendo carregadas -->
            {{ isLoading ? 'Carregando...' : `Tarefa - ${task?.name || ''}` }}
          </h5>
          <button type="button" class="btn-close" @click="$emit('close')"></button>
        </div>
        <div class="modal-body">
          <div v-if="isLoading">
            Carregando dados da tarefa...
          </div>
          <div v-else>
            <ul class="nav nav-tabs" role="tablist">
              <li class="nav-item">
                <a class="nav-link" :class="{ active: activeTab === 'edit' }" @click="activeTab = 'edit'"
                  role="tab">Editar Tarefa</a>
              </li>
              <li class="nav-item">
                <a class="nav-link" :class="{ active: activeTab === 'track' }" @click="activeTab = 'track'"
                  role="tab">Tempo Rastreado</a>
              </li>
            </ul>

            <div class="tab-content mt-3">
              <!-- Aba de edição -->
              <div v-if="activeTab === 'edit'">
                <form @submit.prevent="submitForm">
                  <div v-if="task">
                    <div class="mb-2">
                      <label for="taskName" class="form-label">Nome da tarefa</label>
                      <input type="text" id="taskName" class="form-control" v-model="task.name" />
                    </div>
                    <div class="mb-2">
                      <label for="taskDescription" class="form-label">Descrição</label>
                      <input type="text" id="taskDescription" class="form-control" v-model="task.description" />
                    </div>
                    <div class="mb-3">
                      <label for="projectCreated" class="form-label">Data de Criação</label>
                      <input type="text" id="projectCreated" class="form-control" :value="formatDate(task.createdAt)"
                        disabled />
                    </div>
                    <div class="mb-3">
                      <label for="projectSelect" class="form-label">Projeto</label>
                      <select id="projectSelect" class="form-control" v-model="selectedProjectId" required>
                        <option disabled value="">Selecione um projeto</option>
                        <option v-for="project in projects" :key="project.id" :value="project.id">
                          {{ project.name }}
                        </option>
                      </select>
                    </div>
                    <div class="mb-3">
                      <label for="projectUpdated" class="form-label">Última Atualização</label>
                      <input type="text" id="projectUpdated" class="form-control"
                        :value="task.updatedAt ? formatDate(task.updatedAt) : ''" disabled />
                    </div>
                    <button type="submit" class="btn btn-primary">Salvar</button>
                  </div>
                </form>
              </div>

              <!-- Aba de rastreamento de tempo -->
              <div v-if="activeTab === 'track'">
                <div class="mb-2">
                  <p>Tempo total da tarefa:</p>
                  <p>{{ task?.totalTimeSpent }}</p>
                </div>
                <div v-if="activeTab === 'track' && task">
                  <TimeTrackersTable :taskId="task.id" :onTimeTrackerStoped="fetchTask"/>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>



<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { getTaskById, updateTask } from '../../services/api/tasks/tasksService';
import { Task } from '../../services/api/tasks/types';
import { Project } from '../../services/api/projects/types';
import { getProjects } from '../../services/api/projects/projectService';
import { useToast } from 'vue-toastification';
import TimeTrackersTable from './TimeTrackersTable.vue';

export default defineComponent({
  name: 'EditProjetoModal',
  components: {
    TimeTrackersTable,
  },
  props: {
    taskId: {
      type: String,
      required: true,
    }
  },
  setup(props, { emit }) {
    const task = ref<Task | null>(null);
    const isLoading = ref(true);
    const projects = ref<Project[]>([]);
    const selectedProjectId = ref('');
    const toast = useToast();

    const activeTab = ref('edit');
    
    const trackedTime = ref('00:00:00');
    const hasOngoingTracker = ref(false);


    const onOngoingTracker = (value: boolean) => {
      hasOngoingTracker.value = value;
    };

    const fetchProjects = async () => {
      try {
        const response = await getProjects(1, 100);
        projects.value = response.data.items;
      } catch (error) {
        console.error('Erro ao buscar projetos:', error);
        toast.error('Erro ao buscar projetos');
      }
    };

    const fetchTask = async () => {
      isLoading.value = true;
      try {
        const response = await getTaskById(props.taskId);
        task.value = response.data;

        selectedProjectId.value = task.value.projectId;
      } catch (error) {
        console.error('Erro ao carregar tarefa:', error);
        toast.error('Erro ao carregar tarefa');
      } finally {
        isLoading.value = false;
      }
    };

    const submitForm = async () => {
      if (task.value) {
        try {
          await updateTask(task.value.id, { name: task.value.name, description: task.value.description, projectId: selectedProjectId.value });
          emit('close');
          toast.success('Tarefa atualizada com sucesso');
        } catch (error) {
          console.error('Erro ao atualizar projeto:', error);
          toast.error('Erro ao atualizar projeto');
        }
      }
    };

    const formatDate = (dateString: string) => {
      const date = new Date(dateString);
      return date.toLocaleDateString();
    };

    

   
    onMounted(() => {
      fetchTask();
      fetchProjects();
    });

    return {
      task,
      projects,
      isLoading,
      selectedProjectId,
      submitForm,
      formatDate,
      activeTab,
      trackedTime,
      hasOngoingTracker,
      onOngoingTracker,
      fetchTask,
    };
  },
});
</script>


<style scoped>
.modal {
  background: rgba(0, 0, 0, 0.5);
}

.modal-dialog {
  max-width: 600px;
  /* Defina a largura máxima */
  width: 100%;
}

.modal-content {
  max-height: 80vh;
  /* Defina uma altura máxima relativa à altura da tela */
  overflow-y: auto;
  /* Adicione rolagem vertical quando necessário */
}
</style>