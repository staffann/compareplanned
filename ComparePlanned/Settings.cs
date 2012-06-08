using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CompareView
{
    class Settings
    {
        private const int settingsVersionCurrent = 1;

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
        }


    }
}
