export class SaveArtWorkDto {
  id?: string;
  name: string;
  author: string;
  creationYear: number;
  askPrice: number;

  constructor() {
    this.name = '';
    this.author = '';
    this.creationYear = 0;
    this.askPrice = 0;
  }
}
