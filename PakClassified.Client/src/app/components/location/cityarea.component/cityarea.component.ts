import { Component, computed, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModalComponent } from '../../../shared/modal.component/modal.component';
import { CityArea } from '../../../core/models/location/cityarea-model';
import { CityAreaService } from '../../../core/services/location/cityarea-service';
import { City } from '../../../core/models/location/city-model';
import { CityService } from '../../../core/services/location/city-service';
import { AuthService } from '../../../core/services/auth/auth-service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-cityarea',
  standalone: true,
  imports: [CommonModule, ModalComponent, FormsModule, RouterModule],
  templateUrl: './cityarea.component.html',
  styleUrls: ['./cityarea.component.css']
})

export class CityAreaComponent implements OnInit {

  // --- Signals ---
  cityareas = signal<CityArea[]>([]);
  cities = signal<City[]>([]);
  selectedCityArea = signal<CityArea | null>(null);
  modalOpen = signal(false);
  modalMode = signal<'create' | 'update'>('create');
  // --- Auth Signals ---
  roleName = computed(() => this.auth.roleName());
  isAdmin = computed(() => this.roleName() === 'Admin');
  // Search Filter based on keyword
  searchQuery = signal('');
  selectedCityId = signal<number>(0);
  filteredCityareas = computed(() => {
    const q = this.searchQuery().trim().toLowerCase();
    const cityId = this.selectedCityId();

    return this.cityareas().filter(ca => {
      const matchesName = !q || ca.name.toLowerCase().includes(q);
      const matchesCity = cityId === 0 || ca.cityId === cityId;
      return matchesName && matchesCity;
    });
  });

  constructor(private cityareaService: CityAreaService, private cityService: CityService, private auth: AuthService) { }

  ngOnInit() {
    // this.loadCityareas();
    this.loadParent();
  }

  // --- Load Cityareas ---
  loadParent() {
    this.cityService.getAll().subscribe((data: City[]) => {
      this.cities.set(data);
      this.load();
    });
  }
  load() {
    this.cityareaService.getAll().subscribe((cityareaData) => {

      const cityList = this.cities();

      const enrichedCityareas = cityareaData.map(c => ({
        ...c,
        cityName: cityList.find(ca => ca.id === c.cityId)?.name || ''
      }));

      this.cityareas.set(enrichedCityareas);
    });
  }

  cityAreaFields = [
    { key: 'name', label: 'Area Name', type: 'text' },
    { key: 'cityId', label: 'City', type: 'select', options: this.cities }
  ];



  // --- Add CityArea ---
  addCityArea() {
    this.selectedCityArea.set({ id: 0, name: '', cityId: 0, createdBy: '', lastModifiedBy: '' });
    this.modalMode.set('create');
    this.modalOpen.set(true);
  }

  // --- Edit CityArea ---
  editCityArea(cityarea: CityArea) {
    this.selectedCityArea.set({ ...cityarea });
    this.modalMode.set('update');
    this.modalOpen.set(true);
  }

  // --- Delete CityArea ---
  deleteCityArea(id: number) {
    if (!confirm('Delete this city area?')) return;
    this.cityareaService.delete(id).subscribe(() => this.load());
  }

  // --- Save CityArea ---
  saveCityArea(cityarea: CityArea) {
    if (this.modalMode() === 'create') {
      this.cityareaService.create(cityarea).subscribe(() => {
        this.load();
        this.modalOpen.set(false);
      });
    } else {
      this.cityareaService.update(cityarea.id, cityarea).subscribe(() => {
        this.load();
        this.modalOpen.set(false);
      });
    }
  }
}

