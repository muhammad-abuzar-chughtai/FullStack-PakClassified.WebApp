import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent {
  @Input() item: any;
  @Input() role: string = 'Admin'; // default view only
  @Output() edit = new EventEmitter<any>();
  @Output() delete = new EventEmitter<any>();


 getKeys() {
    return Object.keys(this.item || {}).filter(k => k !== 'id' && k !== 'createdBy' && k !== 'lastModifiedBy' && k !== 'countryId' && k !== 'provinceId' && k !== 'cityId');
  }


}
