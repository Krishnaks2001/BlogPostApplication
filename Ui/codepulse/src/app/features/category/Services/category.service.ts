import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { addCategoryRequest } from '../models/add-category-request.models';
import { Observable } from 'rxjs';
import { Category } from '../models/category.model';
import { environment } from 'src/environments/environment.development';
import { UpdateCategory } from '../models/update-category-request';
@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  constructor(private http: HttpClient) {}

  addCategory(model: addCategoryRequest): Observable<void> {
    return this.http.post<void>(
      `${environment.apiBaseUrl}/api/categories?addAuth=true`,
      model
    );
  }

  getAllCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${environment.apiBaseUrl}/api/categories`);
  };

  getCategoryById(id:string) : Observable<Category>{
    return this.http.get<Category>(`${environment.apiBaseUrl}/api/categories/${id}`);
  }

  updateCategory(id:string,category: UpdateCategory) : Observable<Category>{
    return this.http.put<Category>(
      `${environment.apiBaseUrl}/api/categories/${id}?addAuth=true`,
      category
    );
  }

  deleteCategory(id:string):Observable<Category>{
    return this.http.delete<Category>(`${environment.apiBaseUrl}/api/categories/${id}?addAuth=true`);
  }
}
