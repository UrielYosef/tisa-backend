using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TisaBackend.Domain.Models
{
    public class Airline
    {
        [Key]
        public string Name { get; set; }

        public IList<Airplane> Airplanes { get; set; }
    }
}