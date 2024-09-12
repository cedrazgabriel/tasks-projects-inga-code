<template>
  <div class="card">
    <div class="card-header d-flex justify-content-between align-items-center">
      <h4>Projetos</h4>
      <button class="btn btn-primary" @click="openCreateModal">Novo</button>
    </div>
    <div class="card-body">
      <div class="loader-wrapper" v-if="isLoading">
        <RingLoader :color="'#3498db'" />
      </div>
      <div v-else>
        <div class="table-responsive">
          <table class="table table-striped" v-if="projects.length > 0">
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
                <td>{{ projeto.updatedAt ? formatDate(projeto.updatedAt) : "" }}</td>
                <td>
                  <PhPencil class="me-2" @click="openEditModal(projeto)" size="24" color="#FFC107" />
                  <PhTrash class="me-2" @click="confirmDelete(projeto)" size="24" color="#DC3545" />
                </td>
              </tr>
            </tbody>
          </table>
          <p v-if="projects.length === 0" class="text-center">Nenhum projeto encontrado.</p>
        </div>
        <nav v-if="projects.length !== 0">
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
      </div>

      <EditProjetoModal v-if="showEditModal && selectedProject" :project-id="selectedProject.id" @close="closeEditModal" />
      
      <CreateProjetoModal v-if="showCreateModal" @close="closeCreateModal" @create="createProjectHandler" />

      <div v-if="showDeleteModal" class="modal fade show" style="display: block;" tabindex="-1" aria-labelledby="deleteModalLabel" aria-hidden="true">
        <div class="modal-dialog">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title" id="deleteModalLabel">Confirmação de Exclusão</h5>
              <button type="button" class="btn-close" @click="cancelDelete"></button>
            </div>
            <div class="modal-body">
              <p>Tem certeza que deseja deletar o projeto <strong>{{ projectToDelete?.name }}</strong>? Todas as tasks associadas ao projeto serão deletadas.</p>
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" @click="cancelDelete">Não</button>
              <button type="button" class="btn btn-danger" @click="deleteProjectConfirmed">Sim</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted, computed } from 'vue';
import { Project } from '../../services/api/projects/types';
import EditProjetoModal from './EditProjetoModal.vue';
import CreateProjetoModal from './CreateProjetoModal.vue';
import {RingLoader} from  'vue3-spinner';
import {PhPencil, PhTrash} from '@phosphor-icons/vue'
import { createProject, deleteProject, getProjects } from '../../services/api/projects/projectService';
import { useToast } from 'vue-toastification';


export default defineComponent({
  name: 'ProjetosView',
  components: {
    EditProjetoModal, 
    CreateProjetoModal,
    RingLoader,
    PhPencil,
    PhTrash 
  },
  setup() {
    const projects = ref<Project[]>([]); 
    const currentPage = ref(1); 
    const pageSize = 5; 
    const totalRecords = ref(0); 
    const totalPages = computed(() => Math.ceil(totalRecords.value / pageSize)); 
    const isLoading = ref(false); 
    const showEditModal = ref(false); 
    const selectedProject = ref<Project | null>(null); 
    const showDeleteModal = ref(false); 
    const projectToDelete = ref<Project | null>(null); 
    const showCreateModal = ref(false);

    const toast = useToast();
   
    const fetchProjects = async (page: number) => {
      isLoading.value = true;
      try {
        const response = await getProjects(page, pageSize);
        projects.value = response.data.items;
        totalRecords.value = response.data.totalRecords;
      } catch (error) {
        toast.error(error);
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
        fetchProjects(page); 
      }
    };
    const openEditModal = (project: Project) => {
      selectedProject.value = project;
      showEditModal.value = true;
    };

    const closeEditModal = () => {
      showEditModal.value = false;
      selectedProject.value = null; 
      fetchProjects(currentPage.value);
    };


    const confirmDelete = (project: Project) => {
      projectToDelete.value = project;
      showDeleteModal.value = true;
    };

 
    const cancelDelete = () => {
      projectToDelete.value = null;
      showDeleteModal.value = false;
    };

  
    const deleteProjectConfirmed = async () => {
      if (projectToDelete.value) {
        isLoading.value = true; 
        try {
          await deleteProject(projectToDelete.value.id); 
          fetchProjects(currentPage.value); 
        } catch (error) {
          toast.error(error);
        } finally {
          isLoading.value = false; 
          showDeleteModal.value = false; 
          projectToDelete.value = null;
          toast.success('Projeto deletado com sucesso!');
        }
      }
    };

    const openCreateModal = () => {
      showCreateModal.value = true;
    };

    const closeCreateModal = () => {
      showCreateModal.value = false;
    };
  
    onMounted(() => {
      fetchProjects(currentPage.value);
    });

    const createProjectHandler = async (name: string) => {
      try {
        isLoading.value = true;
        await createProject({ name });
        await fetchProjects(currentPage.value);
     
      } catch (error) {
        toast.error(error);
      } finally {
        isLoading.value = false;
        toast.success('Projeto criado com sucesso!');
      }
    };

    return {
      projects,
      currentPage,
      totalPages,
      changePage,
      formatDate,
      openEditModal,
      closeEditModal,
      confirmDelete,
      cancelDelete,
      deleteProjectConfirmed,
      deleteProject,
      showEditModal,
      selectedProject,
      showDeleteModal,
      projectToDelete,
      isLoading,
      showCreateModal,
      openCreateModal,
      closeCreateModal,
      createProjectHandler,
    };
  },
});
</script>

<style scoped>
.loader-wrapper {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 300px; /* Ajuste conforme necessário */
}

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
</style>