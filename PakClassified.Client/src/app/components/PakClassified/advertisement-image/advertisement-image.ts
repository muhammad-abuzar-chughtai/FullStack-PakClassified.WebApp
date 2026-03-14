import { Component, computed, EventEmitter, Input, Output, signal } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ImageCard } from "./image-card/image-card";
import { Advertisement } from '../../../core/models/pakClassified/advertisement-model';
import { AdvertisementImageGet, AdvertisementImagePost } from '../../../core/models/pakClassified/advertisement-image-model';
import { AdvertisementService } from '../../../core/services/pakClassified/advertisement-service';
import { AuthService } from '../../../core/services/auth/auth-service';
import { AdvertisementImageService } from '../../../core/services/pakClassified/advertisement-image-service';
import { forkJoin } from 'rxjs';
import { CommonModule } from '@angular/common';
import { ViewModal } from "./view-modal/view-modal";

@Component({
  selector: 'app-advertisement-image',
  standalone: true,
  imports: [RouterModule, CommonModule, ImageCard, ViewModal],
  templateUrl: './advertisement-image.html',
  styleUrl: './advertisement-image.css',
})
export class AdvertisementImageComponent {
  @Input() ad: any;
  @Output() view = new EventEmitter();
  @Output() edit = new EventEmitter();
  @Output() delete = new EventEmitter();

  // --- Signals ---
  imagesGet = signal<AdvertisementImageGet[]>([]);
  imagesPost = signal<AdvertisementImagePost | null>(null);
  modalOpen = signal(false);
  modalMode = signal<'create' | 'edit' | 'view'>('view');
  selectedImage = signal<AdvertisementImageGet | null>(null);
  // --- Auth Signals ---
  roleName = computed(() => this.auth.roleName());
  isAdmin = computed(() => this.roleName() === 'Admin');
  // --- Parent Data ---
  advertisements = signal<Advertisement[]>([]);

  constructor(
    private adService: AdvertisementService,
    private auth: AuthService,
    private imageService: AdvertisementImageService,

  ) { }

  ngOnInit() {
    this.loadParent();
  }

  // --- Fetching Parent Data ---
  loadParent() {
    forkJoin({
      ads: this.adService.getAll()
    }).subscribe(({ ads, }) => {
      this.advertisements.set(ads);

      this.load();
    });
  }

  load() {
    this.imageService.getAll().subscribe((imageData) => {

      const adList = this.advertisements();

      const enrichedImages = imageData.map(images => ({
        ...images,
        advertisement: adList.find(a => a.id === images.advertisementId)?.name || '',
      }));

      this.imagesGet.set(enrichedImages);
    });
  }

  // --- View Image ---
  viewImage(id: number) {
    const image = this.imagesGet().find(i => i.id === id);
    if (!image) return;
    this.modalMode.set('view');
    this.selectedImage.set(image);
    this.modalOpen.set(true);
  }

  // --- Edit Image ---
  editImage(id: number) {
    const image = this.imagesGet().find(i => i.id === id);
    if (!image) return;
    this.modalMode.set('edit');
    this.selectedImage.set(image);
    this.modalOpen.set(true);

  }

  // --- Add Image ---
  addImage() {
    this.modalMode.set('create');
    this.selectedImage.set(null);  // blank form
    this.modalOpen.set(true);
  }

  // --- Delete Images ---
  deleteImage(id: number) {
    if (!confirm('Delete this Image?')) return;
    this.imageService.delete(id).subscribe(() => this.load());
  }


  // --- Save Image ---
  saveImage(Image: AdvertisementImagePost) {
    if (this.modalMode() === 'create') {
      this.imageService.create(Image).subscribe({
        next: () => { this.load(); this.modalOpen.set(false); },
        error: (err) => console.log('CREATE ERROR:', JSON.stringify(err.error.errors))
      });
    } else {
      this.imageService.update(Image.id, Image).subscribe({
        next: () => { this.load(); this.modalOpen.set(false); },
        error: (err) => console.log('UPDATE ERROR:', err.error)  // ← this will show exact fields
      });
    }
  }

  closeModal() {
    this.modalOpen.set(false);
    this.selectedImage.set(null);
  }
}
