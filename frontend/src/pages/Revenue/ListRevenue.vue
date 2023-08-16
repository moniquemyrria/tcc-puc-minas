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
          <div class="text-subtitle2">Busca de dados das Receitas</div>

          <br />
          <div class="row">
            <div
              class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
            >
              <q-select
                :options="listRevenueStatus"
                class="k-input-pa"
                dense
                outlined
                label="Situação da Receita"
                v-model="revenueFilter.revenueStatus"
              />
            </div>
          </div>
          <div class="row">
            <div
              class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
            >
              <q-select
                :options="listRevenueCategory"
                class="k-input-pa"
                dense
                outlined
                label="Por Categoria"
                option-label="description"
                v-model="revenueFilter.revenueCategory"
                reactive-rules
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
                v-model="revenueFilter.supplier"
                reactive-rules
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
                v-model="revenueFilter.account"
                reactive-rules
              />
            </div>
          </div>
          <div class="row">
            <div
              class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
            >
              <q-select
                :options="listRevenueReceipt"
                class="k-input-pa"
                dense
                outlined
                label="Por Forma de Recebimento"
                v-model="revenueFilter.revenueReceipt"
                reactive-rules
              />
            </div>
          </div>

          <br />

          <div class="text-subtitle2">
            <q-checkbox
              v-model="filterPeriod"
              label="Busca por período de Data da Receita"
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

    <div class="row">
      <div
        class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3 col-xxl-3"
        style="text-align: start"
      >
        <h5 class="title" style="margin: 4px 0px">Receitas</h5>
        <div class="col-12 sub-title" style="text-align: start">
          Gerencie suas Receitas e proventos
        </div>
      </div>

      <div
        class="col-12 col-sm-9 col-md-9 col-lg-9 col-xl-9 col-xxl-9"
        style="text-align: start"
      >
        <div class="row">
          <div class="col-12 col-sm-8 col-md-8 col-lg-8 col-xl-8 col-xxl-8">
            <q-input
              dense
              outlined
              label="Pesquisa por Situação, Data de Recebimento ou Valor"
              v-model="filter"
              style="float: right; width: 64%"
            >
              <template v-slot:append>
                <q-icon name="search" />
              </template>
            </q-input>
          </div>

          <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4 col-xxl-4">
            <div class="row">
              <div
                class="col-12 col-sm-12 col-md-5 col-lg-5 col-xl-5 col-xxl-5"
              >
                <KButton
                  text="Filtrar"
                  @click="showFilters"
                  title="Filtrar dados"
                  style="float: right"
                ></KButton>
              </div>
              <div
                class="col-12 col-sm-12 col-md-7 col-lg-7 col-xl-7 col-xxl-7"
              >
                <KButton
                  style="float: right"
                  text="Novo Lançamento"
                  @click="showNewItem"
                  title="Nova Receita"
                ></KButton>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <br />
    <div class="row">
      <div class="col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3 col-xxl-3"></div>
      <div class="col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3 col-xxl-3"></div>
      <div class="col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3 col-xxl-3"></div>
      <div class="col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3 col-xxl-3">
        <q-card flat bordered style="max-width: 98%">
          <q-card-section>
            <div style="text-align: start" class="text-h5 q-mt-sm q-mb-xs">
              {{
                "R$ " +
                parseFloat(
                  items.length > 0 ? items[0].totalSumRevenue : 0
                ).toFixed(2)
              }}
            </div>
            <div style="text-align: start" class="text-overline">
              Total de Receitas Ativas
            </div>
          </q-card-section>

          <div class="card-revenue" style="background-color: #3f51b5"></div>
        </q-card>
      </div>
    </div>

    <br />

    <!-- <div v-else><br /><br /></div> -->

    <div class="row">
      <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12">
        <div class="row">
          <div
            class="col-10 sub-title"
            style="text-align: start; font-weight: bold"
          >
            {{ "Período da Listagem: " + date + " a " + date2 }}
          </div>
        </div>
      </div>
    </div>

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
                    title="Editar Receita"
                    @click="onEdit(props.row)"
                    :icon="'edit'"
                  />
                  <KButtonAction
                    :title="
                      props.row.isActive ? 'Cancelar receita' : 'Ativar receita'
                    "
                    :icon="props.row.isActive ? 'block' : 'refresh'"
                    @click="showActiveInactiveRevenue(props.row)"
                  />
                </template>

                <template v-else-if="col.name == 'revenueCategory'">
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

                <template v-else-if="col.name == 'colorRevenueCategory'">
                  <q-avatar
                    size="sm"
                    :style="'background-color: ' + col.value"
                  />
                </template>

                <template v-else-if="col.name == 'revenueStatus'">
                  <span
                    :style="
                      col.value == 'A Receber' ? 'color: orange' : 'color: blue'
                    "
                    >{{ col.value }}</span
                  >
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
import KButtonCancel from "../../components/KButtonCancel.vue";
import { IResult } from "@/core/types/IKResult";
import KDialogQuestion from "@/components/KDialogQuestion.vue";
import { storeUser } from "@/core/store/userStore";
import { IRevenue } from "@/pages/Revenue/Type/IRevenue";
import { IRevenueFilter } from "@/pages/Revenue/Type/IRevenueFilter";
import { IAccount } from "../Administration/Account/Type/IAccount";
import { IRevenueCategory } from "../Administration/RevenueCategory/Type/IRevenueCategory";
import { ITypePerson } from "../Administration/TypePerson/Type/ITypePerson";
import moment from "moment";

const router = useRouter();
const userStore = storeUser();
const userLogged = userStore.getUser.user;

const date = ref(moment(new Date()).format("DD/MM/YYYY").toString());
const date2 = ref(moment(new Date()).format("DD/MM/YYYY").toString());

const dialogQuestion = ref(false);
const dialogMessage = ref(false);
const dialogMessageText = ref("");
const dialogMessageTitle = ref("");
const dialogLoading = ref(false);
const dialogNotify = ref(false);
const dialogFilters = ref(false);

const items = ref<Array<IRevenue>>([]);

const revenue = ref<IRevenue>({} as IRevenue);
const revenueFilter = ref(new IRevenueFilter());

const listAccount = ref([] as IAccount[]);
const listRevenueCategory = ref([] as IRevenueCategory[]);
const listTypePerson = ref([] as ITypePerson[]);

const listRevenueReceipt = ref([
  "Pix",
  "Dinheiro",
  "Boleto",
  "Cheque",
  "Depósito Bancário",
  "Transferência Bancária",
  "Não Especificado",
]);

const listRevenueStatus = ref(["Todas", "Recebido", "A Receber"]);

const dataBreadCrumbs = ref([
  {
    label: "Lista de Receitas",
    router: "/listRevenue",
  },
  {
    label: "Nova Receita",
    router: "/formRevenue",
  },
]);

const filter = ref("");
const filterPeriod = ref(false);

const headers = ref([
  {
    name: "isActive",
    align: "left",
    label: "Status",
    field: "isActive",
    sortable: false,
  },

  {
    name: "revenueCategory",
    align: "left",
    label: "Categoria",
    field: "revenueCategory",
    sortable: false,
  },
  {
    name: "account",
    align: "left",
    label: "Conta",
    field: "account",
    sortable: false,
  },
  {
    name: "typePerson",
    align: "left",
    label: "Fornecedor",
    field: "supplier",
    sortable: false,
  },
  {
    name: "revenueReceipt",
    align: "left",
    label: "Forma de Recebimento",
    field: "revenueReceipt",
    sortable: true,
  },
  // {
  //   name: "description",
  //   align: "left",
  //   label: "Descrição",
  //   field: "description",
  //   sortable: true,
  // },

  {
    name: "dateCreatedFormat",
    align: "left",
    label: "Data Recebimento",
    field: "dateCreatedFormat",
    sortable: true,
  },

  {
    name: "revenueStatus",
    align: "left",
    label: "Situação",
    field: "revenueStatus",
    sortable: false,
  },

  {
    name: "value",
    align: "left",
    label: "Valor",
    field: "value",
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

async function onFilter() {
  revenueFilter.value.dateInitial = filterPeriod.value
    ? moment(date.value, "DD/MM/YYYY").toDate()
    : null;
  revenueFilter.value.dateFinal = filterPeriod.value
    ? moment(date2.value, "DD/MM/YYYY").toDate()
    : null;

  if (revenueFilter.value.revenueCategory?.id == 0) {
    revenueFilter.value.revenueCategory = null;
  }
  if (revenueFilter.value.supplier?.id == 0) {
    revenueFilter.value.supplier = null;
  }
  if (revenueFilter.value.account?.id == 0) {
    revenueFilter.value.account = null;
  }

  if (
    revenueFilter.value.revenueCategory == null &&
    revenueFilter.value.supplier == null &&
    revenueFilter.value.account == null &&
    revenueFilter.value.revenueReceipt == "" &&
    !filterPeriod.value &&
    revenueFilter.value.revenueStatus == "Todas"
  ) {
    dialogFilters.value = false;
    showItemsRevenue();
    return;
  }

  dialogLoading.value = true;
  let result = await Api.postResult<IResult>(
    "/Revenue/Filters/" + userLogged.id,
    revenueFilter.value
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

function cancelFilter() {
  revenueFilter.value = new IRevenueFilter();
  dialogFilters.value = false;
}

async function showFilters() {
  revenueFilter.value = new IRevenueFilter();
  if (listAccount.value.length == 0) {
    await showItemsAccount();
  }

  if (listRevenueCategory.value.length == 0) {
    await showItemsRevenueCategory();
  }

  if (listTypePerson.value.length == 0) {
    await showItemsTypePerson();
  }

  if (
    listAccount.value.length > 0 &&
    listRevenueCategory.value.length > 0 &&
    listTypePerson.value.length > 0
  ) {
    filterPeriod.value = false;
    dialogFilters.value = true;
  }
}

function closeDialogMessage() {
  dialogMessage.value = false;
}

function showActiveInactiveRevenue(item: IRevenue) {
  revenue.value = item;

  dialogMessageTitle.value = item.isActive
    ? "Cancelar Receita"
    : "Ativar Receita";
  dialogMessageText.value = item.isActive
    ? "Deseja cancelar a Receita selecionada?"
    : "Deseja ativar a Receita selecionada?";

  dialogQuestion.value = true;
}

function callActiveInactiveYes() {
  //console.log('teste')
  onActiveInactiveRevenue(revenue.value);
  dialogQuestion.value = false;
  dialogMessage.value = false;
}

function callActiveInactiveNo() {
  dialogQuestion.value = false;
  revenue.value = {} as IRevenue;
}

async function showItemsRevenue() {
  dialogLoading.value = true;
  let result: IResult;
  result = await Api.getResult<IResult>("/Revenue/" + userLogged.id);

  if (result.data.sucesso) {
    items.value = result.data.tRetorno;
    dialogLoading.value = false;
  }
}

async function onActiveInactiveRevenue(revenue: IRevenue) {
  dialogLoading.value = true;
  revenue.dateUpdate = new Date();
  revenue.isActive = !revenue.isActive;
  let result = await Api.deleteResult<IResult>("/Revenue", revenue.id);

  dialogLoading.value = false;
  dialogMessage.value = true;
  dialogMessageTitle.value = result.data.sucesso ? "Informação" : "Erro";
  dialogMessageText.value =
    result.data.message != null ? result.data.message : "";

  if (result.data.sucesso) {
    onFilter();
  }
}

function showNewItem() {
  router.push("/formRevenue");
}

function onEdit(item: any) {
  //console.log(item);
  router.push("/editRevenue/" + item.id);
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
  filterPeriod.value = true;
  date.value = moment(new Date()).format("01/MM/YYYY").toString();
  date2.value = moment(new Date())
    .endOf("month")
    .format("DD/MM/YYYY")
    .toString();
  onFilter();
  //showItemsRevenue();
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

.card-revenue {
  min-height: 5px;
}
</style>
