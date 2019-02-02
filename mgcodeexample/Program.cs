using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MG_Test
{
    public class Program
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

            var resultObject = Parser.parseFileRecord(csvFileData);

            var serialized = SerializerHelper(resultObject);
            
            Console.WriteLine(serialized);
        }
        private static string SerializerHelper(object input)
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
