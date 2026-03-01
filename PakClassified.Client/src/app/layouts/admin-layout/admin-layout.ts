import { Component } from '@angular/core';
import { RouterOutlet } from "@angular/router";
import { FooterComponent } from "../../shared/footer.component/footer.component";
import { SidebarComponent } from "../../shared/sidebar.component/sidebar.component";
import { HeaderComponent } from "../../shared/header.component/header.component";

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [RouterOutlet, FooterComponent, SidebarComponent, HeaderComponent],
  templateUrl: './admin-layout.html',
  styleUrl: './admin-layout.css',
})
export class AdminLayout {

}
