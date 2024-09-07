<template>
    <div class="login-page">
      <div class="login-container">
        <form @submit.prevent="handleSubmit">
          <div class="mb-3">
            <label for="username" class="login-form">Usuário</label>
            <input
              type="text"
              class="form-control"
              id="username"
              v-model="username"
              :class="{ 'is-invalid': errors.username }"
              placeholder="Digite seu usuário"
            />
            <div class="invalid-feedback">{{ errors.username }}</div>
          </div>
  
          <div class="mb-3">
            <label for="password" class="form-label">Senha</label>
            <input
              type="password"
              class="form-control"
              id="password"
              v-model="password"
              :class="{ 'is-invalid': errors.password }"
              placeholder="Digite sua senha"
            />
            <div class="invalid-feedback">{{ errors.password }}</div>
          </div>
  
          <button type="submit" class="btn btn-primary w-100 login-button">Login</button>
        </form>
  
        <div v-if="apiError" class="alert alert-danger mt-3" role="alert">
          {{ apiError }}
        </div>
      </div>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent, reactive, ref } from 'vue';
  import { useRouter } from 'vue-router';
  import { AuthService } from '../services/auth';
  import { login } from '../services/api/user/userService';
  import './LoginForm.scss'; 
  
  export default defineComponent({
    name: 'LoginForm',
    setup() {
      const username = ref('');
      const password = ref('');
      const errors = reactive({
        username: '',
        password: '',
      });
      const apiError = ref('');
      const router = useRouter();
  
      const validateForm = () => {
        errors.username = '';
        errors.password = '';
  
        if (!username.value || username.value.length < 5) {
          errors.username = 'Usuário deve ter pelo menos 6 caracteres';
        }
  
        if (!password.value) {
          errors.password = 'Senha é obrigatório';
        }
  
        return !errors.username && !errors.password;
      };
  
      const handleSubmit = async () => {
        if (validateForm()) {
          try {
            const response = await login({
              username: username.value,
              password: password.value,
            });
            AuthService.login(response.data.token);
            router.push('/');
          } catch (error) {
            apiError.value = 'Credenciais inválidas';
            console.log(error);
          }
        }
      };
  
      return {
        username,
        password,
        errors,
        apiError,
        handleSubmit,
      };
    },
  });
  </script>
  