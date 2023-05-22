import { Component, Input } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { Product } from 'src/app/shared/models/product';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss']
})
export class ProductItemComponent {           //92
  @Input() product?: Product

  constructor(private basketService: BasketService) {}

  addItemToBasket() {         //csak hozzáadni engedünk eltávolítani csak basketnál lehet vagy product details page
    this.product && this.basketService.addItemToBasket(this.product);   // && kell mert a product lehet undefined szval csak akkor hajtódik végre ha van product
  }
}
