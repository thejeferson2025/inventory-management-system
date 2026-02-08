import { CommonModule } from '@angular/common';
import { Product } from '../../models/product.model';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { ProductService } from '../../services/product.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Component, Inject, OnInit, inject } from '@angular/core';
import { TransactionService } from '../../services/transaction.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-transaction-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatSnackBarModule
  ],
  templateUrl: './transaction-form.html',
  styleUrls: ['./transaction-form.scss']
})
export class TransactionFormComponent implements OnInit {
  form: FormGroup;
  isEditMode = false;
  products: Product[] = [];


  types = [
    { value: 'Purchase', viewValue: 'Compra' },
    { value: 'Sale', viewValue: 'Venta' }
  ];

  private fb = inject(FormBuilder);
  private transactionService = inject(TransactionService);
  private productService = inject(ProductService);
  private dialogRef = inject(MatDialogRef<TransactionFormComponent>);
  private snackBar = inject(MatSnackBar);

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
    this.form = this.fb.group({
      productId: ['', Validators.required],
      type: ['Sale', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      detail: ['']
    });
  }

 ngOnInit(): void {
    this.productService.getAll().subscribe(prods => this.products = prods);
    if (this.data) {
      this.isEditMode = true;
      this.form.patchValue(this.data);
    }
  }

 save() {
    if (this.form.invalid) return;

    const transactionData = this.form.value;
    const selectedProduct = this.products.find(p => p.id === transactionData.productId);

    if (transactionData.type === 'Sale' && selectedProduct) {
      if (transactionData.quantity > selectedProduct.stock) {
        this.snackBar.open('El stock actual no lo permite', 'Entendido', {
          duration: 4000,
          verticalPosition: 'top',
          horizontalPosition: 'center',
          panelClass: ['warning-snackbar']
        });
        return;
      }
    }

    if (this.isEditMode) {
      this.transactionService.update(this.data.id, transactionData).subscribe({
        next: () => this.handleSuccess('Transacción actualizada'),
        error: (err) => this.handleError(err)
      });
    } else {
      this.transactionService.create(transactionData).subscribe({
        next: () => this.handleSuccess('Transacción registrada'),
        error: (err) => this.handleError(err)
      });
    }
  }

  private handleSuccess(msg: string) {
    this.snackBar.open(msg, 'Cerrar', { duration: 3000, verticalPosition: 'top' });
    this.dialogRef.close(true);
  }

  private handleError(err: any) {
    this.snackBar.open(err.message || 'Ocurrió un error inesperado', 'Entendido', {
      duration: 5000,
      verticalPosition: 'top',
      horizontalPosition: 'center',
      panelClass: ['error-snackbar']
    });
  }

  close() {
    this.dialogRef.close(false);
  }
}
