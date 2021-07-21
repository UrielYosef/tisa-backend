using System.ComponentModel.DataAnnotations;

namespace TisaBackend.Domain.Models
{
    public class FlightPrice
    {
        public int PriceInDollars { get; set; }
        public int FlightId { get; set; }
        public string DepartmentType { get; set; }

        public Flight Flight { get; set; }
    }
}