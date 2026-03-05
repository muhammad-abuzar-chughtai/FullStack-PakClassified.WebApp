import { Component, computed, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModalComponent } from '../../../shared/modal.component/modal.component';
import { Province } from '../../../core/models/location/province-model';
import { ProvinceService } from '../../../core/services/location/province-service';
import { Country } from '../../../core/models/location/country-model';
import { CountryService } from '../../../core/services/location/country-service';
import { AuthService } from '../../../core/services/auth/auth-service';

@Component({
  selector: 'app-province',
  standalone: true,
  imports: [CommonModule, ModalComponent, FormsModule],
  templateUrl: './province.component.html',
  styleUrls: ['./province.component.css']
})
export class ProvinceComponent implements OnInit {

  // --- Signals ---
  provinces = signal<Province[]>([]);
  countries = signal<Country[]>([]);
  selectedProvince = signal<Province | null>(null);
  modalOpen = signal(false);
  modalMode = signal<'create' | 'update'>('create');
  // --- Auth Signals ---
  roleName = computed(() => this.auth.roleName());
  isAdmin = computed(() => this.roleName() === 'Admin');



  constructor(private provinceService: ProvinceService, private countryService: CountryService, private auth: AuthService) { }

  ngOnInit() {
    // this.loadProvinces();
    this.load();
  }

  // --- Load provinces ---
  // loadProvinces() {
  //   this.provinceService.getAll().subscribe((data) => {
  //     this.provinces.set(data); // signal update triggers template
  //   });
  // }


  // --- Fetching Parent Data ---
  load() {
    this.countryService.getAll().subscribe((data: Country[]) => {
      this.countries.set(data);
      this.loadParent();
    });
  }
  loadParent() {
    this.provinceService.getAll().subscribe((provinceData) => {

      const countryList = this.countries();

      const enrichedProvinces = provinceData.map(p => ({
        ...p,
        countryName: countryList.find(c => c.id === p.countryId)?.name || ''
      }));

      this.provinces.set(enrichedProvinces);
    });
  }


  provinceFields = [
    { key: 'name', label: 'Province Name', type: 'text' },
    { key: 'countryId', label: 'Country', type: 'select', options: this.countries }
  ];

  // --- Add Province ---
  addProvince() {
    this.selectedProvince.set({ id: 0, name: '', countryId: 0, createdBy: '', lastModifiedBy: '' });
    this.modalMode.set('create');
    this.modalOpen.set(true);
  }

  // --- Edit Province ---
  editProvince(province: Province) {
    this.selectedProvince.set({ ...province });
    this.modalMode.set('update');
    this.modalOpen.set(true);
  }

  // --- Delete Province ---
  deleteProvince(id: number) {
    if (!confirm('Delete this province?')) return;
    this.provinceService.delete(id).subscribe(() => this.loadParent());
  }

  // --- Save Province ---
  saveProvince(province: Province) {
    if (this.modalMode() === 'create') {
      this.provinceService.create(province).subscribe(() => {
        this.loadParent();
        this.modalOpen.set(false);
      });
    } else {
      this.provinceService.update(province.id, province).subscribe(() => {
        this.loadParent();
        this.modalOpen.set(false);
      });
    }
  }
}
