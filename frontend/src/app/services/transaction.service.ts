import { catchError } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Transaction, CreateTransactionDto } from '../models/transaction.model';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  private http = inject(HttpClient);

  // Conexion con Backend Microservicio Transaction
  private apiUrl = 'http://localhost:5235/api/transactions';


  getAll(): Observable<Transaction[]> {
    return this.http.get<Transaction[]>(this.apiUrl);
  }

  create(transaction: CreateTransactionDto): Observable<Transaction> {
    return this.http.post<Transaction>(this.apiUrl, transaction).pipe(
      catchError(this.handleError)
    );
  }

  update(id: string, transaction: any): Observable<void> {
  return this.http.put<void>(`${this.apiUrl}/${id}`, transaction);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Ocurrió un error desconocido';

    if (error.error instanceof ErrorEvent) {

      errorMessage = `Error: ${error.error.message}`;
    } else {

      if (error.error && error.error.message) {
        errorMessage = error.error.message;
      } else {
        errorMessage = `Código: ${error.status}\nMensaje: ${error.message}`;
      }
    }
    
    return throwError(() => new Error(errorMessage));
  }
}
