import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { CategoryService } from '../Services/category.service';
import { Category } from '../models/category.model';
import { UpdateCategory } from '../models/update-category-request';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css'],
})
export class EditCategoryComponent implements OnInit, OnDestroy {
  id: string | null = null;
  category ?: Category;
  paramsSubscription?: Subscription;
  editCategorySubscription?:Subscription;
  constructor(private route: ActivatedRoute, private categoryService: CategoryService, private router: Router) {}

  ngOnInit(): void {
    this.paramsSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');
        if(this.id){
          this.categoryService.getCategoryById(this.id).subscribe(
            {
              next: (response)=>{
                this.category = response;
              }
            }
          )
        }
      },
    });
  }

  onFormSubmit():void{
    const updateCategory : UpdateCategory = {
      name : this.category?.name ?? '',
      urlHandle : this.category?.urlHandle ?? ''
    };
    if(this.id){
      this.editCategorySubscription = this.categoryService.updateCategory(this.id,updateCategory).subscribe({
        next : (response) => {
          this.router.navigateByUrl('/admin/categories');
        }
      });
    }

  }
  onDelete(id:string):void{
    this.categoryService.deleteCategory(id).subscribe({
      next:(response)=>{
        this.router.navigateByUrl('/admin/categories');
      }
    })
  }

  ngOnDestroy(): void {
    this.paramsSubscription?.unsubscribe();
    this.editCategorySubscription?.unsubscribe();
  }
}
