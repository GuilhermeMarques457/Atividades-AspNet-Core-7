using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service;
using ServiceContracts.DTO;
using ServiceContracts;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using StocksApp.Models;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly TradingOptions _tradingOptions;
        private readonly IStockService _stockService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TradeController> _logger;

        public TradeController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, IStockService stockService, IConfiguration configuration, ILogger<TradeController> logger)
        {
            _stockService = stockService;
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions.Value;
            _configuration = configuration;
            _logger = logger;   
        }

        [Route("[action]")]
        public async Task<IActionResult> Index(string? stockToTrade)
        {
            _logger.LogError("Index action reached");

            ViewBag.css = "trade.css";

            if (stockToTrade == null)
            {
                stockToTrade = "MSFT";
            }
            Dictionary<string, object>? responseDictionary = await _finnhubService.GetStockPriceQuote(stockToTrade);
            Dictionary<string, object>? responseCompany = await _finnhubService.GetCompanyProfile(stockToTrade);


            StockTrade stockTrade = new StockTrade()
            {
                StockSymbol = _tradingOptions.DefaultStockSymbol,
                StockName = responseCompany!["name"].ToString(),
                Price = Convert.ToDouble(responseDictionary!["c"].ToString()),
                Image = responseCompany["logo"].ToString(),
            };

            ViewBag.StockTrade = stockTrade;

            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> BuyOrder(OrderRequest Order)
        {
            ViewBag.css = "trade.css";

            _logger.LogInformation("Client Buyed stock");

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
        [Route("[action]")]
        public async Task<IActionResult> SellOrder(OrderRequest Order)
        {
            ViewBag.css = "trade.css";

            _logger.LogInformation("Client Selled stock");

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
        [Route("[action]")]
        public async Task<IActionResult> SeeOrders()
        {
            ViewBag.css = "trade.css";

            _logger.LogInformation("Client wants to see orders");

            List<OrderResponse> OrderResponses = await _stockService.GetOrders();
            ViewBag.BuyOrders = OrderResponses.Where(b => b.OrderType == "BuyOrder");
            ViewBag.SellOrders = OrderResponses.Where(b => b.OrderType == "SellOrder");

            
            return View();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Explore(string? searchStock, string? clickedStock)
        {
            ViewBag.css = "trade.css";

            _logger.LogInformation("Client wants to explore stocks");

            List<StockTrade> stockTrades = new List<StockTrade>();

            _tradingOptions.StockSymbols = _configuration.GetSection("TradingOptions:Top25PopularStocks")!.Value;

            List<string> stocksList = _tradingOptions.StockSymbols!.Split(",").ToList();
            List<string>? stocksSearched;

            if(searchStock != null && clickedStock != null)
            {
                ViewBag.CurrentSearch = searchStock;

                stocksSearched = stocksList.Where(s => s.Contains(searchStock)).ToList();

                foreach (var stock in stocksSearched)
                {
                    Dictionary<string, object>? responseDictionary = await _finnhubService.GetStockPriceQuote(stock);
                    Dictionary<string, object>? responseCompany = await _finnhubService.GetCompanyProfile(stock);

                    StockTrade stockTrade = new StockTrade()
                    {
                        StockSymbol = stock,
                        StockName = responseCompany!["name"].ToString(),
                        Price = Convert.ToDouble(responseDictionary!["c"].ToString()),
                        Image = responseCompany["logo"].ToString(),
                        Exchange = responseCompany["exchange"].ToString(),
                        Industry = responseCompany["finnhubIndustry"].ToString()

                    };

                    stockTrades.Add(stockTrade);
                }

                ViewBag.ClickedStock = stockTrades.Where(s => s.StockSymbol == clickedStock).FirstOrDefault();
            }
            else if(searchStock != null)
            {
                stocksSearched = stocksList.Where(s => s.Contains(searchStock)).ToList();

                foreach (var stock in stocksSearched)
                {
                    Dictionary<string, object>? responseDictionary = await _finnhubService.GetStockPriceQuote(stock);
                    Dictionary<string, object>? responseCompany = await _finnhubService.GetCompanyProfile(stock);

                    StockTrade stockTrade = new StockTrade()
                    {
                        StockSymbol = stock,
                        StockName = responseCompany!["name"].ToString(),
                        Price = Convert.ToDouble(responseDictionary!["c"].ToString()),
                        Image = responseCompany["logo"].ToString(),
                        Exchange = responseCompany["exchange"].ToString(),
                        Industry = responseCompany["finnhubIndustry"].ToString()

                    };

                    stockTrades.Add(stockTrade);
                }

                ViewBag.CurrentSearch = searchStock;

            }
            else if(clickedStock != null)
            {
                ViewBag.clickedStock = clickedStock;

                stockTrades.Clear();

                Dictionary<string, object>? responseDictionary = await _finnhubService.GetStockPriceQuote(clickedStock);
                Dictionary<string, object>? responseCompany = await _finnhubService.GetCompanyProfile(clickedStock);

                StockTrade stockTrade = new StockTrade()
                {
                    StockSymbol = clickedStock,
                    StockName = responseCompany!["name"].ToString(),
                    Price = Convert.ToDouble(responseDictionary!["c"].ToString()),
                    Image = responseCompany["logo"].ToString(),
                    Exchange = responseCompany["exchange"].ToString(),
                    Industry = responseCompany["finnhubIndustry"].ToString()

                };

                ViewBag.ClickedStock = stockTrade;

                stockTrades.Add(stockTrade);
            }
            else
            {
                foreach (var stock in stocksList)
                {
                    Dictionary<string, object>? responseDictionary = await _finnhubService.GetStockPriceQuote(stock);
                    Dictionary<string, object>? responseCompany = await _finnhubService.GetCompanyProfile(stock);

                    StockTrade stockTrade = new StockTrade()
                    {
                        StockSymbol = stock,
                        StockName = responseCompany!["name"].ToString(),
                        Price = Convert.ToDouble(responseDictionary!["c"].ToString()),
                        Image = responseCompany["logo"].ToString(),
                        Exchange = responseCompany["exchange"].ToString(),
                        Industry = responseCompany["finnhubIndustry"].ToString()

                    };
                    stockTrades.Add(stockTrade);
                }
            }
           

            return View(stockTrades);

        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> TradePDF()
        {
            List<OrderResponse> OrderResponses = await _stockService.GetOrders();
            List<OrderResponse> OrderByDate = OrderResponses.OrderByDescending(o => o.DateAndTimeOfOrder).ToList();


            return new ViewAsPdf("TradePDF", OrderByDate, ViewData)
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
