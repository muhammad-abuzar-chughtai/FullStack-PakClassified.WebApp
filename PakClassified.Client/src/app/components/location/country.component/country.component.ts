import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModalComponent } from '../../../shared/modal.component/modal.component';
import { Country } from '../../../core/models/location/country-model';
import { CountryService } from '../../../core/services/location/country-service';

@Component({
  selector: 'app-country',
  standalone: true,
  imports: [CommonModule, ModalComponent, FormsModule],
  templateUrl: './country.component.html',
  styleUrls: ['./country.component.css']
})
export class CountryComponent implements OnInit {

  // --- Signals ---
  countries = signal<Country[]>([]);
  selectedCountry = signal<Country | null>(null);
  modalOpen = signal(false);
  modalMode = signal<'create' | 'update'>('create');
  userRole = 'Admin';



  constructor(private countryService: CountryService) { }

  ngOnInit() {
    this.loadCountries();
  }

  // --- Load countries from API ---
  loadCountries() {
    this.countryService.getAll().subscribe((data) => {
      this.countries.set(data);  // set signal value — template auto updates
    });
  }

  countryFields = [
    { key: 'name', label: 'Country Name', type: 'text' }
  ];

  // --- Add Country ---
  // this.selectedCountry.set({ id: 0, name: '', createdBy: '', lastModifiedBy: '' });
  addCountry() {
    this.selectedCountry.set({ id: 0, name: '', createdBy: '', lastModifiedBy: '' } as Country);
    this.modalMode.set('create');
    this.modalOpen.set(true);
  }

  // --- Edit Country ---
  editCountry(country: Country) {
    this.selectedCountry.set({ ...country });
    this.modalMode.set('update');
    this.modalOpen.set(true);
  }

  // --- Delete Country ---
  deleteCountry(id: number) {
    if (!confirm('Are you sure you want to delete this country?')) return;
    this.countryService.delete(id).subscribe(() => {
      this.loadCountries();
    });
  }

  // --- Save Country ---
  saveCountry(country: Country) {
    if (this.modalMode() === 'create') {
      this.countryService.create(country).subscribe(() => {
        this.loadCountries();
        this.modalOpen.set(false);
      });
    } else {
      this.countryService.update(country.id, country).subscribe(() => {
        this.loadCountries();
        this.modalOpen.set(false);
      });
    }
  }
}
