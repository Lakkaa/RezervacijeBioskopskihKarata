export interface Korisnik {
  korisnikId?: number;
  ime: string;
  prezime: string;
  email: string;
}

export interface RezervacijaRequest {
  korisnikId: number;
  projekcijaId: number;
  sjedisteId: number;
}
