class ProductInfo {
  constructor() {
    this.id = '';
    this.productName = '';
    this.shortDescription = '';
    this.detailedDescription = '';
    this.category = 1;
    this.startingPrice = 0;
    this.bidEndDate = '';
    this.firstName = '';
    this.lastName = '';
    this.address = '';
    this.city = '';
    this.state = '';
    this.pin = '0';
    this.phone = '';
    this.email = '';
    this.SelectedVal = 0;
    this.bidsforProd = [];
  }
  id: string;
  productName: string;
  shortDescription: string;
  detailedDescription: string;
  category: number;
  startingPrice: number;
  bidEndDate: string;
  firstName: string;
  lastName: string;
  address: string;
  city: string;
  state: string;
  pin: string;
  phone: string;
  email: string;
  SelectedVal: number;
  bidsforProd: Bid[];
}
class Bid {
  constructor() {
    this.id = '';
    this.firstName = '';
    this.lastName = '';
    this.address = '';
    this.city = '';
    this.state = '';
    this.pin = '';
    this.phone = '';
    this.email = '';
    this.productId = '';
    this.bidAmount = 0;
  }
  id: string;
  firstName: string;
  lastName: string;
  address: string;
  city: string;
  state: string;
  pin: string;
  phone: string;
  email: string;
  productId: string;
  bidAmount: number;
}
export { ProductInfo };
