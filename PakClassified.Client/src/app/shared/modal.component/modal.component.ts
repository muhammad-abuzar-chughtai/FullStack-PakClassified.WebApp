import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css']
})
export class ModalComponent {
  @Input() fields: any[] = [];
  @Input() model: any = {};
  @Input() mode: 'create' | 'update' = 'create';
  @Input() entityName: string = '';

  @Output() save = new EventEmitter<any>();
  @Output() close = new EventEmitter<void>();

  submit() {
  const hasEmpty = this.fields.some(f => !this.model[f.key]);
  if (hasEmpty) return;

  this.save.emit(this.model);
}

  getKeys() {
    return Object.keys(this.model || {}).filter(k => k !== 'Id' && k !== 'CreatedBy' && k !== 'LastModifiedBy');
  }

}
 