import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserService } from '@modules/auth/service/user.service';
import { Observable } from 'rxjs';
import { FavoriteItem } from 'src/app/core/models/shared.model';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class FavoritesService {

  private apiUrl = `${environment.apiUrl}/favorites`;

  constructor(private http: HttpClient) {}

  getFavorites(userId: number): Observable<FavoriteItem[]> {
    return this.http.get<FavoriteItem[]>(`${this.apiUrl}/${userId}`);
  }

  addFavorite(favorite: any): Observable<any> {
    return this.http.post(`${this.apiUrl}`, favorite);
  }

  removeFavorite(userId: number, favoriteId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${userId}/${favoriteId}`);
  }
    
  }