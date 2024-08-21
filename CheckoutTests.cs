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

            IPriceProvider priceProvider = new PriceProvider(itemPrices);
            _checkout = new Checkout(priceProvider);
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
        [DataRow(new[] { "A", "A", "A", "B", "B", "D" }, 225)]
        public void ScanMultiItems_WithoutSpecialOffer_ExpectNormalPriceTotal(string[] items, int expectedTotal)
        {
            foreach (var item in items)
            {
                _checkout.Scan(item);
            }

            Assert.AreEqual(expectedTotal, _checkout.GetTotalPrice());
        }








    }
}