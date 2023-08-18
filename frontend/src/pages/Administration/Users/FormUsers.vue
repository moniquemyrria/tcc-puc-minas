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

    <!-- <KBreadCrumbs :data="dataBreadCrumbs" /> -->

    <KNotify
      :dialog="dialogNotify"
      :text="dialogMessageText"
      :type="dialogMessageType"
      @kNotifyClose="closeNotify"
    />

    <br />

    <div class="row">
      <div class="col-12" style="text-align: start">
        <h5 style="margin: 4px 0px 20px" class="title">
          {{ idUser == "" ? "Novo Usuário" : "Meus dados" }}
        </h5>
        <div class="col-12 sub-title" style="text-align: start">
          {{
            idUser == ""
              ? "Preencha os campos para criar um novo Usuário"
              : "Altere seus dados de cadastro"
          }}
        </div>
      </div>
    </div>
    <br />

    <KCardForm>
      <q-card-section class="q-pt-none" align="center">
        <q-list bordered class="rounded-borders">
          <q-expansion-item
            :model-value="true"
            style="text-align: left"
            label="Dados do Usuário"
            expand-separator
          >
            <q-card>
              <q-card-section>
                <q-form>
                  <div class="row">
                    <div
                      class="col-12 col-sm-12 col-md-5 col-lg-5 col-xl-5 col-xxl-5"
                    >
                      <k-input
                        v-model="user.name"
                        dense
                        outlined
                        label="Seu Nome *"
                        lazy-rules
                        :rules="[(val) => (val && val.length > 0) || '']"
                        maxlength="100"
                      />
                    </div>

                    <div
                      class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4 col-xxl-4"
                    >
                      <k-input
                        v-model="user.email"
                        dense
                        outlined
                        label="Seu E-mail *"
                        lazy-rules
                        :rules="[(val) => (val && val.length > 0) || '']"
                        maxlength="100"
                      />
                    </div>
                    <div
                      class="col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3 col-xxl-3"
                    >
                      <k-input
                        v-model="user.userName"
                        dense
                        outlined
                        label="Seu Login "
                        lazy-rules
                        :rules="[(val) => (val && val.length > 0) || '']"
                        maxlength="30"
                        readonly
                      />
                    </div>
                  </div>


                </q-form>
              </q-card-section>
            </q-card>
            <br />
          </q-expansion-item>
        </q-list>
      </q-card-section>

      <q-separator />

      <q-card-actions align="right">
        <KButtonCancel :text="'Cancelar'" @click="cancelEditUser()" />

        <KButton v-if="route.params.id" :text="'Salvar'" @click="onEdit()" />
        <KButton v-else :text="'Salvar'" @click="onSave()" />
      </q-card-actions>
    </KCardForm>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from "vue";
import { useRouter, useRoute } from "vue-router";
import { useQuasar } from "quasar";
import KBreadCrumbs from "./../../../components/KBreadCrumbs.vue";
import KCardForm from "./../../../components/KCardForm.vue";
import KButton from "./../../../components/KButton.vue";
import KButtonCancel from "./../../../components/KButtonCancel.vue";
import KButtonAction from "../../../components/KButtonAction.vue";
import KNotify from "../../../components/KNotify.vue";
import KInput from "@components/KInput.vue";
import Api from "../../../core/api/api";
import KDialog from "@/components/KDialog.vue";
import KLoading from "../../../components/KLoading.vue";
import { IResult } from "@/core/types/IKResult";
import { IUsers } from "@/core/types/IUsers";
import { parse } from "@babel/parser";
import { IWebCodes } from "./Types/IWebCodes";

const tab = ref("movies");
const listApprovalLevels = ref([] as any[]);
const listApprover = ref(["1 - Aprovador Geral", "2 - Aprovador Moderado"]);
const user = ref(new IUsers());

const listYesOrNo = ref(["Sim", "Não"]);

const listLevelsUser = ref([
  "Usuário Normal",
  "Usuário Aprovador Intermediario",
]);

const listVpcApprovalLimit = ref([
  "Acima de",
  "Abaixo de",
  "Apenas validação",
  "Nenhum",
]);
const idUser = ref("");
const value = ref("");
const router = useRouter();
const route = useRoute();

const dialogMessage = ref(false);
const dialogMessageText = ref("");
const dialogMessageTitle = ref("");
const dialogMessageType = ref("W");
const dialogLoading = ref(false);
const dialogNotify = ref(false);
const dialogMessageException = ref(false);

const userStatus = ref("");
const listUserStatus = ref(["Ativo", "Bloqueado"]);

const dataBreadCrumbs = ref([
  {
    label: "Lista Níveis do Cargos",
    router: "/listJobLevels",
  },
]);

const disableApprovalLevel = ref(true);

const filter = ref("");

const headers = ref([
  {
    name: "cod",
    align: "left",
    label: "Código",
    field: "cod",
    sortable: false,
  },

  {
    name: "name",
    align: "left",
    label: "Nome",
    field: "name",
    sortable: true,
  },
   {
    name: "typePerson",
    align: "left",
    label: "Tipo",
    field: "typePerson",
    sortable: true,
  },
  {
    name: "webCode",
    align: "left",
    label: "Código Web",
    field: "webCode",
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


function showList() {
  router.push("/listUsers");
}

function cancelEditUser() {
  router.push("/");
}

function callItemsBreadCrumbs() {
  dataBreadCrumbs.value.push(
    route.params.id
      ? {
          label: "Editar Usuário",
          router: "/",
        }
      : {
          label: "Novo Usuário",
          router: "/formUsers",
        }
  );
}


function closeDialogMessage() {
  dialogMessage.value = false;

  if (!dialogMessageException.value) {
    showList();
  }
}

function closeNotify() {
  dialogNotify.value = false;
}

function validateEmail(email: string) {
  let userEmail = email.substring(0, email.indexOf("@"));
  let dominio = user.value.email.substring(
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

function validateFields() {
  if (!user.value.name) {
    dialogMessageText.value = "Informe o nome do Usuário.";
    dialogNotify.value = true;
    return true;
  }

  if (!user.value.email) {
    dialogMessageText.value = "Informe o E-mail do Usuário.";
    dialogNotify.value = true;
    return true;
  }

  if (!user.value.userName) {
    dialogMessageText.value = "Informe o Login de acesso para este Usuário.";
    dialogNotify.value = true;
    return true;
  }


  return false;
}

async function onSave() {
  if (!validateFields()) {
    if (!validateEmail(user.value.email)) {
      dialogLoading.value = true;

      user.value.isActive = userStatus.value == "Ativo" ? true : false;
      user.value.name = user.value.name.toUpperCase();
      user.value.normalizedUserName = user.value.name;
      user.value.normalizedEmail = user.value.email;
        value.value == "" ? 0 : parseFloat(value.value.replace(",", "."));

      let result = await Api.postResult<IResult>(
        "/ServiceUser/register",
        user.value
      );
      dialogLoading.value = false;
      dialogMessage.value = true;
      dialogMessageTitle.value = result.data.sucesso ? "Informação" : "Erro";
      dialogMessageText.value =
        result.data.message != null ? result.data.message : "";

      dialogMessageException.value = !result.data.sucesso ? true : false;
    }
  }
}

async function onEdit() {
  if (!validateFields()) {
    if (!validateEmail(user.value.email)) {
      dialogLoading.value = true;
      user.value.dateUpdate = new Date();
      user.value.isActive = userStatus.value == "Ativo" ? true : false;
      user.value.name = user.value.name.toUpperCase();
      user.value.normalizedUserName = user.value.name;
      user.value.normalizedEmail = user.value.email;
        value.value == "" ? 0 : parseFloat(value.value.replace(",", "."));

      let result = await Api.putResult<IResult>("/ServiceUser", user.value);
      dialogLoading.value = false;
      dialogMessage.value = true;
      dialogMessageTitle.value = result.data.sucesso ? "Informação" : "Erro";
      dialogMessageText.value =
        result.data.message != null ? result.data.message : "";
    }
  }
}

async function showUser() {
  if (route.params.id) {
    idUser.value = route.params.id.toString();

    dialogLoading.value = true;
    let result: IResult = {} as IResult;
    result = await Api.getIdResult<IResult>(
      "/ServiceUser",
      idUser.value.toString()
    );

    if (result.data.sucesso) {
      let itemResult = result.data.tRetorno;

      userStatus.value = itemResult.isActive ? "Ativo" : "Bloqueado";

      value.value = itemResult.value;

      user.value = itemResult;
    }

    dialogLoading.value = false;
  } 
}

onMounted(() => {
  callItemsBreadCrumbs();
  showUser();
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