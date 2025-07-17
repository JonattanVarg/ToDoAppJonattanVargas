import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { todoListGuard } from './todo-list.guard';

describe('todoListGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => todoListGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
