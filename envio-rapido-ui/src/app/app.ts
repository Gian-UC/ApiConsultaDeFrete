import { Component } from '@angular/core';
import { CalcFreteComponent } from './pages/calc-frete/calc-frete';
import { RouterOutlet } from "@angular/router";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.html',
})
export class AppComponent {}
