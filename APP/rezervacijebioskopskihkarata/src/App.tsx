import { Routes, Route } from "react-router-dom";
import Filmovi from "./pages/filmovi.tsx";
import Sale from "./pages/sale.tsx";
import "./styles/global.css";
import Index from "./pages/index.tsx";
import FilmDetaljiStranica from "./pages/filmDetalji.tsx";
import FilmTerminiStranica from "./pages/filmTermini.tsx";
import RezervacijaCreate from "./pages/rezervacijaCreate.tsx";
import ONama from "./pages/oNama.tsx";

function App() {
  return (
    <div className="app-container">
      <Routes>
        <Route path="/" element={<Index />} />
        <Route path="/filmovi" element={<Filmovi />} />
        <Route path="/filmovi/:id" element={<FilmDetaljiStranica />} />
        <Route path="/sale" element={<Sale />} />
        <Route path="/filmovi/:id/termini" element={<FilmTerminiStranica />} />
        <Route
          path="/filmovi/:filmId/termini/:projekcijaId"
          element={<RezervacijaCreate />}
        />
        <Route path="/oNama" element={<ONama />} />
      </Routes>
    </div>
  );
}

export default App;
