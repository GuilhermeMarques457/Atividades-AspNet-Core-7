using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockApp.Services;
using StocksApp.Models;

namespace StocksApp.Controllers
{
    public class TradeController : Controller
    {
        private readonly FinnhubService _finnhubService;
        private readonly TradingOptions _tradingOptions;

        public TradeController(FinnhubService finnhubService, IOptions<TradingOptions> tradingOptions)
        {
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions.Value;
        }

        [Route("Trade/Index")]
        public async Task<IActionResult> Index()
        {
            if (_tradingOptions.DefaultStockSymbol == null)
            {
                _tradingOptions.DefaultStockSymbol = "MSFT";
            }
            Dictionary<string, object> responseDictionary = await _finnhubService.GetStockPriceQuote(_tradingOptions.DefaultStockSymbol);
            Dictionary<string, object> responseCompany = await _finnhubService.GetCompanyProfile(_tradingOptions.DefaultStockSymbol);


            StockTrade stockTrade = new StockTrade()
            {
                StockSymbol = _tradingOptions.DefaultStockSymbol,
                StockName = responseCompany["name"].ToString(),
                Price = Convert.ToDouble(responseDictionary["c"].ToString()),
            };

            return View(stockTrade);
        }
    }
}
