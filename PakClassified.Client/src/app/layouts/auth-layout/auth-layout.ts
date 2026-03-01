import { Component, signal, computed, inject } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth/auth-service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './auth-layout.html',
  styleUrls: ['./auth-layout.css']
})

export class AuthComponent {

  // ---- Mode ----
  isSignUp = signal(false);

  // ---- UI Computed ----
  title = computed(() => this.isSignUp() ? 'Create Account' : 'Sign In');
  buttonText = computed(() =>
    this.isSignUp() ? 'Register' : 'Login'
  );

  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  constructor() { }


  form = this.fb.group({
    name: [''],
    email: ['', [Validators.required, Validators.email]],
    pass: ['', Validators.required],
    profilePic: [null],
    contactNo: [''],
    dob: [''],
    secQuestion: [''],
    secAnswer: [''],
    createdBy: ['self'],
    lastmodifiedBy: ['self']
  });

  toggleMode() {
    this.isSignUp.update(v => !v);
  }

  // async submit() {

  //   if (this.form.invalid) return;

  //   const value = this.form.getRawValue();

  //   const request$ = this.isSignUp()
  //     ? this.auth.signup(value as any)
  //     : this.auth.signin({
  //       email: value.email!,
  //       password: value.password!
  //     });

  //   try {
  //     await firstValueFrom(request$);
  //   } catch (error) {
  //     console.error('Authentication failed', error);
  //   }
  // }
  async submit() {

    if (this.form.invalid) return;

    const value = this.form.getRawValue();

    const request$ = this.isSignUp()
      ? this.auth.signup(value as any)
      : this.auth.signin({
        email: value.email!,
        pass: value.pass!
      });

    try {
      await firstValueFrom(request$);

      const role = this.auth.roleId();

      if (role === 4) {
        this.router.navigate(['/admin']);
      }
      else if (role === 5) {
        this.router.navigate(['/manager']);
      }
      else if (role === 1) {
        this.router.navigate(['/customer']);
      }
      else {
        this.router.navigate(['/']);
      }

    } catch (err) {
      console.error(err);
    }
  }


  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.form.patchValue({ profilePic: file });
    }
  }
}
