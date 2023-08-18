<template>
  <q-card flat style="min-width: 100%">
    <q-card-section class="col bg-white full-height">
      <div class="column full-height">
         <slot></slot>
      </div>
    </q-card-section>
  </q-card>
</template>

<script>
export default {
  props: {},
};
</script>
