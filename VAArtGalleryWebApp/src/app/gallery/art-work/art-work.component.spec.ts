import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ArtWorkComponent } from './art-work.component';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { GalleryService } from '../services/art-gallery.service';
import { ArtWorkService } from '../services/art-work.service';
import { AlertService } from '../../shared-components/alert.service';
import { of, throwError } from 'rxjs';
import { ArtGalleryDto } from '../dtos/art-gallery/art-gallery.dto';
import { ArtWorkDto } from '../dtos/art-work/art-work.dto';
import { SaveArtWorkDto } from '../dtos/art-work/save-art-work.dto';
import { MatIconModule } from '@angular/material/icon';
import { DialogSaveArtWorkComponent } from './dialog-save-art-work/dialog-save-art-work.component';

describe('ArtWorkComponent', () => {
  let component: ArtWorkComponent;
  let fixture: ComponentFixture<ArtWorkComponent>;
  let mockRouter = jasmine.createSpyObj('Router', ['navigate', 'getCurrentNavigation']);
  let mockDialog = jasmine.createSpyObj('MatDialog', ['open']);
  let mockGalleryService = jasmine.createSpyObj('GalleryService', ['getArtGalleryArtWorks']);
  let mockArtWorkService = jasmine.createSpyObj('ArtWorkService', [
    'getArtWorkById',
    'deleteArtWork',
    'createArtWork',
    'updateArtWork',
  ]);
  let mockAlertService = jasmine.createSpyObj('AlertService', ['error', 'success']);

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MatIconModule],
      declarations: [ArtWorkComponent],
      providers: [
        { provide: Router, useValue: mockRouter },
        { provide: MatDialog, useValue: mockDialog },
        { provide: GalleryService, useValue: mockGalleryService },
        { provide: ArtWorkService, useValue: mockArtWorkService },
        { provide: AlertService, useValue: mockAlertService },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ArtWorkComponent);
    component = fixture.componentInstance;

    const artGallery = { id: '1', name: 'Test Gallery' } as ArtGalleryDto;
    mockRouter.getCurrentNavigation.and.returnValue({ extras: { state: { artGallery } } } as any);
    component.artGallery = artGallery;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('loadArtWorks', () => {
    it('should load artworks successfully', () => {
      const artWorks: ArtWorkDto[] = [{ id: '1', name: 'Art Work 1' } as ArtWorkDto];
      mockGalleryService.getArtGalleryArtWorks.and.returnValue(of(artWorks));

      component.loadArtWorks();

      expect(mockGalleryService.getArtGalleryArtWorks).toHaveBeenCalledWith(
        component.artGallery.id
      );
      expect(component.artWorks).toEqual(artWorks);
    });

    it('should handle error when loading artworks', () => {
      mockGalleryService.getArtGalleryArtWorks.and.returnValue(
        throwError(() => new Error('error'))
      );

      component.loadArtWorks();

      expect(mockGalleryService.getArtGalleryArtWorks).toHaveBeenCalledWith(
        component.artGallery.id
      );
      expect(mockAlertService.error).toHaveBeenCalledWith(
        'Erro ao carregar obras de arte da galeria selecionada'
      );
    });
  });

  describe('formatPrice', () => {
    it('should format price correctly', () => {
      const formattedPrice = component.formatPrice(1234567.89);
      expect(formattedPrice).toBe('1.234.567,890 EUR');
    });

    it('should format integer price', () => {
      const formattedPrice = component.formatPrice(1234567);
      expect(formattedPrice).toBe('1.234.567,000 EUR');
    });
  });

  describe('createArtWorkClick', () => {
    it('should open the dialog to create an artwork', () => {
      component.createArtWorkClick();

      expect(mockDialog.open).toHaveBeenCalledWith(DialogSaveArtWorkComponent, {
        data: { saveMethod: jasmine.any(Function) },
      });
    });
  });

  describe('editArtWorkClick', () => {
    it('should open the dialog to edit an artwork', () => {
      const artWork: ArtWorkDto = { id: '1', name: 'Art Work 1' } as ArtWorkDto;
      mockArtWorkService.getArtWorkById.and.returnValue(of(artWork));

      component.editArtWorkClick('1');

      expect(mockArtWorkService.getArtWorkById).toHaveBeenCalledWith('1');
      expect(mockDialog.open).toHaveBeenCalledWith(DialogSaveArtWorkComponent, {
        data: { artWork, saveMethod: jasmine.any(Function) },
      });
    });
  });

  describe('deleteArtWorkClick', () => {
    it('should delete an artwork successfully', () => {
      mockArtWorkService.deleteArtWork.and.returnValue(of({}));
      spyOn(component, 'loadArtWorks');

      component.deleteArtWorkClick('1');

      expect(mockArtWorkService.deleteArtWork).toHaveBeenCalledWith('1');
      expect(mockAlertService.success).toHaveBeenCalledWith('Removido com sucesso');
      expect(component.loadArtWorks).toHaveBeenCalled();
    });

    it('should handle error when deleting an artwork', () => {
      mockArtWorkService.deleteArtWork.and.returnValue(throwError(() => new Error('error')));

      component.deleteArtWorkClick('1');

      expect(mockArtWorkService.deleteArtWork).toHaveBeenCalledWith('1');
      expect(mockAlertService.success).toHaveBeenCalledWith('Erro ao remover');
    });
  });

  describe('saveArtWork', () => {
    it('should create a new artwork successfully', () => {
      const saveArtWorkDto = {
        id: undefined,
        name: 'New Art Work',
        author: 'Updated Author',
        askPrice: 1000,
        creationYear: 2021,
      } as SaveArtWorkDto;

      mockArtWorkService.createArtWork.and.returnValue(of({}));
      spyOn(component, 'loadArtWorks');

      component.saveArtWork(saveArtWorkDto);

      expect(mockArtWorkService.createArtWork).toHaveBeenCalledWith(
        component.artGallery.id,
        saveArtWorkDto
      );
      expect(mockAlertService.success).toHaveBeenCalledWith('Guardado com sucesso');
      expect(component.loadArtWorks).toHaveBeenCalled();
    });

    it('should update an existing artwork successfully', () => {
      const saveArtWorkDto = {
        id: '1',
        name: 'New Art Work',
        author: 'Updated Author',
        askPrice: 1000,
        creationYear: 2021,
      } as SaveArtWorkDto;

      mockArtWorkService.updateArtWork.and.returnValue(of({}));
      spyOn(component, 'loadArtWorks');

      component.saveArtWork(saveArtWorkDto);

      expect(mockArtWorkService.updateArtWork).toHaveBeenCalledWith(
        component.artGallery.id,
        saveArtWorkDto
      );
      expect(mockAlertService.success).toHaveBeenCalledWith('Guardado com sucesso');
      expect(component.loadArtWorks).toHaveBeenCalled();
    });

    it('should handle error when creating a new artwork', () => {
      const saveArtWorkDto = {
        id: undefined,
        name: 'New Art Work',
        author: 'Updated Author',
        askPrice: 1000,
        creationYear: 2021,
      } as SaveArtWorkDto;

      mockArtWorkService.createArtWork.and.returnValue(throwError(() => new Error('error')));

      component.saveArtWork(saveArtWorkDto);

      expect(mockArtWorkService.createArtWork).toHaveBeenCalledWith(
        component.artGallery.id,
        saveArtWorkDto
      );
      expect(mockAlertService.success).toHaveBeenCalledWith('Erro ao gravar');
    });

    it('should handle error when updating an existing artwork', () => {
      const saveArtWorkDto = {
        id: '1',
        name: 'New Art Work',
        author: 'Updated Author',
        askPrice: 1000,
        creationYear: 2021,
      } as SaveArtWorkDto;

      mockArtWorkService.updateArtWork.and.returnValue(throwError(() => new Error('error')));

      component.saveArtWork(saveArtWorkDto);

      expect(mockArtWorkService.updateArtWork).toHaveBeenCalledWith(
        component.artGallery.id,
        saveArtWorkDto
      );
      expect(mockAlertService.success).toHaveBeenCalledWith('Erro ao gravar');
    });
  });

  describe('backToGalleryList', () => {
    it('should navigate back to the gallery list', () => {
      component.backToGalleryList();

      expect(mockRouter.navigate).toHaveBeenCalledWith(['art-galleries']);
    });
  });
});
