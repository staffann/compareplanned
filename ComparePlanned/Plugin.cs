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
using ZoneFiveSoftware.Common.Visuals.Fitness;

namespace CompareView
{
    class Plugin: IPlugin
    {
        private static IApplication application;

        #region IPlugin Members

        public IApplication Application
        {
            set { application = value; }
            get { return application; }
        }

        public static IApplication GetApplication()
        {
            return application;
        }

        public Guid Id
        {
            get { return new Guid("ba8477fd-baf2-4c56-99b1-fc8295a2b3ae"); }
        }

        public string Name
        {
            get { return "CompareView"; }
        }

        public void ReadOptions(XmlDocument xmlDoc, XmlNamespaceManager nsmgr, XmlElement pluginNode)
        {
            Settings.ReadOptions(xmlDoc, nsmgr, pluginNode);
        }

        public string Version
        {
            get { return GetType().Assembly.GetName().Version.ToString(3); }
        }

        public void WriteOptions(XmlDocument xmlDoc, XmlElement pluginNode)
        {
            Settings.WriteOptions(xmlDoc, pluginNode);
        }

        #endregion
    }
}
