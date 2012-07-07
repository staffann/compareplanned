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
using System.Text;
using System.Xml;

namespace CompareView
{
    class Settings
    {
        private const int settingsVersionCurrent = 1;

        public static bool boGroupDaily = false;
        public static bool boGroupWeekly = true;
        public static bool boExpanded = false;
        
        private static IList<string> m_CompareTreeListColumns = new List<string>();
        public static IList<string> CompareTreeListColumns
        {
            get
            {
                return m_CompareTreeListColumns;
            }
            set
            {
                m_CompareTreeListColumns = value;
            }
        }

        static Settings()
        {
            m_CompareTreeListColumns.Add("StartTime");
            m_CompareTreeListColumns.Add("Time");
            m_CompareTreeListColumns.Add("DistanceMeters");
            m_CompareTreeListColumns.Add("PlannedTime");
            m_CompareTreeListColumns.Add("PlannedDistanceMeters");
        }

        public static void ReadOptions(XmlDocument xmlDoc, XmlNamespaceManager nsmgr, XmlElement pluginNode)
        {
            String attr;
            attr = pluginNode.GetAttribute("ListColumns");
            if (attr.Length > 0)
            {
                m_CompareTreeListColumns.Clear();
                String[] values = attr.Split(';');
                foreach (String column in values)
                {
                    m_CompareTreeListColumns.Add(column);
                }
            }
            
            bool.TryParse(pluginNode.GetAttribute("GroupWeekly"), out boGroupWeekly);
            bool.TryParse(pluginNode.GetAttribute("GroupDaily"), out boGroupDaily);
            bool.TryParse(pluginNode.GetAttribute("Expanded"), out boExpanded);

        }

        public static void WriteOptions(XmlDocument xmlDoc, XmlElement pluginNode)
        {
            String colText = null;
            foreach (String column in m_CompareTreeListColumns)
            {
                if (colText == null) 
                    colText = column; 
                else 
                    colText += ";" + column;
            }
            pluginNode.SetAttribute("ListColumns", colText);
            pluginNode.SetAttribute("GroupWeekly", boGroupWeekly.ToString());
            pluginNode.SetAttribute("GroupDaily", boGroupDaily.ToString());
            pluginNode.SetAttribute("Expanded", boExpanded.ToString());
        }


    }
}
