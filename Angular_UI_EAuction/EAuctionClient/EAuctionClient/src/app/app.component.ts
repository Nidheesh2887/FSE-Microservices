import { Component, OnInit } from '@angular/core';
import { ProductService } from './services/product.service';
import { ProductInfo } from './models/product-info';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'EAuctionClient';
  product: ProductInfo[];
  model: ProductInfo;
  selectedVal!: string;
  constructor(public _productservice: ProductService) {
    this.product = new Array<ProductInfo>();
    this.model = new ProductInfo();
  }
  ngOnInit() {
    this.load();
  }
  load() {
    void this._productservice
      .getProductInfo()
      .then((response) => {
        this.product = response;

        console.log(this.product);
      })
      .catch((error) => {
        console.log(error);
      })
      .finally(() => {});
  }
  // getProduct() {
  //   this.model = this.product.filter((s) => s.Id == this.selectedVal.toString())[0];
  //    void this._productservice
  //    .getProductDetails(this.selectedVal)
  //    .then(response => {
  //       this.model=response;
  //       console.log(this.model)
  //    })
  //    .catch(error => {
  //        console.log(error)
  //    })
  //    .finally(() => {

  //    });
  getProduct() {
    console.log(this.selectedVal.toString());
    this.model = this.product.filter(
      (s) => s.id == this.selectedVal.toString()
    )[0];
    console.log(this.model);
  }
}
