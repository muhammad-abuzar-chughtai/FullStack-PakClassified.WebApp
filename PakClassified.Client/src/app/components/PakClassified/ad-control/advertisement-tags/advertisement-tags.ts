import { Component, computed, signal } from '@angular/core';
import { AdvertisementTag } from '../../../../core/models/pakClassified/advertisement-tag-model';
import { CommonModule } from '@angular/common';
import { ModalComponent } from '../../../../shared/modal.component/modal.component';
import { AdvertisementTagService } from '../../../../core/services/pakClassified/advertisement-tag-service';
import { AuthService } from '../../../../core/services/auth/auth-service';

@Component({
  selector: 'app-advertisement-tags',
  imports: [CommonModule, ModalComponent],
  templateUrl: './advertisement-tags.html',
  styleUrl: './advertisement-tags.css',
})
export class AdvertisementTagsComponent {

  // -------- Signals --------
  tags = signal<AdvertisementTag[]>([]);
  selectedTags = signal<AdvertisementTag | null>(null);
  modalOpen = signal(false);
  modalMode = signal<'create' | 'update'>('create');

  // -------- Auth Signals --------
  roleName = computed(() => this.auth.roleName());
  isAdmin = computed(() => this.roleName() === 'Admin');
  
  constructor(
    private tagService: AdvertisementTagService,
    private auth: AuthService
  ) { }

  ngOnInit() {
    this.loadTags();
  }

  // -------- Load Data --------
  loadTags() {
    this.tagService.getAll().subscribe(data => {
      this.tags.set(data);
    });
  }

  tagFields = [
    { key: 'name', label: 'Tag Name', type: 'text' },
  ]
  // -------- Add --------
  addTags() {
    this.selectedTags.set({ id: 0, name: '', createdBy: '', lastModifiedBy: '' } as AdvertisementTag);
    this.modalMode.set('create');
    this.modalOpen.set(true);
  }

  // -------- Edit --------
  editTags(tag: AdvertisementTag) {
    this.selectedTags.set({ ...tag });
    this.modalMode.set('update');
    this.modalOpen.set(true);
  }

  // -------- Delete --------
  deleteTags(id: number) {
    if (!confirm('Are you sure you want to delete this Tag?')) return;

    this.tagService.delete(id).subscribe(() => {
      this.loadTags();
    });
  }

  // -------- Save --------
  saveTags(tag: AdvertisementTag) {

    if (this.modalMode() === 'create') {

      this.tagService.create(tag).subscribe(() => {
        this.loadTags();
        this.modalOpen.set(false);
      });

    } else {

      this.tagService.update(tag.id, tag).subscribe(() => {
        this.loadTags();
        this.modalOpen.set(false);
      });
    }
  }
}