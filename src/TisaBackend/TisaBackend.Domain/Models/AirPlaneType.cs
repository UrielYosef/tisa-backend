using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TisaBackend.Domain.Models
{
    public class AirplaneType
    {
        [Key]
        public string Type { get; set; }

        public IList<AirplaneDepartmentSeats> AirplaneDepartmentSeats { get; set; }
    }
}