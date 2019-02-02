using System;
using System.Collections.Generic;
using System.Linq;

namespace MG_Test
{
    public class Parser
    {
        // this code will take in a list of records (represented as a list of strings; each line of the csv)
        // and return a populated FileRecord
        static public FileRecord parseFileRecord(List<List<string>> records)
        {
            // if the file is truly malformed then we're in trouble - throw exception
            if (records[0][0] != "F")
            {
                throw new ArgumentException("Expected F entry");
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
                        };
                        break;
                    case "O":
                        fileRecord.Orders = fileRecord.Orders ?? new List<Order>();
                        fileRecord.Orders.Add(new Order()
                        {
                            date = record[1],
                            code = record[2],
                            number = record[3],
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
                        fileRecord.Orders.Last().items = fileRecord.Orders.Last().items ?? new List<Item>();

                        fileRecord.Orders.Last().items.Add(new Item()
                        {
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
                        // ignore other entries
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
        public class FileRecord
        {
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

    }
}
