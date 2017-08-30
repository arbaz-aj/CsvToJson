using System;
using System.Diagnostics;
using System.Reflection;

namespace CsvToJsonV3
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            CsvToJson csvToJson = new CsvToJson();
            csvToJson.JsonParser();

            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
