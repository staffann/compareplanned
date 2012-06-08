using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Visuals.Fitness;

namespace CompareView
{
    class ExtendViews:IExtendViews 
    {
        #region IExtendViews Members

        public IList<ZoneFiveSoftware.Common.Visuals.IView> Views
        {            
            get 
            { 
                ComparePlannedView view = new ComparePlannedView();
                return new ComparePlannedView[] { view };
            }
        }

        #endregion
    }
}
