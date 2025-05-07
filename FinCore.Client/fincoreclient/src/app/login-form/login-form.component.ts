import {ChangeDetectionStrategy, Component, inject} from '@angular/core';
import {TuiInputPhoneModule} from '@taiga-ui/legacy';
import {TuiIcon, TuiTextfield, TuiButton, TuiAlertService} from '@taiga-ui/core';
import {TuiCopy, TuiPassword,TuiButtonLoading} from '@taiga-ui/kit';
import {AsyncPipe} from '@angular/common';
import {catchError, map, startWith, Subject, switchMap, throwError, timer} from 'rxjs';
import {TUI_FALSE_HANDLER} from '@taiga-ui/cdk';
import {FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {Router, RouterLink} from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
@Component({
  selector: 'app-login-form',
  imports: [HttpClientModule,AsyncPipe, TuiInputPhoneModule, TuiIcon, TuiTextfield, TuiCopy, TuiPassword, TuiButtonLoading, TuiButton, ReactiveFormsModule, RouterLink],
  templateUrl: './login-form.component.html',
  standalone: true,
  styleUrl: './login-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginFormComponent {

  constructor(private http: HttpClient,private router: Router) {
  }

  private readonly alerts = inject(TuiAlertService);
  protected loginForm = new FormGroup({
    phoneNumber: new FormControl('',Validators.pattern(/^\+7\d{10}$/)),
    password: new FormControl('', Validators.required)
  })

  protected readonly trigger$ = new Subject<void>();
  protected readonly loading$ = this.trigger$.pipe(
    switchMap(() => timer(2000).pipe(map(TUI_FALSE_HANDLER), startWith('Loading'))),
  );
  protected showLoginErrorNotification(): void {
    this.alerts
      .open('Неверный <strong>Пароль</strong> или <strong>Номер телефона</strong>!', { label: 'Ошибка' , appearance: 'negative' })
      .subscribe();
  }

  protected showValidationErrrorNotification(): void {
    this.alerts
      .open('Введите все данные!', { label: 'Ошибка' , appearance: 'negative' })
      .subscribe();
  }

  onButtonClick(): void {
    this.trigger$.next();
    this.login();
  }

  login(){
    if(this.loginForm.valid){
      const data = this.loginForm.getRawValue();
      this.http.post<any>('https://localhost:7151/api/Auth/login', data)
        .pipe(
          catchError(error => {
            this.showLoginErrorNotification();
            return throwError(() => error);
          })
        )
        .subscribe((response => {
          localStorage.setItem('jwtToken', response.token);
          this.router.navigate(['/main'])
            .then(success => {
              if (success) {
                this.alerts.open('Вход выполнен успешно!', { label: 'Успех' , autoClose: 2000, appearance: 'positive'}).subscribe();
              } else {
                this.alerts.open('Ошибка перехода на главную страницу', { label: 'Ошибка' }).subscribe();
              }
            });
        }))
    }
    else{
      this.showValidationErrrorNotification();
    }
  }
}
