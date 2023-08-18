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
      <q-card style="min-width: 800px">
        <q-card-section>
          <div class="text-h6">Filtros</div>
          <div class="text-caption text-grey">
            Utilize os campos abaixo para filtrar os dados de produto
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <div class="row">
            <div class="col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3 col-xxl-3">
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

            <div class="col-12 col-sm-12 col-md-9 col-lg-9 col-xl-9 col-xxl-9">
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

            <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4 col-xxl-4">
              <q-select
                :options="listStatus"
                class="k-input-pa"
                dense
                outlined
                label="Status"
                v-model="filterFields.status"
                reactive-rules
              />
            </div>

            <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4 col-xxl-4">
              <q-select
                :options="listProductsGroups"
                option-label="description"
                class="k-input-pa"
                dense
                outlined
                label="Grupo"
                v-model="filterFields.group"
                reactive-rules
              />
            </div>

            <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4 col-xxl-4">
              <q-select
                v-model="filterFields.type"
                :options="listProductsTypes"
                option-label="description"
                class="k-input-pa"
                dense
                outlined
                label="Types"
                reactive-rules
              />
            </div>
          </div>
        </q-card-section>

        <q-card-actions align="right">
          <div class="row"></div>
          <KButtonCancel @click="cancelFilter()" :text="'Cancelar'" />

          <KButton @click="callFilter()" :text="'Filtrar'" />
          <!-- <q-btn size="md" v-close-popup :style="'min-width: ' + size + 'px; background:' + color + '; color:' +  colorText" @click="callFilter()"> Filtrar </q-btn> -->
        </q-card-actions>
      </q-card>
    </q-dialog>

    <div class="row">
      <div
        class="col-12 col-sm-7 col-md-7 col-lg-7 col-xl-7 col-xxl-7"
        style="text-align: start"
      >
        <h5 class="title" style="margin: 4px 0px">Todos os Produtos</h5>
        <div class="col-12 sub-title" style="text-align: start">
          Acompanhe os cadastros de produtos
        </div>
      </div>

      <div
        class="col-12 col-sm-5 col-md-5 col-lg-5 col-xl-5 col-xxl-5"
        style="text-align: start"
      >
        <div class="row">
          <div class="col-12 col-sm-12 col-md-9 col-lg-9 col-xl-9 col-xxl-9">
            <q-input
              v-model="filterColumns"
              dense
              outlined
              label="Pesquisar"
              style="width: 102%"
            >
              <template v-slot:append>
                <q-icon name="search" />
              </template>
            </q-input>
          </div>

          <!-- <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3 col-xxl-3">
            <KButtonErp
              style="float: right"
              :text="'ERP'"
              @click="showNovaAtividade"
              :title="'Importar Produtos'"
            ></KButtonErp>
          </div> -->
          <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3 col-xxl-3">
            <KButton
              @click="showFilter()"
              style="float: right"
              :text="'Filtrar'"
              :title="'Filtro de Dados'"
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
          :filter="filterColumns"
          :rows-per-page-options="[10]"
        >
          <template v-slot:body="props">
            <q-tr :props="props">
              <q-td
                v-for="col in props.cols"
                :key="col.name"
                :props="props"
                :filter="filter"
              >
                <template v-if="col.name == 'status'">
                  <q-chip
                    square
                    style="min-width: 100px"
                    :style="
                      col.value !== '1'
                        ? 'background-color: #DDF4D9; color: #436B3D'
                        : 'background-color: #E6E6E6; color: #777779'
                    "
                  >
                    <div class="col-12" align="center">
                      <span style="font-weight: bold">{{
                        col.value !== '1'  ? "Em Linha" : "Descontinuado"
                      }}</span>
                    </div>
                  </q-chip>
                </template>
                <template v-else-if="col.name == 'actions'">
                  <KButtonAction
                    :title="'Visualizar'"
                    @click="onView(props.row.id)"
                    :icon="'visibility'"
                  />
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

import KDialog from "@/components/KDialog.vue";
import KLoading from "@/components/KLoading.vue";
import KNotify from "@/components/KNotify.vue";
import KBreadCrumbs from "@/components/KBreadCrumbs.vue";

import routes from "@/modules/router";
import KButtonAction from "@/components/KButtonAction.vue";
import KButtonErp from "@/components/KButtonErp.vue";
import KButton from "@/components/KButton.vue";
import KButtonCancel from "@/components/KButtonCancel.vue";
import KInput from "@components/KInput.vue";
import { IResult } from "@/core/types/IKResult";
import KDialogQuestion from "@/components/KDialogQuestion.vue";
import { IProductsGroups } from "./Types/IProductsGroups";
import { IProductsTypes } from "./Types/IProductsTypes";
import {
  IProductsContractFilters,
  IProductsFilters,
} from "./Types/IProductsFilters";
import { IProducts } from "@/core/types/IProducts";

const router = useRouter();

const dados = ref([]);

const dialogQuestion = ref(false);
const dialogMessage = ref(false);
const dialogMessageText = ref("");
const dialogMessageTitle = ref("");
const dialogLoading = ref(false);
const dialogNotify = ref(false);
const dialogFilters = ref(false);
const dialogMessageType = ref("W");

const itemsTemp = ref([] as IProducts[]);
const items = ref([] as IProducts[]);
const listProductsGroups = ref([] as IProductsGroups[]);
const listProductsTypes = ref([] as IProductsTypes[]);
const listStatus = ref(["Descontinuado", "Em Linha"]);
const filterFields = ref(new IProductsFilters());
const filter = ref([] as IProductsContractFilters[]);
const filterColumns = ref("");
const dataBreadCrumbs = ref([
  {
    label: "Lista Produtos",
    router: "/listProducts",
  },
]);

const headers = ref([
  {
    name: "status",
    align: "left",
    label: "Status",
    field: "status",
    sortable: false,
  },
  {
    name: "code",
    align: "left",
    label: "Códigos",
    field: "code",
    sortable: false,
  },
  {
    name: "description",
    align: "left",
    label: "Descrição",
    field: "description",
    sortable: true,
  },
  {
    name: "groupDescription",
    align: "center",
    label: "Grupo",
    field: "groupDescription",
    sortable: true,
  },
  {
    name: "type",
    align: "center",
    label: "Tipo",
    field: "type",
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

function closeNotify() {
  dialogNotify.value = false;
}

function validateFilter() {
  if (
    filterFields.value.code.trim() == "" &&
    filterFields.value.description == "" &&
    filterFields.value.group.description == "" &&
    filterFields.value.type.description == "" &&
    filterFields.value.status == ""
  ) {
    dialogMessageText.value =
      "Informe uma das opções disponíveis para realizar o filtro dos dados.";
    dialogNotify.value = true;
    return true;
  }

  console.log("teste: " + filterFields.value.code);

  return false;
}

async function callFilter() {
  if (!validateFilter()) {
    if (filterFields.value.code) {
      items.value = [];
      let itemsAdd = [] as IProducts[];
      itemsAdd = itemsTemp.value.filter(
        (t) =>
          t.code.toString().trim() == filterFields.value.code.toString().trim()
      );

      if (itemsAdd.length > 0) {
        items.value = JSON.parse(JSON.stringify(itemsAdd));
        cancelFilter();
      } else {
        let item = new IProductsContractFilters();
        item.fieldName = "code";
        item.filterData = filterFields.value.code;
        filter.value.push(item);
      }
    }

    if (filterFields.value.description) {
      items.value = [];
      let itemsAdd = [] as IProducts[];
      itemsAdd = itemsTemp.value.filter(
        (t) =>
          t.description.toString().trim() ==
          filterFields.value.description.toString().trim()
      );

      if (itemsAdd.length > 0) {
        items.value = JSON.parse(JSON.stringify(itemsAdd));
        cancelFilter();
      } else {
        let item = new IProductsContractFilters();
        item.fieldName = "description";
        item.filterData = filterFields.value.description;
        filter.value.push(item);
      }
    }

    if (filterFields.value.group.description) {
      let item = new IProductsContractFilters();
      item.fieldName = "groupDescription";
      item.filterData = filterFields.value.group.description;
      filter.value.push(item);

      items.value = [];
    }

    if (filterFields.value.status) {
      let item = new IProductsContractFilters();

      let status = filterFields.value.status == "Descontinuado" ? "1" : "2";

      item.fieldName = "status";
      item.filterData = status;
      filter.value.push(item);

      items.value = [];
    }

    if (filterFields.value.type.description) {
      let item = new IProductsContractFilters();
      item.fieldName = "type";
      item.filterData = filterFields.value.type.description;
      filter.value.push(item);

      items.value = [];
    }

    if (items.value.length == 0) {
      dialogLoading.value = true;
      let result: IResult;
      result = await Api.postResult<IResult>(
        "Products/ProductsFilter",
        filter.value
      );

      if (result.data.sucesso) {
        await findItems(result.data.tRetorno).then(
          (t) => (dialogLoading.value = false)
        );

        items.value = result.data.tRetorno;
        cancelFilter();
      }
    }
  }
}

async function findItems(itemsResult: IProducts[]): Promise<void> {
  let olddItems = JSON.parse(JSON.stringify(itemsTemp.value));
  items.value = [];

  await itemsResult.forEach((item: IProducts) => {
    let itemFind: any;
    itemFind = olddItems.find((t: IProducts) => t.code == item.code);

    if (itemFind == undefined) {
      itemsTemp.value.push(item);
    }
  });
}

function cancelFilter() {
  dialogFilters.value = false;
  filterFields.value = new IProductsFilters();
  filter.value = [];
}

function showFilter() {
  dialogFilters.value = true;
  filterFields.value = new IProductsFilters();
  filter.value = [];
}

function closeDialogMessage() {
  dialogMessage.value = false;
}

async function showProducsGroups() {
  dialogLoading.value = true;
  let result: IResult;
  result = await Api.getResult<IResult>("/Products/GetProductsGroups");

  if (result.data.sucesso) {
    listProductsGroups.value = result.data.tRetorno;
    dialogLoading.value = false;
  }
}

async function showProducsTypes() {
  dialogLoading.value = true;
  let result: IResult;
  result = await Api.getResult<IResult>("/Products/GetProductsTypes");

  if (result.data.sucesso) {
    listProductsTypes.value = result.data.tRetorno;
    dialogLoading.value = false;
    dialogFilters.value = true;
  }
}

async function showItems() {
  dialogLoading.value = true;
  let result: IResult;
  result = await Api.getResult<IResult>("/Products");

  if (result.data.sucesso) {
    itemsTemp.value = result.data.tRetorno;
    items.value = result.data.tRetorno;
    dialogLoading.value = false;
  }
}

function onView(id: number) {
  //console.log(item);
  router.push("/viewProducts/" + id);
}

onMounted(() => {
  //showItems();
  showProducsGroups();
  showProducsTypes();
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
