using CsvHelper;

using Dads_Web_Scraper.Models;

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
            IWebDriver driver = new ChromeDriver();
            string url = "https://www.realcommercial.com.au/for-sale/3000/";
            string outputPath = "C:/Users/Alana/Desktop/Scraper.csv";

            driver.Navigate().GoToUrl(url);
            List<Property> propertyList = new List<Property>();
            List<string> stringList = new List<string>();
            stringList.Add($"Address|Size|Type|Price");
            bool timeToEnd = false;
            bool lastRound = false;

            do
            {
                timeToEnd = lastRound;
                int propertyNumber = 1;

                IList<IWebElement> propertiesOnPage = driver.FindElements(By.XPath("//*[@id='wrapper']/div/div[3]/div[1]/div[2]/div[1]/ol/li"));
                if (propertiesOnPage.Count == 0)
                {
                    break;
                }

                foreach (IWebElement propertyOnPage in propertiesOnPage)
                {

                    string xPathPreString = $"//*[@id='wrapper']/div/div[3]/div[1]/div[2]/div[1]/ol/li[{propertyNumber}]";
                    string xpathPropertyVariation1 = "/div/div[1]/div[2]/div[1]/h2/a";
                    string xpathPropertyVariation2 = "/div/div[3]/div[1]/h2/a";
                    string xpathPropertyVariation3 = "/div/div[2]/div[2]/div/h2/a";

                    string address = "";
                    string price = "";
                    string size = "";
                    string type = "";
                    bool addProperty = false;



                    if (driver.FindElements(By.XPath(xPathPreString + "/div/div")).Count == 4) //big listing
                    {
                        address = driver.FindElement(By.XPath(xPathPreString + xpathPropertyVariation2)).GetAttribute("title");
                        price = driver.FindElement(By.XPath(xPathPreString + "/div/div[3]/div[1]/h3")).Text;
                        size = driver.FindElement(By.XPath(xPathPreString + "/div/div[3]/div[1]/div/span[1]")).Text;

                        if (driver.FindElements(By.XPath(xPathPreString + "/div/div[3]/div[1]/div/span")).Count == 2)
                        {
                            type = driver.FindElement(By.XPath(xPathPreString + "/div/div[3]/div[1]/div/span[2]")).Text;
                        }
                        else
                        {
                            type = driver.FindElement(By.XPath(xPathPreString + "/div/div[3]/div[1]/div/span[3]")).Text;
                        }

                        addProperty = true;
                    }
                    else if (driver.FindElements(By.XPath(xPathPreString + "/div/div")).Count == 2)
                    {

                        if (driver.FindElements(By.XPath(xPathPreString + "/div/div[1]/div")).Count == 1)//small listing end
                        {
                            address = driver.FindElement(By.XPath(xPathPreString + xpathPropertyVariation3)).GetAttribute("title");
                            price = driver.FindElement(By.XPath(xPathPreString + "/div/div[2]/div[2]/div/h3")).Text;
                            size = driver.FindElement(By.XPath(xPathPreString + "/div/div[2]/div[2]/div/div/span[1]")).Text;
                            type = driver.FindElement(By.XPath(xPathPreString + "/div/div[2]/div[2]/div/div/span[2]")).Text;
                            addProperty = true;
                        }
                        else if (driver.FindElements(By.XPath(xPathPreString + "/div/div[1]/div")).Count == 2)//small listing start
                        {

                            int asd = driver.FindElements(By.XPath(xPathPreString + "/div/div[1]/div[2]/div")).Count;
                            int asasdd = driver.FindElements(By.XPath(xPathPreString + "/div/div[1]/div[2]/h2")).Count;
                            if (driver.FindElements(By.XPath(xPathPreString + "/div/div[1]/div[2]/div")).Count == 2)
                            {
                                address = driver.FindElement(By.XPath(xPathPreString + xpathPropertyVariation1)).GetAttribute("title");
                            }
                            else
                            {
                                address = driver.FindElement(By.XPath(xPathPreString + "/div/div[1]/div[2]/h2/a")).GetAttribute("title");
                            }

                            price = driver.FindElement(By.XPath(xPathPreString + "/div/div[1]/div[2]/h3")).Text;

                            if (driver.FindElements(By.XPath(xPathPreString + "/div/div[1]/div[2]/div")).Count == 2)
                            {
                                size = driver.FindElement(By.XPath(xPathPreString + "/div/div[1]/div[2]/div[2]/span[1]")).Text;
                            } else
                            {
                                size = driver.FindElement(By.XPath(xPathPreString + "/div/div[1]/div[2]/div/span[1]")).Text;
                            }

                            if (driver.FindElements(By.XPath(xPathPreString + "/div/div[1]/div[2]/div[2]/span")).Count == 3)
                            {
                                type = driver.FindElement(By.XPath(xPathPreString + "/div/div[1]/div[2]/div[2]/span[3]")).Text;
                            }
                            else
                            {
                                type = driver.FindElement(By.XPath(xPathPreString + "/div/div[1]/div[2]/div/span[1]")).Text;
                            }

                            addProperty = true;
                        }
                    }

                    if (addProperty)
                    {
                        Property property = new Property()
                        {
                            Address = address,
                            Price = price,
                            Size = size,
                            Type = type

                        };
                        propertyList.Add(property);
                        stringList.Add($"{property.Address}|{property.Size}|{property.Type}|{property.Price}");
                    }
                    propertyNumber++;
                }               

                Thread.Sleep(600);
                if (driver.FindElements(By.ClassName("NextPageButton_label_1CDrV")).Count > 0 && !timeToEnd)
                {
                    driver.FindElement(By.ClassName("NextPageButton_label_1CDrV")).Click();
                }
                Thread.Sleep(600);
                if (driver.FindElements(By.ClassName("Pagination_notActive_3p6uc")).Count > 0)
                {
                    lastRound = true;
                }

            } while (!timeToEnd);

            System.IO.File.WriteAllLines(outputPath, stringList);
            driver.Quit();
        }
    }
}
