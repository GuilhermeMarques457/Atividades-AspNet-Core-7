using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service;
using ServiceContracts.DTO;
using ServiceContracts;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly TradingOptions _tradingOptions;
        private readonly IStockService _stockService;

        public TradeController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, IStockService stockService)
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
            Dictionary<string, object>? responseDictionary = await _finnhubService.GetStockPriceQuote(_tradingOptions.DefaultStockSymbol);
            Dictionary<string, object>? responseCompany = await _finnhubService.GetCompanyProfile(_tradingOptions.DefaultStockSymbol);


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
        public async Task<IActionResult> BuyOrder(OrderRequest Order)
        {
            Order.DateAndTimeOfOrder = DateTime.Now;
            Order.OrderType = "BuyOrder";

            if (ModelState.IsValid)
            {
                OrderResponse OrderResponse = await _stockService.CreateOrder(Order);

                return RedirectToAction("SeeOrders");

            }

            return RedirectToAction("Index");


        }

        [HttpPost]
        [Route("Trade/SellOrder")]
        public async Task<IActionResult> SellOrder(OrderRequest Order)
        {
            Order.DateAndTimeOfOrder = DateTime.Now;
            Order.OrderType = "SellOrder";

            if (ModelState.IsValid)
            {
                OrderResponse OrderResponse = await _stockService.CreateOrder(Order);

                return RedirectToAction("SeeOrders");

            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Trade/SeeOrders")]
        public async Task<IActionResult> SeeOrders()
        {
            List<OrderResponse> OrderResponses = await _stockService.GetOrders();
            ViewBag.BuyOrders = OrderResponses.Where(b => b.OrderType == "BuyOrder");
            ViewBag.SellOrders = OrderResponses.Where(b => b.OrderType == "SellOrder");

            
            return View();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> TradePDF()
        {
            List<OrderResponse> OrderResponses = await _stockService.GetOrders();

            return new ViewAsPdf("TradePDF", OrderResponses, ViewData)
            {
                PageMargins = new Margins()
                {
                    Top = 30,
                    Left = 20,
                    Right = 20,
                    Bottom = 30,
                },
                PageOrientation = Orientation.Landscape,

            };

            //return View(OrderResponses);

        }
    }
}
