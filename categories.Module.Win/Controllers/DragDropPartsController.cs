using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using categories.Module.BusinessObjects;

namespace categories.Module.Win.Controllers
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using DevExpress.Data;
    using DevExpress.Data.Helpers;
    using DevExpress.ExpressApp;
    using DevExpress.ExpressApp.Win.Editors;
    using DevExpress.Utils.Behaviors;
    using DevExpress.Utils.DragDrop;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraPrinting.Native;
   
    using ListView = DevExpress.ExpressApp.ListView;

    namespace VIV.JobTalk2.Module.Win.Controllers.ToDoList
    {
        public partial class DragNDropController : ViewController<ListView>
        {
            private BehaviorManager behaviorManager1;
            DragDropBehavior behaviorField = null;

            public DragNDropController()
            {
                

                TargetObjectType = typeof(MPart);
                TargetViewType = ViewType.ListView;
            }

            protected override void OnActivated()
            {
                base.OnActivated();
                View.EditorChanged += View_EditorChanged;
                SetupEditor();
            }

            protected override void OnDeactivated()
            {

                if (behaviorField != null)
                {
                    behaviorField.DragOver -= Behavior_DragOver;
                    behaviorField.DragDrop -= Behavior_DragDrop;
                    behaviorField.BeginDragDrop -= Behavior_BeginDragDrop;
                }

                View.EditorChanged -= View_EditorChanged;
                base.OnDeactivated();
            }

            private void View_EditorChanged(object sender, EventArgs e)
            {
                SetupEditor();

            }

            private void SetupEditor()
            {
                if (View.Editor == null) return;
                View.Editor.ControlsCreated += Editor_ControlsCreated;

            }

            private void Editor_ControlsCreated(object sender, EventArgs e)
            {

                if (!(View.Editor is GridListEditor editor) || editor.GridView == null) return;

                editor.Grid.AllowDrop = false;
                behaviorManager1 = new BehaviorManager();

                behaviorManager1.Attach(editor.GridView, MyBehaviorSettings());

            }

            private Action<DragDropBehavior> MyBehaviorSettings()
            {
                return behavior =>
                {
                    behavior.Properties.AllowDrop = true;
                    behavior.Properties.InsertIndicatorVisible = true;
                    behavior.Properties.PreviewVisible = true;
                    behavior.DragOver += Behavior_DragOver;
                    behavior.DragDrop += Behavior_DragDrop;
                    behavior.BeginDragDrop += Behavior_BeginDragDrop;
                    behaviorField = behavior; // so we can release the events later
                };
            }

            private void Behavior_DragOver(object sender, DragOverEventArgs e)
            {
                var args = DragOverGridEventArgs.GetDragOverGridEventArgs(e);
                e.InsertType = args.InsertType;
                e.InsertIndicatorLocation = args.InsertIndicatorLocation;
                e.Action = args.Action;
                Cursor.Current = args.Cursor;
                args.Handled = true;
            }

            private void Behavior_BeginDragDrop(object sender, BeginDragDropEventArgs e)
            {
                if (!(View.Editor is GridListEditor editor) || editor.GridView == null) return;

                var sortedColumn = editor.GridView.SortedColumns.FirstOrDefault();
                if (sortedColumn != null &&
                    (sortedColumn.Name != "Priority" || sortedColumn.SortOrder != ColumnSortOrder.Ascending))
                    MessageBox.Show("You need to be sorted by Priority (Ascending) to use prioritization");
            }


            private void Behavior_DragDrop(object sender, DragDropEventArgs e)
            {
                //https://www.devexpress.com/Support/Center/Question/Details/T583428/how-to-use-new-dragdropbehavior-in-xaf

                try
                {


                    var targetGrid = e.Target as GridView;


                    if (e.Action == DragDropActions.None)
                        return;
                    if (targetGrid == null) return;

                    try
                    {
                        RePrioritiseIfNeeded();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        MessageBox.Show(exception.ToString());
                        throw;
                    }



                    var hitPoint = targetGrid.GridControl.PointToClient(Cursor.Position);
                    var hitInfo = targetGrid.CalcHitInfo(hitPoint);

                    var droppedOnRowHandle = hitInfo.RowHandle;
                    var droppedOnRow = targetGrid.GetRow(droppedOnRowHandle);


                    if (!(droppedOnRow is MPart droppedOnTask))

                    {
                        Debug.Print("huh?");
                        return;
                    }


                    var newPriority = 0;

                    switch (e.InsertType)
                    {
                        case InsertType.Before:
                            var rowBefore = targetGrid.GetRow(droppedOnRowHandle - 1);
                            switch (rowBefore)
                            {
                                case null:
                                    newPriority = droppedOnTask.Priority - 1;
                                    break;
                                case MPart prevTaskBeforeDroppedOn:
                                    var diff = droppedOnTask.Priority - prevTaskBeforeDroppedOn.Priority;
                                    newPriority = Convert.ToInt32(droppedOnTask.Priority - diff / 2);
                                    break;
                            }


                            break;
                        case InsertType.After:
                            var rowAfter = targetGrid.GetRow(droppedOnRowHandle + 1);
                            switch (rowAfter)
                            {
                                case null:
                                    newPriority = droppedOnTask.Priority + 1;
                                    break;
                                case MPart nextTaskAfterDroppedOn:
                                    var diff = nextTaskAfterDroppedOn.Priority - droppedOnTask.Priority;
                                    newPriority = Convert.ToInt32(droppedOnTask.Priority + diff / 2);
                                    break;
                            }

                            break;
                    }


                    TagToSetPriority(targetGrid, newPriority);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }

            private void TagToSetPriority(GridView view, int newPriority)
            {
                var selectedRows = view.GetSelectedRows();
                var offset = 0;

                foreach (var rowNum in selectedRows)
                {
                    var row = view.GetRow(rowNum);
                    if (!(row is MPart task)) throw new Exception("Expected task");
                    task.Priority = newPriority + offset;
                    task.TagToSetPriority = true;
                    offset = offset + 10;
                }

                SaveTasksAndRefresh();
            }

            private void SaveTasksAndRefresh()
            {
                try
                {
                    var rows = GetOrderedRows();
                    if (rows == null) return;
                    //var nowTime = HandyBusinessFunctions.GetServerTime();
                    //var staffId = HandyBusinessFunctions.GetStaffIdForLoggedInUser();

                    using (var connect = new MyDbContext())
                    {
                        foreach (var row in rows)
                        {
                            var task = (MPart)row;
                            var t = connect.Parts.SingleOrDefault(x => x.Id == task.Id);
                            if (t == null) continue;
                            // if (t.TaskPriority == task.Priority) continue;
                            if (!task.TagToSetPriority) continue;
                            t.Priority = task.Priority;
                            //t.TaskPrioritedByStaffId = staffId;
                            //t.TaskPrioritisedAt = nowTime;
                        }
                        connect.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    Console.WriteLine(e);
                    throw;
                }
                View.Refresh();
            }

            private bool NeedsRePrioritization()
            {
                var rows = GetOrderedRows();
                if (rows == null) return false;
                decimal prevPriority = 0;
                foreach (var rec in rows)
                {
                    if (!(rec is MPart task)) throw new Exception("Unexpected type in view");
                    if (task.Priority <= 0 || task.Priority == prevPriority) return true;
                    prevPriority = task.Priority;
                }

                return false;
            }

            private IList GetOrderedRows()
            {
                if (!(View.Editor is GridListEditor editor) || editor.GridView == null) return null;
                var rows = editor.GetOrderedObjects();
                return rows;
            }

            private void RePrioritiseIfNeeded()
            {
                if (!NeedsRePrioritization()) return;
                var listRows = GetOrderedRows(); // the order displayed
                                                 //var oldTasks = rows.ConvertAll(x => (TaskResult) x).ToList().Where(x => x?.Priority != null)
                                                 //    .Where(x => x.Priority > 0).OrderBy(x=>x.IsDone).ThenBy(x => x.ReadyByDate).ThenByDescending(x => x.Promised).ToList();
                var oldTasks = listRows.ConvertAll(x => (MPart)x).ToList().Where(x => x?.Priority != null)
                    .Where(x => x.Priority > 0);
                // var hasOldTasks = oldTasks.Any();
                //var i = 1;
                if (listRows == null) return;
                var rows = listRows.ConvertAll(x => (MPart)x);
                var taskResults = rows as MPart[] ?? rows.ToArray();
                //var undoneRows = taskResults.Where(x => !x.IsDone).OrderBy(x => x.Priority);
                //foreach (var row in undoneRows)
                //{
                //    // var priority = row?.Priority ?? 0;
                //    // if (priority == 0 && hasOldTasks) continue;
                //    // if (row == null) continue;
                //    row.Priority = i * 1000;
                //    row.TagToSetPriority = true;
                //    i++;
                //}
                //var doneRows = taskResults.Where(x => x.IsDone).OrderBy(x => x.Priority);
                //foreach (var row in doneRows)
                //{
                //    //  var priority = row?.Priority ?? 0;
                //    //  if (priority == 0 && hasOldTasks) continue;
                //    //  if (row == null) continue;
                //    row.Priority = i * 1000;
                //    row.TagToSetPriority = true;
                //    i++;
                //}

                SaveTasksAndRefresh();
            }

             
        }
    }
}
