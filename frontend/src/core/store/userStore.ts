import { defineStore } from "pinia";
import { useStorage } from "@vueuse/core";

import Security from "../../modules/security";

export const storeUser = defineStore("user-store", {
  state: () => {
    return {
      user: useStorage("user", Security.encrypted("{}")),
      logado: useStorage("logado", Security.encrypted("false")),
      language: useStorage("language", Security.encrypted("P")),
    };
  },

  getters: {
    getUser(state) {
      return JSON.parse(Security.decrypted(state.user));
    },

    getLanguage(state) {
      return  Security.decrypted(state.language);
    },

    isLogado(state) {
      return Security.decrypted(state.logado) === "true";
    },
  },

  actions: {
    
    setLanguage(lang: string){
      this.language = Security.encrypted(JSON.stringify(lang));
    },

    setUser(userData: object) {
      this.user = Security.encrypted(JSON.stringify(userData));
      this.logado = Security.encrypted("true");
    },

    setUserLogout() {
      this.user = Security.encrypted("{}");
      this.logado = Security.encrypted("false");
    },
  },
});
