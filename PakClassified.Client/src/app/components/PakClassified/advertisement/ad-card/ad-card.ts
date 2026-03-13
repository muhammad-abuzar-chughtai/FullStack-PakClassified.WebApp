import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Advertisement } from '../../../../core/models/pakClassified/advertisement-model';

@Component({
  selector: 'app-ad-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './ad-card.html',
  styleUrl: './ad-card.css',
})
export class AdCardComponent {

  @Input() ad!: Advertisement;

  @Output() view = new EventEmitter<number>();
  @Output() edit = new EventEmitter<number>();
  @Output() delete = new EventEmitter<number>();

  onView() {
    this.view.emit(this.ad.id);
  }

  onEdit() {
    this.edit.emit(this.ad.id);
  }

  onDelete() {
    this.delete.emit(this.ad.id);
  }

}