<template>
    <div class="modal fade show" style="display: block;" tabindex="-1" aria-labelledby="editProjectModalLabel" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="editProjectModalLabel">Editar Projeto</h5>
            <button type="button" class="btn-close" @click="$emit('close')"></button>
          </div>
          <div class="modal-body">
            <form @submit.prevent="submitForm">
              <div class="mb-2">
                <label for="projectName" class="form-label">Nome do Projeto</label>
                <input type="text" id="projectName" class="form-control" v-model="project.name" />
              </div>
              <div class="mb-3">
                <label for="projectCreated" class="form-label">Data de Criação</label>
                <input type="text" id="projectCreated" class="form-control" :value="formatDate(project.createdAt)" disabled />
              </div>
              <div class="mb-3">
                <label for="projectUpdated" class="form-label">Última Atualização</label>
                <input type="text" id="projectUpdated" class="form-control" :value="formatDate(project.updatedAt)" disabled />
              </div>
              <button type="submit" class="btn btn-primary">Salvar</button>
            </form>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent, PropType } from 'vue';
  import { Project } from '../services/api/projects/types';
  
  export default defineComponent({
    name: 'EditProjetoModal',
    props: {
      project: {
        type: Object as PropType<Project>,
        required: true,
      },
    },
    methods: {
      formatDate(dateString: string) {
        const date = new Date(dateString);
        return date.toLocaleDateString();
      },
      submitForm() {
        // Função para salvar o projeto (chamada à API para atualização)
        console.log('Projeto atualizado:', this.project);
        this.$emit('close');
      },
    },
  });
  </script>
  
  <style scoped>
  .modal {
    background: rgba(0, 0, 0, 0.5);
  }
  </style>
  