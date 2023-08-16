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
        <h5 class="title" style="margin: 4px 0px">Cadastro de Usuários</h5>
        <div class="col-12 sub-title" style="text-align: start">
          Gerencie os dados e acessos dos usuários
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
              :text="'Novo'"
              @click="showNewUser"
              :title="'Novo Nível'"
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
                    :title="'Editar Usuário'"
                    @click="onEdit(props.row)"
                    :icon="'edit'"
                  />
                  <KButtonAction
                    :title="
                      props.row.isActive
                        ? 'Bloquear Usuário'
                        : 'Desbloquear Usuário'
                    "
                    :icon="props.row.isActive ? 'block' : 'refresh'"
                    @click="showActiveInactive(props.row)"
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
                        col.value ? "Ativo" : "Bloqueado"
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
import { IUsers } from "@/core/types/IUsers";

const router = useRouter();

const dialogQuestion = ref(false);
const dialogMessage = ref(false);
const dialogMessageText = ref("");
const dialogMessageTitle = ref("");
const dialogLoading = ref(false);
const dialogNotify = ref(false);

const items = ref<Array<IUsers>>([]);

const user = ref<IUsers>({} as IUsers);

const dataBreadCrumbs = ref([
  {
    label: "Lista Usuários",
    router: "/listUsers",
  },
  {
    label: "Novo Usuário",
    router: "/formUsers",
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
  // {
  //   name: "id",
  //   align: "left",
  //   label: "Código",
  //   field: "id",
  //   sortable: true,
  // },
  {
    name: "userName",
    align: "left",
    label: "Usuário",
    field: "userName",
    sortable: true,
  },
  {
    name: "name",
    align: "left",
    label: "Nome",
    field: "name",
    sortable: true,
  },
  {
    name: "approvalLevel",
    align: "left",
    label: "Níveis de Aprovação",
    field: "approvalLevel",
    sortable: true,
  },

  {
    name: "lastAccessDate",
    align: "left",
    label: "Último Acesso",
    field: "lastAccessDate",
    sortable: true,
  },

  {
    name: "webCode",
    align: "left",
    label: "Código Web",
    field: "webCode",
    sortable: true,
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

function showActiveInactive(userDelete: any) {
  user.value = userDelete;

  console.log(user.value);

  dialogMessageTitle.value = userDelete.isActive
    ? "Desativar Usuário"
    : "Ativar Usuário";
  dialogMessageText.value = userDelete.isActive
    ? "Deseja desativar o Usuário selecionado?"
    : "Deseja ativar o Usuário selecionado?";

  dialogQuestion.value = true;
}

function callActiveInactiveYes() {
  //console.log('teste')
  onActiveInactive(user.value);
  dialogQuestion.value = false;
  dialogMessage.value = false;
}

function callActiveInactiveNo() {
  dialogQuestion.value = false;
  user.value = {} as IUsers;
}

async function showItems() {
  dialogLoading.value = true;
  let result: IResult;
  result = await Api.getResult<IResult>("/ServiceUser");

  if (result.data.sucesso) {
    items.value = result.data.tRetorno;
    dialogLoading.value = false;
  }
}

async function onActiveInactive(user: IUsers) {
  dialogLoading.value = true;
  user.dateUpdate = new Date();
  user.isActive = !user.isActive;
  let result = await Api.deleteResult<IResult>("/ServiceUser", user.id);

  dialogLoading.value = false;
  dialogMessage.value = true;
  dialogMessageTitle.value = result.data.sucesso ? "Informação" : "Erro";
  dialogMessageText.value =
    result.data.message != null ? result.data.message : "";
}

function showNewUser() {
  router.push("/formUsers");
}

function onEdit(item: any) {
  //console.log(item);
  router.push("/editUsers/" + item.id);
}

onMounted(() => {
  showItems();
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