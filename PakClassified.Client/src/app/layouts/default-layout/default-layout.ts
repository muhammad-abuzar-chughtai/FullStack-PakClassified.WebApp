import { Component, computed, inject } from '@angular/core';
import { HeaderComponent } from "../../shared/header.component/header.component";
import { CarouselComponent } from "../../components/Web-Components/carousel.component/carousel.component";
import { AboutSectionComponent } from "../../components/Web-Components/about-section.component/about-section.component";
import { FooterSectionComponent } from "../../components/Web-Components/footer-section.component/footer-section.component";
import { FooterComponent } from "../../shared/footer.component/footer.component";
import { AuthService } from '../../core/services/auth/auth-service';
import { CommonModule } from '@angular/common';
import { AdPage } from "../../components/PakClassified/advertisement/ad-page/ad-page";
import { AdvertisementComponent } from "../../components/PakClassified/advertisement/advertisement";
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-default-layout',
  imports: [CommonModule, RouterLink, HeaderComponent, CarouselComponent, AboutSectionComponent, FooterSectionComponent, FooterComponent, AdvertisementComponent],
  templateUrl: './default-layout.html',
  styleUrl: './default-layout.css',
})
export class DefaultLayout {
// --- Auth Signals ---
  isAuthenticated = computed(() => this.authService.isAuthenticated());
  constructor(private authService: AuthService) { }

  

}
