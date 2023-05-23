import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket/basket.service';
import { AccountService } from './account/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'HWCustoms';

  constructor(private basketService: BasketService, private accountService: AccountService) {}

  ngOnInit(): void {
    this.loadBasket();
    this.loadCurrentUser();
  }

  loadBasket() {
    const basketId = localStorage.getItem('basket_id');      // hold basketId  151   //194 changed
    if (basketId) this.basketService.getBasket(basketId);    //if we have basketId then pass through basketId
  }

  loadCurrentUser() {
    const token= localStorage.getItem('token');
    this.accountService.loadCurrentUser(token).subscribe();
  }
}
