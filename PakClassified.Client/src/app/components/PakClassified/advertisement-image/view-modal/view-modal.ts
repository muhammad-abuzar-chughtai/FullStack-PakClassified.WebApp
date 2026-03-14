import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AdvertisementImageGet, AdvertisementImagePost } from '../../../../core/models/pakClassified/advertisement-image-model';
import { Advertisement } from '../../../../core/models/pakClassified/advertisement-model';

@Component({
  selector: 'app-view-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
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

  form: Partial<AdvertisementImagePost> = {};
  previewUrl: string | null = null;
  selectedFile: File | null = null;

  ngOnChanges(changes: SimpleChanges) {
    if (changes['mode'] && this.mode === 'create') {
      // blank form for create
      this.form = { id: 0, name: '', caption: '', advertisementId: 0, createdBy: 'string' };
      this.previewUrl = null;
      this.selectedFile = null;
      return;
    }
    if ((changes['image'] || changes['mode']) && this.image) {
      this.form = {
        id: this.image.id,
        name: this.image.name,
        caption: this.image.caption ?? '',
        advertisementId: this.image.advertisementId,
        createdBy: this.image.createdBy,
      };
      this.previewUrl = `data:image/jpeg;base64,${this.image.content}`;
      this.selectedFile = null;
    }
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;
    this.selectedFile = input.files[0];
    const reader = new FileReader();
    reader.onload = () => (this.previewUrl = reader.result as string);
    reader.readAsDataURL(this.selectedFile);
  }

  onSave() {
    if (!this.form.name || !this.form.advertisementId) return;
    if (!this.selectedFile && this.mode === 'create') return; // file required for create

    const contentToSend: File = this.selectedFile
      ? this.selectedFile
      : this.base64ToFile(this.image!.content, this.image!.name);

    this.save.emit({
      id: this.form.id ?? 0,
      name: this.form.name!,
      caption: this.form.caption ?? '',
      advertisementId: Number(this.form.advertisementId),
      createdBy: this.form.createdBy ?? '',
      contentFile: contentToSend,
    } as AdvertisementImagePost);
  }

  // Convert existing base64 back to File so API accepts it
  private base64ToFile(base64: string, filename: string): File {
    const byteString = atob(base64);
    const ab = new ArrayBuffer(byteString.length);
    const ia = new Uint8Array(ab);
    for (let i = 0; i < byteString.length; i++) {
      ia[i] = byteString.charCodeAt(i);
    }
    const blob = new Blob([ab], { type: 'image/jpeg' });
    return new File([blob], filename + '.jpg', { type: 'image/jpeg' });
  }

  onClose() {
    this.close.emit();
  }
}