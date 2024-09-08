<template>
  <div class="modal fade show" style="display: block;" tabindex="-1" aria-labelledby="editProjectModalLabel" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="editProjectModalLabel">Editar Projeto</h5>
          <button type="button" class="btn-close" @click="$emit('close')"></button>
        </div>
        <div class="modal-body">
          <div v-if="isLoading">
            Carregando dados do projeto...
          </div>
          <form v-else @submit.prevent="submitForm">
            <!-- Verificação para garantir que 'project' não seja null -->
            <div v-if="project">
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
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { Project } from '../../services/api/projects/types';
import { getProjectById, updateProject } from '../../services/api/projects/projectService';

export default defineComponent({
  name: 'EditProjetoModal',
  props: {
    projectId: {
      type: String,
      required: true,
    },
  },
  setup(props, { emit }) {
    const project = ref<Project | null>(null);
    const isLoading = ref(true);

    const fetchProject = async () => {
      isLoading.value = true;
      try {
        const response = await getProjectById(props.projectId);
        project.value = response.data; 
      } catch (error) {
        console.error('Erro ao carregar projeto:', error);
      } finally {
        isLoading.value = false;
      }
    };

    const submitForm = async () => {
      if (project.value) {
        try {
          await updateProject(project.value.id, {name: project.value.name}); 
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
      fetchProject(); 
    });

    return {
      project,
      isLoading,
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
