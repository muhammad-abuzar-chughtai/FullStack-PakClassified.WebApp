import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AdvertisementImageGet } from '../../../../core/models/pakClassified/advertisement-image-model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-image-card',
  imports: [CommonModule],
  templateUrl: './image-card.html',
  styleUrl: './image-card.css',
})
export class ImageCard {


  @Input() image!: AdvertisementImageGet;

  @Output() view = new EventEmitter<number>();
  @Output() edit = new EventEmitter<number>();
  @Output() delete = new EventEmitter<number>();

  onView() {
    this.view.emit(this.image.id);
  }

  onEdit() {
    this.edit.emit(this.image.id);
  }

  onDelete() {
    this.delete.emit(this.image.id);
  }


}
