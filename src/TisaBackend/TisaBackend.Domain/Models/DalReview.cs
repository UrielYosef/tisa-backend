using System;
using System.ComponentModel.DataAnnotations;

namespace TisaBackend.Domain.Models
{
    public class DalReview
    {
        public int Id { get; set; }
        public int AirlineId { get; set; }
        public string UserId { get; set; }
        public string Headline { get; set; }
        public string Content { get; set; }
        [Range(1,5)]
        public int Ranking { get; set; }
        public DateTime ReviewDate { get; set; }

        public Airline Airline { get; set; }
    }
}