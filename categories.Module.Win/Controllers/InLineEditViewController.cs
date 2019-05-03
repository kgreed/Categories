using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

using categories.Module.BusinessObjects;
using DevExpress.ExpressApp.Win.Editors;
 





namespace categories.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class InLineEditViewController : ViewController
    {
        public InLineEditViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(ICanEditInLIne);
        }
        private void ViewOnControlsCreated(object sender, EventArgs eventArgs)
        {
            var listView = View as ListView;
            if (!(listView?.Editor is GridListEditor editor)) return;
            var allowEdit = !(Frame.Template is ILookupPopupFrameTemplate);
            editor.NewItemRowPosition = NewItemRowPosition.Bottom;
            editor.AllowEdit = View.Model.AllowEdit = allowEdit;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            View.ControlsCreated += ViewOnControlsCreated;
           
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            View.ControlsCreated -= ViewOnControlsCreated;
        }
    }
}
