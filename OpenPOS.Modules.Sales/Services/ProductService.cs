using System;
using System.Linq;
using OpenPOS.Data.Models;
using OpenPOS.Data.Storage;
using OpenPOS.Infrastructure.Interfaces;

namespace OpenPOS.Modules.Sales.Services
{
    public class ProductService : IProductService
    {
        ISession _session;

        public ProductService(ISession session)
        {
            _session = session;
        }

        public Product[] GetProducts()
        {
            return _session.All<Product>().OrderBy(p => p.Code).ToArray();
        }

        public Product GetProduct(Guid productId)
        {
            return _session.Single<Product>(p => p.Id == productId);
        }

        public Category[] GetCategories()
        {
            return _session.All<Category>().OrderBy(p => p.Name).ToArray();
        }

        public Product[] GetProductsByCategory(Category category)
        {
            if (category == null)
                return null;

            return _session.All<Product>().Where(p => p.CategoryId == category.Id).OrderBy(p => p.Code).ToArray();
        }

        public Product GetProductByCode(string code)
        {
            return _session.Single<Product>(p => p.Code == code);
        }


        public Tax GetTaxByProduct(Product product)
        {
            // Hack for MongoDB
            Tax tax = new Tax()
            {
                Rate = 0.00
            };

            try
            {
                tax = _session.Single<Tax>(t => t.Id == product.TaxId);
            }
            catch 
            {

            }

            return tax;
        }
    }
}
