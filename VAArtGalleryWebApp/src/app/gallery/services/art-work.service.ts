import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ArtWorkDto } from '../dtos/art-work/art-work.dto';
import { SaveArtWorkDto } from '../dtos/art-work/save-art-work.dto';

@Injectable({
  providedIn: 'root',
})
export class ArtWorkService {
  baseUrl = 'https://localhost:7042/api/art-works';
  constructor(private http: HttpClient) {}

  getArtWorkById(artWorkId: string): Observable<ArtWorkDto> {
    return this.http.get<ArtWorkDto>(`${this.baseUrl}/getartworkbyid`, {
      params: { artWorkId },
    });
  }

  createArtWork(artGalleryId: string, saveArtWorkDto: SaveArtWorkDto): Observable<ArtWorkDto> {
    return this.http.post<ArtWorkDto>(`${this.baseUrl}`, saveArtWorkDto, {
      params: { artGalleryId },
    });
  }

  updateArtWork(artGalleryId: string, saveArtWorkDto: SaveArtWorkDto): Observable<ArtWorkDto> {
    return this.http.put<ArtWorkDto>(`${this.baseUrl}`, saveArtWorkDto, {
      params: { artGalleryId },
    });
  }

  deleteArtWork(artWorkId: string): Observable<boolean> {
    return this.http.delete<boolean>(`${this.baseUrl}`, {
      params: { artWorkId },
    });
  }
}
