import { Film } from "../types/Film.ts";
import styles from "./FilmLista.module.css";
import { Link } from "react-router-dom";

interface FilmListaProps {
  filmovi: Film[];
}

export function FilmLista({ filmovi }: FilmListaProps) {
  const getImagePath = (path: string) => {
    return (
      path?.replace(
        "D:\\fakultet\\Apeiron\\Diplomski\\APP\\rezervacijebioskopskihkarata\\public",
        ""
      ) || ""
    );
  };

  return (
    <div className="row">
      {filmovi.map((film) => (
        <div key={film.filmId} className="col-md-4 mb-4">
          <div
            className={`card border border-secondary shadow-sm ${styles.hoverEffect}`}
          >
            <div className="position-relative">
              <img
                src={getImagePath(film.slika || "")}
                alt={film.naziv}
                className="card-img-top"
              />
              <h2 className={styles.titleOverlay}>{film.naziv}</h2>
            </div>
            <div className="card-body">
              <div className={styles.buttonContainer}>
                <Link
                  to={`/filmovi/${film.filmId}`}
                  className="btn btn-success"
                >
                  Prikaži detalje
                </Link>
              </div>
            </div>
          </div>
        </div>
      ))}
    </div>
  );
}
