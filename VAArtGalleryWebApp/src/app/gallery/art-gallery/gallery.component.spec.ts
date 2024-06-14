import { ComponentFixture, TestBed } from '@angular/core/testing';
import { GalleryComponent } from './gallery.component';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { GalleryService } from '../services/art-gallery.service';
import { AlertService } from '../../shared-components/alert.service';
import { of, throwError } from 'rxjs';
import { ArtGalleryDto } from '../dtos/art-gallery/art-gallery.dto';
import { DialogSaveArtGalleryComponent } from './dialog-save-art-gallery/dialog-save-art-gallery.component';
import { SaveArtGalleryDto } from '../dtos/art-gallery/save-art-gallery.dto';
import { MatIconModule } from '@angular/material/icon';

describe('GalleryComponent', () => {
  let component: GalleryComponent;
  let fixture: ComponentFixture<GalleryComponent>;
  let mockRouter = jasmine.createSpyObj('Router', ['navigate']);
  let mockDialog = jasmine.createSpyObj('MatDialog', ['open']);
  let mockGalleryService = jasmine.createSpyObj('GalleryService', [
    'getGalleries',
    'getArtGalleryById',
    'deleteArtGallery',
    'createArtGallery',
    'updateArtGallery',
  ]);
  let mockAlertService = jasmine.createSpyObj('AlertService', ['error', 'success']);

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MatIconModule],
      declarations: [GalleryComponent],
      providers: [
        { provide: Router, useValue: mockRouter },
        { provide: MatDialog, useValue: mockDialog },
        { provide: GalleryService, useValue: mockGalleryService },
        { provide: AlertService, useValue: mockAlertService },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(GalleryComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  describe('ngOnInit', () => {
    it('should load galleries on init', () => {
      spyOn(component, 'loadArtGalleries');
      component.ngOnInit();
      expect(component.loadArtGalleries).toHaveBeenCalled();
    });
  });

  describe('loadArtGalleries', () => {
    it('should load galleries successfully', () => {
      const galleries: ArtGalleryDto[] = [{ id: '1', name: 'Gallery 1' } as ArtGalleryDto];
      mockGalleryService.getGalleries.and.returnValue(of(galleries));

      component.loadArtGalleries();

      expect(mockGalleryService.getGalleries).toHaveBeenCalled();
      expect(component.galleries).toEqual(galleries);
    });

    it('should handle error when loading galleries', () => {
      mockGalleryService.getGalleries.and.returnValue(throwError(() => new Error('error')));

      component.loadArtGalleries();

      expect(mockGalleryService.getGalleries).toHaveBeenCalled();
      expect(mockAlertService.error).toHaveBeenCalledWith('Erro ao carregar as galerias de artes');
    });
  });

  describe('createArtGalleryClick', () => {
    it('should open the dialog to create a gallery', () => {
      component.createArtGalleryClick();

      expect(mockDialog.open).toHaveBeenCalledWith(DialogSaveArtGalleryComponent, {
        data: { saveMethod: jasmine.any(Function) },
      });
    });
  });

  describe('editArtGalleryClick', () => {
    it('should open the dialog to edit a gallery', () => {
      const artGallery: ArtGalleryDto = { id: '1', name: 'Gallery 1' } as ArtGalleryDto;
      mockGalleryService.getArtGalleryById.and.returnValue(of(artGallery));

      component.editArtGalleryClick('1');

      expect(mockGalleryService.getArtGalleryById).toHaveBeenCalledWith('1');
      expect(mockDialog.open).toHaveBeenCalledWith(DialogSaveArtGalleryComponent, {
        data: { artGallery, saveMethod: jasmine.any(Function) },
      });
    });
  });

  describe('deleteArtGalleryClick', () => {
    it('should delete a gallery successfully', () => {
      mockGalleryService.deleteArtGallery.and.returnValue(of({}));
      spyOn(component, 'loadArtGalleries');

      component.deleteArtGalleryClick('1');

      expect(mockGalleryService.deleteArtGallery).toHaveBeenCalledWith('1');
      expect(mockAlertService.success).toHaveBeenCalledWith('Removido com sucesso');
      expect(component.loadArtGalleries).toHaveBeenCalled();
    });

    it('should handle error when deleting a gallery', () => {
      mockGalleryService.deleteArtGallery.and.returnValue(throwError(() => new Error('error')));

      component.deleteArtGalleryClick('1');

      expect(mockGalleryService.deleteArtGallery).toHaveBeenCalledWith('1');
      expect(mockAlertService.success).toHaveBeenCalledWith('Erro ao remover');
    });
  });

  describe('saveArtGallery', () => {
    it('should create a new gallery successfully', () => {
      const saveArtGalleryDto: SaveArtGalleryDto = {
        id: undefined,
        name: 'New Gallery',
        city: 'City',
        manager: 'Manager',
      };
      mockGalleryService.createArtGallery.and.returnValue(of({}));
      spyOn(component, 'loadArtGalleries');

      component.saveArtGallery(saveArtGalleryDto);

      expect(mockGalleryService.createArtGallery).toHaveBeenCalledWith(saveArtGalleryDto);
      expect(mockAlertService.success).toHaveBeenCalledWith('Guardado com sucesso');
      expect(component.loadArtGalleries).toHaveBeenCalled();
    });

    it('should update an existing gallery successfully', () => {
      const saveArtGalleryDto: SaveArtGalleryDto = {
        id: '1',
        name: 'Updated Gallery',
        city: 'City',
        manager: 'Manager',
      };
      mockGalleryService.updateArtGallery.and.returnValue(of({}));
      spyOn(component, 'loadArtGalleries');

      component.saveArtGallery(saveArtGalleryDto);

      expect(mockGalleryService.updateArtGallery).toHaveBeenCalledWith(saveArtGalleryDto);
      expect(mockAlertService.success).toHaveBeenCalledWith('Guardado com sucesso');
      expect(component.loadArtGalleries).toHaveBeenCalled();
    });

    it('should handle error when creating a new gallery', () => {
      const saveArtGalleryDto: SaveArtGalleryDto = {
        id: undefined,
        name: 'New Gallery',
        city: 'City',
        manager: 'Manager',
      };
      mockGalleryService.createArtGallery.and.returnValue(throwError(() => new Error('error')));

      component.saveArtGallery(saveArtGalleryDto);

      expect(mockGalleryService.createArtGallery).toHaveBeenCalledWith(saveArtGalleryDto);
      expect(mockAlertService.success).toHaveBeenCalledWith('Erro ao gravar');
    });

    it('should handle error when updating an existing gallery', () => {
      const saveArtGalleryDto: SaveArtGalleryDto = {
        id: '1',
        name: 'Updated Gallery',
        city: 'City',
        manager: 'Manager',
      };
      mockGalleryService.updateArtGallery.and.returnValue(throwError(() => new Error('error')));

      component.saveArtGallery(saveArtGalleryDto);

      expect(mockGalleryService.updateArtGallery).toHaveBeenCalledWith(saveArtGalleryDto);
      expect(mockAlertService.success).toHaveBeenCalledWith('Erro ao gravar');
    });
  });

  describe('openArtWorksList', () => {
    it('should navigate to the artworks list', () => {
      const artGallery: ArtGalleryDto = { id: '1', name: 'Gallery 1' } as ArtGalleryDto;

      component.openArtWorksList(artGallery);

      expect(mockRouter.navigate).toHaveBeenCalledWith(
        [`art-galleries/${artGallery.id}/art-works`],
        {
          state: { artGallery },
        }
      );
    });
  });
});
