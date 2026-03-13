import { Component, computed, EventEmitter, Input, Output, signal } from '@angular/core';
import { AdCardComponent } from "./ad-card/ad-card";
import { CommonModule } from '@angular/common';
import { Advertisement } from '../../../core/models/pakClassified/advertisement-model';
import { CityArea } from '../../../core/models/location/cityarea-model';
import { UserGet } from '../../../core/models/user/user-model';
import { AdvertisementStatus } from '../../../core/models/pakClassified/advertisement-status-model';
import { AdvertisementType } from '../../../core/models/pakClassified/advertisement-type-model';
import { AdvertisementTag } from '../../../core/models/pakClassified/advertisement-tag-model';
import { AdvertisementService } from '../../../core/services/pakClassified/advertisement-service';
import { AdvertisementSubCategory } from '../../../core/models/pakClassified/advertisement-subcategory-model';
import { CityAreaService } from '../../../core/services/location/cityarea-service';
import { AuthService } from '../../../core/services/auth/auth-service';
import { UserService } from '../../../core/services/user/user-service';
import { AdvertisementSubCategoryService } from '../../../core/services/pakClassified/advertisement-subcategory-service';
import { AdvertisementTypeService } from '../../../core/services/pakClassified/advertisement-type-service';
import { AdvertisementTagService } from '../../../core/services/pakClassified/advertisement-tag-service';
import { AdvertisementStatusService } from '../../../core/services/pakClassified/advertisement-status-service';
import { forkJoin } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-advertisement',
  standalone: true,
  imports: [CommonModule, AdCardComponent],
  templateUrl: './advertisement.html',
  styleUrl: './advertisement.css',
})
export class AdvertisementComponent {
  @Input() ad: any;
  @Output() view = new EventEmitter();
  @Output() edit = new EventEmitter();
  @Output() delete = new EventEmitter();

  // --- Signals ---
  advertisements = signal<Advertisement[]>([]);
  selectedAdvertisement = signal<Advertisement | null>(null);
  modalOpen = signal(false);
  modalMode = signal<'create' | 'update'>('create');
  // --- Auth Signals ---
  roleName = computed(() => this.auth.roleName());
  isAdmin = computed(() => this.roleName() === 'Admin');
  // --- Parent Data ---
  cityAreas = signal<CityArea[]>([]);
  users = signal<UserGet[]>([]);
  statuses = signal<AdvertisementStatus[]>([]);
  subCategories = signal<AdvertisementSubCategory[]>([]);
  types = signal<AdvertisementType[]>([]);
  tags = signal<AdvertisementTag[]>([]);


  constructor(
    private adService: AdvertisementService,
    private auth: AuthService,
    private router: Router,
    private cityAreaService: CityAreaService,
    private postedByService: UserService,
    private statusService: AdvertisementStatusService,
    private subCategoryService: AdvertisementSubCategoryService,
    private typeService: AdvertisementTypeService,
    private tagService: AdvertisementTagService
  ) { }

  ngOnInit() {
    this.loadParent();
  }

  // --- Fetching Parent Data ---
  loadParent() {
    forkJoin({
      cityAreas: this.cityAreaService.getAll(),
      statuses: this.statusService.getAll(),
      subCategories: this.subCategoryService.getAll(),
      types: this.typeService.getAll(),
      tags: this.tagService.getAll(),
    }).subscribe(({ cityAreas, statuses, subCategories, types, tags }) => {
      // Set all signals FIRST
      this.cityAreas.set(cityAreas);
      this.statuses.set(statuses);
      this.subCategories.set(subCategories);
      this.types.set(types);
      this.tags.set(tags);

      // THEN load and enrich advertisements
      this.loadUsers();
      this.load();
    });
  }
  loadUsers() {
    if (this.isAdmin()) {
      this.postedByService.getAll().subscribe({
        next: (users) => this.users.set(users),
        error: () => this.users.set([]),  // graceful fallback
      });
    }
    // } else {
    //   // Non-admins: fetch only their own user profile
    //   this.postedByService.getById(this.auth.userId()).subscribe({
    //     next: (user) => this.users.set([user]),
    //     error: () => this.users.set([]),
    //     complete: () => this.load()
    //   });
    // }
  }


  load() {
    this.adService.getAll().subscribe((adData) => {

      const cityAreaList = this.cityAreas();
      const postedBy = this.users();
      const statusesList = this.statuses();
      const subCategoryList = this.subCategories();
      const typesList = this.types();
      const tagsList = this.tags();

      const enrichedAdvertisements = adData.map(ad => ({
        ...ad,
        cityArea: cityAreaList.find(ca => ca.id === ad.cityAreaId)?.name || '',
        postedBy: postedBy.find(u => u.id === ad.postedById),
        status: statusesList.find(s => s.id === ad.statusId)?.name || '',
        subCategory: subCategoryList.find(sc => sc.id === ad.subCategoryId)?.name || '',
        type: typesList.find(t => t.id === ad.typeId)?.name || '',
        tagNames: tagsList.filter(tag => ad.tagsId.includes(tag.id)).map(tag => tag.name),
      }));

      this.advertisements.set(enrichedAdvertisements);
    });
  }





  // --- View Advertisement ---
  viewAdvertisement(id: number) {
    const ad = this.advertisements().find(a => a.id === id);
    if (!ad) return;

    this.selectedAdvertisement.set(ad);
    this.router.navigate(['/admin/advertisement', id], { state: { ad } });
  }

  // --- Edit Advertisement ---
  editAdvertisement(id: number) {
    this.router.navigate(['/admin/advertisement/edit', id]);
  }

  // --- Add Advertisement ---
  addAdvertisement() {
    this.router.navigate(['/admin/advertisement/create']);
  }


  // --- Delete Advertisement ---
  deleteAdvertisement(id: number) {
    if (!confirm('Delete this Advertisement?')) return;
    this.adService.delete(id).subscribe(() => this.load());
  }

  // --- Save Advertisement ---
  saveAdvertisement(Advertisement: Advertisement) {
    if (this.modalMode() === 'create') {
      this.adService.create(Advertisement).subscribe(() => {
        this.load();
        this.modalOpen.set(false);
      });
    } else {
      this.adService.update(Advertisement.id, Advertisement).subscribe(() => {
        this.load();
        this.modalOpen.set(false);
      });
    }
  }
}
