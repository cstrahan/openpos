using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenPOS.Infrastructure.Administration;
using OpenPOS.Data.Storage;
using OpenPOS.Infrastructure.Editors;
using OpenPOS.Data.Models;
using System.Windows.Threading;

namespace OpenPOS.Modules.Maintance.ViewModels
{
    public class MaintanceAdministrationViewModel : AdministrationViewModel
    {
        private readonly ISession _session;

        public MaintanceAdministrationViewModel(ISession session)
        {
            _session = session;

            #region Maintance - Users

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
                                                                                      <User>(_session);

                                                                              IsBusy = false;
                                                                          }, DispatcherPriority.Background);
                },
                Category = "Maintance",
                Title = "Users"
            });

            #endregion

            #region Maintance - Roles

            Actions.Add(new AdministrationActionViewModel()
            {
                Action = (p) =>
                {
                                    IsBusy = true;

                    Dispatcher.CurrentDispatcher.BeginInvoke((Action) delegate
                                                                          {
                                                                                View = new EditorView(_session);
                                                                                View.DataContext = new EditorViewModel<Role>(_session);
                                                                              IsBusy = false;
                                                                          }, DispatcherPriority.Background);
                },
                Category = "Maintance",
                Title = "Roles"
            });

            #endregion
        }


    }
}
