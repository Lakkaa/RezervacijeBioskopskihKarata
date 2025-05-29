import React from "react";
import Header from "../components/header";

const ONama: React.FC = () => {
  return (
    <>
      <Header />
      <div className="container py-4">
        <div className="card shadow p-4 mb-5">
          <h1 className="text-center fw-bold mb-3">O nama</h1>

          <hr className="mb-4" />

          <div className="fs-5">
            <p>
              Bioskop je kompanija za filmove koji ima sedište u Austriji.
              Bioskop je osnovan 1993. i posluje kao multipleks.
            </p>

            <p>
              Osnovan je u Austriji, gde ima 27 bioskopa. Kompanija se proširila
              na Jugoistočnu Evropu krajem 2000. godine. Kompanija je
              rasprostranjena na ovim područjima: Maribor, Celje, Murska Sobota,
              Novo Mesto, Kranj i Koper u Sloveniji; Zagreb i Split u Hrvatskoj;
              Beograd, Niš, Kragujevac, Novi Sad, Priština i Prizren u Srbiji;
              Podgorica u Crnoj Gori; Tirana u Albaniji; Skoplje u Severnoj
              Makedoniji; Banja Luka u BiH; Solun u Grčkoj; i Satu Mare i
              Bukurešt u Rumuniji. Na Zapadu ima samo jedan bioskop u Bolcanu,
              Južni Tirol, Italija.
            </p>

            <p>
              Bioskop godišnje poseti preko 12 miliona posetilaca širom Evrope.
            </p>
          </div>
        </div>
      </div>
    </>
  );
};

export default ONama;
