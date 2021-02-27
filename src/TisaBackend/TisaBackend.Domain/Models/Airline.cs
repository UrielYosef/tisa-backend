using System.Collections.Generic;

namespace TisaBackend.Domain.Models
{
    public class Airline
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IList<Airplane> Airplanes { get; set; }
    }
}