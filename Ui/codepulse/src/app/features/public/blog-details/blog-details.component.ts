import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BlogPostService } from '../../blog-post/blog-post.service';
import { BlogPost } from '../../blog-post/models/blog-post.model';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-blog-details',
  templateUrl: './blog-details.component.html',
  styleUrls: ['./blog-details.component.css']
})
export class BlogDetailsComponent implements OnInit{
  url:string | null  = null;
  blogposts$ ?: Observable<BlogPost>;
  constructor(private route : ActivatedRoute,
    private blogpostService : BlogPostService){

  }
  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next : (params) => {
        this.url = params.get("url");
      }
    });
    if(this.url){
      this.blogposts$ = this.blogpostService.getBlogPostByUrl(this.url);
    }
  }
}
