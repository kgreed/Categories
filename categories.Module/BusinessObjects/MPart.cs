using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;

namespace categories.Module.BusinessObjects
{
    [NavigationItem("Home")]
    [DefaultProperty("Name")]
    public class MPart
    {
        public MPart() {
            Categories= new BindingList<MCategory>();  
        }

        [Browsable(false)]
        [Key]
        public int Id { get; set; }
        [ModelDefault("Caption", "PartName")]
        public string Name { get; set; }

        public int Priority { get; set; }

        [NotMapped]
        public bool TagToSetPriority { get; set; }

        [Aggregated]
        public IList<MCategory> Categories { get; set; }
    }
}