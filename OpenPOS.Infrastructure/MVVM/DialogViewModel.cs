using System;

namespace OpenPOS.Infrastructure.MVVM
{
    public class DialogViewModel : ViewModelBase
    {


        #region RequestClose [event]

        public event EventHandler RequestClose;

        protected void OnRequestClose()
        {
            var handler = RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        #endregion 
    }
}
