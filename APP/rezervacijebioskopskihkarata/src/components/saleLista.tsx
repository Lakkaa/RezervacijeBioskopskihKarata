import { Sala } from "../types/sala.ts";
import styles from "./saleLista.module.css";

interface SaleListaProps {
  sale: Sala[];
}

export function SaleLista({ sale }: SaleListaProps) {
  const getImagePath = (path: string) => {
    return path?.replace(
      "D:\\fakultet\\Apeiron\\Diplomski\\APP\\rezervacijebioskopskihkarata\\public",
      ""
    );
  };

  return (
    <div className="row justify-content-center">
      {sale.map((sala) => (
        <div key={sala.salaId} className="col-12 col-md-8 text-center mb-5">
          <h1 className="display-4">{sala.naziv}</h1>

          {sala.slika && (
            <img
              src={getImagePath(sala.slika)}
              alt={sala.naziv}
              className="img-fluid w-100"
            />
          )}

          <div className={styles.tableContainer}>
            <table className={styles.salaTable}>
              <tbody>
                <tr>
                  <th>Naziv:</th>
                  <td>{sala.naziv}</td>
                </tr>
                {sala.kapacitet && (
                  <tr>
                    <th>Kapacitet:</th>
                    <td>{sala.kapacitet}</td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      ))}
    </div>
  );
}
