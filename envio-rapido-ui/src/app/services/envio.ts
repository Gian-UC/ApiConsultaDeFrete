import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class EnvioService {
  private apiUrl = 'http://localhost:5145';

  constructor(private http: HttpClient) {}

  calcularFrete(dto: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Envios`, dto);
  }
}
