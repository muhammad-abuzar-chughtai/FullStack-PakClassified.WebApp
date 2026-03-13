import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ImageCard } from "./image-card/image-card";

@Component({
  selector: 'app-advertisement-image',
  imports: [RouterModule, ImageCard],
  templateUrl: './advertisement-image.html',
  styleUrl: './advertisement-image.css',
})
export class AdvertisementImageComponent {

}
