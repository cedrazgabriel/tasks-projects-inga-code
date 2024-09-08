import { createApp } from 'vue'
import App from './App.vue'
import router from './router';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap';
import './assets/global.scss';
import Toast, { POSITION } from "vue-toastification";
import "vue-toastification/dist/index.css";



const app = createApp(App)

const options = {
    position: POSITION.TOP_RIGHT,
    timeout: 3000,
    closeOnClick: true,
    pauseOnHover: true
};

app.use(router)
app.use(Toast, options);
app.mount('#app');
