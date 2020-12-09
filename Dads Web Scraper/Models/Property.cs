using System;
using System.Collections.Generic;
using System.Text;

namespace Dads_Web_Scraper.Models
{
    public class Property
    {
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Price { get; set; }
        public string Zoning { get; set; }
        public string LandArea { get; set; }
        public string FloorArea { get; set; }
        public string Url { get; set; }
        public string CarSpaces { get; set; }
        public string PropertyExtent { get; set; }
        public string TenureType { get; set; }
        public string Description { get; set; }
        public string Agency { get; set; }
        public string Size { get; set; }
        public string AgenctContactName { get; set; }
        public string AgenctContactNumber { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
        public string ParkingInfo { get; set; }
        public List<Attribute> Attributes { get; set; }

    }

    public class Attribute
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }
}
