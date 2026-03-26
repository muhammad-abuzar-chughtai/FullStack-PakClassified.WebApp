import { CommonModule } from '@angular/common';
import { Component, computed, EventEmitter, Input, Output } from '@angular/core';
import { Advertisement } from '../../../../core/models/pakClassified/advertisement-model';
import { AuthService } from '../../../../core/services/auth/auth-service';

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

constructor(private auth: AuthService) { }

   // --- Auth Signals ---
  roleName = computed(() => this.auth.roleName());
  isAdmin = computed(() => this.roleName() === 'Admin');


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