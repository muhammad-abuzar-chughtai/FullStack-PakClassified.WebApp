import { CommonModule } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ModalComponent } from '../../../shared/modal.component/modal.component';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserGet, UserPost } from '../../../core/models/user/user-model';
import { Role } from '../../../core/models/user/role-model';
import { UserService } from '../../../core/services/user/user-service';
import { RoleService } from '../../../core/services/user/role-service';
import { AuthService } from '../../../core/services/auth/auth-service';
import { CreateEditUser } from "./create-edit/create-edit";

@Component({
  selector: 'app-user.component',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, CreateEditUser],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css',
})
export class UserComponent implements OnInit {

  private fb = inject(FormBuilder);

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
  // --- Loader ---
  isLoading = signal(false);

  constructor(private userService: UserService, private roleService: RoleService, private auth: AuthService) { }

  ngOnInit() {
    this.loadParent();
  }

  // --- Fetching Parent Data ---
  loadParent() {
    this.isLoading.set(true);
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
    this.isLoading.set(false);
  }


  userFields = [
    { key: 'name', label: 'Full Name', type: 'text' },
    { key: 'email', label: 'Email', type: 'email' },
    { key: 'password', label: 'Password', type: 'pass' },
    { key: 'profilePic', label: 'Profile Picture', type: 'File' },
    { key: 'contactNo', label: 'Contact No.', type: 'number' },
    { key: 'dob', label: 'Date Of Birth', type: 'Date' },
    { key: 'secQues', label: 'Security Question', type: 'text', required: false },
    { key: 'secAns', label: 'Security Answer', type: 'text', required: false },
    { key: 'roleId', label: 'Roles', type: 'select', options: this.roles }
  ];

  // --- Add User ---
  addUser() {
    this.selectedUser.set(null);
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
    this.userService.delete(id).subscribe(() => this.load());
  }

  // --- Save User ---
  saveUser(user: UserPost) {
    debugger;
    if (this.modalMode() === 'create') {
      user.createdBy = 'nothing';
      this.userService.create(user).subscribe(() => {
        this.load();
        this.modalOpen.set(false);
      });
    } else {
      user.id = this.selectedUser()!.id; // ← get id from selectedUser signal
      this.userService.update(user.id, user).subscribe(() => {
        this.load();
        this.modalOpen.set(false);
      });
    }
  }


  // --- CHANGE PASSWORD LOGIC WITH COMPONENT TS ---
  // --- Change Password Signals ---
  changePassOpen = signal(false);
  changePassUser = signal<UserGet | null>(null);
  changePassLoading = signal(false);
  changePassError = signal('');
  changePassStep = signal<1 | 2>(1);
  changePassToken = '';

  // --- Change Password Forms ---
  verifyForm = this.fb.group({
    secQues: ['', Validators.required],
    secAns: ['', Validators.required],
  });

  changePassForm = this.fb.group({
    newPassword: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', Validators.required],
  });

  // --- Open / Close ---
  changePass(user: UserGet) {
    this.changePassUser.set(user);
    this.changePassStep.set(1);
    this.changePassError.set('');
    this.verifyForm.reset();
    this.changePassForm.reset();
    this.changePassOpen.set(true);
  }

  closeChangePass() {
    this.changePassOpen.set(false);
    this.changePassUser.set(null);
    this.changePassStep.set(1);
    this.changePassError.set('');
    this.changePassToken = '';
    this.verifyForm.reset();
    this.changePassForm.reset();
  }

  // --- Error Helpers ---
  showCPError(name: string, form: 'verify' | 'reset'): boolean {
    const f = form === 'verify' ? this.verifyForm : this.changePassForm;
    const c = (f as any).get(name);
    return !!c && c.invalid && c.touched;
  }

  cpErrorMsg(name: string, form: 'verify' | 'reset'): string {
    const f = form === 'verify' ? this.verifyForm : this.changePassForm;
    const errors = (f as any).get(name)?.errors;
    if (!errors) return '';
    if (errors['required']) return 'This field is required.';
    if (errors['minlength']) return `Minimum ${errors['minlength'].requiredLength} characters required.`;
    return 'Invalid value.';
  }

  // --- Step 1: Verify Security ---
  submitVerify() {
    this.verifyForm.markAllAsTouched();
    this.changePassError.set('');

    if (this.verifyForm.invalid) return;

    this.changePassLoading.set(true);
    const user = this.changePassUser()!;
    const { secQues, secAns } = this.verifyForm.getRawValue();

    this.auth.verifySecurity({
      email: user.email,
      secQues: secQues!,
      secAns: secAns!
    }).subscribe({
      next: (token) => {
        this.changePassToken = token;
        this.changePassStep.set(2);
        this.changePassLoading.set(false);
      },
      error: () => {
        this.changePassError.set('Invalid security question or answer.');
        this.changePassLoading.set(false);
      }
    });
  }

  // --- Step 2: Reset Password ---
  submitChangePass() {
    this.changePassForm.markAllAsTouched();
    this.changePassError.set('');

    if (this.changePassForm.invalid) {
      this.changePassError.set('Please fill in all required fields.');
      return;
    }

    const { newPassword, confirmPassword } = this.changePassForm.getRawValue();

    if (newPassword !== confirmPassword) {
      this.changePassError.set('Passwords do not match.');
      return;
    }

    this.changePassLoading.set(true);

    this.auth.resetPassword({
      resetToken: this.changePassToken,
      newPassword: newPassword!
    }).subscribe({
      next: () => {
        this.changePassLoading.set(false);
        this.closeChangePass();
      },
      error: () => {
        this.changePassError.set('Token expired. Please verify again.');
        this.changePassStep.set(1);
        this.changePassToken = '';
        this.changePassLoading.set(false);
      }
    });
  }










}
