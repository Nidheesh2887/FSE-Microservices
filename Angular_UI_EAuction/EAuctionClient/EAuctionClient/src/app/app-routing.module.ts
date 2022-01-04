import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {ProductInfoComponent} from './screens/product-info/product-info.component'

const routes: Routes = [];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
