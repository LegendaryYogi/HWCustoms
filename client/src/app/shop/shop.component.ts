import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Product } from '../shared/models/product';
import { ShopService } from './shop.service';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import { ShopParams } from '../shared/models/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild('search') searchTerm?: ElementRef;       //103
  products: Product[] = [];
  brands: Brand[] = [];         //93
  types: Type[] = [];
  shopParams: ShopParams;                   
  sortOptions = [               //96 adding sort func
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low to high', value: 'priceAsc'},
    {name: 'Price: High to low', value: 'priceDesc'},
  ];
  totalCount = 0;

  constructor(private shopService: ShopService) {
    this.shopParams = shopService.getShopParams();
  }

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts() {
    this.shopService.getProducts().subscribe({      //if you not subscribe to an observable nothing happens   this.idselected gives the function
      next: response => {
        this.products = response.data;                      //98 5p
        this.totalCount = response.count;
      },
      error: error => console.log(error)
    })   
  }

  getBrands() {
    this.shopService.getBrands().subscribe({              //if you not subscribe to an observable nothing happens
      next: response => this.brands = [{id: 0, name: 'All'}, ...response],  //... spread operator  94 takes the array of brands that are inside the response and spreads them
      error: error => console.log(error)
    })   
  }

  getTypes() {
    this.shopService.getTypes().subscribe({              //if you not subscribe to an observable nothing happens
      next: response => this.types = [{id: 0, name: 'All'}, ...response],
      error: error => console.log(error)
    })   
  }

  onBrandSelected(brandId: number) {      //94
    const params = this.shopService.getShopParams();
    params.brandId = brandId;
    params.pageNumber = 1;         // fixing bug 104      update 281
    this.shopService.setShopParams(params);
    this.shopParams = params;
    this.getProducts();
  }

  onTypeSelected(typeId: number) {
    const params = this.shopService.getShopParams();
    params.typeId = typeId;
    params.pageNumber = 1;         // fixing bug 104
    this.shopService.setShopParams(params);
    this.shopParams = params;
    this.getProducts();
  }

  onSortSelected(event: any) {    //96 2p
    const params = this.shopService.getShopParams();
    params.sort = event.target.value;
    this.shopService.setShopParams(params);
    this.shopParams = params;
    this. getProducts();
  }

  onPageChanged(event: any) {
    const params = this.shopService.getShopParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.shopService.setShopParams(params);
      this.shopParams = params;
      this.getProducts();
    }
  }

  onSearch() {
    const params = this.shopService.getShopParams();
    params.search = this.searchTerm?.nativeElement.value;
    params.pageNumber = 1;                   // fixing bug 104
    this.shopService.setShopParams(params);
    this.shopParams = params;
    this.getProducts();
  }

  onReset() {
    if (this.searchTerm) this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.shopService.setShopParams(this.shopParams);
    this.getProducts();
  }

}
