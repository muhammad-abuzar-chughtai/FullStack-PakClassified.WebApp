import { Component, Input, Output, EventEmitter, signal, OnChanges, SimpleChanges, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css']
})
export class ModalComponent implements OnChanges {
  @Input() fields: any[] = [];
  @Input() model: any = {};
  @Input() mode: 'create' | 'update' = 'create';
  @Input() entityName: string = '';

  @Output() save = new EventEmitter<any>();
  @Output() close = new EventEmitter<void>();

  private fb = inject(FormBuilder);
  form!: FormGroup;

  ngOnChanges(changes: SimpleChanges) {
  if ((changes['fields'] || changes['model'] || changes['mode']) && this.fields.length && this.model) {
    this.buildForm();
  }
}

 buildForm() {
  const controls: any = {};
  this.fields.forEach(f => {
    const val = this.model?.[f.key] ?? '';
    if (f.required === false) {
      controls[f.key] = [val];
    } else if (f.type === 'select') {
      controls[f.key] = [val, [Validators.required, Validators.min(1)]];
    } else {
      controls[f.key] = [val, Validators.required];
    }
  });
  this.form = this.fb.group(controls);
}

  ctrl(key: string): AbstractControl {
    return (this.form as any).get(key)!;
  }

  showError(key: string): boolean {
    const c = this.ctrl(key);
    return c.invalid && c.touched;
  }

  errorMsg(key: string): string {
  const errors = this.ctrl(key).errors;
  if (!errors) return '';
  if (errors['required']) return 'This field is required.';
  if (errors['min'])      return 'Please select an option.';
  return 'Invalid value.';
}

  submit() {
    debugger;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;

    // merge form values back into original model to preserve id, createdBy etc.
    const result = { ...this.model, ...this.form.getRawValue() };
    // console.table(this.model);
    this.save.emit(result);
  }

  onClose() {
    this.form.reset();
    this.close.emit();
  }

  getKeys() {
    return Object.keys(this.model || {}).filter(k => k !== 'Id' && k !== 'CreatedBy' && k !== 'LastModifiedBy');
  }
}