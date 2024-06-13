namespace VAArtGalleryWebAPI.WebApi.Models
{
    public class SaveArtGalleryResult(Guid id, string name, string city, string manager)
    {
        public Guid Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string City { get; set; } = city;
        public string Manager { get; set; } = manager;
    }
}