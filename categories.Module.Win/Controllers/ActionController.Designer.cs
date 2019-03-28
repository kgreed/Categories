namespace categories.Module.Win.Controllers
{
    partial class ActionController
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
            this.simpleAction1 = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.actIndent = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.actOutdent = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // simpleAction1
            // 
            this.simpleAction1.Caption = "Re-Seed Data";
            this.simpleAction1.ConfirmationMessage = null;
            this.simpleAction1.Id = "Re-Seed Data";
            this.simpleAction1.ToolTip = null;
            this.simpleAction1.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleAction1_Execute);
            // 
            // actIndent
            // 
            this.actIndent.Caption = "Indent";
            this.actIndent.ConfirmationMessage = null;
            this.actIndent.Id = "Indent";
            this.actIndent.ToolTip = null;
            this.actIndent.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.actIndent_Execute);
            // 
            // actOutdent
            // 
            this.actOutdent.Caption = "Outdent";
            this.actOutdent.ConfirmationMessage = null;
            this.actOutdent.Id = "Indent";
            this.actOutdent.ToolTip = null;
            this.actOutdent.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.actOutdent_Execute);
            // 
            // ActionController
            // 
            this.Actions.Add(this.simpleAction1);
            this.Actions.Add(this.actIndent);
            this.Actions.Add(this.actOutdent);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction simpleAction1;
        private DevExpress.ExpressApp.Actions.SimpleAction actIndent;
        private DevExpress.ExpressApp.Actions.SimpleAction actOutdent;
    }
}
