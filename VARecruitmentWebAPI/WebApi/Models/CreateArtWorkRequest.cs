namespace VAArtGalleryWebAPI.WebApi.Models
{
    public class CreateArtWorkRequest(string name, string author, int creationYear, decimal askPrice)
    {
        public string Name { get; set; } = name;
        public string Author { get; set; } = author;
        public int CreationYear { get; set; } = creationYear;
        public decimal AskPrice { get; set; } = askPrice;
    }
}