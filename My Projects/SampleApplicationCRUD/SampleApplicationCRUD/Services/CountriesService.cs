using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Linq;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly AspNetDbContext _context;

        public CountriesService(AspNetDbContext context)
        {
            _context = context;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            switch (countryAddRequest)
            {
                case null:
                    throw new ArgumentNullException(nameof(countryAddRequest));
            }

            switch (countryAddRequest.CountryName)
            {
                case null:
                    throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            if (await _context.Countries.CountAsync(country => country.CountryName == countryAddRequest.CountryName) > 0)
            {
                throw new ArgumentException("Given country name already exists");
            };

            Country country = countryAddRequest.ToCountry();
         
            country.CountryID = Guid.NewGuid(); 

            await _context.Countries.AddAsync(country);

            await _context.SaveChangesAsync();

            return country.ToCountryResponse();

        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return await _context.Countries.Select(c => c.ToCountryResponse()).ToListAsync();
        }

        public async Task<CountryResponse?> GetCountryById(Guid? countryID)
        {
            if (countryID == null) 
            {
                return null;
            }
            else
            {
                Country? country = await _context.Countries.FirstOrDefaultAsync(c => c.CountryID == countryID);

                if(country == null)
                {
                    return null;
                }

                return country.ToCountryResponse();
            }
            

            
        }

        public async Task<int> UploadFromExcelFile(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();

            await formFile.CopyToAsync(memoryStream);

            int countriesInserted = 0;

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets["Countries"];

                int rowCount = workSheet.Dimension.Rows;

                for(int row = 2 ; row <= rowCount; row++)
                {
                    string? cellValue = workSheet.Cells[row, 1].Value.ToString();

                    if(!string.IsNullOrEmpty(cellValue)) 
                    { 
                        string countryName = cellValue;

                        if(_context.Countries.Where(c => c.CountryName == countryName).Count() == 0)
                        {
                            CountryAddRequest countryAddRequest = new CountryAddRequest()
                            {
                                CountryName = countryName,
                            };

                            await AddCountry(countryAddRequest);
                            await _context.SaveChangesAsync();

                            countriesInserted++;
                        }
                    }
                }
            }

            return countriesInserted;
        }
    }
}