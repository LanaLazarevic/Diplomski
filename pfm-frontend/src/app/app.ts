import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {TransactionList} from './components/trancastion-list/transaction-list.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('Personal Finance Manager');
}
