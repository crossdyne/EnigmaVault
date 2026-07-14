import { Directive, ElementRef, HostListener, Input, Renderer2, OnDestroy } from '@angular/core';

@Directive({
  selector: '[appTooltip]',
  standalone: true
})
export class TooltipDirective implements OnDestroy {
  @Input('appTooltip') text = '';
  private tooltip: HTMLElement | null = null;

  constructor(private el: ElementRef, private renderer: Renderer2) {}

  @HostListener('mouseenter')
  onMouseEnter() {
    if (!this.text) return;

    this.tooltip = this.renderer.createElement('div');
    this.renderer.addClass(this.tooltip, 'app-tooltip');
    this.renderer.setProperty(this.tooltip, 'textContent', this.text);

    this.renderer.appendChild(document.body, this.tooltip);
    this.position();
  }

  @HostListener('mouseleave')
  onMouseLeave() {
    this.destroy();
  }

  ngOnDestroy() {
    this.destroy();
  }

  private position() {
    if (!this.tooltip) return;

    const rect = this.el.nativeElement.getBoundingClientRect();
    const tooltipRect = this.tooltip.getBoundingClientRect();
    const gap = 12;

    const top = rect.top - tooltipRect.height - gap;
    const left = rect.left + (rect.width - tooltipRect.width) / 2;

    this.renderer.setStyle(this.tooltip, 'top', `${top}px`);
    this.renderer.setStyle(this.tooltip, 'left', `${left}px`);
  }

  private destroy() {
    if (this.tooltip) {
      this.renderer.removeChild(document.body, this.tooltip);
      this.tooltip = null;
    }
  }
}