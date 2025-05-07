import {Component, inject, Input, OnInit} from '@angular/core';
import {FormBuilder, Validators, FormGroup, ReactiveFormsModule, FormControl, FormsModule} from '@angular/forms';
import {TuiInputModule, TuiTextfieldControllerModule} from '@taiga-ui/legacy';
import {TuiAlertService, TuiButton, TuiIcon, TuiTextfield} from '@taiga-ui/core';
import {TuiChevron, TuiDataListWrapper, TuiInputNumber, TuiSelect} from '@taiga-ui/kit';
import {TuiCurrencyPipe, TuiInputCard, TuiPaymentSystem, TuiThumbnailCard} from '@taiga-ui/addon-commerce';
import {NgIf} from '@angular/common';
import {MainPageComponent} from '../main-page/main-page.component';
import {HttpClient} from '@angular/common/http';
import {Router} from '@angular/router';
import {Card} from '../card.model';
import {routes} from '../app.routes';
import { HttpClientModule} from '@angular/common/http';
import {concatWith} from 'rxjs';

interface CardUI {
  id: string | undefined;
  number: string | undefined;
  name: string | undefined;
  paymentSystem: string | undefined;
  amount: number | undefined;
}

@Component({
  selector: 'app-payment-form',
  imports: [
    TuiInputModule,
    ReactiveFormsModule,
    TuiButton,
    TuiTextfield,
    TuiInputNumber,
    FormsModule,
    TuiCurrencyPipe,
    TuiChevron,
    TuiSelect,
    TuiDataListWrapper,
    TuiThumbnailCard,
    NgIf,
    TuiInputCard,
    MainPageComponent,
    TuiTextfieldControllerModule,
    TuiIcon
  ],
  templateUrl: './payment-form.component.html',
  standalone: true,
  styleUrl: './payment-form.component.scss'
})

export class PaymentFormComponent {
  clientId: string;
  cards: Card[] = [];

  protected cardsUI: CardUI[] = [];




  constructor(private http: HttpClient , private router: Router) {
    this.clientId = this.router.url.slice(10)
    this.loadClientCards()
  }
  NavigateHome(){
    this.router.navigate(['/main']);
  }
  readonly PaymentForm = new FormGroup({
    clienttocardnumber: new FormControl('',[Validators.required,Validators.maxLength(16),
      Validators.minLength(16)]),
    amount: new FormControl('',Validators.required),
    clientfromcardnumber: new FormControl('',Validators.required)
  })




  loadClientCards(): void {

      const clientId = this.clientId
      this.http.get<Card[]>(`https://localhost:7151/Fincore.API/Card/client/${clientId}`)
        .subscribe(
          (cards) => {
            this.cards = cards;
            console.log(this.cards)
          },
          (error) => {
            console.error('Ошибка при загрузке карт:', error);
          }
        );
    }

  private readonly alerts = inject(TuiAlertService);
  protected ShowSuccessTransaction(): void {
    this.alerts
      .open('Транзакция <strong>успешна</strong>!', { label: 'Успех' , appearance: 'positive' })
      .subscribe();
  }

  protected ShowErrorTransaction(): void {
    this.alerts
      .open('Транзакция <strong>провалилась</strong>!', { label: 'Ошибка' , appearance: 'negative' })
      .subscribe();
  }



  onSubmit() {
    if(this.PaymentForm.valid){
      const raw = this.PaymentForm.getRawValue()
      const dataToSend = {
        description: "Покупка",
        merchant: "Банк",
        type: 1,
        cardFromNumber: raw.clientfromcardnumber,
        cardToNumber: raw.clienttocardnumber,
        amount: raw.amount
      };

      this.http.post(
        `https://localhost:7151/Fincore.API/Transaction/MakeTransaction`,
        dataToSend,
      )
        .subscribe(
          (response) => {
            this.ShowSuccessTransaction()
            this.router.navigate(['/main']);
          },
          error => {
            this.ShowErrorTransaction()
          }
        );
    } else{
      this.ShowErrorTransaction()
    }
  }
}

