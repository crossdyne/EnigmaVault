import { Component, input, model } from '@angular/core';
import { AssetUrlResponse } from '../../../features/passwords/models/asset-urls.response';

@Component({
  selector: 'icons',
  imports: [],
  templateUrl: './icons.component.html',
  styleUrl: './icons.component.scss',
})
export class IconsComponent {
  icons = input.required<AssetUrlResponse[]>();
  selectedIcon = model<AssetUrlResponse | null>();
}