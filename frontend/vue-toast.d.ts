// vue-toast.d.ts
import { Plugin } from "vue";
import { ToastInterface } from "vue-toastification";

declare module "@vue/runtime-core" {
    export interface ComponentCustomProperties {
        $toast: ToastInterface;
    }
}