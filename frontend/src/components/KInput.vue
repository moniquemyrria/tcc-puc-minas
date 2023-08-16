<template>
  <q-input
    class="k-input-pa"
    stack-label
    :model-value="modelValue"
    @update:model-value="(value) => emit('update:modelValue', value)"
    
  />
</template>

<script setup>
defineProps(['modelValue'])
const emit = defineEmits(['update:modelValue']);
</script>
