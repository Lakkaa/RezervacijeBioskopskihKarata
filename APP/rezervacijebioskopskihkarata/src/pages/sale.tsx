import { useEffect, useState } from "react";
import api from "../api/axios.ts";
import { Sala } from "../types/sala.ts";
import { SaleLista } from "../components/saleLista.tsx";
import Header from "../components/header";

export default function Sale() {
  const [sale, setSale] = useState<Sala[]>([]);

  useEffect(() => {
    api
      .get("/api/Sale")
      .then((res) => setSale(res.data))
      .catch((err) => console.error(err));
  }, []);

  return (
    <>
      <Header />
      <main>
        <h1 className="display-3 text-center my-4">Sale</h1>
        <div className="p-4">
          <SaleLista sale={sale} />
        </div>
      </main>
    </>
  );
}
