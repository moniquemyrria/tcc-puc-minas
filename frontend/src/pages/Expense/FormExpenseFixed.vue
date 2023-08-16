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

    <!-- PAYMENT METHODS -->
    <q-dialog v-model="dialogPayment" persistent>
      <q-card style="min-width: 1000px">
        <q-card-section>
          <div class="text-h6">Métodos de Pagamentos da Despesa Fixa</div>
          <div class="text-caption text-grey">
            Informe os valores pagos na despesa por Método de Pagamento
          </div>
        </q-card-section>

        <div class="row">
          <div
            class="col-12 col-sm-12 col-md-8 col-lg-8 col-xl-8 col-xxl-8"
          ></div>
          <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4 col-xxl-4">
            <k-input
              filled
              class="k-input-pa"
              label="Valor Total Pago (R$) *"
              type="number"
              prefix="R$"
              v-model="valueExpense"
              placeholder="0.00"
              readonly
            >
              <template v-slot:append> </template>
            </k-input>
          </div>
        </div>
        <br />

        <q-card-section class="q-pt-none">
          <div
            class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
          >
            <q-table
              :rows="expense.paymentMethods.paymentMethodsTypes"
              hide-pagination
              :columns="headers"
              :rows-per-page-options="[10]"
              hide-bottom
              flat
              class="k-table-payment"
            >
              <template v-slot:body-cell-description="props">
                <q-td :props="props">
                  <span style="width: 250px; float: right">{{
                    props.row.description + ": "
                  }}</span>
                </q-td>
              </template>

              <template v-slot:body-cell-trafficTicket="props">
                <q-td :props="props">
                  <k-input
                    borderless
                    style="max-height: 65px; margin-top: -12px"
                    type="number"
                    v-model="props.row.trafficTicket"
                    lazy-rules
                    placeholder="0.00"
                    @update:model-value="
                      (value) => onChangeSumPaymentMethods(value)
                    "
                    :readonly="idExpense !== ''"
                  >
                    <template v-slot:append> </template>
                  </k-input>
                </q-td>
              </template>
              <template v-slot:body-cell-taxRate="props">
                <q-td :props="props">
                  <k-input
                    borderless
                    style="max-height: 65px; margin-top: -12px"
                    type="number"
                    v-model="props.row.taxRate"
                    lazy-rules
                    placeholder="0.00"
                    @update:model-value="
                      (value) => onChangeSumPaymentMethods(value)
                    "
                    :readonly="idExpense !== ''"
                  >
                    <template v-slot:append> </template>
                  </k-input>
                </q-td>
              </template>
              <template v-slot:body-cell-value="props">
                <q-td :props="props">
                  <k-input
                    borderless
                    style="max-height: 65px; margin-top: -12px"
                    type="number"
                    v-model="props.row.value"
                    lazy-rules
                    placeholder="0.00"
                    @update:model-value="
                      (value) => onChangeSumPaymentMethods(value)
                    "
                    :readonly="idExpense !== ''"
                  >
                    <template v-slot:append> </template>
                  </k-input>
                </q-td>
              </template>
              <template v-slot:body-cell-totalPayment="props">
                <q-td :props="props">
                  <k-input
                    borderless
                    style="max-height: 65px; margin-top: -12px"
                    v-model="props.row.totalPayment"
                    readonly
                    placeholder="0.00"
                  >
                    <template v-slot:append> </template>
                  </k-input>
                </q-td>
              </template>
            </q-table>
          </div>
        </q-card-section>

        <q-card-actions align="right">
          <div class="row"></div>
          <KButtonCancel text="Voltar" @click="cancelShowPayment()" />
        </q-card-actions>
      </q-card>
    </q-dialog>

    <div class="row">
      <div class="col-12" style="text-align: start">
        <h5 style="margin: 4px 0px 20px" class="title">
          {{ idExpense == "" ? "Nova Despesa Fixa" : "Editar Despesa Fixa" }}
        </h5>
        <div class="col-12 sub-title" style="text-align: start">
          {{
            idExpense == ""
              ? "Preencha os campos para criar uma nova Despesa Fixa"
              : "Editar os campos da Despesa Fixa"
          }}
        </div>
      </div>
    </div>
    <br />

    <KCardForm>
      <q-card-section class="q-pt-none" align="center">
        <q-form class="q-gutter-md">
          <div class="row">
            <div
              class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
            >
              <k-input
                filled
                label="Descrição"
                hint="Por favor, informe a Descrição"
                lazy-rules
                :rules="[
                  (val) =>
                    (val && val.length > 0) || 'Por favor, informe a Descrição',
                ]"
                maxlength="100"
                v-model="expense.description"
              />
            </div>
          </div>

          <div class="row">
            <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4 col-xxl-4">
              <q-select
                :options="listExpenseCategory"
                class="k-input-pa"
                filled
                label="Categoria *"
                option-label="description"
                reactive-rules
                v-model="expense.expenseCategory"
              />
            </div>
            <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4 col-xxl-4">
              <q-select
                :options="listAccount"
                class="k-input-pa"
                filled
                label="Conta *"
                option-label="name"
                reactive-rules
                v-model="expense.account"
              />
            </div>

            <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4 col-xxl-4">
              <q-select
                :options="listTypePerson"
                class="k-input-pa"
                filled
                label="Fornecedor *"
                option-label="name"
                reactive-rules
                v-model="expense.supplier"
              />
            </div>
          </div>
          <q-separator />

          <div class="row">
            <div class="col-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2">
              <q-input
                label="Data de Vencimento"
                class="k-input-pa"
                filled
                v-model="date"
                :readonly="idExpense != ''"
              >
                <template v-slot:append>
                  <q-icon
                    name="event"
                    class="cursor-pointer"
                    v-if="idExpense == ''"
                  >
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
            <div class="col-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2">
              <q-input
                filled
                class="k-input-pa"
                label="Valor da Despesa (R$) *"
                type="number"
                prefix="R$"
                v-model="valueExpense"
                lazy-rules
                :rules="[
                  (val) =>
                    (val > 0 && val !== 0) ||
                    'O Valor da despesa não pode ser negativou ou zerado',
                ]"
                placeholder="0.00"
                :readonly="idExpense != ''"
              >
                <template v-slot:append> </template>
              </q-input>
            </div>
          </div>

          <q-separator></q-separator>

          <div
            class="row"
            v-if="
              expense.paymentMethods != mull &&
              expense.paymentMethods.paymentMethodsTypes.length > 0 &&
              expense.expenseStatus == 'Pago'
            "
          >
            <q-btn
              v-if="idExpense == ''"
              @click="showPaymentMethods()"
              color="secondary"
              title="Inserir/Visualizar Método de Pagamento"
              icon="payment"
              unelevated
              rounded
              label="Métodos de Pagamento"
              size="md"
            />
            <q-btn
              v-if="idExpense != ''"
              @click="showViewPaymentMethods()"
              color="secondary"
              title="Visualizar Método de Pagamento"
              icon="visibility"
              unelevated
              rounded
              label="Visualizar Pagamento"
              size="md"
              style="width: 280px"
            />
          </div>

          <div class="row">
            <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4 col-xxl-4">
              <q-select
                :options="listExpenseStatus"
                class="k-input-pa text-orange"
                filled
                label="Situação da Despesa"
                v-model="expense.expenseStatus"
                @update:model-value="(value) => onChangePaymentMethods(value)"
                :readonly="idExpense != ''"
              />
            </div>
            <div
              v-if="expense.expenseStatus == 'Pago'"
              class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4 col-xxl-4"
            >
              <q-select
                :options="listPaymentMethods"
                class="k-input-pa"
                filled
                label="Formas de Pagamento *"
                option-label="name"
                reactive-rules
                v-model="expense.paymentMethods"
                @update:model-value="(value) => onChangePaymentMethods(value)"
                :readonly="idExpense != ''"
              />
            </div>
          </div>

          <q-separator />
          <div class="row">
            <div
              class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
            >
              <q-input
                filled
                label="Observações "
                type="textarea"
                class="k-input-pa"
                v-model="expense.obs"
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
import KBreadCrumbs from "./../../components/KBreadCrumbs.vue";
import KCardForm from "./../../components/KCardForm.vue";
import KButton from "./../../components/KButton.vue";
import KButtonCancel from "./../../components/KButtonCancel.vue";
import KNotify from "../../components/KNotify.vue";
import Api from "../../core/api/api";
import KDialog from "@/components/KDialog.vue";
import KLoading from "../../components/KLoading.vue";
import KInput from "@components/KInput.vue";
import { IResult } from "@/core/types/IKResult";
import { IExpense, IExpenseInstallments } from "@/pages/Expense/Type/IExpense";
import { storeUser } from "@/core/store/userStore";
import moment from "moment";
import { IExpenseCategory } from "../Administration/ExpenseCategory/Type/IExpenseCategory";
import { IAccount } from "../Administration/Account/Type/IAccount";
import { ITypePerson } from "../Administration/TypePerson/Type/ITypePerson";
import { IPaymentMethods } from "../Administration/PaymentMethods/Type/IPaymentMethods";
import { IPaymentMethodsTypes } from "../Administration/PaymentMethods/Type/IPaymentMethodsTypes";

const date = ref(moment(new Date()).format("DD/MM/YYYY").toString());

const listExpenseStatus = ref(["A Pagar", "Pago"]);
const listExpenseCategory = ref([] as IExpenseCategory[]);
const listAccount = ref([] as IAccount[]);
const listTypePerson = ref([] as ITypePerson[]);
const listPaymentMethods = ref([] as IPaymentMethods[]);

const userStore = storeUser();
const userLogged = userStore.getUser.user;

const tab = ref("movies");
const expense = ref(new IExpense());
const idExpense = ref("");
const router = useRouter();
const route = useRoute();

const dialogMessage = ref(false);
const dialogMessageText = ref("");
const dialogMessageTitle = ref("");
const dialogMessageType = ref("W");
const dialogLoading = ref(false);
const dialogNotify = ref(false);
const dialogMessageException = ref(false);
const dialogPayment = ref(false);

//const itemsPaymentMethodsTypes = ref([] as IPaymentMethodsTypes[])

const valueExpense = ref("");

const dataBreadCrumbs = ref([
  {
    label: "Lista de Despesas",
    router: "/listExpense",
  },
]);

const headers = ref([
  {
    name: "description",
    align: "right",
    label: "",
    field: "description",
    sortable: false,
  },

  {
    name: "trafficTicket",
    align: "left",
    label: "Multas (R$)",
    field: "trafficTicket",
    sortable: false,
  },

  {
    name: "taxRate",
    align: "left",
    label: "Juros (R$)",
    field: "taxRate",
    sortable: false,
  },

  {
    name: "value",
    align: "left",
    label: "Valor Pago(R$)",
    field: "value",
    sortable: false,
  },

  {
    name: "totalPayment",
    align: "left",
    label: "Total por Método(R$)",
    field: "totalPayment",
    sortable: false,
  },
]);

const listExpenseInstallments: any = ref([] as IExpenseInstallments[]);

function showViewPaymentMethods() {
  dialogPayment.value = true;
}

function showPaymentMethods() {
  dialogPayment.value = true;
}

function cancelShowPayment() {
  dialogPayment.value = false;
}

function onChangeSumPaymentMethods(value: number) {
  let sumTotalExpense = 0;

  if (expense.value.paymentMethods != null) {
    expense.value.paymentMethods.paymentMethodsTypes.forEach((item) => {
      let taxRate =
        item.taxRate == null ||
        item.taxRate == undefined ||
        item.taxRate.toString() == ""
          ? 0
          : item.taxRate;
      let trafficTicket =
        item.trafficTicket == null ||
        item.trafficTicket == undefined ||
        item.trafficTicket.toString() == ""
          ? 0
          : item.trafficTicket;
      let value =
        item.value == null ||
        item.value == undefined ||
        item.value.toString() == ""
          ? 0
          : item.value;

      let totalPayment =
        parseFloat(taxRate.toString().replace(",", ".")) +
        parseFloat(trafficTicket.toString().replace(",", ".")) +
        parseFloat(value.toString().replace(",", "."));

      item.totalPayment = totalPayment > 0 ? totalPayment : null;

      if (item.totalPayment != null && item.totalPayment > 0) {
        sumTotalExpense =
          sumTotalExpense +
          parseFloat(item.totalPayment.toString().replace(",", "."));
      }
    });

    expense.value.amount = JSON.parse(JSON.stringify(sumTotalExpense));
    valueExpense.value =
      expense.value.amount != null ? expense.value.amount.toString() : "";
  }
}

function onChangePaymentMethods(value: number) {
  expense.value.amount = 0;
  //valueExpense.value = "";

  if (
    expense.value.expenseStatus == "Pago" &&
    expense.value.paymentMethods != null &&
    expense.value.paymentMethods.paymentMethodsTypes.length > 0
  ) {
    dialogPayment.value = true;
  }
}

function closeNotify() {
  dialogNotify.value = false;
}

function showList() {
  router.push("/listExpense");
}

function callItemsBreadCrumbs() {
  dataBreadCrumbs.value.push(
    route.params.id
      ? {
          label: "Editar Despesa",
          router: "/",
        }
      : {
          label: "Nova Despesa",
          router: "/formExpense",
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
  if (expense.value.expenseCategory.id == 0) {
    dialogMessageText.value = "Selecione uma categoria para a despesa.";
    dialogNotify.value = true;
    return true;
  }
  if (expense.value.account.id == 0) {
    dialogMessageText.value = "Selecione uma conta para a despesa.";
    dialogNotify.value = true;
    return true;
  }

  if (expense.value.supplier.id == 0) {
    dialogMessageText.value =
      "Selecione um Fornecedor a despesa.";
    dialogNotify.value = true;
    return true;
  }
  if (valueExpense.value == "") {
    dialogMessageText.value = "Informe o valor da despesa.";
    dialogNotify.value = true;
    return true;
  }

  if (
    expense.value.expenseStatus == "Pago" &&
    expense.value.paymentMethods?.id == 0
  ) {
    dialogMessageText.value =
      "Selecione o Método de Pagamento para a Despesa Paga.";
    dialogNotify.value = true;
    return true;
  }

  if (
    expense.value.expenseStatus == "Pago" &&
    expense.value.paymentMethods != null
  ) {
    if (expense.value.paymentMethods.paymentMethodsTypes.length > 0) {
      let qtdeValues = expense.value.paymentMethods.paymentMethodsTypes.filter(
        (t: IPaymentMethodsTypes) => t.value == null
      ).length;

      if (
        qtdeValues == expense.value.paymentMethods.paymentMethodsTypes.length
      ) {
        dialogMessageText.value =
          "Informe os valores de pagamento da despesa por MÉTODO DE PAGAMENTO.";
        dialogNotify.value = true;
        return true;
      }
    }
  }

  return false;
}

function addInstallments() {
  let numberInstallments =
    expense.value.numberInstallments != null
      ? parseInt(expense.value.numberInstallments)
      : 1;

  let newDueDate = moment(date.value, "DD/MM/YYYY").toDate();
  let valueInstallments =
    parseFloat(valueExpense.value.replace(",", ".")) / numberInstallments;

  for (let i = 0; i < numberInstallments; i++) {
    let installment = {
      dueDate: newDueDate,
      dueDateFormatter: moment(newDueDate).format("DD/MM/YYYY").toString(),
      installment: i + 1,
      installmentDescription: (i + 1).toString() + " / " + numberInstallments,
      value: parseFloat(valueInstallments.toFixed(2)),
      valueFormatter: valueInstallments.toFixed(2),
      isPaid: expense.value.expenseStatus == "Pago" ? true : false,
      paymentDate: expense.value.expenseStatus == "Pago" ? new Date() : null,
    };

    let daysBetweenInstallments =
      expense.value.daysBetweenInstallments != null
        ? expense.value.daysBetweenInstallments
        : 0;

    newDueDate =
      expense.value.typeInstallment == "Em Dias"
        ? moment(newDueDate).add(daysBetweenInstallments, "days").toDate()
        : moment(newDueDate).add(1, "months").toDate();
    listExpenseInstallments.value.push(installment);
  }
}

async function onSave() {
  if (!validateFields()) {
    expense.value.idPaymentMethods =
      expense.value.paymentMethods != null
        ? expense.value.paymentMethods.id
        : 0;

    if (expense.value.idPaymentMethods == 0) {
      expense.value.paymentMethods = null;
    }

    if (expense.value.numberInstallments == "1") {
      addInstallments();
      expense.value.numberInstallments =
        expense.value.numberInstallments.toString();

       expense.value.expenseInstallments = listExpenseInstallments.value;
    }

    expense.value.amount = parseFloat(valueExpense.value.replace(",", "."));
    expense.value.idUser = userLogged.id;
    expense.value.idAccount = expense.value.account.id;
    expense.value.idExpenseCategory = expense.value.expenseCategory.id;
    expense.value.idTypePerson = expense.value.supplier.id;
    expense.value.expenseType = "F";
    expense.value.typeInstallment = null;
    expense.value.dueDate = moment(date.value, "DD/MM/YYYY").toDate();

    dialogLoading.value = true;
    expense.value.idUser = userLogged.id;

    let result = await Api.postResult<IResult>(
      "/Expense/" + userLogged.id,
      expense.value
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

    expense.value.idUser = userLogged.id;
    expense.value.idAccount = expense.value.account.id;
    expense.value.idExpenseCategory = expense.value.expenseCategory.id;
    expense.value.idTypePerson = expense.value.supplier.id;
    expense.value.dateUpdate = new Date();

    dialogLoading.value = true;
    expense.value.idUser = userLogged.id;

    let result = await Api.putResult<IResult>("/Expense", expense.value);
    dialogLoading.value = false;
    dialogMessage.value = true;
    dialogMessageTitle.value = result.data.sucesso ? "Informação" : "Erro";
    dialogMessageText.value =
      result.data.message != null ? result.data.message : "";
  }
}

async function showExpense() {
  if (route.params.id) {
    idExpense.value = route.params.id.toString();

    dialogLoading.value = true;
    let result: IResult = {} as IResult;
    result = await Api.getIdResult<IResult>(
      "/Expense/Edit",
      idExpense.value.toString()
    );

    console.log(result.data);

    if (result.data.sucesso) {
      expense.value = result.data.tRetorno;
      valueExpense.value = result.data.tRetorno.amount;
    }

    dialogLoading.value = false;
  }
}

async function showItemsPaymentMethods() {
  dialogLoading.value = true;
  let result: IResult;
  result = await Api.getResult<IResult>(
    "/PaymentMethods/Status/true/" + userLogged.id
  );

  if (result.data.sucesso) {
    listPaymentMethods.value = result.data.tRetorno;
    dialogLoading.value = false;
  }
}

async function showItemsExpenseCategory() {
  dialogLoading.value = true;
  let result: IResult;
  result = await Api.getResult<IResult>(
    "/ExpenseCategory/Status/true/" + userLogged.id
  );

  if (result.data.sucesso) {
    listExpenseCategory.value = result.data.tRetorno;
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

onMounted(() => {
  showItemsPaymentMethods();
  showItemsTypePerson();
  showItemsExpenseCategory();
  showItemsAccount();
  callItemsBreadCrumbs();
  showExpense();
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

.k-table-payment .q-table__top,
thead tr:first-child th {
  background-color: white !important;
  color: black !important;
}
</style>

<style lang="sass"></style>
