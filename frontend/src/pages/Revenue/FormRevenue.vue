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
          {{ idRevenue == "" ? "Nova Receita" : "Editar Receita" }}
        </h5>
        <div class="col-12 sub-title" style="text-align: start">
          {{
            idRevenue == ""
              ? "Preencha os campos para criar uma nova Receita"
              : "Editar os campos da Receita"
          }}
        </div>
      </div>
    </div>
    <br />

    <KCardForm>
      <q-card-section class="q-pt-none" align="center">
        <q-form class="q-gutter-md">
          <div class="row">
            <div class="col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3 col-xxl-3">
              <q-input
                filled
                class="k-input-pa"
                label="Valor da receita (R$) *"
                type="number"
                prefix="R$"
                v-model="valueRevenue"
                lazy-rules
                :rules="[
                  (val) =>
                    (val > 0 && val !== 0) ||
                    'O Valor da receita não pode ser negativou ou zerado',
                ]"
                placeholder="0.00"
              >
                <template v-slot:append> </template>
              </q-input>
            </div>
          </div>
          <br />

          <q-separator></q-separator>

          <div class="row">
            <div class="col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3 col-xxl-3">
              <q-input
                label="Data de Recebimento"
                class="k-input-pa"
                filled
                v-model="date"
              >
                <template v-slot:append>
                  <q-icon name="event" class="cursor-pointer">
                    <q-popup-proxy
                      cover
                      transition-show="scale"
                      transition-hide="scale"
                    >
                      <q-date mask="DD/MM/YYYY" v-model="date">
                        <div class="row items-center justify-end">
                          <q-btn
                            v-close-popup
                            label="FECHAR CALENDÁRIO"
                            color="primary"
                            flat
                          />
                        </div>
                      </q-date>
                    </q-popup-proxy>
                  </q-icon>
                </template>
              </q-input>
            </div>
            <div class="col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3 col-xxl-4">
              <q-select
                :options="listRevenueStatus"
                class="k-input-pa"
                filled
                label="Situação da Receita"
                v-model="revenue.revenueStatus"
              />
            </div>
          </div>
          <div class="row">
            <div
              class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
            >
              <k-input filled v-model="revenue.description" label="Descrição" />
            </div>
          </div>
          <div class="row">
            <div class="col-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
              <q-select
                :options="listRevenueReceipt"
                class="k-input-pa"
                filled
                label="Forma de Recebimento"
                v-model="revenue.revenueReceipt"
                reactive-rules
              />
            </div>
            <div class="col-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
              <q-select
                :options="listTypePerson"
                class="k-input-pa"
                filled
                label="Fornecedor *"
                option-label="name"
                v-model="revenue.supplier"
                reactive-rules
              />
            </div>
          </div>
          <div class="row">
            <div class="col-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
              <q-select
                :options="listRevenueCategory"
                class="k-input-pa"
                filled
                label="Categoria *"
                option-label="description"
                v-model="revenue.revenueCategory"
                reactive-rules
              />
            </div>
            <div class="col-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
              <q-select
                :options="listAccount"
                class="k-input-pa"
                filled
                label="Conta *"
                option-label="name"
                v-model="revenue.account"
                reactive-rules
              />
            </div>
          </div>

          <div>
            <div
              class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
            >
              <q-input
                filled
                v-model="revenue.obs"
                label="Observações "
                type="textarea"
                class="k-input-pa"
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
//import DadosFuncionario from "@/components/DadosFuncionario.vue";
//import DescicaoCargo from "@/components/DescricaoCargo.vue";
import { ref, onMounted, watch } from "vue";
import { useRouter, useRoute } from "vue-router";
import { useQuasar } from "quasar";

import KCardForm from "./../../components/KCardForm.vue";
import KButtonCancel from "./../../components/KButtonCancel.vue";
import Api from "../../core/api/api";
import KInput from "@components/KInput.vue";

import KDialog from "./../../components/KDialog.vue";
import KLoading from "./../../components/KLoading.vue";
import KNotify from "../../components/KNotify.vue";
import KBreadCrumbs from "./../../components/KBreadCrumbs.vue";

import routes from "@/modules/router";
import KButtonAction from "../../components/KButtonAction.vue";
import KButton from "../../components/KButton.vue";
import { IResult } from "@/core/types/IKResult";
import KDialogQuestion from "@/components/KDialogQuestion.vue";
import { storeUser } from "@/core/store/userStore";
import { IRevenue } from "./Type/IRevenue";
import { IAccount } from "../Administration/Account/Type/IAccount";
import { IRevenueCategory } from "../Administration/RevenueCategory/Type/IRevenueCategory";
import moment from "moment";
import { ITypePerson } from "../Administration/TypePerson/Type/ITypePerson";

const userStore = storeUser();
const userLogged = userStore.getUser.user;

const date = ref(moment(new Date()).format("DD/MM/YYYY").toString());

const tab = ref("movies");
const revenue = ref(new IRevenue());
const idRevenue = ref("");
const router = useRouter();
const route = useRoute();
const valueRevenue = ref("");

const dialogMessage = ref(false);
const dialogMessageText = ref("");
const dialogMessageTitle = ref("");
const dialogMessageType = ref("W");
const dialogLoading = ref(false);
const dialogNotify = ref(false);
const dialogMessageException = ref(false);

const listRevenueReceipt = ref([
  "Pix",
  "Dinheiro",
  "Boleto",
  "Cheque",
  "Depósito Bancário",
  "Transferência Bancária",
  "Não Especificado",
]);

const listRevenueStatus = ref(["Recebido", "A Receber"]);
const listAccount = ref([] as IAccount[]);
const listRevenueCategory = ref([] as IRevenueCategory[]);
const listTypePerson = ref([] as ITypePerson[]);

const dataBreadCrumbs = ref([
  {
    label: "Lista Receitas",
    router: "/listRevenue",
  },
]);

function closeNotify() {
  dialogNotify.value = false;
}

function showList() {
  router.push("/listRevenue");
}

function callItemsBreadCrumbs() {
  dataBreadCrumbs.value.push(
    route.params.id
      ? {
          label: "Editar Receita",
          router: "/",
        }
      : {
          label: "Nova Receita",
          router: "/formRevenue",
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
  if (
    parseFloat(valueRevenue.value) < 0 ||
    parseFloat(valueRevenue.value) == 0 ||
    parseFloat(valueRevenue.value) < 0 ||
    valueRevenue.value.trim() == ""
  ) {
    dialogMessageText.value = "O valor da Receita informado, esté inválido.";
    dialogNotify.value = true;
    return true;
  }

  if (!revenue.value.supplier.id) {
    dialogMessageText.value = "Selecione o Fornecedor.";
    dialogNotify.value = true;
    return true;
  }

  if (!revenue.value.revenueCategory.id) {
    dialogMessageText.value = "Selecione a Categoria.";
    dialogNotify.value = true;
    return true;
  }

  if (!revenue.value.account.id) {
    dialogMessageText.value = "Selecione a Conta.";
    dialogNotify.value = true;
    return true;
  }

  return false;
}

async function onSave() {
  if (!validateFields()) {
    dialogLoading.value = true;
    revenue.value.idUser = userLogged.id;
    revenue.value.idAccount = revenue.value.account.id;
    revenue.value.idRevenueCategory = revenue.value.revenueCategory.id;
    revenue.value.idTypePerson = revenue.value.supplier.id;
    revenue.value.dateCreated = moment(date.value, "DD/MM/YYYY").toDate();
    revenue.value.value = parseFloat(valueRevenue.value.replace(",", "."));

    let result = await Api.postResult<IResult>(
      "/Revenue/" + userLogged.id,
      revenue.value
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
    revenue.value.idUser = userLogged.id;
    revenue.value.idAccount = revenue.value.account.id;
    revenue.value.idRevenueCategory = revenue.value.revenueCategory.id;
    revenue.value.idTypePerson = revenue.value.supplier.id;
    revenue.value.dateCreated = moment(date.value, "DD/MM/YYYY").toDate();
    revenue.value.value = parseFloat(valueRevenue.value.replace(",", "."));
    dialogLoading.value = true;
    revenue.value.dateUpdate = new Date();
    let result = await Api.putResult<IResult>("/Revenue", revenue.value);
    dialogLoading.value = false;
    dialogMessage.value = true;
    dialogMessageTitle.value = result.data.sucesso ? "Informação" : "Erro";
    dialogMessageText.value =
      result.data.message != null ? result.data.message : "";
  }
}

async function showRevenue() {
  if (route.params.id) {
    idRevenue.value = route.params.id.toString();

    dialogLoading.value = true;
    let result: IResult = {} as IResult;
    result = await Api.getIdResult<IResult>(
      "/Revenue/Edit",
      idRevenue.value.toString()
    );

    if (result.data.sucesso) {
      revenue.value = result.data.tRetorno;
      valueRevenue.value = revenue.value.value.toString();
      date.value = moment(revenue.value.dateCreated).format("DD/MM/YYYY");
    }

    dialogLoading.value = false;
  }
}
async function showItemsTypePerson() {
  dialogLoading.value = true;
  let result: IResult;
  result = await Api.getResult<IResult>(
    "/TypePerson/Status/true/" + userLogged.id
  );

  if (result.data.sucesso) {
    listTypePerson.value = result.data.tRetorno;
    dialogLoading.value = false;
  }
}

async function showItemsAccount() {
  dialogLoading.value = true;
  let result: IResult;
  result = await Api.getResult<IResult>(
    "/Account/Status/true/" + userLogged.id
  );

  if (result.data.sucesso) {
    listAccount.value = result.data.tRetorno;
    dialogLoading.value = false;
  }
}

async function showItemsRevenueCategory() {
  dialogLoading.value = true;
  let result: IResult;
  result = await Api.getResult<IResult>(
    "/RevenueCategory/Status/true/" + userLogged.id
  );

  if (result.data.sucesso) {
    listRevenueCategory.value = result.data.tRetorno;
    dialogLoading.value = false;
  }
}

onMounted(() => {
  showItemsAccount();
  showItemsTypePerson();
  showItemsRevenueCategory();
  callItemsBreadCrumbs();
  showRevenue();
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
