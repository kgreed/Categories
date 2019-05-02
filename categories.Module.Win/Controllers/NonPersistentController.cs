using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using categories.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;


namespace categories.Module.Win.Controllers
{
    // https://documentation.devexpress.com/eXpressAppFramework/115672/Task-Based-Help/Business-Model-Design/Non-Persistent-Objects/How-to-Perform-CRUD-Operations-with-Non-Persistent-Objects
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    //public partial class NonPersistentController : WindowController
    public partial class NonPersistentController : ViewController
    {
        private static List<INonPersistent> objectsCache;
        static NonPersistentController()
        {
        }
        public NonPersistentController()
         : base()
        {
            //TargetWindowType = WindowType.Main;
            this.TargetObjectType = typeof(INonPersistent);
        }
        private void ObjectSpace_CustomRefresh(object sender, HandledEventArgs e)
        {
            IObjectSpace objectSpace = (IObjectSpace)sender;
            objectsCache = DataGetters.GetNPObjects(View.ObjectTypeInfo, ObjectSpace);
            objectSpace.ReloadCollection(objectsCache);
        }

        private void NonPersistentObjectSpace_ObjectsGetting(Object sender, ObjectsGettingEventArgs e)
        {
            ITypeInfo info = XafTypesInfo.Instance.FindTypeInfo(e.ObjectType);
            //if (e.ObjectType == typeof(NPCategory))
            if (info.Implements<INonPersistent>())
            {
                IObjectSpace objectSpace = (IObjectSpace)sender;
                BindingList<INonPersistent> objects = new BindingList<INonPersistent>();
                objects.AllowNew = false;
                objects.AllowEdit = true;
                objects.AllowRemove = false;
                //objectsCache = DataGetters.GetNPCategories();
                objectsCache = DataGetters.GetNPObjects(info, ObjectSpace);
                foreach (INonPersistent obj in objectsCache)
                {
                    objects.Add(objectSpace.GetObject(obj));
                }
                e.Objects = objects;
            }
        }
        private void NonPersistentObjectSpace_ObjectGetting(object sender, ObjectGettingEventArgs e)
        {
            if (e.SourceObject is IObjectSpaceLink)
            {
                ((IObjectSpaceLink)e.TargetObject).ObjectSpace = (IObjectSpace)sender;
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
  
        private void NonPersistentObjectSpace_Committing(Object sender, CancelEventArgs e)
        {
            IObjectSpace objectSpace = (IObjectSpace)sender;
            foreach (Object obj in objectSpace.ModifiedObjects)
            {
                if (obj is INonPersistent)
                {
                    if (objectSpace.IsNewObject(obj))
                    {
                        objectsCache.Add((INonPersistent)obj);
                    }
                    else if (objectSpace.IsDeletedObject(obj))
                    {
                        objectsCache.Remove((INonPersistent)obj);
                    }

                    else
                    {
                        ((IXafEntityObject)obj).OnSaving();
                    }
                }
            }
        }

        //private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e)
        //{
        //    if (e.ObjectSpace is NonPersistentObjectSpace nonPersistentObjectSpace )
        //    {
        //        nonPersistentObjectSpace.ObjectsGetting += NonPersistentObjectSpace_ObjectsGetting;
        //        nonPersistentObjectSpace.ObjectByKeyGetting += NonPersistentObjectSpace_ObjectByKeyGetting;
        //        nonPersistentObjectSpace.ObjectGetting += NonPersistentObjectSpace_ObjectGetting;
        //        nonPersistentObjectSpace.Committing += NonPersistentObjectSpace_Committing;
        //        e.ObjectSpace.CustomRefresh += ObjectSpace_CustomRefresh;
        //    }
            
        //}

   

        protected override void OnActivated()
        {
            base.OnActivated();

            if (ObjectSpace is NonPersistentObjectSpace nonPersistentObjectSpace)
            {
                nonPersistentObjectSpace.ObjectsGetting += NonPersistentObjectSpace_ObjectsGetting;
                nonPersistentObjectSpace.ObjectByKeyGetting += NonPersistentObjectSpace_ObjectByKeyGetting;
                nonPersistentObjectSpace.ObjectGetting += NonPersistentObjectSpace_ObjectGetting;
                nonPersistentObjectSpace.Committing += NonPersistentObjectSpace_Committing;
                ObjectSpace.CustomRefresh += ObjectSpace_CustomRefresh;

                ObjectSpace.Refresh();
            }
            //Application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
        }
        protected override void OnDeactivated()
        {
            if (ObjectSpace is NonPersistentObjectSpace nonPersistentObjectSpace)
            {
                nonPersistentObjectSpace.ObjectsGetting -= NonPersistentObjectSpace_ObjectsGetting;
                nonPersistentObjectSpace.ObjectByKeyGetting -= NonPersistentObjectSpace_ObjectByKeyGetting;
                nonPersistentObjectSpace.ObjectGetting -= NonPersistentObjectSpace_ObjectGetting;
                nonPersistentObjectSpace.Committing -= NonPersistentObjectSpace_Committing;
                ObjectSpace.CustomRefresh -= ObjectSpace_CustomRefresh;
            }

            base.OnDeactivated();
            //Application.ObjectSpaceCreated -= Application_ObjectSpaceCreated;
        }
        
    }
}
