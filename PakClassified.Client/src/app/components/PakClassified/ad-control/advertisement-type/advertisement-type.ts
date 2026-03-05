import { Component, computed, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdvertisementTypeService } from '../../../../core/services/pakClassified/advertisement-type-service';
import { AuthService } from '../../../../core/services/auth/auth-service';
import { AdvertisementType } from '../../../../core/models/pakClassified/advertisement-type-model';
import { ModalComponent } from "../../../../shared/modal.component/modal.component";

@Component({
  selector: 'app-advertisement-type',
  imports: [CommonModule, ModalComponent],
  templateUrl: './advertisement-type.html',
  styleUrl: './advertisement-type.css',
})
export class AdvertisementTypeComponent implements OnInit {

  // -------- S ignals --------
  types = signal<AdvertisementType[]>([]);
  selectedType = signal<AdvertisementType | null>(null);
  modalOpen = signal(false);
  modalMode = signal<'create' | 'update'>('create');

  // -------- Auth Signals --------
  roleName = computed(() => this.auth.roleName());
  isAdmin = computed(() => this.roleName() === 'Admin');
  
  constructor(
    private typeService: AdvertisementTypeService,
    private auth: AuthService
  ) { }

  ngOnInit() {
    this.loadTypes();
  }

  // -------- Load Data --------
  loadTypes() {
    this.typeService.getAll().subscribe(data => {
      this.types.set(data);
    });
  }

  typeFields = [
    { key: 'name', label: 'Type Name', type: 'text' }
  ];

  // -------- Add --------
  addType() {
    this.selectedType.set({ id: 0, name: '', createdBy: '', lastModifiedBy: '' } as AdvertisementType);
    this.modalMode.set('create');
    this.modalOpen.set(true);
  }

  // -------- Edit --------
  editType(type: AdvertisementType) {
    this.selectedType.set({ ...type });
    this.modalMode.set('update');
    this.modalOpen.set(true);
  }

  // -------- Delete --------
  deleteType(id: number) {
    if (!confirm('Are you sure you want to delete this type?')) return;

    this.typeService.delete(id).subscribe(() => {
      this.loadTypes();
    });
  }

  // -------- Save --------
  saveType(type: AdvertisementType) {

    if (this.modalMode() === 'create') {

      this.typeService.create(type).subscribe(() => {
        this.loadTypes();
        this.modalOpen.set(false);
      });

    } else {

      this.typeService.update(type.id, type).subscribe(() => {
        this.loadTypes();
        this.modalOpen.set(false);
      });
    }
  }
}