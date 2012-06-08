using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Reflection;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;
using ZoneFiveSoftware.Common.Visuals.Forms;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data;
using CompareView.Properties;

namespace CompareView
{
    public partial class CompareViewControl : UserControl
    {
        ITheme m_visualTheme;
        List<CVTreeListEntry> m_CVTreeListEntries = new List<CVTreeListEntry>();
        List<TreeList.TreeListNode> m_CVWeeklyTreeListNodes = new List<TreeList.TreeListNode>();

        Guid ActivityReportsViewGuid = new Guid("99498256-cf51-11db-9705-005056c00008");
        Guid DailyActivityViewGuid = new Guid("1dc82ca0-88aa-45a5-a6c6-c25f56ad1fc3"); 
        private string MyActivitiesCatRefId = "fa756214-cf71-11db-9705-005056c00008";

        private bool PlannedActivity(IActivityCategory actCat)
        {
            while (actCat.Parent != null)
            {
                actCat = actCat.Parent;
            }
            bool boPlannedActivity = !actCat.ReferenceId.Equals(MyActivitiesCatRefId);
            return boPlannedActivity;
        }

        private int GetWeekOfYear(DateTime dt)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            return ciCurr.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek);
        }

        private DateTime GetFirstDayOfWeek(DateTime dt)
        {
            int weekDay = (int)dt.DayOfWeek;
            return dt.Subtract(new TimeSpan(weekDay - 1, 0, 0, 0));
        }

        private TreeList.TreeListNode ShowBestEntryForDate(IList<TreeList.TreeListNode> nodeList, DateTime date)
        {
            for (int i = nodeList.Count - 1; i >= 0; i--)
            {
                TreeList.TreeListNode tn = nodeList[i];
                if (tn.Element != null)
                {
                    CVTreeListEntry entry = (CVTreeListEntry)tn.Element;
                    if (entry.Date.CompareTo(date) <= 0)
                    {
                        // If any children exists, search those for the best activity, otherwise return this node
                        if (tn.Children.Count > 0)
                        {
                            TreeList.TreeListNode bestChild = ShowBestEntryForDate(tn.Children, date);
                            System.Collections.IList expanded = CompareTreeList.Expanded;
                            expanded.Add(tn);
                            CompareTreeList.Expanded = expanded;
                            if (bestChild == null)
                            {
                                CompareTreeList.SelectedItems = new List<TreeList.TreeListNode>(new TreeList.TreeListNode[] { tn });
                                CompareTreeList.EnsureVisible(tn);
                                return tn;
                            }
                            else
                                return bestChild;
                        }
                        else
                        {
                            CompareTreeList.SelectedItems = new List<TreeList.TreeListNode>(new TreeList.TreeListNode[] { tn });
                            CompareTreeList.EnsureVisible(tn);
                            return tn;
                        }
                    }
                }
            }
            return null;

        }

        private void CalenderDateChangedEventHandler(object ob, EventArgs args)
        {
            ShowBestEntryForDate((IList<TreeList.TreeListNode>)CompareTreeList.RowData, Plugin.GetApplication().Calendar.Selected);
        }
        
        public CompareViewControl()
        {
            InitializeComponent();

            //TreeList.Column column = new TreeList.Column("StartTime", CommonResources.Text.LabelDate, 100, StringAlignment.Near);
            //CompareTreeList.Columns.Add(column);
            //column = new TreeList.Column("Time", CommonResources.Text.LabelTime, 70, StringAlignment.Near);
            //CompareTreeList.Columns.Add(column);
            //column = new TreeList.Column("DistanceMeters", CommonResources.Text.LabelDistance, 70, StringAlignment.Near);
            //CompareTreeList.Columns.Add(column);

            //column = new TreeList.Column("PlannedTime", Resources.Planned + " " + CommonResources.Text.LabelTime, 70, StringAlignment.Near);
            //CompareTreeList.Columns.Add(column);
            //column = new TreeList.Column("PlannedDistanceMeters", Resources.Planned + " " + CommonResources.Text.LabelDistance, 70, StringAlignment.Near);
            //CompareTreeList.Columns.Add(column);
            
            //CompareTreeList.CheckBoxes = true;
            RefreshColumns();
            CompareTreeList.RowDataRenderer.RowAlternatingColors = true;
            CompareTreeList.LabelProvider = new CVLabelProvider();

            Plugin.GetApplication().Calendar.SelectedChanged += new EventHandler(CalenderDateChangedEventHandler);
            //CompareTreeList.CheckedChanged += new TreeList.ItemEventHandler(treeView_CheckedChanged);
            //Plugin.GetApplication().Logbook.Activities.CollectionChanged += new NotifyCollectionChangedEventHandler<IActivity>(UpdateControlEventHandler);

        }

        public void ThemeChanged(ITheme visualTheme)
        {
            m_visualTheme = visualTheme;

            this.CompareTreeList.ThemeChanged(visualTheme);
            //Non ST controls
            //this.panelAct.BackColor = visualTheme.Control;
            //this.panel3.BackColor = visualTheme.Control;
            this.BackColor = visualTheme.Control;
        }

        private void RefreshColumns()
        {
            CompareTreeList.Columns.Clear();
            foreach (string id in Settings.CompareTreeListColumns)
            {
                foreach (IListColumnDefinition columnDef in CompareColumnIds.ColumnDefs())
                {
                    if (columnDef.Id == id)
                    {
                        TreeList.Column column = new TreeList.Column(
                            columnDef.Id,
                            columnDef.Text(columnDef.Id),
                            columnDef.Width,
                            columnDef.Align
                        );
                        CompareTreeList.Columns.Add(column);
                        break;
                    }
                }
            }
        }
        

        public void UpdateData()
        {
            if (Plugin.GetApplication().Logbook != null)
            {
                // Enter planned and actual activities into appropriate row elements
                m_CVTreeListEntries.Clear();
                List<IActivity> activities = new List<IActivity>(Plugin.GetApplication().Logbook.Activities);
                activities.Sort(new ActivityComparer());
                CVTreeListEntry entry = null;
                while (activities.Count > 0)
                {
                    bool boPlannedActivity = PlannedActivity(activities[0].Category);
                    if (entry == null ||
                        !entry.Date.ToLocalTime().Date.Equals(activities[0].StartTime.Date) ||
                        boPlannedActivity && entry.PlannedActivity != null ||
                        !boPlannedActivity && entry.Activity != null)
                    {
                        if (entry != null)
                            m_CVTreeListEntries.Add(entry);

                        entry = new CVTreeListEntry();
                        entry.Date = activities[0].StartTime.ToLocalTime().Date;
                    }

                    if (boPlannedActivity)
                        entry.PlannedActivity = activities[0];
                    else
                        entry.Activity = activities[0];
                    
                    activities.RemoveAt(0);
                }
                if (entry != null)
                    m_CVTreeListEntries.Add(entry);
                
                // Create weekly groups
                m_CVWeeklyTreeListNodes.Clear();
                List<CVTreeListEntry> entries = new List<CVTreeListEntry>(m_CVTreeListEntries);
                CVTreeListEntry wkEntry = null; //entry for the top node containing the week
                entry = null; //entry for the child node containing the individual workouts
                TreeList.TreeListNode node = null;
                while (entries.Count > 0)
                {
                    if (node != null)
                        wkEntry = (CVTreeListEntry)node.Element;
                    else
                        wkEntry = null;

                    if (wkEntry == null ||
                        GetWeekOfYear(wkEntry.Date) != GetWeekOfYear(entries[0].Date) ||
                        wkEntry.Date.Year != entries[0].Date.Year)
                    {
                        if (wkEntry != null)
                            m_CVWeeklyTreeListNodes.Add(node);

                        wkEntry = new CVTreeListEntry();

                        wkEntry.Date = GetFirstDayOfWeek(entries[0].Date);
                        //wkEntry.Date = entries[0].Date; //TODO: Enter the first day of the week here instead!
                        node = new TreeList.TreeListNode(null, wkEntry);
                    }
                    entry = entries[0];
                    node.Children.Add(new TreeList.TreeListNode(node, entry));
                    entries.RemoveAt(0);
                }
                if (wkEntry != null)
                    m_CVWeeklyTreeListNodes.Add(node);
                
                // Insert results into the TreeList
                CompareTreeList.RowData = m_CVWeeklyTreeListNodes;
                //CompareTreeList.Expanded = m_CVWeeklyTreeListNodes;

                DateTime calenderDT = Plugin.GetApplication().Calendar.Selected;
                ShowBestEntryForDate(m_CVWeeklyTreeListNodes, calenderDT);

                //// Use ST calender as a base to know what date the user is interested in
                //DateTime calenderDT = Plugin.GetApplication().Calendar.Selected;
                //for (int i = m_CVWeeklyTreeListNodes.Count - 1; i >= 0; i--)
                //{
                //    TreeList.TreeListNode tn = m_CVWeeklyTreeListNodes[i];
                //    if (tn.Element != null)
                //    {
                //        entry = (CVTreeListEntry)tn.Element;
                //        if (entry.Date.CompareTo(calenderDT) <= 0)
                //        {
                //            CompareTreeList.Expanded = new List<TreeList.TreeListNode>(new TreeList.TreeListNode[] {tn});
                //            CompareTreeList.SelectedItems = new List<TreeList.TreeListNode>(new TreeList.TreeListNode[] { tn });
                //            CompareTreeList.EnsureVisible(tn);
                //            break;
                //        }
                //    }
                //}
            }
        }

        private class ActivityComparer : IComparer<IActivity>
        {
            #region IComparer<IActivity> Members

            public int Compare(IActivity x, IActivity y)
            {
                return x.StartTime.CompareTo(y.StartTime);
            }

            #endregion
        }

        private void tableSettingsMenuItem_Click(object sender, EventArgs e)
        {
            ListSettingsDialog dialog = new ListSettingsDialog();
            dialog.AvailableColumns = CompareColumnIds.ColumnDefs();
            dialog.ThemeChanged(m_visualTheme);
            dialog.AllowFixedColumnSelect = false;
            dialog.SelectedColumns = Settings.CompareTreeListColumns;
            dialog.NumFixedColumns = 3;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int numFixedColumns = dialog.NumFixedColumns;
                Settings.CompareTreeListColumns = dialog.SelectedColumns;
                RefreshColumns();
            }
        }

        private void ExpandAllMenuItem_Click(object sender, EventArgs e)
        {
            CompareTreeList.Expanded = m_CVWeeklyTreeListNodes;
            CompareTreeList.EnsureVisible(CompareTreeList.SelectedItems[0]);
        }

        private void CollapseAllMenuItem_Click(object sender, EventArgs e)
        {
            CompareTreeList.Expanded = new List<object>();
            CompareTreeList.EnsureVisible(CompareTreeList.SelectedItems[0]);
        }

        private void CompareTLMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            bool OverlayLoaded = false;
            GoToPlannedMenuItem.Enabled = false;
            GoToPerformedMenuItem.Enabled = false;
            SendToOverlayMenuItem.Enabled = false;

            foreach (IPlugin pi in Plugin.GetApplication().PluginExtensions.LoadedPlugins)
            {
                if (pi.Id.CompareTo(new Guid("{489FD22A-DB13-49DB-A77C-57E45CB1D049}")) == 0)
                    OverlayLoaded = true;
            }
            if (CompareTreeList.SelectedItems.Count > 0)
            {
                TreeList.TreeListNode tn = (TreeList.TreeListNode)CompareTreeList.SelectedItems[0];
                if (tn.Element != null)
                {
                    CVTreeListEntry entry = (CVTreeListEntry)tn.Element;
                    if (entry.PlannedActivity != null)
                    {
                        GoToPlannedMenuItem.Enabled = true;
                    }
                    if (entry.Activity != null)
                    {
                        GoToPerformedMenuItem.Enabled = true;
                    }
                }
            }
            if (OverlayLoaded && (GoToPlannedMenuItem.Enabled ||GoToPerformedMenuItem.Enabled))
            {
                SendToOverlayMenuItem.Enabled = true;
            }
        }

        private void GoToPlannedMenuItem_Click(object sender, EventArgs e)
        {
            if (CompareTreeList.SelectedItems.Count > 0)
            {
                TreeList.TreeListNode node = (TreeList.TreeListNode)CompareTreeList.SelectedItems[0];
                if (node.Element != null)
                {
                    CVTreeListEntry entry = (CVTreeListEntry)node.Element;
                    if (entry.PlannedActivity != null)
                        Plugin.GetApplication().ShowView(ActivityReportsViewGuid, "id=" + entry.PlannedActivity.ReferenceId);
                }
            }
        }

        private void GoToPerformedMenuItem_Click(object sender, EventArgs e)
        {
            if (CompareTreeList.SelectedItems.Count > 0)
            {
                TreeList.TreeListNode node = (TreeList.TreeListNode)CompareTreeList.SelectedItems[0];
                if (node.Element != null)
                {
                    CVTreeListEntry entry = (CVTreeListEntry)node.Element;
                    if (entry.Activity != null)
                        Plugin.GetApplication().ShowView(ActivityReportsViewGuid, "id=" + entry.Activity.ReferenceId);
                }
            }
        }

        private void SendToOverlayMenuItem_Click(object sender, EventArgs e)
        {
            IList<IActivity> actList = new List<IActivity>();
            bool includePlanned = false;
            bool includePerformed = false;

            if (sender == SendPlannedToOverlayMenuItem || sender == SendAllToOverlayMenuItem)
                includePlanned = true;
            if (sender == SendPerformedToOverlayMenuItem || sender == SendAllToOverlayMenuItem)
                includePerformed = true;

            foreach (TreeList.TreeListNode tn in CompareTreeList.SelectedItems)
            {
                CVTreeListEntry entry = (CVTreeListEntry)tn.Element;
                if (includePlanned && entry.PlannedActivity != null)
                    actList.Add(entry.PlannedActivity);
                if (includePerformed && entry.Activity != null)
                    actList.Add(entry.Activity);
            }

            try
            {
                AppDomain currentDomain = AppDomain.CurrentDomain;
                Assembly[] assems = currentDomain.GetAssemblies();

                foreach (Assembly assem in assems)
                {
                    AssemblyName assemName = new AssemblyName((assem.FullName));
                    if (assemName.Name.Equals("OverlayPlugin"))
                    {
                        Type type = assem.GetType("GpsRunningPlugin.Source.OverlayView");
                        //Activator.CreateInstance(assem.GetType("GpsRunningPlugin.Source.OverlayView"), m_par);
                        Activator.CreateInstance(assem.GetType("GpsRunningPlugin.Source.OverlayView"), new object [] { actList, true });
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Problem when starting Overlay: "+ex.Message );
            }
            
        }
    }


    public class CVTreeListEntry
    {
        public DateTime Date;
        public IActivity Activity = null;
        public IActivity PlannedActivity = null;
    }
}
