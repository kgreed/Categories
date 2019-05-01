using System;
using DevExpress.ExpressApp;
 
using System.Collections.Generic;
 
using DevExpress.ExpressApp.EF;
using System.Linq;
namespace categories.Module.BusinessObjects
{
    public static class DataPutters
    {
        public static void PutMCategory(NPCategory nPCategory, NonPersistentObjectSpace objectSpace)
        {
            var persistentObjectSpace = objectSpace.AdditionalObjectSpaces.FirstOrDefault();
            using (var connect = MakeConnect(persistentObjectSpace))
            {
                var category =connect.Categories.Find(nPCategory.Id);
                var part = connect.Parts.Find(nPCategory.PartId);
                category.MPart = part;
               // connect.Entry(category).State = System.Data.Entity.EntityState.Modified;
                connect.SaveChanges();

            }
        }

        public static MyDbContext MakeConnect(IObjectSpace objectSpace)
        {
            var oc = ((EFObjectSpace)objectSpace).ObjectContext;
            return new MyDbContext(oc);

        }
    }
}