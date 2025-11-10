import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-calc-frete',
  standalone: true,
  templateUrl: './calc-frete.html',
  styleUrls: ['./calc-frete.scss'],
  imports: [CommonModule, FormsModule, MatFormFieldModule, MatInputModule, MatButtonModule],
})
export class CalcFreteComponent {
  modelo = {
    origemCep: '',
    destinoCep: '',
    peso: 0,
  };

  resultado: any = null;
  terminalText: string = '';

  // Validação de CEP (boa prática 🧠)
  validarCep(cep: string): boolean {
    cep = cep.replace(/\D/g, '');
    return /^[0-9]{8}$/.test(cep);
  }

  calcular() {
    // Verifica CEP de origem
    if (!this.validarCep(this.modelo.origemCep)) {
      this.terminalText = 'CEP de origem inválido 😿';
      this.resultado = null;
      return;
    }

    // Verifica CEP de destino
    if (!this.validarCep(this.modelo.destinoCep)) {
      this.terminalText = 'CEP de destino inválido 😿';
      this.resultado = null;
      return;
    }

    // Se chegar aqui: está tudo certo ❤️
    this.terminalText = 'Calculando frete... 🚚✨';

    // Cálculo temporário de exemplo (apenas visual)
    this.resultado = { valorFrete: (this.modelo.peso * 2.5).toFixed(2) };

    this.terminalText = 'Frete calculado com sucesso! 🎉';
  }
}
