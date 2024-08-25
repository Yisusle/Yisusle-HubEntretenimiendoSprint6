import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../../../shared/components/header/header.component';
import { FooterComponent } from '../../../../shared/components/footer/footer.component';
import { FavoritesService } from '@modules/favorites/service/favorites.service';
import { FavoriteItem } from 'src/app/core/models/shared.model';
import { CommonModule } from '@angular/common';
import { UserService } from '@modules/auth/service/user.service';

@Component({
  selector: 'app-favorites-page',
  standalone: true,
  imports: [CommonModule,HeaderComponent,FooterComponent],
  templateUrl: './favorites-page.component.html',
  styleUrl: './favorites-page.component.css'
})
export class FavoritesPageComponent implements OnInit {
  favorites: FavoriteItem[] = [];

  constructor(
    private userService: UserService,
    private favoritesService: FavoritesService
  ) {}

  ngOnInit() {
    this.loadFavorites();
  }

  loadFavorites(): void {
    const currentUser = this.userService.getCurrentUser();
    if (currentUser && currentUser.Id) {
      this.favoritesService.getFavorites(currentUser.Id).subscribe(
        (favorites) => {
          this.favorites = favorites;
        },
        (error) => {
          console.error('Error al cargar favoritos', error);
        }
      );
    } else {
      console.error('Usuario no identificado');
    }
  }

  removeFavorite(favoriteId: number): void {
    const currentUser = this.userService.getCurrentUser();
    if (currentUser && currentUser.Id) {
      this.favoritesService.removeFavorite(currentUser.Id, favoriteId).subscribe(
        () => {
          this.loadFavorites();  // Recargar los favoritos después de eliminar uno
        },
        (error) => {
          console.error('Error al eliminar favorito', error);
        }
      );
    }
  }
}
