using Dads_Web_Scraper.Sources;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.IO;

namespace Dads_Web_Scraper.Processors
{
    class MainProcessor
    {
        private readonly RealCommercial realCommercial;
        private readonly CommercialRealEstate commercialRealEstate;

        public MainProcessor()
        {
            this.realCommercial = new RealCommercial();
            this.commercialRealEstate = new CommercialRealEstate();
        }

        public void Process(string outputLocation, string url)
        {

            IWebDriver driver = new ChromeDriver();       

            driver.Navigate().GoToUrl(url);
            List<string> stringList = new List<string>();
           

            if (url.Contains("realcommercial.com.au"))
            {
                stringList = realCommercial.GetPropertiesFromRealCommercial(driver);
            }
            else if (url.Contains("commercialrealestate.com.au"))
            {
                stringList = commercialRealEstate.GetPropertiesFromCommercialRealEstate(driver);
            }

            driver.Quit();
            File.WriteAllLines(outputLocation, stringList);

        }
    }

}
