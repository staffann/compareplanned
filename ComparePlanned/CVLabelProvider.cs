using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.Measurement;

namespace CompareView
{
    class CVLabelProvider : TreeList.DefaultLabelProvider
    {        
        #region ILabelProvider Members

        private TimeSpan? GetTime(TreeList.TreeListNode node)
        {
            CVTreeListEntry entry = (CVTreeListEntry)node.Element;
            TimeSpan? time = null;
            // If the node has children, sum the distance of all the children
            if (node.Children.Count > 0)
            {
                foreach (TreeList.TreeListNode child in node.Children)
                {
                    TimeSpan? childTime = GetTime(child);
                    if (childTime != null)
                    {
                        if (time != null)
                            time = time.Value.Add(childTime.Value);
                        else
                            time = childTime;
                    }
                }
            }
            else if (entry.Activity != null)
            {
                time = ActivityInfoCache.Instance.GetInfo(entry.Activity).Time;
            }
            else
            {
            }
            return time;
        }
        
        private double? GetDistanceMeters(TreeList.TreeListNode node)
        {
            CVTreeListEntry entry = (CVTreeListEntry)node.Element;
            double? distanceMeters = null;

            // If the node has children, sum the distance of all the children
            if (node.Children.Count > 0)
            {
                foreach (TreeList.TreeListNode child in node.Children)
                {
                    double? childDistance = GetDistanceMeters(child);
                    if (childDistance != null)
                    {
                        if (distanceMeters != null)
                            distanceMeters += childDistance;
                        else
                            distanceMeters = childDistance;
                    }
                }
            }
            else if (entry.Activity != null)
            {
                distanceMeters = ActivityInfoCache.Instance.GetInfo(entry.Activity).DistanceMeters;
            }
            else
            {
            }
            return distanceMeters;
        }
        
        private TreeList.TreeListNode TransformPlannedToActual(TreeList.TreeListNode tn, TreeList.TreeListNode parent)
        {
            if (tn == null)
            {
                return null;
            }
            else
            {
                CVTreeListEntry entry = (CVTreeListEntry)tn.Element;
                CVTreeListEntry transEntry = new CVTreeListEntry();
                transEntry.Date = entry.Date;
                transEntry.Activity = entry.PlannedActivity;
                transEntry.PlannedActivity = null;
                TreeList.TreeListNode transNode = new TreeList.TreeListNode(null, transEntry);
                transNode.Parent = parent;
                foreach (TreeList.TreeListNode childNode in tn.Children)
                {
                    transNode.Children.Add(TransformPlannedToActual(childNode, tn));
                }
                return transNode;
            }

        }
        public override System.Drawing.Image GetImage(object element, TreeList.Column column)
        {
            return base.GetImage(element, column);
        }

        public override string GetText(object element, TreeList.Column column)
        {
            TreeList.TreeListNode node = (TreeList.TreeListNode)element;
            CVTreeListEntry entry = (CVTreeListEntry)node.Element;

            if (column.Id.Contains("Planned"))
            {
                if (entry.PlannedActivity == null && node.Children.Count == 0)
                    return "";
                else
                {
                    // Don't duplicate how to show info - call this function back instead as if it was a performed activity
                    //CVTreeListEntry plannedEntry = new CVTreeListEntry();
                    //plannedEntry.Date = entry.Date;
                    //plannedEntry.Activity = entry.PlannedActivity;
                    //plannedEntry.PlannedActivity = null;
                    //TreeList.TreeListNode plannedNode = new TreeList.TreeListNode(null, plannedEntry);
                    TreeList.TreeListNode plannedNode = TransformPlannedToActual(node, (TreeList.TreeListNode)node.Parent);
                    return GetText(plannedNode, new TreeList.Column(column.Id.Replace("Planned", "")));
                }
            }
            else if (column.Id.Equals("StartTime"))
            {
                return entry.Date.ToShortDateString();
            }
            else if (column.Id.Equals("DistanceMeters"))
            {
                double? distanceMeters = GetDistanceMeters(node);
                if (distanceMeters == null)
                    return "";
                else
                    return Length.Convert(distanceMeters.Value, Length.Units.Meter, Length.Units.Kilometer).ToString("0.00");

                //// If the node has children, sum the distance of all the children
                //if (node.Children.Count > 0)
                //{
                //    distanceMeters = 0;
                //    foreach (TreeList.TreeListNode child in node.Children)
                //    {
                //        CVTreeListEntry childEntry = (CVTreeListEntry)child.Element;
                //        if (childEntry.Activity != null)
                //            distanceMeters += ActivityInfoCache.Instance.GetInfo(childEntry.Activity).DistanceMeters;
                //    }
                //}
                //else if (entry.Activity != null)
                //{
                //    distanceMeters = ActivityInfoCache.Instance.GetInfo(entry.Activity).DistanceMeters;
                //}
                //else
                //    return "";

                ////return Length.Convert(ActivityInfoCache.Instance.GetInfo(entry.Activity).DistanceMeters, 
                ////                        Length.Units.Meter, 
                ////                        Length.Units.Kilometer).ToString("0.00");
                //return Length.Convert(distanceMeters, Length.Units.Meter, Length.Units.Kilometer).ToString("0.00");
            }
            else if (column.Id.Equals("Time"))
            {
                TimeSpan? time = GetTime(node);
                if (time == null)
                    return "";
                else
                {
                    // Round time to full second, ToString in .Net 2.0 doesn not have use a format string.
                    // Rounding not always correct for exactly 0.5s but who cares...
                    if (time.Value.Milliseconds <= 500)
                        time = time.Value.Subtract(TimeSpan.FromMilliseconds(time.Value.Milliseconds));
                    else
                        time = time.Value.Add(TimeSpan.FromMilliseconds(1000 - time.Value.Milliseconds));
                    return time.ToString();
                }
                    
                //// If the node has children, sum the distance of all the children
                //if (node.Children.Count > 0)
                //{
                //    foreach (TreeList.TreeListNode child in node.Children)
                //    {
                //        CVTreeListEntry childEntry = (CVTreeListEntry)child.Element;
                //        if (childEntry.Activity != null)
                //            time = time.Add(ActivityInfoCache.Instance.GetInfo(childEntry.Activity).Time);
                //    }
                //}
                //else if (entry.Activity != null)
                //{
                //    time = ActivityInfoCache.Instance.GetInfo(entry.Activity).Time;
                //}
                //else
                //    return "";

                //// Round time to full second, ToString in .Net 2.0 doesn not have use a format string.
                //// Rounding not always correct for exactly 0.5s but who cares...
                //if (time.Milliseconds <= 500)
                //    time = time.Subtract(TimeSpan.FromMilliseconds(time.Milliseconds));
                //else
                //    time = time.Add(TimeSpan.FromMilliseconds(1000 - time.Milliseconds));
                //return time.ToString();

            }
            else if (column.Id.Equals(CompareColumnIds.TimeDiff))
            {
                TimeSpan? performedTime = GetTime(node);
                TreeList.TreeListNode transNode = TransformPlannedToActual(node, (TreeList.TreeListNode)node.Parent);
                TimeSpan? plannedTime = GetTime(transNode);

                if (performedTime == null)
                {
                    performedTime = new TimeSpan(0);
                }
                if (plannedTime == null)
                {
                    plannedTime = new TimeSpan(0);
                }
                TimeSpan time = performedTime.Value.Subtract(plannedTime.Value);
                // Round time to full second, ToString in .Net 2.0 doesn not have use a format string.
                // Rounding not always correct for exactly 0.5s but who cares...
                if (time.Milliseconds <= 500)
                    time = time.Subtract(TimeSpan.FromMilliseconds(time.Milliseconds));
                else
                    time = time.Add(TimeSpan.FromMilliseconds(1000 - time.Milliseconds));
                return time.ToString();
            }
            else if (column.Id.Equals(CompareColumnIds.DistanceDiff))
            {
                double? performedDistance = GetDistanceMeters(node);
                TreeList.TreeListNode transNode = TransformPlannedToActual(node, (TreeList.TreeListNode)node.Parent);
                double? plannedDistance = GetDistanceMeters(transNode);

                if (performedDistance == null)
                {
                    performedDistance = 0;
                }
                if (plannedDistance == null)
                {
                    plannedDistance = 0;
                }
                double distanceMeters = performedDistance.Value - plannedDistance.Value;
                return Length.Convert(distanceMeters, Length.Units.Meter, Length.Units.Kilometer).ToString("0.00");
            }
            else if (column.Id.Equals(CompareColumnIds.AvgPace))
                if (entry.Activity != null)
                {
                    return Speed.ToPaceString(ActivityInfoCache.Instance.GetInfo(entry.Activity).AverageSpeedMetersPerSecond,
                                              new TimeSpan(0, 0, 1),
                                              new Length(1, Length.Units.Kilometer),
                                              "");
                }
                else
                    return "";
            else if (column.Id.Equals(CompareColumnIds.AvgSpeed))
                if (entry.Activity != null)
                {
                    return Speed.ToSpeedString(ActivityInfoCache.Instance.GetInfo(entry.Activity).AverageSpeedMetersPerSecond,
                                              new TimeSpan(0, 0, 1),
                                              new Length(1, Length.Units.Kilometer),
                                              "###.00");
                }
                else
                    return "";
            else if (column.Id.Equals(CompareColumnIds.AvgHR))
                if (entry.Activity != null)
                {
                    return ActivityInfoCache.Instance.GetInfo(entry.Activity).AverageHeartRate.ToString("###");
                }
                else
                    return "";
            else if (column.Id.Equals(CompareColumnIds.AvgCad))
                if (entry.Activity != null)
                {
                    return ActivityInfoCache.Instance.GetInfo(entry.Activity).AverageCadence.ToString("###");
                }
                else
                    return "";
            else // Default - handle anything that isn't explicitly handled above
            {
                if (entry.Activity == null)
                    return "";
                else
                {
                    string retString;
                    retString = base.GetText(entry.Activity, column);
                    if (retString.Equals(""))
                    {
                        retString = base.GetText(ActivityInfoCache.Instance.GetInfo(entry.Activity), column);
                    }
                    return retString;
                }
            }
        }

        #endregion
    }
}
