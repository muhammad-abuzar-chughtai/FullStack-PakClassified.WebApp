import { Component, computed, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalComponent } from '../../../../shared/modal.component/modal.component';
import { AdvertisementStatus } from '../../../../core/models/pakClassified/advertisement-status-model';
import { AdvertisementStatusService } from '../../../../core/services/pakClassified/advertisement-status-service';
import { AuthService } from '../../../../core/services/auth/auth-service';

@Component({
  selector: 'app-status',
  standalone: true,
  imports: [CommonModule, ModalComponent],
  templateUrl: './advertisement-status.html',
  styleUrl: './advertisement-status.css'
})
export class AdvertisementStatusComponent implements OnInit {

  // -------- Signals --------
  statuses = signal<AdvertisementStatus[]>([]);
  selectedStatus = signal<AdvertisementStatus | null>(null);
  modalOpen = signal(false);
  modalMode = signal<'create' | 'update'>('create');

  // -------- Auth Signals --------
  roleName = computed(() => this.auth.roleName());
  isAdmin = computed(() => this.roleName() === 'Admin');

  constructor(
    private statusService: AdvertisementStatusService,
    private auth: AuthService
  ) { }

  ngOnInit() {
    this.loadStatuses();
  }

  // -------- Load Data --------
  loadStatuses() {
    this.statusService.getAll().subscribe(data => {
      this.statuses.set(data);
    });
  }

  statusFields = [
    { key: 'name', label: 'Status Name', type: 'text' }
  ];

  // -------- Add --------
  addStatus() {
    this.selectedStatus.set({ id: 0, name: '', createdBy: '', lastModifiedBy: '' } as AdvertisementStatus);
    this.modalMode.set('create');
    this.modalOpen.set(true);
  }

  // -------- Edit --------
  editStatus(status: AdvertisementStatus) {
    this.selectedStatus.set({ ...status });
    this.modalMode.set('update');
    this.modalOpen.set(true);
  }

  // -------- Delete --------
  deleteStatus(id: number) {
    if (!confirm('Are you sure you want to delete this status?')) return;

    this.statusService.delete(id).subscribe(() => {
      this.loadStatuses();
    });
  }

  // -------- Save --------
  saveStatus(status: AdvertisementStatus) {

    if (this.modalMode() === 'create') {

      this.statusService.create(status).subscribe(() => {
        this.loadStatuses();
        this.modalOpen.set(false);
      });

    } else {

      this.statusService.update(status.id, status).subscribe(() => {
        this.loadStatuses();
        this.modalOpen.set(false);
      });
    }
  }
}