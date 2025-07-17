import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TodoItem } from '../interfaces/todo-item';
import { TodoMetrics } from '../interfaces/todo-metrics';
import { environment } from 'src/environments/environment.development';
import { GenericResponse } from '../interfaces/generic-response';
import { TodoItemCreate } from '../interfaces/todo-item-create';
import { TodoItemUpdate } from '../interfaces/todo-item-update';

@Injectable({ providedIn: 'root' })
export class TodoService {
  private apiUrl = `${environment.apiUrl}/todoitems`;

  constructor(private http: HttpClient) { }

  // Métodos sin headers manuales (el interceptor añadirá el token)
  getAllTodos(): Observable<GenericResponse<TodoItem[]>> {
    return this.http.get<GenericResponse<TodoItem[]>>(this.apiUrl);
  }

  getCompletedTodos(): Observable<GenericResponse<TodoItem[]>> {
    return this.http.get<GenericResponse<TodoItem[]>>(`${this.apiUrl}/completed`);
  }

  getPendingTodos(): Observable<GenericResponse<TodoItem[]>> {
    return this.http.get<GenericResponse<TodoItem[]>>(`${this.apiUrl}/pending`);
  }

  getTodoById(id: number): Observable<GenericResponse<TodoItem>> {
    return this.http.get<GenericResponse<TodoItem>>(`${this.apiUrl}/${id}`);
  }

  createTodo(todo: TodoItemCreate): Observable<GenericResponse<TodoItem>> {
    return this.http.post<GenericResponse<TodoItem>>(this.apiUrl, todo);
  }

  updateTodo(id: number, todo: TodoItemUpdate): Observable<GenericResponse<TodoItem>> {
    return this.http.put<GenericResponse<TodoItem>>(`${this.apiUrl}/${id}`, todo);
  }

  deleteTodo(id: number): Observable<GenericResponse<TodoItem>> {
    return this.http.delete<GenericResponse<TodoItem>>(`${this.apiUrl}/${id}`);
  }

  getMetrics(): Observable<GenericResponse<TodoMetrics>> {
    return this.http.get<GenericResponse<TodoMetrics>>(`${this.apiUrl}/metrics`);
  }
}