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

    <br />

    <div class="row">
      <div class="col-12" style="text-align: start">
        <h5 style="margin: 4px 0px 20px" class="title">
          {{ productItem.description }}
        </h5>
      </div>
    </div>
    <br />

    <KCardForm>
      <q-card-section class="q-pt-none" align="center">
        <q-form>
          <div class="row">
            <div class="col-12 col-sm-12 col-md-1 col-lg-1 col-xl-1 col-xxl-1">
              <k-input readonly v-model="productItem.code" label="Código" />
            </div>

            <div class="col-12 col-sm-12 col-md-5 col-lg-5 col-xl-5 col-xxl-5">
              <k-input
                readonly
                v-model="productItem.description"
                label="Descrição"
              />
            </div>
            <div class="col-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2">
              <k-input readonly v-model="productItem.group" label="Grupo" />
            </div>
            <div class="col-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2">
              <k-input readonly v-model="productItem.type" label="Tipo" />
            </div>
            <div class="col-12 col-sm-12 col-md-1 col-lg-1 col-xl-1 col-xxl-1">
              <k-input readonly v-model="productItem.um" label="Unidade" />
            </div>
          </div>

          <div class="row">
            <div class="col-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2">
              <k-input
                readonly
                v-model="productItem.barCode"
                label="Código de Barras"
              />
            </div>
            <div class="col-12 col-sm-12 col-md-1 col-lg-1 col-xl-1 col-xxl-1">
              <k-input readonly v-model="productItem.origem" label="Origem" />
            </div>
            <div class="col-12 col-sm-12 col-md-1 col-lg-1 col-xl-1 col-xxl-1">
              <k-input readonly v-model="productItem.desciptionOrigem" />
            </div>

            <div class="ccol-12 col-sm-12 col-md-1 col-lg-1 col-xl-1 col-xxl-1">
              <k-input
                readonly
                v-model="productItem.weight"
                label="Peso Líquido"
              />
            </div>
            <div class="ccol-12 col-sm-12 col-md-1 col-lg-1 col-xl-1 col-xxl-1">
              <k-input
                readonly
                v-model="productItem.grossWeight"
                label="Peso Bruto"
              />
            </div>
            <div class="ccol-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2">
              <k-input
                readonly
                v-model="productItem.category"
                label="Categoria Marg"
              />
            </div>
            <div class="ccol-12 col-sm-12 col-md-1 col-lg-1 col-xl-1 col-xxl-1">
              <k-input
                readonly
                v-model="productItem.pdvValue"
                label="Valor PDV"
              />
            </div>
            <div class="col-12 col-sm-12 col-md-2 col-lg-2 col-xl-2 col-xxl-2">
              <k-input
                readonly
                v-model="productItem.blockedStatus"
                label="Status"
              />
            </div>
          </div>

          <div class="row">
            <div class="col-12 col-md-2 col-md-2 col-lg-2 col-xl-2 col-xxl-2">
              <k-input
                readonly
                v-model="productItem.stock"
                label="Estoque Atual"
              />
            </div>
            <div class="col-12 col-md-2 col-md-2 col-lg-2 col-xl-2 col-xxl-2">
              <k-input
                readonly
                v-model="productItem.empenho"
                label="Pedidos Empenhados"
              />
            </div>
            <div class="col-12 col-md-2 col-md-2 col-lg-2 col-xl-2 col-xxl-2">
              <k-input
                readonly
                v-model="productItem.stockSP"
                label="Estoque Disponível em SP"
              />
            </div>
          </div>
        </q-form>
        <br />
        <q-form class="q-gutter-md">
          <div
            class="col-12 col-sm-7 col-md-7 col-lg-7 col-xl-7 col-xxl-7"
            style="text-align: start"
          >
            <h5 class="sub-title-black" style="margin: 4px 0px">
              Tabela de Preço para este Produto
            </h5>
          </div>
          <div class="col-12 col-sm-10 col-md-12 col-lg-2 col-xl-2 col-xxl-2">
            <q-table
              :rows="productItem.productsPriceList"
              :columns="headersProductPriceList"
              row-key="code"
              :rows-per-page-options="[5]"
            >
            </q-table>
          </div>
          <div class="row">
            <div class="col-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
              <h5 class="sub-title-black" style="margin: 4px 0px">
                Histórico dos seus Clientes com este Produto
              </h5>
              <div class="col-12 sub-title">
                Acompanhe os dados do produto selecionado
              </div>
            </div>
            <div class="col-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6">
              <div class="row">
                <div
                  class="col-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6"
                >
                  <k-input dense outlined label="Filtrar"> </k-input>
                </div>
                <div
                  class="col-6 col-sm-6 col-md-3 col-lg-3 col-xl-3 col-xxl-3"
                >
                  <KButtonCancel
                    style="
                      align-items: center;
                      position: relative;
                      top: 11px;
                      min-width: 95% !important;
                    "
                    :text="'Atualizar'"
                  />
                </div>
                <div
                  class="col-6 col-sm-6 col-md-3 col-lg-3 col-xl-3 col-xxl-3"
                >
                  <KButton
                    style="
                      float: right;
                      position: relative;
                      top: 11px;
                      min-width: 95% !important;
                    "
                    :text="'ReLatório'"
                  />
                </div>
              </div>
            </div>
          </div>
        </q-form>
        <br />
        <q-form class="q-gutter-md">
          <div
            class="col-12 col-sm-7 col-md-7 col-lg-7 col-xl-7 col-xxl-7"
            style="text-align: start"
          >
            <q-radio val="line" label="Clentes que Compraram este produto" />
            <q-radio
              val="rectangle"
              label="Clientes que Não compraram este Produto"
            />
          </div>
          <div class="col-12 col-sm-10 col-md-12 col-lg-2 col-xl-2 col-xxl-2">
            <q-table :rows="historicalItem" :columns="headersHistorical">
            </q-table>
          </div>
        </q-form>
      </q-card-section>
    </KCardForm>
  </div>
</template>

<script setup lang="ts">
//import DadosFuncionario from "@/components/DadosFuncionario.vue";
//import DescicaoCargo from "@/components/DescricaoCargo.vue";
import { ref, onMounted, watch } from "vue";
import { useRouter, useRoute } from "vue-router";
import { useQuasar } from "quasar";
import KBreadCrumbs from "@/components/KBreadCrumbs.vue";
import KCardForm from "@/components/KCardForm.vue";
import KInput from "@components/KInput.vue";
import KButton from "@/components/KButton.vue";
import KButtonCancel from "@/components/KButtonCancel.vue";
import { IProducts } from "@/core/types/IProducts";
import Api from "@/core/api/api";
import KDialog from "@/components/KDialog.vue";
import KLoading from "@/components/KLoading.vue";
import { IResult } from "@/core/types/IKResult";
import moment from "moment";

const tab = ref("movies");
const productItem = ref({} as IProducts);
const idProduct = ref("");
const router = useRouter();
const route = useRoute();

const dialogMessage = ref(false);
const dialogMessageText = ref("");
const dialogMessageTitle = ref("");
const dialogLoading = ref(false);

const dataBreadCrumbs = ref([
  {
    label: "Lista Produtos",
    router: "/ListProducts",
  },
]);
const headersProductPriceList = ref([
  {
    name: "code",
    align: "center",
    label: "Códigos",
    field: "code",
    sortable: false,
  },
  {
    name: "salePriceTab",
    align: "left",
    label: "Preço",
    field: "salePriceTab",
    sortable: true,
  },
  {
    name: "descriptionTab",
    align: "left",
    label: "Descrição",
    field: "descriptionTab",
    sortable: true,
  },
  {
    name: "expirationDateFormat",
    align: "center",
    label: "Vigência Até",
    field: "expirationDateFormat",
    sortable: true,
  },
]);

const headersHistorical = ref([
  {
    name: "codigo",
    align: "center",
    label: "CNPJ Base",
    field: "codigo",
    sortable: false,
  },
  {
    name: "preco",
    align: "center",
    label: "Nome",
    field: "preco",
    sortable: true,
  },
  {
    name: "descricao",
    align: "center",
    label: "Cidade",
    field: "descricao",
    sortable: true,
  },
  {
    name: "vigencia",
    align: "center",
    label: "Estado",
    field: "vigencia",
    sortable: true,
  },
  {
    name: "vigencia",
    align: "center",
    label: "Gerente",
    field: "vigencia",
    sortable: true,
  },
  {
    name: "vigencia",
    align: "center",
    label: "Representante",
    field: "vigencia",
    sortable: true,
  },
  {
    name: "vigencia",
    align: "center",
    label: "Ultima Negociação",
    field: "vigencia",
    sortable: true,
  },
]);
const historicalItem = ref([]);
function showList() {
  router.push("/listAtividades");
}

function callItemsBreadCrumbs() {
  dataBreadCrumbs.value.push({
    label: "Visualizar Produto",
    router: "/",
  });
}

function closeDialogMessage() {
  dialogMessage.value = false;
  showList();
}

async function showProduct() {
  if (route.params.id) {
    idProduct.value = route.params.id.toString();

    dialogLoading.value = true;
    let result: IResult = {} as IResult;
    result = await Api.getIdResult<IResult>(
      "/Products",
      idProduct.value.toString()
    );

    if (result.data.sucesso) {
      productItem.value = result.data.tRetorno;

      productItem.value.productsPriceList.forEach((item: any) => {
        item.expirationDateFormat = formatDates(item.expirationDate.toString());
      });

      productItem.value = JSON.parse(JSON.stringify(productItem.value));
    }

    dialogLoading.value = false;
  }
}

function formatDates(date: string) {
  console.log(date);

  return moment(date).format("DD/MM/YYYY").toString();
}

onMounted(() => {
  callItemsBreadCrumbs();
  showProduct();
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

.sub-title-black {
  /* UI Properties */
  text-align: left;
  font: normal normal normal bold 14px/16px Helvetica;
  letter-spacing: 0px;
  color: #000000;
  opacity: 1;
}
</style>
