import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ArtGalleryDto } from '../dtos/art-gallery/art-gallery.dto';
import { SaveArtGalleryDto } from '../dtos/art-gallery/save-art-gallery.dto';
import { GalleryService } from './art-gallery.service';
import { ArtWorkDto } from '../dtos/art-work/art-work.dto';
import { GalleryComponent } from '../art-gallery/gallery.component';

describe('GalleryService', () => {
  let service: GalleryService;
  let httpMock: HttpTestingController;

  const baseUrl = 'https://localhost:7042/api/art-galleries';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [GalleryComponent],
      providers: [GalleryService],
    });
    service = TestBed.inject(GalleryService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch all galleries', () => {
    const mockGalleries: ArtGalleryDto[] = [
      {
        id: '1',
        name: 'Gallery 1',
        city: 'City 1',
        manager: 'Manager 1',
        nbrOfArtWorksOnDisplay: 10,
      },
      {
        id: '2',
        name: 'Gallery 2',
        city: 'City 2',
        manager: 'Manager 2',
        nbrOfArtWorksOnDisplay: 20,
      },
    ];

    service.getGalleries().subscribe((galleries: any) => {
      expect(galleries).toEqual(mockGalleries);
    });

    const req = httpMock.expectOne(`${baseUrl}/GetAllGalleries`);
    expect(req.request.method).toBe('GET');
    req.flush(mockGalleries);
  });

  it('should fetch artworks for a given gallery', () => {
    const mockArtWorks: ArtWorkDto[] = [
      { id: '1', name: 'ArtWork 1', author: 'Author 1', creationYear: 2020, askPrice: 1000 },
      { id: '2', name: 'ArtWork 2', author: 'Author 2', creationYear: 2021, askPrice: 2000 },
    ];
    const galleryId = '1';

    service.getArtGalleryArtWorks(galleryId).subscribe((artWorks: any) => {
      expect(artWorks).toEqual(mockArtWorks);
    });

    const req = httpMock.expectOne(`${baseUrl}/GetArtGalleryArtWorks?artGalleryId=${galleryId}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockArtWorks);
  });

  it('should fetch a gallery by id', () => {
    const mockGallery: ArtGalleryDto = {
      id: '1',
      name: 'Gallery 1',
      city: 'City 1',
      manager: 'Manager 1',
      nbrOfArtWorksOnDisplay: 10,
    };
    const galleryId = '1';

    service.getArtGalleryById(galleryId).subscribe((gallery: any) => {
      expect(gallery).toEqual(mockGallery);
    });

    const req = httpMock.expectOne(`${baseUrl}/GetArtGalleryById?artGalleryId=${galleryId}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockGallery);
  });

  it('should create a gallery work', () => {
    const saveArtGalleryDto: SaveArtGalleryDto = {
      name: 'New Gallery',
      city: 'New City',
      manager: 'New Manager',
    };
    const mockGallery: ArtGalleryDto = {
      id: '1',
      name: 'New Gallery',
      city: 'New City',
      manager: 'New Manager',
      nbrOfArtWorksOnDisplay: 0,
    };

    service.createArtGallery(saveArtGalleryDto).subscribe((gallery: any) => {
      expect(gallery).toEqual(mockGallery);
    });

    const req = httpMock.expectOne(`${baseUrl}`);
    expect(req.request.method).toBe('POST');
    req.flush(mockGallery);
  });

  it('should update a gallery work', () => {
    const saveArtGalleryDto: SaveArtGalleryDto = {
      id: '1',
      name: 'Updated Gallery',
      city: 'Updated City',
      manager: 'Updated Manager',
    };
    const mockGallery: ArtGalleryDto = {
      id: '1',
      name: 'Updated Gallery',
      city: 'Updated City',
      manager: 'Updated Manager',
      nbrOfArtWorksOnDisplay: 0,
    };

    service.updateArtGallery(saveArtGalleryDto).subscribe((gallery: any) => {
      expect(gallery).toEqual(mockGallery);
    });

    const req = httpMock.expectOne(`${baseUrl}`);
    expect(req.request.method).toBe('PUT');
    req.flush(mockGallery);
  });

  it('should delete a gallery work', () => {
    const galleryId = '1';

    service.deleteArtGallery(galleryId).subscribe((result: any) => {
      expect(result).toBeTrue();
    });

    const req = httpMock.expectOne(`${baseUrl}?artGalleryId=${galleryId}`);
    expect(req.request.method).toBe('DELETE');
    req.flush(true);
  });
});
