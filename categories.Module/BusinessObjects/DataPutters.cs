using System;
using DevExpress.ExpressApp;
 
using System.Collections.Generic;
 
using DevExpress.ExpressApp.EF;
using System.Linq;
namespace categories.Module.BusinessObjects
{
    public static class DataPutters
    {
        public static void PutMCategory(NPCategory nPCategory, IObjectSpace objectSpace)
        {
            using (var connect = MakeConnect(objectSpace))
            {
               var category =connect.Categories.Find(nPCategory.Id);
                category.MPart = nPCategory.MPart;
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