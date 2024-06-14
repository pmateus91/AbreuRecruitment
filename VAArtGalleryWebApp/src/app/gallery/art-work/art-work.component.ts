import { Component, OnInit } from '@angular/core';
import { ArtWorkDto } from '../dtos/art-work/art-work.dto';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { DialogSaveArtWorkComponent } from './dialog-save-art-work/dialog-save-art-work.component';
import { ArtGalleryDto } from '../dtos/art-gallery/art-gallery.dto';
import { SaveArtWorkDto } from '../dtos/art-work/save-art-work.dto';
import { AlertService } from '../../shared-components/alert.service';
import { GalleryService } from '../services/art-gallery.service';
import { ArtWorkService } from '../services/art-work.service';

@Component({
  selector: 'app-artWork',
  templateUrl: './art-work.component.html',
  styleUrl: './art-work.component.css',
})
export class ArtWorkComponent implements OnInit {
  artGallery: ArtGalleryDto;
  artWorks: ArtWorkDto[] = [];
  displayedColumns: string[] = ['name', 'author', 'creationYear', 'askPrice', 'actions'];

  constructor(
    private router: Router,
    private dialog: MatDialog,
    private artGalleryService: GalleryService,
    private artWorkService: ArtWorkService,
    private alertService: AlertService
  ) {
    this.artGallery = this.router.getCurrentNavigation()?.extras.state?.['artGallery'];
  }

  ngOnInit(): void {
    this.loadArtWorks();
  }

  loadArtWorks() {
    this.artGalleryService.getArtGalleryArtWorks(this.artGallery.id).subscribe({
      next: (res) => {
        this.artWorks = res;
      },
      error: () => {
        this.alertService.error('Erro ao carregar obras de arte da galeria selecionada');
      },
    });
  }

  formatPrice(price: number): string {
    if (price == null) return '';

    let numberString = price.toString();
    let [integerPart, decimalPart] = numberString.split('.');
    integerPart = integerPart.replace(/\B(?=(\d{3})+(?!\d))/g, '.');

    if (decimalPart) {
      decimalPart = decimalPart.substring(0, 3).padEnd(3, '0');
    } else {
      decimalPart = '000';
    }

    return `${integerPart},${decimalPart} EUR`;
  }

  createArtWorkClick() {
    this.dialog.open(DialogSaveArtWorkComponent, {
      data: { saveMethod: this.saveArtWork.bind(this) },
    });
  }

  editArtWorkClick(artWorkId: string) {
    this.artWorkService.getArtWorkById(artWorkId).subscribe((res) => {
      if (!res) return;

      this.dialog.open(DialogSaveArtWorkComponent, {
        data: { artWork: res, saveMethod: this.saveArtWork.bind(this) },
      });
    });
  }

  deleteArtWorkClick(artWorkId: string) {
    this.artWorkService.deleteArtWork(artWorkId).subscribe({
      next: () => {
        this.alertService.success('Removido com sucesso');
        this.loadArtWorks();
      },
      error: () => {
        this.alertService.success('Erro ao remover');
      },
    });
  }

  saveArtWork(saveArtWorkDto: SaveArtWorkDto) {
    if (!saveArtWorkDto) return;
    if (saveArtWorkDto.id == null) {
      this.artWorkService.createArtWork(this.artGallery.id, saveArtWorkDto).subscribe({
        next: () => {
          this.alertService.success('Guardado com sucesso');
          this.loadArtWorks();
        },
        error: () => {
          this.alertService.success('Erro ao gravar');
        },
      });
    } else {
      this.artWorkService.updateArtWork(this.artGallery.id, saveArtWorkDto).subscribe({
        next: () => {
          this.alertService.success('Guardado com sucesso');
          this.loadArtWorks();
        },
        error: () => {
          this.alertService.success('Erro ao gravar');
        },
      });
    }
  }

  backToGalleryList() {
    this.router.navigate(['art-galleries']);
  }
}
