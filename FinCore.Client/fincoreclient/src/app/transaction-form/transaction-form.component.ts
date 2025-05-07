import {ChangeDetectionStrategy, Component, inject, Input } from '@angular/core';
import {TuiAvatar, TuiBadge, TuiComment} from '@taiga-ui/kit';
import {TuiAutoColorPipe, TuiIcon, TuiTitle} from '@taiga-ui/core';
import {TuiAmountPipe} from '@taiga-ui/addon-commerce';
import {AsyncPipe} from '@angular/common';
import {TUI_IS_MOBILE} from '@taiga-ui/cdk';
import {TuiBlockDetails} from '@taiga-ui/layout';

@Component({
  selector: 'app-transaction-form',
  imports: [
    TuiAvatar,
    TuiTitle,
    TuiAmountPipe,
    AsyncPipe,
    TuiComment,
    TuiBadge,
    TuiIcon,
    TuiBlockDetails,
    TuiAutoColorPipe
  ],
  templateUrl: './transaction-form.component.html',
  standalone: true,
  styleUrl: './transaction-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TransactionFormComponent {
  protected readonly isMobile = inject(TUI_IS_MOBILE);
}
