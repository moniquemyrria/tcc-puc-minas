<template>
  <q-dialog v-model="dialog" seamless position="bottom">
    <q-card style="width: 450px">
      <q-card-section
        class="row items-center no-wrap"
        :class="
          type == 'E'
            ? 'bg-red-9 text-white'
            : type == 'W'
            ? 'bg-yellow-9 text-black'
            : 'bg-green-8 text-white'
        "
      >
        <div>
          <div v-if="title != ''" class="text-weight-bold">{{ title }}</div>
          <div :class="type == 'W' ? 'text-black' : text-white">{{ text }}</div>
        </div>

        <q-space />

        <q-btn  @click="kNotifyClose" title="Fechar" flat round icon="close" v-close-popup />
      </q-card-section>

    </q-card>
  </q-dialog>
</template>

<script>
export default {
  props: {
    title: String,
    text: String,
    dialog: Boolean,
    type: {
      type: String,
      default: "I", //I - INFO /  W - WARNING / E - ERROR
    },
  },
  methods: {
    kNotifyClose() {
      this.$emit("kNotifyClose");
    },
  },
};
</script>
