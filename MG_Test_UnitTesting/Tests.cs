using NUnit.Framework;
using System.Collections.Generic;
using static MG_Test.Parser;
using Newtonsoft.Json;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void ParseTest()
        {
            var csvFileData = new List<List<string>>() {
                new List<string> {"F","08/04/2018","By Batch #"},
                new List<string> {"O","08/04/2018","ONF002793300","080427bd1"},
                new List<string> {"B","Brett Nagy","5825 221st Place S.E.","98027"},
                new List<string> {"L","602527788265","02"},
            };

            var expected = SerializerHelper(new FileRecord()
            {
                Date = "08/04/2018",
                Type = "By Batch #",
                Orders = new List<Order>()
                {
                    new Order()
                    {
                        date = "08/04/2018",
                        code = "ONF002793300",
                        number = "080427bd1",
                        buyer = new Buyer()
                        {
                            name = "Brett Nagy",
                            street = "5825 221st Place S.E.",
                            zip = "98027"
                        },
                        items = new List<Item>
                        {
                            new Item
                            {
                                qty = 2,
                                sku = "602527788265"
                            }
                        }
                        
                    }
                }
            });

            var actual = SerializerHelper(parseFileRecord(csvFileData));

            Assert.AreEqual(expected, actual);

        }



        [Test]
        public void ShouldIgnoreInvalidLineEntries()
        {
            var csvFileData = new List<List<string>>() {
                new List<string> {"F","08/04/2018","By Batch #"},
                new List<string> {"X","FAKE","FAKE","FAKE"},
            };

            var actual = SerializerHelper(parseFileRecord(csvFileData));

            // expect to just contain the F entry
            var expected = SerializerHelper(new FileRecord()
            {
                Date = "08/04/2018",
                Type = "By Batch #"
            });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ThrowArgumentExceptionOnMalformedFile()
        {
            var csvFileData = new List<List<string>>() {
                new List<string> {"X","FAKE","FAKE","FAKE"},
                new List<string> {"F","08/04/2018","By Batch #"},
            };

            Assert.Throws<System.ArgumentException>(() => parseFileRecord(csvFileData));
        }



        private string SerializerHelper(object input)
        {
            return JsonConvert.SerializeObject(input,
                            Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
        }
    }
}