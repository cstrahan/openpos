using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenPOS.Infrastructure.MVVM;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace OpenPOS.Infrastructure.Administration
{
    public class AdministrationActionViewModel : ViewModelBase
    {
        private string title;
        public string Title
        {
            get { return title; } 
            set
            {
                if (title != value)
                {
                    title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }
        private string category;
        public string Category
        {
            get { return category; }
            set
            {
                if (category != value)
                {
                    category = value;
                    RaisePropertyChanged("Category");
                }
            }
        }

        private Action<object> action;
        public Action<object> Action
        {
            get { return action; }
            set
            {
                if (action != value)
                {
                    action = value;
                    RaisePropertyChanged("Action");

                    Command = new RelayCommand(action);
                }
            }
        }

        private ICommand command;
        public ICommand Command
        {
            get { return command; }
            private set
            {
                if (command != value)
                {
                    command = value;
                    RaisePropertyChanged("Command");
                }
            }
        }
    }

    public class AdministrationViewModel : ViewModelBase
    {
        public AdministrationViewModel()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(Actions);
            if (view != null)
            {
                view.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            }
        }

        private ObservableCollection<AdministrationActionViewModel> actions = new ObservableCollection<AdministrationActionViewModel>();
        public ObservableCollection<AdministrationActionViewModel> Actions
        {
            get { return actions; }
            private set
            {
                if (actions != value)
                {
                    actions = value;
                    RaisePropertyChanged("Actions");
                }
            }
        }

        //private ViewModelBase viewModel;
        //public ViewModelBase ViewModel
        //{
        //    get { return viewModel;  }
        //    set
        //    {
        //        if (viewModel!=value)
        //        {
        //            viewModel = value;
        //            RaisePropertyChanged("ViewModel");
        //        }
        //    }
        //}

        private UserControl view;
        public UserControl View
        {
            get { return view; }
            set
            {
                if (view != value)
                {
                    view = value;
                    RaisePropertyChanged("View");
                }
            }
        }
    }
}
