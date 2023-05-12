using StocksApp.Models;
using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTO;
using StocksApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace TestStock
{
    public class StockServiceTest
    {
        private readonly ITestOutputHelper _helper;
        private readonly IStockService _stockService;

        public StockServiceTest(ITestOutputHelper testOutputHelper)
        {
            _helper = testOutputHelper;
            _stockService = new StockService();
        }

        #region BuyOrder

        [Fact]
        public void CreateBuyOrder_nullOrder()
        {
            BuyOrderRequest? buyOrderRequest = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                 _stockService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void CreateBuyOrder_MinimumQtde()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("20-04-1998"),
                Price = 123,
                StockName = "Test",
                Quantity = 0,
                StockSymbol = "Test",
            };

            if(buyOrderRequest.Quantity == 0)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    _stockService.CreateBuyOrder(buyOrderRequest);
                });
            }
        }

        [Fact]
        public void CreateBuyOrder_MaxQtde()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("20-04-1998"),
                Price = 123,
                StockName = "Test",
                Quantity = 100001,
                StockSymbol = "Test",
            };

            if (buyOrderRequest.Quantity >= 100000)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    _stockService.CreateBuyOrder(buyOrderRequest);
                });
            }
        }

        [Fact]
        public void CreateBuyOrder_MinPrice()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("20-04-1998"),
                Price = 0,
                StockName = "Test",
                Quantity = 1323,
                StockSymbol = "Test",
            };

            if (buyOrderRequest.Price == 0)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    _stockService.CreateBuyOrder(buyOrderRequest);
                });
            }
        }

        [Fact]
        public void CreateBuyOrder_MaxPrice()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("20-04-1998"),
                Price = 10001,
                StockName = "Test",
                Quantity = 132,
                StockSymbol = "Test",
            };

            _helper.WriteLine(buyOrderRequest.Price.ToString());

            if (buyOrderRequest.Price >= 10000)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    _stockService.CreateBuyOrder(buyOrderRequest);
                });
            }    
        }

        [Fact]
        public void CreateBuyOrder_StockSymbolNull()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("20-04-1998"),
                Price = 1002,
                StockName = "Test",
                Quantity = 132,
                StockSymbol = "Test"
            };

            if (buyOrderRequest.StockSymbol == null)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    _stockService.CreateBuyOrder(buyOrderRequest);
                });
            }
        }

        [Fact]
        public void CreateBuyOrder_InvalidDate()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2008-04-08"),
                Price = 1023,
                StockName = "Test",
                Quantity = 132,
                StockSymbol = "Test"
            };

            
                Assert.Throws<ArgumentException>(() =>
                {
                    _stockService.CreateBuyOrder(buyOrderRequest);
                });
          
        }

        [Fact]
        public void CreateBuyOrder_ProperBuy()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("1998-04-18"),
                Price = 101,
                StockName = "Test",
                Quantity = 132,
                StockSymbol = "Test"
            };

            BuyOrderResponse buyOrderResponse =  _stockService.CreateBuyOrder(buyOrderRequest);
            List<BuyOrderResponse> allBuys =  _stockService.GetBuyOrders();

            _helper.WriteLine(buyOrderResponse.BuyOrderID.ToString());

            Assert.True(buyOrderResponse.BuyOrderID != Guid.Empty);
            Assert.Contains(buyOrderResponse, allBuys);
        }
        #endregion

        #region GetAllOrders

        [Fact]
        public void GetAllOrders_EmptyList()
        {
            List<BuyOrderResponse> buyOrderResponses = _stockService.GetBuyOrders();

            Assert.Empty(buyOrderResponses);
        }

        [Fact]
        public void GetAllOrders_ProperList()
        {
            List<BuyOrderRequest> buyOrderRequests = new List<BuyOrderRequest>()
            {
                new BuyOrderRequest()
                {
                    DateAndTimeOfOrder = DateTime.Parse("20-04-1928"),
                    Price = 101,
                    StockName = "Teste2",
                    Quantity = 132,
                    StockSymbol = "Teste1"
                },
                new BuyOrderRequest()
                {
                    DateAndTimeOfOrder = DateTime.Parse("20-04-1968"),
                    Price = 21,
                    StockName = "Teste2",
                    Quantity = 142,
                    StockSymbol = "Teste2"
                },
            };

            List<BuyOrderResponse> buyOrderResponses = new List<BuyOrderResponse>();

            foreach(BuyOrderRequest buyOrderRequest in buyOrderRequests)
            {
                buyOrderResponses.Add(_stockService.CreateBuyOrder(buyOrderRequest));
                _helper.WriteLine(buyOrderResponses.ToString());
            }

            foreach(BuyOrderResponse buyOrderResponse in buyOrderResponses)
            {
                _helper.WriteLine(buyOrderResponse.ToString());
            }

            List<BuyOrderResponse> buyOrderResponseMethodGet = _stockService.GetBuyOrders();

            foreach (BuyOrderResponse buyOrderRequest in buyOrderResponseMethodGet)
            {
                _helper.WriteLine(buyOrderRequest.ToString());
            }

            foreach (BuyOrderResponse buyOrderResponseExpected in buyOrderResponses)
            {
                Assert.Contains(buyOrderResponseExpected, buyOrderResponseMethodGet);
            }
        }


        #endregion

        #region SellOrder

        [Fact]
        public void CreateSellOrder_nullOrder()
        {
            SellOrderRequest? sellOrderRequest = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _stockService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void CreateSellOrder_MinimumQtde()
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("20-04-1998"),
                Price = 123,
                StockName = "Test",
                Quantity = 0,
                StockSymbol = "Test",
            };

            if (sellOrderRequest.Quantity == 0)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    _stockService.CreateSellOrder(sellOrderRequest);
                });
            }
        }

        [Fact]
        public void CreateSellOrder_MaxQtde()
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("20-04-1998"),
                Price = 123,
                StockName = "Test",
                Quantity = 100001,
                StockSymbol = "Test",
            };

            if (sellOrderRequest.Quantity >= 100000)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    _stockService.CreateSellOrder(sellOrderRequest);
                });
            }
        }

        [Fact]
        public void CreateSellOrder_MinPrice()
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("20-04-1998"),
                Price = 0,
                StockName = "Test",
                Quantity = 1323,
                StockSymbol = "Test",
            };

            if (sellOrderRequest.Price == 0)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    _stockService.CreateSellOrder(sellOrderRequest);
                });
            }
        }

        [Fact]
        public void CreateSellOrder_MaxPrice()
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("20-04-1998"),
                Price = 10001,
                StockName = "Test",
                Quantity = 132,
                StockSymbol = "Test",
            };


            if (sellOrderRequest.Price >= 10000)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    _stockService.CreateSellOrder(sellOrderRequest);
                });
            }
        }

        [Fact]
        public void CreateSellOrder_StockSymbolNull()
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("20-04-1998"),
                Price = 1002,
                StockName = "Test",
                Quantity = 132,
                StockSymbol = "Test"
            };

            if (sellOrderRequest.StockSymbol == null)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    _stockService.CreateSellOrder(sellOrderRequest);
                });
            }
        }

        [Fact]
        public void CreateSellOrder_InvalidDate()
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2008-04-08"),
                Price = 1023,
                StockName = "Test",
                Quantity = 132,
                StockSymbol = "Test"
            };


            Assert.Throws<ArgumentException>(() =>
            {
                _stockService.CreateSellOrder(sellOrderRequest);
            });

        }

        [Fact]
        public void CreateSellOrder_ProperBuy()
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("1998-04-18"),
                Price = 101,
                StockName = "Test",
                Quantity = 132,
                StockSymbol = "Test"
            };

            SellOrderResponse sellOrderResponse = _stockService.CreateSellOrder(sellOrderRequest);
            List<SellOrderResponse> allBuys = _stockService.GetSellOrders();

            Assert.True(sellOrderResponse.BuyOrderID != Guid.Empty);
            Assert.Contains(sellOrderResponse, allBuys);
        }
        #endregion

        #region GetAllSells

        [Fact]
        public void GetSellSells_EmptyList()
        {
            List<SellOrderResponse> sellOrderResponses = _stockService.GetSellOrders();

            Assert.Empty(sellOrderResponses);
        }

        [Fact]
        public void GetAllSells_ProperList()
        {
            List<SellOrderRequest> sellOrderRequests = new List<SellOrderRequest>()
            {
                new SellOrderRequest()
                {
                    DateAndTimeOfOrder = DateTime.Parse("20-04-1928"),
                    Price = 101,
                    StockName = "Teste2",
                    Quantity = 132,
                    StockSymbol = "Teste1"
                },
                new SellOrderRequest()
                {
                    DateAndTimeOfOrder = DateTime.Parse("20-04-1968"),
                    Price = 21,
                    StockName = "Teste2",
                    Quantity = 142,
                    StockSymbol = "Teste2"
                },
            };

            List<SellOrderResponse> sellOrderResponses = new List<SellOrderResponse>();

            foreach (SellOrderRequest sellOrderRequest in sellOrderRequests)
            {
                sellOrderResponses.Add(_stockService.CreateSellOrder(sellOrderRequest));
           
            }

            List<SellOrderResponse> sellOrderResponseMethodGet = _stockService.GetSellOrders();

            foreach (SellOrderResponse sellOrderResponseExpected in sellOrderResponses)
            {
                Assert.Contains(sellOrderResponseExpected, sellOrderResponseMethodGet);
            }
        }


        #endregion
    }
}
