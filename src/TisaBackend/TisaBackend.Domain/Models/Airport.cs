namespace TisaBackend.Domain.Models
{
    public class Airport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AlphaCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}