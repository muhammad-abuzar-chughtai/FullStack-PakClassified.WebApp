import { Component, computed, inject } from '@angular/core';
import { AuthService } from '../../../core/services/auth/auth-service';

@Component({
  selector: 'app-welcome-admin',
  standalone: true,
  imports: [],
  templateUrl: './welcome-admin.html',
  styleUrl: './welcome-admin.css',
})
export class WelcomeAdmin {
  private auth = inject(AuthService);

  isLoggedIn = computed(() => this.auth.isAuthenticated());
  currentUser = computed(() => this.auth.user()?.name ?? '[admin]');
}
    