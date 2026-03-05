import { Component, computed, OnInit, signal } from '@angular/core';
import { AdvertisementCategory } from '../../../../core/models/pakClassified/advertisement-category-model';
import { AdvertisementCategoryService } from '../../../../core/services/pakClassified/advertisement-category-service';
import { AuthService } from '../../../../core/services/auth/auth-service';
import { CommonModule } from '@angular/common';
import { ModalComponent } from '../../../../shared/modal.component/modal.component';

@Component({
  selector: 'app-advertisement-category',
  imports: [CommonModule, ModalComponent],
  templateUrl: './advertisement-category.html',
  styleUrl: './advertisement-category.css',
})
export class AdvertisementCategoryComponent implements OnInit {

  // --- Signals ---
  categories = signal<AdvertisementCategory[]>([]);
  selectedCategory = signal<AdvertisementCategory | null>(null);
  modalOpen = signal(false);
  modalMode = signal<'create' | 'update'>('create');
  // --- Auth Signals ---
  roleName = computed(() => this.auth.roleName());
  isAdmin = computed(() => this.roleName() === 'Admin');

  constructor(private categoryService: AdvertisementCategoryService, private auth: AuthService) { }

  ngOnInit() {
    this.loadCategories();
  }
  
  // --- Load categories from API ---
  loadCategories() {
    this.categoryService.getAll().subscribe((data) => {
      this.categories.set(data);  // set signal value — template auto updates
    });
  }

  categoryFields = [
    { key: 'name', label: 'Category Name', type: 'text' },
    { key: 'Description', label: 'Description', type: 'textarea' }
  ];

  // --- Add Category ---
  addCategory() {
    this.selectedCategory.set({ id: 0, name: '', description: '', createdBy: '', lastModifiedBy: '' } as AdvertisementCategory);
    this.modalMode.set('create');
    this.modalOpen.set(true);
  }

  // --- Edit Category ---
  editCategory(category: AdvertisementCategory) {
    this.selectedCategory.set({ ...category });
    this.modalMode.set('update');
    this.modalOpen.set(true);
  }

  // --- Delete Category ---
  deleteCategory(id: number) {
    if (!confirm('Are you sure you want to delete this category?')) return;
    this.categoryService.delete(id).subscribe(() => {
      this.loadCategories();
    });
  }

  // --- Save Category ---
  saveCategory(category: AdvertisementCategory) {
    if (this.modalMode() === 'create') {
      this.categoryService.create(category).subscribe(() => {
        this.loadCategories();
        this.modalOpen.set(false);
      });
    } else {
      this.categoryService.update(category.id, category).subscribe(() => {
        this.loadCategories();
        this.modalOpen.set(false);
      });
    }
  }
}

