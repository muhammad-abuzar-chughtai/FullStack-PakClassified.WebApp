import { Component } from '@angular/core';
import { HeaderComponent } from "../../shared/header.component/header.component";
import { CarouselComponent } from "../../components/Web-Components/carousel.component/carousel.component";
import { AboutSectionComponent } from "../../components/Web-Components/about-section.component/about-section.component";
import { CtaSectionComponent } from "../../components/Web-Components/cta-section.component/cta-section.component";
import { FooterSectionComponent } from "../../components/Web-Components/footer-section.component/footer-section.component";
import { FooterComponent } from "../../shared/footer.component/footer.component";

@Component({
  selector: 'app-default-layout',
  imports: [HeaderComponent, CarouselComponent, AboutSectionComponent, CtaSectionComponent, FooterSectionComponent, FooterComponent],
  templateUrl: './default-layout.html',
  styleUrl: './default-layout.css',
})
export class DefaultLayout {

}
