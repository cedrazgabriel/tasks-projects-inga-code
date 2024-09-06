<script setup lang="ts">
import Button from './ui/button/Button.vue';

defineProps<{ msg: string }>()


</script>

<template>
<div class="bg-red">
  <Button>
    click me
  </Button>
</div>
</template>

<style scoped>
.read-the-docs {
  color: #888;
}
</style>
