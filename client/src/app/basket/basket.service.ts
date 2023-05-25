import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket, BasketItem, BasketTotals } from '../shared/models/basket';
import { HttpClient } from '@angular/common/http';
import { Product } from '../shared/models/product';
import { DeliveryMethod } from '../shared/models/deliveryMethod';

@Injectable({             //148
  providedIn: 'root'
})
export class BasketService {        //singleton
  baseUrl = environment.apiUrl;               //create url to go to our api
  private basketSource = new BehaviorSubject<Basket | null>(null)     //create an observable which is a type of basket or null, we add a value null so when it starts it's null
  basketSource$ = this.basketSource.asObservable();  //so basketSource observable will operate like any other observable we use and components will be able to subscribe and get info from it
  private basketTotalSource = new BehaviorSubject<BasketTotals | null>(null);     //when basket gets updated our basket total gets updated 154
  basketTotalSource$ = this.basketTotalSource.asObservable();
  shipping = 0;

  constructor(private http: HttpClient) { }   //acces to http client to get baskets

  setShippingPrice(deliveryMethod: DeliveryMethod) {
    this.shipping = deliveryMethod.price;
    this.calculateTotals();
  }

  getBasket(id: string){
    return this.http.get<Basket>(this.baseUrl + 'basket?id=' + id).subscribe({
      next: basket => {
        this.basketSource.next(basket);
        this.calculateTotals();
      }
    })      //method updates basketSourse observable and our components will subscribe to basket source, subscribe inside our service when method called by any of our components
  }

  setBasket(basket: Basket) {        //take basket of type Basket
    return this.http.post<Basket>(this.baseUrl + 'basket', basket).subscribe({        //pass through the basket
      next: basket => {
        this.basketSource.next(basket);
        this.calculateTotals();
      }
    }) 
  }

  getCurrentBasketValue(){
    return this.basketSource.value;
  }

  addItemToBasket(item: Product | BasketItem, quantity = 1) {
    if (this.isProduct(item)) item = this.mapProductItemToBasketItem(item);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();       //if left of ?? is null then right will be executed
    basket.items = this.addOrUpdateItem(basket.items, item, quantity)
    this.setBasket(basket);
  }

  removeItemFromBasket(id: number, quantity = 1) {      //156
    const basket = this.getCurrentBasketValue();
    if (!basket) return;
    const item = basket.items.find(x => x.id === id);
    if (item) {
      item.quantity -= quantity
      if (item.quantity === 0) {
        basket.items = basket.items.filter(x => x.id !==id);
      }
      if (basket.items.length > 0) this.setBasket(basket);
      else this.deleteBasket(basket);
    }
  }

  deleteBasket(basket: Basket) {
    return this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe({
      next: () => {
        this.deleteLocalBasket();
      }
    })
  }

  deleteLocalBasket() {
    this.basketSource.next(null);
    this.basketTotalSource.next(null);
    localStorage.removeItem('basket_id');
  }
  
  private addOrUpdateItem(items: BasketItem[], itemToAdd: BasketItem, quantity: number): BasketItem[] {       //149
    const item = items.find(x => x.id === itemToAdd.id)  //if we have an item in our basket that matches id then it will be hold on item
    if (item) item.quantity += quantity;    //if we have item increase its quantity
    else {                                  // if we don't have item we add it to basket
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }
    return items;
  }

  private createBasket(): Basket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);   //sotre basket id in client browser local storage
    return basket;
  }

  private mapProductItemToBasketItem(item: Product): BasketItem {         //return a Basket Item
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      quantity: 0,      //quantity is required item inside our BasketItem
      pictureUrl: item.pictureUrl,
      brand: item.productBrand,
      type: item.productType
    }
  }

  private calculateTotals() {
    const basket = this.getCurrentBasketValue();
    if (!basket) return;   //if basket empty won't do anything
    const subtotal = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0);  //take array of item, price * quantity, and get sum total of all items 154
    const total = subtotal + this.shipping;
    this.basketTotalSource.next({shipping: this.shipping, total, subtotal});
  }

  private isProduct(item: Product | BasketItem): item is Product {    //type guard 156
    return (item as Product).productBrand !== undefined;
  }
}
