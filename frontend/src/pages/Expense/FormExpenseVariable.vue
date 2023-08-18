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

    <!-- PAYMENT METHODS -->
    <q-dialog v-model="dialogPayment" persistent>
      <q-card style="min-width: 1000px">
        <q-card-section>
          <div class="text-h6">Métodos de Pagamentos da Despesa Variável</div>
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
              label="Valor Total Pago (R$)"
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
              :columns="headersPaymentMethods"
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

    <!-- INSTALLMENTS -->
    <q-dialog v-model="dialogInstallments" persistent>
      <q-card style="min-width: 1000px">
        <q-card-section>
          <div class="text-h6">Parcelas da Despesa</div>
          <div class="text-caption text-grey">
            {{ "Tipo de parcelamento: " + expense.typeInstallment }}<br />
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <div class="row">
            <div class="col-8 col-sm-8 col-md-8 col-lg-8 col-xl-8 col-xxl-8">
              <br />
              <div class="text-caption text-grey">
                <span
                  >Vencimento da 1º Parcela:
                  <b style="color: #0D47A1">{{
                    listExpenseInstallments[0].dueDateFormatter
                  }}</b></span
                >
                <br />
                <span
                  >Vencimento da última Parcela:
                  <b style="color: #0D47A1">{{
                    listExpenseInstallments[listExpenseInstallments.length - 1]
                      .dueDateFormatter
                  }}</b>
                </span>
              </div>
            </div>
            <div class="col-4 col-sm-4 col-md-4 col-lg-4 col-xl-4 col-xxl-4">
              <q-input
                filled
                class="k-input-pa"
                label="Valor da Despesa (R$) *"
                type="number"
                prefix="R$"
                v-model="valueExpense"
                lazy-rules
                placeholder="0.00"
                readonly
              >
                <template v-slot:append> </template>
              </q-input>
            </div>
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <div
            class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
          >
            <q-table
              :rows="listExpenseInstallments"
              :columns="headers"
              :rows-per-page-options="[5]"
            >
              <template v-slot:body-cell-isPaid="props">
                <q-td :props="props">
                  <q-chip
                    square
                    style="min-width: 100px"
                    :style="
                      props.row.isPaid
                        ? 'background-color: #E3F2FD; color: #0D47A1'
                        : 'background-color: #FFEBEE; color: #B71C1C'
                    "
                  >
                    <div class="col-12" align="center">
                      <span style="font-weight: bold">{{
                        props.row.isPaid ? "Pago" : "A Pagar"
                      }}</span>
                    </div>
                  </q-chip>
                </q-td>
              </template>
            </q-table>
          </div>
        </q-card-section>

        <q-card-actions align="right">
          <div class="row"></div>
          <KButtonCancel text="Voltar" @click="cancelShowInstallments()" />
        </q-card-actions>
      </q-card>
    </q-dialog>

    <br />

    <div class="row">
      <div class="col-12" style="text-align: start">
        <h5 style="margin: 4px 0px 20px" class="title">
          {{
            idExpense == ""
              ? "Nova Despesa Variável"
              : "Editar Despesa Variável"
          }}
        </h5>
        <div class="col-12 sub-title" style="text-align: start">
          {{
            idExpense !== ""
              ? "Altere os campos disponíveis da Despesa Variável"
              : "Preencha os campos para criar uma nova Despesa Variável"
          }}
        </div>
      </div>
    </div>
    <br />

    <KCardForm>
      <q-card-section class="q-pt-none" align="center">
        <q-form class="q-gutter-md">
          <!-- <div class="row">
            <div class="col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3 col-xxl-3">
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
                    'Valor Inválido',
                ]"
                placeholder="0.00"
                readonly
              >
                <template v-slot:append> </template>
              </q-input>
            </div>
          </div>

          <q-separator></q-separator> -->

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
                :options="listTypePerson"
                class="k-input-pa"
                filled
                label="Fornecedor *"
                option-label="name"
                reactive-rules
                v-model="expense.supplier"
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
          </div>

          <q-separator></q-separator>

          <div
            class="row"
            v-if="
              expense.numberInstallments != null &&
              expense.numberInstallments > 1
            "
          >
            <q-btn
              v-if="idExpense != ''"
              label="Visualizar Parcelas"
              @click="showViewInstallments()"
              unelevated
              rounded
              color="secondary"
              title="Visualizar Parcelas da despesa"
              icon="visibility"
              size="md"
              style="width: 280px"
            />

            <q-btn
              v-if="idExpense == ''"
              label="Parcelas"
              @click="addInstallments(true)"
              unelevated
              rounded
              color="secondary"
              title="Parcelas da despesa"
              icon="add"
              size="md"
              style="width: 280px"
            />

            <!-- <q-btn
              v-if="idExpense == ''"
              outline
              @click="deleteInstallments()"
              label="Limpar Parcelas"
              unelevated
              rounded
              color="secondary"
              title="Limpar dados de Parcelamento"
              icon="delete_outline"
              size="md"
              style="margin-left: 10px; width: 280px"
            /> -->
          </div>

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
                label="Nº de Parcelas"
                type="number"
                v-model="expense.numberInstallments"
                lazy-rules
                :rules="[(val) => (val > 0 && val !== 0) || 'Valor inválido']"
                placeholder="0"
                @update:model-value="
                  (value) => onChangeNumberInstallments(value)
                "
                :readonly="idExpense != ''"
              >
                <template v-slot:append> </template>
              </q-input>
            </div>

            <div
              class="col-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2"
              v-if="
                expense.numberInstallments != null &&
                expense.numberInstallments > 1
              "
            >
              <q-select
                :options="listInstallmentTypes"
                class="k-input-pa"
                filled
                label="Tipo de Parcelamento"
                v-model="expense.typeInstallment"
                :readonly="idExpense != ''"
              />
            </div>

            <div
              class="col-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2"
              v-if="
                expense.typeInstallment == 'Em Dias' &&
                expense.numberInstallments != null &&
                expense.numberInstallments > 1
              "
            >
              <q-input
                filled
                class="k-input-pa"
                label="Intervalo em dias"
                placeholder="Ex. 30 dias"
                type="number"
                v-model="expense.daysBetweenInstallments"
                lazy-rules
                :rules="[(val) => (val > 0 && val !== 0) || 'Valor inválido']"
                :readonly="idExpense != ''"
              >
                <template v-slot:append> </template>
              </q-input>
            </div>

            <div class="col-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2">
              <q-input
                filled
                class="k-input-pa"
                label="Valor Total da Despesa (R$) *"
                type="number"
                prefix="R$"
                v-model="valueExpense"
                lazy-rules
                :rules="[(val) => (val > 0 && val !== 0) || 'Valor Inválido']"
                placeholder="0.00"
                :readonly="idExpense != ''"
              >
                <template v-slot:append> </template>
              </q-input>
            </div>
          </div>

          <q-separator />

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
              style="width: 280px"
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
                class="k-input-pa"
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
                :readonly="idExpense != ''"
                :options="listPaymentMethods"
                class="k-input-pa"
                filled
                label="Formas de Pagamento *"
                option-label="name"
                reactive-rules
                v-model="expense.paymentMethods"
                @update:model-value="(value) => onChangePaymentMethods(value)"
              />
            </div>
          </div>

          <!-- <div
            class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
          >
            <div class="row">
              <span
                >Dados de
                <l style="font-weight: bold" class="text-primary"
                  >Parcelamento</l
                ></span
              >
            </div>
            <br />
            <div class="row">
              <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4 col-xxl-4">
               
              </div>
            </div>
            <q-separator />
          </div> -->

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

          <div class="row" style="text-align: start">
            <div
              class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
            >
             <div class="text-grey">* Ao insirir uma despesa como "paga", automaticamente definiremos o nº de parcela como 1.</div>
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
const listInstallmentTypes = ref(["Em Dias", "Em Meses"]);
const listExpenseCategory = ref([] as IExpenseCategory[]);
const listAccount = ref([] as IAccount[]);
const listTypePerson = ref([] as ITypePerson[]);
const listPaymentMethods = ref([] as IPaymentMethods[]);

const userStore = storeUser();
const userLogged = userStore.getUser.user;
const listExpenseInstallments: any = ref([] as IExpenseInstallments[]);

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
const dialogInstallments = ref(false);
const dialogPayment = ref(false);
const valueExpense = ref("");

const dataBreadCrumbs = ref([
  {
    label: "Lista de Despesas",
    router: "/listExpense",
  },
]);

const headers = ref([
  {
    name: "isPaid",
    align: "center",
    label: "Status",
    field: "isPaid",
    sortable: false,
  },

  {
    name: "dueDateFormatter",
    align: "center",
    label: "Data de Vencimento",
    field: "dueDateFormatter",
    sortable: true,
  },

  {
    name: "installmentDescription",
    align: "center",
    label: "Parcelas",
    field: "installmentDescription",
    sortable: false,
  },

  {
    name: "valueFormatter",
    align: "center",
    label: "Valor Parcela (R$)",
    field: "valueFormatter",
    sortable: false,
  },

  {
    name: "paymentDateFormatter",
    align: "center",
    label: "Data de Pagamento",
    field: "paymentDateFormatter",
    sortable: false,
  },
]);

const headersPaymentMethods = ref([
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

function showViewPaymentMethods() {
  dialogPayment.value = true;
}

function showViewInstallments() {
  listExpenseInstallments.value = expense.value.expenseInstallments;
  dialogInstallments.value = true;
}

function onChangeNumberInstallments(value: number) {
  if (value > 1) {
    expense.value.expenseStatus = "A Pagar";
  }
}

function showPaymentMethods() {
  dialogPayment.value = true;
}

function cancelShowPayment() {
  dialogPayment.value = false;
}

function cancelShowInstallments() {
  dialogInstallments.value = false;
}

function deleteInstallments() {
  listExpenseInstallments.value = JSON.parse(JSON.stringify([]));
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

function validateInstallments() {
  if (valueExpense.value == "") {
    dialogMessageText.value = "Informe o valor da despesa.";
    dialogNotify.value = true;
    return true;
  }

  return false;
}

function addInstallments(onShowDialog: boolean) {
  if (!validateInstallments()) {
    deleteInstallments();

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

    if (onShowDialog) {
      dialogInstallments.value = true;
    }
  }
}

// function onChangeSumPaymentMethods(value: number) {
//   let sum = 0;

//   expense.value.paymentMethods.paymentMethodsTypes.forEach((item) => {
//     if (item.value != null && item.value.toString() != "") {
//       sum = sum + parseFloat(item.value.toString().replace(",", "."));
//     }
//   });

//   expense.value.amount = JSON.parse(JSON.stringify(sum));
//   valueExpense.value = expense.value.amount.toString();
// }

function onChangePaymentMethods(value: number) {
  if (expense.value.expenseStatus == "Pago"){
    expense.value.numberInstallments = '1'
    expense.value.daysBetweenInstallments = null
  }
  
  expense.value.amount = 0;
  //valueExpense.value = "";

  if (
    expense.value.expenseStatus !== "Pago" &&
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
      "Selecione um Fornecedor para a despesa.";
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

  // if (
  //   expense.value.numberInstallments != null &&
  //   expense.value.numberInstallments > "1" &&
  //   listExpenseInstallments.value.length == 0
  // ) {
  //   dialogMessageText.value =
  //     "É necessário inserir as Parcelas no botão de opção ADICIONAR PARCELAS";
  //   dialogNotify.value = true;
  //   return true;
  // }

  if (valueExpense.value == "0" || valueExpense.value == "") {
    dialogMessageText.value = "Informe o valor da Despesa";
    dialogNotify.value = true;
    return true;
  }

  return false;
}

async function onSave() {
  if (!validateFields()) {
    if (expense.value.numberInstallments == "1") {
      addInstallments(false);
      expense.value.numberInstallments =
        expense.value.numberInstallments.toString();
    } else {
      addInstallments(false);
    }

    expense.value.amount = parseFloat(valueExpense.value.replace(",", "."));
    expense.value.expenseInstallments = listExpenseInstallments.value;
    expense.value.idUser = userLogged.id;
    expense.value.idAccount = expense.value.account.id;
    expense.value.idExpenseCategory = expense.value.expenseCategory.id;
    expense.value.idPaymentMethods =
      expense.value.paymentMethods != null
        ? expense.value.paymentMethods.id
        : 0;
    expense.value.idTypePerson = expense.value.supplier.id;
    expense.value.expenseType = "V";
    expense.value.dueDate = moment(date.value, "DD/MM/YYYY").toDate();
    if (expense.value.idPaymentMethods == 0) {
      expense.value.paymentMethods = null;
    }

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
</style>
