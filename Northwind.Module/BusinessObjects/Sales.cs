using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Northwind.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class Sales : BaseObject
    {
        public Sales(Session session)
            : base(session) {}

        private Contact _Contact;
        public Contact Contact
        {
            get { return _Contact; }
            set { SetPropertyValue(nameof(Contact), ref _Contact, value); }
        }

        private Product _Product;
        public Product Product
        {
            get { return _Product; }
            set { SetPropertyValue(nameof(Product), ref _Product, value); }
        }

        private int _Number;
        public int Number
        {
            get { return _Number; }
            set { SetPropertyValue(nameof(Number), ref _Number, value); }
        }

        private DateTime _SaleDateTime;
        public DateTime SaleDateTime
        {
            get { return _SaleDateTime; }
            set { SetPropertyValue(nameof(SaleDateTime), ref _SaleDateTime, value); }
        }
    }
}