import { Component, OnInit, signal } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Advertisement } from '../../../../core/models/pakClassified/advertisement-model';
import { CommonModule } from '@angular/common';
import { AdvertisementService } from '../../../../core/services/pakClassified/advertisement-service';

@Component({
  selector: 'app-ad-page',
  imports: [CommonModule],
  templateUrl: './ad-page.html',
  styleUrl: './ad-page.css',
})
export class AdPage implements OnInit {

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private adService: AdvertisementService
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
    // First, try to get from state (most reliable)
    const adData = history.state?.ad;

    if (adData) {
      this.ad.set(adData);
    } else {
      // Fallback: get ID from route param and fetch from service
      this.route.paramMap.subscribe(params => {
        const id = params.get('id');
        if (id) {
          // TODO: Fetch ad by ID from your service
          this.adService.getById(+id).subscribe(ad => this.ad.set(ad));
        }
      });
    }
  }

  changeImage(event: Event): void {
    const thumbnail = event.target as HTMLImageElement;
    const mainImage = document.getElementById("displayframe") as HTMLImageElement | null;
    if (mainImage) {
      mainImage.src = thumbnail.src;
    }
  }

  back() {
    this.router.navigate(['/admin/advertisements']);
  }
}