import { Component, computed, input, output } from '@angular/core';
import { AssetUrlResponse } from '../../../features/passwords/models/dto/asset-urls.response';
import { CdkContextMenuTrigger, CdkMenu, CdkMenuItem } from '@angular/cdk/menu';
import { IconCategoryResponse } from '../../../features/passwords/models/dto/icon-category.response';
import { TooltipDirective } from '../../directives/tooltip.directive';

@Component({
  selector: 'icons',
  templateUrl: './icons.component.html',
  styleUrl: './icons.component.scss',
  standalone: true,
  imports: [
    TooltipDirective,
    CdkMenu,
    CdkMenuItem,
    CdkContextMenuTrigger
  ]
})
export class IconsComponent {
  icons = input.required<AssetUrlResponse[]>();
  categories = input.required<IconCategoryResponse[]>();
  groupedIcons = computed(() =>{
    const icons = this.icons();
    const categories = this.categories();

    const categoryMap = new Map<string, string>();
    for (const cat of categories) {
      categoryMap.set(cat.categoryId, cat.name);
    }

    const groups = new Map<string, AssetUrlResponse[]>();
    for (const icon of icons){
      const categoryName = categoryMap.get(icon.categoryId) || 'Без категории';

      if (!groups.has(categoryName)) 
        groups.set(categoryName, []);

      groups.get(categoryName)!.push(icon);
    }

    return Array.from(groups.entries())
      .sort(([titleA], [titleB]) => titleA.localeCompare(titleB, 'ru'))
      .map(([title, groupIcons]) => {
        const sortedIcons = [...groupIcons].sort((a, b) => a.assetName.localeCompare(b.assetName, 'ru'));

        return { title, icons: sortedIcons };
      })
  });

  selectedIcon = output<AssetUrlResponse>();
}