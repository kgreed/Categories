﻿using System;
using System.Text;
using System.Linq;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using System.Data.Entity;
using categories.Module.BusinessObjects;

namespace categories.Module {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
    public sealed partial class categoriesModule : ModuleBase {
        static categoriesModule() {
            DevExpress.Data.Linq.CriteriaToEFExpressionConverter.SqlFunctionsType = typeof(System.Data.Entity.SqlServer.SqlFunctions);
			DevExpress.Data.Linq.CriteriaToEFExpressionConverter.EntityFunctionsType = typeof(System.Data.Entity.DbFunctions);
			DevExpress.ExpressApp.SystemModule.ResetViewSettingsController.DefaultAllowRecreateView = false;
            // Uncomment this code to delete and recreate the database each time the data model has changed.
            // Do not use this code in a production environment to avoid data loss.
            // #if DEBUG
            // Database.SetInitializer(new DropCreateDatabaseIfModelChanges<categoriesDbContext>());
            // #endif 
        }
        public categoriesModule() {
            InitializeComponent();
			AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.HCategory));
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
            application.ObjectSpaceCreated += Application_ObjectSpaceCreated;

        }

        private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e)
        {
            if (e.ObjectSpace is NonPersistentObjectSpace)
            {
                IObjectSpace additionalObjectSpace = Application.CreateObjectSpace(typeof(MCategory));
                ((NonPersistentObjectSpace)e.ObjectSpace).AdditionalObjectSpaces.Add(additionalObjectSpace);
                ((NonPersistentObjectSpace)e.ObjectSpace).ObjectGetting += ObjectSpace_ObjectGetting;
                e.ObjectSpace.Disposed += (s, args) => {
                    ((NonPersistentObjectSpace)s).ObjectGetting -= ObjectSpace_ObjectGetting;
                    additionalObjectSpace.Dispose();
                };
            }
        }
        private void ObjectSpace_ObjectGetting(object sender, ObjectGettingEventArgs e)
        {
            if (e.SourceObject is IObjectSpaceLink)
            {
                e.TargetObject = e.SourceObject;
                ((IObjectSpaceLink)e.TargetObject).ObjectSpace = (IObjectSpace)sender;
            }
        }
         
    }
}
