import { useParams } from "react-router-dom";

export default function RezervacijaCreate() {
  const { projekcijaId } = useParams();

  return (
    <div className="container py-5">
      <h1>Rezervacija karte</h1>
      <p>Projekcija ID: {projekcijaId}</p>
    </div>
  );
}
