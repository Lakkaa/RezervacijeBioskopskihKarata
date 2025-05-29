import { Link } from "react-router-dom";
import styles from "./header.module.css";
import { useEffect } from "react";

export default function Header() {
  useEffect(() => {
    document.body.classList.add(styles.noShift);

    const offcanvasElement = document.getElementById("mainNavbar");
    const handleOffcanvasHidden = () => {
      document.body.classList.add(styles.noShift);
    };

    offcanvasElement?.addEventListener(
      "hidden.bs.offcanvas",
      handleOffcanvasHidden
    );

    return () => {
      offcanvasElement?.removeEventListener(
        "hidden.bs.offcanvas",
        handleOffcanvasHidden
      );
    };
  }, []);

  return (
    <header className={styles.header}>
      <nav className="navbar navbar-dark">
        <div className={`container-fluid ${styles.navContainer}`}>
          <div className="w-100 position-relative">
            <Link className={`navbar-brand ${styles.navbarBrand}`} to="/">
              Bioskop
            </Link>
          </div>
          <button
            className={`navbar-toggler ${styles.hamburgerButton}`}
            type="button"
            data-bs-toggle="offcanvas"
            data-bs-target="#mainNavbar"
            aria-controls="mainNavbar"
            aria-label="Toggle navigation"
          >
            <span className={`navbar-toggler-icon ${styles.Icon}`}></span>
          </button>
          <div
            className={`offcanvas offcanvas-end text-bg-primary ${styles.noShift} ${styles.narrowMenu}`}
            tabIndex={-1}
            id="mainNavbar"
            aria-labelledby="mainNavbarLabel"
          >
            <div className="offcanvas-header">
              <h5 className="offcanvas-title" id="mainNavbarLabel">
                Bioskop
              </h5>
              <button
                type="button"
                className="btn-close btn-close-white"
                data-bs-dismiss="offcanvas"
                aria-label="Close"
              ></button>
            </div>
            <div className="offcanvas-body">
              <ul className="navbar-nav ms-auto">
                <li className="nav-item">
                  <Link className={`nav-link ${styles.navLink}`} to="/filmovi">
                    Filmovi
                  </Link>
                  <Link className={`nav-link ${styles.navLink}`} to="/sale">
                    Sale
                  </Link>

                  <Link className={`nav-link ${styles.navLink}`} to="/oNama">
                    O nama
                  </Link>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </nav>
    </header>
  );
}
