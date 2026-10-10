import { Component, OnInit, signal } from '@angular/core';
import { AvatarModule } from 'primeng/avatar';
import { ButtonModule } from 'primeng/button';
import { PopoverModule } from 'primeng/popover';
import { ToolbarModule } from 'primeng/toolbar';
import { updatePrimaryPalette } from '@primeuix/themes';

const primaryShades = [50, 100, 200, 300, 400, 500, 600, 700, 800, 900, 950];
const primaryColorKey = 'spoolledger-primary';

@Component({
  selector: 'app-toolbar',
  standalone: true,
  imports: [AvatarModule, ButtonModule, PopoverModule, ToolbarModule],
  templateUrl: './toolbar.html',
  styleUrl: './toolbar.css',
})
export class Toolbar implements OnInit {
  protected readonly dark = signal(false);
  protected readonly colors = [
    { name: 'emerald', value: '#10b981' },
    { name: 'green', value: '#22c55e' },
    { name: 'lime', value: '#84cc16' },
    { name: 'orange', value: '#f97316' },
    { name: 'amber', value: '#f59e0b' },
    { name: 'yellow', value: '#eab308' },
    { name: 'teal', value: '#14b8a6' },
    { name: 'cyan', value: '#06b6d4' },
    { name: 'sky', value: '#0ea5e9' },
    { name: 'blue', value: '#3b82f6' },
    { name: 'indigo', value: '#6366f1' },
    { name: 'violet', value: '#8b5cf6' },
    { name: 'purple', value: '#a855f7' },
    { name: 'fuchsia', value: '#d946ef' },
    { name: 'pink', value: '#ec4899' },
    { name: 'rose', value: '#f43f5e' },
    { name: 'zinc', value: '#71717a' },
  ];

  protected selectPrimary(name: string): void {
    updatePrimaryPalette(
      Object.fromEntries(primaryShades.map((shade) => [shade, `{${name}.${shade}}`])),
    );

    const root = document.documentElement;
    for (const shade of primaryShades) {
      root.style.setProperty(`--p-primary-${shade}`, `var(--p-${name}-${shade})`);
    }
    root.style.setProperty('--p-primary-color', 'light-dark(var(--p-primary-600), var(--p-primary-500))');
    root.style.setProperty('--p-primary-hover-color', 'light-dark(var(--p-primary-700), var(--p-primary-400))');
    root.style.setProperty('--p-primary-active-color', 'light-dark(var(--p-primary-800), var(--p-primary-300))');
    localStorage.setItem(primaryColorKey, name);
  }

  ngOnInit(): void {
    const saved = localStorage.getItem(primaryColorKey);
    if (saved && this.colors.some((color) => color.name === saved)) {
      this.selectPrimary(saved);
    }
  }

  protected toggleDarkMode(): void {
    const next = !this.dark();
    this.dark.set(next);
    document.documentElement.classList.toggle('app-dark', next);
  }
}
