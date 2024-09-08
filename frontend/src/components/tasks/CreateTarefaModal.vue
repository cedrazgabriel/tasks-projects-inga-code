<template>
    <div class="modal fade show" style="display: block;" tabindex="-1" aria-labelledby="createTaskModalLabel" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="createTaskModalLabel">Criar Nova Tarefa</h5>
            <button type="button" class="btn-close" @click="$emit('close')"></button>
          </div>
          <div class="modal-body">
            <form @submit.prevent="submitForm">
              <div class="mb-3">
                <label for="taskName" class="form-label">Nome da tarefa</label>
                <input type="text" id="taskName" class="form-control" v-model="taskName" required />
              </div>
              <div class="mb-3">
                <label for="taskDescription" class="form-label">Descrição</label>
                <input type="text" id="taskDescription" class="form-control" v-model="taskDescription" required />
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
              <button type="submit" class="btn btn-primary">Criar</button>
            </form>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent, ref, onMounted } from 'vue';
  import { getProjects } from '../../services/api/projects/projectService';
  import { Project } from '../../services/api/projects/types'; // Certifique-se de que o caminho esteja correto

  
  export default defineComponent({
    name: 'CreateTarefaModal',
    setup(_, { emit }) {
      const taskName = ref('');
      const taskDescription = ref('');
      const selectedProjectId = ref('');
      const projects = ref<Project[]>([]); 
  
      const submitForm = () => {
        if (taskName.value && selectedProjectId.value) {
            
          const taskData = {
            name: taskName.value,
            description: taskDescription.value,
            projectId: selectedProjectId.value,
          };
          emit('create', taskData); // Emite a criação da tarefa com o projeto selecionado
          emit('close');
        }
      };
  
      const fetchProjects = async () => {
        try {
          const response = await getProjects(1, 100); // Buscando até 100 projetos na página 1
          projects.value = response.data.items;
        } catch (error) {
          console.error('Erro ao buscar projetos:', error);
        }
      };
  
      onMounted(() => {
        fetchProjects(); // Busca os projetos quando o componente é montado
      });
  
      return {
        taskName,
        taskDescription,
        selectedProjectId,
        projects,
        submitForm,
      };
    },
  });
  </script>
  
  <style scoped>
  .modal {
    background: rgba(0, 0, 0, 0.5);
  }
  </style>
  