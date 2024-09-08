<template>
    <div class="modal fade show" style="display: block;" tabindex="-1" aria-labelledby="createProjectModalLabel" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="createProjectModalLabel">Criar Novo Projeto</h5>
            <button type="button" class="btn-close" @click="$emit('close')"></button>
          </div>
          <div class="modal-body">
            <form @submit.prevent="submitForm">
              <div class="mb-3">
                <label for="projectName" class="form-label">Nome do Projeto</label>
                <input type="text" id="projectName" class="form-control" v-model="projectName" required />
              </div>
              <button type="submit" class="btn btn-primary">Criar</button>
            </form>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent, ref } from 'vue';
  
  export default defineComponent({
    name: 'CreateProjetoModal',
    setup(_, { emit }) {
      const projectName = ref('');
  
      const submitForm = () => {
        if (projectName.value) {
          emit('create', projectName.value); // Emite o nome do projeto para o componente pai
          emit('close'); // Fecha o modal
        }
      };
  
      return {
        projectName,
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
  