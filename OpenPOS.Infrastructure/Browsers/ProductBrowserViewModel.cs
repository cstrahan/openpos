using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenPOS.Data.Models;
using OpenPOS.Infrastructure.MVVM;

namespace OpenPOS.Infrastructure.Browsers
{
    public class ProductBrowserViewModel : ViewModelBase
    {
        #region Products

        private List<Category> _categories;
        public List<Category> Categories
        {
            get { return _categories; }
            set
            {
                if (_categories == value) return;
                _categories = value;
                RaisePropertyChanged("Categories");
            }
        }

        protected virtual void OnSelectedCategoryChanged(Category category)
        {
            
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                if (_selectedCategory == value) return;
                _selectedCategory = value;
                RaisePropertyChanged("SelectedCategory");

                OnSelectedCategoryChanged(_selectedCategory);
            }
        }


        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                if (_products == value) return;
                _products = value;
                RaisePropertyChanged("Products");
            }
        }

        protected virtual void OnSelectedProductChanged(Product product)
        {

        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (_selectedProduct == value) return;
                _selectedProduct = value;
                RaisePropertyChanged("SelectedProduct");

                OnSelectedProductChanged(_selectedProduct);
            }
        }

        #endregion
    }
}
