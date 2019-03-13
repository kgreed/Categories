using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Entity;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.Data.Helpers;
using DevExpress.ExpressApp.EF.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;

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
		public DbSet<ModuleInfo> ModulesInfo { get; set; }
		//public DbSet<HCategory> HCategories { get; set; }
        public DbSet<MCategory> Categories { get; set; }
	}

    public class MyInitializer : CreateDatabaseIfNotExists<MyDbContext>
    {
        protected override void Seed(MyDbContext context)
        {
            SeedRecords(context);

            base.Seed(context);
        }

        public static void SeedRecords(MyDbContext context)
        {
            for (var i = 0; i < 20; i++)
            {
                var cat = new MCategory {Name = $"A{i:D2}"};
                for (var j = 0; j < 10; j++)
                {
                    var child = new MCategory {Name = $"A{i:D2}-B{j:D2}",SortId = j};
                    cat.Children.Add(child);
                }

                context.Categories.Add(cat);
            }

            context.SaveChanges();
        }
    }

    //https://www.devexpress.com/Support/Center/Question/Details/T171380/how-to-implement-a-self-referencing-entity-for-further-use-with-tree-list-editors-based
    [NavigationItem("Home")]
    public class MCategory : IHCategory
    {
        public MCategory()
        {
            Children = new BindingList<MCategory>();
        }
        [Browsable(false)]
        [Key]
        public Int32 ID { get; protected set; }
        public String Name { get; set; }
        public int SortId { get; set; }
        public virtual MCategory Parent { get; set; }
        public virtual IList<MCategory> Children { get; set; }
        [NotMapped, Browsable(false), RuleFromBoolProperty("HCategoryCircularReferences", DefaultContexts.Save, "Circular refrerence detected. To correct this error, set the Parent property to another value.", UsedProperties = "Parent")]
        public Boolean IsValid
        {
            get
            {
                MCategory currentObj = Parent;
                while (currentObj != null)
                {
                    if (currentObj == this)
                    {
                        return false;
                    }
                    currentObj = currentObj.Parent;
                }
                return true;
            }
        }
        IBindingList ITreeNode.Children => Children as IBindingList;

        ITreeNode IHCategory.Parent
        {
            get => Parent as IHCategory;
            set => Parent = value as MCategory;
        }
        ITreeNode ITreeNode.Parent => Parent as ITreeNode;
    }
}

    
 