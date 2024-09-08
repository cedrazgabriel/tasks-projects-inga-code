<template>
  <div class="card">
    <div class="card-header d-flex justify-content-between align-items-center">
      <h4>Tarefas</h4>
      <button class="btn btn-primary" @click="openCreateModal">Nova Tarefa</button>
    </div>
    <div class="card-body">
      <div class="table-responsive">
        <RingLoader v-if="isLoading" :color="'#3498db'" />
        <table class="table table-striped" v-if="!isLoading && tasks.length > 0">
          <thead>
            <tr>
              <th>Nome</th>
              <th>Descrição</th>
              <th>Projeto</th>
              <th>Data de Criação</th>
              <th>Última Atualização</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="tarefa in tasks" :key="tarefa.id">
              <td>{{ tarefa.name }}</td>
              <td>{{ tarefa.description }}</td>
              <td>{{ tarefa.projectName }}</td>
              <td>{{ formatDate(tarefa.createdAt) }}</td>
              <td>{{ tarefa.updatedAt ? formatDate(tarefa.updatedAt) : "" }}</td>
              <td>
                <button>
                  <PhPencil class="me-2" @click="openEditModal(tarefa)" size="24" color="#FFC107" />
                </button>
                <button>
                  <PhTrash class="me-2" @click="confirmDelete(tarefa)" size="24" color="#DC3545" />
                </button>
              </td>
            </tr>
          </tbody>
        </table>
        <p v-if="!isLoading && tasks.length === 0" class="text-center">Nenhuma tarefa encontrada.</p>
      </div>
      <nav v-if="!isLoading && tasks.length !== 0">
        <ul class="pagination justify-content-center">
          <li class="page-item" :class="{ disabled: currentPage === 1 }">
            <a class="page-link" href="#" @click.prevent="changePage(currentPage - 1)">Anterior</a>
          </li>
          <li class="page-item" v-for="page in totalPages" :key="page" :class="{ active: currentPage === page }">
            <a class="page-link" href="#" @click.prevent="changePage(page)">{{ page }}</a>
          </li>
          <li class="page-item" :class="{ disabled: currentPage === totalPages }">
            <a class="page-link" href="#" @click.prevent="changePage(currentPage + 1)">Próxima</a>
          </li>
        </ul>
      </nav>
      <EditTarefaModal v-if="showEditModal && selectedTask" :task-id="selectedTask.id"  @close="closeEditModal" />
 
      <CreateTarefaModal v-if="showCreateModal" @close="closeCreateModal" @create="createTaskHandler" />

      <!-- Modal de confirmação de exclusão -->
      <div v-if="showDeleteModal" class="modal fade show" style="display: block;" tabindex="-1" aria-labelledby="deleteModalLabel" aria-hidden="true">
        <div class="modal-dialog">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title" id="deleteModalLabel">Confirmação de Exclusão</h5>
              <button type="button" class="btn-close" @click="cancelDelete"></button>
            </div>
            <div class="modal-body">
              <p>Tem certeza que deseja deletar a tarefa <strong>{{ taskToDelete?.name }}</strong>? Todo o tempo rastreado será perdido.</p>
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" @click="cancelDelete">Não</button>
              <button type="button" class="btn btn-danger" @click="deleteTaskConfirmed">Sim</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted, computed } from 'vue';
import {RingLoader} from 'vue3-spinner';
import {PhPencil, PhTrash} from '@phosphor-icons/vue';
import { Task, UpdateTaskRequest } from '../../services/api/tasks/types';
import { createTask, deleteTask, getTasks } from '../../services/api/tasks/tasksService';
import CreateTarefaModal from './CreateTarefaModal.vue';
import EditTarefaModal from './EditTarefaModal.vue';

export default defineComponent({
  name: 'Tarefas',
  components: {
    RingLoader,
    PhPencil,
    PhTrash,
    CreateTarefaModal,
    EditTarefaModal
  },
  setup() {
    const tasks = ref<Task[]>([]);
    const currentPage = ref(1);
    const pageSize = 5;
    const totalRecords = ref(0);
    const totalPages = computed(() => Math.ceil(totalRecords.value / pageSize));
    const isLoading = ref(false);
    const showEditModal = ref(false);
    const selectedTask = ref<Task | null>(null);
    const showDeleteModal = ref(false);
    const taskToDelete = ref<Task | null>(null);
    const showCreateModal = ref(false);

    const fetchTasks = async (page: number) => {
      isLoading.value = true;
      try {
        const response = await getTasks(page, pageSize);
        tasks.value = response.data.items;
        totalRecords.value = response.data.totalRecords;
      } catch (error) {
        console.error('Erro ao buscar as tarefas:', error);
      } finally {
        isLoading.value = false;
      }
    };

    const formatDate = (dateString: string) => {
      const date = new Date(dateString);
      return date.toLocaleDateString();
    };

    const changePage = (page: number) => {
      if (page > 0 && page <= totalPages.value) {
        currentPage.value = page;
        fetchTasks(page);
      }
    };

    const openEditModal = (task: Task) => {
      selectedTask.value = task;
      showEditModal.value = true;
    };

    const closeEditModal = () => {
      showEditModal.value = false;
      selectedTask.value = null;
      fetchTasks(currentPage.value);
    };

    const confirmDelete = (task: Task) => {
      taskToDelete.value = task;
      showDeleteModal.value = true;
    };

    const cancelDelete = () => {
      taskToDelete.value = null;
      showDeleteModal.value = false;
    };

    const deleteTaskConfirmed = async () => {
      if (taskToDelete.value) {
        isLoading.value = true;
        try {
          await deleteTask(taskToDelete.value.id);
          fetchTasks(currentPage.value);
        } catch (error) {
          console.error('Erro ao deletar a tarefa:', error);
        } finally {
          isLoading.value = false;
          showDeleteModal.value = false;
          taskToDelete.value = null;
        }
      }
    };

    const openCreateModal = () => {
      showCreateModal.value = true;
    };

    const closeCreateModal = () => {
      showCreateModal.value = false;
    };

    const createTaskHandler = async (data: UpdateTaskRequest) => {
      try {
        isLoading.value = true; 
        await createTask(data);
        await fetchTasks(currentPage.value);
      } catch (error) {
        console.error('Erro ao criar tarefa:', error);
      } finally {
        isLoading.value = false;
      }
    };

    onMounted(() => {
      fetchTasks(currentPage.value);
    });

    return {
      tasks,
      currentPage,
      totalPages,
      changePage,
      formatDate,
      openEditModal,
      closeEditModal,
      confirmDelete,
      cancelDelete,
      deleteTaskConfirmed,
      showEditModal,
      selectedTask,
      showDeleteModal,
      taskToDelete,
      isLoading,
      showCreateModal,
      openCreateModal,
      closeCreateModal,
      createTaskHandler,
    };
  },
});
</script>

<style scoped>
.card-body {
  padding: 1rem;
  max-height: 500px;
  overflow-y: auto;
}

.table-responsive {
  max-width: 100%;
  overflow-x: auto;
}

.table {
  margin-top: 1rem;
  width: 100%;
}

.pagination {
  margin-top: 1rem;
}

.card {
  min-width: 450px;
}

button{
  border: none;
}
</style>
