using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;

namespace categories.Module.BusinessObjects
{
    [DomainComponent]
    [DefaultClassOptions]
    //[ImageName("BO_Unknown")]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class NPCategory : IXafEntityObject, IObjectSpaceLink, INotifyPropertyChanged, ICanEditInLIne, INonPersistent
    {
        private IObjectSpace objectSpace;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public NPCategory()
        {
           // Oid = Guid.NewGuid();

        }

        [DevExpress.ExpressApp.Data.Key]
        [Browsable(false)]  // Hide the entity identifier from UI.
       // public Guid Oid { get; set; }
       public int Id { get; set; }
        public string CategoryName { get; set; }
        public string PartName { get; set; }
         
        public virtual MPart MPart { get; set; }
        private int MPart_Id { get; set; }
      
        [XafDisplayName("MyPartId"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "00"), VisibleInListView(false)]
        [RuleRequiredField(DefaultContexts.Save)]
        public int PartId
        {
            get { return MPart_Id; }
            set
            {
                if (MPart_Id!= value)
                {
                    MPart_Id  = value;
                    OnPropertyChanged();
                }
            }
        }

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.SampleProperty = "Paid";
        //}

        #region IXafEntityObject members (see https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppIXafEntityObjecttopic.aspx)
        void IXafEntityObject.OnCreated()
        {
            // Place the entity initialization code here.
            // You can initialize reference properties using Object Space methods; e.g.:
            // this.Address = objectSpace.CreateObject<Address>();
        }
        void IXafEntityObject.OnLoaded()
        {
            // Place the code that is executed each time the entity is loaded here.
            MPart = DataGetters.GetMPart(MPart_Id, objectSpace);
        }
        void IXafEntityObject.OnSaving()
        {
            // Place the code that is executed each time the entity is saved here.
            DataPutters.PutMCategory(this,(NonPersistentObjectSpace)objectSpace);
        }
        #endregion

        #region IObjectSpaceLink members (see https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppIObjectSpaceLinktopic.aspx)
        // Use the Object Space to access other entities from IXafEntityObject methods (see https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113707.aspx).
        IObjectSpace IObjectSpaceLink.ObjectSpace
        {
            get { return objectSpace; }
            set { objectSpace = value; }
        }
        #endregion

        #region INotifyPropertyChanged members (see http://msdn.microsoft.com/en-us/library/system.componentmodel.inotifypropertychanged(v=vs.110).aspx)
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public static List<INonPersistent> GetData(IObjectSpace space)
        {
            return DataGetters.GetNPCategories().OfType<INonPersistent>().ToList();
        }
    }
}