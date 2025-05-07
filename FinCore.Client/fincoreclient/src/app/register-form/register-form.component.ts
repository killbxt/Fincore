import {ChangeDetectionStrategy, Component, inject} from '@angular/core';
import {TuiButton, TuiError, TuiIcon, TuiRoot, TuiTextfield,TuiAlertService} from '@taiga-ui/core';
import {FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {Router, RouterLink} from '@angular/router';
import {AsyncPipe, NgIf} from '@angular/common';
import {TuiButtonLoading, TuiChip, TuiFieldErrorPipe, TuiPassword, TuiTooltip} from '@taiga-ui/kit';
import {TuiInputPhoneModule} from '@taiga-ui/legacy';
import {TuiDataListWrapper, TuiEmailsPipe} from '@taiga-ui/kit';
import {TuiInputModule} from '@taiga-ui/legacy';
import {TuiDataList} from '@taiga-ui/core';
import { TuiInputDateModule,  TuiTextfieldControllerModule, TuiUnfinishedValidator,} from '@taiga-ui/legacy';
import {HttpClient, HttpClientModule} from '@angular/common/http';
import {TUI_FALSE_HANDLER, TuiDay} from '@taiga-ui/cdk';
import {catchError, map, startWith, Subject, switchMap, throwError, timer} from 'rxjs';


@Component({
  selector: 'app-register-form',
  imports: [

    TuiInputDateModule,
    TuiInputPhoneModule,
    TuiTextfieldControllerModule,
    TuiUnfinishedValidator,
    TuiInputModule,
    HttpClientModule,
    TuiInputDateModule,
    TuiTextfieldControllerModule,
    TuiUnfinishedValidator,
    TuiDataList,
    TuiIcon,
    TuiTextfield,
    FormsModule,
    RouterLink,
    AsyncPipe,
    TuiButton,
    TuiButtonLoading,
    ReactiveFormsModule,
    TuiPassword,
    TuiInputPhoneModule,
    NgIf,
    TuiEmailsPipe,
    TuiDataListWrapper,
    TuiInputModule,
    AsyncPipe,
    ReactiveFormsModule,
    TuiError,
    TuiFieldErrorPipe,
    TuiInputDateModule,
    TuiTextfieldControllerModule,
    TuiUnfinishedValidator,
    TuiRoot,
    TuiTooltip,
    TuiChip
  ],
  providers:[

  ],
  templateUrl: './register-form.component.html',
  standalone: true,
  styleUrl: './register-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RegisterFormComponent  {
  private readonly alerts = inject(TuiAlertService);
  protected defaultForEmail = '';
  protected minYear = new TuiDay(1960,1,1);


  readonly registerForm = new FormGroup({
    name: new FormControl('',Validators.required),
    phoneNumber: new FormControl('',Validators.required),
    surname: new FormControl('',Validators.required),
    patronymic: new FormControl('',),
    dateOfBirth: new FormControl(),
    email: new FormControl('',Validators.required),
    password: new FormControl('',Validators.required),
  });


  constructor(
    private http: HttpClient,
    private router: Router,
    private fb: FormBuilder
  ) {

  }




  get f() { return this.registerForm.controls; }

  onSubmit() {
    if(this.registerForm.valid) {
      this.http.post('https://localhost:7151/Fincore.API/Client', this.registerForm.getRawValue()).pipe(
        catchError(error => {
          this.showErrorNotification();
          return throwError(() => error);
        })
      ).subscribe({
        next: () => {
          this.showSuccessNotification()
          this.router.navigate(['/login']);
        },
      });
    }
    else{
      this.showValidationErrorNotification();
    }
  }
  protected showValidationErrorNotification(): void {
    this.alerts
      .open('Заполните все данные корректно. После этого нажмите на кнопку регистрации', { label: 'Ошибка' , appearance: 'negative' ,autoClose: 5000})
      .subscribe();
  }

  protected showErrorNotification(): void {
    this.alerts
      .open('Проблема <strong>регистрации</strong>. Вероятнее пользователь с таким <strong>Номером</strong> или <strong>Электронным адресом</strong> уже существует', { label: 'Ошибка' , appearance: 'negative' ,autoClose: 5000})
      .subscribe();
  }
  protected showSuccessNotification(): void {
    this.alerts
      .open('Теперь введите <strong>данные</strong>', { label: 'Успешная регистрация' , appearance: 'positive' , autoClose: 3000})
      .subscribe();
  }
  markAllAsTouched(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);
      if (control instanceof FormGroup) {
        this.markAllAsTouched(control);
      } else {
        control?.markAsTouched({ onlySelf: true });
      }
    });
  }

  protected readonly trigger$ = new Subject<void>();
  protected readonly loading$ = this.trigger$.pipe(
    switchMap(() => timer(2000).pipe(map(TUI_FALSE_HANDLER), startWith('Loading'))),
  );
  protected readonly TuiDay = TuiDay;
}
