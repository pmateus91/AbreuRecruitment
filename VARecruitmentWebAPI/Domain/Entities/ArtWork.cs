namespace VAArtGalleryWebAPI.Domain.Entities
{
    public class ArtWork(string name, string author, int creationYear, decimal askPrice)
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Invalid art work name", nameof(name)) : name;

        public string Author { get; set; } = string.IsNullOrWhiteSpace(author) ? throw new ArgumentException("Invalid author name", nameof(author)) : author;

        public int CreationYear { get; set; } = ValidateCreationYear(creationYear) ? throw new ArgumentException("Invalid work date", nameof(creationYear)) : creationYear;

        public decimal AskPrice { get; set; } = ValidateAskPrice(askPrice) ? throw new ArgumentException("Invalid ask price", nameof(askPrice)) : askPrice;

        public ArtWork AdjustAskPrice(decimal newAmount)
        {
            if (!ValidateAskPrice(newAmount))
            {
                throw new ArgumentException("Ask price must be a positive number");
            }

            AskPrice = newAmount;
            return this;
        }

        public ArtWork Update(string name, string author, int creationYear, decimal askPrice)
        {
            Name = string.IsNullOrWhiteSpace(name) ? Name : name;
            Author = string.IsNullOrWhiteSpace(author) ? Author : author;
            CreationYear = ValidateCreationYear(creationYear) ? CreationYear : creationYear;
            AskPrice = ValidateAskPrice(askPrice) ? AskPrice : askPrice;

            return this;
        }

        private static bool ValidateCreationYear(decimal year) => year > DateTime.Now.Year;

        private static bool ValidateAskPrice(decimal amount) => amount <= 0;
    }
}