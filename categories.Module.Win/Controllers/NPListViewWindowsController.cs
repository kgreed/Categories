using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using categories.Module.BusinessObjects;
using DevExpress.ExpressApp;
 

namespace categories.Module.Win.Controllers
{
    // https://documentation.devexpress.com/eXpressAppFramework/115672/Task-Based-Help/Business-Model-Design/Non-Persistent-Objects/How-to-Perform-CRUD-Operations-with-Non-Persistent-Objects
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class NPListViewWindowsController : WindowController
    {
        private static List<NPCategory> objectsCache;
        static NPListViewWindowsController()
        {
            //InitializeComponent();

            objectsCache = DataGetters.GetNPCategories();
            
        }
        public NPListViewWindowsController()
         : base()
        {
            TargetWindowType = WindowType.Main;
        }
    

        private void NonPersistentObjectSpace_ObjectsGetting(Object sender, ObjectsGettingEventArgs e)
        {
            if (e.ObjectType == typeof(NPCategory))
            {
                IObjectSpace objectSpace = (IObjectSpace)sender;
                BindingList<NPCategory> objects = new BindingList<NPCategory>();
                objects.AllowNew = false;
                objects.AllowEdit = true;
                objects.AllowRemove = false;
                foreach (NPCategory obj in objectsCache)
                {
                    objects.Add(objectSpace.GetObject<NPCategory>(obj));
                }
                e.Objects = objects;
            }
        }

        private void NonPersistentObjectSpace_ObjectByKeyGetting(object sender, ObjectByKeyGettingEventArgs e)
        {
            IObjectSpace objectSpace = (IObjectSpace)sender;
            foreach (Object obj in objectsCache)
            {
                if (obj.GetType() == e.ObjectType && Equals(objectSpace.GetKeyValue(obj), e.Key))
                {
                    e.Object = objectSpace.GetObject(obj);
                    break;
                }
            }
        }
        private void NonPersistentObjectSpace_ObjectGetting(object sender, ObjectGettingEventArgs e)
        {
            if (e.SourceObject is IObjectSpaceLink)
            {
                ((IObjectSpaceLink)e.TargetObject).ObjectSpace = (IObjectSpace)sender;
            }
        }
        private void NonPersistentObjectSpace_Committing(Object sender, CancelEventArgs e)
        {
            IObjectSpace objectSpace = (IObjectSpace)sender;
            foreach (Object obj in objectSpace.ModifiedObjects)
            {
                if (obj is NPCategory)
                {
                    if (objectSpace.IsNewObject(obj))
                    {
                        objectsCache.Add((NPCategory)obj);
                    }
                    else if (objectSpace.IsDeletedObject(obj))
                    {
                        objectsCache.Remove((NPCategory)obj);
                    }

                    else
                    {
                        ((IXafEntityObject)obj).OnSaving();
                    }
                }
            }
        }

        private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e)
        {
            if (e.ObjectSpace is NonPersistentObjectSpace nonPersistentObjectSpace)
            {
                nonPersistentObjectSpace.ObjectsGetting += NonPersistentObjectSpace_ObjectsGetting;
                nonPersistentObjectSpace.ObjectByKeyGetting += NonPersistentObjectSpace_ObjectByKeyGetting;
                nonPersistentObjectSpace.ObjectGetting += NonPersistentObjectSpace_ObjectGetting;
                nonPersistentObjectSpace.Committing += NonPersistentObjectSpace_Committing;
            }
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            Application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            Application.ObjectSpaceCreated -= Application_ObjectSpaceCreated;
        }
        
    }
}
