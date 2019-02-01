using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MG_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // we're going to assume that nested items are in the correct order,
            // so F contains multiple O's which contain multiple L's
            // so a file with F L L O entries can not exist.
            // And F O L L L O L E contains two orders with three and one items respectively

            var csvFileData = new List<List<string>>() {
                new List<string> {"F","08/04/2018"," By Batch #"},
                new List<string> {"O","08/04/2018","ONF002793300","080427bd1"},
                new List<string> {"B","Brett Nagy","5825 221st Place S.E.","98027"},
                new List<string> {"L","602527788265","02"},
                new List<string> {"L","602517642850","01"},
                new List<string> {"T","3","3","0","2","0"},
                new List<string> {"E","1","2","9"}
            };

            var resultObject = parseFileRecord(csvFileData);

            var serialized = JsonConvert.SerializeObject(resultObject);
            
            Console.WriteLine(serialized);
        }

        static public FileRecord parseFileRecord(List<List<string>> records)
        {
            // if the file is truly malformed then we're in trouble
            
            if (records[0][0] != "F") 
            {
                throw new Exception("Expected F entry");
            }
            FileRecord fileRecord = null;

            foreach (var record in records)
            {
                switch (record[0])
                {
                    case "F":
                        fileRecord = new FileRecord()
                        {
                            Date = record[1],
                            Type = record[2],
                            Orders = new List<Order>(),
                            ender = new Ender(),
                        };
                        break;
                    case "O":
                        fileRecord.Orders.Add(new Order()
                        {
                            date = record[1],
                            code = record[2],
                            number = record[3],
                            buyer = new Buyer(),
                            items = new List<Item>(),
                            timings = new Timings()
                        });
                        break;
                    case "B":
                        fileRecord.Orders.Last().buyer = new Buyer()
                        {
                            name = record[1],
                            street = record[2],
                            zip = record[3],
                        };
                        break;
                    case "L":
                        fileRecord.Orders.Last().items.Add(new Item() {
                            sku = record[1],
                            qty = int.Parse(record[2])
                        });
                        break;
                    case "T":
                        fileRecord.Orders.Last().timings = new Timings
                        {
                            start = int.Parse(record[1]),
                            stop = int.Parse(record[2]),
                            gap = int.Parse(record[3]),
                            offset = int.Parse(record[4]),
                            pause = int.Parse(record[5]),
                        };
                        break;
                    case "E":
                        fileRecord.ender = new Ender()
                        {
                            process = int.Parse(record[1]),
                            paid = int.Parse(record[2]),
                            created = int.Parse(record[3])
                        };
                        break;
                    default:
                        break;
                }
            }
            
            return fileRecord;
        }

        public class Ender
        {
            public int process { get; set; }
            public int paid { get; set; }
            public int created { get; set; }
        }

        public class Buyer
        {
            public string name { get; set; }
            public string street { get; set; }
            public string zip { get; set; }
        }

        public class Order
        {
            public string date { get; set; }
            public string code { get; set; }
            public string number { get; set; }
            public Buyer buyer { get; set; }
            public List<Item> items { get; set; }
            public Timings timings { get; set; }
        }

        public class Item
        {
            public string sku { get; set; }
            public int qty { get; set; }
        }
        public class FileRecord { 
            public string Date { get; set; }
            public string Type { get; set; }
            public List<Order> Orders { get; set; }
            public Ender ender { get; set; }
        }
        
        public class Timings
        {
            public int start { get; set; }
            public int stop { get; set; }
            public int gap { get; set; }
            public int offset { get; set; }
            public int pause { get; set; }
        }
        /*
        {
    "date":"08/04/2018",
    "type":"By Batch #",
    "orders":[
    {
    "date":"08/04/2018",
    "code":"ONF002793300",
    "number":"080427bd1",
    "buyer":{
    "name":"Brett Nagy",
    "street":"5825 221st Place S.E.",
    "zip":"98027"
    },
    "items":[
    {
    "sku":"602527788265",
    "qty":2
    },
    {
    "sku":"602517642850",
    "qty":1
    }
    ],
    "timings":{
    "start":3,
    "stop":3,
    "gap":0,
    "offset":2,
    "pause":0
    }
    }
    ],
    "ender":{
    "process":1,
    "paid":2,
    "created":9
    }
    } 
         */
    }

}
