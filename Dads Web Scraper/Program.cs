using Dads_Web_Scraper.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace Dads_Web_Scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebDriver driver = new ChromeDriver();
            string url = "https://www.realcommercial.com.au/";
            string searchString = "3138";
            //Console.WriteLine("Hello World!");

            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.XPath("//*[@id='wrapper']/div/div[3]/div[1]/div/form/div[1]/div/div[1]/input")).SendKeys(searchString);
            driver.FindElement(By.XPath("//*[@id='wrapper']/div/div[3]/div[1]/div/form/div[1]/button")).Click();
            List<Property> propertyList = new List<Property>();
            int i = 1;
            System.Threading.Thread.Sleep(5000);
            do
            {
                if (driver.FindElement(By.XPath($"//*[@id='wrapper']/div/div[3]/div/div[2]/div[1]/ol/li[{i}]/div/div[3]")).Displayed)
                {
                    Property property = new Property()
                    {
                        Address = driver.FindElement(By.XPath($"//*[@id='wrapper']/div/div[3]/div/div[2]/div[1]/ol/li[{i}]/div/div[3]/div[1]/h2/a")).GetAttribute("title"),
                        Size = driver.FindElement(By.XPath($"//*[@id='wrapper']/div/div[3]/div/div[2]/div[1]/ol/li[{i}]/div/div[3]/div[1]/div/span[1]")).Text,
                        Type = driver.FindElement(By.XPath($"//*[@id='wrapper']/div/div[3]/div/div[2]/div[1]/ol/li[{i}]/div/div[3]/div[1]/div/span[2]")).Text,
                        Price = driver.FindElement(By.XPath($"//*[@id='wrapper']/div/div[3]/div/div[2]/div[1]/ol/li[{i}]/div/div[3]/div[1]/h3")).Text
                    };
                    propertyList.Add(property);
                }
                i++;
            } while (driver.FindElement(By.XPath($"//*[@id='wrapper']/div/div[3]/div/div[2]/div[1]/ol/li[{i}]")).Displayed);
        }
    }
}
