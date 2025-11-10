import { Routes } from '@angular/router';
import { CalcFreteComponent } from './pages/calc-frete/calc-frete';


export const routes: Routes = [
  { path: '', redirectTo: 'frete', pathMatch: 'full' },
  { path: 'frete', component: CalcFreteComponent },
];