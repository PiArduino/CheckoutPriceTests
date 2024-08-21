using CheckoutPrice;

namespace Tests
{
    [TestClass()]
    public class CheckoutTests
    {
        private Checkout _checkout;


        [TestInitialize]
        public void Init() 
        {
            var itemPrices = new Dictionary<string, int>
            {
                {"A", 50},
                {"B", 30},
                {"C", 20},
                {"D", 15}
            };

            var specialPrices = new Dictionary<string, SpecialPriceProvider>
            {
                {"A", new SpecialPriceProvider(3, 130)},
                {"B", new SpecialPriceProvider(2, 45)}
            };

            IPriceProvider priceProvider = new PriceProvider(itemPrices);
            ISpecialPriceManager specialPriceManager = new SpecialPriceManager(specialPrices);
            _checkout = new Checkout(priceProvider, specialPriceManager);
        }


        [TestMethod()]
        [DataRow("A", 50)]
        [DataRow("C", 20)]
        [DataRow("B", 30)]
        public void ScanSingleItem_ExpectSinglePriceTotal(string item, int expectedTotal)
        {
            _checkout.Scan(item);

            Assert.AreEqual(expectedTotal, _checkout.GetTotalPrice());
        }

        [TestMethod()]
        public void ScanUnknownItem_ExpectZeroPriceTotal()
        {
            _checkout.Scan("Unknown");

            Assert.AreEqual(0, _checkout.GetTotalPrice());
        }

        [TestMethod()]
        [DataRow(new[] { "A", "B" }, 80)]
        [DataRow(new[] { "A", "A" }, 100)]
        [DataRow(new[] { "A", "B", "C" }, 100)]
        [DataRow(new[] { "C", "A", "B" }, 100)]
        public void ScanMultiItems_WithoutSpecialOffer_ExpectNormalPriceTotal(string[] items, int expectedTotal)
        {
            foreach (var item in items)
            {
                _checkout.Scan(item);
            }

            Assert.AreEqual(expectedTotal, _checkout.GetTotalPrice());
        }



        [TestMethod()]
        [DataRow(new[] { "A", "A", "A" }, 130)]
        [DataRow(new[] { "B", "B" }, 45)]
        public void ScanMultiItems_WithSpecialOffers_ExpectDiscountedTotal(string[] items, int expectedTotal)
        {
            foreach (var item in items)
            {
                _checkout.Scan(item);
            }

            Assert.AreEqual(expectedTotal, _checkout.GetTotalPrice());
        }


        [TestMethod()]
        [DataRow(new[] { "A", "A", "A", "A", "B", "B", "B" }, 255)]
        public void ScanMultiItems_WithSpecialAndNormalOffers_ExpectDiscountedTotal(string[] items, int expectedTotal)
        {
            foreach (var item in items)
            {
                _checkout.Scan(item);
            }

            Assert.AreEqual(expectedTotal, _checkout.GetTotalPrice());
        }
    }
}