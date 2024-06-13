namespace VAArtGalleryWebAPI.WebApi.Models
{
    public class GetArtGalleryResult(Guid id, string name, string city, string manager, int nbrOfArtWorksOnDisplay)
    {
        public Guid Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string City { get; set; } = city;
        public string Manager { get; set; } = manager;
        public int NbrOfArtWorksOnDisplay { get; set; } = nbrOfArtWorksOnDisplay;
    }
}