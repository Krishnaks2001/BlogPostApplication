import { Component, OnDestroy, OnInit } from '@angular/core';
import { AddBlogPost } from '../models/add-blog-post.models';
import { BlogPostService } from '../blog-post.service';
import { Router } from '@angular/router';
import { CategoryService } from '../../category/Services/category.service';
import { Observable, Subscription } from 'rxjs';
import { Category } from '../../category/models/category.model';
import { TmplAstHoverDeferredTrigger } from '@angular/compiler';
import { ImageService } from 'src/app/shared/components/image-selector/image.service';
@Component({
  selector: 'app-add-blogpost',
  templateUrl: './add-blogpost.component.html',
  styleUrls: ['./add-blogpost.component.css'],
})
export class AddBlogpostComponent implements OnInit ,OnDestroy{
  model: AddBlogPost;
  category$?: Observable<Category[]>;
  isImageSelectorVisible: boolean = false;
  imageSelectorSubscription ?: Subscription;
  constructor(
    private blogPostService: BlogPostService,
    private router: Router,
    private categoryService: CategoryService,
    private imageService : ImageService
  ) {
    this.model = {
      title: '',
      shortDescription: '',
      content: '',
      featuredImageUrl: '',
      urlHandle: '',
      author: '',
      publishedDate: new Date(),
      isVisible: true,
      categories: [],
    };
  }
  ngOnInit(): void {
    this.category$ = this.categoryService.getAllCategories();
    this.imageSelectorSubscription = this.imageService.onSelectImage().subscribe({
      next:(selectedImage) => {
        this.model.featuredImageUrl = selectedImage.url;
        this.closeImageSelector();
      }
    });
  }

  onFormSubmit(): void {
    console.log(this.model);

    this.blogPostService.createBlogPost(this.model).subscribe({
      next: (response) => {
        this.router.navigateByUrl('/admin/blogposts');
      },
    });
  }
  openImageSelector(): void {
    this.isImageSelectorVisible = true;
  }

  closeImageSelector(): void {
    this.isImageSelectorVisible = false;
  }
  ngOnDestroy(): void {
    this.imageSelectorSubscription?.unsubscribe();
  }
}
