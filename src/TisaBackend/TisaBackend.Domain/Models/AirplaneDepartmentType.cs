using System.ComponentModel.DataAnnotations;

namespace TisaBackend.Domain.Models
{
    public class AirplaneDepartmentType
    {
        [Key]
        public string Name { get; set; }
    }
}