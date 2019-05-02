using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.EF;
using System.Linq;

namespace categories.Module.BusinessObjects
{
    public static class DataGetters
    {
        public static MCategory GetMCategory(int categoryId, IObjectSpace objectSpace)
        {
            using (var connect = MakeConnect(objectSpace))
            {
                return connect.Categories.Find(categoryId);
            }
        }

        public static MyDbContext MakeConnect(IObjectSpace objectSpace)
        {
            var oc = ((EFObjectSpace)objectSpace).ObjectContext;
            return new MyDbContext(oc);

        }

        public static List<NPCategory> GetNPCategories()
        {
            using (var connect = new MyDbContext())
            {

                const string sql = "select c.ID , MPart_Id ,c.Name as CategoryName, p.Name as PartName from MCategories c inner join MParts p on c.mpart_Id = p.Id";
                var results = connect.Database.SqlQuery<NPCategory>(sql).ToList();
                return results;

            }
        }

        internal static MPart GetMPart(int partId, IObjectSpace objectSpace)
        {
            using (var connect = MakeConnect(objectSpace))
            {
                return connect.Parts.Find(partId);
            }
        }
    }
}