using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ZoneFiveSoftware.Common.Visuals;
using CompareView.Properties;

namespace CompareView
{
    class CompareTreeListColumnDefinition : IListColumnDefinition
    {
        public CompareTreeListColumnDefinition(string id, string text, string groupName, int width, StringAlignment align)
        {
            this.align = align;
            this.groupName = groupName;
            this.id = id;
            this.width = width;
            this.text = text;
        }
        private StringAlignment align;
        public StringAlignment Align
        {
            get
            {
                return align;
            }
        }
        string groupName;
        public string GroupName
        {
            get
            {
                return groupName;
            }
        }
        string id;
        public string Id
        {
            get
            {
                return id;
            }
        }
        int width;
        public int Width
        {
            get
            {
                return width;
            }
        }
        string text;
        public string Text(string id)
        {
            return text;
        }
        public override string ToString()
        {
            return text;
        }
        public bool CanSelect
        {
            //Should some fields be unsortable?
            get { return true; }
        }
    }
    public static class CompareColumnIds
    {
        public const string StartTime = "StartTime";
        public const string Time = "Time";
        public const string DistanceMeters = "DistanceMeters";

        public const string Name = "Name";
        public const string Location = "Location";
        public const string Category = "Category";
        
        public const string AvgSpeed = "AverageSpeedMetersPerSecond";
        public const string AvgPace = "AvgPace";
        public const string AvgHR = "AverageHeartRate";
        public const string AvgCad = "AverageCadence";
        public const string AvgPower = "AveragePower";

        public const string PlannedTime = "PlannedTime";
        public const string PlannedDistanceMeters = "PlannedDistanceMeters";
        public const string PlannedName = "PlannedName";
        public const string PlannedCategory = "PlannedCategory";

        public const string PlannedAvgSpeed = "PlannedAverageSpeedMetersPerSecond";
        public const string PlannedAvgPace = "PlannedAvgPace";
        public const string PlannedAvgHR = "PlannedAverageHeartRate";
        public const string PlannedAvgCad = "PlannedAverageCadence";
        public const string PlannedAvgPower = "PlannedAveragePower";

        public const string TimeDiff = "TimeDiff";
        public const string DistanceDiff = "DistanceMetersDiff";

        //public const string MaxSpeed = "FastestSpeedMetersPerSecond";
        //public const string MaxPace = "MaxPace";
        //public const string MaxHR = "MaximumHeartRate";
        //public const string MaxCad = "MaximumCadence";
        //public const string MaxPower = "MaximumPower";
        //public const string TotAsc = "TotalAscendingMeters";
        //public const string TotDesc = "TotalDescendingMeters";

        //public const string TimeDiff = "TimeDiff";
        //public const string DistanceDiff = "DistanceMetersDiff";
        //public const string AvgSpeedDiff = "AverageSpeedMetersPerSecondDiff";
        //public const string AvgPaceDiff = "AvgPaceDiff";
        //public const string AvgHRDiff = "AverageHeartRateDiff";
        //public const string AvgPowerDiff = "AveragePowerDiff";
        //public const string AvgCadDiff = "AverageCadenceDiff";

        public static ICollection<IListColumnDefinition> ColumnDefs()
        {
            IList<IListColumnDefinition> columnDefs = new List<IListColumnDefinition>();
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.StartTime, CommonResources.Text.LabelDate, "", 100, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.Time, CommonResources.Text.LabelTime, "", 70, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.DistanceMeters, CommonResources.Text.LabelDistance, "", 70, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.Name, CommonResources.Text.LabelName, "", 150, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.Location, CommonResources.Text.LabelLocation, "", 100, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.Category, CommonResources.Text.LabelCategory, "", 100, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.AvgSpeed, CommonResources.Text.LabelAvgSpeed, "", 70, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.AvgPace, CommonResources.Text.LabelAvgPace, "", 70, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.AvgHR, CommonResources.Text.LabelAvgHR, "", 70, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.AvgCad, CommonResources.Text.LabelAvgCadence, "", 70, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.AvgPower, CommonResources.Text.LabelAvgPower, "", 70, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.PlannedTime, Resources.Planned + " " + CommonResources.Text.LabelTime, "", 70, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.PlannedDistanceMeters, Resources.Planned + " " + CommonResources.Text.LabelDistance, "", 70, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.PlannedName, Resources.Planned + " " + CommonResources.Text.LabelName, "", 150, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.PlannedCategory, Resources.Planned + " " + CommonResources.Text.LabelCategory, "", 100, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.PlannedAvgSpeed, Resources.Planned + " " + CommonResources.Text.LabelAvgSpeed, "", 70, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.PlannedAvgPace, Resources.Planned + " " + CommonResources.Text.LabelAvgPace, "", 70, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.PlannedAvgHR, Resources.Planned + " " + CommonResources.Text.LabelAvgHR, "", 70, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.PlannedAvgCad, Resources.Planned + " " + CommonResources.Text.LabelAvgCadence, "", 70, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.PlannedAvgPower, Resources.Planned + " " + CommonResources.Text.LabelAvgPower, "", 70, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.TimeDiff, Resources.Diff + " " + CommonResources.Text.LabelTime, "", 70, StringAlignment.Near));
            columnDefs.Add(new CompareTreeListColumnDefinition(CompareColumnIds.DistanceDiff, Resources.Diff + " " + CommonResources.Text.LabelDistance, "", 70, StringAlignment.Near));

            return columnDefs;
        }
    }
}
