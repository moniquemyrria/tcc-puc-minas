<template>
  <div>
    <KDialog
      :title="dialogMessageTitle"
      :dialog="dialogMessage"
      :text="dialogMessageText"
      @kDialogClose="closeDialogMessage"
    />

    <KDialogQuestion
      :title="dialogMessageTitle"
      :dialog="dialogQuestion"
      :text="dialogMessageText"
      @kDialogQuestionYes="callActiveInactiveYes"
      @kDialogQuestionNo="callActiveInactiveNo"
    ></KDialogQuestion>

    <KLoading :title="'Carregando Dados... Aguarde!'" :dialog="dialogLoading" />

    <KNotify :dialog="dialogNotify" :text="'Teste'" :type="'W'" />

    <KBreadCrumbs :data="dataBreadCrumbs" />
    <br />

    <div class="row">
      <div
        class="col-12 col-sm-7 col-md-7 col-lg-7 col-xl-7 col-xxl-7"
        style="text-align: start"
      >
        <h5 class="title" style="margin: 4px 0px">Fornecedores</h5>
        <div class="col-12 sub-title" style="text-align: start">
          Gerencie seus Registros de fornecedores
        </div>
      </div>

      <div
        class="col-12 col-sm-5 col-md-5 col-lg-5 col-xl-5 col-xxl-5"
        style="text-align: start"
      >
        <div class="row">
          <div class="col-12 col-sm-9 col-md-9 col-lg-9 col-xl-9 col-xxl-9">
            <q-input
              dense
              outlined
              label="Pesquisar"
              style="width: 102%"
              v-model="filter"
            >
              <template v-slot:append>
                <q-icon name="search" />
              </template>
            </q-input>
          </div>

          <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3 col-xxl-3">
            <KButton
              style="float: right"
              text="Novo"
              @click="showNewItem"
              title="Novo Registro"
            ></KButton>
          </div>
        </div>
      </div>
    </div>

    <br />
    <br />

    <div class="row">
      <div class="col-12">
        <q-table
          :rows="items"
          :columns="headers"
          :filter="filter"
          :rows-per-page-options="[10]"
        >
          <template v-slot:body="props">
            <q-tr :props="props">
              <q-td v-for="col in props.cols" :key="col.name" :props="props">
                <template v-if="col.name == 'actions'">
                  <KButtonAction
                    title="Editar Registro"
                    @click="onEdit(props.row)"
                    :icon="'edit'"
                  />
                  <KButtonAction
                    :title="
                      props.row.isActive
                        ? 'Desativar registro'
                        : 'Ativar registro'
                    "
                    :icon="props.row.isActive ? 'block' : 'refresh'"
                    @click="showActiveInactiveTypePerson(props.row)"
                  />
                </template>
                <template v-else-if="col.name == 'color'">
                  <q-avatar
                    size="sm"
                    :style="'background-color: ' + col.value"
                  />
                </template>
                <template v-else-if="col.name == 'isActive'">
                  <q-chip
                    square
                    style="min-width: 100px"
                    :style="
                      col.value
                        ? 'background-color: #DDF4D9; color: #436B3D'
                        : 'background-color: #E6E6E6; color: #777779'
                    "
                  >
                    <div class="col-12" align="center">
                      <span style="font-weight: bold">{{
                        col.value ? "Ativo" : "Inativo"
                      }}</span>
                    </div>
                  </q-chip>
                </template>
                <template v-else>
                  {{ col.value }}
                </template>
              </q-td>
            </q-tr>
          </template>
        </q-table>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from "vue";
import { useRouter, useRoute } from "vue-router";
import Api from "@/core/api/api";

import KDialog from "./../../../components/KDialog.vue";
import KLoading from "./../../../components/KLoading.vue";
import KNotify from "../../../components/KNotify.vue";
import KBreadCrumbs from "./../../../components/KBreadCrumbs.vue";

import routes from "@/modules/router";
import KButtonAction from "../../../components/KButtonAction.vue";
import KButton from "../../../components/KButton.vue";
import { IResult } from "@/core/types/IKResult";
import KDialogQuestion from "@/components/KDialogQuestion.vue";
import { storeUser } from "@/core/store/userStore";
import { ITypePerson } from "@/pages/Administration/TypePerson/Type/ITypePerson";

const router = useRouter();
const userStore = storeUser();
const userLogged = userStore.getUser.user


const dialogQuestion = ref(false);
const dialogMessage = ref(false);
const dialogMessageText = ref("");
const dialogMessageTitle = ref("");
const dialogLoading = ref(false);
const dialogNotify = ref(false);

const items = ref<Array<ITypePerson>>([]);

const typePerson = ref<ITypePerson>({} as ITypePerson);

const dataBreadCrumbs = ref([
  {
    label: "Lista de Registros",
    router: "/listTypePerson",
  },
  {
    label: "Novo Registro",
    router: "/formTypePerson",
  },
]);

const filter = ref("");

const headers = ref([
  {
    name: "isActive",
    align: "left",
    label: "Status",
    field: "isActive",
    sortable: false,
  },


  {
    name: "name",
    align: "left",
    label: "Nome",
    field: "name",
    sortable: true,
  },

  {
    name: "typePerson",
    align: "left",
    label: "Tipo",
    field: "typePerson",
    sortable: false,
  },

  {
    name: "actions",
    align: "center",
    label: "Ações",
    field: "actions",
    sortable: false,
  },
]);

function closeDialogMessage() {
  dialogMessage.value = false;
}

function showActiveInactiveTypePerson(item: ITypePerson) {
  typePerson.value = item;

  dialogMessageTitle.value = item.isActive
    ? "Desativar Registro"
    : "Ativar Registro";
  dialogMessageText.value = item.isActive
    ? "Deseja desativar o Registro selecionado?"
    : "Deseja ativar o Registro selecionado?";

  dialogQuestion.value = true;
}

function callActiveInactiveYes() {
  //console.log('teste')
  onActiveInactiveTypePerson(typePerson.value);
  dialogQuestion.value = false;
  dialogMessage.value = false;
}

function callActiveInactiveNo() {
  dialogQuestion.value = false;
  typePerson.value = {} as ITypePerson;
}

async function showItemsTypePerson() {
  dialogLoading.value = true;
  let result: IResult;
  result = await Api.getResult<IResult>("/TypePerson/" + userLogged.id);

  if (result.data.sucesso) {
    items.value = result.data.tRetorno;
    dialogLoading.value = false;
  }
}

async function onActiveInactiveTypePerson(
  typePerson: ITypePerson
) {
  dialogLoading.value = true;
  typePerson.dateUpdate = new Date();
  typePerson.isActive = !typePerson.isActive;
  let result = await Api.deleteResult<IResult>(
    "/TypePerson",
    typePerson.id
  );

  dialogLoading.value = false;
  dialogMessage.value = true;
  dialogMessageTitle.value = result.data.sucesso ? "Informação" : "Erro";
  dialogMessageText.value =
    result.data.message != null ? result.data.message : "";
}

function showNewItem() {
  router.push("/formTypePerson");
}

function onEdit(item: any) {
  //console.log(item);
  router.push("/editTypePerson/" + item.id);
}

onMounted(() => {
  showItemsTypePerson();
});
</script>

<style scoped>
.title {
  /* UI Properties */
  text-align: left;
  font: normal normal bold 30px/35px Helvetica;
  letter-spacing: 0px;
  color: #000000;
  opacity: 1;
}

.sub-title {
  /* UI Properties */
  text-align: left;
  font: normal normal normal 14px/16px Helvetica;
  letter-spacing: 0px;
  color: #707070;
  opacity: 1;
}
</style>
