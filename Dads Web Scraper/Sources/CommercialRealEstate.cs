using Dads_Web_Scraper.Models;

using OpenQA.Selenium;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Dads_Web_Scraper.Sources
{
    class CommercialRealEstate
    {
        public List<string> GetPropertiesFromCommercialRealEstate(IWebDriver driver)
        {
            //try
            //{
            List<Property> propertyList = new List<Property>();
            List<string> stringList = new List<string>();
            stringList.Add($"Address,Size,Type,Price,Url");

            string address = "";
            string price = "";
            string size = "";
            string type = "";
            string propertyUrl = "";
            bool isLastPage = false;

            do
            {
                int propertyNumber = 1;
                //This counts the number of properties listed on the page:
                IList<IWebElement> propertiesOnPage = driver.FindElements(By.XPath("//*[@id='maincontent']/div/div[2]/div[2]/ul/li"));

                if (propertiesOnPage.Count == 0)
                {
                    return null;
                }

                foreach (IWebElement propertyOnPage in propertiesOnPage)
                {
                    bool addProperty = false;

                    if (driver.FindElements(By.XPath($"//*[@id='maincontent']/div/div[2]/div[2]/ul/li[{propertyNumber}]/h2/a")).Count > 0)
                    {

                        propertyUrl = driver.FindElement(By.XPath($"//*[@id='maincontent']/div/div[2]/div[2]/ul/li[{propertyNumber}]/h2/a")).GetAttribute("href");

                        if (driver.FindElements(By.XPath($"//*[@id='maincontent']/div/div[2]/div[2]/ul/li[{propertyNumber}]/div")).Count > 4)
                        {
                            address = driver.FindElement(By.XPath($"//*[@id='maincontent']/div/div[2]/div[2]/ul/li[{propertyNumber}]/div[3]/div[1]/a")).Text;
                            price = driver.FindElement(By.XPath($"//*[@id='maincontent']/div/div[2]/div[2]/ul/li[{propertyNumber}]/div[3]/div[2]")).Text;
                        }
                        else
                        {
                            if (driver.FindElements(By.XPath($"//*[@id='maincontent']/div/div[2]/div[2]/ul/li[{propertyNumber}]/div[2]/div[1]/a")).Count > 0)
                            {
                                address = driver.FindElement(By.XPath($"//*[@id='maincontent']/div/div[2]/div[2]/ul/li[{propertyNumber}]/div[2]/div[1]/a")).Text;
                            }
                            else
                            {
                                address = driver.FindElement(By.XPath($"//*[@id='maincontent']/div/div[2]/div[2]/ul/li[{propertyNumber}]/div[2]/div[1]")).Text;
                            }
                            price = driver.FindElement(By.XPath($"//*[@id='maincontent']/div/div[2]/div[2]/ul/li[{propertyNumber}]/div[2]/div[2]")).Text;
                        }

                        type = driver.FindElement(By.XPath($"//*[@id='maincontent']/div/div[2]/div[2]/ul/li[{propertyNumber}]/div[4]")).Text;
                        if (type == "")
                        {
                            type = driver.FindElement(By.XPath($"//*[@id='maincontent']/div/div[2]/div[2]/ul/li[{propertyNumber}]/div[3]")).Text;
                        }

                    }
                    else
                    {

                        propertyUrl = driver.FindElement(By.XPath($"//*[@id='maincontent']/div/div[2]/div[2]/ul/li[{propertyNumber}]/div[2]/div[2]/h2/a")).GetAttribute("href");
                        //*[@id="maincontent"]/div/div[2]/div[2]/ul/li[1]/h2/a
                        address = driver.FindElement(By.XPath($"//*[@id='maincontent']/div/div[2]/div[2]/ul/li[{propertyNumber}]/div[2]/div[2]/div[2]/div[1]/a")).Text;
                        price = driver.FindElement(By.XPath($"//*[@id='maincontent']/div/div[2]/div[2]/ul/li[{propertyNumber}]/div[2]/div[2]/div[2]/div[2]")).Text;
                        type = driver.FindElement(By.XPath($"//*[@id='maincontent']/div/div[2]/div[2]/ul/li[{propertyNumber}]/div[2]/div[2]/div[3]/div")).Text;

                    }

                    addProperty = true;

                    if (addProperty)
                    {
                        Property property = new Property()
                        {
                            Address = address.Replace(",", " "),
                            Price = price.Replace(",", " "),
                            Size = size.Replace(",", " ").Replace("²", "2"),
                            Url = propertyUrl.Replace(",", " "),
                            Type = type.Replace(",", " ")

                        };
                        propertyList.Add(property);
                        stringList.Add($"{property.Address},{property.Size},{property.Type},{property.Price},{property.Url}");
                    }
                    propertyNumber++;
                }

                //This is where we handle paging. Checks if next page button is available, and if it is, clicks it.
                Thread.Sleep(800);
                if (driver.FindElements(By.XPath("//*[@id='maincontent']/div/div[2]/div[2]/ul/div")).Count > 0) //check if Next Page button exists
                {

                    //There are 2 kinds of Next Page button, so need to make sure we try to click the right one before clicking.
                    if (driver.FindElements(By.XPath("//*[@id='maincontent']/div/div[2]/div[2]/ul/div[4]/div/a")).Count > 0)
                    {
                        driver.FindElement(By.XPath("//*[@id='maincontent']/div/div[2]/div[2]/ul/div[4]/div/a")).Click();
                    }
                    else if (driver.FindElements(By.XPath("//*[@id='maincontent']/div/div[2]/div[2]/ul/div[3]/div/a")).Count == 1)
                    {
                        driver.FindElement(By.XPath("//*[@id='maincontent']/div/div[2]/div[2]/ul/div[3]/div/a")).Click();

                    }
                    else if (driver.FindElements(By.XPath("//*[@id='maincontent']/div/div[2]/div[2]/ul/div[3]/div/a")).Count == 2)
                    {
                        driver.FindElement(By.XPath("//*[@id='maincontent']/div/div[2]/div[2]/ul/div[3]/div/a[2]")).Click();
                    }
                    else
                    {
                        isLastPage = true;
                    }
                    Thread.Sleep(800);

                }
                else
                {
                    isLastPage = true;
                }

            } while (isLastPage == false);

            return stringList;

            //} catch(Exception e)
            //{
            //    Console.WriteLine("Failed Procesing for commercialrealestate.com.au.");
            //    throw e;
            //}
        }
    }
}
