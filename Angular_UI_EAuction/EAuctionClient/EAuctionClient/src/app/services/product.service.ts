import { Injectable } from '@angular/core';
import { ProductInfo } from '../models/product-info';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  http: HttpClient;

  baseApiUrl = environment.baseApiUrl;
  constructor(httpClient: HttpClient) {
    this.http = httpClient;
  }

  public async getProductInfo(): Promise<ProductInfo[]> {
    return this.http
      .get<ProductInfo[]>(
        `${this.baseApiUrl}/${'Seller'}/${'GetProductsnBids'}`
      )
      .toPromise();
  }
  // public async getProductDetails(ProductId: Number): Promise<ProductInfo> {
  //   return this.http
  //     .get<ProductInfo>(`${this.baseApiUrl}/${'Seller'}/${ProductId}`)
  //     .toPromise();
  // }
}
