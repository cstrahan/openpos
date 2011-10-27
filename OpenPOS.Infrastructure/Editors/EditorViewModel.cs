using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using Microsoft.Practices.Composite.Presentation.Commands;
using OpenPOS.Data.Storage;
using OpenPOS.Infrastructure.MVVM;

namespace OpenPOS.Infrastructure.Editors
{
    public class GenericEventArgs<T> : EventArgs
    {
        public T Item { get; set; }
    }

    public class EditorViewModel<T> : ViewModelBase
        where T : class
    {
        readonly ISession _session;

        readonly ListCollectionView _view;

        public EditorViewModel(ISession session)
        {
            _session = session;

            Items = new ObservableCollection<T>(_session.All<T>().ToArray());

            _view = (ListCollectionView)CollectionViewSource.GetDefaultView(Items);
            _view.Filter += item =>
                {
                    string name = ((dynamic)item).Name;
                    if (name != string.Empty)
                        if (!string.IsNullOrEmpty(Filter))
                            return name.ToUpper().Contains(Filter.ToUpper());
                    return true;
                };

            SelectedItem = Items.FirstOrDefault();
        }

        public Type ItemType
        {
            get { return typeof(T); }
        }

        //private T internalSelectedItem;
        private T _selectedItem;

        private void SetSelectedItem(T item)
        {
            // Should place the original in the internalSelectedItem and copy it to the selectedItem
            
            _selectedItem = item;
            RaisePropertyChanged("SelectedItem");
        }

        public T SelectedItem
        {
            get { return _selectedItem; }

            set
            {
                SetSelectedItem(value);
                //selectedItem = value;
                //RaisePropertyChanged("SelectedItem");
            }
        }

        private ObservableCollection<T> _items;
        public ObservableCollection<T> Items
        {
            get { return _items; }

            set
            {
                if (_items == value) return;
                _items = value;
                RaisePropertyChanged("Items");
            }
        }

        private string _filter;
        public string Filter
        {
            get { return _filter; }

            set
            {
                if (_filter == value) return;
                _filter = value;
                RaisePropertyChanged("Filter");

                _view.Refresh();
            }
        }

        #region AddCommand

        private DelegateCommand<object> _addCommand;
        public DelegateCommand<object> AddCommand
        {
            get
            {
                if (_addCommand != null) return _addCommand;
                _addCommand = new DelegateCommand<object>(t =>
                                                             {
                                                                 SelectedItem = (T)Activator.CreateInstance(typeof(T));
                                                             });
                return _addCommand;
            }
        }

        #endregion

        #region SaveCommand

        public event EventHandler<GenericEventArgs<T>> NewItemSaved;

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                if (_saveCommand != null) return _saveCommand;
                return _saveCommand = new RelayCommand(t =>
                                                          {
                                                              if (((dynamic)_selectedItem).Id == Guid.Empty)
                                                              {
                                                                  ((dynamic)_selectedItem).Id = Guid.NewGuid();
                                                                  _session.Add<T>(_selectedItem);
                                                                  _session.CommitChanges();

                                                                  if (NewItemSaved != null)
                                                                      NewItemSaved(this, new GenericEventArgs<T>
                                                                                             {
                                                                                                 Item = SelectedItem
                                                                                             });
                                                              }
                                                              else
                                                              {
                                                                  _session.Update<T>(_selectedItem);
                                                                  _session.CommitChanges();
                                                              }

                                                              Items = new ObservableCollection<T>(_session.All<T>().ToArray());
                                                          },
                                                      p => true);
            }
        }

        #endregion

        #region RemoveCommand

        public event EventHandler<GenericEventArgs<T>> ItemRemoved;

        private RelayCommand _deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                if (_deleteCommand != null) return _deleteCommand;
                _deleteCommand = new RelayCommand(t =>
                                                     {
                                                         _session.Delete<T>(_selectedItem);
                                                         _session.CommitChanges();

                                                         if (ItemRemoved != null)
                                                             ItemRemoved(this, new GenericEventArgs<T>
                                                                                   {
                                                                                       Item = SelectedItem
                                                                                   });

                                                         Items = new ObservableCollection<T>(_session.All<T>().ToArray());
                                                     },
                                                 t => (SelectedItem != null));
                return _deleteCommand;
            }
        }

        #endregion
    }
}
