import { Component, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth/auth-service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {

  private auth = inject(AuthService);
  private router = inject(Router);

  isLoggedIn = computed(() => this.auth.isAuthenticated());
  displayName = computed(() => this.auth.user()?.name ?? '');
  profilePic = computed(() => {
    const photo = this.auth.user()?.profilePic;

    return photo
      ? `data:image/png;base64,${photo}`
      : './user.png';
  });

  login() {
    this.router.navigate(['/login']);
  }

  signup() {
    this.router.navigate(['/signup']);
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }

  navigate(route: string) {
    this.router.navigate([route || '/']);
  }
}
