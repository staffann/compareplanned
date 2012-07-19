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
using System.ComponentModel;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;

namespace CompareView
{
    class ComparePlannedView:IView
    {
        private CompareViewControl control;
        
        public ComparePlannedView()
        {
//            control = new CompareViewControl();
        }
        
        #region IView Members

        public IList<IAction> Actions
        {
            get { return new List<IAction>(); }
        }

        public Guid Id
        {
            get { return new Guid("9A019A3E-112E-4EC2-8DF1-C5EF7A2F47AB"); }
        }

        public string SubTitle
        {
            get { return ""; }
        }

        public void SubTitleClicked(System.Drawing.Rectangle subTitleRect)
        {
            // Do nothing
        }

        public bool SubTitleHyperlink
        {
            get { return false; }
        }

        public string TasksHeading
        {
            get { return "Tasks"; }
        }

        #endregion

        #region IDialogPage Members

        public System.Windows.Forms.Control CreatePageControl()
        {
            if (control == null)
                control = new CompareViewControl();

            return control;
        }

        public bool HidePage()
        {
            control.FormHidden();
            Plugin.GetApplication().PropertyChanged -= new PropertyChangedEventHandler(UpdateControlEventHandler);
            return true;
        }

        public string PageName
        {
            get { return "ComparePlanned"; }
        }

        public void ShowPage(string bookmark)
        {
            control.UpdateData();
            Plugin.GetApplication().PropertyChanged += new PropertyChangedEventHandler(UpdateControlEventHandler);
        }

        public IPageStatus Status
        {
            get { return null; }
        }

        public void ThemeChanged(ITheme visualTheme)
        {
            control.ThemeChanged(visualTheme);
        }

        public string Title
        {
            get { return "ComparePlanned"; }
        }

        public void UICultureChanged(System.Globalization.CultureInfo culture)
        {
            // Culture settings not implemented
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void UpdateControlEventHandler(object sender, EventArgs e)
        {
            if (control != null)
                control.UpdateData();
        }

    }
}
