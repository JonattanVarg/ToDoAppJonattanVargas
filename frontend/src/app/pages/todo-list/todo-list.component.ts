import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatTabsModule } from '@angular/material/tabs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { TodoItem } from 'src/app/interfaces/todo-item';
import { TodoMetrics } from 'src/app/interfaces/todo-metrics';
import { TodoService } from 'src/app/services/todo.service';
import { MatDialog } from '@angular/material/dialog';
import { GenericResponse } from 'src/app/interfaces/generic-response';
import { TodoItemCreate } from 'src/app/interfaces/todo-item-create';
import { TodoItemUpdate } from 'src/app/interfaces/todo-item-update';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.css']
})
export class TodoListComponent implements OnInit {
  todos: TodoItem[] = [];
  metrics = { total: 0, completed: 0, pending: 0 };
  filter: 'all' | 'completed' | 'pending' = 'all';

  form: FormGroup;
  editingId: number | null = null;

  constructor(
    private todoService: TodoService,
    private snackBar: MatSnackBar,
    private fb: FormBuilder
  ) {
    this.form = this.fb.group({
      title: ['', [Validators.required]],
      description: [''],
    });
  }

  ngOnInit(): void {
    this.loadTodos();
    this.loadMetrics();
  }

  loadTodos(): void {
    const fetcher =
      this.filter === 'completed'
        ? this.todoService.getCompletedTodos()
        : this.filter === 'pending'
        ? this.todoService.getPendingTodos()
        : this.todoService.getAllTodos();

    fetcher.subscribe({
      next: (res) => {
        if (res.isSuccess) this.todos = res.data ?? [];
      },
      error: () => this.showNotification('Error al cargar las tareas'),
    });
  }

  loadMetrics(): void {
    this.todoService.getMetrics().subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.metrics.total = res.data.totalTasks;
          this.metrics.completed = res.data.completedTasks;
          this.metrics.pending = res.data.pendingTasks;
        }
      },
      error: () => this.showNotification('Error al obtener métricas'),
    });
  }

  applyFilter(filter: 'all' | 'completed' | 'pending') {
    this.filter = filter;
    this.loadTodos();
  }

  submit(): void {
    if (this.form.invalid) return;

    const dto = this.form.value;

    if (this.editingId !== null) {
      const update: TodoItemUpdate = {
        ...dto,
        isCompleted: this.todos.find(t => t.id === this.editingId)?.isCompleted ?? false
      };
      this.todoService.updateTodo(this.editingId, update).subscribe({
        next: (res) => {
          if (res.isSuccess) {
            this.showNotification('Tarea actualizada con éxito');
            this.resetForm();
            this.loadTodos();
            this.loadMetrics();
          }
        },
        error: () => this.showNotification('Error al actualizar tarea'),
      });
    } else {
      this.todoService.createTodo(dto).subscribe({
        next: (res) => {
          if (res.isSuccess) {
            this.showNotification('Tarea creada con éxito');
            this.resetForm();
            this.loadTodos();
            this.loadMetrics();
          }
        },
        error: () => this.showNotification('Error al crear tarea'),
      });
    }
  }

  edit(todo: TodoItem): void {
    this.form.setValue({ title: todo.title, description: todo.description });
    this.editingId = todo.id;
  }

  delete(id: number): void {
    this.todoService.deleteTodo(id).subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.showNotification('Tarea eliminada');
          this.loadTodos();
          this.loadMetrics();
        }
      },
      error: () => this.showNotification('Error al eliminar tarea'),
    });
  }

  toggleCompleted(todo: TodoItem): void {
    const update: TodoItemUpdate = {
      title: todo.title,
      description: todo.description,
      isCompleted: !todo.isCompleted,
    };

    this.todoService.updateTodo(todo.id, update).subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.showNotification('Tarea actualizada');
          this.loadTodos();
          this.loadMetrics();
        }
      },
      error: () => this.showNotification('Error al cambiar estado'),
    });
  }

  resetForm(): void {
    this.form.reset();
    this.editingId = null;
  }

  showNotification(message: string): void {
    this.snackBar.open(message, 'Cerrar', { duration: 3000 });
  }
}