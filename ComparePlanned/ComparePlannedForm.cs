/*
Copyright (C) 2012 Staffan Nilsson

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 3 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library. If not, see <http://www.gnu.org/licenses/>.
 */
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
        List<TreeList.TreeListNode> m_CVTreeListNodes = new List<TreeList.TreeListNode>(); // The top nodes to in the TreeList.

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

            RefreshColumns();
            CompareTreeList.RowDataRenderer.RowAlternatingColors = true;
            CompareTreeList.LabelProvider = new CVLabelProvider();

            Plugin.GetApplication().Calendar.SelectedChanged += new EventHandler(CalenderDateChangedEventHandler);
            //Plugin.GetApplication().Logbook.Activities.CollectionChanged += new NotifyCollectionChangedEventHandler<IActivity>(UpdateControlEventHandler);

        }

        public void ThemeChanged(ITheme visualTheme)
        {
            m_visualTheme = visualTheme;

            this.CompareTreeList.ThemeChanged(visualTheme);
            //Non ST controls
            //example: this.panelAct.BackColor = visualTheme.Control;
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
                // Enter planned and actual activities into TreeNodes
                m_CVTreeListNodes.Clear();
                List<IActivity> activities = new List<IActivity>(Plugin.GetApplication().Logbook.Activities);
                List<DateTime> allPerfActivityDates = new List<DateTime>(); //Used to highlight the dates of all activites in the ST calendar
                List<DateTime> allPlannedActivityDates = new List<DateTime>(); //Used to highlight the dates of all activites in the ST calendar
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
                            m_CVTreeListNodes.Add(new TreeList.TreeListNode(null, entry));

                        entry = new CVTreeListEntry();
                        entry.Date = activities[0].StartTime.ToLocalTime().Date;
                    }

                    if (boPlannedActivity)
                    {
                        entry.PlannedActivity = activities[0];
                        allPlannedActivityDates.Add(entry.Date);
                    }
                    else
                    {
                        entry.Activity = activities[0];
                        allPerfActivityDates.Add(entry.Date);
                    }
                    
                    activities.RemoveAt(0);
                }
                if (entry != null)
                    m_CVTreeListNodes.Add(new TreeList.TreeListNode(null, entry));
                Plugin.GetApplication().Calendar.SetHighlightedDates(allPerfActivityDates); //Show the dates of all activities in the ST calendar
                Plugin.GetApplication().Calendar.SetMarkedDates(allPlannedActivityDates); //Show the dates of all activities in the ST calendar

                //If chosen, create daily groups
                if (Settings.boGroupDaily)
                {
                    List<TreeList.TreeListNode> nodes = new List<TreeList.TreeListNode>(m_CVTreeListNodes);
                    m_CVTreeListNodes.Clear();
                    CVTreeListEntry dayEntry = null; //entry for the top node containing the week
                    entry = null; //entry for the child node containing the individual workouts
                    TreeList.TreeListNode node = null;
                    while (nodes.Count > 0)
                    {
                        if (node != null)
                            dayEntry = (CVTreeListEntry)node.Element;
                        else
                            dayEntry = null;

                        entry = (CVTreeListEntry)nodes[0].Element;
                        if (dayEntry == null ||
                            !dayEntry.Date.Equals(entry.Date))
                        {
                            if (dayEntry != null)
                                m_CVTreeListNodes.Add(node);

                            dayEntry = new CVTreeListEntry();

                            dayEntry.Date = entry.Date;

                            node = new TreeList.TreeListNode(null, dayEntry);
                        }
                        nodes[0].Parent = node;
                        node.Children.Add(nodes[0]);
                        nodes.RemoveAt(0);
                    }
                    if (dayEntry != null)
                        m_CVTreeListNodes.Add(node);
                }

                //If chosen, create weekly groups
                if (Settings.boGroupWeekly)
                {
                    List<TreeList.TreeListNode> nodes = new List<TreeList.TreeListNode>(m_CVTreeListNodes);
                    m_CVTreeListNodes.Clear();
                    CVTreeListEntry wkEntry = null; //entry for the top node containing the week
                    entry = null; //entry for the child node containing the individual workouts
                    TreeList.TreeListNode node = null;
                    while (nodes.Count > 0)
                    {
                        if (node != null)
                            wkEntry = (CVTreeListEntry)node.Element;
                        else
                            wkEntry = null;

                        entry = (CVTreeListEntry)nodes[0].Element;
                        if (wkEntry == null ||
                            GetWeekOfYear(wkEntry.Date) != GetWeekOfYear(entry.Date) ||
                            wkEntry.Date.Year != entry.Date.Year)
                        {
                            if (wkEntry != null)
                                m_CVTreeListNodes.Add(node);

                            wkEntry = new CVTreeListEntry();

                            wkEntry.Date = GetFirstDayOfWeek(entry.Date);
                            node = new TreeList.TreeListNode(null, wkEntry);
                        }
                        nodes[0].Parent = node;
                        node.Children.Add(nodes[0]);
                        nodes.RemoveAt(0);
                    }
                    if (wkEntry != null)
                        m_CVTreeListNodes.Add(node);
                }

                // Insert results into the TreeList
                CompareTreeList.RowData = m_CVTreeListNodes;
                if (Settings.boExpanded)
                    ExpandAllMenuItem_Click(null, null);

                DateTime calenderDT = Plugin.GetApplication().Calendar.Selected;
                ShowBestEntryForDate(m_CVTreeListNodes, calenderDT);

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

        private void ExpandNode(IList<TreeList.TreeListNode> expandedNodes, TreeList.TreeListNode node)
        {
            expandedNodes.Add(node);
            foreach (TreeList.TreeListNode child in node.Children)
            {
                ExpandNode(expandedNodes, child);
            }
        }
        
        private void ExpandAllMenuItem_Click(object sender, EventArgs e)
        {
            //CompareTreeList.Expanded = m_CVTreeListNodes;
            List<TreeList.TreeListNode> expandedNodes = new List<TreeList.TreeListNode>();
            foreach (TreeList.TreeListNode node in m_CVTreeListNodes)
            {
                ExpandNode(expandedNodes, node);
            } 
            CompareTreeList.Expanded = expandedNodes;
            if (CompareTreeList.SelectedItems.Count > 0)
                CompareTreeList.EnsureVisible(CompareTreeList.SelectedItems[0]);
            Settings.boExpanded = true;
        }

        private void CollapseAllMenuItem_Click(object sender, EventArgs e)
        {
            CompareTreeList.Expanded = new List<object>();
            if (CompareTreeList.SelectedItems.Count > 0)
                CompareTreeList.EnsureVisible(CompareTreeList.SelectedItems[0]);
            Settings.boExpanded = false;
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

        private void WeeklyGroupMenuItem_Click(object sender, EventArgs e)
        {
            Settings.boGroupWeekly = !Settings.boGroupWeekly;
            UpdateData();
        }

        private void DailyGroupMenuItem_Click(object sender, EventArgs e)
        {
            Settings.boGroupDaily = !Settings.boGroupDaily;
            UpdateData();
        }

        private void GroupMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            WeeklyGroupMenuItem.Checked = Settings.boGroupWeekly;
            DailyGroupMenuItem.Checked = Settings.boGroupDaily;
        }
    }


    public class CVTreeListEntry
    {
        public DateTime Date;
        public IActivity Activity = null;
        public IActivity PlannedActivity = null;
    }
}
