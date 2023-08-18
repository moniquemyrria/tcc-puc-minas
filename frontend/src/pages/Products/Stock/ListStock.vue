<template>
  <div>
    <KDialog
      :title="dialogMessageTitle"
      :dialog="dialogMessage"
      :text="dialogMessageText"
      @kDialogClose="closeDialogMessage"
    />
    <KLoading :title="'Carregando Dados... Aguarde!'" :dialog="dialogLoading" />

    <KNotify
      :dialog="dialogNotify"
      :text="dialogMessageText"
      :type="dialogMessageType"
      @kNotifyClose="closeNotify"
    />

    <KBreadCrumbs :data="dataBreadCrumbs" />
    <br />

    <q-dialog v-model="dialogFilters" persistent>
      <q-card style="min-width: 1000px">
        <q-card-section>
          <div class="text-h6">Filtros</div>
          <div class="text-caption text-grey">
            Utilize os campos abaixo para filtrar os dados de estoque
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <div class="row">
            <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3 col-xxl-3">
              <k-input
                v-model="filterFields.code"
                dense
                outlined
                label="Código"
                lazy-rules
                maxlength="50"
                autofocus
              />
            </div>
            <div class="col-12 col-sm-2 col-md-2 col-lg-2 col-xl-2 col-xxl-2">
              <k-input
                v-model="filterFields.filial"
                dense
                outlined
                label="Filial"
                lazy-rules
                maxlength="2"
                autofocus
              />
            </div>
            <div class="col-12 col-sm-2 col-md-2 col-lg-2 col-xl-2 col-xxl-2">
              <k-input
                v-model="filterFields.local"
                dense
                outlined
                label="Local"
                lazy-rules
                maxlength="2"
                autofocus
              />
            </div>

            <div class="col-12 col-sm-5 col-md-5 col-lg-5 col-xl-5 col-xxl-5">
              <k-input
                v-model="filterFields.description"
                dense
                outlined
                label="Descrição"
                lazy-rules
                maxlength="100"
                autofocus
              />
            </div>
          </div>

          <div class="row">
            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
              <q-select
                :options="listStocksGroups"
                option-label="description"
                class="k-input-pa"
                dense
                outlined
                label="Grupo"
                v-model="filterFields.group"
                reactive-rules
              />
            </div>
            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
              <q-select
                :options="listStocksCategory"
                option-label="description"
                class="k-input-pa"
                dense
                outlined
                label="Categoria Marg"
                v-model="filterFields.category"
                reactive-rules
              />
            </div>
          </div>
        </q-card-section>

        <q-card-actions align="right">
          <div class="row"></div>
          <KButtonCancel @click="cancelFilter()" :text="'Cancelar'" />

          <KButton @click="callFilter()" :text="'Filtrar'" />
        </q-card-actions>
      </q-card>
    </q-dialog>

    <div class="row">
      <div
        class="col-12 col-sm-7 col-md-7 col-lg-7 col-xl-7 col-xxl-7"
        style="text-align: start"
      >
        <h5 class="title" style="margin: 4px 0px">Estoque Disponível</h5>
        <div class="col-12 sub-title" style="text-align: start">
          Acompanhe os dados de estoque dos produtos
        </div>
      </div>

      <div
        class="col-12 col-sm-5 col-md-5 col-lg-5 col-xl-5 col-xxl-5"
        style="text-align: start"
      >
        <div class="row">
          <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
            <q-input v-model="filter" dense outlined label="Pesquisar">
              <template v-slot:append>
                <q-icon name="search" />
              </template>
            </q-input>
          </div>
          <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3 col-xxl-3">
            <KButton
              style="float: right"
              :text="'Filtrar'"
              :title="'Filtrar Dados'"
              @click="showFilter()"
            ></KButton>
          </div>
          <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3 col-xxl-3">
            <KButton
              style="float: right"
              :text="'Relatório'"
              :title="'Baixar Relatório'"
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
          row-key="name"
          :rows-per-page-options="[10]"
          :filter="filter"
        >
        </q-table>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from "vue";
import { useRouter, useRoute } from "vue-router";
import Api from "@/core/api/api";

import KDialog from "@/components/KDialog.vue";
import KLoading from "@/components/KLoading.vue";
import KNotify from "@/components/KNotify.vue";
import KBreadCrumbs from "@/components/KBreadCrumbs.vue";

import routes from "@/modules/router";
import KButtonAction from "@/components/KButtonAction.vue";
import KButtonErp from "@/components/KButtonErp.vue";
import KButton from "@/components/KButton.vue";
import { IResult } from "@/core/types/IKResult";
import KDialogQuestion from "@/components/KDialogQuestion.vue";
import { IStocksContractFilters, IStocksFilters } from "./Types/IStocksFilters";
import { IStocksGroups } from "./Types/IStocksGroups";
import { IStocksCategory } from "./Types/IStocksCategory";
import KButtonCancel from "@/components/KButtonCancel.vue";
import KInput from "@components/KInput.vue";
import { IStocks } from "./Types/IStocks";

const router = useRouter();

const dados = ref([]);
const listaFuncionariosAux = ref([]);

const dialogQuestion = ref(false);
const dialogMessage = ref(false);
const dialogMessageText = ref("");
const dialogMessageTitle = ref("");
const dialogMessageType = ref("W");
const dialogLoading = ref(false);
const dialogNotify = ref(false);
const filter = ref("");

const listStocksGroups = ref([] as IStocksGroups[]);
const listStocksCategory = ref([] as IStocksCategory[]);
const filterFields = ref(new IStocksFilters());
const filterContract = ref([] as IStocksContractFilters[]);
const dialogFilters = ref(false);

const items = ref([] as IStocks[]);
//const itemsTemp = ref([] as IStocks[]);

const dataBreadCrumbs = ref([
  {
    label: "Estoque Disponível",
    router: "/listStock",
  },
]);

const headers = ref([
  {
    name: "code",
    align: "center",
    label: "Código",
    field: "code",
    sortable: false,
  },
  {
    name: "description",
    align: "center",
    label: "Descrição",
    field: "description",
    sortable: true,
  },
  {
    name: "local",
    align: "center",
    label: "Local",
    field: "local",
    sortable: true,
  },
  {
    name: "descGroup",
    align: "center",
    label: "Grupo",
    field: "descGroup",
    sortable: true,
  },
  {
    name: "category",
    align: "center",
    label: "Categoria Marg",
    field: "category",
    sortable: true,
  },
  {
    name: "balance",
    align: "center",
    label: "Saldo",
    field: "balance",
    sortable: true,
  },
  {
    name: "empenho",
    align: "center",
    label: "Empenho",
    field: "empenho",
    sortable: true,
  },
  {
    name: "balanceAvailable",
    align: "center",
    label: "Disponivel",
    field: "balanceAvailable",
    sortable: true,
  },
]);

function closeNotify() {
  dialogNotify.value = false;
}

function closeDialogMessage() {
  dialogMessage.value = false;
}

async function showStockItem() {
  dialogLoading.value = true;
  let result: IResult;
  result = await Api.getResult<IResult>("/Stocks");

  if (result.data.sucesso) {
    items.value = result.data.tRetorno;
    dialogLoading.value = false;
  }
}

function validateFilter() {
  if (
    filterFields.value.category.description == "" &&
    filterFields.value.code == "" &&
    filterFields.value.description == "" &&
    filterFields.value.filial == "" &&
    filterFields.value.group.description == "" &&
    filterFields.value.local == ""
  ) {
    dialogMessageText.value =
      "Informe uma das opções disponíveis para realizar o filtro dos dados.";
    dialogNotify.value = true;
    return true;
  }

  return false;
}

async function callFilter() {
  if (!validateFilter()) {
    items.value = [];
    if (filterFields.value.code) {
      let item = new IStocksContractFilters();
      item.fieldName = "code";
      item.filterData = filterFields.value.code;
      filterContract.value.push(item);
    }
    if (filterFields.value.filial) {
      let item = new IStocksContractFilters();
      item.fieldName = "filial";
      item.filterData = filterFields.value.filial;
      filterContract.value.push(item);
    }
    if (filterFields.value.local) {
      let item = new IStocksContractFilters();
      item.fieldName = "local";
      item.filterData = filterFields.value.local;
      filterContract.value.push(item);
    }
    if (filterFields.value.description) {
      let item = new IStocksContractFilters();
      item.fieldName = "description";
      item.filterData = filterFields.value.description;
      filterContract.value.push(item);
    }

    if (filterFields.value.group.description) {
      let item = new IStocksContractFilters();
      item.fieldName = "descGroup";
      item.filterData = filterFields.value.group.description;
      filterContract.value.push(item);
    }
    if (filterFields.value.category.description) {
      let item = new IStocksContractFilters();
      item.fieldName = "category";
      item.filterData = filterFields.value.category.description;
      filterContract.value.push(item);
    }

    if (items.value.length == 0) {
      dialogLoading.value = true;
      let result: IResult;
      result = await Api.postResult<IResult>(
        "Stocks/StockFilter",
        filterContract.value
      );

      if (result.data.sucesso) {
        items.value = result.data.tRetorno;
        dialogLoading.value = false;
        cancelFilter();
      }
    }
  }
}

function cancelFilter() {
  dialogFilters.value = false;
  filterFields.value = new IStocksFilters();
  filterContract.value = [];
}

function showFilter() {
  dialogFilters.value = true;
  filterFields.value = new IStocksFilters();
  filterContract.value = [];
}

async function showNewStocksGroups() {
  dialogLoading.value = true;
  let result: IResult;
  result = await Api.getResult<IResult>("/Stocks/GetStocksGroups");

  if (result.data.sucesso) {
    listStocksGroups.value = result.data.tRetorno;
    console.log(listStocksGroups.value);
    dialogLoading.value = false;
     showStocksCategory();
  
  }
}

async function showStocksCategory() {
  dialogLoading.value = true;
  let result: IResult;
  result = await Api.getResult<IResult>("/Stocks/GetStocksCategory");

  if (result.data.sucesso) {
    listStocksCategory.value = result.data.tRetorno;
    dialogLoading.value = false;
    showStockItem();
  }
}

onMounted(() => {
  showNewStocksGroups();
 
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
