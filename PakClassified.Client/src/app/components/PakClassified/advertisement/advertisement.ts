import { Component, computed, EventEmitter, input, Input, Output, signal } from '@angular/core';
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
import { forkJoin, of } from 'rxjs';
import { Router } from '@angular/router';
import { AdvertisementImageGet } from '../../../core/models/pakClassified/advertisement-image-model';
import { AdvertisementImageService } from '../../../core/services/pakClassified/advertisement-image-service';
import { FormsModule } from '@angular/forms';
import { SearchFilterModal } from '../../../core/models/pakClassified/advertisement-search-filter-model';

@Component({
  selector: 'app-advertisement',
  standalone: true,
  imports: [CommonModule, FormsModule, AdCardComponent],
  templateUrl: './advertisement.html',
  styleUrl: './advertisement.css',
})
export class AdvertisementComponent {
  @Input() ad: any;
  @Output() view = new EventEmitter();
  @Output() edit = new EventEmitter();
  @Output() delete = new EventEmitter();
  @Output() applyFilter = new EventEmitter<SearchFilterModal[]>();

  // --- Signals ---
  advertisements = signal<Advertisement[]>([]);
  selectedAdvertisement = signal<Advertisement | null>(null);
  modalOpen = signal(false);
  showFilter = signal(false);
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
  // --- Images ---
  images = signal<AdvertisementImageGet[]>([]);
  // Add signal
  searchTerm = signal('');
  // Loading
  isLoading = signal(false);
  // Toolbar Hide on default page
  showToolbar = input<boolean>(true);
  // --- Filter Signals ---
  filterCityArea = signal('');
  filterPostedBy = signal('');
  filterStatus = signal('');
  filterType = signal('');
  filterSubCategory = signal('');
  filterTags = signal<string[]>([]);

  // Add filtered computed signal
  filteredAdvertisements = computed(() => {
    if (!this.showToolbar()) return this.advertisements();

    const term = this.searchTerm().toLowerCase();
    const cityArea = this.filterCityArea();
    const postedBy = this.filterPostedBy();
    const status = this.filterStatus();
    const type = this.filterType();
    const subCategory = this.filterSubCategory();
    const tags = this.filterTags();

    return this.advertisements().filter(ad => {
      const matchesSearch = !term || ad.name?.toLowerCase().includes(term) || ad.title?.toLowerCase().includes(term) || ad.description?.toLowerCase().includes(term);
      const matchesCityArea = !cityArea || ad.cityArea === cityArea;
      const matchesPostedBy = !postedBy || ad.postedBy?.id === Number(postedBy);
      const matchesStatus = !status || ad.status === status;
      const matchesType = !type || ad.type === type;
      const matchesSubCat = !subCategory || ad.subCategory === subCategory;
      const matchesTags = tags.length === 0 || tags.every(t => ad.tagNames?.includes(t));

      return matchesSearch && matchesCityArea && matchesPostedBy && matchesStatus && matchesType && matchesSubCat && matchesTags;
    });
  });



  constructor(
    private adService: AdvertisementService,
    private auth: AuthService,
    private router: Router,
    private cityAreaService: CityAreaService,
    private postedByService: UserService,
    private statusService: AdvertisementStatusService,
    private subCategoryService: AdvertisementSubCategoryService,
    private typeService: AdvertisementTypeService,
    private tagService: AdvertisementTagService,
    private imageService: AdvertisementImageService
  ) { }

  ngOnInit() {
    this.loadParent();
  }

  // --- Fetching Parent Data ---
  loadParent() {
    // Build the base observables
    const base$ = forkJoin({
      cityAreas: this.cityAreaService.getAll(),
      statuses: this.statusService.getAll(),
      subCategories: this.subCategoryService.getAll(),
      types: this.typeService.getAll(),
      tags: this.tagService.getAll(),
      images: this.imageService.getAll(),
    });

    // Conditionally include users for admins
    const users$ = this.isAdmin()
      ? this.postedByService.getAll()
      : of<UserGet[]>([]);

    this.isLoading.set(true);
    forkJoin({ base: base$, users: users$ }).subscribe(({ base, users }) => {
      const { cityAreas, statuses, subCategories, types, tags, images } = base;

      this.cityAreas.set(cityAreas);
      this.statuses.set(statuses);
      this.subCategories.set(subCategories);
      this.types.set(types);
      this.tags.set(tags);
      this.images.set(images);
      this.users.set(users);

      this.load();
    });
  }


  load() {
    this.adService.getAll().subscribe((adData) => {

      const cityAreaList = this.cityAreas();
      const postedBy = this.users();
      const statusesList = this.statuses();
      const subCategoryList = this.subCategories();
      const typesList = this.types();
      const tagsList = this.tags();
      const ImageList = this.images();

      const enrichedAdvertisements = adData.map(ad => ({
        ...ad,
        cityArea: cityAreaList.find(ca => ca.id === ad.cityAreaId)?.name || '',
        postedBy: postedBy.find(u => u.id === ad.postedById),
        status: statusesList.find(s => s.id === ad.statusId)?.name || '',
        subCategory: subCategoryList.find(sc => sc.id === ad.subCategoryId)?.name || '',
        type: typesList.find(t => t.id === ad.typeId)?.name || '',
        tagNames: tagsList.filter(tag => ad.tagsId.includes(tag.id)).map(tag => tag.name),
        firstImage: ImageList.find(img => img.advertisementId === ad.id)?.content ?? null,
        adImages: ImageList.filter(img => img.advertisementId === ad.id),
      }));

      this.advertisements.set(enrichedAdvertisements);
    });
    this.isLoading.set(false);
  }





  // --- View Advertisement ---
  viewAdvertisement(id: number) {
    const ad = this.advertisements().find(a => a.id === id);
    if (!ad) return;

    this.selectedAdvertisement.set(ad);
    this.router.navigate(['/advertisement', id], { state: { ad } });
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

  // --- Show Filter Pannel ---
  ShowFilter() {
    this.showFilter.set(true);
  }

  CloseFilter() {
    this.showFilter.set(false);
  }

  onFiltersApplied(filtered: Advertisement[]) {
    this.advertisements.set(filtered);
  }

  toggleTag(tagName: string): void {
    this.filterTags.update(tags =>
      tags.includes(tagName) ? tags.filter(t => t !== tagName) : [...tags, tagName]
    );
  }

  resetFilters(): void {
    this.filterCityArea.set('');
    this.filterPostedBy.set('');
    this.filterStatus.set('');
    this.filterType.set('');
    this.filterSubCategory.set('');
    this.filterTags.set([]);
  }

  get activeFilterCount(): number {
    return [this.filterCityArea(), this.filterPostedBy(), this.filterStatus(),
    this.filterType(), this.filterSubCategory()].filter(Boolean).length
      + this.filterTags().length;
  }

}
