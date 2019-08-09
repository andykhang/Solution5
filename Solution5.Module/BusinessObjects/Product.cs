using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
//using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace Solution5.Module.BusinessObjects
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Product : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Product(Session session)
            : base(session)
        {
        }
        private string fName;
        public string Name
        {
            get { return fName; }
            set { SetPropertyValue("Name", ref fName, value); }
        }
        [Association("Product-Orders"), Aggregated]
        public XPCollection<Order> Orders
        {
            get { return GetCollection<Order>("Orders"); }
        }
        [Persistent("OrdersCount")]
        private int? fOrdersCount = null;
        public int? OrdersCount
        {
            get
            {
                if (!IsLoading && !IsSaving && fOrdersCount == null)
                    UpdateOrdersCount(false);
                return fOrdersCount;
            }
        }
        [Persistent("OrdersTotal")]
        private decimal? fOrdersTotal = null;
        public decimal? OrdersTotal
        {
            get
            {
                if (!IsLoading && !IsSaving && fOrdersTotal == null)
                    UpdateOrdersTotal(false);
                return fOrdersTotal;
            }
        }
        [Persistent("MaximumOrder")]
        private decimal? fMaximumOrder = null;
        public decimal? MaximumOrder
        {
            get
            {
                if (!IsLoading && !IsSaving && fMaximumOrder == null)
                    UpdateMaximumOrder(false);
                return fMaximumOrder;
            }
        }
        public void UpdateOrdersCount(bool forceChangeEvents)
        {
            int? oldOrdersCount = fOrdersCount;
            fOrdersCount = Convert.ToInt32(Evaluate(CriteriaOperator.Parse("Orders.Count")));
            if (forceChangeEvents)
                OnChanged("OrdersCount", oldOrdersCount, fOrdersCount);
        }
        public void UpdateOrdersTotal(bool forceChangeEvents)
        {
            decimal? oldOrdersTotal = fOrdersTotal;
            decimal tempTotal = 0m;
            foreach (Order detail in Orders)
                tempTotal += detail.Total;
            fOrdersTotal = tempTotal;
            if (forceChangeEvents)
                OnChanged("OrdersTotal", oldOrdersTotal, fOrdersTotal);
        }
        public void UpdateMaximumOrder(bool forceChangeEvents)
        {
            decimal? oldMaximumOrder = fMaximumOrder;
            decimal tempMaximum = 0m;
            foreach (Order detail in Orders)
                if (detail.Total > tempMaximum)
                    tempMaximum = detail.Total;
            fMaximumOrder = tempMaximum;
            if (forceChangeEvents)
                OnChanged("MaximumOrder", oldMaximumOrder, fMaximumOrder);
        }
        protected override void OnLoaded()
        {
            Reset();
            base.OnLoaded();
        }
        private void Reset()
        {
            fOrdersCount = null;
            fOrdersTotal = null;
            fMaximumOrder = null;
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
    [DefaultClassOptions]
    public class Order : BaseObject
    {
        public Order(Session session) : base(session) { }

        private string fDescription;
        public string Description
        {
            get { return fDescription; }
            set { SetPropertyValue("Description", ref fDescription, value); }
        }
        private decimal fTotal;
        public decimal Total
        {
            get { return fTotal; }
            set {
                bool modified = SetPropertyValue("Total", ref fTotal, value);
                if (!IsLoading && !IsSaving && Product != null && modified)
                {
                    Product.UpdateOrdersTotal(true);
                    Product.UpdateMaximumOrder(true);
                }
            }
        }
        private Product fProduct;
        [Association("Product-Orders")]
        public Product Product
        {
            get { return fProduct; }
            set
            {
                Product oldProduct = fProduct;
                bool modified = SetPropertyValue("Product", ref fProduct, value);
                if (!IsLoading && !IsSaving && oldProduct != fProduct && modified)
                {
                    oldProduct = oldProduct ?? fProduct;
                    oldProduct.UpdateOrdersCount(true);
                    oldProduct.UpdateOrdersTotal(true);
                    oldProduct.UpdateMaximumOrder(true);
                }
            }
        }

    }
}
