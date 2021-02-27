namespace TisaBackend.Domain.Models
{
    public class Airplane
    {
        public int Id { get; set; }
        public int AirPlaneTypeId { get; set; }
        public int AirlineId { get; set; }

        public AirPlaneType AirPlaneType { get; set; }
        public Airline Airline { get; set; }
    }
}