using System.Data.Entity;

namespace categories.Module.BusinessObjects
{
    public class MyInitializer :DropCreateDatabaseIfModelChanges<MyDbContext>
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
                var ch = (char)(65+i);
                var cat = new MCategory {Name = $"{ch}{i:D2}", MPart = new MPart {Name = $"Name{i}", Priority = i}};

                for (var j = 0; j < 10; j++)
                {
                    var child = new MCategory {Name = $"{ch}{j:D2}", SortId = j, MPart = cat.MPart};
                    cat.Children.Add(child);
                }
                
                context.Categories.Add(cat);
            }

            
            context.SaveChanges();

            //for (var i = 0; i < 100; i++)
            //{
            //    var part = new MPart {Name = $"Name{i}", Priority = i};
            //    context.Parts.Add(part);
            //}

            //context.SaveChanges();
        }
    }
}