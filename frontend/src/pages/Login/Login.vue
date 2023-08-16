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

    <KNotify
      :dialog="dialogNotify"
      :text="dialogMessageText"
      :type="dialogMessageType"
      @kNotifyClose="closeNotify"
    />

    <!-- Como podemos ajuda-lo -->
    <q-dialog v-model="dialogHelp" persistent :position="'right'">
      <q-card style="min-width: 500px">
        <q-card-section class="row items-center q-pb-none">
          <div class="text-h6">Como podemos ajudá-lo?</div>
          <q-space />
          <q-btn
            style="color: #53258e"
            icon="close"
            flat
            round
            dense
            v-close-popup
          />
        </q-card-section>

        <q-card-section>
          <q-form>
            <div class="row">
              <div
                class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
              >
                <k-input
                  dense
                  outlined
                  label="Nome *"
                  lazy-rules
                  maxlength="100"
                  v-model="help.name"
                  :rules="[
                    (val) =>
                      (val && val.length > 0) || 'Por favor, informe o Nome',
                  ]"
                />
              </div>
            </div>
            <div class="row">
              <div
                class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
              >
                <k-input
                  dense
                  outlined
                  label="E-mail *"
                  lazy-rules
                  maxlength="100"
                  v-model="help.email"
                  :rules="[
                    (val) =>
                      (val && val.length > 0) || 'Por favor, informe o E-mail',
                  ]"
                />
              </div>
            </div>
            <div class="row">
              <div
                class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
              >
                <q-select
                  outlined
                  label="Tipo de Pessoa *"
                  dense
                  class="k-input-pa"
                  v-model="help.typePerson"
                  :options="listTypePerson"
                  lazy-rules
                  :rules="[
                    (val) =>
                      (val && val.length > 0) ||
                      'Por favor, informe o tipo de pessoa',
                  ]"
                />
              </div>
            </div>
            <div class="row">
              <div
                class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
              >
                <k-input
                  dense
                  outlined
                  label="CPF/CNPJ *"
                  lazy-rules
                  maxlength="18"
                  v-model="help.cpf"
                  :rules="[
                    (val) =>
                      (val && val.length > 0) ||
                      'Por favor, informe o CPF/CNPJ',
                  ]"
                />
              </div>
            </div>
            <div class="row">
              <div
                class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
              >
                <q-select
                  outlined
                  label="Assunto *"
                  dense
                  class="k-input-pa"
                  v-model="help.subject"
                  :options="optionsSubject"
                  lazy-rules
                  :rules="[
                    (val) =>
                      (val && val.length > 0) ||
                      'Por favor, informe selecione o Assunto',
                  ]"
                />
              </div>
            </div>
            <div class="row">
              <div
                class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
              >
                <k-input
                  type="textarea"
                  dense
                  outlined
                  label="Mensagem *"
                  lazy-rules
                  v-model="help.message"
                  :rules="[
                    (val) =>
                      (val && val.length > 0) ||
                      'Por favor, insira uma Menssagem',
                  ]"
                />
              </div>
            </div>
          </q-form>
        </q-card-section>
        <div style="margin: 0 auto">
          <q-card flat style="width: 500px; background-color: ">
            <q-card-section>
              <div class="row">
                <q-btn
                  size="md"
                  style="background: #53258e; color: white; width: 100%"
                  label="Enviar"
                  @click="sendHelp()"
                />
              </div>
            </q-card-section>
          </q-card>
        </div>
      </q-card>
    </q-dialog>

    <!-- recuperação de senha -->
    <q-dialog v-model="dialogRefreshPass" persistent>
      <q-card style="min-width: 350px">
        <q-card-section>
          <div class="text-h6">Recuperação de Senha</div>
          <div class="text-caption text-grey">
            Digite o usuário ou o e-mail para receber a nova senha. <br />
            A nova senha será enviada para seu e-mail informado no cadastro.
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <k-input
            v-model="refreshPassword"
            dense
            outlined
            label="Usuário ou E-mail"
            lazy-rules
            :rules="[(val) => (val && val.length > 0) || '']"
            maxlength="100"
            autofocus
          />
        </q-card-section>

        <q-card-actions align="right">
          <div class="row"></div>
          <KButtonCancel :text="'Cancelar'" @click="cancelRefeshPassword()" />

          <KButton :text="'Recuperar'" @click="sendRefreshPassword()" />
        </q-card-actions>
      </q-card>
    </q-dialog>

    <!-- cadastro de usuario  -->
    <q-dialog v-model="dialogNewUser" persistent>
      <q-card style="min-width: 600px">
        <q-card-section>
          <div class="text-h6">Cadastro Payment On Time</div>
          <div class="text-caption text-grey">
            Seu sistema para controle de finanças pessoais. Seja muito
            bem-vindo(a) <br />
          </div>
        </q-card-section>

        <q-card-section class="q-pt-none">
          <div
            class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
          >
            <k-input
              v-model="userRegister.name"
              autofocus
              stack-label
              label="Nome completo *"
            />
          </div>

          <div
            class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
          >
            <k-input
              v-model="userRegister.userName"
              label="Nome de usuário *"
              placeholder="Ex.: mrocha"
              stack-label
            />
          </div>
          <div
            class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
          >
            <k-input
              v-model="userRegister.email"
              stack-label
              label="E-mail *"
            />
          </div>
          <div
            class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12"
          >
            <k-input v-model="userRegister.passwordRegister" placeholder="8 caracteres" stack-label label="Senha *" />
          </div>
        </q-card-section>

        <q-card-actions align="right">
          <div class="row"></div>
          <KButtonCancel :text="'Voltar'" @click="cancelOnSave()" />
          <KButton :text="'Cadastrar'" @click="onSave()" />
        </q-card-actions>
      </q-card>
    </q-dialog>

    <div class="container-fluid">
      <div class="row">
        <div
          class="col-12 col-sm-12 col-md-12 col-lg-6 col-xl-6 col-xxl-6 loginFluid d-flex align-center justify-center"
        >
          <!-- <div style="position: relative; top: 5%">
            <img
              style="height: 30%; width: 70%"
              src="./../../../public/LogoOriginal.png"
            />
          </div> -->
        </div>

        <div
          class="col-12 col-sm-12 col-md-12 col-lg-6 col-xl-6 col-xxl-6 d-flex align-center justify-center"
        >
          <div class="center-card">
            <div style="margin: 0 auto">
              <img
                style="height: 20vh; width: 100%"
                src="./../../../public/LogoOriginal3.png"
              />
            </div>
            <br />
            <!-- <div>
              <q-chip @click="onChangeLanguage('P')" clickable  icon="reviews"  :color="language == 'P' ? 'secondary' :  ''" :text-color="language == 'P' ? 'white' : ''">
                Português
              </q-chip>
              <q-chip @click="onChangeLanguage('E')" clickable  icon="reviews" :color="language == 'E' ? 'secondary' :  ''" :text-color="language == 'E' ? 'white' : ''">
                Inglês
              </q-chip>
              <q-chip @click="onChangeLanguage('S')" clickable  icon="reviews" :color="language == 'S' ? 'secondary' :  ''" :text-color="language == 'S' ? 'white' : ''">
                Espanhol
              </q-chip>
            </div> -->
            <div style="margin: 0 auto">
              <q-card flat style="width: 500px; background-color: ">
                <q-card-section>
                  <div class="row">
                    <q-input
                      v-model="user.user"
                      outlined
                      label="Usuário"
                      style="width: 100%"
                    >
                      <template v-slot:append>
                        <q-icon name="perm_identity" />
                      </template>
                    </q-input>
                  </div>
                  <br />
                  <div class="row">
                    <q-input
                      v-model="user.pass"
                      type="password"
                      outlined
                      label="Senha"
                      style="width: 100%"
                    >
                      <template v-slot:append>
                        <q-icon name="lock" />
                      </template>
                    </q-input>
                  </div>
                  <br />
                  <a class="row" style="float: right; cursor: pointer">
                    <span @click="showRefreshPassword()"
                      >Esqueceu sua senha?</span
                    >
                  </a>
                  <br />
                  <br />
                  <a class="row" style="float: right; cursor: pointer">
                    <span @click="showNewuser()"
                      >Ainda não tem acesso? Criar uma conta</span
                    >
                  </a>
                  <br />
                  <br />
                  <div class="row">
                    <q-btn
                      size="lg"
                      style="background: #53258e; color: white; width: 100%"
                      label="Acessar"
                      @click="callAccess()"
                    />
                  </div>
                </q-card-section>
              </q-card>
            </div>
            <!-- <div class="row">
              <div
                class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 col-xxl-12 q-pa-md q-gutter-sm"
                style="position: relative; top: -2vh"
              >
                <q-btn
                  title="Como podemos ajudá-lo?"
                  round
                  style="
                    background: #53258e;
                    color: white;
                    float: right;
                    width: 50px;
                    height: 50px;
                  "
                  icon="mode_comment"
                  @click="showHelp()"
                />
              </div>
            </div> -->
          </div>
        </div>
      </div>
      <div class="row footer d-flex align-center justify-center">
        <span class="footer-text"
          >{{
            "@" +
            currentYear +
            " Sistema para controle de despesas e receitas pessoais"
          }}
        </span>
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

import routes from "@/modules/router";
import KButtonAction from "../../../../components/KButtonAction.vue";
import KButtonCancel from "./../../components/KButtonCancel.vue";
import KButton from "./../../components/KButton.vue";
import { IResult } from "@/core/types/IKResult";
import KDialogQuestion from "@/components/KDialogQuestion.vue";
import KInput from "@components/KInput.vue";
import { ILogin } from "./ILogin";
import { storeUser } from "@/core/store/userStore";
import { IHelp } from "@/core/types/IHelp";
import { IUsers } from "@/core/types/IUsers";

const router = useRouter();
const userStore = storeUser();
const userRegister = ref(new IUsers());

const language = ref("P");

const help = ref(new IHelp());
const dialogMessageException = ref(false);
const optionsSubject = ref(["Problema", "Dúvida", "Sugestão"]);

const dados = ref([]);
const listaFuncionariosAux = ref([]);

const dialogQuestion = ref(false);
const dialogMessage = ref(false);
const dialogMessageText = ref("");
const dialogMessageTitle = ref("");
const dialogMessageType = ref("W");
const dialogLoading = ref(false);
const dialogNotify = ref(false);
const dialogRefreshPass = ref(false);
const dialogNewUser = ref(false);
const isPwd = ref(true);
const dialogHelp = ref(false);
const refreshPassword = ref("");
const currentYear = ref("");
const listTypePerson = ref(["Jurídico", "Físico"]);

const user = ref({} as ILogin);

function cancelOnSave() {
  dialogNewUser.value = false;
  userRegister.value = new IUsers();
}

function validateFields() {
  if (!userRegister.value.name) {
    dialogMessageText.value = "Informe o nome do Usuário.";
    dialogNotify.value = true;
    return true;
  }

  if (!userRegister.value.email) {
    dialogMessageText.value = "Informe o E-mail do Usuário.";
    dialogNotify.value = true;
    return true;
  }

  if (!userRegister.value.userName) {
    dialogMessageText.value = "Informe o Login de acesso para este Usuário.";
    dialogNotify.value = true;
    return true;
  }


if (!userRegister.value.passwordRegister) {
    dialogMessageText.value = "Informe a senha de acesso.";
    dialogNotify.value = true;
    return true;
  }

  return false;
}

function validateEmail(email: string) {
  let userEmail = email.substring(0, email.indexOf("@"));
  let dominio = userRegister.value.email.substring(
    email.indexOf("@") + 1,
    email.length
  );

  if (
    userEmail.length >= 1 &&
    dominio.length >= 3 &&
    userEmail.search("@") == -1 &&
    dominio.search("@") == -1 &&
    userEmail.search(" ") == -1 &&
    dominio.search(" ") == -1 &&
    dominio.search(".") != -1 &&
    dominio.indexOf(".") >= 1 &&
    dominio.lastIndexOf(".") < dominio.length - 1
  ) {
    return false;
  } else {
    dialogMessageText.value =
      "E-mail invalido. Por favor, verifique os dados do e-mail informado.";
    dialogNotify.value = true;
    return true;
  }
}

async function onSave() {
  if (!validateFields()) {
    if (!validateEmail(userRegister.value.email)) {
      dialogLoading.value = true;

      userRegister.value.isActive = true;
      userRegister.value.name = userRegister.value.name.toUpperCase();
      userRegister.value.normalizedUserName = userRegister.value.name;
      userRegister.value.normalizedEmail = userRegister.value.email;

      let result = await Api.postResult<IResult>(
        "/ServiceUser/register",
        userRegister.value
      );
      dialogLoading.value = false;
      dialogMessage.value = true;
      dialogMessageTitle.value = result.data.sucesso ? "Olá :)" : "Erro";
      dialogMessageText.value =
        result.data.message != null ? result.data.message : "";

      dialogMessageException.value = !result.data.sucesso ? true : false;

      if(result.data.sucesso){
        dialogNewUser.value = false;
      }
      
    }
  }
}

function closeNotify() {
  dialogNotify.value = false;
}

function cancelRefeshPassword() {
  dialogRefreshPass.value = false;
}

function validateFieldsRefreshPass() {
  if (!refreshPassword.value) {
    dialogMessageText.value =
      "Informe o usuario para o o envio da nova senha de acesso.";
    dialogNotify.value = true;
    return true;
  }

  return false;
}

async function sendRefreshPassword() {
  if (!validateFieldsRefreshPass()) {
    dialogLoading.value = true;

    let refreshPass: any = {
      userNameOrEmail: refreshPassword.value,
    };

    let result = await Api.postResult<IResult>(
      "/ServiceUser/refreshpass",
      refreshPass
    );
    dialogLoading.value = false;
    dialogMessage.value = true;
    dialogMessageTitle.value = result.data.sucesso ? "Informação" : "Erro";
    dialogMessageText.value =
      result.data.message != null ? result.data.message : "";
    
    dialogRefreshPass.value = false;
    
     
  }
}

function showNewuser() {
  dialogNewUser.value = true;
  userRegister.value = new IUsers();
}

function showRefreshPassword() {
  refreshPassword.value = user.value.user;
  dialogRefreshPass.value = true;
}

function closeDialogMessage() {
  dialogMessage.value = false;
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
  if (!user.value.user || !user.value.pass) {
    dialogMessageTitle.value = "Login de acesso Controle de Orçamento";
    dialogMessageText.value =
      "Para logar é necessário informe o usuário e a senha.";
    dialogMessage.value = true;
    return true;
  }

  return false;
}

function validateHelp() {
  if (!help.value.name) {
    dialogMessageText.value = "Informe o seu Nome.";
    dialogNotify.value = true;
    return true;
  }
  if (!help.value.email) {
    dialogMessageText.value = "Informe o seu E-mail.";
    dialogNotify.value = true;
    return true;
  }
  if (!help.value.typePerson) {
    dialogMessageText.value = "Seleciona o tipo de pessoa.";
    dialogNotify.value = true;
    return true;
  }
  if (!help.value.cpf) {
    dialogMessageText.value = "Informe o seu CPF.";
    dialogNotify.value = true;
    return true;
  }
  if (!help.value.subject) {
    dialogMessageText.value = "Seleciona um assunto.";
    dialogNotify.value = true;
    return true;
  }
  if (!help.value.message) {
    dialogMessageText.value = "Informe uma menssagem.";
    dialogNotify.value = true;
    return true;
  }

  return false;
}

function showHelp() {
  dialogHelp.value = true;
  help.value = new IHelp();
}

async function sendHelp() {
  console.log(help.value);
  return;
  if (!validateHelp()) {
    dialogLoading.value = true;
    dialogHelp.value = false;

    let result = await Api.postResult<IResult>("/Help", help.value);
    dialogLoading.value = false;
    dialogMessage.value = true;
    dialogMessageTitle.value = result.data.sucesso ? "Informação" : "Erro";
    dialogMessageText.value =
      result.data.message != null ? result.data.message : "";
    help.value = new IHelp();
    dialogMessageException.value = !result.data.sucesso ? true : false;
  }
}

async function callAccess() {
  if (!validate()) {
    dialogLoading.value = true;
    let result: IResult;
    result = await Api.postResult<IResult>("/ServiceUser/login", user.value);

    if (result.data.sucesso) {
      result = result.data.tRetorno;
      userStore.setUser(result);

      router.push("/dashboard");
    } else {
      dialogMessageTitle.value = "Login de acesso Controle de Orçamento";
      dialogMessageText.value = result.data.message;
      dialogMessage.value = true;
    }
    dialogLoading.value = false;
  }
}

function onChangeLanguage(lang: string) {
  language.value = lang;

  userStore.setLanguage(lang);
}

onMounted(() => {
  currentYear.value = new Date().getUTCFullYear().toString();
  userStore.setLanguage("P");
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
.loginFluid {
  background-image: url("./../../../public/login.png");
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
  top: 20%;
}

.footer {
  background-color: black;
  text-align: center;
  height: 3vh;
}

.footer-text {
  font: normal normal 14px/17px Helvetica;
  letter-spacing: 0px;
  color: #ffffff;
  opacity: 1;
  text-align: center;
}
</style>
