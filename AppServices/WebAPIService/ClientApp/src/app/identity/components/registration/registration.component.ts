import { Component, Input, OnInit, OnDestroy} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthenticationService } from '../../services/authentication.service';
import { Router } from '@angular/router';
import { takeUntil, catchError, flatMap } from 'rxjs/operators';
import { Subject, Observable, forkJoin, Subscription } from 'rxjs';
import { FormErrorStateMatcher } from '../../../material/form-error-matcher';
import { MatSnackBar } from '@angular/material/snack-bar';
import { environment } from '../../../../environments/environment';
import { UserRegistrationData } from '../../models/userRegistrationData';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit, OnDestroy {

  private unsubscribe$: Subject<any> = new Subject<any>();
  subscriptions:Array<Subscription> = new Array<Subscription>();
  form: FormGroup;
  codeform:FormGroup;
  matcher = new FormErrorStateMatcher();
  mobNumberPattern = "^\\d*\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}$";
  passwordPattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$";
  
  step = 1;
  userRegistrationData:UserRegistrationData;

  constructor(private athenticationService:AuthenticationService,
              private formBuilder: FormBuilder,
              private router: Router,
              private _snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group(
      {
        userPhone: ['', [Validators.required, Validators.pattern(this.mobNumberPattern)]],
        userEmail: ['', [Validators.required, Validators.email]],
        userPassword: ['', [Validators.required, Validators.pattern(this.passwordPattern)]],
        userConfirmPassword: ['', [Validators.required]]
      },
      {
        validator: this.mustMatch('userPassword', 'userConfirmPassword')
      }
    );

    this.codeform = this.formBuilder.group(
      {
        userCode: ['', Validators.required]
      }
    )

  }

  ngOnDestroy() {       
    this.subscriptions.forEach(element => {
      element.unsubscribe();
    });
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  get userCode() { return this.codeform.controls.userCode; }
  get userPhone() { return this.form.controls.userPhone; }
  get userEmail() { return this.form.controls.userEmail; }
  get userPassword() { return this.form.controls.userPassword; }
  get userConfirmPassword() { return this.form.controls.userConfrimPassword; }

  submit() {
    if (!this.form.valid) {
      console.error(this.form.errors);
    }        
    this.athenticationService
        .createIdentity(this.form.value.userPhone,
                        this.form.value.userEmail,
                        this.form.value.userPassword,
                        this.form.value.userConfirmPassword)
        .pipe(
          takeUntil(this.unsubscribe$)          
        )
        .subscribe(
          (data)=> {
            this.step=2;
            this.userRegistrationData = data;
          },
          (error) => {
            this._snackBar.open(error,'',{
              duration: environment.constants.snackBarDuration,
              announcementMessage: error
            });
          }
        );
  }

  submitCode() {
    if (!this.codeform.valid) {
      console.error(this.form.errors);
    }    
    this.subscriptions.forEach(element => {
      element.unsubscribe();
    });
    this.athenticationService
        .confirmIdentity(this.userRegistrationData.Id,
                        this.userRegistrationData.Phone,
                        this.userRegistrationData.Password,
                        this.codeform.value.userCode)                        
        .pipe(
          takeUntil(this.unsubscribe$),
          flatMap(
            data  => {
              this.subscriptions.push(this.athenticationService.sendTokenRequest(this.userRegistrationData.Phone, this.userRegistrationData.Password).subscribe());
              return data;
            }
          )
        )
        .subscribe(
          data => this.router.navigate(['/']),
          error => this._snackBar.open(error,'',{
            duration: environment.constants.snackBarDuration,
            announcementMessage: error
          })
        );
  }

  signin() {
    this.router.navigate(['/identity/']);
  }

  backToStepOne() {
    this.step=1;
  }
  
  @Input() error: string | null;

  mustMatch(controlName: string, matchingControlName: string) {
      return (formGroup: FormGroup) => {
          const control = formGroup.controls[controlName];
          const matchingControl = formGroup.controls[matchingControlName];

          if (matchingControl.errors && !matchingControl.errors.mustMatch) {
              // return if another validator has already found an error on the matchingControl
              return;
          }

          // set error on matchingControl if validation fails
          if (control.value !== matchingControl.value) {
              matchingControl.setErrors({ mustMatch: true });
          } else {
              matchingControl.setErrors(null);
          }
      }
  }

}
