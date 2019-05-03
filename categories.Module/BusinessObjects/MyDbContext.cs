using System;
using System.Data;
using System.Linq;
using System.Data.Entity;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using DevExpress.Data.Helpers;
using DevExpress.ExpressApp.EF.Updating;
using DevExpress.Persistent.BaseImpl.EF;

namespace categories.Module.BusinessObjects {
	public class MyDbContext : DbContext {
        

        public MyDbContext(String connectionString)
			: base(connectionString) {
            Database.SetInitializer(new MyInitializer());
		}

        
        
        public MyDbContext(DbConnection connection)
			: base(connection, false) {
            Database.SetInitializer(new MyInitializer());
        }
		public MyDbContext()
			: base("name=ConnectionString") {
            Database.SetInitializer(new MyInitializer());
        }

        public MyDbContext(ObjectContext oc) : base(oc,false)
        {
             
        }

        public DbSet<ModuleInfo> ModulesInfo { get; set; }
		//public DbSet<HCategory> HCategories { get; set; }
        public DbSet<MCategory> Categories { get; set; }

        public DbSet<MPart> Parts { get; set; }
	}

    //https://www.devexpress.com/Support/Center/Question/Details/T171380/how-to-implement-a-self-referencing-entity-for-further-use-with-tree-list-editors-based
}

    
 