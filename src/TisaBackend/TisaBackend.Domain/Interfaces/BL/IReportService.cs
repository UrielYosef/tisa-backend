﻿using System.Threading.Tasks;
using System.Collections.Generic;
using TisaBackend.Domain.Models;

namespace TisaBackend.Domain.Interfaces.BL
{
    public interface IReportService
    {
        Task<IList<UserReportData>> GetRegisteredUsersAsync();
        Task<IList<AirlineReportData>> GetAirlinesReportsDataAsync(string username, bool isAdmin);
        Task<AirlineReportData> GetAirlineReportDataAsync(int airlineId, string username, bool isAdmin);
        Task<AirlineOrdersReportData> GetAirlineOrdersReportDataAsync(int airlineId, string username, bool isAdmin);
    }
}