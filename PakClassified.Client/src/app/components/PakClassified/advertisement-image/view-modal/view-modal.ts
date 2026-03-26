import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnChanges, Output, signal, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule, AbstractControl } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { AdvertisementImageGet, AdvertisementImagePost } from '../../../../core/models/pakClassified/advertisement-image-model';
import { Advertisement } from '../../../../core/models/pakClassified/advertisement-model';

@Component({
  selector: 'app-view-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './view-modal.html',
  styleUrl: './view-modal.css',
})
export class ViewModal implements OnChanges {
  @Input() open = false;
  @Input() mode: 'view' | 'edit' | 'create' = 'view';
  @Input() image: AdvertisementImageGet | null = null;
  @Input() advertisements: Advertisement[] = [];

  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<AdvertisementImagePost>();

  previewUrl: string | null = null;
  selectedFile: File | null = null;
  formError = signal('');
  fileError = signal(false);

  private fb = inject(FormBuilder);

  form = this.fb.group({
    name: ['', Validators.required],
    caption: [''],
    advertisementId: [0, [Validators.required, Validators.min(1)]],
  });

  // ── same helpers ──
  ctrl(name: string): AbstractControl {
    return (this.form as any).get(name)!;
  }
  showError(name: string): boolean {
    const c = this.ctrl(name);
    return c.invalid && c.touched;
  }
  errorMsg(name: string): string {
    const errors = this.ctrl(name).errors;
    if (!errors) return '';
    if (errors['required']) return 'This field is required.';
    if (errors['min']) return 'Please select an advertisement.';
    return 'Invalid value.';
  }

  ngOnChanges(changes: SimpleChanges) {
    this.formError.set('');
    this.fileError.set(false);

    if (changes['mode'] && this.mode === 'create') {
      this.form.reset({ name: '', caption: '', advertisementId: 0 });
      this.previewUrl = null;
      this.selectedFile = null;
      return;
    }
    if ((changes['image'] || changes['mode']) && this.image) {
      this.form.patchValue({
        name: this.image.name,
        caption: this.image.caption ?? '',
        advertisementId: this.image.advertisementId,
      });
      this.previewUrl = `data:image/jpeg;base64,${this.image.content}`;
      this.selectedFile = null;
    }
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;
    this.selectedFile = input.files[0];
    this.fileError.set(false);
    const reader = new FileReader();
    reader.onload = () => (this.previewUrl = reader.result as string);
    reader.readAsDataURL(this.selectedFile);
  }

  onSave() {
    this.form.markAllAsTouched();
    this.formError.set('');
    this.fileError.set(false);

    if (this.form.invalid) {
      this.formError.set('Please fill in all required fields.');
      return;
    }

    if (!this.selectedFile && this.mode === 'create') {
      this.formError.set('Please select an image file.');
      this.fileError.set(true);
      return;
    }

    const val = this.form.getRawValue();
    const contentToSend: File = this.selectedFile
      ? this.selectedFile
      : this.base64ToFile(this.image!.content, this.image!.name);

    this.save.emit({
      id: this.image?.id ?? 0,
      name: val.name!,
      caption: val.caption ?? '',
      advertisementId: Number(val.advertisementId),
      createdBy: this.image?.createdBy ?? '',
      contentFile: contentToSend,
    } as AdvertisementImagePost);
  }

  private base64ToFile(base64: string, filename: string): File {
    const byteString = atob(base64);
    const ab = new ArrayBuffer(byteString.length);
    const ia = new Uint8Array(ab);
    for (let i = 0; i < byteString.length; i++) ia[i] = byteString.charCodeAt(i);
    const blob = new Blob([ab], { type: 'image/jpeg' });
    return new File([blob], filename + '.jpg', { type: 'image/jpeg' });
  }

  onClose() {
    this.form.reset({ name: '', caption: '', advertisementId: 0 });
    this.previewUrl = null;
    this.selectedFile = null;
    this.formError.set('');
    this.fileError.set(false);
    this.close.emit();
  }
}