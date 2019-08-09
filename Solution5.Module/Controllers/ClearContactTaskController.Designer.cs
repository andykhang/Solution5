namespace Solution5.Module.Controllers
{
    partial class ClearContactTaskController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ClearTaskAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ClearTaskAction
            // 
            this.ClearTaskAction.Caption = "Clear Task ";
            this.ClearTaskAction.Category = "View";
            this.ClearTaskAction.ConfirmationMessage = "Are you sure you want to clear the task list?";
            this.ClearTaskAction.Id = "ClearTaskAction";
            this.ClearTaskAction.ImageName = "Action_Clear";
            this.ClearTaskAction.ToolTip = null;
            this.ClearTaskAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ClearTaskAction_Execute);
            // 
            // ClearContactTaskController
            // 
            this.Actions.Add(this.ClearTaskAction);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.Activated += new System.EventHandler(this.ClearContactTaskController_Activated);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ClearTaskAction;
    }
}
