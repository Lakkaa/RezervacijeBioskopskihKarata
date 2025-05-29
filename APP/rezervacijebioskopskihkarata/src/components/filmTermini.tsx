import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import api from "../api/axios.ts";
import { Film } from "../types/Film.ts";
import Header from "./header.tsx";
import { FilmProjekcija } from "../types/filmProjekcija.ts";

export default function FilmTermini() {
  const { id } = useParams();
  const [film, setFilm] = useState<Film | null>(null);
  const [projekcije, setProjekcije] = useState<FilmProjekcija[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);

        const filmResponse = await api.get(`/api/Filmovi/${id}`);
        setFilm(filmResponse.data);

        const projekcijeResponse = await api.get(
          `/api/Filmovi/projekcijeByFilm/${id}`
        );
        console.log("API Response:", projekcijeResponse.data);
        setProjekcije(projekcijeResponse.data);

        setError(null);
      } catch (err) {
        console.error(err);
        setError("Nije moguće učitati podatke o projekcijama.");
      } finally {
        setLoading(false);
      }
    };

    if (id) {
      fetchData();
    }
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
        <div className="row mb-4">
          <div className="col-md-3">
            {film.slika && (
              <img
                src={getImagePath(film.slika)}
                alt={film.naziv}
                className="img-fluid rounded shadow-sm"
              />
            )}
          </div>
          <div className="col-md-9">
            <h1 className="mb-3">{film.naziv} - Termini projekcija</h1>
            <div className="d-flex align-items-center mb-3">
              <div className="badge bg-primary p-2 me-2">
                <i className="bi bi-clock"></i> {film.trajanje} min
              </div>
            </div>
            <p>{film.opis}</p>
          </div>
        </div>

        <div className="card shadow-sm">
          <div className="card-header bg-light">
            <h3 className="mb-0">Dostupni termini</h3>
          </div>
          <div className="card-body">
            {projekcije.length === 0 ? (
              <div className="alert alert-info">
                Trenutno nema dostupnih termina za ovaj film.
              </div>
            ) : (
              <div className="table-responsive">
                <table className="table table-hover">
                  <thead>
                    <tr>
                      <th>Sala</th>
                      <th>Dan</th>
                      <th>Termin</th>
                      <th>Akcija</th>
                    </tr>
                  </thead>
                  <tbody>
                    {projekcije.map((projekcija) => (
                      <tr key={projekcija.projekcijaId}>
                        <td>{projekcija.salaNaziv}</td>
                        <td>{projekcija.danNaziv}</td>
                        <td>{projekcija.terminNaziv}</td>
                        <td>
                          <Link
                            to={`/filmovi/${id}/termini/${projekcija.projekcijaId}`}
                            className="btn btn-sm btn-success"
                          >
                            Rezerviši kartu
                          </Link>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}
          </div>
        </div>

        <div className="mt-4 text-end">
          <Link to={`/filmovi/${id}`} className="btn btn-outline-secondary">
            &larr; Nazad na detalje filma
          </Link>
        </div>
      </main>
    </>
  );
}
