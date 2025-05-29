import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import api from "../api/axios";
import { Sjediste } from "../types/sjediste";
import { Film } from "../types/Film";
import { FilmProjekcija } from "../types/filmProjekcija";
import { Korisnik, RezervacijaRequest } from "../types/korisnik";
import Header from "./header";
import styles from "./sjedistaSala.module.css";
import RezervacijaModal from "./rezervacijaModal";

export default function SjedistaSala() {
  const { filmId, projekcijaId } = useParams();
  const navigate = useNavigate();

  const [sjedista, setSjedista] = useState<Sjediste[]>([]);
  const [film, setFilm] = useState<Film | null>(null);
  const [projekcija, setProjekcija] = useState<FilmProjekcija | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedSjedista, setSelectedSjedista] = useState<Sjediste[]>([]);
  const [showModal, setShowModal] = useState(false);
  const [justReservedSjedista, setJustReservedSjedista] = useState<number[]>(
    []
  );

  const seatPrice = 10;

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);

        const sjedistaResponse = await api.get(
          `/api/Sjedista/Projekcija/${projekcijaId}`
        );
        console.log("Seats data:", sjedistaResponse.data);
        setSjedista(sjedistaResponse.data);

        const filmResponse = await api.get(`/api/Filmovi/${filmId}`);
        setFilm(filmResponse.data);

        // info o projekcijama po filmu
        const projekcijeResponse = await api.get(
          `/api/Filmovi/projekcijeByFilm/${filmId}`
        );
        const allProjekcije = projekcijeResponse.data;

        const currentProjekcija = allProjekcije.find(
          (p: FilmProjekcija) => p.projekcijaId === Number(projekcijaId)
        );

        if (currentProjekcija) {
          setProjekcija(currentProjekcija);
        } else {
          const projekcijaResponse = await api.get(
            `/api/Projekcije/Details/${projekcijaId}`
          );
          setProjekcija(projekcijaResponse.data);
        }

        setError(null);
      } catch (err) {
        console.error(err);
        setError("Nije moguće učitati podatke o sjedištima.");
      } finally {
        setLoading(false);
      }
    };

    if (projekcijaId && filmId) {
      fetchData();
    }
  }, [projekcijaId, filmId]);

  const handleSjedisteClick = (sjediste: Sjediste) => {
    if (sjediste.isReserved) return;

    const isAlreadySelected = selectedSjedista.some(
      (s) => s.sjedisteId === sjediste.sjedisteId
    );

    if (isAlreadySelected) {
      setSelectedSjedista(
        selectedSjedista.filter((s) => s.sjedisteId !== sjediste.sjedisteId)
      );
    } else {
      setSelectedSjedista([...selectedSjedista, sjediste]);
    }
  };

  const getStatusClass = (
    isReserved: boolean,
    isSelected: boolean,
    sjedisteId: number
  ) => {
    if (isSelected) return styles.odabrano;

    const isJustReserved = justReservedSjedista.includes(sjedisteId);
    if (isJustReserved) return styles.justReserved;

    return isReserved ? styles.zauzeto : styles.slobodno;
  };

  const handleReservationClick = () => {
    if (selectedSjedista.length === 0) {
      alert("Molimo vas da izaberete barem jedno sjedište.");
      return;
    }
    setShowModal(true);
  };

  const handleModalClose = () => {
    setShowModal(false);
  };

  const handleConfirmReservation = async (korisnik: Korisnik) => {
    try {
      let korisnikId;
      try {
        const korisnikResponse = await api.post("/api/Korisnici", {
          ime: korisnik.ime,
          prezime: korisnik.prezime,
          email: korisnik.email,
        });
        korisnikId = korisnikResponse.data.korisnikId;
      } catch (error: any) {
        if (error.response && error.response.status === 409) {
          const existingUserResponse = await api.get(
            `/api/Korisnici/ByEmail/${korisnik.email}`
          );
          korisnikId = existingUserResponse.data.korisnikId;
        } else {
          throw error;
        }
      }

      const seatIds = selectedSjedista.map((s) => s.sjedisteId);

      await api.post("/api/Rezervacije/CreateWithSeats", {
        korisnikId: korisnikId,
        projekcijaId: Number(projekcijaId),
        sjedistaIds: seatIds,
      });

      setShowModal(false);
      setJustReservedSjedista(seatIds);
      setSelectedSjedista([]);
      alert(`Uspješno ste rezervisali ${seatIds.length} kartu/e!`);

      const sjedistaResponse = await api.get(
        `/api/Sjedista/Projekcija/${projekcijaId}`
      );
      setSjedista(sjedistaResponse.data);
    } catch (err) {
      console.error("Greska prilikom rezervacije:", err);
      alert(
        "Došlo je do greške prilikom rezervacije. Molimo pokušajte ponovo."
      );
    }
  };

  const getImagePath = (path: string) => {
    return path?.replace(
      "D:\\fakultet\\Apeiron\\Diplomski\\APP\\rezervacijebioskopskihkarata\\public",
      ""
    );
  };

  const groupedSjedista: Record<string, Sjediste[]> = {};

  if (sjedista && sjedista.length > 0) {
    sjedista.forEach((sjediste) => {
      const rowKey = sjediste.red || "0";
      if (!groupedSjedista[rowKey]) {
        groupedSjedista[rowKey] = [];
      }
      groupedSjedista[rowKey].push(sjediste);
    });

    Object.keys(groupedSjedista).forEach((rowKey) => {
      groupedSjedista[rowKey].sort((a, b) => a.sjedisteId - b.sjedisteId);

      groupedSjedista[rowKey].forEach((sjediste, index) => {
        (sjediste as any).displayNumber = index + 1;
      });
    });
  }

  const sortedRows = Object.keys(groupedSjedista).sort();

  const totalPrice = selectedSjedista.length * seatPrice;

  if (loading) return <div className="text-center p-5">Učitavanje...</div>;
  if (error) return <div className="text-center p-5 text-danger">{error}</div>;

  return (
    <>
      <Header />
      <main className="container py-5">
        {film && (
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
              <h1 className="mb-3">{film.naziv}</h1>
              <p>{film.opis}</p>
              {projekcija && (
                <div className="mt-3">
                  <div className="badge bg-primary p-2 me-2">
                    <i className="bi bi-clock"></i> {film.trajanje} min
                  </div>
                  <div className="badge bg-info p-2 me-2">
                    Sala: {projekcija.salaNaziv}
                  </div>
                  <div className="badge bg-secondary p-2 me-2">
                    Dan: {new Date(projekcija.danNaziv).toLocaleDateString()}
                  </div>
                  <div className="badge bg-dark p-2">
                    Termin: {projekcija.terminNaziv}
                  </div>
                </div>
              )}
            </div>
          </div>
        )}

        <div className="card shadow-sm">
          <div className="card-header bg-light">
            <h3 className="mb-0">Raspored sjedišta</h3>
          </div>
          <div className="card-body">
            <div className="text-center mb-4">
              <div className={styles.screen}>EKRAN</div>
            </div>

            <div className={styles.legendContainer}>
              <div className={styles.legendItem}>
                <div className={`${styles.sjediste} ${styles.slobodno}`}></div>
                <span>Slobodno</span>
              </div>
              <div className={styles.legendItem}>
                <div className={`${styles.sjediste} ${styles.zauzeto}`}></div>
                <span>Zauzeto</span>
              </div>
              <div className={styles.legendItem}>
                <div className={`${styles.sjediste} ${styles.odabrano}`}></div>
                <span>Odabrano</span>
              </div>
              <div className={styles.legendItem}>
                <div
                  className={`${styles.sjediste} ${styles.justReserved}`}
                ></div>
                <span>Upravo rezervisano</span>
              </div>
            </div>

            {justReservedSjedista.length > 0 && (
              <div className="alert alert-success mx-3 mt-3">
                <h5>
                  <i className="bi bi-check-circle-fill me-2"></i>
                  Uspješno ste rezervisali sjedišta!
                </h5>
                <p className="mb-0">
                  Vaša rezervisana sjedišta su označena narandžastom bojom.
                  Hvala na rezervaciji!
                </p>
              </div>
            )}

            <div className={styles.salaContainer}>
              {!sjedista || sjedista.length === 0 ? (
                <div className="alert alert-warning text-center">
                  Nema dostupnih sjedišta za prikaz
                </div>
              ) : (
                sortedRows.map((red) => (
                  <div key={red} className={styles.red}>
                    <div className={styles.redOznaka}>{red}</div>
                    <div className={styles.sjedista}>
                      {groupedSjedista[red].map((sjediste) => {
                        const isSelected = selectedSjedista.some(
                          (s) => s.sjedisteId === sjediste.sjedisteId
                        );
                        const isJustReserved = justReservedSjedista.includes(
                          sjediste.sjedisteId
                        );

                        let statusClass = styles.slobodno;
                        if (isSelected) {
                          statusClass = styles.odabrano;
                        } else if (isJustReserved) {
                          statusClass = styles.justReserved;
                        } else if (sjediste.isReserved) {
                          statusClass = styles.zauzeto;
                        }

                        const displayNumber = (sjediste as any).displayNumber;

                        return (
                          <div
                            key={sjediste.sjedisteId}
                            className={`${styles.sjediste} ${statusClass}`}
                            onClick={() =>
                              isJustReserved || sjediste.isReserved
                                ? null
                                : handleSjedisteClick(sjediste)
                            }
                            title={`Red ${
                              sjediste.red
                            }, Sjedište ${displayNumber} ${
                              isJustReserved ? "- Upravo rezervisano" : ""
                            }`}
                          >
                            {displayNumber}
                          </div>
                        );
                      })}
                    </div>
                  </div>
                ))
              )}
            </div>

            <div className="mt-4">
              {selectedSjedista.length > 0 ? (
                <div className="alert alert-info">
                  <div>Odabrali ste {selectedSjedista.length} sjedište/a:</div>
                  <ul className="mb-0 mt-2">
                    {selectedSjedista.map((sjediste) => (
                      <li key={sjediste.sjedisteId}>
                        Red {sjediste.red}, Sjedište{" "}
                        {(sjediste as any).displayNumber}
                      </li>
                    ))}
                  </ul>
                  <div className="mt-2 fw-bold">
                    Ukupna cijena: {totalPrice} KM
                  </div>
                </div>
              ) : (
                <div className="alert alert-warning">
                  Kliknite na slobodna sjedišta za rezervaciju
                </div>
              )}
            </div>

            <div className="d-flex justify-content-between mt-4 p-3">
              <button
                onClick={() => navigate(`/filmovi/${filmId}/termini`)}
                className="btn btn-outline-secondary"
              >
                &larr; Nazad na termine
              </button>
              {justReservedSjedista.length > 0 ? (
                <button
                  onClick={() => setJustReservedSjedista([])}
                  className="btn btn-outline-success"
                >
                  Rezerviši još karata
                </button>
              ) : (
                <button
                  onClick={handleReservationClick}
                  className="btn btn-success"
                  disabled={selectedSjedista.length === 0}
                >
                  Rezerviši{" "}
                  {selectedSjedista.length > 0
                    ? `${selectedSjedista.length} karte`
                    : "kartu"}
                </button>
              )}
            </div>
          </div>
        </div>
      </main>
      <RezervacijaModal
        show={showModal}
        onHide={handleModalClose}
        film={film}
        projekcija={projekcija}
        selectedSjedista={selectedSjedista}
        seatPrice={seatPrice}
        onConfirm={handleConfirmReservation}
      />
    </>
  );
}
