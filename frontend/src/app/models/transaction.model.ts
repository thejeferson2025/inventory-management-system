export interface Transaction {
  id: string;
  date: string;
  type: string;
  productId: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  detail?: string;
}

export interface CreateTransactionDto {
  type: string;      
  productId: string;
  quantity: number;
  detail?: string;
}
