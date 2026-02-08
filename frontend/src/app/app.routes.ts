import { Routes } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list';
import { TransactionListComponent} from './components/transaction-list/transaction-list';
export const routes: Routes = [
  { path: '', redirectTo: '/products', pathMatch: 'full' },
  { path: 'products', component: ProductListComponent},
  { path: 'transactions', component: TransactionListComponent }
];
