import axios from "axios";

const api = axios.create({
  baseURL: "https://localhost:7043",
});

export default api;
