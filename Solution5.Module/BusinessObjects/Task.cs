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
using DevExpress.Persistent.Base.General;
using DevExpress.ExpressApp.ConditionalAppearance;
//using DevExpress.ExpressApp.Model;

namespace Solution5.Module.BusinessObjects
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [ModelDefault("Caption", "Task")]
    [Appearance("FontColorRed", AppearanceItemType = "ViewItem", TargetItems = "*", Context = "ListView",
    Criteria = "Status!='Completed'", FontColor = "Red")]
    public class DemoTask : Task
    {
        private Priority priority;
        [Appearance("PriorityBackColorPink", AppearanceItemType = "ViewItem", Context = "Any",
        Criteria = "Priority=2", BackColor = "255, 240, 240")]
        public Priority Priority
        {
            get { return priority; }
            set
            {
                SetPropertyValue("Priority", ref priority, value);
            }
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Priority = Priority.Normal;
        }
        public DemoTask(Session session) : base(session) { }
        [Association("Contact-DemoTask")]
        public XPCollection<Contact> Contacts
        {
            get
            {
                return GetCollection<Contact>("Contacts");
            }
        }
        [Action(ToolTip = "Postpone the task to the next day")]
        public void Postpone()
        {
            if (DueDate == DateTime.MinValue)
            {
                DueDate = DateTime.Now;
            }
            DueDate = DueDate + TimeSpan.FromDays(1);
        }
        private string des;
        public string Description { get { return des; }  set { SetPropertyValue("Description", ref des, value); } }
        private TaskStatus status;
        public TaskStatus Status { get { return status; } set { SetPropertyValue("Status", ref status, value); } }
        private DateTime Due;
        public DateTime DueDate { get { return Due; } set { SetPropertyValue("Due Date", ref Due, value); } }
    }
    public enum Priority
    {
        [ImageName("State_Priority_Low")]
        Low = 0,
        [ImageName("State_Priority_Normal")]
        Normal = 1,
        [ImageName("State_Priority_High")]
        High = 2
    }
    public class Task : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Task(Session session)
            : base(session)
        {
        }
        public string Subject
        {
            get { return GetPropertyValue<string>("Subject"); }
            set { SetPropertyValue<string>("Subject", value); }
        }
        private bool isCompleted;
        public bool IsCompleted
        {
            get { return isCompleted; }
            set { SetPropertyValue("IsCompleted", ref isCompleted, value); }
        }
        [Action(Caption = "Complete", TargetObjectsCriteria = "Not [IsCompleted]")]
        public void Complete()
        {
            IsCompleted = true;
        }
        private DateTime? deadline;
        public DateTime? Deadline
        {
            get { return deadline; }
            set { SetPropertyValue("Deadline", ref deadline, value); }
        }
        private string comments;
        [Size(SizeAttribute.Unlimited), ModelDefault("AllowEdit", "False")]
        public string Comments
        {
            get { return comments; }
            set { SetPropertyValue("Comments", ref comments, value); }
        }
        [DomainComponent]
        public class PostponeParametersObject
        {
            public PostponeParametersObject() { PostponeForDays = 1; }
            public uint PostponeForDays { get; set; }
            [Size(SizeAttribute.Unlimited)]
            public string Comment { get; set; }
        }
        [Action(Caption = "Postpone",
    TargetObjectsCriteria = "[Deadline] Is Not Null And Not [IsCompleted]")]
        public void Postpone(PostponeParametersObject parameters)
        {
            if (Deadline.HasValue && !IsCompleted && (parameters.PostponeForDays > 0))
            {
                Deadline += TimeSpan.FromDays(parameters.PostponeForDays);
                Comments += String.Format("Postponed for {0} days, new deadline is {1:d}\r\n{2}\r\n",
                parameters.PostponeForDays, Deadline, parameters.Comment);
            }
        }
        public Issue Issue
        {
            get { return GetPropertyValue<Issue>("Issue"); }
            set { SetPropertyValue<Issue>("Issue", value); }
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
}