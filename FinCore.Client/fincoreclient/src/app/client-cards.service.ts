import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Card } from './card.model'; // Подправьте путь при необходимости
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ClientCardsService {

  constructor(private http: HttpClient) {}
  loadClientCards(clientId: string): Observable<Card[]> {
    if (clientId) {
      const url = "https://localhost:7151/fincore.api/card/client/${clientId}";
      return this.http.get<Card[]>(url);
    } else {
      console.warn('clientId не определен.');
      return new Observable((subscriber) => subscriber.error('clientId не определен'));
    }
  }
}
