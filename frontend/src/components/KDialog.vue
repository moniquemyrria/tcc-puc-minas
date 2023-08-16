<template>
  <q-dialog v-model="dialog">
    <q-card style="max-width: 500px; width: 1000px">
      <q-card-section>
        <!-- :class="
          type == 'E'
            ? 'bg-red-9 text-white'
            : type == 'W'
            ? 'bg-yellow-9 text-black'
            : 'bg-primary text-white'
        " -->
        <div class="text-h6">{{ title }}</div>
      </q-card-section>

      <q-card-section
        class="q-pt-none"
        align="left"
        style="height: 90px; position: relative; top: 30px"
      >
        {{ text }}
      </q-card-section>

      <q-card-actions align="right">
        <q-btn
          @click="kDialogClose"
          flat
          :label="textButton"
          :color="type == 'E' ? 'red-9' : type == 'W' ? 'yellow-9' : 'secondary'"
          :style="textButton == 'OK' ? 'width: 100px' : ''"
          v-close-popup
        ></q-btn>
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

<script lang="ts">
export default {
  props: {
    textButton: {
      type: String,
      default: "OK",
    },
    type: {
      type: String,
      default: "I", //I - INFO /  W - WARNING / E - ERROR
    },
    text: String,
    title: String,
    dialog: Boolean,
  },

  methods: {
    kDialogClose() {
      this.$emit("kDialogClose");
    },
  },
};
</script>