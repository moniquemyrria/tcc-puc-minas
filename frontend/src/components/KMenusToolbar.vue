<template>
  <!-- <div class="q-pa-md"> -->
  <div class="">
    <q-layout
      view="hHh Lpr lff"
      container
      style="min-height: 100vh"
      class="shadow-2"
    >
      <q-header elevated class="bg-white text-black">
        <q-toolbar>
          <q-btn flat @click="drawer = !drawer" round dense icon="menu" />
          <q-toolbar-title style="text-align: start"
            >Payment On Time</q-toolbar-title
          >
          <!-- <i icon="Notifications"></i>
          <span>Ola: Usuario</span>
          <q-btn flat style="color: black" float-right icon="logout" /> -->
          <q-btn-dropdown flat color="black" :label="'Olá, ' + user.name">
            <div class="q-pa-md" style="min-width: 300px">
              <q-list>
                <q-item clickable v-ripple>
                  <q-item-section @click="perfil()">Meus Dados</q-item-section>
                  <q-item-section avatar>
                    <q-icon color="primary" name="face" />
                  </q-item-section>
                </q-item>
              </q-list>
              <q-separator />
              <q-list>
                <q-item clickable v-ripple>
                  <q-item-section @click="refreshPassword()"
                    >Trocar senha</q-item-section
                  >
                  <q-item-section avatar>
                    <q-icon color="primary" name="lock" />
                  </q-item-section>
                </q-item>
              </q-list>
              <q-separator />
              <q-list>
                <q-item clickable v-ripple>
                  <q-item-section @click="exitApp()">Sair</q-item-section>
                  <q-item-section avatar>
                    <q-icon color="primary" name="logout" />
                  </q-item-section>
                </q-item>
              </q-list>
            </div>
          </q-btn-dropdown>
        </q-toolbar>
      </q-header>

      <!-- mini-to-overlay -> sombra, bordered -> borda -->
      <q-drawer
        v-model="drawer"
        show-if-above
        :mini="miniState"
        @mouseover="miniState = false"
        @mouseout="miniState = true"
        :width="260"
        :breakpoint="500"
        class="bg-white"
      >
        <q-scroll-area class="fit" style="border-right: 1px solid #ddd">
          <q-list padding>
            <template v-for="(item, index) in MenuItem" :key="index">
              <q-item
                v-if="!item.subMenu.length"
                active
                v-ripple
                :to="item.url"
                clickable
                style="text-align: left; color: black"
              >
                <q-item-section avatar style="font-size: 10pt">
                  <q-icon :name="item.icon" />
                </q-item-section>

                <q-item-section>
                  {{ item.title }}
                </q-item-section>
              </q-item>

              <q-expansion-item
                v-else
                :icon="item.icon"
                :label="item.title"
                active
                style="text-align: left"
              >
                <q-item
                  v-for="(item2, index2) in item.subMenu"
                  :key="index2"
                  active
                  clickable
                  v-ripple
                  :to="item2.url"
                >
                  <q-item-section
                    style="align-items: start; position: relative; left: 53px"
                  >
                    {{ item2.title }}
                  </q-item-section>
                  <q-item-section avatar>
                    <q-icon :name="item2.icon" />
                  </q-item-section>
                </q-item>
              </q-expansion-item>
            </template>
          </q-list>
        </q-scroll-area>
      </q-drawer>

      <q-page-container>
        <q-page padding>
          <router-view></router-view>
        </q-page>
      </q-page-container>

      <!-- <q-footer elevated class="bg-grey-8 text-white">
      <q-toolbar>
        <q-toolbar-title>
          <q-avatar>
            <img src="https://cdn.quasar.dev/logo-v2/svg/logo-mono-white.svg">
          </q-avatar>
          <div>Title</div>
        </q-toolbar-title>
      </q-toolbar>
    </q-footer> -->
    </q-layout>
  </div>
</template>

<script setup lang="ts">
import { storeUser } from "@/core/store/userStore";
import routes from "@/modules/router";
import { ref, onMounted, watch } from "vue";
import { useRouter, useRoute } from "vue-router";

const drawer = ref(false);
const miniState = ref(true);
const router = useRouter();

const userStore = storeUser();

const user = userStore.getUser ? userStore.getUser.user : "";

const MenuItem = ref([
  { title: "Dashboard", icon: "home", url: "/dashboard", subMenu: [] },

  {
    title: "Administração",
    icon: "settings",
    url: "/listLevels",
    subMenu: [
      //{ title: "Cadastro de Usuários", icon: "", url: "/listUsers" },
      { title: "Categoria de Receitas", icon: "", url: "/listRevenueCategory" },
      { title: "Categoria de Despesa", icon: "", url: "/listExpenseCategory" },
      { title: "Categoria de Conta", icon: "", url: "/listAccountCategory" },
      { title: "Fornecedores", icon: "", url: "/listTypePerson" },
      { title: "Contas", icon: "", url: "/listAccount" },
      { title: "Formas de Pagamento", icon: "", url: "/listPaymentMethods" },
    ],
  },

  {
    title: "Receitas",
    icon: "savings",
    url: "/listRevenue",
    subMenu: [],
  },

  {
    title: "Despesas",
    icon: "payments",
    url: "/listExpense",
    subMenu: [],
  },

  // {
  //   title: "Produtos",
  //   icon: "inventory_2",
  //   url: "",
  //   subMenu: [{ title: "Todos os Produtos", icon: "", url: "/listProducts" }],
  // },
]);

function exitApp() {
  router.push("/");
  userStore.setLanguage("P");
}

function refreshPassword() {
  router.push("/refreshPassword");
}

function perfil() {
  router.push("/editUsers/" + user.id);
}

onMounted(() => {});
</script>
