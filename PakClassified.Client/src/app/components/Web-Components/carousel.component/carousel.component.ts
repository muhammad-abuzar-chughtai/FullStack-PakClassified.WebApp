import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterLink, RouterModule } from '@angular/router';

@Component({
  selector: 'app-carousel',
  imports: [CommonModule, RouterModule, RouterLink],
  templateUrl: './carousel.component.html',
  styleUrl: './carousel.component.css',
})
export class CarouselComponent implements OnInit, OnDestroy {
  currentSlide = 0;
  private autoPlayInterval: any;

  slides = [
    {
      icon: 'fa-store',
      heading: 'Buy & Sell Anything, Anywhere in Pakistan',
      subtext: 'From electronics to real estate, cars to services — discover thousands of verified classified ads posted daily across all major cities.',
      bgImage: 'https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?w=1600&q=80',
      btnText: 'Discover More',
      btnLink: 'advertisement-page/'
    },
    {
      icon: 'fa-shield-halved',
      heading: 'Verified Listings. Trusted Transactions.',
      subtext: 'Every ad is reviewed for authenticity. Deal with confidence knowing you\'re interacting with real people and genuine offers.',
      bgImage: 'https://images.unsplash.com/photo-1551836022-d5d88e9218df?w=1600&q=80',
      btnText: 'Learn More',
      btnLink: 'advertisement-page/'
    },
    {
      icon: 'fa-rocket',
      heading: 'Post Your Ad — Reach Millions for Free',
      subtext: 'List your products, property, or services in minutes. No hidden charges. No listing fees. Just pure exposure to buyers nationwide.',
      bgImage: 'https://images.unsplash.com/photo-1517245386807-bb43f82c33c4?w=1600&q=80',
      btnText: 'Explore More',
      btnLink: 'advertisement-page/'
    }
  ];

  ngOnInit(): void {
    this.startAutoPlay();
  }

  ngOnDestroy(): void {
    this.stopAutoPlay();
  }

  startAutoPlay(): void {
    this.autoPlayInterval = setInterval(() => {
      this.currentSlide = (this.currentSlide + 1) % this.slides.length;
    }, 5000);
  }

  stopAutoPlay(): void {
    if (this.autoPlayInterval) {
      clearInterval(this.autoPlayInterval);
    }
  }

  goToSlide(index: number): void {
    this.currentSlide = index;
    this.stopAutoPlay();
    this.startAutoPlay();
  }
}