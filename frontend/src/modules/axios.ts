// API
import axios from 'axios'
import { storeUser } from "@/core/store/userStore";
const userStore = storeUser();

const token = userStore.getUser ? userStore.getUser.token: ''

const urlapi = import.meta.env.VITE_API_REST_URL

var axiosInstance = axios.create({
  baseURL: urlapi,
  responseType: "json",
  responseEncoding: "utf8",
  headers: {
    Accept: "application/json",
    "Content-Type": "application/json;charset=UTF-8",
    Authorization: "Bearer " + token
  }
});



export default axiosInstance;

