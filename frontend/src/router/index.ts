import { createRouter, createWebHistory } from 'vue-router';
import LoginForm from '../components/login/LoginForm.vue';
import MainPage from '../components/MainPage.vue';
import { AuthService } from '../services/auth';
import Projetos from '../components/projects/Projetos.vue';
import Tarefas from '../components/tasks/Tarefas.vue';
import Dashboard from '../components/Dashboard.vue';

const routes = [
    {
        path: '/login',
        name: 'Login',
        component: LoginForm,
    },
    {
        path: '/',
        name: 'Main',
        component: MainPage,
        redirect: '/dashboard',
        children: [
            {
                path: 'projetos',
                name: 'Projetos',
                component: Projetos,
            },
            {
                path: 'tarefas',
                name: 'Tarefas',
                component: Tarefas,
            },
            {
                path: 'dashboard',
                name: 'Dashboard',
                component: Dashboard,
            },
        ],
        meta: { requiresAuth: true },
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

// Middleware para proteger rotas
router.beforeEach((to, _, next) => {
    const isAuthenticated = AuthService.isAuthenticated();


    if (to.matched.some((record) => record.meta.requiresAuth)) {
        if (!isAuthenticated) {

            next({ name: 'Login' });
        } else {
            next();
        }
    }

    else if (to.name === 'Login' && isAuthenticated) {
        next({ name: 'Main' });
    } else {
        next();
    }
});

export default router;
