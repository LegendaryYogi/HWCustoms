import { Injectable } from '@angular/core';
import { Pagination } from '../shared/models/pagination';
import { Product } from '../shared/models/product';
import { HttpClient, HttpClientModule, HttpParams } from '@angular/common/http';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import { ShopParams } from '../shared/models/shopParams';
import { Observable, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShopService {                      //88
  baseUrl = 'https://localhost:5001/api/';
  products: Product[] = [];             //280
  brands: Brand[] = [];
  types: Type[] = [];
  pagination?: Pagination<Product[]>;
  shopParams = new ShopParams;
  productCache = new Map<string, Pagination<Product[]>>();       //282

  constructor(private http: HttpClient) { }

  getProducts(useCache = true):Observable<Pagination<Product[]>> {                 //returning an Observable of Paginationof Product array

    if (!useCache)  this.productCache = new Map();    //282

    if (this.productCache.size > 0 && useCache) {           //Map function to get it's size (it's not an array)
      if (this.productCache.has(Object.values(this.shopParams).join('-'))) {    //method allow to check if we have the info
        this.pagination = this.productCache.get(Object.values(this.shopParams).join('-')) //and pagination object store the response
        if (this.pagination) return of(this.pagination);
      }
    }     

    let params = new HttpParams();

    if (this.shopParams.brandId > 0) params = params.append('brandId', this.shopParams.brandId);      //94 part of adding function to filter
    if (this.shopParams.typeId > 0) params = params.append('typeId', this.shopParams.typeId);
    params = params.append('sort', this.shopParams.sort);           //if we don't have the cached (usecache pagination then this goes and get the info from API)
    params = params.append('pageIndex', this.shopParams.pageNumber);
    params = params.append('pageSize', this.shopParams.pageSize);
    if (this.shopParams.search) params = params.append('search', this.shopParams.search);
    
    return this.http.get<Pagination<Product[]>>(this.baseUrl + 'products', {params}).pipe(      //{params} = {params: params} in an object + 280
      map(response => {
        this.productCache.set(Object.values(this.shopParams).join('-'), response)
        this.pagination = response;
        return response;
      })
    )   
  }

  setShopParams(params: ShopParams) {
    this.shopParams = params;
  }

  getShopParams() {
    return this.shopParams;
  }

  getProduct(id: number) {
    const product = [...this.productCache.values()]   //280         now product loading is instant
      .reduce((acc, paginatedResult) => {       // acc = accumulator
        return {...acc, ...paginatedResult.data.find(x => x.id === id)}                     //282
      }, {} as Product)
    

    if (Object.keys(product).length !== 0) return of (product);       //we use of to return back an Observable

    return this.http.get<Product>(this.baseUrl + 'products/' + id);     //111
  }

  getBrands() {
    if (this.brands.length > 0) return of (this.brands);

    return this.http.get<Brand[]>(this.baseUrl + 'products/brands').pipe(   //93 adding product filters
      map(brands => this.brands = brands)
    );      
  }

  getTypes() {
    if (this.types.length > 0) return of (this.types);      //281

    return this.http.get<Type[]>(this.baseUrl + 'products/types').pipe(
      map(types => this.types = types)
    );
  }
}
