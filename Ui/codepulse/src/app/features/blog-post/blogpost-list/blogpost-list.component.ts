import { Component, OnInit } from '@angular/core';
import { BlogPost } from '../models/blog-post.model';
import { BlogPostService } from '../blog-post.service';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-blogpost-list',
  templateUrl: './blogpost-list.component.html',
  styleUrls: ['./blogpost-list.component.css'],
})
export class BlogpostListComponent implements OnInit {
  blogposts$?: Observable<BlogPost[]>;
  constructor(private blogPostService: BlogPostService,private router: Router) {}
  ngOnInit(): void {
    this.blogposts$ = this.blogPostService.getAllBlogPosts();
  }
  
}
