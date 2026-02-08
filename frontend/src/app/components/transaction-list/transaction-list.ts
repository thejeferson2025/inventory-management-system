import { CommonModule } from '@angular/common';
import { Product } from '../../models/product.model';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatOptionModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { Transaction } from '../../models/transaction.model';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { ProductService } from '../../services/product.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { TransactionService } from '../../services/transaction.service';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { TransactionFormComponent } from '../transaction-form/transaction-form';
import { TransactionDetailComponent } from '../transaction-detail/transaction-detail';





@Component({
  selector: 'app-transaction-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatInputModule,
    MatFormFieldModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatDialogModule,
    MatSelectModule,
    MatOptionModule
  ],
  templateUrl: './transaction-list.html',
  styleUrls: ['./transaction-list.scss']
})
export class TransactionListComponent implements OnInit {
  private transactionService = inject(TransactionService);
  private productService = inject(ProductService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);


  displayedColumns: string[] = ['date', 'type', 'productName', 'quantity', 'unitPrice', 'totalPrice', 'actions'];
  dataSource!: MatTableDataSource<Transaction>;


  productMap = new Map<string, string>();
  products: Product[] = [];

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {

    this.productService.getAll().subscribe(products => {
      this.products = products;

      products.forEach(p => this.productMap.set(p.id, p.name));

      this.loadTransactions();
    });
  }

  loadTransactions() {
    this.transactionService.getAll().subscribe({
      next: (data) => {
        this.dataSource = new MatTableDataSource(data);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;


        this.dataSource.filterPredicate = (data: Transaction, filter: string) => {
          const productName = this.productMap.get(data.productId)?.toLowerCase() || '';
          const type = data.type.toLowerCase();
          const dataStr = `${productName} ${type}`;
          return dataStr.indexOf(filter) !== -1;
        };
      },
      error: (err) => console.error('Error cargando transacciones', err)
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }


  getProductName(id: string): string {
    return this.productMap.get(id) || 'Producto Eliminado';
  }

  showDetail(transaction: Transaction) {
    const productName = this.getProductName(transaction.productId);

    this.dialog.open(TransactionDetailComponent, {
      width: '500px',
      data: {
        transaction: transaction,
        productName: productName
      }
    });
  }


  editTransaction(transaction: Transaction) {
    const dialogRef = this.dialog.open(TransactionFormComponent, {
      width: '500px',
      disableClose: true,
      data: transaction
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadData();
        this.snackBar.open('Transacción actualizada correctamente', 'Cerrar', {
          duration: 3000,
          verticalPosition: 'top'
        });
      }
    });
  }

  deleteTransaction(id: string) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '350px',
      data: { message: '¿Eliminar este registro de transacción permanentemente?' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.transactionService.delete(id).subscribe({
          next: () => {
            this.loadTransactions();
            this.snackBar.open('Transacción eliminada', 'Cerrar', { duration: 3000, verticalPosition: 'top' });
          },
          error: (err) => alert("Error: " + err.message)
        });
      }
    });
  }

 openForm() {
  const dialogRef = this.dialog.open(TransactionFormComponent, {
    width: '500px',
    disableClose: true
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      this.loadData();
    }
  });
}
}
