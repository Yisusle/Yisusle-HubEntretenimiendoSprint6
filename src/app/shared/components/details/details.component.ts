import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { UserService } from '@modules/auth/service/user.service';
import { FavoritesService } from '@modules/favorites/service/favorites.service';
import { Movie } from 'src/app/core/models/movie.model';

@Component({
  selector: 'app-details',
  standalone: true,
  imports: [CommonModule, MatDialogModule],
  templateUrl: './details.component.html',
  styleUrl: './details.component.css'
})
export class DetailsComponent implements OnInit{

  currentUser: any;

    constructor(
    public dialogRef: MatDialogRef<DetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Movie,
    private userService: UserService,
    private favoritesService: FavoritesService
  ) {}

  ngOnInit(): void {
    this.currentUser = this.userService.getCurrentUser();
    console.log(this.currentUser);
    //console.log(localStorage.getItem('currentUser'));
  }
  onClose(): void {
    this.dialogRef.close();
  } 

  addToFavorites(): void {
    console.log(this.currentUser);
    if (this.currentUser && this.currentUser.Id) {
      const isMovie = this.data.type === 'movie';
      const favoriteItem = {
        usuarioId: this.currentUser.Id,
        peliculaId: isMovie ? this.data.id : null,
        serieId: !isMovie ? this.data.id : null,
        type: isMovie ? 'movie' : 'series'
      };

      this.favoritesService.addFavorite(favoriteItem).subscribe({
        next: (response) => {
          console.log('Favorito añadido correctamente', response);
          this.dialogRef.close(); // Opcionalmente cerrar el diálogo después de añadir a favoritos
        },
        error: (error) => {
          console.error('Error al añadir a favoritos', error);
        }
      });
    } else {
      console.error('Usuario no encontrado');
    }
  }
}
