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

    <KNotify
      :dialog="dialogNotify"
      :text="dialogMessageText"
      :type="'W'"
      @kNotifyClose="closeNotify"
    />

    <KBreadCrumbs :data="dataBreadCrumbs" />
    <br />

    <!-- Filtros -->
    <q-dialog
      v-model="dialogFilters"
      full-height
      position="right"
      persistent
      :maximized="true"
    >
      <q-card class="column full-height" style="width: 400px">
        <q-card-section>
          <div class="text-h6">Filtros</div>
        </q-card-section>

        <q-card-section class="col q-pt-none">
          <div class="text-subtitle2">Busca de dados das Despesas</div>

          <br />
          <div class="row">
            <div
              class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
            >
              <q-select
                :options="listExpenseStatus"
                class="k-input-pa"
                dense
                outlined
                label="Situação da Despesa"
                v-model="expenseFilter.expenseStatus"
              />
            </div>
          </div>
          <div class="row">
            <div
              class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
            >
              <q-select
                :options="listExpenseCategory"
                class="k-input-pa"
                dense
                outlined
                label="Por Categoria"
                option-label="description"
                reactive-rules
                v-model="expenseFilter.expenseCategory"
              />
            </div>
          </div>
          <div class="row">
            <div
              class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
            >
              <q-select
                :options="listTypePerson"
                class="k-input-pa"
                dense
                outlined
                label="Por Fornecedor *"
                option-label="name"
                reactive-rules
                v-model="expenseFilter.supplier"
              />
            </div>
          </div>
          <div class="row">
            <div
              class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
            >
              <q-select
                :options="listAccount"
                class="k-input-pa"
                dense
                outlined
                label="Por Conta"
                option-label="name"
                reactive-rules
                v-model="expenseFilter.account"
              />
            </div>
          </div>

          <br />

          <div class="text-subtitle2">
            <q-checkbox
              v-model="filterPeriod"
              label="Busca por período de Vencimento"
            />
          </div>

          <div class="row">
            <div
              class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
            >
              <q-input
                label="Data Inicial"
                class="k-input-pa"
                outlined
                dense
                v-model="date"
                readonly
                :disable="!filterPeriod"
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
          </div>

          <div class="row">
            <div
              class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
            >
              <q-input
                label="Data Final"
                class="k-input-pa"
                outlined
                dense
                v-model="date2"
                readonly
                :disable="!filterPeriod"
              >
                <template v-slot:append>
                  <q-icon name="event" class="cursor-pointer">
                    <q-popup-proxy
                      cover
                      transition-show="scale"
                      transition-hide="scale"
                    >
                      <q-date mask="DD/MM/YYYY" v-model="date2">
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
          </div>
        </q-card-section>

        <q-card-actions align="center" class="bg-white text-teal">
          <KButtonCancel
            style="min-width: 48%"
            :text="'Voltar'"
            @click="cancelFilter()"
          />
          <KButton
            style="min-width: 48%"
            :text="'Filtrar'"
            @click="onFilter()"
          />
        </q-card-actions>
      </q-card>
    </q-dialog>

    <!-- Pagamento das Parcelas -->
    <q-dialog
      v-model="dialogPaymentInstallments"
      persistent
      :maximized="maximizedPayment"
      transition-show="slide-up"
      transition-hide="slide-down"
    >
      <q-card style="min-width: 190vh">
        <q-bar>
          <q-space />

          <q-btn
            dense
            flat
            icon="close"
            v-close-popup
            @click="cancelOnPayment()"
          >
            <q-tooltip class="bg-white text-primary"
              >Cancelar Pagamento</q-tooltip
            >
          </q-btn>
        </q-bar>
        <q-card-section>
          <div class="text-h6">Pagamento de Despesa</div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <div>Dados da Despesa</div>
          <br />

          <div class="row">
            <div
              class="col-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2"
              style="text-align: start; font-weight: bold"
            >
              Valor da Parcela:
              <span class="sub-title" style="font-size: 16px; color: #535bf2">
                <b>{{
                  "R$ " + parseFloat(expenseShow.valueInstallment).toFixed(2)
                }}</b>
              </span>
            </div>

            <div
              class="col-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2"
              style="text-align: start; font-weight: bold"
            >
              Parcela:
              <span class="sub-title" style="font-size: 16px; color: #535bf2">
                <b>{{ expenseShow.installmentDescription }}</b>
              </span>
            </div>

            <div
              class="col-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2"
              style="text-align: start; font-weight: bold"
            >
              Total da Despesa:
              <span class="sub-title">
                {{ "R$ " + parseFloat(expenseShow.amount).toFixed(2) }}
              </span>
            </div>

            <div
              class="col-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2"
              style="text-align: start; font-weight: bold"
            >
              Criada em::
              <span class="sub-title">
                {{ expenseShow.dateCreatedFormat }}
              </span>
            </div>
          </div>
          <br />

          <div class="row">
            <div
              class="col-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2"
              style="text-align: start; font-weight: bold"
            >
              Lançamento:
              <span class="sub-title">
                {{
                  expenseShow.expenseType == "F"
                    ? "Despesa Fixa"
                    : "Despesa Variável"
                }}
              </span>
            </div>
            <div
              class="col-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2"
              style="text-align: start; font-weight: bold"
            >
              Vencimento:
              <span class="sub-title">
                {{ expenseShow.dateCreatedFormat }}</span
              >
            </div>
            <div
              class="col-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2"
              style="text-align: start; font-weight: bold"
            >
              Categoria:
              <span class="sub-title">
                {{ expenseShow.expenseCategory.description }}</span
              >
            </div>
            <div
              class="col-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2"
              style="text-align: start; font-weight: bold"
            >
              Fornecedor:
              <span class="sub-title"> {{ expenseShow.supplier.name }}</span>
            </div>

            <div
              class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4 col-xxl-4"
              style="text-align: start; font-weight: bold"
              v-if="
                expenseShow.description != '' && expenseShow.description != null
              "
            >
              Descrição:
              <span class="sub-title"> {{ expenseShow.description }}</span>
            </div>
          </div>
          <br />
        </q-card-section>

        <q-separator />

        <q-card-section class="q-pt-none">
          <br />
          <div>Dados de Pagamento</div>

          <div class="row">
            <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4 col-xxl-4">
              <q-input
                label="Data de Pagamento"
                class="k-input-pa"
                filled
                v-model="datePayment"
              >
                <template v-slot:append>
                  <q-icon name="event" class="cursor-pointer">
                    <q-popup-proxy
                      cover
                      transition-show="scale"
                      transition-hide="scale"
                    >
                      <q-date mask="DD/MM/YYYY" v-model="datePayment">
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
            <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4 col-xxl-4">
              <q-select
                :options="listAccount"
                class="k-input-pa"
                filled
                label="Conta *"
                option-label="name"
                reactive-rules
                v-model="expensePayment.account"
              />
            </div>
            <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4 col-xxl-4">
              <q-select
                :options="listPaymentMethods"
                class="k-input-pa"
                filled
                label="Formas de Pagamento *"
                option-label="name"
                reactive-rules
                @update:model-value="(value) => onChangePaymentMethods(value)"
                v-model="expensePayment.paymentMethods"
              />
            </div>
          </div>
          <br />

          <div class="row" v-if="maximizedPayment">
            <div
              class="col-12 col-sm-8 col-md-8 col-lg-8 col-xl-8 col-xxl-8"
              style="text-align: start; font-weight: bold"
            >
              <div class="text-caption text-grey">
                Informe os valores pagos na despesa por Método de Pagamento
              </div>
            </div>
            <div
              class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4 col-xxl-4"
              style="font-weight: bold"
            >
              <div class="col-12 sub-title" style="float: right">
                <span>
                  Total do
                  <l style="font-size: 16px; font-weight: bold">Pagamento:</l>
                </span>
                <span class="sub-title" style="font-size: 18px; color: #535bf2">
                  <b>{{
                    " R$ " + parseFloat(expensePayment.amount).toFixed(2)
                  }}</b>
                </span>
              </div>
            </div>
          </div>
          <br />
          <div class="row" v-if="maximizedPayment">
            <div
              class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
            >
              <q-table
                :rows="expensePayment.paymentMethods.paymentMethodsTypes"
                hide-pagination
                :columns="headersPayment"
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
          </div>
        </q-card-section>

        <q-card-actions align="right" v-if="maximizedPayment">
          <KButtonCancel :text="'Cancelar'" @click="cancelOnPayment()" />
          <KButton @click="onPayment()" :text="'Pagar'" />
        </q-card-actions>
      </q-card>
    </q-dialog>

    <div class="row">
      <div
        class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3 col-xxl-3"
        style="text-align: start"
      >
        <h5 class="title" style="margin: 4px 0px">Despesas</h5>
        <div class="col-12 sub-title" style="text-align: start">
          Gerencie suas Despesas Variáveis ou Fixas
        </div>
      </div>

      <div
        class="col-12 col-sm-9 col-md-9 col-lg-9 col-xl-9 col-xxl-9"
        style="text-align: start"
      >
        <div class="row">
          <div class="col-12 col-sm-7 col-md-7 col-lg-7 col-xl-7 col-xxl-7">
            <q-input
              dense
              outlined
              label="Pesquisar por Código, Parcela, Datas ou Situação "
              style="width: 70%; float: right"
              v-model="filter"
            >
              <template v-slot:append>
                <q-icon name="search" />
              </template>
            </q-input>
          </div>

          <div class="col-12 col-sm-5 col-md-5 col-lg-5 col-xl-5 col-xxl-5">
            <div class="row">
              <div
                class="col-12 col-sm-12 col-md-5 col-lg-5 col-xl-5 col-xxl-5"
              >
                <div style="margin-left: 120px">
                  <KButton
                    text="Filtrar"
                    @click="showFilters"
                    title="Filtrar dados"
                    style="float: right"
                  ></KButton>
                </div>
              </div>
              <div
                class="col-12 col-sm-12 col-md-7 col-lg-7 col-xl-7 col-xxl-7"
              >
                <q-btn-dropdown
                  style="float: right"
                  color="secondary"
                  label="Novo Lançamento"
                >
                  <div class="q-pa-md" style="min-width: 300px">
                    <q-list>
                      <q-item clickable v-ripple>
                        <q-item-section @click="showNewFixedExpense()"
                          >Despesa Fixa</q-item-section
                        >
                        <q-item-section avatar>
                          <q-icon color="primary" name="paid" />
                        </q-item-section>
                      </q-item>
                    </q-list>
                    <q-separator />
                    <q-list>
                      <q-item clickable v-ripple>
                        <q-item-section @click="showNewVariableExpense()"
                          >Despesa Variável</q-item-section
                        >
                        <q-item-section avatar>
                          <q-icon color="primary" name="currency_exchange" />
                        </q-item-section>
                      </q-item>
                    </q-list>
                  </div>
                </q-btn-dropdown>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <br />

    <!-- cards -->
    <div class="row" v-if="items.sumExpense">
      <div class="col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3 col-xxl-3">
        <q-card flat bordered style="max-width: 98%">
          <q-card-section>
            <div style="text-align: start" class="text-h5 q-mt-sm q-mb-xs">
              {{ "R$ " + items.sumExpenseFixed.toFixed(2) }}
            </div>
            <div style="text-align: start" class="text-overline">
              Total de Despesas Fixas
            </div>
          </q-card-section>

          <div class="card-expense" style="background-color: #3f51b5"></div>
        </q-card>
      </div>
      <div class="col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3 col-xxl-3">
        <q-card flat bordered style="max-width: 98%">
          <q-card-section>
            <div style="text-align: start" class="text-h5 q-mt-sm q-mb-xs">
              {{ "R$ " + items.sumExpenseVariable.toFixed(2) }}
            </div>
            <div style="text-align: start" class="text-overline">
              Total de Despesas Variáveis
            </div>
          </q-card-section>

          <div class="card-expense" style="background-color: #2196f3"></div>
        </q-card>
      </div>
      <div class="col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3 col-xxl-3">
        <q-card flat bordered style="max-width: 98%">
          <q-card-section>
            <div style="text-align: start" class="text-h5 q-mt-sm q-mb-xs">
              {{ "R$ " + items.sumExpensePaid.toFixed(2) }}
            </div>
            <div style="text-align: start" class="text-overline">
              Total de Despesas Pagas
            </div>
          </q-card-section>

          <div class="card-expense" style="background-color: #1b5e20"></div>
        </q-card>
      </div>
      <div class="col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3 col-xxl-3">
        <q-card flat bordered style="max-width: 98%">
          <q-card-section>
            <div style="text-align: start" class="text-h5 q-mt-sm q-mb-xs">
              {{ "R$ " + items.sumExpenseUnpaid.toFixed(2) }}
            </div>
            <div style="text-align: start" class="text-overline">
              Total de Despesas A Pagar
            </div>
          </q-card-section>

          <div class="card-expense" style="background-color: #f57f17"></div>
        </q-card>
      </div>
    </div>
    <br />

    <div class="row">
      <div
        class="col-10 sub-title"
        style="text-align: start; font-weight: bold"
      >
        {{ "Período da Listagem por vencimento: " + date + " a " + date2 }}
      </div>
    </div>
    <br />
    <div class="row">
      <div class="col-12">
        <q-table
          :rows="items.expenses"
          :columns="headers"
          :filter="filter"
          :rows-per-page-options="[10]"
        >
          <template v-slot:body="props">
            <q-tr :props="props">
              <q-td v-for="col in props.cols" :key="col.name" :props="props">
                <template v-if="col.name == 'actions'">
                  <KButtonAction
                    v-if="!props.row.isPaid && props.row.isActive"
                    title="Pagar Despesa"
                    @click="showOnPayment(props.row)"
                    :icon="'payments'"
                  />
                  <KButtonAction
                    title="Editar despesa"
                    @click="onEdit(props.row)"
                    :icon="'edit'"
                  />
                  <KButtonAction
                    :title="
                      props.row.isActive
                        ? 'Desativar despesa'
                        : 'Ativar despesa'
                    "
                    :icon="props.row.isActive ? 'block' : 'refresh'"
                    @click="showActiveInactiveExpense(props.row)"
                  />
                </template>
                <template v-else-if="col.name == 'expenseType'">
                  <q-chip
                    square
                    style="
                      width: 150px;
                      background-color: #e6e6e6;
                      color: #777779;
                    "
                  >
                    <div class="col-12" align="left">
                      <span style="font-weight: bold">{{
                        col.value == "F" ? "Despesa Fixa" : "Despesa Variável"
                      }}</span>
                    </div>
                  </q-chip>
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
                        : 'background-color: #F6CFD2; color: #B6443F'
                    "
                  >
                    <div class="col-12" align="center">
                      <span style="font-weight: bold">{{
                        col.value ? "Ativo" : "Cancelado"
                      }}</span>
                    </div>
                  </q-chip>
                </template>
                <template v-else-if="col.name == 'expenseCategory'">
                  <q-avatar
                    size="sm"
                    :style="'background-color: ' + col.value.color"
                  />
                  <span style="margin-left: 20px">{{
                    col.value.description
                  }}</span>
                </template>

                <template v-else-if="col.name == 'value'">
                  <span style="font-size: 15px; font-weight: bold">{{
                    "R$ " + parseFloat(col.value).toFixed(2)
                  }}</span>
                </template>

                <template v-else-if="col.name == 'typePerson'">
                  {{ col.value.name }}
                </template>

                <template v-else-if="col.name == 'account'">
                  {{ col.value.name }}
                </template>
                <template v-else-if="col.name == 'expenseStatus'">
                  <span
                    :style="
                      col.value == 'A Pagar' ? 'color: orange' : 'color: blue'
                    "
                    >{{ col.value }}</span
                  >
                </template>
                <template v-else-if="col.name == 'valueInstallment'">
                  <span style="font-size: 15px; font-weight: bold">{{
                    "R$ " + parseFloat(col.value).toFixed(2)
                  }}</span>
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
import { IExpense } from "@/pages/Expense/Type/IExpense";
import { IExpensePayment } from "@/pages/Expense/Type/IExpensePayment";
import moment from "moment";
import { IAccount } from "../Administration/Account/Type/IAccount";
import { IExpenseCategory } from "../Administration/ExpenseCategory/Type/IExpenseCategory";
import { ITypePerson } from "../Administration/TypePerson/Type/ITypePerson";
import { IExpenseFilter } from "@/pages/Expense/Type/IExpenseFilter";
import KButtonCancel from "../../components/KButtonCancel.vue";
import { IPaymentMethods } from "../Administration/PaymentMethods/Type/IPaymentMethods";
import KInput from "@components/KInput.vue";
import { IPaymentMethodsTypes } from "../Administration/PaymentMethods/Type/IPaymentMethodsTypes";

const router = useRouter();
const userStore = storeUser();
const userLogged = userStore.getUser.user;

const date = ref(moment(new Date()).format("DD/MM/YYYY").toString());
const date2 = ref(moment(new Date()).format("DD/MM/YYYY").toString());
const datePayment = ref(moment(new Date()).format("DD/MM/YYYY").toString());

const listExpenseStatus = ref(["Todas", "A Pagar", "Pago"]);
const listAccount = ref([] as IAccount[]);
const listExpenseCategory = ref([] as IExpenseCategory[]);
const listTypePerson = ref([] as ITypePerson[]);

const dialogQuestion = ref(false);
const dialogMessage = ref(false);
const dialogMessageText = ref("");
const dialogMessageTitle = ref("");
const dialogLoading = ref(false);
const dialogNotify = ref(false);
const dialogFilters = ref(false);
const dialogPaymentInstallments = ref(false);
const maximizedPayment = ref(false);

const filterPeriod = ref(false);

const items = ref<Array<IExpense>>([]);

const expense = ref<IExpense>({} as IExpense);
const expenseShow = ref(new IExpense());
const expenseFilter = ref(new IExpenseFilter());
const listPaymentMethods = ref([] as IPaymentMethods[]);
const paymentMethods: IPaymentMethods | null = new IPaymentMethods();

const dataBreadCrumbs = ref([
  {
    label: "Lista de Despesas",
    router: "/listExpense",
  },
  // {
  //   label: "Nova Despesa",
  //   router: "/formExpenseFixed",
  // },
]);

const expensePayment = ref(new IExpensePayment());

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
    name: "expenseType",
    align: "left",
    label: "Tipo de Lançamento",
    field: "expenseType",
    sortable: false,
  },

  {
    name: "id",
    align: "left",
    label: "Cod. Despesa",
    field: "id",
    sortable: false,
  },

  {
    name: "installmentDescription",
    align: "left",
    label: "Parcela",
    field: "installmentDescription",
    sortable: false,
  },

  {
    name: "expenseCategory",
    align: "left",
    label: "Categoria",
    field: "expenseCategory",
    sortable: true,
  },
  {
    name: "account",
    align: "left",
    label: "Conta",
    field: "account",
    sortable: true,
  },

  {
    name: "typePerson",
    align: "left",
    label: "Fornecedor",
    field: "supplier",
    sortable: false,
  },

  {
    name: "dateCreatedFormat",
    align: "left",
    label: "Lançada em",
    field: "dateCreatedFormat",
    sortable: false,
  },

  {
    name: "dueDateFormat",
    align: "left",
    label: "Vencimento",
    field: "dueDateFormat",
    sortable: false,
  },

  {
    name: "paymentDateFormat",
    align: "left",
    label: "Data de Pagto.",
    field: "paymentDateFormat",
    sortable: false,
  },

  {
    name: "expenseStatus",
    align: "left",
    label: "Situação",
    field: "expenseStatus",
    sortable: false,
  },

  {
    name: "valueInstallment",
    align: "left",
    label: "Valor Parcela",
    field: "valueInstallment",
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

const headersPayment = ref([
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

function closeNotify() {
  dialogNotify.value = false;
}

function validatePayment() {
  if (expensePayment.value.account.id == 0) {
    dialogMessageText.value =
      "Selecione uma conta para associar ao pagamento da despesa.";
    dialogNotify.value = true;
    return true;
  }

  if (expensePayment.value.paymentMethods?.id == 0) {
    dialogMessageText.value = "Selecione o Método de Pagamento para a Despesa.";
    dialogNotify.value = true;
    return true;
  }

  if (expensePayment.value.paymentMethods.id != 0) {
    if (expensePayment.value.paymentMethods.paymentMethodsTypes.length > 0) {
      let qtdeValues =
        expensePayment.value.paymentMethods.paymentMethodsTypes.filter(
          (t: IPaymentMethodsTypes) => t.value == null
        ).length;

      if (
        qtdeValues ==
        expensePayment.value.paymentMethods.paymentMethodsTypes.length
      ) {
        dialogMessageText.value =
          "Informe os valores de pagamento da despesa por MÉTODO DE PAGAMENTO.";
        dialogNotify.value = true;
        return true;
      }
    }
  }

  if (expensePayment.value.amount < expenseShow.value.valueInstallment) {
    dialogMessageText.value =
      "O Total do Pagamento não pode ser inferior ao valor da Parcela (R$ " +
      expenseShow.value.valueInstallment.toFixed(2) +
      ").";
    dialogNotify.value = true;
    return true;
  }

  return false;
}

async function cancelOnPayment() {
  expenseShow.value = new IExpense();
  expensePayment.value = new IExpensePayment();
  expensePayment.value.paymentMethods = new IPaymentMethods();
  dialogPaymentInstallments.value = false;
  maximizedPayment.value = false;
}

async function onPayment() {
  if (!validatePayment()) {
    dialogLoading.value = true;
    expensePayment.value.paymentDate = moment(
      datePayment.value,
      "DD/MM/YYYY"
    ).toDate();
    expensePayment.value.idExpense = expenseShow.value.id;
    expensePayment.value.idInstallment = expenseShow.value.idInstallment;

    let result: IResult;
    result = await Api.postResult<IResult>(
      "/Expense/Payment",
      expensePayment.value
    );

    if (result.data.sucesso) {
      dialogLoading.value = false;
      dialogLoading.value = false;
      dialogMessage.value = true;
      dialogMessageTitle.value = result.data.sucesso ? "Informação" : "Erro";
      dialogMessageText.value =
        result.data.message != null ? result.data.message : "";

      dialogPaymentInstallments.value = false;
      onFilter();
    }
  }
}

async function showOnPayment(expense: IExpense) {
  cancelOnPayment();
  if (listAccount.value.length == 0) {
    await showItemsAccount();
  }

  if (listPaymentMethods.value.length == 0) {
    await showItemsPaymentMethods();
  }

  expenseShow.value = expense;
  dialogPaymentInstallments.value = true;
}

function closeDialogMessage() {
  dialogMessage.value = false;
}

function showActiveInactiveExpense(item: IExpense) {
  expense.value = item;

  dialogMessageTitle.value = item.isActive
    ? "Desativar Despesa"
    : "Ativar Despesa";
  dialogMessageText.value = item.isActive
    ? "Deseja desativar a Despesa selecionada?"
    : "Deseja ativar a Despesa selecionada?";

  dialogQuestion.value = true;
}

function onChangeSumPaymentMethods(value: number) {
  let sumTotalExpense = 0;

  if (expensePayment.value.paymentMethods != null) {
    expensePayment.value.paymentMethods.paymentMethodsTypes.forEach(
      (item: IPaymentMethodsTypes) => {
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
      }
    );

    expensePayment.value.amount = JSON.parse(JSON.stringify(sumTotalExpense));
  }
}

function onChangePaymentMethods(item: IPaymentMethods) {
  if (item.paymentMethodsTypes.length > 0) {
    maximizedPayment.value = true;
  } else {
    maximizedPayment.value = false;
  }
}

function callActiveInactiveYes() {
  //console.log('teste')
  onActiveInactiveExpense(expense.value);
  dialogQuestion.value = false;
  dialogMessage.value = false;
}

function callActiveInactiveNo() {
  dialogQuestion.value = false;
  expense.value = {} as IExpense;
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

async function showItemsExpense() {
  dialogLoading.value = true;
  let result: IResult;
  result = await Api.getResult<IResult>("/Expense/" + userLogged.id);

  if (result.data.sucesso) {
    items.value = result.data.tRetorno;
    dialogLoading.value = false;
  }
}

async function onActiveInactiveExpense(expense: IExpense) {
  dialogLoading.value = true;
  expense.dateUpdate = new Date();
  expense.isActive = !expense.isActive;
  let result = await Api.deleteResult<IResult>("/Expense", expense.id);

  dialogLoading.value = false;
  dialogMessage.value = true;
  dialogMessageTitle.value = result.data.sucesso ? "Informação" : "Erro";
  dialogMessageText.value =
    result.data.message != null ? result.data.message : "";
}

function showNewFixedExpense() {
  router.push("/formExpenseFixed");
}

function showNewVariableExpense() {
  router.push("/formExpenseVariable");
}

function onEdit(item: any) {
  //console.log(item);

  if (item.expenseType == "V") {
    router.push("/editExpenseVariable/" + item.id);
  }

  if (item.expenseType == "F") {
    router.push("/editExpenseFixed/" + item.id);
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

async function onFilter() {
  expenseFilter.value.dateInitial = filterPeriod.value
    ? moment(date.value, "DD/MM/YYYY").toDate()
    : null;
  expenseFilter.value.dateFinal = filterPeriod.value
    ? moment(date2.value, "DD/MM/YYYY").toDate()
    : null;

  if (expenseFilter.value.expenseCategory?.id == 0) {
    expenseFilter.value.expenseCategory = null;
  }
  if (expenseFilter.value.supplier?.id == 0) {
    expenseFilter.value.supplier = null;
  }
  if (expenseFilter.value.account?.id == 0) {
    expenseFilter.value.account = null;
  }

  if (
    expenseFilter.value.expenseCategory == null &&
    expenseFilter.value.supplier == null &&
    expenseFilter.value.account == null &&
    !filterPeriod.value &&
    expenseFilter.value.expenseStatus == "Todas"
  ) {
    dialogFilters.value = false;
    expenseFilter.value.dateInitial = moment(date.value, "DD/MM/YYYY").toDate();
    expenseFilter.value.dateFinal = moment(date2.value, "DD/MM/YYYY").toDate();
  }

  dialogLoading.value = true;
  let result = await Api.postResult<IResult>(
    "/Expense/Filters/" + userLogged.id,
    expenseFilter.value
  );

  if (result.data.sucesso) {
    items.value = result.data.tRetorno;
    dialogFilters.value = false;
  } else {
    dialogMessage.value = true;
    dialogMessageTitle.value = result.data.sucesso ? "Informação" : "Erro";
    dialogMessageText.value =
      result.data.message != null ? result.data.message : "";
  }

  dialogLoading.value = false;
}

async function showFilters() {
  expenseFilter.value = new IExpenseFilter();
  if (listAccount.value.length == 0) {
    await showItemsAccount();
  }

  if (listExpenseCategory.value.length == 0) {
    await showItemsExpenseCategory();
  }

  if (listTypePerson.value.length == 0) {
    await showItemsTypePerson();
  }

  if (
    listAccount.value.length > 0 &&
    listExpenseCategory.value.length > 0 &&
    listTypePerson.value.length > 0
  ) {
    filterPeriod.value = false;
    dialogFilters.value = true;
  }
}

function cancelFilter() {
  expenseFilter.value = new IExpenseFilter();
  dialogFilters.value = false;
}

onMounted(() => {
  date.value = moment(new Date()).format("01/MM/YYYY").toString();
  date2.value = moment(new Date())
    .endOf("month")
    .format("DD/MM/YYYY")
    .toString();
  onFilter();
  //showItemsExpense();
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

.card-expense {
  min-height: 5px;
}
</style>
