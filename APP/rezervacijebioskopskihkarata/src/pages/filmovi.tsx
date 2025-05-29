import { useEffect, useState } from "react";
import api from "../api/axios.ts";
import { Film } from "../types/Film.ts";
import { FilmLista } from "../components/filmLista.tsx";
import Header from "../components/header";

export default function Filmovi() {
  const [filmovi, setFilmovi] = useState<Film[]>([]);

  useEffect(() => {
    api
      .get("/api/Filmovi")
      .then((res) => setFilmovi(res.data))
      .catch((err) => console.error(err));
  }, []);

  return (
    <>
      <Header />
      <main>
        <h1 className="display-3 text-center my-4">Filmovi</h1>
        <div className="p-4">
          <FilmLista filmovi={filmovi} />
        </div>
      </main>
    </>
  );
}
