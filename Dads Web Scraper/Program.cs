using CsvHelper;
using Dads_Web_Scraper.Models;
using Dads_Web_Scraper.Processors;
using Dads_Web_Scraper.Sources;
using DocumentFormat.OpenXml.Drawing.Charts;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web.Helpers;

namespace Dads_Web_Scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            string outputPath = "C:/Users/Alana/Desktop/Scraper.csv";
            string url = "https://www.commercialrealestate.com.au/business-for-sale/melbourne-region-vic/medical/";
            var processor = new MainProcessor();

            processor.Process(outputPath, url);
        }
    }
}
