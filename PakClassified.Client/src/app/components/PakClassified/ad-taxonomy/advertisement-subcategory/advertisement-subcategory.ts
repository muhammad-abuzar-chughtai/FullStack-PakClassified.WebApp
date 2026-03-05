import { Component, computed, signal } from '@angular/core';
import { ModalComponent } from "../../../../shared/modal.component/modal.component";
import { AdvertisementSubCategory } from '../../../../core/models/pakClassified/advertisement-subcategory-model';
import { AdvertisementSubCategoryService } from '../../../../core/services/pakClassified/advertisement-subcategory-service';
import { AuthService } from '../../../../core/services/auth/auth-service';
import { AdvertisementCategoryService } from '../../../../core/services/pakClassified/advertisement-category-service';
import { AdvertisementCategory } from '../../../../core/models/pakClassified/advertisement-category-model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-advertisement-subcategory',
  imports: [CommonModule, ModalComponent, FormsModule],
  templateUrl: './advertisement-subcategory.html',
  styleUrl: './advertisement-subcategory.css',
})
export class AdvertisementSubcategoryComponent {

  // --- Signals ---
  subCategories = signal<AdvertisementSubCategory[]>([]);
  categories = signal<AdvertisementCategory[]>([]);
  selectedSubCategory = signal<AdvertisementSubCategory | null>(null);
  modalOpen = signal(false);
  modalMode = signal<'create' | 'update'>('create');
  // --- Auth Signals ---
  roleName = computed(() => this.auth.roleName());
  isAdmin = computed(() => this.roleName() === 'Admin');

  constructor(private subCategoryService: AdvertisementSubCategoryService, private categoryService: AdvertisementCategoryService, private auth: AuthService) { }

  ngOnInit() {
    this.load();
  }

  // --- Fetching Parent Data ---
  load() {
    this.categoryService.getAll().subscribe((data: AdvertisementCategory[]) => {
      this.categories.set(data);
      this.loadParent();
    });
  }
  loadParent() {
    this.subCategoryService.getAll().subscribe((subCategoryData) => {

      const CategoryList = this.categories();

      const enrichedsubCategories = subCategoryData.map(sc => ({
        ...sc,
        categoryName: CategoryList.find(c => c.id === sc.categoryId)?.name || ''
      }));

      this.subCategories.set(enrichedsubCategories);
    });
  }

  // --- Load categories from API ---
  loadSubCategories() {
    this.subCategoryService.getAll().subscribe((data) => {
      this.subCategories.set(data);  // set signal value — template auto updates
    });
  }

  subCategoryFields = [
    { key: 'name', label: 'Category Name', type: 'text' },
    { key: 'category', label: 'Parent Category', type: 'select', options: this.categories },
    { key: 'Description', label: 'Description', type: 'textarea' },
  ];

  // --- Add Category ---
  addSubCategory() {
    this.selectedSubCategory.set({ id: 0, name: '', description: '', categoryId: 0, createdBy: '', lastModifiedBy: '' } as AdvertisementSubCategory);
    this.modalMode.set('create');
    this.modalOpen.set(true);
  }

  // --- Edit Category ---
  editSubCategory(category: AdvertisementSubCategory) {
    this.selectedSubCategory.set({ ...category });
    this.modalMode.set('update');
    this.modalOpen.set(true);
  }

  // --- Delete Category ---
  deleteSubCategory(id: number) {
    if (!confirm('Are you sure you want to delete this category?')) return;
    this.subCategoryService.delete(id).subscribe(() => {
      this.loadSubCategories();
    });
  }

  // --- Save Category ---
  saveSubCategory(category: AdvertisementSubCategory) {
    if (this.modalMode() === 'create') {
      this.subCategoryService.create(category).subscribe(() => {
        this.loadSubCategories();
        this.modalOpen.set(false);
      });
    } else {
      this.subCategoryService.update(category.id, category).subscribe(() => {
        this.loadSubCategories();
        this.modalOpen.set(false);
      });
    }
  }
}

