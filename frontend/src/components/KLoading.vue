<template>
    <q-dialog v-model="dialog" persistent>
        <q-card style="min-width: 350px">
            <q-card-section class="text-center">
                {{title}}
            </q-card-section>
            <q-linear-progress query />
        </q-card>
    </q-dialog>
</template>

<script>

export default {
  props: {
    title: String,
    dialog: Boolean
  },

  methods:{
     kLoadingClose(){
      this.$emit("kLoadingClose");
    }
  }

}
</script>
