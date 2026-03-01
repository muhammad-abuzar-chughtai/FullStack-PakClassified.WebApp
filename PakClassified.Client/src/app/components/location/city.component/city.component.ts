import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModalComponent } from '../../../shared/modal.component/modal.component';
import { City } from '../../../core/models/location/city-model';
import { CityService } from '../../../core/services/location/city-service';
import { Province } from '../../../core/models/location/province-model';
import { ProvinceService } from '../../../core/services/location/province-service';

@Component({
  selector: 'app-city',
  standalone: true,
  imports: [CommonModule, ModalComponent, FormsModule],
  templateUrl: './city.component.html',
  styleUrls: ['./city.component.css']
})
export class CityComponent implements OnInit {

  // --- Signals ---
  cities = signal<City[]>([]);
  provinces = signal<Province[]>([]);
  selectedCity = signal<City | null>(null);
  modalOpen = signal(false);
  modalMode = signal<'create' | 'update'>('create');
  userRole = 'Admin';


  constructor(private cityService: CityService, private provinceService: ProvinceService) { }

  ngOnInit() {
    // this.loadCities();
    this.load();
  }

  // --- Load Cities ---
  load() {
    this.provinceService.getAll().subscribe((data: Province[]) => {
      this.provinces.set(data);
      this.loadParent();
    });
  }
  loadParent() {
    this.cityService.getAll().subscribe((cityData) => {

      const provinceList = this.provinces();

      const enrichedCities = cityData.map(c => ({
        ...c,
        provinceName: provinceList.find(p => p.id === c.provinceId)?.name || ''
      }));

      this.cities.set(enrichedCities);
    });
  }

  cityFields = [
    { key: 'name', label: 'City Name', type: 'text' },
    { key: 'provinceId', label: 'Province', type: 'select', options: this.provinces }
  ];

  // --- Add City ---
  addCity() {
    this.selectedCity.set({ id: 0, name: '', provinceId: 0, createdBy: '', lastModifiedBy: '' });
    this.modalMode.set('create');
    this.modalOpen.set(true);
  }

  // --- Edit City ---
  editCity(city: City) {
    this.selectedCity.set({ ...city });
    this.modalMode.set('update');
    this.modalOpen.set(true);
  }

  // --- Delete City ---
  deleteCity(id: number) {
    if (!confirm('Delete this city?')) return;
    this.cityService.delete(id).subscribe(() => this.loadParent());
  }

  // --- Save City ---
  saveCity(city: City) {
    if (this.modalMode() === 'create') {
      this.cityService.create(city).subscribe(() => {
        this.loadParent();
        this.modalOpen.set(false);
      });
    } else {
      this.cityService.update(city.id, city).subscribe(() => {
        this.loadParent();
        this.modalOpen.set(false);
      });
    }
  }
}
