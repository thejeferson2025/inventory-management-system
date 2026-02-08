import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { ProductService } from '../../services/product.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CreateUpdateProductDto } from '../../models/product.model';
import { Component, Inject, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';


@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule
  ],
  templateUrl: './product-form.html',
  styleUrls: ['./product-form.scss']
})
export class ProductFormComponent implements OnInit {
  form: FormGroup;
  isEditMode = false;
  imagePreview: string | null = null;

  categories = ['Electrónica', 'Hogar', 'Accesorios', 'Ropa', 'Otros'];

  private fb = inject(FormBuilder);
  private productService = inject(ProductService);
  private dialogRef = inject(MatDialogRef<ProductFormComponent>);
  private cd = inject(ChangeDetectorRef);

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
    this.form = this.fb.group({
      name: ['', Validators.required],
      category: ['', Validators.required],
      description: [''],
      price: [0, [Validators.required, Validators.min(0.01)]],
      stock: [0, [Validators.required, Validators.min(0)]],
      imageUrl: ['']
    });
  }

  ngOnInit(): void {
    if (this.data) {
      this.isEditMode = true;
      this.form.patchValue(this.data);
      if (this.data.imageUrl) {
        this.imagePreview = this.data.imageUrl;
      }
    }
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();

      reader.onload = () => {
        const base64String = reader.result as string;

        this.imagePreview = base64String;
        this.form.patchValue({ imageUrl: base64String });
        this.cd.detectChanges();
      };

      reader.readAsDataURL(file);
    }
  }

  save() {
    if (this.form.invalid) return;

    const productDto: CreateUpdateProductDto = this.form.value;

    if (this.isEditMode) {
      this.productService.update(this.data.id, productDto).subscribe({
        next: () => this.dialogRef.close(true),
        error: (err) => alert('Error: ' + err.message)
      });
    } else {
      this.productService.create(productDto).subscribe({
        next: () => this.dialogRef.close(true),
        error: (err) => alert('Error: ' + err.message)
      });
    }
  }

  close() {
    this.dialogRef.close(false);
  }
}
