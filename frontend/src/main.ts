import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { Quasar, Notify } from 'quasar'
import quasarLang from 'quasar/lang/pt-BR'

import router from './modules/router'

import '@quasar/extras/material-icons/material-icons.css'

import VueApexCharts from "vue3-apexcharts";
import ApexCharts from "apexcharts";

// Import Quasar css
import 'quasar/dist/quasar.css'

import './css/config.css'
import './style.css'
import App from './App.vue'

const pinia = createPinia()

const app = createApp(App);
app.config.globalProperties.$apexcharts = ApexCharts;
app.use(VueApexCharts);

app.use(pinia)

app.use(Quasar, {
    plugins: {
      Notify
    }, // import Quasar plugins and add here
    lang: quasarLang,
    
  config: {
    brand: {
      primary: '#7E7E7E',
      secondary: "#53258E",
      error: "#C10015",
      warning: '#F2C037',
      confirm: "#21BA45",

      // ... or all other brand colors
    },
    notify: {}, // default set of options for Notify Quasar plugin
    /*loading: {...}, // default set of options for Loading Quasar plugin
    loadingBar: { ... }, // settings for LoadingBar Quasar plugin
    // ..and many more (check Installation card on each Quasar component/directive/plugin)
    */
  }
  
})
app.use(router)
app.mount('#app')

