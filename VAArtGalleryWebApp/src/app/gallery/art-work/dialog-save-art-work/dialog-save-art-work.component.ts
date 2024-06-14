import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ArtWorkDto } from '../../dtos/art-work/art-work.dto';
import { SaveArtWorkDto } from '../../dtos/art-work/save-art-work.dto';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-dialog-save-art-work',
  templateUrl: './dialog-save-art-work.component.html',
  styleUrl: './dialog-save-art-work.component.css',
})
export class DialogSaveArtWorkComponent {
  formArtWorkGroup: FormGroup;

  constructor(
    @Inject(MAT_DIALOG_DATA) public obj: any,
    private _dialogRef: MatDialogRef<DialogSaveArtWorkComponent>,
    private _formBuilder: FormBuilder
  ) {
    this.formArtWorkGroup = this._formBuilder.group({
      name: ['', Validators.required],
      author: ['', Validators.required],
      creationYear: ['', Validators.required],
      askPrice: ['', Validators.required],
    });

    if (obj && obj.artWork) {
      this.setFormValues(obj.artWork);
    }
  }

  setFormValues(artWork: ArtWorkDto) {
    this.formArtWorkGroup.setValue({
      name: artWork.name,
      author: artWork.author,
      creationYear: artWork.creationYear,
      askPrice: artWork.askPrice,
    });
  }

  saveClick() {
    if (this.formArtWorkGroup.valid) {
      let saveArtGalleryDto: SaveArtWorkDto = {
        id: this.obj?.artWork ? this.obj.artWork.id : null,
        name: this.formArtWorkGroup.value.name,
        author: this.formArtWorkGroup.value.author,
        creationYear: this.formArtWorkGroup.value.creationYear,
        askPrice: this.formArtWorkGroup.value.askPrice,
      };

      if (this.obj?.saveMethod) {
        this.obj.saveMethod(saveArtGalleryDto);
      }

      this._dialogRef.close(this.formArtWorkGroup.value);
    } else {
      this.formArtWorkGroup.markAllAsTouched();
    }
  }

  backClick() {
    this._dialogRef.close();
  }
}
