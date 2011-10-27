///////////////////////////////////////////////////////////////////////////////
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
///////////////////////////////////////////////////////////////////////////////
using System.Windows.Controls;

using OpenPOS.Modules.Users.ViewModels;

namespace OpenPOS.Modules.Users.Views
{
    public partial class UsersView : UserControl
    {
        public UsersView(UsersViewModel viewModel)
        {
            InitializeComponent();

            // Set the ViewModel as this View's data context.
            this.DataContext = viewModel;
        }
    }
}
