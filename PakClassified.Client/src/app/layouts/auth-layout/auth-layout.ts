import { Component, signal, computed, inject } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule, FormsModule, AbstractControl } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth/auth-service';
import { firstValueFrom } from 'rxjs';
import { ForgetPassDto } from '../../core/models/auth/forgetpass-model';
import { ResetPasswordDto } from '../../core/models/auth/resetpassword-model';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './auth-layout.html',
  styleUrls: ['./auth-layout.css']
})

export class AuthComponent {

  // ---- Mode ----
  isSignUp = signal(false);
  // ---- Loader ----
  isLoading = signal(false);

  // ---- UI Computed ----
  title = computed(() => this.isSignUp() ? 'Create Account' : 'Sign In');
  buttonText = computed(() =>
    this.isSignUp() ? 'Register' : 'Login'
  );
  // ── Forgot Password Signals ──
  isForgotPassword = signal(false);
  forgotStep = signal<1 | 2>(1);
  forgotLoading = signal(false);
  forgotError = signal('');
  // forgotEmailTouched = signal(false);
  // newPasswordTouched = signal(false);
  resetToken = '';
  apiError = signal('');

  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  constructor() { }


  form = this.fb.group({
    name: [''],
    email: ['', [Validators.required, Validators.email]],
    pass: ['', [Validators.required, Validators.minLength(6)]],
    profilePic: [null],
    contactNo: ['', [Validators.pattern(/^\d{9}$/)]],
    dob: [''],
    secQuestion: [''],
    secAnswer: [''],
    createdBy: ['self'],
    lastmodifiedBy: ['self']
  });
  forgotForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    secQues: ['', Validators.required],
    secAns: ['', Validators.required],
  });

  resetForm = this.fb.group({
    newPassword: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', Validators.required],
  });

  // ── Error Message ──
  showError(name: string, formName: 'main' | 'forgot' | 'reset' = 'main'): boolean {
    const c = this.ctrl(name, formName);
    return c.invalid && c.touched;
  }

  errorMsg(name: string, formName: 'main' | 'forgot' | 'reset' = 'main'): string {
    const errors = this.ctrl(name, formName).errors;
    if (!errors) return '';
    if (errors['required']) return 'This field is required.';
    if (errors['email']) return 'Enter a valid email address.';
    if (errors['minlength']) return `Minimum ${errors['minlength'].requiredLength} characters.`;
    if (errors['pattern']) return 'Enter a valid phone number (9 digits).';
    return 'Invalid value.';
  }

  toggleMode() {
    this.isSignUp.update(v => !v);
    this.apiError.set('');
    this.form.markAsUntouched();

    // Adjust validators on signup-only fields when toggling
    const signupRequired = ['name', 'contactNo', 'dob', 'secQuestion', 'secAnswer'];
    signupRequired.forEach(field => {
      const ctrl = this.form.get(field)!;
      if (this.isSignUp()) {
        ctrl.addValidators(Validators.required);
      } else {
        ctrl.clearValidators();
      }
      ctrl.updateValueAndValidity();
    });
  }


  async submit() {

    this.apiError.set('');
    this.isLoading.set(true);          // ← start
    const value = this.form.getRawValue();

    const request$ = this.isSignUp()
      ? this.auth.signup(value as any)
      : this.auth.signin({ email: value.email!, pass: value.pass! });

    try {
      await firstValueFrom(request$);
      this.router.navigate([this.auth.roleId() === 4 ? '/admin' : '/']);
    } catch (err: any) {
      const serverMsg = err?.error?.message || err?.error || null;
      this.apiError.set(
        typeof serverMsg === 'string'
          ? serverMsg
          : this.isSignUp()
            ? 'Registration failed. Please try again.'
            : 'Invalid email or password.'
      );
    } finally {
      this.isLoading.set(false);       // ← always stop
    }
  }

  goToForgotPassword() {
    this.isForgotPassword.set(true);
    this.forgotStep.set(1);
    this.forgotError.set('');
    this.forgotForm.reset();
    this.resetForm.reset();
  }

  backToLogin() {
    this.isForgotPassword.set(false);
    this.forgotError.set('');
  }

  verifyIdentity() {
    this.forgotForm.markAllAsTouched();
    if (this.forgotForm.invalid) return;

    this.forgotError.set('');
    this.forgotLoading.set(true);

    const { email, secQues, secAns } = this.forgotForm.getRawValue();
    this.auth.verifySecurity({ email: email!, secQues: secQues!, secAns: secAns! }).subscribe({
      next: (token) => { this.resetToken = token; this.forgotStep.set(2); this.forgotLoading.set(false); },
      error: () => { this.forgotError.set('Invalid email or security answer.'); this.forgotLoading.set(false); }
    });
  }

  resetPassword() {
    this.resetForm.markAllAsTouched();
    if (this.resetForm.invalid) return;

    this.forgotError.set('');
    const { newPassword, confirmPassword } = this.resetForm.getRawValue();

    if (newPassword !== confirmPassword) {
      this.forgotError.set('Passwords do not match.');
      return;
    }

    this.forgotLoading.set(true);
    this.auth.resetPassword({ resetToken: this.resetToken, newPassword: newPassword! }).subscribe({
      next: () => { this.forgotLoading.set(false); this.backToLogin(); },
      error: () => {
        this.forgotError.set('Token expired. Please start over.');
        this.forgotStep.set(1);
        this.resetToken = '';
        this.forgotLoading.set(false);
      }
    });
  }


  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.form.patchValue({ profilePic: file });
    }
  }

  // ── Helpers to read controls cleanly ──
  ctrl(name: string, formName: 'main' | 'forgot' | 'reset' = 'main'): AbstractControl {
  const f = formName === 'forgot' ? this.forgotForm : formName === 'reset' ? this.resetForm : this.form;
  return (f as any).get(name)!;
}
}
