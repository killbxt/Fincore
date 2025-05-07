import {ChangeDetectionStrategy, Component} from '@angular/core';
import {TuiTagModule} from '@taiga-ui/legacy';

@Component({
  selector: 'app-about-form',
  imports: [
    TuiTagModule
  ],
  templateUrl: './about-form.component.html',
  standalone: true,
  styleUrl: './about-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AboutFormComponent {

}
