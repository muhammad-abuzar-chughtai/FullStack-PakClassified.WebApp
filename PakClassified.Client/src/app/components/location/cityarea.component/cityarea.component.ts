import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModalComponent } from '../../../shared/modal.component/modal.component';
import { CityArea } from '../../../core/models/location/cityarea-model';
import { CityAreaService } from '../../../core/services/location/cityarea-service';
import { City } from '../../../core/models/location/city-model';
import { CityService } from '../../../core/services/location/city-service';

@Component({
  selector: 'app-cityarea',
  standalone: true,
  imports: [CommonModule, ModalComponent, FormsModule],
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
  userRole = 'Admin';
 

  constructor(private cityareaService: CityAreaService, private cityService: CityService) { }

  ngOnInit() {
      // this.loadCityareas();
      this.load();
    }
  
    // --- Load Cityareas ---
    load() {
      this.cityService.getAll().subscribe((data: City[]) => {
        this.cities.set(data);
        this.loadParent();
      });
    }
    loadParent() {
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
    this.cityareaService.delete(id).subscribe(() => this.loadParent());
  }

  // --- Save CityArea ---
  saveCityArea(cityarea: CityArea) {
    if (this.modalMode() === 'create') {
      this.cityareaService.create(cityarea).subscribe(() => {
        this.loadParent();
        this.modalOpen.set(false);
      });
    } else {
      this.cityareaService.update(cityarea.id, cityarea).subscribe(() => {
        this.loadParent();
        this.modalOpen.set(false);
      });
    }
  }
}

