<template>
  <q-btn
    flat
    round
    :style="'min-width: ' + size + 'px'"
    :color="color"
    :icon="icon"
    :title="title"
    size="md"
  >
    <slot></slot
  ></q-btn>
</template>

<script>
export default {
  props: {
    text: String,
    color: {
      color: String,
      default: "blach",
    },
    colorText: {
      color: String,
      default: "text-white",
    },
    icon: String,
    title: String,
  },
};
</script>
