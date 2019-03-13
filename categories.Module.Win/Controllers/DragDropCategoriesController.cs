using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using categories.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.TreeListEditors.Win;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;

namespace categories.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DragDropCategoriesController : ViewController<ListView>
    {
        public DragDropCategoriesController()
        {
            InitializeComponent();
            TargetObjectType = typeof(MCategory);
            TargetViewType = ViewType.ListView;
        }
        private void View_EditorChanged(object sender, EventArgs e)
        {
            SetupEditor();
        }

        private void SetupEditor()
        {
            if (View.Editor == null) return;
            View.Editor.ControlsCreated += Editor_ControlsCreated;
         
            var editor = View.Editor as TreeListEditor;

            SetupDragDrop();
        }
        private void Editor_ControlsCreated(object sender, EventArgs e)
        {
            SetupDragDrop();
        }
        private void SetupDragDrop()
        {
            if (!(View.Editor is TreeListEditor editor) || editor.TreeList == null) return;
            var treeList = editor.TreeList;

            treeList.AllowDrop = false;  // setting this to true prevents any drag drop icon

            treeList.OptionsDragAndDrop.DragNodesMode = DragNodesMode.Multiple; // this is required to turn on drag drop
            treeList.OptionsDragAndDrop.DropNodesMode = DropNodesMode.Advanced;
            treeList.DragObjectDrop += TreeList_DragObjectDrop;
            treeList.AfterDropNode += TreeList_AfterDropNode;
            treeList.AfterDragNode += TreeList_AfterDragNode;
            UpdateNodesPositions(treeList.Nodes);
        }

        private void TreeList_AfterDragNode(object sender, AfterDragNodeEventArgs e)
        {
            SaveNewRecordPosition(e);
        }
        private void SaveNewRecordPosition(NodeEventArgs e)
        {
            var nodes = e.Node.ParentNode == null ? e.Node.TreeList.Nodes
                : e.Node.ParentNode.Nodes;
            for (var i = 0; i < nodes.Count; i++)
            {
                nodes[i].SetValue("SortId", i);
                var categorory = nodes[i].Tag as MCategory;
                categorory.SortId = i;
            }
            View.ObjectSpace.CommitChanges();
        }
        private void UpdateNodesPositions(TreeListNodes nodes)
        {
            var ns = new List<TreeListNode>();
            foreach (TreeListNode n in nodes)
            {
                ns.Add(n);
            }
            foreach (TreeListNode n in ns)
            {
                UpdateNodesPositions(n.Nodes);
                n.TreeList.SetNodeIndex(n, Convert.ToInt32(n.GetValue("SortId")));
            }
        }
        private void TreeList_DragObjectDrop(object sender, DragObjectDropEventArgs e)
        {
            // never fires
            var o = e.DragObject;
        }

        private void TreeList_AfterDropNode(object sender, AfterDropNodeEventArgs e)
        {
            if (e.DestinationNode != null)
            {
                DropNodes(e.Node, e.DestinationNode); // works but temporarily displays double nodes
            }

        }

        private string GetNodeName(TreeListNode node)
        {
            try
            {
                if (!(node?.Tag is MCategory cat)) return "";
                return cat.Name;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }

        private void DropNodes(TreeListNode sourceNode, TreeListNode droppedOnNode)
        {

            if (!(View.Editor is TreeListEditor editor) || editor.TreeList == null) return;
            var treeList = editor.TreeList;
           
            var sourceNodeName = GetNodeName(sourceNode);
            var prevVisibleNodeName = GetNodeName(sourceNode.PrevVisibleNode);
            var droppedOnNodeName= GetNodeName(droppedOnNode);
          
            droppedOnNode.Expand();

            var droppedOnCategory = droppedOnNode.Tag as MCategory;
            var sourceCategory = sourceNode.Tag as MCategory;
            sourceCategory.Parent = droppedOnCategory; // for a blue icon it will be dropping on the parent of the highlighted node
            var prevCategory = sourceNode.PrevVisibleNode.Tag as MCategory;
            //var newName = prevCategory.Name + "-" + sourceCategory.Name;
            //sourceCategory.Name = newName;
            View.ObjectSpace.CommitChanges();
            treeList.RefreshNode(sourceNode);
            treeList.RefreshNode(droppedOnNode);


        }
        protected override void OnActivated()
        {
            base.OnActivated();
            IModelListView model = View.Model;
            model.Sorting.ClearNodes();
            foreach (IModelColumn col in model.Columns)
            {
                col.SortIndex = -1;
                col.SortOrder = DevExpress.Data.ColumnSortOrder.None;
            }
            View.EditorChanged += View_EditorChanged;
            SetupEditor();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            if (!(View.Editor is TreeListEditor editor) || editor.TreeList == null) return;
            var treeList = editor.TreeList;

            treeList.DragObjectDrop -= TreeList_DragObjectDrop;
            treeList.AfterDropNode -= TreeList_AfterDropNode;
            treeList.AfterDragNode -= TreeList_AfterDragNode;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
