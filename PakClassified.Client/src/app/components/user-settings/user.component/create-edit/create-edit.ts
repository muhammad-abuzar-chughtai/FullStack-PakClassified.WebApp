import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges, inject, signal } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-create-edit-user',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create-edit.html',
  styleUrl: './create-edit.css',
})
export class CreateEditUser implements OnChanges {
  @Input() fields: any[] = [];
  @Input() model: any = {};
  @Input() mode: 'create' | 'update' = 'create';
  @Input() entityName: string = '';

  @Output() save = new EventEmitter<any>();
  @Output() close = new EventEmitter<void>();

  private fb = inject(FormBuilder);

  form = this.fb.group({});
  formError = signal('');
  fileValues: Record<string, File> = {};

  ngOnChanges(changes: SimpleChanges) {
    if (changes['fields'] && this.fields.length) {
      this.buildForm();
    }

    if (changes['model'] && this.model) {
      this.patchForm();
    }

    this.formError.set('');
  }

  // ── Build form controls from fields config ──
  private buildForm() {
    const group: Record<string, any> = {};

    this.fields.forEach(field => {
      if (field.type === 'File') return; // Files handled separately

      const validators: ValidatorFn[] = [];

      // Required unless explicitly false
      if (field.required !== false) {
        validators.push(Validators.required);
      }
      if (field.type === 'email') {
        validators.push(Validators.email);
      }
      if (field.minLength) {
        validators.push(Validators.minLength(field.minLength));
      }
      if (field.type === 'pass' && this.mode === 'update') {
        group[field.key] = [null]; // no validators
        return;
      }
      if (field.maxLength) {
        validators.push(Validators.maxLength(field.maxLength));
      }
      if (field.type === 'select') {
        validators.push(Validators.min(1));
      }

      group[field.key] = [null, validators];
    });

    this.form = this.fb.group(group);
    this.patchForm();
  }

  // ── Patch form with current model values ──
  private patchForm() {
    if (!this.form || !this.model) return;
    const patch: Record<string, any> = {};
    this.fields.forEach(field => {
      if (field.type !== 'File' && this.model[field.key] !== undefined) {
        patch[field.key] = this.model[field.key];
      }
    });
    this.form.patchValue(patch);
  }

  // ── Same helpers as your working example ──
  ctrl(name: string): AbstractControl {
    return this.form.get(name)!;
  }

  showError(name: string): boolean {
    const c = this.form.get(name);
    return !!c && c.invalid && c.touched;
  }

  errorMsg(name: string): string {
    const field = this.fields.find(f => f.key === name);
    const errors = this.form.get(name)?.errors;
    if (!errors) return '';
    if (errors['required']) return `${field?.label ?? name} is required.`;
    if (errors['email']) return 'Please enter a valid email address.';
    if (errors['minlength']) return `Minimum ${errors['minlength'].requiredLength} characters required.`;
    if (errors['maxlength']) return `Maximum ${errors['maxlength'].requiredLength} characters allowed.`;
    if (errors['min']) return `Please select a valid option.`;
    return 'Invalid value.';
  }

  onFileChange(event: Event, key: string) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) this.fileValues[key] = file;
  }

  submit() {
    this.form.markAllAsTouched();
    this.formError.set('');

    if (this.form.invalid) {
      this.formError.set('Please fill in all required fields.');
      return;
    }

    const processedModel: Record<string, any> = { ...this.form.getRawValue() as any };

    // Remove password from update payload entirely
    if (this.mode === 'update') {
      this.fields
        .filter(f => f.type === 'pass')
        .forEach(f => delete processedModel[f.key]);
    }
            
    // Merge file values back in
    this.fields
      .filter(f => f.type === 'File')
      .forEach(f => {
        if (this.fileValues[f.key]) {
          processedModel[f.key] = this.fileValues[f.key];
        } else if (this.model[f.key]) {
          // Keep existing file if not replaced
          const val = this.model[f.key];
          processedModel[f.key] = typeof val === 'string'
            ? this.base64ToFile(val, f.key)
            : val;
        }
      });

    this.save.emit(processedModel);
  }

  private base64ToFile(base64: string, filename: string): File {
    const byteString = atob(base64);
    const ab = new ArrayBuffer(byteString.length);
    const ia = new Uint8Array(ab);
    for (let i = 0; i < byteString.length; i++) ia[i] = byteString.charCodeAt(i);
    const blob = new Blob([ab], { type: 'image/jpeg' });
    return new File([blob], `${filename}.jpg`, { type: 'image/jpeg' });
  }
}