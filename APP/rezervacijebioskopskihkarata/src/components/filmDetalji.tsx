import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import api from "../api/axios.ts";
import { Film } from "../types/Film.ts";
import Header from "./header.tsx";

export default function FilmDetalji() {
  const { id } = useParams();
  const [film, setFilm] = useState<Film | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    setLoading(true);
    api
      .get(`/api/Filmovi/${id}`)
      .then((res) => {
        setFilm(res.data);
        setLoading(false);
      })
      .catch((err) => {
        console.error(err);
        setError("Nije moguće učitati detalje filma.");
        setLoading(false);
      });
  }, [id]);

  const getImagePath = (path: string) => {
    return path?.replace(
      "D:\\fakultet\\Apeiron\\Diplomski\\APP\\rezervacijebioskopskihkarata\\public",
      ""
    );
  };

  if (loading) return <div className="text-center p-5">Učitavanje...</div>;
  if (error) return <div className="text-center p-5 text-danger">{error}</div>;
  if (!film) return <div className="text-center p-5">Film nije pronađen</div>;

  return (
    <>
      <Header />
      <main className="container py-5">
        <div className="row">
          <div className="col-md-4">
            {film.slika && (
              <img
                src={getImagePath(film.slika)}
                alt={film.naziv}
                className="img-fluid rounded shadow-sm"
              />
            )}
          </div>
          <div className="col-md-8">
            <h1 className="display-4 mb-3">{film.naziv}</h1>
            <p className="lead">{film.opis}</p>
            <div className="d-flex align-items-center mt-4">
              <div className="badge bg-primary p-2 me-3">
                <i className="bi bi-clock"></i> {film.trajanje} min
              </div>
            </div>

            <div className="mt-4 d-flex justify-content-between">
              <Link to={`/filmovi/${id}/termini`} className="btn btn-primary">
                <i className="bi bi-calendar-event me-1"></i> Pogledaj termine
              </Link>
              <Link to="/filmovi" className="btn btn-outline-secondary">
                &larr; Nazad na filmove
              </Link>
            </div>
          </div>
        </div>
      </main>
    </>
  );
}
