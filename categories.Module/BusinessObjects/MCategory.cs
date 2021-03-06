﻿using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using KeyAttribute = DevExpress.ExpressApp.Data.KeyAttribute;
using RequiredAttribute = DevExpress.ExpressApp.Model.RequiredAttribute;

namespace categories.Module.BusinessObjects
{
    [NavigationItem("Home")]
    public class MCategory : IHCategory
    {
        public MCategory()
        {
            Children = new BindingList<MCategory>();
        }
        [Browsable(false)]
        [Key]
        public Int32 ID { get; set; }

       // [System.ComponentModel.DataAnnotations.Required]  wrong, use XAF
       [Required]
        [VisibleInDetailView(true)]
        public virtual MPart MPart { get; set; }

        // public string PartName => MPart?.Name;   
        [ModelDefault("Caption","CatName")]
        public String Name { get; set; }
        //[Browsable(false)]  causes error when I try to sort
        public int SortId { get; set; }
        [Browsable(false)]
        public virtual MCategory Parent { get; set; }
        public virtual IList<MCategory> Children { get; set; }
        [NotMapped]
        [Browsable(false)]
        public bool Expanded { get; set; }
        [NotMapped, Browsable(false), RuleFromBoolProperty("HCategoryCircularReferences", DefaultContexts.Save, "Circular reference detected. To correct this error, set the Parent property to another value.", UsedProperties = "Parent")]
        public Boolean IsValid
        {
            get
            {
                var currentObj = Parent;
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