import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('client');
  http = inject(HttpClient);
  stock: any;
  
  ngOnInit(): void {
    this.http.get('https://localhost:5001/api/stock').subscribe({
      next: response => this.stock = response,
      error: error => console.log(error),
      complete: ()=> console.info('the request has been completed')
    });
  }

}
