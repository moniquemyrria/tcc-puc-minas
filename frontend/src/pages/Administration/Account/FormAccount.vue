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
          {{ idAccount == "" ? "Nova Conta" : "Editar Conta" }}
        </h5>
        <div class="col-12 sub-title" style="text-align: start">
          {{
            idAccount == ""
              ? "Preencha os campos para criar uma nova Conta"
              : "Editar os campos da Conta"
          }}
        </div>
      </div>
    </div>
    <br />

    <KCardForm>
      <q-card-section class="q-pt-none" align="center">
        <q-form class="q-gutter-md">
          <div class="row">
            <div class="col-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
              <k-input
                v-model="account.name"
                filled
                label="Nome da Conta *"
                hint="Por favor, informe o nome"
                lazy-rules
                :rules="[
                  (val) =>
                    (val && val.length > 0) || 'Por favor, informe o nome',
                ]"
                maxlength="100"
              />
            </div>
          </div>

          <div class="row">
            <div class="col-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
              <q-select
                :options="listAccountCategry"
                class="k-input-pa"
                filled
                label="Categoria"
                option-label="description"
                v-model="account.accountCategory"
                reactive-rules
              />
            </div>
            <div class="col-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
              <q-select
                :options="listTypePerson"
                class="k-input-pa"
                filled
                label="Fornecedor"
                option-label="name"
                v-model="account.typePerson"
                reactive-rules
              />
            </div>
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
import { IAccount } from "@/pages/Administration/Account/Type/IAccount";
import { storeUser } from "@/core/store/userStore";
import { IAccountCategory } from "../AccountCategory/Type/IAccountCategory";
import { ITypePerson } from "../TypePerson/Type/ITypePerson";

const userStore = storeUser();
const userLogged = userStore.getUser.user;

const tab = ref("movies");
const account = ref(new IAccount());
const idAccount = ref("");
const router = useRouter();
const route = useRoute();

const dialogMessage = ref(false);
const dialogMessageText = ref("");
const dialogMessageTitle = ref("");
const dialogMessageType = ref("W");
const dialogLoading = ref(false);
const dialogNotify = ref(false);
const dialogMessageException = ref(false);

const listAccountCategry = ref([] as IAccountCategory[]);
const listTypePerson = ref([] as ITypePerson[]);

const dataBreadCrumbs = ref([
  {
    label: "Lista Contas",
    router: "/listAccount",
  },
]);

function closeNotify() {
  dialogNotify.value = false;
}

function showList() {
  router.push("/listAccount");
}

function callItemsBreadCrumbs() {
  dataBreadCrumbs.value.push(
    route.params.id
      ? {
          label: "Editar Conta",
          router: "/",
        }
      : {
          label: "Nova Conta",
          router: "/formAccount",
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
  if (!account.value.name) {
    dialogMessageText.value = "Informe o nome da Conta.";
    dialogNotify.value = true;
    return true;
  }

  if (!account.value.accountCategory.id) {
    dialogMessageText.value = "Selecione a categoria da conta.";
    dialogNotify.value = true;
    return true;
  }

  if (!account.value.typePerson.id) {
    dialogMessageText.value = "Selecione o Fornecedor.";
    dialogNotify.value = true;
    return true;
  }

  return false;
}

async function onSave() {
  if (!validateFields()) {
    dialogLoading.value = true;
    account.value.idUser = userLogged.id;
    account.value.idAccountCategory = account.value.accountCategory.id;
    account.value.idTypePerson = account.value.typePerson.id;

    let result = await Api.postResult<IResult>(
      "/Account/" + userLogged.id,
      account.value
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
    account.value.dateUpdate = new Date();
    account.value.idUser = userLogged.id;
    account.value.idAccountCategory = account.value.accountCategory.id;
    account.value.idTypePerson = account.value.typePerson.id;
    let result = await Api.putResult<IResult>("/Account", account.value);
    dialogLoading.value = false;
    dialogMessage.value = true;
    dialogMessageTitle.value = result.data.sucesso ? "Informação" : "Erro";
    dialogMessageText.value =
      result.data.message != null ? result.data.message : "";
  }
}

async function showAccount() {
  if (route.params.id) {
    idAccount.value = route.params.id.toString();

    dialogLoading.value = true;
    let result: IResult = {} as IResult;
    result = await Api.getIdResult<IResult>(
      "/Account/Edit",
      idAccount.value.toString()
    );

    console.log(result.data);

    if (result.data.sucesso) {
      account.value = result.data.tRetorno;
    }

    dialogLoading.value = false;
  }
}

async function showItemsAccountCategory() {
  dialogLoading.value = true;
  let result: IResult;
  result = await Api.getResult<IResult>(
    "/AccountCategory/Status/true/" + userLogged.id
  );

  if (result.data.sucesso) {
    listAccountCategry.value = result.data.tRetorno;
    dialogLoading.value = false;
  }
}

async function showItemsTypePerson() {
  dialogLoading.value = true;
  let result: IResult;
  result = await Api.getResult<IResult>(
    "/TypePerson/Person/J/" + userLogged.id
  );

  if (result.data.sucesso) {
    listTypePerson.value = result.data.tRetorno;
    dialogLoading.value = false;
  }
}

onMounted(() => {
  callItemsBreadCrumbs();
  showItemsAccountCategory();
  showItemsTypePerson();
  showAccount();
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
