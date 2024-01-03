import { Component, OnInit } from '@angular/core';
import { BlogPostService } from '../../blog-post/blog-post.service';
import { BlogPost } from '../../blog-post/models/blog-post.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{
  blogs$ ?: Observable<BlogPost[]>;
  constructor(private blogpostService : BlogPostService){

  }
  ngOnInit(): void {
    this.blogs$ = this.blogpostService.getAllBlogPosts();
  }
}
