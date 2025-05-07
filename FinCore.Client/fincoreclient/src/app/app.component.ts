import { TuiRoot } from "@taiga-ui/core";
import {ChangeDetectionStrategy, Component} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoginFormComponent} from './login-form/login-form.component';
import {RegisterFormComponent} from './register-form/register-form.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, TuiRoot, LoginFormComponent,RegisterFormComponent],
  templateUrl: './app.component.html',
  standalone: true,
  styleUrl: './app.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppComponent {
  title = 'fincoreclient';
}
