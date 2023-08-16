<template>
  <div class="row items-center">
    <q-breadcrumbs class="text-grey">
      <template v-slot:separator>
        <q-icon size="1.5em" name="chevron_right" color="primary" />
      </template>
      <q-breadcrumbs-el label="Home" icon="home" to="/" />
      <q-breadcrumbs-el
        v-for="(item, index) in data"
        :key="index"
        :label="item.label"
        :to="item.router"
      />
    </q-breadcrumbs>
  </div>
</template>

<script>
export default {
  props: {
    data: Array,
  },
};
</script>
