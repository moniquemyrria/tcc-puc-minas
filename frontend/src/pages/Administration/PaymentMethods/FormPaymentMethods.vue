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
          {{
            idPaymentMethods == ""
              ? "Nova Forma de Pagamento"
              : "Editar Forma de Pagamento"
          }}
        </h5>
        <div class="col-12 sub-title" style="text-align: start">
          {{
            idPaymentMethods == ""
              ? "Preencha os campos para criar uma nova Forma de Pagamento"
              : "Editar os campos da Forma de Pagamento"
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
                v-model="paymentMethods.name"
                
                filled
                label="Nome *"
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
                :options="listAccount"
                class="k-input-pa"
                
                filled
                label="Conta"
                option-label="name"
                v-model="paymentMethods.account"
                reactive-rules
              />
            </div>
            <div class="col-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
              <q-select
                :options="listAcceptInstallment"
                class="k-input-pa"
                
                filled
                label="Aceita Parcelamento? "
                option-label="name"
                v-model="acceptInstallment"
                reactive-rules
              />
            </div>
          </div>
          <q-separator />
          <div class="row">
            <span>Dados dos Tipos de Pagamentos</span>
          </div>
          <div class="row">
            <div class="col-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
              <q-select
                :options="listPaymentMethodsTypes"
                class="k-input-pa"
                
                filled
                label="Tipos de Formas de Pagamento *"
                v-model="paymentMethodsTypes"
              />
            </div>
            <div class="col-12 col-sm-12 col-md-1 col-lg-1 col-xl-1 col-xxl-1">
              <q-btn
                @click="addPaymentMethodsTypes()"
                round
                color="green"
                title="Adcionar Tipo"
                icon="add"
              />
            </div>
          </div>
          <div class="row">
            <div
              class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
            >
              <q-table
                :rows="listPaymentMethodsTypesSelected.filter(t => t.isActive)"
                :columns="headers"
                :filter="filter"
                :rows-per-page-options="[10]"
              >
                <template v-slot:body-cell-actions="props">
                  <q-td :props="props">
                    <KButtonAction
                      title="Excluir"
                      @click="onDelete(props.row)"
                      :icon="'delete'"
                    />
                  </q-td>
                </template>
              </q-table>
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
import KButtonAction from "../../../components/KButtonAction.vue";
import KNotify from "../../../components/KNotify.vue";
import Api from "../../../core/api/api";
import KDialog from "@/components/KDialog.vue";
import KLoading from "../../../components/KLoading.vue";
import KInput from "@components/KInput.vue";
import { IResult } from "@/core/types/IKResult";
import { storeUser } from "@/core/store/userStore";
import { IPaymentMethods } from "../PaymentMethods/Type/IPaymentMethods";
import { ITypePerson } from "../TypePerson/Type/ITypePerson";
import { IPaymentMethodsTypes } from "./Type/IPaymentMethodsTypes";
import { IAccount } from "../Account/Type/IAccount";

const userStore = storeUser();
const userLogged = userStore.getUser.user;

const tab = ref("movies");
const paymentMethods = ref(new IPaymentMethods());
const idPaymentMethods = ref("");
const router = useRouter();
const route = useRoute();

const dialogMessage = ref(false);
const dialogMessageText = ref("");
const dialogMessageTitle = ref("");
const dialogMessageType = ref("W");
const dialogLoading = ref(false);
const dialogNotify = ref(false);
const dialogMessageException = ref(false);

const headers = ref([
  {
    name: "description",
    align: "left",
    label: "Descrição do Tipo",
    field: "description",
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

const listAcceptInstallment = ref(["Sim", "Não"]);

const listPaymentMethodsTypes = ref([
  "Pix",
  "Dinheiro",
  "Cheque",
  "Cartão de Crédito",
  "Cartão de Débito",
  "Boleto Bancário",
  "Crediário",
  "Carteira Digital",
  "Carteira Digital (E-Wallets)",
  "Link de Pagamento",
  "Transferência Bancária",
  "Transferência Eletrônica",
  "Troca de Pontos / Milhas",
  "Intermediador de Pagamentos",
  "Depósito Bancário",
]);
const listAccount = ref([] as IAccount[]);
const listPaymentMethodsTypesSelected = ref([] as IPaymentMethodsTypes[]);
const paymentMethodsTypes = ref("");

const acceptInstallment = ref("Não");

const dataBreadCrumbs = ref([
  {
    label: "Lista Forma de Pagamentos",
    router: "/listPaymentMethod",
  },
]);

function onDelete(item: IPaymentMethodsTypes) {
  console.log(idPaymentMethods.value);

  if (idPaymentMethods.value == "") {

    let items = listPaymentMethodsTypesSelected.value.filter(
      (t) => t.description.trim() != item.description.trim()
    );

    listPaymentMethodsTypesSelected.value = JSON.parse(JSON.stringify(items));
    
  } else {
    
    listPaymentMethodsTypesSelected.value.forEach(t => 
      (t.description.trim() == item.description.trim()) ? t.isActive = false : true
    )

    listPaymentMethodsTypesSelected.value = JSON.parse(JSON.stringify(listPaymentMethodsTypesSelected.value));
  }
}

function addPaymentMethodsTypes() {
  if (!paymentMethodsTypes.value) {
    dialogMessageText.value = "Selecione o tipo da forma de pagamento.";
    dialogNotify.value = true;
    return;
  } else {
    if (
      listPaymentMethodsTypesSelected.value.filter(
        (t: IPaymentMethodsTypes) => t.description == paymentMethodsTypes.value
      ).length == 0
    ) {
      let item = new IPaymentMethodsTypes();

      item.description = paymentMethodsTypes.value;
      listPaymentMethodsTypesSelected.value.push(item);
    } else {
      dialogMessageText.value = "Este tipo já está inserido a listagem.";
      dialogNotify.value = true;
    }
  }
}

function closeNotify() {
  dialogNotify.value = false;
}

function showList() {
  router.push("/listPaymentMethods");
}

function callItemsBreadCrumbs() {
  dataBreadCrumbs.value.push(
    route.params.id
      ? {
          label: "Editar Forma de Pagamento",
          router: "/",
        }
      : {
          label: "Nova Forma de Pagamento",
          router: "/formPaymentMethods",
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
  if (!paymentMethods.value.name) {
    dialogMessageText.value = "Informe o nome da Forma de Pagamento.";
    dialogNotify.value = true;
    return true;
  }

  if (!paymentMethods.value.account.id) {
    dialogMessageText.value = "Selecione a conta.";
    dialogNotify.value = true;
    return true;
  }

  if (listPaymentMethodsTypesSelected.value.length == 0) {
    dialogMessageText.value =
      "É necessário inserir ao menos um tipo a forma de pagamento.";
    dialogNotify.value = true;
    return true;
  }

  return false;
}

async function onSave() {
  if (!validateFields()) {
    dialogLoading.value = true;
    paymentMethods.value.idUser = userLogged.id;
    paymentMethods.value.idAccount = paymentMethods.value.account.id;
    paymentMethods.value.paymentMethodsTypes =
      listPaymentMethodsTypesSelected.value;

    let result = await Api.postResult<IResult>(
      "/PaymentMethods/" + userLogged.id,
      paymentMethods.value
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
    paymentMethods.value.dateUpdate = new Date();
    paymentMethods.value.idUser = userLogged.id;
    paymentMethods.value.idAccount = paymentMethods.value.account.id;
    paymentMethods.value.paymentMethodsTypes =
      listPaymentMethodsTypesSelected.value;
    let result = await Api.putResult<IResult>(
      "/PaymentMethods",
      paymentMethods.value
    );
    dialogLoading.value = false;
    dialogMessage.value = true;
    dialogMessageTitle.value = result.data.sucesso ? "Informação" : "Erro";
    dialogMessageText.value =
      result.data.message != null ? result.data.message : "";
  }
}

async function showPaymentMethods() {
  if (route.params.id) {
    idPaymentMethods.value = route.params.id.toString();

    dialogLoading.value = true;
    let result: IResult = {} as IResult;
    result = await Api.getIdResult<IResult>(
      "/PaymentMethods/Edit",
      idPaymentMethods.value.toString()
    );

    console.log(result.data);

    if (result.data.sucesso) {
      paymentMethods.value = result.data.tRetorno;
      listPaymentMethodsTypesSelected.value =
        result.data.tRetorno.paymentMethodsTypes;
    }

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

onMounted(() => {
  callItemsBreadCrumbs();
  showItemsAccount();
  showPaymentMethods();
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
