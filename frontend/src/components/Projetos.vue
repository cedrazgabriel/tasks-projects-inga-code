<template>
  <div class="card">
    <div class="card-header">
      <h4>Projetos</h4>
    </div>
    <div class="card-body">
      <div class="table-responsive">
        <RingLoader v-if="isLoading" :size="100" :color="'#3498db'" />
        <table class="table table-striped" v-if="!isLoading && projects.length > 0">
          <thead>
            <tr>
              <th>Nome</th>
              <th>Data de Criação</th>
              <th>Última Atualização</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="projeto in projects" :key="projeto.id">
              <td>{{ projeto.name }}</td>
              <td>{{ formatDate(projeto.createdAt) }}</td>
              <td>{{ formatDate(projeto.updatedAt) }}</td>
              <td>
                <button class="btn btn-warning btn-sm me-2" @click="openEditModal(projeto)">Editar</button>
                <button class="btn btn-danger btn-sm" @click="deleteProject(projeto.id)">Deletar</button>
              </td>
            </tr>
          </tbody>
        </table>
        <p v-if="!isLoading && projects.length === 0" class="text-center">Nenhum projeto encontrado.</p>
      </div>
      <nav v-if="!isLoading">
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
      <EditProjetoModal v-if="showEditModal && selectedProject" :project="selectedProject" @close="closeEditModal" />
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted, computed } from 'vue';
import { Project } from '../services/api/projects/types';
import EditProjetoModal from './EditProjetoModal.vue';
import {RingLoader} from  'vue3-spinner';
import { deleteProject, getProjects } from '../services/api/projects/projectService';


export default defineComponent({
  name: 'Projetos',
  components: {
    EditProjetoModal, 
    RingLoader, 
  },
  setup() {
    const projects = ref<Project[]>([]); 
    const currentPage = ref(1); 
    const pageSize = 10; 
    const totalRecords = ref(0); 
    const totalPages = computed(() => Math.ceil(totalRecords.value / pageSize)); 
    const isLoading = ref(false); 
    const showEditModal = ref(false); 
    const selectedProject = ref<Project | null>(null); 
   
    const fetchProjects = async (page: number) => {
      isLoading.value = true;
      try {
        const response = await getProjects(page, pageSize);
        projects.value = response.data.items;
        totalRecords.value = response.data.totalRecords;
      } catch (error) {
        console.error('Erro ao buscar os projetos:', error);
      } finally {
        isLoading.value = false; 
      }
    };

    // Função para formatar as datas
    const formatDate = (dateString: string) => {
      const date = new Date(dateString);
      return date.toLocaleDateString();
    };

    // Função para mudar de página
    const changePage = (page: number) => {
      if (page > 0 && page <= totalPages.value) {
        currentPage.value = page;
        fetchProjects(page); 
      }
    };

    // Função para abrir o modal de edição
    const openEditModal = (project: Project) => {
      selectedProject.value = project;
      showEditModal.value = true;
    };

   
    const closeEditModal = () => {
      showEditModal.value = false;
      selectedProject.value = null; 
    };

 
    const deleteProjectHandler = async (id: string) => {
      isLoading.value = true; // Ativa o loading durante a exclusão
      try {
        await deleteProject(id); 
        fetchProjects(currentPage.value); 
      } catch (error) {
        console.error('Erro ao deletar o projeto:', error);
      } finally {
        isLoading.value = false; // Desativa o loading
      }
    };

  
    onMounted(() => {
      fetchProjects(currentPage.value);
    });

    return {
      projects,
      currentPage,
      totalPages,
      changePage,
      formatDate,
      openEditModal,
      closeEditModal,
      deleteProject: deleteProjectHandler,
      showEditModal,
      selectedProject,
      isLoading,
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
</style>
