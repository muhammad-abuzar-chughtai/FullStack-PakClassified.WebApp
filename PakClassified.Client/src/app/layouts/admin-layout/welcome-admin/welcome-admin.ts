import { Component, computed, inject, signal, Signal } from '@angular/core';
import { AuthService } from '../../../core/services/auth/auth-service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AdvertisementService } from '../../../core/services/pakClassified/advertisement-service';
import { Advertisement, AdvertisementGetPost } from '../../../core/models/pakClassified/advertisement-model';
import { UserGet } from '../../../core/models/user/user-model';
import { UserService } from '../../../core/services/user/user-service';

@Component({
  selector: 'app-welcome-admin',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './welcome-admin.html',
  styleUrl: './welcome-admin.css',
})
export class WelcomeAdmin {

  private authService = inject(AuthService);
  private adService = inject(AdvertisementService);
  private userService = inject(UserService);

  isLoggedIn = computed(() => this.authService.isAuthenticated());
  currentUser = computed(() => this.authService.user()?.name ?? '[admin]');

  totalAds = signal<AdvertisementGetPost[]>([]);
  totalUsers = signal<UserGet[]>([]);
  totalAdCount = computed(() => this.totalAds().length);
  totalUserCount = computed(() => this.totalUsers().length);

  ngOnInit() {
    this.adService.getAll().subscribe((data) => {
      this.totalAds.set(data);
    });
    this.userService.getAll().subscribe((data) => {
      this.totalUsers.set(data);
    });
  }
}

