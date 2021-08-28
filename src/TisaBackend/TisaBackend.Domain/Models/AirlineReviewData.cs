using System.Linq;
using System.Collections.Generic;

namespace TisaBackend.Domain.Models
{
    //TODO: Add to design document
    public class AirlineReviewData
    {
        public int AirlineId { get; set; }
        public string AirlineName { get; set; }
        public IList<Review> Reviews { get; set; }
        public double? Ranking => Reviews?.Average(data => data?.Ranking);
    }
}