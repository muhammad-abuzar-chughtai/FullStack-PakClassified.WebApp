import { CommonModule } from '@angular/common';
import { Component, computed } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HeaderComponent } from '../../shared/header.component/header.component';
import { FooterSectionComponent } from '../../components/Web-Components/footer-section.component/footer-section.component';
import { AdvertisementComponent } from '../../components/PakClassified/advertisement/advertisement';
import { FooterComponent } from '../../shared/footer.component/footer.component';
import { AuthService } from '../../core/services/auth/auth-service';

@Component({
  selector: 'app-advertisement-page-layout',
  imports: [CommonModule, RouterLink, HeaderComponent, FooterSectionComponent, FooterComponent, AdvertisementComponent],
  templateUrl: './advertisement-page-layout.html',
  styleUrl: './advertisement-page-layout.css',
})
export class AdvertisementPageLayout {
// --- Auth Signals ---
  isAuthenticated = computed(() => this.authService.isAuthenticated());
  constructor(private authService: AuthService) { }
}
