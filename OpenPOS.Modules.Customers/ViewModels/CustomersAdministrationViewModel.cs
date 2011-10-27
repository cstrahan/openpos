using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenPOS.Infrastructure.Administration;
using OpenPOS.Data.Storage;
using OpenPOS.Infrastructure.Editors;
using OpenPOS.Data.Models;
using System.Windows.Threading;

namespace OpenPOS.Modules.Customers.ViewModels
{
    public class CustomersAdministrationViewModel : AdministrationViewModel
    {
        private readonly ISession _session;

        public CustomersAdministrationViewModel(ISession session)
        {
            _session = session;

            #region Maintance - Customers

            Actions.Add(new AdministrationActionViewModel()
            {
                Action = (p) =>
                {
                                    IsBusy = true;

                    Dispatcher.CurrentDispatcher.BeginInvoke((Action) delegate
                                                                          {
                                                                              View =
                                                                                  new EditorView(_session);
                                                                              View.DataContext =
                                                                                  new EditorViewModel
                                                                                      <Customer>(_session);

                                                                              IsBusy = false;
                                                                          }, DispatcherPriority.Background);
                },
                Category = "Maintance",
                Title = "Customers"
            });

            #endregion
        }


    }
}
