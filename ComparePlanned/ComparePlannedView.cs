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
            control.UpdateData();
        }

    }
}
