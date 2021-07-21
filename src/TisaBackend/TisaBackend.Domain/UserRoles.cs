namespace TisaBackend.Domain
{
    public class UserRoles
    {
        public const string Admin = "Admin";
        public const string AirlineManager = "AirlineManager";
        public const string AirlineAgent = "AirlineAgent";
        public const string Client = "Client";

        public const string AdminAndAirlineManager = Admin + "," + AirlineManager;
        public const string AdminAndAirlineManagerAndAirlineAgent = Admin + "," + AirlineManager + "," + AirlineAgent;
        public const string AllRoles = Admin + "," + AirlineManager + "," + AirlineAgent + "," + Client;
    }
}