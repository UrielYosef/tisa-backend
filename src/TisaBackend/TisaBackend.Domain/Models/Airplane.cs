namespace TisaBackend.Domain.Models
{
    public class Airplane
    {
        public int Id { get; set; }
        public int AirplaneTypeId { get; set; }
        public int AirlineId { get; set; }

        public AirplaneType AirplaneType { get; set; }
        public Airline Airline { get; set; }
    }
}