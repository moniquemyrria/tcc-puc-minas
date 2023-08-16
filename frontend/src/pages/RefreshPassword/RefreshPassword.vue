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
      @kDialogQuestionYes="callActiveInactiveYesAtividade"
      @kDialogQuestionNo="callActiveInactiveNoAtividade"
    ></KDialogQuestion>

    <KLoading :title="'Carregando Dados... Aguarde!'" :dialog="dialogLoading" />

    <KNotify :dialog="dialogNotify" :text="'Teste'" :type="'W'" />

    <div class="container-fluid">
      <div class="loginFluid">
        <div class="center-card">
          <div style="margin: 0 auto">
            <q-card flat style="width: 500px; background-color: ">
              <q-card-section>
                <div class="row">
                  <div class="col-12" style="text-align: start">
                    <h5 style="margin: 4px 0px 20px" class="title">
                      Alteraçao de Senha
                    </h5>
                  </div>
                </div>
                <br />
                <div class="row">
                  <q-input
                    type="text"
                    outlined
                    label="Senha atual"
                    style="width: 100%"
                    v-model="refreshPass.oldPass"
                  >
                    <template v-slot:append>
                      <q-icon name="lock" />
                    </template>
                  </q-input>
                </div>
                <br />
                <div class="row">
                  <q-input
                    type="text"
                    outlined
                    label="Nova Senha"
                    style="width: 100%"
                    v-model="refreshPass.newPass"
                  >
                    <template v-slot:append>
                      <q-icon name="lock" />
                    </template>
                  </q-input>
                </div>
                <br />
                <div class="row">
                  <q-input
                    type="text"
                    outlined
                    label="Confirmação de Senha"
                    style="width: 100%"
                    v-model="refreshPass.confirmPass"
                  >
                    <template v-slot:append>
                      <q-icon name="lock" />
                    </template>
                  </q-input>
                </div>
                <br />

                <br />
                <div class="row">
                  <div
                    class="col-12 col-sm-12 col-md-12 col-lg-6 col-xl-6 col-xxl-6"
                  >
                    <q-btn
                      title="Retornar para o Login"
                      size="md"
                      outline
                      style="color: #53258e; width: 95%"
                      label="Cancelar"
                      @click="cancelRefreshPassword()"
                    />
                  </div>
                  <div
                    class="col-12 col-sm-12 col-md-12 col-lg-6 col-xl-6 col-xxl-6"
                  >
                    <q-btn
                      size="md"
                      style="background: #53258e; color: white; width: 95%"
                      label="Altera senha"
                      @click="refreshPassword()"
                    />
                  </div>
                </div>
                <br />
                <br />
                <div style="margin: 0 auto">
                  <img
                    style="height: 50%; width: 50%"
                    src="./../../../public/LogoOriginal4.png"
                  />
                </div>
              </q-card-section>
            </q-card>
          </div>
        </div>
      </div>
      <div class="row footer d-flex align-center justify-center">
        <span class="footer-text"
          >Sistema para controle de despesas e receitas pessoais</span
        >
      </div>
    </div>

    <q-footer elevated>
      <q-toolbar style="background-color: black">
        <q-toolbar-title>Footer</q-toolbar-title>
      </q-toolbar>
    </q-footer>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from "vue";
import { useRouter, useRoute } from "vue-router";
import Api from "@/core/api/api";

import KDialog from "./../../components/KDialog.vue";
import KLoading from "./../../components/KLoading.vue";
import KNotify from "../../components/KNotify.vue";

import routes from "@/modules/router";
import KButtonAction from "../../../../components/KButtonAction.vue";
import KButton from "./../../components/KButton.vue";
import { IResult } from "@/core/types/IKResult";
import KDialogQuestion from "@/components/KDialogQuestion.vue";
import { storeUser } from "@/core/store/userStore";
import { IRefreshPassword } from "./IRefreshPassword";

const userStore = storeUser();
const router = useRouter();

const dados = ref([]);
const listaFuncionariosAux = ref([]);

const dialogQuestion = ref(false);
const dialogMessage = ref(false);
const dialogMessageText = ref("");
const dialogMessageTitle = ref("");
const dialogLoading = ref(false);
const dialogNotify = ref(false);
const isPwd = ref(true);
const isSuccess = ref(false);
const refreshPass = ref({} as IRefreshPassword);

function closeDialogMessage() {
  dialogMessage.value = false;

  if (isSuccess.value) {
    router.push("/");
  }
}

function callActiveInactiveYesAtividade() {
  dialogQuestion.value = false;
  dialogMessage.value = false;
}

function callActiveInactiveNoAtividade() {
  dialogQuestion.value = false;
}

function showNovaAtividade() {
  router.push("/formAtividades/");
}

function onEdit(item: any) {
  //console.log(item);
  router.push("/editAtividades/" + item.id);
}

function validate() {
  if (!refreshPass.value.oldPass) {
    dialogMessageTitle.value = "Atualização de Senha Controle de Orçamento";
    dialogMessageText.value =
      "Para anterar a senha atual é necessário informar a Senha Antiga.";
    dialogMessage.value = true;
    return true;
  }

  if (!refreshPass.value.newPass) {
    dialogMessageTitle.value = "Atualização de Senha Controle de Orçamento";
    dialogMessageText.value = "Informe a nova senha.";
    dialogMessage.value = true;
    isSuccess.value = false;
    return true;
  }

  if (!refreshPass.value.confirmPass) {
    dialogMessageTitle.value = "Atualização de Senha Controle de Orçamento";
    dialogMessageText.value = "É necessário confirmar a Nova senha informada.";
    dialogMessage.value = true;
    isSuccess.value = false;
    return true;
  }

  if (refreshPass.value.oldPass.trim() == refreshPass.value.newPass.trim()) {
    dialogMessageTitle.value = "Atualização de Senha Controle de Orçamento";
    dialogMessageText.value =
      "A Nova senha não pode ser identica a Senha antiga.";
    dialogMessage.value = true;
    isSuccess.value = false;
    return true;
  }

  if (
    refreshPass.value.newPass.trim() != refreshPass.value.confirmPass.trim()
  ) {
    dialogMessageTitle.value = "Atualização de Senha Controle de Orçamento";
    dialogMessageText.value = "A Nova senha e a Senha confirmada não conferem.";
    dialogMessage.value = true;
    isSuccess.value = false;
    return true;
  }

  return false;
}

async function cancelRefreshPassword() {
  router.push("/");
}

async function refreshPassword() {
  if (!validate()) {
    dialogLoading.value = true;
    let result: IResult;
    refreshPass.value.userName = userStore.getUser.user.userName;

    result = await Api.putRefreshPasswordResult<IResult>(
      "/ServiceUser/newpass",
      refreshPass.value
    );

    if (result.data.sucesso) {
      dialogMessageTitle.value = "Login de acesso Controle de Orçamento";
      dialogMessageText.value = result.data.message;
      dialogMessage.value = true;
      isSuccess.value = true;
    } else {
      dialogMessageTitle.value = "Login de acesso Controle de Orçamento";
      dialogMessageText.value = result.data.message;
      dialogMessage.value = true;
      isSuccess.value = false;
    }
    dialogLoading.value = false;
  }
}

onMounted(() => {});
</script>

<style scoped>
.title {
  /* UI Properties */
  text-align: left;
  font: normal normal bold 20px/25px Helvetica;
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
.loginFluid {
  background-image: url("./../../../public/RefreshPass.png");
  background-size: cover;
  height: 97vh;
}

.center-card {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  flex-direction: column;
  text-align: center;
  position: relative;
  top: 12%;
}

.footer {
  background-color: black;
  text-align: center;
  height: 3vh;
}

.footer-text {
  font: normal normal bold 14px/17px Helvetica;
  letter-spacing: 0px;
  color: #ffffff;
  opacity: 1;
  text-align: center;
}
</style>
