import { CommonModule } from '@angular/common';
import { Component, computed, OnInit, signal } from '@angular/core';
import { ModalComponent } from '../../../shared/modal.component/modal.component';
import { FormsModule } from '@angular/forms';
import { UserGet, UserPost } from '../../../core/models/user/user-model';
import { Role } from '../../../core/models/user/role-model';
import { UserService } from '../../../core/services/user/user-service';
import { RoleService } from '../../../core/services/user/role-service';
import { AuthService } from '../../../core/services/auth/auth-service';

@Component({
  selector: 'app-user.component',
  standalone: true,
  imports: [CommonModule, ModalComponent, FormsModule],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css',
})
export class UserComponent implements OnInit {

  // --- Signals ---
  getUsers = signal<UserGet[]>([]);
  postUser = signal<UserPost | null>(null);
  roles = signal<Role[]>([]);
  selectedUser = signal<UserGet | null>(null);
  modalOpen = signal(false);
  modalMode = signal<'create' | 'update'>('create');
  // --- Auth Signals ---
  roleName = computed(() => this.auth.roleName());
  isAdmin = computed(() => this.roleName() === 'Admin');

  constructor(private userService: UserService, private roleService: RoleService, private auth: AuthService) { }

  ngOnInit() {
    this.loadParent();
  }

  // --- Fetching Parent Data ---
  loadParent() {
    this.roleService.getAll().subscribe((data: Role[]) => {
      this.roles.set(data);
      this.load();
    });
  }
  load() {
    this.userService.getAll().subscribe((userData) => {

      const rolesList = this.roles();

      const enrichedUsers = userData.map(u => ({
        ...u,
        roleName: rolesList.find(r => r.id === u.roleId)?.name || ''
      }));

      this.getUsers.set(enrichedUsers);
    });
  }


  userFields = [
    { key: 'name', label: 'User Name', type: 'text' },
    { key: 'roleId', label: 'Roles', type: 'select', options: this.roles }
  ];

  // --- Add User ---
  addUser() {
    this.selectedUser.set({
      id: 0,
      name: '',
      email: '',
      profilePic: '',
      contactNo: 0,
      dob: new Date(),
      createdBy: '',
      roleId: 0,
    });
    this.modalMode.set('create');
    this.modalOpen.set(true);
  }

  // --- Edit User ---
  editUser(user: UserGet) {
    this.selectedUser.set({ ...user });
    this.modalMode.set('update');
    this.modalOpen.set(true);
  }

  // --- Delete User ---
  deleteUser(id: number) {
    if (!confirm('Delete this User?')) return;
    this.userService.delete(id).subscribe(() => this.loadParent());
  }

  // --- Save User ---
  saveUser(user: UserPost) {
    if (this.modalMode() === 'create') {
      this.userService.create(user).subscribe(() => {
        this.load();
        this.modalOpen.set(false);
      });
    } else {
      this.userService.update(user.id, user).subscribe(() => {
        this.load();
        this.modalOpen.set(false);
      });
    }
  }
}
