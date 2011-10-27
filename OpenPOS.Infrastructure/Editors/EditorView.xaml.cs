using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using OpenPOS.Data.Models;
using OpenPOS.Data.Storage;

namespace OpenPOS.Infrastructure.Editors
{
    public partial class EditorView
    {
        readonly ISession _session;
        readonly Dictionary<string, object> _collectors = new Dictionary<string, object>();

        public EditorView()
        {
            InitializeComponent();
        }

        public EditorView(ISession session)
        {
            InitializeComponent();

            _session = session;

            df.CommandButtonsVisibility = DataFormCommandButtonsVisibility.None;
            df.AutoCommit = true;

            _collectors.Add("RoleId", new Collector<Role>(_session));
            _collectors.Add("CategoryId", new Collector<Category>(_session));
            _collectors.Add("TaxId", new Collector<Tax>(_session));
        }

        #region DataForm

        private void DataFormAutoGeneratingField(object sender, DataFormAutoGeneratingFieldEventArgs e)
        {
            if (e.PropertyName != "Id")
            {
                if (e.PropertyName.EndsWith("Id"))
                {
                    dynamic collector = _collectors[e.PropertyName];

                    var comboBox = new ComboBox
                                       {
                                           Margin = new Thickness(0, 3, 0, 3),
                                           DisplayMemberPath = "Name",
                                           ItemsSource = collector.Collect(),
                                           SelectedValuePath = "Id"
                                       };

                    var binding = new Binding(e.PropertyName)
                                      {
                                          Source = df.CurrentItem,
                                          Mode = BindingMode.TwoWay,
                                          ValidatesOnExceptions = true,
                                          UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                      };

                    comboBox.SetBinding(Selector.SelectedValueProperty, binding);

                    e.Field.Label = e.PropertyName.Remove(e.PropertyName.Length - 2, 2);
                    e.Field.Content = comboBox;
                }
            }
            else
            {
                e.Cancel = true;
                return;
            }
        }

        private void DataFormEditEnded(object sender, DataFormEditEndedEventArgs e)
        {
            if (e.EditAction != DataFormEditAction.Commit) return;

            // Ugly hack
            dynamic vm = DataContext;
            vm.SaveCommand.Execute(null);
        }

        #endregion
    }

    public class Collector<T>
        where T : class
    {
        readonly ISession _session;

        public Collector(ISession session)
        {
            _session = session;
        }

        public List<T> Collect()
        {
            return _session.All<T>().ToList();
        }
    }

}
