﻿using System;
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
    public partial class ActionController : ViewController
    {
        public ActionController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
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

        private void simpleAction1_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
             
             using (var db = new MyDbContext())
             {
                 db.Categories.RemoveRange(db.Categories);
                 db.SaveChanges();
                 MyInitializer.SeedRecords(db);
            
             }
              
             View.ObjectSpace.Refresh();
        }

        private void actOutdent_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var category = e.CurrentObject as MCategory;
            if (category?.Parent == null) return;
            category.Parent = category.Parent?.Parent;
            ObjectSpace.SetModified(category);
            ObjectSpace.CommitChanges();
        }

        private void actIndent_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //var category = e.CurrentObject as MCategory;
            //View.
            //if (category?.Parent == null) return;
            //category.Parent = category.Parent?.Parent;
            //ObjectSpace.SetModified(category);
            //ObjectSpace.CommitChanges();
        }
    }
}
