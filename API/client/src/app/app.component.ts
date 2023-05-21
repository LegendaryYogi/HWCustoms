import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Product } from './models/product';
import { Pagination } from './models/pagination';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'HWCustoms';
  products: Product[] = [];     //it needs to be able to undefiend or give it initial value

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<Pagination<Product[]>>('https://localhost:5001/api/products?pageSize=50').subscribe({
      next: response => this.products = response.data, //what to do next        80     response type of any
      error: error => console.log(error),  //what to do if there is an error    specify error pass it as an argument
      complete: () => {
        console.log('request completed');
        console.log('extra statment');
      }
    })
  }
}
