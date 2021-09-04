using System;
using System.Linq;
using System.Collections.Generic;

namespace TisaBackend.Domain.Models
{
    public class AirlineReviewData
    {
        public int AirlineId { get; set; }
        public string AirlineName { get; set; }
        public IList<Review> Reviews { get; set; }
        public decimal Ranking => CalculateAverageRanking();

        private decimal CalculateAverageRanking()
        {
            if (!Reviews?.Any() ?? true)
                return 0;

            return Math.Round((decimal)Reviews.Average(data => data?.Ranking), 1);
        }
    }
}