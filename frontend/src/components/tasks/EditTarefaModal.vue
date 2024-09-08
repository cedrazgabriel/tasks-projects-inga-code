<template>
    <div class="modal fade show" style="display: block;" tabindex="-1" aria-labelledby="editProjectModalLabel" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="editProjectModalLabel">Editar Tarefa</h5>
            <button type="button" class="btn-close" @click="$emit('close')"></button>
          </div>
          <div class="modal-body">
            <div v-if="isLoading">
              Carregando dados da tarefa...
            </div>
            <form v-else @submit.prevent="submitForm">
          
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
                  <input type="text" id="projectCreated" class="form-control" :value="formatDate(task.createdAt)" disabled />
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
                  <input type="text" id="projectUpdated" class="form-control" :value="formatDate(task.updatedAt)" disabled />
                </div>
                <button type="submit" class="btn btn-primary">Salvar</button>
              </div>
            </form>
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
  
  export default defineComponent({
    name: 'EditProjetoModal',
    props: {
      taskId: {
        type: String,
        required: true,
      },
    },
    setup(props, { emit }) {
      const task = ref<Task | null>(null);
      const isLoading = ref(true);
      const projects = ref<Project[]>([]); 
      const selectedProjectId = ref('');
  
      const fetchProjects = async () => {
        try {
          const response = await getProjects(1, 100); // Buscando até 100 projetos na página 1
          projects.value = response.data.items;
        } catch (error) {
          console.error('Erro ao buscar projetos:', error);
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
        } finally {
          isLoading.value = false;
        }
      };
  
      const submitForm = async () => {
        if (task.value) {
          try {
            await updateTask(task.value.id, {name: task.value.name, description: task.value.description, projectId: selectedProjectId.value}); 
            emit('close');
          } catch (error) {
            console.error('Erro ao atualizar projeto:', error);
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
      };
    },
  });
  </script>
  
  <style scoped>
  .modal {
    background: rgba(0, 0, 0, 0.5);
  }
  </style>
  