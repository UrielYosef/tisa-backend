namespace TisaBackend.Domain.Models
{
    public class DalDepartmentPrice
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public int DepartmentId { get; set; }
        public int Price { get; set; }

        public Flight Flight { get; set; }
        public DepartmentType Department { get; set; }

        public DalDepartmentPrice()
        {
            
        }

        public DalDepartmentPrice(int flightId, int departmentId, int price)
        {
            FlightId = flightId;
            DepartmentId = departmentId;
            Price = price;
        }
    }
}