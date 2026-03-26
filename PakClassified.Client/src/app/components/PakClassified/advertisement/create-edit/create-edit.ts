import { Component, computed, inject, NgModule, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, Validators, ReactiveFormsModule, AbstractControl } from '@angular/forms';
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
import { AdvertisementCategory } from '../../../../core/models/pakClassified/advertisement-category-model';
import { AdvertisementCategoryService } from '../../../../core/services/pakClassified/advertisement-category-service';

@Component({
  selector: 'app-create-edit',
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule],
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

  categories = signal<AdvertisementCategory[]>([]);
  selectedCategoryId = signal<number>(0);

  // filtered subcategories based on selected category
  filteredSubCategories = signal<AdvertisementSubCategory[]>([]);
  // error message for category
  categoryError = signal('');


  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private auth: AuthService,
    private adService: AdvertisementService,
    private cityAreaService: CityAreaService,
    private statusService: AdvertisementStatusService,
    private categoryService: AdvertisementCategoryService,
    private subCategoryService: AdvertisementSubCategoryService,
    private typeService: AdvertisementTypeService,
    private tagService: AdvertisementTagService
  ) { }

  private fb = inject(FormBuilder);

  adForm = this.fb.group({
    name: ['', Validators.required],
    title: ['', Validators.required],
    description: ['', Validators.required],
    price: [0, [Validators.required, Validators.min(1)]],
    startsOn: ['', Validators.required],
    endsOn: ['', Validators.required],
    categoryId: [0, [Validators.required, Validators.min(1)]],
    subCategoryId: [0, [Validators.required, Validators.min(1)]],
    cityAreaId: [0, [Validators.required, Validators.min(1)]],
    statusId: [0, [Validators.required, Validators.min(1)]],
    typeId: [0, [Validators.required, Validators.min(1)]],
  });

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
      categories: this.categoryService.getAll(),
      subCategories: this.subCategoryService.getAll(),
      types: this.typeService.getAll(),
      tags: this.tagService.getAll(),
    }).subscribe(res => {

      this.cityAreas.set(res.cityAreas);
      this.statuses.set(res.statuses);
      this.categories.set(res.categories);
      this.subCategories.set(res.subCategories);
      this.filteredSubCategories.set(res.subCategories);
      this.types.set(res.types);
      this.tags.set(res.tags);

    });
  }

  loadAdvertisement(id: number) {
    this.adService.getById(id).subscribe(ad => {
      this.advertisement.set(ad);

      const matchedSub = this.subCategories().find(sc => sc.id === ad.subCategoryId);
      if (matchedSub) this.selectedCategoryId.set(matchedSub.categoryId);


      this.adForm.patchValue({
        name: ad.name,
        title: ad.title,
        description: ad.description,
        price: ad.price,
        startsOn: this.toDateInput(ad.startsOn),
        endsOn: this.toDateInput(ad.endsOn),
        subCategoryId: ad.subCategoryId,
        cityAreaId: ad.cityAreaId,
        statusId: ad.statusId,
        typeId: ad.typeId,
      });
    });
  }

  save() {
     this.adForm.markAllAsTouched();
     if (this.adForm.invalid) return;

    if (!this.selectedCategoryId()) {
      this.categoryError.set('Please select a category.');
    }

    if (this.adForm.invalid || !this.selectedCategoryId()) return;

    const formVal = this.adForm.getRawValue();
    const ad: AdvertisementGetPost = {
      ...this.advertisement(),
      name: formVal.name!,
      title: formVal.title!,
      description: formVal.description!,
      price: formVal.price!,
      startsOn: new Date(formVal.startsOn!),
      endsOn: new Date(formVal.endsOn!),
      subCategoryId: formVal.subCategoryId!,
      cityAreaId: formVal.cityAreaId!,
      statusId: formVal.statusId!,
      typeId: formVal.typeId!,
    };

    if (this.mode() === 'create') {
      this.adService.create(ad).subscribe(() => this.router.navigate(['/admin/advertisements']));
    } else {
      this.adService.update(ad.id, ad).subscribe(() => this.router.navigate(['/admin/advertisements']));
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

  onCategoryChange(categoryId: any) {
    const id = Number(categoryId);
    this.selectedCategoryId.set(id);
    this.categoryError.set('');
    this.adForm.patchValue({ categoryId: id, subCategoryId: 0 });

    if (!id) {
      this.filteredSubCategories.set(this.subCategories()); // show all if none selected
    } else {
      this.filteredSubCategories.set(
        this.subCategories().filter(sc => sc.categoryId === id)
      );
    }
  }

  // ── same helpers as auth ──
  ctrl(name: string): AbstractControl {
    return (this.adForm as any).get(name)!;
  }

  showError(name: string): boolean {
    const c = this.ctrl(name);
    return c.invalid && c.touched;
  }

  errorMsg(name: string): string {
    const errors = this.ctrl(name).errors;
    if (!errors) return '';
    if (errors['required']) return 'This field is required.';
    if (errors['min']) return name === 'price' ? 'Price must be greater than 0.' : 'Please select an option.';
    return 'Invalid value.';
  }

}

