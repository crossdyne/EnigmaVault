import { Component, input, output } from '@angular/core';
import { AssetUrlResponse } from '../../../features/passwords/models/dto/asset-urls.response';
import { CdkContextMenuTrigger, CdkMenu, CdkMenuItem, CdkMenuTrigger } from '@angular/cdk/menu';

@Component({
  selector: 'icons',
  templateUrl: './icons.component.html',
  styleUrl: './icons.component.scss',
  standalone: true,
  imports: [
    CdkMenu,
    CdkMenuItem,
    CdkContextMenuTrigger
  ]
})
export class IconsComponent {
  icons = input.required<AssetUrlResponse[]>();
  selectedIcon = output<AssetUrlResponse>();
}