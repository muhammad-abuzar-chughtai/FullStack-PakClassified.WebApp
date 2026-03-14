import { Component, computed, OnInit, signal } from '@angular/core';
import { ModalComponent } from "../../../shared/modal.component/modal.component";
import { CommonModule } from '@angular/common';
import { Role } from '../../../core/models/user/role-model';
import { AuthService } from '../../../core/services/auth/auth-service';
import { RoleService } from '../../../core/services/user/role-service';

@Component({
  selector: 'app-role.component',
  standalone: true,
  imports: [CommonModule, ModalComponent],
  templateUrl: './role.component.html',
  styleUrl: './role.component.css',
})
export class RoleComponent implements OnInit {

  // --- Signals ---
  roles = signal<Role[]>([]);
  selectedRole = signal<Role | null>(null);
  modalOpen = signal(false);
  modalMode = signal<'create' | 'update'>('create');
  // --- Auth Signals ---
  roleName = computed(() => this.auth.roleName());
  isAdmin = computed(() => this.roleName() === 'Admin');

  
  constructor(private roleService: RoleService, private auth: AuthService) { }

  ngOnInit() {
    this.loadRoles();
  }

  // --- Load roles from API ---
  loadRoles() {
    this.roleService.getAll().subscribe((data) => {
      this.roles.set(data);  // set signal value — template auto updates
    });
  }

  roleFields = [
    { key: 'name', label: 'Role Name', type: 'text' }
  ];

  // --- Add Role ---
  addRole() {
    this.selectedRole.set({ id: 0, name: '', createdBy: '', lastModifiedBy: '' } as Role);
    this.modalMode.set('create');
    this.modalOpen.set(true);
  }

  // --- Edit Role ---
  editRole(role: Role) {
      if (role.id !== 1 && role.id !== 4) {
    this.selectedRole.set({ ...role });
    this.modalMode.set('update');
    this.modalOpen.set(true);
      } else {
        window.alert("This Role Cannot be Change.");
      }
  }

  // --- Delete Role ---
  deleteRole(id: number) {
    debugger;
    if (id !== 1 && id !== 4) {
      if (!confirm('Are you sure you want to delete this role?')) return;
      this.roleService.delete(id).subscribe(() => {
        this.loadRoles();
      });
    }
    else {
      window.alert("This Role Cannot be Deleted.");
    }
  }

  // --- Save Role ---
  saveRole(role: Role) {
    if (this.modalMode() === 'create') {
      this.roleService.create(role).subscribe(() => {
        this.loadRoles();
        this.modalOpen.set(false);
      });
    } else {
      if (role.id !== 1 && role.id !== 4) {
        this.roleService.update(role.id, role).subscribe(() => {
          this.loadRoles();
          this.modalOpen.set(false);
        });
      } else {
        window.alert("This Role Cannot be Change.");
      }
    }
  }
}