import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  constructor(private snackBar: MatSnackBar) {}

  success(message: string) {
    this.snackBar.open(message, '', {
      duration: 2500,
      panelClass: 'snackbar-success',
    });
  }

  error(message: string) {
    this.snackBar.open(message, '', {
      duration: 2500,
      panelClass: 'snackbar-error',
    });
  }
}
