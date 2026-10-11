import { Component } from '@angular/core';
import { CardModule } from 'primeng/card';

@Component({
  selector: 'app-home',
  imports: [CardModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  protected readonly images = ['/017.jpg', '/080.jpg', '/136.jpg'];
}
