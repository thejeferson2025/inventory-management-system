import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-transaction-detail',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule],
  templateUrl: './transaction-detail.html',
  styleUrls: ['./transaction-detail.scss']
})
export class TransactionDetailComponent {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {

  }

}
