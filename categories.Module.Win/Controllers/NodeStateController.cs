using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using categories.Module.BusinessObjects;
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

namespace categories.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class NodeStateController : ViewController
    {
        private RefreshController refreshController;
        public NodeStateController()
        {
            InitializeComponent();
            TargetObjectType = typeof(MCategory);
            TargetViewType = ViewType.ListView;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {

            base.OnActivated();
            refreshController = Frame.GetController<RefreshController>();
            if (refreshController == null) return;
            refreshController.RefreshAction.Executing += RefreshAction_Executing;
            refreshController.RefreshAction.Executed += RefreshAction_Executed;
            // Perform various tasks depending on the target View.
        }

        private void RefreshAction_Executed(object sender, ActionBaseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void RefreshAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           // throw new NotImplementedException();
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
