using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
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

    public class Contact : Person
    {
        
        [Association("Contact-DemoTask")]
        public XPCollection<DemoTask> Tasks
        {
            get
            {
                return GetCollection<DemoTask>("Tasks");
            }
        }
        public Contact(Session session) : base(session) { }
        private string webPageAddress;
        public string WebPageAddress
        {
            get { return webPageAddress; }
            set { SetPropertyValue("WebPageAddress", ref webPageAddress, value); }
        }
        private string nickName;
        public string NickName
        {
            get { return nickName; }
            set { SetPropertyValue("NickName", ref nickName, value); }
        }
        private string spouseName;
        public string SpouseName
        {
            get { return spouseName; }
            set { SetPropertyValue("SpouseName", ref spouseName, value); }
        }
        private TitleOfCourtesy titleOfCourtesy;
        public TitleOfCourtesy TitleOfCourtesy
        {
            get { return titleOfCourtesy; }
            set { SetPropertyValue("TitleOfCourtesy", ref titleOfCourtesy, value); }
        }
        private DateTime anniversary;
        public DateTime Anniversary
        {
            get { return anniversary; }
            set { SetPropertyValue("Anniversary", ref anniversary, value); }
        }
        private string notes;
        [Size(4096)]
        public string Notes
        {
            get { return notes; }
            set { SetPropertyValue("Notes", ref notes, value); }
        }
        private Department department;
        [Association("Department-Contacts")]
        public Department Department
        {
            get { return department; }
            set { SetPropertyValue("Department", ref department, value);
                if (!IsLoading)
                {
                    Position = null;
                    if (Manager != null && Manager.Department != value)
                    {
                        Manager = null;
                    }
                }
            }
        }
        private Position position;
        public Position Position
        {
            get { return position; }
            set { SetPropertyValue("Position", ref position, value); }
        }
        private Contact manager;
        [DataSourceProperty("Department.Contacts", DataSourcePropertyIsNullMode.SelectAll)]
        [DataSourceCriteria("Position.Title = 'Manager' AND Oid != '@This.Oid'")]
        public Contact Manager
        {
            get { return manager; }
            set { SetPropertyValue("Manager", ref manager, value); }
        }
        private XPCollection<AuditDataItemPersistent> changeHistory;
        public XPCollection<AuditDataItemPersistent> ChangeHistory
        {
            get
            {
                if (changeHistory == null)
                {
                    changeHistory = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return changeHistory;
            }
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            FirstName = "Sam";
            TitleOfCourtesy = TitleOfCourtesy.Mr;
        }

    }
    public enum TitleOfCourtesy { Dr, Miss, Mr, Mrs, Ms };
    [DefaultClassOptions]
    [System.ComponentModel.DefaultProperty("Title")]
    public class Department : BaseObject
    {
        [Association("Department-Contacts")]
    public XPCollection<Contact> Contacts {
        get {
            return GetCollection<Contact>("Contacts");
        }
    }
        public Department(Session session) : base(session) { }
        [Association("Departments-Positions")]
        public XPCollection<Position> Positions
        {
            get { return GetCollection<Position>("Positions"); }
        }
        private string title;
        public string Title
        {
            get { return title; }
            set { SetPropertyValue("Title", ref title, value); }
        }
        private string office;
        public string Office
        {
            get { return office; }
            set { SetPropertyValue("Office", ref office, value); }
        }
    }
    [DefaultClassOptions]
    [System.ComponentModel.DefaultProperty("Title")]
    public class Position : BaseObject
    {
        public Position(Session session) : base(session) { }
        [Association("Departments-Positions")]
        public XPCollection<Department> Departments
        {
            get { return GetCollection<Department>("Departments"); }
        }
        private string title;
        [RuleRequiredField(DefaultContexts.Save)]
        public string Title
        {
            get { return title; }
            set { SetPropertyValue("Title", ref title, value); }
        }
    }
}