using Dads_Web_Scraper.Models;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Threading;

namespace Dads_Web_Scraper.Sources
{
    class RealCommercial
    {
        public List<string> GetPropertiesFromRealCommercial(IWebDriver driver)
        {
            List<Property> propertyList = new List<Property>();
            List<string> stringList = new List<string>();
            bool timeToEnd = false;
            bool lastRound = false;
            stringList.Add($"Address,Size,Type,Price,Url");

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
                    string propertyUrl = "";
                    bool addProperty = false;

                    if (driver.FindElements(By.XPath("/html/body/div[3]/div/button")).Count > 0)
                    {
                        driver.FindElement(By.XPath("/html/body/div[3]/div/button")).Click();
                    }

                    if (driver.FindElements(By.XPath(xPathPreString + "/div/div")).Count == 4) //big listing
                    {
                        address = driver.FindElement(By.XPath(xPathPreString + xpathPropertyVariation2)).GetAttribute("title");
                        price = driver.FindElement(By.XPath(xPathPreString + "/div/div[3]/div[1]/h3")).Text;
                        size = driver.FindElement(By.XPath(xPathPreString + "/div/div[3]/div[1]/div/span[1]")).Text;
                        propertyUrl = driver.FindElement(By.XPath(xPathPreString + "/div/div[4]/a")).GetAttribute("href");

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
                            propertyUrl = driver.FindElement(By.XPath(xPathPreString + "/div/div[2]/div[3]/a")).GetAttribute("href");
                            addProperty = true;
                        }
                        else if (driver.FindElements(By.XPath(xPathPreString + "/div/div[1]/div")).Count == 2)//small listing start
                        {

                            int asd = driver.FindElements(By.XPath(xPathPreString + "/div/div[1]/div[2]/div")).Count;
                            int asasdd = driver.FindElements(By.XPath(xPathPreString + "/div/div[1]/div[2]/h2")).Count;

                            if (driver.FindElements(By.XPath(xPathPreString + "/div/div[1]/div[2]/div")).Count == 2)
                            {
                                try
                                {
                                    address = driver.FindElement(By.XPath(xPathPreString + xpathPropertyVariation1)).GetAttribute("title");
                                }
                                catch
                                {
                                    address = driver.FindElement(By.XPath(xPathPreString + xpathPropertyVariation1)).GetAttribute("title");
                                }
                            }
                            else
                            {
                                address = driver.FindElement(By.XPath(xPathPreString + "/div/div[1]/div[2]/h2/a")).GetAttribute("title");
                            }

                            price = driver.FindElement(By.XPath(xPathPreString + "/div/div[1]/div[2]/h3")).Text;

                            if (driver.FindElements(By.XPath(xPathPreString + "/div/div[1]/div[2]/div")).Count == 2)
                            {
                                size = driver.FindElement(By.XPath(xPathPreString + "/div/div[1]/div[2]/div[2]/span[1]")).Text;
                            }
                            else
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

                            propertyUrl = driver.FindElement(By.XPath(xPathPreString + "//div/div[2]/div/a")).GetAttribute("href");

                            addProperty = true;
                        }
                    }

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

                Thread.Sleep(800);
                if (driver.FindElements(By.ClassName("NextPageButton_label_1CDrV")).Count > 0 && !timeToEnd)
                {
                    Thread.Sleep(800);
                    driver.FindElement(By.ClassName("NextPageButton_label_1CDrV")).Click();
                }
                Thread.Sleep(800);
                if (driver.FindElements(By.ClassName("Pagination_notActive_3p6uc")).Count > 0)
                {
                    lastRound = true;
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

            return stringList;
        }
    }
}
