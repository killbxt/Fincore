import {ChangeDetectionStrategy, ChangeDetectorRef, Component, inject, Input, OnInit} from '@angular/core';
import {TuiButton, TuiDialogService, TuiIcon, TuiSurface, TuiTextfield, TuiTitle} from '@taiga-ui/core';
import {AsyncPipe, DatePipe, NgClass, NgForOf, NgIf, NgStyle} from '@angular/common';
import {TuiAccordion, TuiChip, TuiFade, TuiTiles} from '@taiga-ui/kit';
import {TuiCard, TuiHeader} from '@taiga-ui/layout';
import {TuiAmountPipe, TuiThumbnailCard} from '@taiga-ui/addon-commerce';
import {FormsModule} from '@angular/forms';
import {HttpClient} from '@angular/common/http';
import {HttpClientModule} from '@angular/common/http';

interface Card {
  id: string;
  cardNumber: string;
  cvc: string;
  clientId: string;
  issueDate: string;
  expiryDate: string;
  type: number;
  status: number;
  isDeleted: boolean;
  amount: number;
}


@Component({
  selector: 'app-client-form',
  imports: [
    TuiIcon,
    DatePipe,
    TuiAccordion,
    TuiTextfield,
    TuiCard,
    TuiSurface,
    TuiFade,
    TuiThumbnailCard,
    FormsModule,
    NgIf,
    NgForOf,
    TuiButton,
    TuiChip,
    TuiAmountPipe,
    AsyncPipe,
    TuiTiles,
    TuiHeader,
    TuiTitle,
    NgClass,
    NgStyle
  ],
  templateUrl: './client-form.component.html',
  standalone: true,
  styleUrl: './client-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ClientFormComponent implements OnInit{
  @Input() userData: any;
  cards: Card[] | null = null;

  private readonly dialogs = inject(TuiDialogService);

  protected showDialog(card: Card): void {
    const cardInfo = `
      Номер карты: ${card.cardNumber}<br>
      Тип: ${card.type}<br>
      Срок действия: ${card.expiryDate}
    `;
    this.dialogs
      .open(cardInfo, { label: 'Информация по карте', size: 's' })
      .subscribe();
  }

  constructor(private http: HttpClient,private cdr: ChangeDetectorRef) {}


  ngOnInit(): void {
    this.loadClientCards();
  }

  loadClientCards(): void {

    if (this.userData && this.userData.id) {
      const clientId = this.userData.id;
      console.log(clientId);
      this.http.get<Card[]>(`https://localhost:7151/Fincore.API/Card/client/${clientId}`)
        .subscribe(
          (cards) => {
            this.cards = cards;
            this.cdr.detectChanges();
            console.log("Полученные карты:", this.cards); // Выводим полученные карты здесь!
          },
          (error) => {
            console.error('Ошибка при загрузке карт:', error);
          }
        );
    } else {
      console.warn('userData или userData.id не определены.');
    }
  }


}
