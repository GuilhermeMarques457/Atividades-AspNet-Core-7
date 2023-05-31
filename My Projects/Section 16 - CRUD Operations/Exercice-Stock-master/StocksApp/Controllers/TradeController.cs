using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockApp.Services;
using StocksApp.Models;
using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTO;

namespace StocksApp.Controllers
{
    public class TradeController : Controller
    {
        private readonly FinnhubService _finnhubService;
        private readonly TradingOptions _tradingOptions;
        private readonly IStockService _stockService;

        public TradeController(FinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, IStockService stockService)
        {
            _stockService = stockService;
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

            ViewBag.StockTrade = stockTrade;

            return View();
        }

        [HttpPost]
        [Route("Trade/BuyOrder")]
        public IActionResult BuyOrder(BuyOrderRequest buyOrder)
        {
            buyOrder.DateAndTimeOfOrder = DateTime.Now;

            if (ModelState.IsValid)
            {
                BuyOrderResponse buyOrderResponse = _stockService.CreateBuyOrder(buyOrder);

                return RedirectToAction("SeeOrders");
                
            }
           
            return View(buyOrder);
           
        }

        [HttpPost]
        [Route("Trade/SellOrder")]
        public IActionResult SellOrder(SellOrderRequest sellOrder)
        {
            sellOrder.DateAndTimeOfOrder = DateTime.Now;

            if (ModelState.IsValid)
            {
                SellOrderResponse sellOrderResponse = _stockService.CreateSellOrder(sellOrder);

               

                return RedirectToAction("SeeOrders");

            }
           
            return View(sellOrder);
        }

        [HttpGet]
        [Route("Trade/SeeOrders")]
        public IActionResult SeeOrders()
        {
            List<BuyOrderResponse> buyOrderResponses = _stockService.GetBuyOrders(); 
            List<SellOrderResponse> sellOrderResponses = _stockService.GetSellOrders(); 

            SellBuyVM sellBuyVM = new SellBuyVM() {  BuyOrders= buyOrderResponses, SellOrders = sellOrderResponses };

            return View(sellBuyVM);
        }
    }
}
