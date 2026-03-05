import { Component, computed, inject } from '@angular/core';
import { AuthService } from '../../../core/services/auth/auth-service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-welcome-admin',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './welcome-admin.html',
  styleUrl: './welcome-admin.css',
})
export class WelcomeAdmin {
  private auth = inject(AuthService);

  isLoggedIn = computed(() => this.auth.isAuthenticated());
  currentUser = computed(() => this.auth.user()?.name ?? '[admin]');

  notes: string = '';

  ngOnInit() {
    const savedNotes = localStorage.getItem('dev_notes');
    if (savedNotes) {
      this.notes = savedNotes;
    }
  }

  autoSave() {
    localStorage.setItem('dev_notes', this.notes);
  }

}
