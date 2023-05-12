using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulting country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Add a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRequest"></param>
        /// <returns>Retuns the country object after adding it</returns>
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Returns all countries as a list
        /// </summary>
        /// <returns>Return all countries as a list</returns>
        List<CountryResponse> GetAllCountries();

        /// <summary>
        /// Returns a country object based on the given id
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns>Country object as countryResponse object</returns>
        CountryResponse? GetCountryById(Guid? countryID);
    }
}