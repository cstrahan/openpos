using System;
using OpenPOS.Data.Models;

namespace OpenPOS.Infrastructure.Interfaces
{
    public interface IProductService
    {
        Product[] GetProducts();
        Product GetProduct(Guid productId);
        Category[] GetCategories();
        Product[] GetProductsByCategory(Category category);
        Product GetProductByCode(string barcode);
        Tax GetTaxByProduct(Product product);
    }
}
