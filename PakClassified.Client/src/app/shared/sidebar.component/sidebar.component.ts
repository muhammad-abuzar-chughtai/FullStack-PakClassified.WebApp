import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule, NgIf } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css',
})
export class SidebarComponent {
  constructor(private router: Router) { }

  navigate(path: string) {
    if (path === '') {
      // Navigate to admin default page
      this.router.navigate(['/admin']);
      // Also close any open menu groups
      this.openGroup = '';
    } else {
      this.router.navigate(['/admin', path]);
    }
  }

  openGroup: string = '';
  isCollapsed = false;

  toggleGroup(group: string) {
    this.openGroup = this.openGroup === group ? '' : group;
  }

  toggleSidebar() {
    this.isCollapsed = !this.isCollapsed;
  }

}