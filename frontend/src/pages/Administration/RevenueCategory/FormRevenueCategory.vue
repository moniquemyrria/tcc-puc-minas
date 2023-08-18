<template>
  <div>
    <KDialog
      :title="dialogMessageTitle"
      :dialog="dialogMessage"
      :text="dialogMessageText"
      @kDialogClose="closeDialogMessage"
    ></KDialog>

    <KLoading
      :title="'Carregando Dados... Aguarde!'"
      :dialog="dialogLoading"
    ></KLoading>

    <KBreadCrumbs :data="dataBreadCrumbs" />

    <KNotify
      :dialog="dialogNotify"
      :text="dialogMessageText"
      :type="dialogMessageType"
      @kNotifyClose="closeNotify"
    />

    <br />

    <div class="row">
      <div class="col-12" style="text-align: start">
        <h5 style="margin: 4px 0px 20px" class="title">
          {{ idRevenueCategory == "" ? "Nova Categoria" : "Editar Categoria" }}
        </h5>
        <div class="col-12 sub-title" style="text-align: start">
          {{
            idRevenueCategory == ""
              ? "Preencha os campos para criar uma nova Categoria de Receita"
              : "Editar os campos da Categoria de Receita"
          }}
        </div>
      </div>
    </div>
    <br />

    <KCardForm>
      <q-card-section class="q-pt-none" align="center">
        <q-form class="q-gutter-md">
          <div class="row">
            <div class="col-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2">
              <k-input
                v-model="revenueCategory.initials"
                filled
                label="Iniciais *"
              />
            </div>
            <div
              class="col-12 col-sm-12 col-md-10 col-lg-10 col-xl-10 col-xxl-10"
            >
              <k-input
                v-model="revenueCategory.description"
                filled
                label="Descrição *"
                hint="Por favor, informe a Descrição"
                lazy-rules
                :rules="[
                  (val) =>
                    (val && val.length > 0) || 'Por favor, informe a Descrição',
                ]"
                maxlength="100"
              />
            </div>
          </div>

          <q-separator></q-separator>
          <div class="row">Cor da categoria</div>

          <div class="row">
            <q-color
              v-model="revenueCategory.color"
              no-footer
              class="my-picker"
            />
          </div>
        </q-form>
        <br />
      </q-card-section>
      <q-separator />
      <q-card-actions align="right">
        <div class="row"></div>
        <KButtonCancel :text="'Cancelar'" @click="showList()" />

        <KButton v-if="route.params.id" :text="'Salvar'" @click="onEdit()" />
        <KButton v-else :text="'Salvar'" @click="onSave()" />
      </q-card-actions>
    </KCardForm>
  </div>
</template>

<script setup lang="ts">
//import DadosFuncionario from "@/components/DadosFuncionario.vue";
//import DescicaoCargo from "@/components/DescricaoCargo.vue";
import { ref, onMounted, watch } from "vue";
import { useRouter, useRoute } from "vue-router";
import { useQuasar } from "quasar";
import KBreadCrumbs from "./../../../components/KBreadCrumbs.vue";
import KCardForm from "./../../../components/KCardForm.vue";
import KButton from "./../../../components/KButton.vue";
import KButtonCancel from "./../../../components/KButtonCancel.vue";
import KNotify from "../../../components/KNotify.vue";
import Api from "../../../core/api/api";
import KDialog from "@/components/KDialog.vue";
import KLoading from "../../../components/KLoading.vue";
import KInput from "@components/KInput.vue";
import { IResult } from "@/core/types/IKResult";
import { IRevenueCategory } from "@/pages/Administration/RevenueCategory/Type/IRevenueCategory";
import { storeUser } from "@/core/store/userStore";

const userStore = storeUser();
const userLogged = userStore.getUser.user;

const tab = ref("movies");
const revenueCategory = ref(new IRevenueCategory());
const idRevenueCategory = ref("");
const router = useRouter();
const route = useRoute();

const dialogMessage = ref(false);
const dialogMessageText = ref("");
const dialogMessageTitle = ref("");
const dialogMessageType = ref("W");
const dialogLoading = ref(false);
const dialogNotify = ref(false);
const dialogMessageException = ref(false);

const dataBreadCrumbs = ref([
  {
    label: "Lista Categoria de Receitas",
    router: "/listRevenueCategory",
  },
]);

function closeNotify() {
  dialogNotify.value = false;
}

function showList() {
  router.push("/listRevenueCategory");
}

function callItemsBreadCrumbs() {
  dataBreadCrumbs.value.push(
    route.params.id
      ? {
          label: "Editar Categoria de Receita",
          router: "/",
        }
      : {
          label: "Nova Categoria de Receita",
          router: "/formRevenueCategory",
        }
  );
}

function closeDialogMessage() {
  dialogMessage.value = false;

  if (!dialogMessageException.value) {
    showList();
  }
}

function validateFields() {
  if (!revenueCategory.value.description) {
    dialogMessageText.value = "Informe a descrição da Categoria.";
    dialogNotify.value = true;
    return true;
  }

  return false;
}

async function onSave() {
  if (!validateFields()) {
    dialogLoading.value = true;
    revenueCategory.value.idUser = userLogged.id;

    let result = await Api.postResult<IResult>(
      "/RevenueCategory/" + userLogged.id,
      revenueCategory.value
    );
    dialogLoading.value = false;
    dialogMessage.value = true;
    dialogMessageTitle.value = result.data.sucesso ? "Informação" : "Erro";
    dialogMessageText.value =
      result.data.message != null ? result.data.message : "";

    dialogMessageException.value = !result.data.sucesso ? true : false;
  }
}

async function onEdit() {
  if (!validateFields()) {
    dialogLoading.value = true;
    revenueCategory.value.dateUpdate = new Date();
    let result = await Api.putResult<IResult>(
      "/RevenueCategory",
      revenueCategory.value
    );
    dialogLoading.value = false;
    dialogMessage.value = true;
    dialogMessageTitle.value = result.data.sucesso ? "Informação" : "Erro";
    dialogMessageText.value =
      result.data.message != null ? result.data.message : "";
  }
}

async function showExpenseCategory() {
  if (route.params.id) {
    idRevenueCategory.value = route.params.id.toString();

    dialogLoading.value = true;
    let result: IResult = {} as IResult;
    result = await Api.getIdResult<IResult>(
      "/RevenueCategory/Edit",
      idRevenueCategory.value.toString()
    );

    console.log(result.data);

    if (result.data.sucesso) {
      revenueCategory.value = result.data.tRetorno;
    }

    dialogLoading.value = false;
  }
}

onMounted(() => {
  callItemsBreadCrumbs();
  showExpenseCategory();
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
