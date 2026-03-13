import { Component, computed, NgModule, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AdvertisementService } from '../../../../core/services/pakClassified/advertisement-service';
import { CityAreaService } from '../../../../core/services/location/cityarea-service';
import { AdvertisementStatusService } from '../../../../core/services/pakClassified/advertisement-status-service';
import { AdvertisementSubCategoryService } from '../../../../core/services/pakClassified/advertisement-subcategory-service';
import { AdvertisementTypeService } from '../../../../core/services/pakClassified/advertisement-type-service';
import { AdvertisementTagService } from '../../../../core/services/pakClassified/advertisement-tag-service';
import { forkJoin } from 'rxjs';
import { CityArea } from '../../../../core/models/location/cityarea-model';
import { AdvertisementStatus } from '../../../../core/models/pakClassified/advertisement-status-model';
import { AdvertisementSubCategory } from '../../../../core/models/pakClassified/advertisement-subcategory-model';
import { AdvertisementTag } from '../../../../core/models/pakClassified/advertisement-tag-model';
import { AdvertisementType } from '../../../../core/models/pakClassified/advertisement-type-model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdvertisementGetPost } from '../../../../core/models/pakClassified/advertisement-model';
import { AuthService } from '../../../../core/services/auth/auth-service';

@Component({
  selector: 'app-create-edit',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './create-edit.html',
  styleUrl: './create-edit.css',
})
export class CreateEdit {

  mode = signal<'create' | 'update'>('create');
  advertisement = signal<AdvertisementGetPost>({
    id: 0,
    name: '',
    description: '',
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
    imagesId: []
  });

  cityAreas = signal<CityArea[]>([]);
  statuses = signal<AdvertisementStatus[]>([]);
  subCategories = signal<AdvertisementSubCategory[]>([]);
  types = signal<AdvertisementType[]>([]);
  tags = signal<AdvertisementTag[]>([]);
// --- Auth Signals ---
  roleName = computed(() => this.auth.roleName());
  isAdmin = computed(() => this.roleName() === 'Admin');


  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private auth: AuthService,
    private adService: AdvertisementService,
    private cityAreaService: CityAreaService,
    private statusService: AdvertisementStatusService,
    private subCategoryService: AdvertisementSubCategoryService,
    private typeService: AdvertisementTypeService,
    private tagService: AdvertisementTagService
  ) { }

  ngOnInit() {

    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.mode.set('update');
      this.loadAdvertisement(+id);
    }
    else {
    // Set postedById for create mode
    this.advertisement.update(ad => ({
      ...ad,
      postedById: this.auth.user()?.id ?? 0
    }));
  }

    this.loadParentData();
  }

  loadParentData() {

    forkJoin({
      cityAreas: this.cityAreaService.getAll(),
      statuses: this.statusService.getAll(),
      subCategories: this.subCategoryService.getAll(),
      types: this.typeService.getAll(),
      tags: this.tagService.getAll(),
    }).subscribe(res => {

      this.cityAreas.set(res.cityAreas);
      this.statuses.set(res.statuses);
      this.subCategories.set(res.subCategories);
      this.types.set(res.types);
      this.tags.set(res.tags);

    });
  }

  loadAdvertisement(id: number) {

    this.adService.getById(id).subscribe(ad => {
      this.advertisement.set(ad);
    });

  }

  save() {

    const ad = this.advertisement();

    if (!ad.subCategoryId || !ad.cityAreaId || !ad.statusId || !ad.typeId) {
      alert("Please select all dropdown fields");
      return;
    }

    console.log("Advertisement Payload:", ad);

    if (this.mode() === 'create') {

      this.adService.create(ad).subscribe(() => {
        this.router.navigate(['/admin/advertisements']);
      });

    } else {

      this.adService.update(ad.id, ad).subscribe(() => {
        this.router.navigate(['/admin/advertisements']);
      });

    }

  }

  back() {
    this.router.navigate(['/admin/advertisements']);
  }

  toggleTag(tagId: number) {
    const ad = this.advertisement();
    const tagsId = [...(ad.tagsId ?? [])];

    const index = tagsId.indexOf(tagId);
    if (index > -1) {
      tagsId.splice(index, 1);   // remove
    } else {
      tagsId.push(tagId);         // add
    }

    this.advertisement.set({ ...ad, tagsId }); // signal re-set
  }

  // ✅ Template mein check karne ke liye helper
  isTagSelected(tagId: number): boolean {
    return this.advertisement().tagsId?.includes(tagId) ?? false;
  }

  toDateInput(value: any): string {
  if (!value) return '';
  return new Date(value).toISOString().split('T')[0];
}

onStartsOnChange(value: string) {
  this.advertisement.update(ad => ({ ...ad, startsOn: new Date(value) }));
}
onEndsOnChange(value: string) {
  this.advertisement.update(ad => ({ ...ad, endsOn: new Date(value) }));
}

}

