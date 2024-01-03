import { Component, OnDestroy } from '@angular/core';
import { addCategoryRequest } from 'src/app/features/category/models/add-category-request.models';
import { CategoryService } from '../Services/category.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css'],
})
export class AddCategoryComponent implements OnDestroy {
  model: addCategoryRequest;
  addCategorySubscription?: Subscription;
  constructor(private categoryService: CategoryService, private router : Router) {
    this.model = {
      name: '',
      urlHandle: '',
    };
  }

  onFormSubmit() {
    this.addCategorySubscription = this.categoryService.addCategory(this.model).subscribe({
      next: (response) => {
        this.router.navigateByUrl('/admin/categories');
      },
    });
  }

  ngOnDestroy(): void {
    this.addCategorySubscription?.unsubscribe();
  }
}
