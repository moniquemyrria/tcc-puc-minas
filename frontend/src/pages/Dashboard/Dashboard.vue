<template>
  <div>
    <KDialog
      :title="dialogMessageTitle"
      :dialog="dialogMessage"
      :text="dialogMessageText"
      @kDialogClose="closeDialogMessage"
    />

    <KLoading :title="'Carregando Dados... Aguarde!'" :dialog="dialogLoading" />

    <KNotify :dialog="dialogNotify" :text="'Teste'" :type="'W'" />
    <br />

    <div class="row">
      <div
        class="col-12 col-sm-7 col-md-7 col-lg-7 col-xl-7 col-xxl-7"
        style="text-align: start"
      >
        <h5 class="title" style="margin: 4px 0px">Dashboard</h5>
        <div class="col-12 sub-title" style="text-align: start">
          Painel de informações de Receitas e Despesas |
          <b>{{ "Período: " + date + " a " + date2 }}</b>
        </div>
      </div>
    </div>

    <br />
    <br />

    <div class="row">
      <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4 col-xxl-4">
        <q-card flat bordered style="max-width: 98%">
          <q-tooltip>
            {{ "Receitas do período: " + date + " a " + date2 }}
          </q-tooltip>

          <q-card-section>
            <div style="text-align: start" class="text-h5 q-mt-sm q-mb-xs">
              {{ "R$ " + sumTotalRevenue }}
            </div>
            <div style="text-align: start" class="text-overline">
              Total de Receitas
            </div>
          </q-card-section>

          <div class="card-dash" style="background-color: #3f51b5"></div>
        </q-card>
      </div>
      <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4 col-xxl-4">
        <q-card flat bordered style="max-width: 98%">
          <q-tooltip>
            {{ "Despesas do período: " + date + " a " + date2 }}
          </q-tooltip>
          <q-card-section>
            <div style="text-align: start" class="text-h5 q-mt-sm q-mb-xs">
              {{ "R$ " + sumTotalExpense }}
            </div>
            <div style="text-align: start" class="text-overline">
              Total de Despesas
            </div>
          </q-card-section>

          <div class="card-dash" style="background-color: #b71c1c"></div>
        </q-card>
      </div>
      <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4 col-xxl-4">
        <q-card flat bordered style="max-width: 98%">
          <!-- <q-tooltip> Saldo acumulado </q-tooltip> -->
          <q-tooltip> Saldo Atual </q-tooltip>
          <q-card-section>
            <div style="text-align: start" class="text-h5 q-mt-sm q-mb-xs">
              {{ "R$ " + sumTotalBalance }}
              
            </div>
            <div style="text-align: start" class="text-overline">Saldo</div>
          </q-card-section>

          <div class="card-dash" style="background-color: #3a853d"></div>
        </q-card>
      </div>
    </div>
    <br />
    <div class="row">
      <div class="col-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
        <q-card flat bordered style="max-width: 99%; min-height: 100%">
          <q-card-section>
            <q-tooltip> Dados por Categorias de Despesas </q-tooltip>
            <div style="text-align: start" class="text-overline">
              Gráfico de Despesas do Mês
            </div>
          </q-card-section>
          <q-card-section style="max-width: 100%; max-height: 100%">
            <apexchart
              width="100%"
              type="bar"
              :options="chartOptionsExpense"
              :series="chartOptionsExpense.series"
            ></apexchart>
          </q-card-section>
        </q-card>
      </div>
      <div class="col-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
        <q-card flat bordered style="max-width: 99%; max-height: 100%">
          <q-card-section>
            <q-tooltip> Dados por Categorias de Receitas </q-tooltip>
            <div style="text-align: start" class="text-overline">
              Gráfico de Receitas do Mês
            </div>
          </q-card-section>
          <q-card-section style="max-width: 100%; max-height: 100%">
            <apexchart
              width="100%"
              :options="optionsDonutsRevenue"
              :series="optionsDonutsRevenue.series"
            ></apexchart>
          </q-card-section>
        </q-card>
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
import KBreadCrumbs from "./../../../components/KBreadCrumbs.vue";

import routes from "@/modules/router";
import KButtonAction from "../../../components/KButtonAction.vue";
import KButton from "../../../components/KButton.vue";
import { IResult } from "@/core/types/IKResult";
import KDialogQuestion from "@/components/KDialogQuestion.vue";
import { storeUser } from "@/core/store/userStore";
import { IAccount } from "@/pages/Administration/Account/Type/IAccount";
import moment from "moment";
import VueApexCharts from "vue3-apexcharts";
import ApexCharts from "apexcharts";
import { colors } from "quasar";

const router = useRouter();
const userStore = storeUser();
const userLogged = userStore.getUser.user;

const dialogQuestion = ref(false);
const dialogMessage = ref(false);
const dialogMessageText = ref("");
const dialogMessageTitle = ref("");
const dialogLoading = ref(false);
const dialogNotify = ref(false);

const date = ref(moment(new Date()).format("DD/MM/YYYY").toString());
const date2 = ref(moment(new Date()).format("DD/MM/YYYY").toString());

const sumTotalExpense = ref("0,00");
const sumTotalRevenue = ref("0,00");
const sumTotalBalance = ref("0,00");

const chartOptionsExpense: any = ref({
  series: [
    {
      name: "Valor: R$ ",
      data: [],
    },
  ],
  chart: {
    height: 200,
    type: "bar",
  },
  colors: [],

  plotOptions: {
    bar: {
      columnWidth: "45%",
      distributed: true,
    },
  },
  dataLabels: {
    enabled: false,
  },
  legend: {
    show: false,
  },
  xaxis: {
    categories: [],
    labels: {
      style: {
        fontSize: "12px",
      },
    },
  },
});

const optionsDonutsRevenue = ref({
  series: [],
  chart: {
    height: 200,
    type: "pie",
  },
  colors: [],
  labels: [],
  // legend: {
  //   position: "top",
  //   horizontalAlign: "right",
  // },
  responsive: [
    {
      breakpoint: 480,
      options: {
        chart: {
          width: 200,
        },
      },
    },
  ],
});

async function showChartExpenseCategory() {
  let filterDahboard = {
    idUser: userLogged.id,
    dateInitial: moment(date.value, "DD/MM/YYYY").toDate(),
    dateFinal: moment(date2.value, "DD/MM/YYYY").toDate(),
  };

  dialogLoading.value = true;
  let result: IResult;
  result = await Api.postResult<IResult>(
    "/Dashboard/TotalizersChartExpenseCategory",
    filterDahboard
  );

  if (result.data.sucesso) {
    chartOptionsExpense.value.series[0].data = result.data.tRetorno.series;

    result.data.tRetorno.categories.forEach((item: any) => {
      chartOptionsExpense.value.xaxis.categories.push(item);
    });

    result.data.tRetorno.colors.forEach((item: any) => {
      chartOptionsExpense.value.colors.push(item);
    });
    dialogLoading.value = false;
  }
}

async function showChartRevenueCategory() {
  let filterDahboard = {
    idUser: userLogged.id,
    dateInitial: moment(date.value, "DD/MM/YYYY").toDate(),
    dateFinal: moment(date2.value, "DD/MM/YYYY").toDate(),
  };

  dialogLoading.value = true;
  let result: IResult;
  result = await Api.postResult<IResult>(
    "/Dashboard/TotalizersChartRevenueCategory",
    filterDahboard
  );

  if (result.data.sucesso) {
    optionsDonutsRevenue.value.series = result.data.tRetorno.series;

    result.data.tRetorno.labels.forEach((item: any) => {
      optionsDonutsRevenue.value.labels.push(item);
    });

    result.data.tRetorno.colors.forEach((item: any) => {
      optionsDonutsRevenue.value.colors.push(item);
    });

    dialogLoading.value = false;
  }
}

async function showTotalizersCards() {
  let filterDahboard = {
    idUser: userLogged.id,
    dateInitial: moment(date.value, "DD/MM/YYYY").toDate(),
    dateFinal: moment(date2.value, "DD/MM/YYYY").toDate(),
  };

  dialogLoading.value = true;
  let result: IResult;
  result = await Api.postResult<IResult>(
    "/Dashboard/TotalizersCards",
    filterDahboard
  );

  if (result.data.sucesso) {
    sumTotalRevenue.value = result.data.tRetorno.sumTotalRevenue;
    sumTotalExpense.value = result.data.tRetorno.sumTotalExpense;

    var saldo = (parseFloat(result.data.tRetorno.sumTotalRevenue.replace('.', '').replace(',', '.')) - parseFloat(result.data.tRetorno.sumTotalExpense.replace('.', '').replace(',', '.')))//result.data.tRetorno.sumTotalBalance;
    
    if (saldo > 0){
       sumTotalBalance.value = saldo.toLocaleString('de-DE', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }

    dialogLoading.value = false;
  }
}

function closeDialogMessage() {
  dialogMessage.value = false;
}

onMounted(() => {
  date.value = moment(new Date()).format("01/MM/YYYY").toString();
  date2.value = moment(new Date())
    .endOf("month")
    .format("DD/MM/YYYY")
    .toString();

  showTotalizersCards();
  showChartExpenseCategory();
  showChartRevenueCategory();
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

.card-dash {
  min-height: 5px;
}
</style>
