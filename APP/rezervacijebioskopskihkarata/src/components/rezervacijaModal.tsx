import { useState } from "react";
import { Sjediste } from "../types/sjediste";
import { FilmProjekcija } from "../types/filmProjekcija";
import { Film } from "../types/Film";
import { Korisnik } from "../types/korisnik";
import styles from "./rezervacijaModal.module.css";

interface RezervacijaModalProps {
  show: boolean;
  onHide: () => void;
  selectedSjedista: Sjediste[];
  projekcija: FilmProjekcija | null;
  film: Film | null;
  onConfirm: (korisnik: Korisnik) => Promise<void>;
  seatPrice: number;
}

export default function RezervacijaModal({
  show,
  onHide,
  selectedSjedista,
  projekcija,
  film,
  onConfirm,
  seatPrice,
}: RezervacijaModalProps) {
  const [korisnik, setKorisnik] = useState<Korisnik>({
    ime: "",
    prezime: "",
    email: "",
  });
  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState<Record<string, string>>({});

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setKorisnik((prev) => ({ ...prev, [name]: value }));

    if (errors[name]) {
      setErrors((prev) => ({ ...prev, [name]: "" }));
    }
  };

  const validateForm = (): boolean => {
    const newErrors: Record<string, string> = {};

    if (!korisnik.ime.trim()) {
      newErrors.ime = "Ime je obavezno";
    }

    if (!korisnik.prezime.trim()) {
      newErrors.prezime = "Prezime je obavezno";
    }

    if (!korisnik.email.trim()) {
      newErrors.email = "Email je obavezan";
    } else if (!/^\S+@\S+\.\S+$/.test(korisnik.email)) {
      newErrors.email = "Email adresa nije validna";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!validateForm()) {
      return;
    }

    setLoading(true);
    try {
      await onConfirm(korisnik);
    } catch (error) {
      console.error("Greska u rezervaciji", error);
      alert("Došlo je do greške prilikom rezervacije.");
    } finally {
      setLoading(false);
    }
  };

  const totalPrice = selectedSjedista.length * seatPrice;

  if (!show) return null;

  return (
    <div className={styles.modalBackdrop}>
      <div className={styles.modalContainer}>
        <div className={`${styles.modalContent} modal-content`}>
          <div className={styles.modalHeader}>
            <h5 className="modal-title" style={{ margin: 0 }}>
              Potvrdi rezervaciju
            </h5>
            <button
              type="button"
              className="btn-close btn-close-white"
              onClick={onHide}
              disabled={loading}
              aria-label="Close"
            ></button>
          </div>

          <div className="modal-body" style={{ padding: 0 }}>
            <div className="row g-0">
              <div
                className="col-md-6 border-end"
                style={{ padding: "18px 22px" }}
              >
                <h5 className="mb-3">Detalji rezervacije</h5>

                {film && (
                  <p>
                    <strong>Film:</strong> {film.naziv}
                  </p>
                )}

                {projekcija && (
                  <>
                    <p>
                      <strong>Sala:</strong> {projekcija.salaNaziv}
                    </p>
                    <p>
                      <strong>Dan:</strong> {projekcija.danNaziv}
                    </p>
                    <p>
                      <strong>Termin:</strong> {projekcija.terminNaziv}
                    </p>
                  </>
                )}

                <p>
                  <strong>Broj sjedišta:</strong> {selectedSjedista.length}
                </p>

                <p>
                  <strong>Sjedišta:</strong>{" "}
                  {selectedSjedista
                    .map(
                      (s) =>
                        `Red ${s.red}, Sjedište ${(s as any).displayNumber}`
                    )
                    .join("; ")}
                </p>

                <div
                  className="alert alert-info mt-3"
                  style={{ fontWeight: 500, fontSize: "1.1em" }}
                >
                  <strong>Ukupna cijena:</strong> {totalPrice} KM
                </div>
              </div>

              <div className="col-md-6" style={{ padding: "18px 22px" }}>
                <h5 className="mb-3">Vaši podaci</h5>

                <form>
                  <div className="mb-3">
                    <label htmlFor="ime" className="form-label">
                      Ime
                    </label>
                    <input
                      type="text"
                      className={`form-control ${
                        errors.ime ? "is-invalid" : ""
                      }`}
                      id="ime"
                      name="ime"
                      value={korisnik.ime}
                      onChange={handleChange}
                      disabled={loading}
                      required
                    />
                    {errors.ime && (
                      <div className="invalid-feedback">{errors.ime}</div>
                    )}
                  </div>

                  <div className="mb-3">
                    <label htmlFor="prezime" className="form-label">
                      Prezime
                    </label>
                    <input
                      type="text"
                      className={`form-control ${
                        errors.prezime ? "is-invalid" : ""
                      }`}
                      id="prezime"
                      name="prezime"
                      value={korisnik.prezime}
                      onChange={handleChange}
                      disabled={loading}
                      required
                    />
                    {errors.prezime && (
                      <div className="invalid-feedback">{errors.prezime}</div>
                    )}
                  </div>

                  <div className="mb-3">
                    <label htmlFor="email" className="form-label">
                      Email adresa
                    </label>
                    <input
                      type="email"
                      className={`form-control ${
                        errors.email ? "is-invalid" : ""
                      }`}
                      id="email"
                      name="email"
                      value={korisnik.email}
                      onChange={handleChange}
                      disabled={loading}
                      required
                    />
                    {errors.email && (
                      <div className="invalid-feedback">{errors.email}</div>
                    )}
                  </div>
                </form>
              </div>
            </div>
          </div>

          <div className={`modal-footer ${styles.modalFooter}`}>
            <button
              type="button"
              className="btn btn-secondary me-2"
              onClick={onHide}
              disabled={loading}
            >
              Odustani
            </button>
            <button
              type="button"
              className="btn btn-primary"
              onClick={handleSubmit}
              disabled={loading}
            >
              {loading ? (
                <>
                  <span
                    className="spinner-border spinner-border-sm me-2"
                    role="status"
                    aria-hidden="true"
                  ></span>
                  Obrađivanje...
                </>
              ) : (
                "Rezerviši"
              )}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}
