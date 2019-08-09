using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
//using DevExpress.ExpressApp;
using System.ComponentModel;
//using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using Solution5.Module.BusinessObjects;

namespace Solution5.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Resume")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Resume : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Resume(Session session)
            : base(session)
        {
        }
        private Contact contact;
        public Contact Contact
        {
            get
            {
                return contact;
            }
            set
            {
                SetPropertyValue("Contact", ref contact, value);
            }
        }
        [Aggregated, Association("Resume-PortfolioFileData")]
        public XPCollection<PortfolioFileData> Portfolio
        {
            get { return GetCollection<PortfolioFileData>("Portfolio"); }
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
    public class PortfolioFileData : FileAttachmentBase
    {
        public PortfolioFileData(Session session) : base(session) { }
        protected Resume resume;
        [Persistent, Association("Resume-PortfolioFileData")]
        public Resume Resume
        {
            get { return resume; }
            set
            {
                SetPropertyValue("Resume", ref resume, value);
            }
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            documentType = DocumentType.Unknown;
        }
        private DocumentType documentType;
        public DocumentType DocumentType
        {
            get { return documentType; }
            set { SetPropertyValue("DocumentType", ref documentType, value); }
        }
    }
    public enum DocumentType
    {
        SourceCode = 1, Tests = 2, Documentation = 3,
        Diagrams = 4, ScreenShots = 5, Unknown = 6
    };
}