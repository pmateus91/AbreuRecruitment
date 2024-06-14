import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ArtGalleryDto } from '../dtos/art-gallery/art-gallery.dto';
import { DialogSaveArtGalleryComponent } from './dialog-save-art-gallery/dialog-save-art-gallery.component';
import { GalleryService } from '../services/art-gallery.service';
import { AlertService } from '../../shared-components/alert.service';
import { SaveArtGalleryDto } from '../dtos/art-gallery/save-art-gallery.dto';

@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrl: './gallery.component.css',
})
export class GalleryComponent implements OnInit {
  galleries: ArtGalleryDto[] = [];
  displayedColumns: string[] = ['name', 'city', 'manager', 'nbrWorks', 'actions'];

  constructor(
    private artGalleryService: GalleryService,
    private router: Router,
    private dialog: MatDialog,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    this.loadArtGalleries();
  }

  loadArtGalleries() {
    this.artGalleryService.getGalleries().subscribe({
      next: (res) => {
        this.galleries = res;
      },
      error: () => {
        this.alertService.error('Erro ao carregar as galerias de artes');
      },
    });
  }

  createArtGalleryClick() {
    this.dialog.open(DialogSaveArtGalleryComponent, {
      data: { saveMethod: this.saveArtGallery.bind(this) },
    });
  }

  editArtGalleryClick(artGalleryId: string) {
    this.artGalleryService.getArtGalleryById(artGalleryId).subscribe((res) => {
      if (!res) return;

      this.dialog.open(DialogSaveArtGalleryComponent, {
        data: { artGallery: res, saveMethod: this.saveArtGallery.bind(this) },
      });
    });
  }

  deleteArtGalleryClick(artGalleryId: string) {
    this.artGalleryService.deleteArtGallery(artGalleryId).subscribe({
      next: () => {
        this.alertService.success('Removido com sucesso');
        this.loadArtGalleries();
      },
      error: () => {
        this.alertService.success('Erro ao remover');
      },
    });
  }

  saveArtGallery(saveArtGalleryDto: SaveArtGalleryDto) {
    if (!saveArtGalleryDto) return;

    if (saveArtGalleryDto.id == null) {
      this.artGalleryService.createArtGallery(saveArtGalleryDto).subscribe({
        next: () => {
          this.alertService.success('Guardado com sucesso');
          this.loadArtGalleries();
        },
        error: () => {
          this.alertService.success('Erro ao gravar');
        },
      });
    } else {
      this.artGalleryService.updateArtGallery(saveArtGalleryDto).subscribe({
        next: () => {
          this.alertService.success('Guardado com sucesso');
          this.loadArtGalleries();
        },
        error: () => {
          this.alertService.success('Erro ao gravar');
        },
      });
    }
  }

  openArtWorksList(artGallery: ArtGalleryDto) {
    if (!artGallery) {
      return;
    }

    this.router.navigate([`art-gallery/art-works`], {
      state: { artGallery },
    });
  }
}
