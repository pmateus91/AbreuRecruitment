import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ArtGalleryDto } from '../../dtos/art-gallery/art-gallery.dto';
import { SaveArtGalleryDto } from '../../dtos/art-gallery/save-art-gallery.dto';

@Component({
  selector: 'app-dialog-save-art-gallery',
  templateUrl: './dialog-save-art-gallery.component.html',
  styleUrl: './dialog-save-art-gallery.component.css',
})
export class DialogSaveArtGalleryComponent {
  formArtGalleryGroup: FormGroup;

  constructor(
    @Inject(MAT_DIALOG_DATA) public obj: any,
    private _dialogRef: MatDialogRef<DialogSaveArtGalleryComponent>,
    private _formBuilder: FormBuilder
  ) {
    this.formArtGalleryGroup = this._formBuilder.group({
      name: ['', Validators.required],
      city: ['', Validators.required],
      manager: ['', Validators.required],
    });

    if (obj && obj.artGallery) {
      this.setFormValues(obj.artGallery);
    }
  }

  setFormValues(artGallery: ArtGalleryDto) {
    this.formArtGalleryGroup.setValue({
      name: artGallery.name,
      city: artGallery.city,
      manager: artGallery.manager,
    });
  }

  saveClick() {
    if (this.formArtGalleryGroup.valid) {
      let saveArtGalleryDto: SaveArtGalleryDto = {
        id: this.obj?.artGallery ? this.obj.artGallery.id : null,
        name: this.formArtGalleryGroup.value.name,
        city: this.formArtGalleryGroup.value.city,
        manager: this.formArtGalleryGroup.value.manager,
      };

      if (this.obj?.saveMethod) {
        this.obj.saveMethod(saveArtGalleryDto);
      }

      this._dialogRef.close(this.formArtGalleryGroup.value);
    } else {
      this.formArtGalleryGroup.markAllAsTouched();
    }
  }

  backClick() {
    this._dialogRef.close();
  }
}
