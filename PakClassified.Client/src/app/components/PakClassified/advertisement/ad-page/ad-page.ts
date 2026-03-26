import { Component, computed, OnInit, signal } from '@angular/core';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { Advertisement } from '../../../../core/models/pakClassified/advertisement-model';
import { CommonModule } from '@angular/common';
import { AdvertisementService } from '../../../../core/services/pakClassified/advertisement-service';
import { AdvertisementImageGet } from '../../../../core/models/pakClassified/advertisement-image-model';
import { AdvertisementImageService } from '../../../../core/services/pakClassified/advertisement-image-service';
import { forkJoin } from 'rxjs';
import { AuthService } from '../../../../core/services/auth/auth-service';

@Component({
  selector: 'app-ad-page',
  imports: [CommonModule, RouterLink],
  templateUrl: './ad-page.html',
  styleUrl: './ad-page.css',
})
export class AdPage implements OnInit {

  // --- Images ---
  images = signal<AdvertisementImageGet[]>([]);
  activeImage = signal<string | null>(null);
  // --- Auth Signals ---
  roleName = computed(() => this.auth.roleName());
  isAdmin = computed(() => this.roleName() === 'Admin');

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private adService: AdvertisementService,
    private imageService: AdvertisementImageService,
    private auth: AuthService
  ) { }

  ad = signal<Advertisement>({
    id: 0,
    name: '',
    title: '',
    price: 0,
    likes: 0,
    startsOn: new Date(),
    endsOn: new Date(),
    createdBy: '',
    cityAreaId: 0,
    postedById: 0,
    statusId: 0,
    typeId: 0,
    subCategoryId: 0,
    tagsId: [],
    imagesId: [],
  });



  ngOnInit() {
    const adData = history.state?.ad;
    if (adData) {
      this.ad.set(adData);
      this.images.set(adData.adImages ?? []);
      this.activeImage.set(adData.adImages?.[0]?.content ?? null);
    } else {
      this.route.paramMap.subscribe(params => {
        const id = params.get('id');
        if (id) {
          forkJoin({
            ad: this.adService.getById(+id),
            images: this.imageService.getAll(+id)
          }).subscribe(({ ad, images }) => {
            this.ad.set(ad);
            this.images.set(images);
            this.activeImage.set(images[0]?.content ?? null);
          });
        }
      });
    }
  }

  getMainImage(): string {
    const active = this.activeImage();
    if (active) return `data:image/jpeg;base64,${active}`;
    return './ad-placeholder.jpg';
  }

  changeImage(content: string): void {
    this.activeImage.set(content);
  }


  back() {
    this.router.navigate(['/admin/advertisements']);
  }
}