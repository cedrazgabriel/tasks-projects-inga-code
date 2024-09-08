<template>
  <div>
    <!-- Barra de navegação superior -->
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
      <div class="container-fluid">
        <a class="navbar-brand" href="#">Controle de tarefas</a>
        <button
          class="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
          aria-controls="navbarNav"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav me-auto">
            <li class="nav-item">
              <router-link class="nav-link" to="/dashboard">Dashboard</router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" to="/projetos">Projetos</router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" to="/tarefas">Tarefas</router-link>
            </li>
          </ul>
          <button class="btn btn-outline-danger d-flex align-items-center" @click="logout">
            <SignOut size="24" weight="bold" class="me-2"/> Sair
          </button>
        </div>
      </div>
    </nav>

    <div class="container my-4">
      <router-view />
    </div>
  </div>
</template>
  
  <script lang="ts">
  import { defineComponent } from 'vue';
import { AuthService } from '../services/auth';
import {PhSignOut} from '@phosphor-icons/vue'
  
  export default defineComponent({
    name: 'MainPage',
    components: {
      SignOut: PhSignOut
    },
    methods: {
      logout(){
        AuthService.logout();
        this.$router.push('/login');
      }
    }
  });
  </script>
  
  <style scoped>
  
  .container {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 70vh; 
  }
  </style>
  