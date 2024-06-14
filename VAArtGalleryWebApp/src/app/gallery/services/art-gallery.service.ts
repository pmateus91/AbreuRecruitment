import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ArtWorkDto } from '../dtos/art-work/art-work.dto';
import { ArtGalleryDto } from '../dtos/art-gallery/art-gallery.dto';
import { SaveArtGalleryDto } from '../dtos/art-gallery/save-art-gallery.dto';

@Injectable({
  providedIn: 'root',
})
export class GalleryService {
  private baseUrl = 'https://localhost:7042/api/art-galleries';

  constructor(private http: HttpClient) {}

  getGalleries(): Observable<ArtGalleryDto[]> {
    return this.http.get<ArtGalleryDto[]>(`${this.baseUrl}/GetAllGalleries`);
  }

  getArtGalleryArtWorks(artGalleryId: string): Observable<ArtWorkDto[]> {
    return this.http.get<ArtWorkDto[]>(`${this.baseUrl}/GetArtGalleryArtWorks`, {
      params: new HttpParams().set('artGalleryId', artGalleryId),
    });
  }

  getArtGalleryById(artGalleryId: string): Observable<ArtGalleryDto> {
    return this.http.get<ArtGalleryDto>(`${this.baseUrl}/GetArtGalleryById`, {
      params: new HttpParams().set('artGalleryId', artGalleryId),
    });
  }

  createArtGallery(saveArtGalleryDto: SaveArtGalleryDto): Observable<ArtGalleryDto> {
    return this.http.post<ArtGalleryDto>(`${this.baseUrl}`, saveArtGalleryDto);
  }

  updateArtGallery(saveArtGalleryDto: SaveArtGalleryDto): Observable<ArtGalleryDto> {
    return this.http.put<ArtGalleryDto>(`${this.baseUrl}`, saveArtGalleryDto);
  }

  deleteArtGallery(artGalleryId: string): Observable<boolean> {
    return this.http.delete<boolean>(`${this.baseUrl}`, {
      params: { artGalleryId },
    });
  }
}
