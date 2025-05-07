import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  EventEmitter,
  inject, Input,
  OnInit,
  Output,
  signal
} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
import {DatePipe, NgIf} from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import {
  TuiAlertService,
  TuiButton, TuiDataList, TuiDialog,
  TuiDialogs, TuiDialogService,
  TuiExpand,
  TuiIcon,
  TuiTextfield,
  TuiTextfieldComponent,
  TuiTitle
} from '@taiga-ui/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {
  TuiAvatar,
  TuiButtonClose,
  TuiChevron,
  TuiDataListWrapper,
  TuiInputPin,
  TuiSelect,
  TuiTooltip
} from '@taiga-ui/kit';
import { Router } from '@angular/router';
import {TUI_IS_E2E, TuiAutoFocus} from '@taiga-ui/cdk';
import {TuiSwitch} from '@taiga-ui/kit';
import {TuiInputModule, TuiTagModule} from '@taiga-ui/legacy';
import {ClientFormComponent} from '../client-form/client-form.component';
import {TransactionFormComponent} from '../transaction-form/transaction-form.component';
import {AboutFormComponent} from '../about-form/about-form.component';
import {TuiThumbnailCard} from '@taiga-ui/addon-commerce';
import type {TuiPaymentSystem} from '@taiga-ui/addon-commerce';
import {catchError, throwError} from 'rxjs';




interface Card {
  type: string | null;
  name: string;
  number: string;
  paymentSystem: TuiPaymentSystem;
}

@Component({
  selector: 'app-main-page',
  imports: [
    TuiSwitch,
    NgIf,
    TuiTagModule,
    HttpClientModule,
    TuiTextfieldComponent,
    ReactiveFormsModule,
    TuiAvatar,
    TuiButton,
    TuiTitle,
    TuiExpand,
    TuiIcon,
    TuiButtonClose,
    TuiTextfield,
    FormsModule,
    TuiTooltip,
    DatePipe,
    ClientFormComponent,
    TransactionFormComponent,
    AboutFormComponent,
    TuiDialog,
    TuiInputModule,
    TuiAutoFocus,
    TuiDataListWrapper,
    TuiThumbnailCard,
    TuiChevron,
    TuiSelect,
    TuiDataList,
    TuiInputPin,
  ],
  templateUrl: './main-page.component.html',
  standalone: true,
  styleUrl: './main-page.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MainPageComponent {
  private readonly alerts = inject(TuiAlertService);
  private readonly dialogs = inject(TuiDialogService);

  onCardSelected(card: Card): void {
    this.value = card; // теперь у тебя есть карта в компоненте
    console.log('Получена карта:', this.value);
  }

  phoneNumber: string = '';

  userData: any;

  showPayments = false;
  showHistory = false;
  showHelp = false;
  showAccount = true;

  protected readonly control = new FormControl('', Validators.minLength(4));

  protected cardAddForm = new FormGroup({
    cardType: new FormControl<string | null>('', Validators.required),
  });
  protected openCardForm = false;

  protected showCardDialog(): void {
    this.openCardForm = true;
  }

  protected showSuccessNotification(): void {
    this.alerts
      .open('Карта <strong>оформлена</strong>', { label: 'Успешное добавление' , appearance: 'positive' , autoClose: 3000})
      .subscribe();
  }
  protected showErrorNotification(): void {
    this.alerts
      .open('Выберите <strong>тип</strong> карты или повторите попытку позже', { label: 'Ошибка' , appearance: 'warning' , autoClose: 3000})
      .subscribe();
  }


  protected cards: Card[] = [
    {name: 'Дебетовая', number: '1234',type:"1", paymentSystem: 'mastercard'},
    {name: 'Кредитная', number: '1234',type:"2",  paymentSystem: 'mastercard'},
    {name: 'Накопительная', number: '1234',type:"3",  paymentSystem: 'mir'},
  ];
  protected value: Card | null = null;
  today = new Date();
  issueDate = `${this.today.getFullYear()}-${String(this.today.getMonth() + 1).padStart(2, '0')}-${String(this.today.getDate()).padStart(2, '0')}`;
  expiryDate = this.issueDate; // Для упрощения, пусть expiryDate будет такой же
  protected onValueChange(newValue: Card | null): void {
    this.cardAddForm.get('cardType')?.setValue(newValue ? newValue.type! : '');
  }


  protected onSubmit(observer: any): void {

    console.log("Form valid:", this.cardAddForm.valid);
    if (this.cardAddForm.valid) {
      const cardType = this.cardAddForm.get('cardType')?.value;
      const data={
        type: Number(cardType),
        cardNumber: "cardnum",
        cvc: "cvc",
        clientId: this.userData.id,
        issueDate: this.issueDate,
        expiryDate: this.expiryDate,
      }
      console.log(data)
      this.http.post('https://localhost:7151/Fincore.API/Card', data).pipe(
        catchError(error => {
          this.showErrorNotification();
          return throwError(() => error);
        })
      ).subscribe({
        next: () => {
          this.cdr.detectChanges();
          this.showSuccessNotification()
          this.router.navigate(['/main']);
        },
      });
    }
    else{
      this.showErrorNotification();
    }
  }



  constructor( private http: HttpClient , private router: Router,private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {

    const token = localStorage.getItem('jwtToken');
    if (token) {
      const helper = new JwtHelperService();
      const decodedToken = helper.decodeToken(token);

      if (decodedToken) {
        this.phoneNumber = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone"];
        this.getUserData();
      } else {
        console.warn('Не удалось декодировать JWT токен');
      }
    } else {
      console.warn('JWT токен не найден в localStorage');
    }
  }


  NavigatePayments() {
    this.showPayments = true;
    this.showHistory = false;
    this.showHelp = false;
    this.showAccount = false;
    this.router.navigate(['/payments',this.userData.id]);
  }

  showAccountInfo() {
    this.showPayments = false;
    this.showHistory = false;
    this.showHelp = false;
    this.showAccount = true;
  }

  showHistoryInfo() {
    this.showPayments = false;
    this.showHistory = true;
    this.showHelp = false;
    this.showAccount = false;
  }

  showHelpInfo() {
    this.showPayments = false;
    this.showHistory = false;
    this.showHelp = true;
    this.showAccount = false;
  }

  logout() {
    localStorage.removeItem('jwtToken');
    this.router.navigate(['/login']).then();
  }

  getUserData(): void {

    if (this.phoneNumber) {
      const apiUrl = `https://localhost:7151/Fincore.API/Client/phone/${this.phoneNumber}`;
      this.http.get<any>(apiUrl)
        .subscribe(
          (data) => {
            this.userData = data;
            console.log('Данные пользователя:', this.userData);
          },
          (error) => {
            console.error('Ошибка при получении данных пользователя:', error);
          }
        );
    } else {
      console.warn('Номер телефона отсутствует. Невозможно получить данные пользователя.');
    }
  }
}
