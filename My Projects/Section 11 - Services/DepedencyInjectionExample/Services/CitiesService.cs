using ServiceContracts;

namespace Services
{
  public class CitiesService : ICitiesService, IDisposable
  {
        private List<string> _cities;

        public Guid _serviceInstanceId { get; }

        public Guid ServiceInstanceId
        {
            get 
            {
                return _serviceInstanceId; 
            }
        }

        public CitiesService()
        {
            _serviceInstanceId = Guid.NewGuid();    
            _cities = new List<string>() { 
                "London",
                "Paris",
                "New York",
                "Tokyo",
                "Rome"
            };

            //add some logic to open db connection
        }

        public List<string> GetCities()
        {
          return _cities;
        }

        public void Dispose()
        {
            //add logic to cloe db connection
        }
    }
}
