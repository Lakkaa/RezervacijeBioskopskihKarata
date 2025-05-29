export interface Sjediste {
  salaId: number;
  salaNaziv: string;
  brojSjedista: number;
  rezervisanoSjedisteId: number;
  projekcijaId: number;
  rezervacijaId: number;
  sjedisteId: number;
  red: string;
  isReserved: boolean;
}

export interface SelectedSjediste extends Sjediste {
  selected: boolean;
}

export enum SjedisteStatus {
  Slobodno = 0,
  Rezervisano = 1,
  Zauzeto = 2,
}
