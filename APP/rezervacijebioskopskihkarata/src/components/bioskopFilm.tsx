import React from "react";

const BioskopFilm: React.FC = () => {
  return (
    <div
      className="position-relative"
      style={{ height: "100vh", width: "100%", marginTop: "-60px" }}
    >
      <video
        autoPlay
        muted
        loop
        className="w-100 h-100 object-fit-cover"
        style={{ position: "absolute", top: 0, left: 0, zIndex: 0 }}
      >
        <source src="/images/video.mp4" type="video/mp4" />
        Waš web pretraživač ne podržava video tag.
      </video>

      <div className="position-absolute top-50 start-50 translate-middle text-center">
        <a
          href="/filmovi"
          className="btn btn-lg btn-outline-light bg-transparent fs-3 fw-bold text-decoration-none"
        >
          ISTRAŽI REPERTOAR
        </a>
      </div>
    </div>
  );
};

export default BioskopFilm;
