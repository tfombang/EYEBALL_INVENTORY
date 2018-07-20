using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EYEBALL_INVENTORY
{
    class StockClass
    {
        private static string _itemName;
        private static string _category;
        private static string _manufacturer;
        private static string _grossPrice;
        private static string _unitPrice;
        private static string _stockQuantity;
        private static string _dateBought;
        private static string _expiryDate;
        private static string _quantitySold;
        private static string _saleLocation;
        private static string _badProduct;

        //Defining the Properties of the Class

        public string itemName
        {
            set { _itemName = value; }
            get { return _itemName; }
        }

        public string category
        {
            set { _category = value; }
            get { return _category; }
        }

        public string manufacturer
        {
            set { _manufacturer = value; }
            get { return _manufacturer; }
        }

        public string grossPrice
        {
            set { _grossPrice = value; }
            get { return _grossPrice; }
        }

        public string unitPrice
        {
            set { _unitPrice = value; }
            get { return _unitPrice; }
        }

        public string stockQuantity
        {
            set { _stockQuantity = value; }
            get { return _stockQuantity; }
        }

        public string dateBought
        {
            set { _dateBought = value; }
            get { return _dateBought; }
        }

        public string expiryDate
        {
            set { _expiryDate = value; }
            get { return _expiryDate; }
        }

        public string quantitySold
        {
            set { _quantitySold = value; }
            get { return _quantitySold; }
        }

        public string saleLocation
        {
            set { _saleLocation = value; }
            get { return _saleLocation; }
        }

        public string badProduct
        {
            set { _badProduct = value; }
            get { return _badProduct; }
        }

    }
}
