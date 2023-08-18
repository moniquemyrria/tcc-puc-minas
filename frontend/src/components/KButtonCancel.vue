<template>
  <q-btn outline :style="'min-width: ' + size + 'px; background:' + color + '; color:' +  colorText" 
    >{{ text }} <slot></slot
  ></q-btn>
</template>

<script>
export default {
  props: {
    text: String,
    color: {
      color: String,
      default: '#53258E'
    },
    colorText: {
      color: String,
      default: '#53258E'
    },
    size: {
      size: Number,
      default: 130,
    }
  },
};
</script>
