import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ArtWorkService } from './art-work.service';
import { ArtWorkDto } from '../dtos/art-work/art-work.dto';
import { SaveArtWorkDto } from '../dtos/art-work/save-art-work.dto';

describe('ArtWorkService', () => {
  let service: ArtWorkService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ArtWorkService],
    });
    service = TestBed.inject(ArtWorkService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get art work by id', () => {
    const artWorkId = '123';
    const mockArtWork: ArtWorkDto = {
      id: '123',
      name: 'Art Work 1',
      author: 'Author 1',
      creationYear: 2021,
      askPrice: 10000000,
    };
    service.getArtWorkById(artWorkId).subscribe((artWork) => {
      expect(artWork).toEqual(mockArtWork);
    });

    const req = httpMock.expectOne(`${service['baseUrl']}/getartworkbyid?artWorkId=${artWorkId}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockArtWork);
  });

  it('should create art work', () => {
    const artGalleryId = '456';
    const saveArtWorkDto: SaveArtWorkDto = {
      name: 'New Art Work',
      author: 'New Author',
      creationYear: 2021,
      askPrice: 10000000,
    };
    const mockArtWork: ArtWorkDto = {
      id: '789',
      name: 'New Art Work',
      author: 'New Author',
      creationYear: 2021,
      askPrice: 10000000,
    };

    service.createArtWork(artGalleryId, saveArtWorkDto).subscribe((artWork) => {
      expect(artWork).toEqual(mockArtWork);
    });

    const req = httpMock.expectOne(`${service['baseUrl']}?artGalleryId=${artGalleryId}`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(saveArtWorkDto);
    req.flush(mockArtWork);
  });

  it('should update art work', () => {
    const artGalleryId = '456';
    const saveArtWorkDto: SaveArtWorkDto = {
      id: '789',
      name: 'Updated Art Work',
      author: 'Updated Author',
      creationYear: 2021,
      askPrice: 10000000,
    };
    const mockArtWork: ArtWorkDto = {
      id: '789',
      name: 'Updated Art Work',
      author: 'Updated Author',
      creationYear: 2021,
      askPrice: 10000000,
    };

    service.updateArtWork(artGalleryId, saveArtWorkDto).subscribe((artWork) => {
      expect(artWork).toEqual(mockArtWork);
    });

    const req = httpMock.expectOne(`${service['baseUrl']}?artGalleryId=${artGalleryId}`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(saveArtWorkDto);
    req.flush(mockArtWork);
  });

  it('should delete art work', () => {
    const artWorkId = '123';

    service.deleteArtWork(artWorkId).subscribe((result) => {
      expect(result).toBeTrue();
    });

    const req = httpMock.expectOne(`${service['baseUrl']}?artWorkId=${artWorkId}`);
    expect(req.request.method).toBe('DELETE');
    req.flush(true);
  });
});
