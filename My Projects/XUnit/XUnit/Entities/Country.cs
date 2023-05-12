namespace Entities
{
    /// <summary>
    /// Domain Model for storing the country details
    /// </summary>
    public class Country
    {
        //Guid the values are infinity
        public  Guid CountyId { get; set; }
        public string? CountryName { get; set; }
    }
}