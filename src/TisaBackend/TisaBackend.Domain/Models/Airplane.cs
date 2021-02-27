namespace TisaBackend.Domain.Models
{
    public class Airplane
    {
        public int Id { get; set; }
        public string AirplaneTypeId { get; set; }
        public string AirlineId { get; set; }

        public AirplaneType AirplaneType { get; set; }
        public Airline Airline { get; set; }
    }
}